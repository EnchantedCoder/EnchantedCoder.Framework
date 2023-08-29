using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataLoaders;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataLoaders.Internal;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Infrastructure;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.PropertyLambdaExpressions.Internal;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader.Model;
using EnchantedCoder.Data.Patterns.DataLoaders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader
{
	[TestClass]
	public class DbDataLoader_General_Tests : DbDataLoaderTestsBase
	{
		[TestMethod]
		public void DbDataLoader_Load_SkipsNullEntities()
		{
			// Arrange
			SeedOneToManyTestData();

			DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();

			IDataLoader dataLoader = new DbDataLoader(dbContext, new PropertyLoadSequenceResolverIncludingDeletedFilteringCollectionsSubstitution(), new PropertyLambdaExpressionManager(new PropertyLambdaExpressionStore(), new PropertyLambdaExpressionBuilder()), new NoCachingEntityCacheManager(), new DbEntityKeyAccessor(new DbEntityKeyAccessorStorage(), dbContext), Mock.Of<ILogger<DbDataLoader>>(MockBehavior.Loose /* umožníme použití bez setupu */));

			// Act
			dataLoader.Load((Child)null, c => c.Parent);
			dataLoader.LoadAll(new Child[] { null }, c => c.Parent);

			// Assert: No exception was thrown
		}
	}
}
