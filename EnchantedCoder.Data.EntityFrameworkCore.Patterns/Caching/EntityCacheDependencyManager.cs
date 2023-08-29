﻿using EnchantedCoder.Data.EntityFrameworkCore.Patterns.UnitOfWorks;
using EnchantedCoder.Data.Patterns.Infrastructure;
using EnchantedCoder.Diagnostics.Contracts;
using EnchantedCoder.Services.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Třída zajišťující invalidaci závislostí v cache.
	/// </summary>
	public class EntityCacheDependencyManager : IEntityCacheDependencyManager
	{
		private readonly ICacheService cacheService;
		private readonly IEntityCacheDependencyKeyGenerator entityCacheDependencyKeyGenerator;
		private readonly IEntityKeyAccessor entityKeyAccessor;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public EntityCacheDependencyManager(ICacheService cacheService, IEntityCacheDependencyKeyGenerator entityCacheDependencyKeyGenerator, IEntityKeyAccessor entityKeyAccessor)
		{
			this.cacheService = cacheService;
			this.entityCacheDependencyKeyGenerator = entityCacheDependencyKeyGenerator;
			this.entityKeyAccessor = entityKeyAccessor;
		}

		/// <inheritdoc />
		public void InvalidateDependencies(Changes changes)
		{
			if (!cacheService.SupportsCacheDependencies)
			{
				// závislosti nemohou být použity
				return;
			}

			HashSet<Type> typesToInvalidateAnySaveCacheDependencyKey = new HashSet<Type>();

			if (changes.Inserts.Length > 0)
			{
				foreach (var insertedEntity in changes.Inserts)
				{
					InvalidateEntityDependencies(ChangeType.Insert, insertedEntity, typesToInvalidateAnySaveCacheDependencyKey);
				}
			}

			if (changes.Updates.Length > 0)
			{
				foreach (var updatedEntity in changes.Updates)
				{
					InvalidateEntityDependencies(ChangeType.Update, updatedEntity, typesToInvalidateAnySaveCacheDependencyKey);
				}
			}

			if (changes.Deletes.Length > 0)
			{
				foreach (var deletedEntity in changes.Deletes)
				{
					InvalidateEntityDependencies(ChangeType.Delete, deletedEntity, typesToInvalidateAnySaveCacheDependencyKey);
				}
			}

			foreach (Type typeToInvalidateAnySaveCacheDependencyKey in typesToInvalidateAnySaveCacheDependencyKey)
			{
				// Pro omezení zasílání informace o Remove při distribuované cache invalidujeme AnySave jen jednou pro každý typ.
				InvalidateAnySaveCacheDependencyKeyInternal(typeToInvalidateAnySaveCacheDependencyKey);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InvalidateEntityDependencies(ChangeType changeType, object entity, HashSet<Type> typesToInvalidateAnySaveCacheDependencyKey)
		{
			Contract.Requires(entity != null);

			// invalidate entity cache
			Type entityType = entity.GetType();

			object[] entityKeyValues = entityKeyAccessor.GetEntityKeyValues(entity);

			// entity se složeným klíčem (ManyToMany)
			// TODO: Ověřit si, že jde o ManyToMany, nejen o složený klíč
			if (entityKeyValues.Length == 1)
			{
				object entityKeyValue = entityKeyValues.Single();

				if (changeType != ChangeType.Insert)
				{
					// na nových záznamech nemohou být závislosti, neinvalidujeme
					InvalidateSaveCacheDependencyKeyInternal(entityType, entityKeyValue);
				}

				// invalidaci AnySave uděláme jen jednou pro každý typ (omezíme tak množství zpráv předávaných při případné distribuované invalidaci)
				typesToInvalidateAnySaveCacheDependencyKey.Add(entityType);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InvalidateSaveCacheDependencyKeyInternal(Type entityType, object entityKey)
		{
			cacheService.Remove(entityCacheDependencyKeyGenerator.GetSaveCacheDependencyKey(entityType, entityKey, ensureInCache: false));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InvalidateAnySaveCacheDependencyKeyInternal(Type entityType)
		{
			cacheService.Remove(entityCacheDependencyKeyGenerator.GetAnySaveCacheDependencyKey(entityType, ensureInCache: false));
		}

	}
}
