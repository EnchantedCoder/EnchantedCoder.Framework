using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Repositories;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.Model;
using EnchantedCoder.Data.Patterns.DataLoaders;
using EnchantedCoder.Data.Patterns.DataSources;
using EnchantedCoder.Data.Patterns.Infrastructure;

namespace EnchantedCoder.Data.EntityFrameworkCore.TestHelpers.DependencyInjection.Infrastructure.DataLayer
{
	public class LanguageRepository : DbRepository<Language>, ILanguageRepository
	{
		public LanguageRepository(IDbContext dbContext, ILanguageDataSource dataSource, IEntityKeyAccessor<Language, int> entityKeyAccessor, IDataLoader dataLoader, ISoftDeleteManager softDeleteManager, IEntityCacheManager entityCacheManager)
			: base(dbContext, dataSource, entityKeyAccessor, dataLoader, softDeleteManager, entityCacheManager)
		{

		}
	}
}
