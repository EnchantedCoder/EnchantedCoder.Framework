using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.Model;

namespace EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.DataLayer
{
	public class LanguageDataSource : DbDataSource<Language>, ILanguageDataSource
	{
		public LanguageDataSource(IDbContext dbContext, ISoftDeleteManager softDeleteManager) : base(dbContext, softDeleteManager)
		{
		}
	}
}
