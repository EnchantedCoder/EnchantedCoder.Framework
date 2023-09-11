using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using EnchantedCoder.Data.EntityFrameworkCore.Metadata;
using EnchantedCoder.Services;
using EnchantedCoder.Services.Caching;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Vrací vždy null. Pro použití tam, kde nepotřebujeme řešit CacheOptions, např. v unit testech.
	/// </summary>
	public class NullEntityCacheOptionsGenerator : IEntityCacheOptionsGenerator
	{
		/// <inheritdoc />
		public CacheOptions GetEntityCacheOptions<TEntity>(TEntity entity)
			where TEntity : class
		{
			return null;
		}

		/// <inheritdoc />
		public CacheOptions GetCollectionCacheOptions<TEntity>(TEntity entity, string propertyName)
			where TEntity : class
		{
			return null;
		}

		/// <inheritdoc />
		public CacheOptions GetAllKeysCacheOptions<TEntity>()
			where TEntity : class
		{
			return null;
		}
	}
}
