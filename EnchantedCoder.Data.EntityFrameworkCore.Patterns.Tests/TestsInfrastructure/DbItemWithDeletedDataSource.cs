using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.TestsInfrastructure
{
	public class DbItemWithDeletedDataSource : DbDataSource<ItemWithDeleted>
	{
		public DbItemWithDeletedDataSource(IDbContext dbContext, SoftDeleteManager softDeleteManager) : base(dbContext, softDeleteManager)
		{
		}
	}
}