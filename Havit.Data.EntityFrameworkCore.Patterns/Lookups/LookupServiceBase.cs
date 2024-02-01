﻿using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.EntityFrameworkCore.Patterns.UnitOfWorks;
using Havit.Data.Patterns.Exceptions;
using Havit.Data.Patterns.Infrastructure;
using Havit.Data.Patterns.Repositories;
using Havit.Linq;
using Havit.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.Linq.Expressions;

namespace Havit.Data.EntityFrameworkCore.Patterns.Lookups;

/// <summary>
/// Bázová třída pro vyhledávání entit dle klíče (jednoduchého i složeného).
/// Použití:
/// 1) Podědit od této třídy a implementovat abstrakční vlastností
/// 2) Eventuelně nakonfigurovat chování overridováním virtuálních vlastností.
/// 3) Implementovat nějaký vlastní interface, imlementace bude volat GetEntityByLookupKey (ev. GetEntityKeyByLookupKey).
/// </summary>
/// <typeparam name="TLookupKey">Typ klíče.</typeparam>
/// <typeparam name="TEntity">Entita, kterou hledáme.</typeparam>
/// <remarks>
/// Určeno k implementaci občasně měněných entit, ev. entit které se mění hromadně (naráz).
/// Není garantována stoprocentní spolehlivost u entit, které se mění často (myšleno zejména paralelně) v různých transakcích - invalidace a aktualizace může proběhnout v jiném pořadí, než v jakém doběhly commity.
/// </remarks>
public abstract class LookupServiceBase<TLookupKey, TEntity> : ILookupDataInvalidationService
	where TEntity : class
{
	private readonly IEntityLookupDataStorage lookupStorage;
	private readonly IRepository<TEntity> repository; // TODO: QueryTags nedokonalé, bude se hlásit query tag dle DbRepository.
	private readonly IDbContext dbContext;
	private readonly IEntityKeyAccessor entityKeyAccessor;
	private readonly ISoftDeleteManager softDeleteManager;
	private readonly IDistributedLookupDataInvalidationService distributedLookupDataInvalidationService;

	/// <summary>
	/// Konstruktor.
	/// </summary>
	protected LookupServiceBase(IEntityLookupDataStorage lookupStorage, IRepository<TEntity> repository, IDbContext dbContext, IEntityKeyAccessor entityKeyAccessor, ISoftDeleteManager softDeleteManager)
		: this(lookupStorage, repository, dbContext, entityKeyAccessor, softDeleteManager, null)
	{
	}

	/// <summary>
	/// Konstruktor.
	/// </summary>
	protected LookupServiceBase(IEntityLookupDataStorage lookupStorage, IRepository<TEntity> repository, IDbContext dbContext, IEntityKeyAccessor entityKeyAccessor, ISoftDeleteManager softDeleteManager, IDistributedLookupDataInvalidationService distributedLookupDataInvalidationService)
	{
		this.lookupStorage = lookupStorage;
		this.repository = repository;
		this.dbContext = dbContext;
		this.entityKeyAccessor = entityKeyAccessor;
		this.softDeleteManager = softDeleteManager;
		this.distributedLookupDataInvalidationService = distributedLookupDataInvalidationService ?? new NullDistributedLookupDataInvalidationService();
	}

	/// <summary>
	/// Indikuje, zda jsou použity i smazané záznamy (Výchozí hodnota: false).
	/// </summary>
	protected virtual bool IncludeDeleted => false;

	/// <summary>
	/// Indikuje, zda je v případě nenalezení vyhozena výjimka ObjectNotFound (pokud je true), či zda je vrácena null hodnota (pokud je false). (Vychozí hodnota: true).
	/// </summary>
	protected virtual bool ThrowExceptionWhenNotFound => true;

	/// <summary>
	/// Poskytuje klíč, podle kterého jsou záznamy dohledávány.
	/// </summary>
	protected abstract Expression<Func<TEntity, TLookupKey>> LookupKeyExpression { get; }

	/// <summary>
	/// Poskytuje volitelný filtr, který se má na záznamy uplatnit (např. jen neprázdný párovací klíč, atp.).
	/// </summary>
	protected virtual Expression<Func<TEntity, bool>> Filter => null;

	/// <summary>
	/// Nápověda pro efektivnější fungování služby.
	/// </summary>
	protected abstract LookupServiceOptimizationHints OptimizationHints { get; }

	/// <summary>
	/// Vrátí entitu na základě lookup klíče.
	/// Není-li nalezena, řídí se chování dle ThrowExceptionWhenNotFound.
	/// Entita je na základě klíče vrácena z repository, což umožní použít cache.
	/// </summary>
	protected TEntity GetEntityByLookupKey(TLookupKey lookupKey)
	{
		return TryGetEntityKeyByLookupKey(lookupKey, out int entityKey)
			? repository.GetObject(entityKey)
			: null;
	}

	/// <summary>
	/// Vrátí entitu na základě lookup klíče.
	/// Není-li nalezena, řídí se chování dle ThrowExceptionWhenNotFound.
	/// Entita je na základě klíče vrácena z repository, což umožní použít cache.
	/// </summary>
	/// <remarks>
	/// Případné sestavení "lookup-data" neprobíhá asynchronně, považujeme to však za dostatečné řešení.
	/// </remarks>
	protected async Task<TEntity> GetEntityByLookupKeyAsync(TLookupKey lookupKey, CancellationToken cancellationToken = default)
	{
		return TryGetEntityKeyByLookupKey(lookupKey, out int entityKey)
			? await repository.GetObjectAsync(entityKey, cancellationToken).ConfigureAwait(false)
			: null;
	}

	/// <summary>
	/// Vrátí entity na základě lookup klíčů.
	/// Není-li nalezena, řídí se chování dle ThrowExceptionWhenNotFound, pokud nevyhazuje výjimku a entita není nalezena, prostě není ve výsledku metody.
	/// Entity jsou na základě klíče vráceny z repository, což umožní použít cache.
	/// </summary>
	protected List<TEntity> GetEntitiesByLookupKeys(TLookupKey[] lookupKeys)
	{
		var entityKeys = lookupKeys.Select(lookupKey =>
			{
				bool success = TryGetEntityKeyByLookupKey(lookupKey, out int entityKey);
				return new { Success = success, EntityKey = entityKey };
			})
			.Where(result => result.Success)
			.Select(result => result.EntityKey)
			.ToArray();

		return repository.GetObjects(entityKeys);
	}

	/// <summary>
	/// Vrátí entity na základě lookup klíčů.
	/// Není-li nalezena, řídí se chování dle ThrowExceptionWhenNotFound, pokud nevyhazuje výjimku a entita není nalezena, prostě není ve výsledku metody.
	/// Entity jsou na základě klíče vráceny z repository, což umožní použít cache.
	/// </summary>
	/// <remarks>
	/// Případné sestavení "lookup-data" neprobíhá asynchronně, považujeme to však za dostatečné řešení.
	/// </remarks>
	protected async Task<List<TEntity>> GetEntitiesByLookupKeysAsync(TLookupKey[] lookupKeys, CancellationToken cancellationToken = default)
	{
		var entityKeys = lookupKeys.Select(lookupKey =>
		{
			bool success = TryGetEntityKeyByLookupKey(lookupKey, out int entityKey);
			return new { Success = success, EntityKey = entityKey };
		})
			.Where(result => result.Success)
			.Select(result => result.EntityKey)
			.ToArray();

		return await repository.GetObjectsAsync(entityKeys, cancellationToken).ConfigureAwait(false);
	}

	/// <summary>
	/// Vyhledá klíč entity entitu na základě lookup klíče.
	/// Není-li nalezena, řídí se chování dle ThrowExceptionWhenNotFound.
	/// Určeno pro možnost získat si více klíčů entit a následné hromadné načtení.
	/// </summary>
	protected bool TryGetEntityKeyByLookupKey(TLookupKey lookupKey, out int entityKey)
	{
		EntityLookupData<TEntity, int, TLookupKey> entityLookupData = lookupStorage.GetEntityLookupData(GetStorageKey(), CreateEntityLookupData);

		lock (entityLookupData)
		{
			if (entityLookupData.EntityKeyByLookupKeyDictionary.TryGetValue(lookupKey, out entityKey))
			{
				return true;
			}
			else
			{
				if (ThrowExceptionWhenNotFound)
				{
					throw new ObjectNotFoundException($"Object with key '{lookupKey}' not found. To return null instead of throwing exception, please override property {nameof(ThrowExceptionWhenNotFound)} and return false.");
				}
				return false;
			}
		}
	}

	/// <summary>
	/// Vyčistá data používaná pro vyhledávání.
	/// Použití pro
	/// a) možnost invalidovat data v případě změny mimo UnitOfWork
	/// b) možnost invalidovat data pro uvolnění paměti, pokud již párování nemá smysl udržovat v paměti.
	/// </summary>
	protected void ClearLookupData()
	{
		// Pozor, jako side-effekt dojde k nové kompilaci LookupKeyExpression a Filteru.
		distributedLookupDataInvalidationService.Invalidate(GetStorageKey());
		lookupStorage.RemoveEntityLookupData(GetStorageKey());
	}

	/// <summary>
	/// Klíč, pod jakým jsou lookup data uložena ve storage.
	/// </summary>
	protected virtual string GetStorageKey()
	{
		return this.GetType().FullName;
	}

	/// <summary>
	/// Factory pro EntityLookupData.
	/// </summary>
	private EntityLookupData<TEntity, int, TLookupKey> CreateEntityLookupData()
	{
		string entityKeyPropertyName = entityKeyAccessor.GetEntityKeyPropertyNames(typeof(TEntity)).Single();
		Expression<Func<TEntity, TLookupKey>> lookupKeyExpression = LookupKeyExpression;

		ParameterExpression expressionParameter = Expression.Parameter(typeof(TEntity), "item");
		Expression<Func<TEntity, EntityLookupPair<int, TLookupKey>>> lambdaExpression = (Expression<Func<TEntity, EntityLookupPair<int, TLookupKey>>>)Expression.Lambda(
			Expression.MemberInit(Expression.New(typeof(EntityLookupPair<int, TLookupKey>)),
			Expression.Bind(typeof(EntityLookupPair<int, TLookupKey>).GetProperty(nameof(EntityLookupPair<int, TLookupKey>.EntityKey)), Expression.MakeMemberAccess(expressionParameter, typeof(TEntity).GetProperty(entityKeyPropertyName))),
			Expression.Bind(typeof(EntityLookupPair<int, TLookupKey>).GetProperty(nameof(EntityLookupPair<int, TLookupKey>.LookupKey)), ExpressionExt.ReplaceParameter(lookupKeyExpression.Body, lookupKeyExpression.Parameters[0], expressionParameter))),
			expressionParameter);

		string tag = QueryTagBuilder.CreateTag(this.GetType(), nameof(CreateEntityLookupData));
		List<EntityLookupPair<int, TLookupKey>> pairs = (IncludeDeleted ? dbContext.Set<TEntity>().AsQueryable(tag) : dbContext.Set<TEntity>().AsQueryable(tag).WhereNotDeleted(softDeleteManager))
			.WhereIf(Filter != null, Filter)
			.Select(lambdaExpression)
			.ToList();

		Dictionary<TLookupKey, int> entityKeyByLookupKeyDictionary;
		try
		{
			entityKeyByLookupKeyDictionary = pairs.ToDictionary(pair => pair.LookupKey, pair => pair.EntityKey);
		}
		catch (ArgumentException argumentException)
		{
			throw new InvalidOperationException("Source data contains duplicity.", argumentException);
		}

		Dictionary<int, TLookupKey> lookupKeyByEntityKeyDictionary = !OptimizationHints.HasFlag(LookupServiceOptimizationHints.EntityIsReadOnly)
			? pairs.ToDictionary(pair => pair.EntityKey, pair => pair.LookupKey)
			: null;

		return new EntityLookupData<TEntity, int, TLookupKey>(
			entityKeyByLookupKeyDictionary,
			lookupKeyByEntityKeyDictionary,
			LookupKeyExpression.Compile(),
			Filter?.Compile());
	}

	/// <summary>
	/// Provede invalidaci lookup dat bez ohledu na provedené změny.
	/// </summary>
	void ILookupDataInvalidationService.Invalidate()
	{
		ClearLookupData();
	}

	/// <summary>
	/// Aktualizuje data po uložení v UnitOfWork.
	/// </summary>
	/// <remarks>
	/// Metodu nechceme by default veřejnou.
	/// </remarks>
	void ILookupDataInvalidationService.InvalidateAfterCommit(Changes changes)
	{
		Invalidate(changes);
	}

	/// <summary>
	/// Aktualizuje data po uložení v UnitOfWork.
	/// </summary>
	protected virtual void Invalidate(Changes changes)
	{
		// entita je read-only, neprovádíme žádnou invalidaci
		if (OptimizationHints.HasFlag(LookupServiceOptimizationHints.EntityIsReadOnly))
		{
			return;
		}

		// nedošlo k žádné změně sledované entity, neprovádíme žádnou invalidaci
		if (!changes.Any(change => (change.IsOfType<TEntity>())))
		{
			return;
		}

		// musí předcházet opuštění v případě neexistence lookup dat
		distributedLookupDataInvalidationService.Invalidate(GetStorageKey());

		EntityLookupData<TEntity, int, TLookupKey> entityLookupData = lookupStorage.GetEntityLookupData<TEntity, int, TLookupKey>(GetStorageKey(), null);
		if (entityLookupData == null)
		{
			// nemáme sestaven lookupTable, není co invalidovat (avšak distrubuovanou invalidaci nutno řešit, proto předchází tomuto bloku kódu).
			return;
		}

		var updatedAndDeletedEntities = changes
			.Where(change => change.IsOfType<TEntity>() && ((change.ChangeType == ChangeType.Update) || change.ChangeType == ChangeType.Delete))
			.Select(change => (TEntity)change.Entity)
			.ToList();

		foreach (var entity in updatedAndDeletedEntities)
		{
			// mohlo dojít ke změně na entitě (klíče, filtru)
			// podíváme se, zda máme entitu v evidenci a pokud ano, invalidujeme ji
			int entityKey = (int)entityKeyAccessor.GetEntityKeyValues(entity).Single();
			lock (entityLookupData)
			{
				if (entityLookupData.LookupKeyByEntityKeyDictionary.TryGetValue(entityKey, out TLookupKey lookupKey))
				{
					entityLookupData.EntityKeyByLookupKeyDictionary.Remove(lookupKey);
					entityLookupData.LookupKeyByEntityKeyDictionary.Remove(entityKey);
				}
			}
		}

		bool softDeleteSupported = softDeleteManager.IsSoftDeleteSupported<TEntity>();
		Func<TEntity, bool> softDeleteCompiledLambda = softDeleteSupported ? softDeleteManager.GetNotDeletedCompiledLambda<TEntity>() : null;
		Func<TEntity, bool> filterCompiledLambda = entityLookupData.FilterCompiledLambda;

		var insertedAndUpdatedEntities = changes
		.Where(change => change.IsOfType<TEntity>() && ((change.ChangeType == ChangeType.Update) || change.ChangeType == ChangeType.Insert))
		.Select(change => (TEntity)change.Entity)
		.ToList();

		foreach (var entity in insertedAndUpdatedEntities)
		{
			// Když někdo založí příznakem smazanou entitu, pak ji použijeme tehdy, pokud používáme i deleted záznamy
			if (!IncludeDeleted && softDeleteSupported && !softDeleteCompiledLambda(entity))
			{
				// smazanou entitu nechceme evidovat
				continue;
			}

			if (filterCompiledLambda != null && !filterCompiledLambda(entity))
			{
				// entita neodpovídá filtru
				continue;
			}

			int entityKey = (int)entityKeyAccessor.GetEntityKeyValues(entity).Single();
			TLookupKey lookupKey = entityLookupData.LookupKeyCompiledLambda(entity);
			lock (entityLookupData)
			{
				entityLookupData.EntityKeyByLookupKeyDictionary.Add(lookupKey, entityKey);
				entityLookupData.LookupKeyByEntityKeyDictionary.Add(entityKey, lookupKey);
			}
		}
	}

	/// <summary>
	/// Vrátí název vlastnosti, která je reprezentována daným výrazem.
	/// </summary>
	/// <remarks>
	/// Duplicitní kód, avšak nechci jej veřejně sdílet jako extension metodu nebo tak).
	/// </remarks>
	internal string GetPropertyName(Expression item)
	{
		if (item is MemberExpression)
		{
			MemberExpression memberExpression = (MemberExpression)item;
			if (memberExpression.Expression is ParameterExpression)
			{
				return memberExpression.Member.Name;
			}
		}
		throw new NotSupportedException(item.ToString());
	}
}
