﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using Havit.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;
using Havit.Data.EntityFrameworkCore.Patterns.SoftDeletes;
using Havit.Data.Patterns.Attributes;

namespace Havit.EFCoreTests.DataLayer.DataSources.Fakes;

[Fake]
[System.CodeDom.Compiler.GeneratedCode("Havit.Data.EntityFrameworkCore.CodeGenerator", "1.0")]
public class FakeBusinessCaseDataSource : FakeDataSource<Havit.EFCoreTests.Model.BusinessCase>, Havit.EFCoreTests.DataLayer.DataSources.IBusinessCaseDataSource
{
	public FakeBusinessCaseDataSource(params Havit.EFCoreTests.Model.BusinessCase[] data)
		: this((IEnumerable<Havit.EFCoreTests.Model.BusinessCase>)data)
	{			
	}

	public FakeBusinessCaseDataSource(IEnumerable<Havit.EFCoreTests.Model.BusinessCase> data, ISoftDeleteManager softDeleteManager = null)
		: base(data, softDeleteManager)
	{
	}
}
