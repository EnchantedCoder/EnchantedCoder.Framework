using System.Reflection;

namespace Havit.Data.Entity.Patterns.Windsor.Installers
{
	/// <summary>
	/// Installer Havit.Data.Entity.Patterns a souvisej�c�ch slu�eb.
	/// </summary>
	public interface IEntityPatternsInstaller
	{
		/// <summary>
		/// Registruje do windsor containeru:
		/// <list type="bullet">
		///		<item>
		///			<term>IDbContext</term>
		///			<description>Jako p�edan� TDbContext. Lifestyle je ur�en dle options installeru (DbContextLifestyle).</description>
		///		</item>
		/// </list>
		/// </summary>
		IEntityPatternsInstaller RegisterDbContext<TDbContext>()
			where TDbContext : class, IDbContext;

		/// <summary>
		/// Registruje do windsor containeru:
		/// <list type="bullet">
		///		<item>
		///			<term>ILanguageService</term>
		///			<description>
		///				Jako p�edan� ILanguageService pro p�edan� TLanguage (DbLanguageService&lt;TLanguage&gt;).
		///				Lifestyle je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>ILocalizationService</term>
		///			<description>
		///				LocalizationService. 
		///				Lifestyle je singleton.
		///			</description>
		///		</item>
		/// </list>
		/// </summary>
		IEntityPatternsInstaller RegisterLocalizationServices<TLanguage>();

		/// <summary>
		/// Registruje do windsor containeru:
		/// <list type="bullet">
		///		<item>
		///			<term>ISoftDeleteManager</term>
		///			<description>
		///				SoftDeleteManager.
		///				Lifestyle je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IDataEntrySymbolStorage&lt;&gt;</term>
		///			<description>
		///				DbDataEntrySymbolStorage&lt;&gt;
		///				Lifestyle je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>ICurrentCultureService</term>
		///			<description>
		///				CurrentCultureService.
		///				Lifestyle je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IDataSeedRunner</term>
		///			<description>
		///				DataSeedRunner.
		///				Lifestyle je transient.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IDataSeedPersister</term>
		///			<description>
		///				DbDataSeedPersister.
		///				Lifestyle je transient.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IDataSourceFactory&lt;&gt;</term>
		///			<description>
		///				Factory pro IDataSource&lt;&gt;.
		///				Lifestyle factory je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IRepositoryFactory&lt;&gt;</term>
		///			<description>
		///				Factory pro IRepository&lt;&gt;.
		///				Lifestyle factory je singleton.
		///			</description>
		///		</item>
		///		<item>
		///			<term>IUnitOfWork, IUnitOfWorkAsync</term>
		///			<description>
		///				Registruje typ dle options installery (UnitOfWorkType).
		///				Lifestyle je ur�en dle options installeru (UnitOfWorkLifestyle).
		///			</description>
		///		</item>
		///		<item>
		///			<term>IDataLoader, IDataLoaderAsync</term>
		///			<description>
		///				DbDataLoader.
		///				Lifestyle je ur�en dle options installeru (DataLoaderLifestyle).
		///			</description>
		///		</item>
		/// </list>
		/// </summary>
		IEntityPatternsInstaller RegisterEntityPatterns();

		/// <summary>
		/// Registruje do windsor containeru t��dy z assembly p�edan� v parametru dataLayerAssembly:
		/// <list type="bullet">
		///		<item>
		///			<term>t��dy implementuj�c� IDataSeed</term>
		///			<description>
		///				Registruje je pod IDataSeed.
		///				Lifestyle transient.
		///			</description>
		///		</item>
		///		<item>
		///			<term>potomky (implementace) DbDataSource&lt;&gt;</term>
		///			<description>
		///				Registruje je pod IDataSource&lt;TEntity&gt; a IEntityDataSource. TEntity je skute�n� typ entity.
		///				Lifestyle je transient.
		///			</description>
		///		</item>
		///		<item>
		///			<term>potomky (implementace) DbRepository&lt;&gt;</term>
		///			<description>
		///				Registruje je pod IRepository&lt;TEntity&gt; a IEntityRepository. TEntity je skute�n� typ entity.
		///				Lifestyle je ur�en dle options installeru (RepositoriesLifestyle).
		///			</description>
		///		</item>
		///		<item>
		///			<term>implementuj�c� IDataEntries&lt;&gt;</term>
		///			<description>
		///				Registruje je pod IDataSeed.
		///				Lifestyle je ur�en dle options installeru (DataEntriesLifestyle).
		///			</description>
		///		</item>
		/// </list>
		/// <remarks>
		///		Nikdy neregistruje slu�by (t��dy), kter� jsou dekorov�ny atributem FakeAttribute �i kter� jsou abstraktn�.
		/// </remarks>
		/// </summary>
		IEntityPatternsInstaller RegisterDataLayer(Assembly assembly);
	}
}