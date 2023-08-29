using System;
using System.Collections.Generic;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure;
using EnchantedCoder.Services.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection.Caching
{
	/// <summary>
	/// Installer, která zaregistruje službu, která nic necachuje (NoCachingEntityCacheManager).
	/// </summary>
	public sealed class NoCachingInstaller : ICachingInstaller
	{
		/// <inheritdoc />
		public void Install(IServiceCollection services)
		{
			services.AddSingleton<IEntityCacheManager, NoCachingEntityCacheManager>();
		}
	}
}
