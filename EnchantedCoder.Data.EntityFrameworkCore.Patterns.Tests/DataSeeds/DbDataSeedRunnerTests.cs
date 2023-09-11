using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSeeds;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSeeds.Internal;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection.Infrastructure.Factories;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure;
using EnchantedCoder.Data.Patterns.DataSeeds;
using EnchantedCoder.Services.Caching;
using EnchantedCoder.Services.TimeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataSeeds
{
	/// <summary>
	/// Jde o jediný test používající fyzickou databázi v části EF Core (jinde se in-memory provider).
	/// Avšak zde nejsou připraveny migrace, atp. a test parazituje na jiné části testů (ConsoleApp), jehož databázi dokáže využít.
	/// Proto test nezařazuji mezi automaticky spouštěné unit testy. Pro spuštění je třeba odkomentovat atribut níže.
	/// </summary>
	//[TestClass]
	public class DbDataSeedRunnerTests
	{
		[TestMethod]
		public void DbDataSeedRunner_SeedData_SupportsSqlServerConnectionResiliency()
		{
			// see: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency

			// Arrange
			ServiceCollection services = new ServiceCollection();
			services.AddMemoryCache();
			services.AddSingleton<ITimeService, ServerTimeService>();
			services.AddSingleton<ICacheService, MemoryCacheService>();

			services.WithEntityPatternsInstaller()
				.AddDbContext<TestDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=EFCoreTests;Application Name=EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests",
					o => o.EnableRetryOnFailure())) // this is the most important part of the test setup!
				.AddEntityPatterns();


			using var serviceProvider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
			using var scope = serviceProvider.CreateScope();
			IDataSeedRunner dbDataSeedRunner = scope.ServiceProvider.GetRequiredService<IDataSeedRunner>();

			scope.ServiceProvider.GetRequiredService<IDbContext>().Database.Migrate();

			// Act
			dbDataSeedRunner.SeedData<TestHelpers.DependencyInjection.Infrastructure.DataLayer.Seeds.TestProfile>();

			// Assert - no exception was thrown
		}

	}
}
