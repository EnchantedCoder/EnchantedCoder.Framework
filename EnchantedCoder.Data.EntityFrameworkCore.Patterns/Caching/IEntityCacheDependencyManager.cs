﻿using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.UnitOfWorks;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching
{
	/// <summary>
	/// Třída zajišťující invalidaci závislostí v cache.
	/// </summary>
	public interface IEntityCacheDependencyManager
	{
		/// <summary>
		/// Invaliduje závislosti změněných entit.
		/// </summary>
		void InvalidateDependencies(Changes changes);
	}
}
