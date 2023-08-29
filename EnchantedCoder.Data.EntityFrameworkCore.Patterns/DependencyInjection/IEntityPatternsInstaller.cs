using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Lookups;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Reflection;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection
{
	/// <summary>
	/// Installer EnchantedCoder.Data.Entity.Patterns a souvisejících služeb.
	/// </summary>
	public interface IEntityPatternsInstaller
	{
		/// <summary>
		/// Registruje do DI containeru DbContext vč. IDbContextFactory.
		/// </summary>
		public IEntityPatternsInstaller AddDbContext<TDbContext>()
			where TDbContext : EnchantedCoder.Data.EntityFrameworkCore.DbContext, IDbContext;

		/// <summary>
		/// Registruje do DI containeru DbContext vč. IDbContextFactory.
		/// </summary>
		public IEntityPatternsInstaller AddDbContext<TDbContext>(Action<DbContextOptionsBuilder> optionsAction = null)
			where TDbContext : EnchantedCoder.Data.EntityFrameworkCore.DbContext, IDbContext;

		/// <summary>
		/// Registruje do DI containeru DbContext vč. IDbContextFactory.
		/// </summary>
		IEntityPatternsInstaller AddDbContext<TDbContext>(Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null)
			where TDbContext : DbContext, IDbContext;

		/// <summary>
		/// Registruje do DI containeru DbContext s DbContext poolingem vč. IDbContextFactory.
		/// </summary>
		public IEntityPatternsInstaller AddDbContextPool<TDbContext>(Action<DbContextOptionsBuilder> optionsAction, int poolSize = DbContextPool<DbContext>.DefaultPoolSize)
			where TDbContext : EnchantedCoder.Data.EntityFrameworkCore.DbContext, IDbContext;

		/// <summary>
		/// Registruje do DI containeru služby pro lokalizaci.
		/// </summary>
		IEntityPatternsInstaller AddLocalizationServices<TLanguage>()
			where TLanguage : class, EnchantedCoder.Model.Localizations.ILanguage;

		/// <summary>
		/// Zaregistruje do DI containeru lookup službu.
		/// </summary>
		IEntityPatternsInstaller AddLookupService<TService, TImplementation>()
			where TService : class
			where TImplementation : class, TService, ILookupDataInvalidationService;

		/// <summary>
		/// Viz <see cref="IEntityPatternsInstaller"/>
		/// </summary>
		IEntityPatternsInstaller AddLookupService<TService, TImplementation, TLookupDataInvalidationService>()
			where TService : class
			where TImplementation : class, TService
			where TLookupDataInvalidationService : ILookupDataInvalidationService;

		/// <summary>
		/// Registruje do DI containeru služby HFW pro Entity Framework Core.
		/// </summary>
		IEntityPatternsInstaller AddEntityPatterns();

		/// <summary>
		/// Registruje do DI containeru třídy z assembly předané v parametru dataLayerAssembly.
		/// Registrují se data seedy, data sources, repositories a data entries.
		/// </summary>
		IEntityPatternsInstaller AddDataLayer(Assembly assembly);

		/// <summary>
		/// Registruje do DI containeru dataseeds z dané assembly.
		/// </summary>
		IEntityPatternsInstaller AddDataSeeds(Assembly dataSeedsAssembly);
	}
}