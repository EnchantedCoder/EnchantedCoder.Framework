using System;
using System.Collections.Generic;
using EnchantedCoder.Services.Caching;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Úložiště CacheOptions k jednotlivým entitám
	/// </summary>
	public interface IAnnotationsEntityCacheOptionsGeneratorStorage
	{
		/// <summary>
		/// Úložiště CacheOptions k jednotlivým entitám
		/// </summary>
		Dictionary<Type, CacheOptions> Value { get; set; }
	}
}
