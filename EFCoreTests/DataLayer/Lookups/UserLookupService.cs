﻿using Havit.Data.EntityFrameworkCore.Patterns.Lookups;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.DataSources;
using Havit.Data.Patterns.Infrastructure;
using Havit.Data.Patterns.Repositories;
using Havit.EFCoreTests.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Havit.EFCoreTests.DataLayer.Lookups
{
	public class UserLookupService : LookupServiceBase<User, object>, IUserLookupService
	{
		public UserLookupService(IEntityLookupDataStorage lookupStorage, IRepository<User> repository, IDataSource<User> dataSource, IEntityKeyAccessor entityKeyAccessor, ISoftDeleteManager softDeleteManager) : base(lookupStorage, repository, dataSource, entityKeyAccessor, softDeleteManager)
		{
		}

		protected override Expression<Func<User, object>> LookupKeyExpression => user => new { A = user.Username, B = user.Username };

		protected override LookupServiceOptimizationHints OptimizationHints => LookupServiceOptimizationHints.None;

		public User GetUserByUsername(string username) => GetEntityByLookupKey(new { A = username, B = username });
	}
}
