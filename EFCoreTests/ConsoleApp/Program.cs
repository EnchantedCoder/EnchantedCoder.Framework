using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using EnchantedCoder.Data.EntityFrameworkCore;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DependencyInjection;
using EnchantedCoder.Data.Patterns.DataLoaders;
using EnchantedCoder.Data.Patterns.DataSeeds;
using EnchantedCoder.Data.Patterns.DataSeeds.Profiles;
using EnchantedCoder.Data.Patterns.Exceptions;
using EnchantedCoder.Data.Patterns.UnitOfWorks;
using EnchantedCoder.EFCoreTests.DataLayer.DataSources;
using EnchantedCoder.EFCoreTests.DataLayer.Lookups;
using EnchantedCoder.EFCoreTests.DataLayer.Repositories;
using EnchantedCoder.EFCoreTests.DataLayer.Seeds.Core;
using EnchantedCoder.EFCoreTests.DataLayer.Seeds.Persons;
using EnchantedCoder.EFCoreTests.DataLayer.Seeds.ProtectedProperties;
using EnchantedCoder.EFCoreTests.Entity;
using EnchantedCoder.EFCoreTests.Model;
using EnchantedCoder.Services;
using EnchantedCoder.Services.Caching;
using EnchantedCoder.Services.TimeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleApp1
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var host = Host.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration(configurationBuilder =>
					configurationBuilder.AddJsonFile("appsettings.ConsoleApp.json", optional: false)
				)
				.ConfigureLogging((hostingContext, logging) => logging
				.AddSimpleConsole(config => config.TimestampFormat = "[hh:MM:ss.ffff] "))
				.ConfigureServices((hostingContext, services) => ConfigureServices(hostingContext, services))
				.Build();

			UpdateDatabase(host.Services);
			Debug(host.Services);
		}

		private static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
		{
			services.WithEntityPatternsInstaller()
				.AddDataLayer(typeof(IPersonRepository).Assembly)
				.AddDbContext<EnchantedCoder.EFCoreTests.Entity.ApplicationDbContext>(optionsBuilder =>
					optionsBuilder
						.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=EFCoreTests;Application Name=EFCoreTests-Entity;ConnectRetryCount=0")
						.EnableSensitiveDataLogging(true))
				//.UseInMemoryDatabase("ConsoleApp")
				.AddEntityPatterns()
				.AddLookupService<IUserLookupService, UserLookupService>();

			services.AddSingleton<ITimeService, ServerTimeService>();
			services.AddSingleton<ICacheService, MemoryCacheService>();
			services.AddSingleton<IOptions<MemoryCacheOptions>, OptionsManager<MemoryCacheOptions>>();
			services.AddSingleton(typeof(IOptionsFactory<MemoryCacheOptions>), new OptionsFactory<MemoryCacheOptions>(Enumerable.Empty<IConfigureOptions<MemoryCacheOptions>>(), Enumerable.Empty<IPostConfigureOptions<MemoryCacheOptions>>()));
			services.AddSingleton<IMemoryCache, MemoryCache>();
		}

		private static void UpdateDatabase(IServiceProvider serviceProvider)
		{
			using (var scope = serviceProvider.CreateScope())
			{
				//scope.ServiceProvider.GetRequiredService<IDbContext>().Database.EnsureDeleted();
				scope.ServiceProvider.GetRequiredService<IDbContext>().Database.Migrate();
				scope.ServiceProvider.GetRequiredService<IDataSeedRunner>().SeedData<PersonsProfile>();
			}
		}

		private static void Debug(IServiceProvider serviceProvider)
		{
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < 10_000; i++)
			{
				using (var scope = serviceProvider.CreateScope())
				{
					var repository = scope.ServiceProvider.GetRequiredService<IPersonRepository>();

					repository.GetObject(1);
				}
			}
			Console.WriteLine(sw.ElapsedMilliseconds);
		}

	}
}
