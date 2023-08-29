﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EnchantedCoder.Data.EntityFrameworkCore;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Caching;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.Repositories;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using EnchantedCoder.Data.Patterns.DataEntries;
using EnchantedCoder.Data.Patterns.DataLoaders;
using EnchantedCoder.Data.Patterns.Infrastructure;

namespace EnchantedCoder.EFCoreTests.DataLayer.Repositories;

[System.CodeDom.Compiler.GeneratedCode("EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator", "1.0")]
public abstract class PropertyWithProtectedMembersDbRepositoryBase : DbRepository<EnchantedCoder.EFCoreTests.Model.PropertyWithProtectedMembers>
{
	protected PropertyWithProtectedMembersDbRepositoryBase(IDbContext dbContext, EnchantedCoder.EFCoreTests.DataLayer.DataSources.IPropertyWithProtectedMembersDataSource dataSource, IEntityKeyAccessor<EnchantedCoder.EFCoreTests.Model.PropertyWithProtectedMembers, int> entityKeyAccessor, IDataLoader dataLoader, ISoftDeleteManager softDeleteManager, IEntityCacheManager entityCacheManager)
		: base(dbContext, dataSource, entityKeyAccessor, dataLoader, softDeleteManager, entityCacheManager)
	{
	}

}