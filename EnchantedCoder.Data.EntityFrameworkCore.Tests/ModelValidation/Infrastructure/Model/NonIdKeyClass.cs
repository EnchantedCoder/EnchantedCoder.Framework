using System.ComponentModel.DataAnnotations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.ModelValidation.Infrastructure.Model
{
	public class NonIdKeyClass
	{
		[Key]
		public int Key { get; set; }
	}
}
