using System.Collections.Generic;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.ModelValidation.Infrastructure.Model
{
	public class ForeignKeyWithNoNavigationPropertyMasterClass
	{
		public int Id { get; set; }

		public List<ForeignKeyWithNoNavigationPropertyChildClass> Children { get; set; }
	}
}
