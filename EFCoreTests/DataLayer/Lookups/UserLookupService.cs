using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Lookups;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.Patterns.DataSources;
using EnchantedCoder.Data.Patterns.Infrastructure;
using EnchantedCoder.Data.Patterns.Repositories;
using EnchantedCoder.EFCoreTests.Model;

namespace EnchantedCoder.EFCoreTests.DataLayer.Lookups
{
	public class UserLookupService : LookupServiceBase<object, User>, IUserLookupService
	{
		public UserLookupService(IEntityLookupDataStorage lookupStorage, IRepository<User> repository, IDataSource<User> dataSource, IEntityKeyAccessor entityKeyAccessor, ISoftDeleteManager softDeleteManager) : base(lookupStorage, repository, dataSource, entityKeyAccessor, softDeleteManager)
		{
		}

		protected override Expression<Func<User, object>> LookupKeyExpression => user => new { A = user.Username, B = user.Username };

		protected override LookupServiceOptimizationHints OptimizationHints => LookupServiceOptimizationHints.None;

		public User GetUserByUsername(string username) => GetEntityByLookupKey(new { A = username, B = username });
	}
}
