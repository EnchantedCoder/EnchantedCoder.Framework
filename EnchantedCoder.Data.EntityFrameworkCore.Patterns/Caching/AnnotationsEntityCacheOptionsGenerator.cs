using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata.Conventions;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching.Internal;
using EnchantedCoder.Services;
using EnchantedCoder.Services.Caching;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Výchozí strategie definující, zda může být entita cachována. Řídí se anotacemi.
	/// </summary>
	public class AnnotationsEntityCacheOptionsGenerator : IEntityCacheOptionsGenerator
	{
		private readonly IAnnotationsEntityCacheOptionsGeneratorStorage annotationsEntityCacheOptionsGeneratorStorage;
		private readonly IDbContext dbContext;
		private readonly ICollectionTargetTypeService collectionTargetTypeService;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public AnnotationsEntityCacheOptionsGenerator(IAnnotationsEntityCacheOptionsGeneratorStorage annotationsEntityCacheOptionsGeneratorStorage, IDbContext dbContext, ICollectionTargetTypeService collectionTargetTypeService)
		{
			this.annotationsEntityCacheOptionsGeneratorStorage = annotationsEntityCacheOptionsGeneratorStorage;
			this.dbContext = dbContext;
			this.collectionTargetTypeService = collectionTargetTypeService;
		}

		/// <inheritdoc />
		public CacheOptions GetEntityCacheOptions<TEntity>(TEntity entity)
			where TEntity : class
		{
			return GetValueForEntity(typeof(TEntity));
		}

		/// <inheritdoc />
		public CacheOptions GetCollectionCacheOptions<TEntity>(TEntity entity, string propertyName)
			where TEntity : class
		{
			return GetValueForEntity(collectionTargetTypeService.GetCollectionTargetType(typeof(TEntity), propertyName));
		}

		/// <inheritdoc />
		public CacheOptions GetAllKeysCacheOptions<TEntity>()
			where TEntity : class
		{
			return GetValueForEntity(typeof(TEntity));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private CacheOptions GetValueForEntity(Type type)
		{
			if (annotationsEntityCacheOptionsGeneratorStorage.Value == null)
			{
				lock (annotationsEntityCacheOptionsGeneratorStorage)
				{
					if (annotationsEntityCacheOptionsGeneratorStorage.Value == null)
					{
						annotationsEntityCacheOptionsGeneratorStorage.Value = dbContext.Model.GetApplicationEntityTypes().ToDictionary(
												entityType => entityType.ClrType,
												entityType =>
												{
													var options = GetCacheOptions(entityType);
													options?.Freeze();
													return options;
												});
					}
				}
			}

			if (annotationsEntityCacheOptionsGeneratorStorage.Value.TryGetValue(type, out CacheOptions result))
			{
				return result;
			}

			return null;
		}

		/// <summary>
		/// Vrací cache options pro danou entitu.
		/// Neočekává se sdílená instance přes různé typy. CacheOptions jsou následně uzamčeny pro změnu.
		/// </summary>
		protected virtual CacheOptions GetCacheOptions(IReadOnlyEntityType entityType)
		{
			int? absoluteExpiration = (int?)entityType.FindAnnotation(CacheAttributeToAnnotationConvention.AbsoluteExpirationAnnotationName)?.Value;
			int? slidingExpiration = (int?)entityType.FindAnnotation(CacheAttributeToAnnotationConvention.SlidingExpirationAnnotationName)?.Value;
			EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority? priority = (EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority?)entityType.FindAnnotation(CacheAttributeToAnnotationConvention.PriorityAnnotationName)?.Value;

			if ((absoluteExpiration != null) || (slidingExpiration != null) || (priority != null))
			{
				return new CacheOptions
				{
					AbsoluteExpiration = (absoluteExpiration == null) ? (TimeSpan?)null : TimeSpan.FromSeconds(absoluteExpiration.Value),
					SlidingExpiration = (slidingExpiration == null) ? (TimeSpan?)null : TimeSpan.FromSeconds(slidingExpiration.Value),
					Priority = GetPriority(priority)
				};
			}

			return null;
		}

		private EnchantedCoder.Services.Caching.CacheItemPriority GetPriority(EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority? priority)
		{
			switch (priority)
			{
				case null:
				case EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority.Normal:
					return EnchantedCoder.Services.Caching.CacheItemPriority.Normal;

				case EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority.Low:
					return EnchantedCoder.Services.Caching.CacheItemPriority.Low;

				case EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority.High:
					return EnchantedCoder.Services.Caching.CacheItemPriority.High;

				case EnchantedCoder.Data.EntityFrameworkCore.Attributes.CacheItemPriority.NotRemovable:
					return EnchantedCoder.Services.Caching.CacheItemPriority.NotRemovable;

				default:
					throw new InvalidOperationException(priority.ToString());
			}
		}

	}
}
