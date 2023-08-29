using EnchantedCoder.Services.Caching;
using System;
using System.Collections.Generic;

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
