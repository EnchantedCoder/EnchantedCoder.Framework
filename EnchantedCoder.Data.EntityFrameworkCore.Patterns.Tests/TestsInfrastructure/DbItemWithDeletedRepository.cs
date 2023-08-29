using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Repositories;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.Patterns.DataLoaders;
using EnchantedCoder.Data.Patterns.DataSources;
using EnchantedCoder.Data.Patterns.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure
{
	public class DbItemWithDeletedRepository : DbRepository<ItemWithDeleted>
	{
		public DbItemWithDeletedRepository(IDbContext dbContext, IDataSource<ItemWithDeleted> dataSource, IEntityKeyAccessor<ItemWithDeleted, int> entityKeyAccessor, IDataLoader dataLoader, ISoftDeleteManager softDeleteManager, IEntityCacheManager entityCacheManager)
			: base(dbContext, dataSource, entityKeyAccessor, dataLoader, softDeleteManager, entityCacheManager)
		{
		}
	}
}