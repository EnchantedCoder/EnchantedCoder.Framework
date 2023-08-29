using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching.Internal;
using EnchantedCoder.Services;
using EnchantedCoder.Services.Caching;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Strategie definující, zda může být entita cachována. Řídí se anotacemi, ke kterým doplňuje výchozí hodnoty.
	/// </summary>
	public class AnnotationsWithDefaultsEntityCacheOptionsGenerator : AnnotationsEntityCacheOptionsGenerator
	{
		private readonly TimeSpan? absoluteExpiration;
		private readonly TimeSpan? slidingExpiration;

		/// <summary>
		/// Konstruktor.
		/// </summary>
		public AnnotationsWithDefaultsEntityCacheOptionsGenerator(IAnnotationsEntityCacheOptionsGeneratorStorage annotationsEntityCacheOptionsGeneratorStorage, IDbContext dbContext, ICollectionTargetTypeService collectionTargetTypeService, AnnotationsWithDefaultsEntityCacheOptionsGeneratorOptions options) : base(annotationsEntityCacheOptionsGeneratorStorage, dbContext, collectionTargetTypeService)
		{
			this.absoluteExpiration = options.AbsoluteExpiration;
			this.slidingExpiration = options.SlidingExpiration;
		}

		/// <inheritdoc />
		protected override CacheOptions GetCacheOptions(IReadOnlyEntityType entityType)
		{
			CacheOptions result = base.GetCacheOptions(entityType);

			if (result == null)
			{
				result = new CacheOptions();
			}

			if (result.AbsoluteExpiration == null)
			{
				result.SlidingExpiration = absoluteExpiration;
			}

			if (result.SlidingExpiration == null)
			{
				result.SlidingExpiration = slidingExpiration;
			}

			return result;
		}
	}
}
