﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using EnchantedCoder.Data.EntityFrameworkCore;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources;
using EnchantedCoder.Data.EntityFrameworkCore.Patterns.SoftDeletes;

namespace EnchantedCoder.EFCoreTests.DataLayer.DataSources;

[System.CodeDom.Compiler.GeneratedCode("EnchantedCoder.Data.EntityFrameworkCore.CodeGenerator", "1.0")]
public partial class BusinessCaseDbDataSource : DbDataSource<EnchantedCoder.EFCoreTests.Model.BusinessCase>, IBusinessCaseDataSource
{
	public BusinessCaseDbDataSource(IDbContext dbContext, ISoftDeleteManager softDeleteManager)
		: base(dbContext, softDeleteManager)
	{
	}
}
