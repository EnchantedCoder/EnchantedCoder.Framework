using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataLoaders;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataLoaders.Internal;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Infrastructure;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.PropertyLambdaExpressions.Internal;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader.Model;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader
{
	public static class DataLoaderTestHelper
	{
		public static DbDataLoader CreateDataLoader(IDbContext dbContext = null, IEntityCacheManager entityCacheManager = null)
		{
			if (dbContext == null)
			{
				dbContext = new DataLoaderTestDbContext();
			}

			if (entityCacheManager == null)
			{
				entityCacheManager = CachingTestHelper.CreateEntityCacheManager(dbContext);
			}

			Mock<ILogger<DbDataLoader>> loggerMock = new Mock<ILogger<DbDataLoader>>(MockBehavior.Loose); // dovolíme použití loggeru bez setupu
			return new DbDataLoader(dbContext, new PropertyLoadSequenceResolverIncludingDeletedFilteringCollectionsSubstitution(), new PropertyLambdaExpressionManager(new PropertyLambdaExpressionStore(), new PropertyLambdaExpressionBuilder()), entityCacheManager, new DbEntityKeyAccessor(new DbEntityKeyAccessorStorage(), dbContext), loggerMock.Object);
		}
	}
}
