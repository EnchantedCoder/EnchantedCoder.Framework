using System.Collections.Generic;
using System.Reflection;
using EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Attributes.ExtendedProperties;

namespace EnchantedCoder.Data.EntityFrameworkCore.BusinessLayer.Tests.ExtendedProperties
{
	internal class CollectionTestExtendedPropertiesAttribute : ExtendedPropertiesAttribute
	{
		public string FooBar { get; set; }

		public override IDictionary<string, string> GetExtendedProperties(MemberInfo memberInfo)
		{
			return new Dictionary<string, string>()
			{
				{ $"Test_{memberInfo.Name}_{nameof(FooBar)}", FooBar },
			};
		}
	}
}
