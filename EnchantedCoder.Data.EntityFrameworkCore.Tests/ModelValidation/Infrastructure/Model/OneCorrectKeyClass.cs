using System.ComponentModel.DataAnnotations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.ModelValidation.Infrastructure.Model
{
	public class OneCorrectKeyClass
	{
		[Key]
		public int Id { get; set; }

		public int ExternalId { get; set; }
	}
}
