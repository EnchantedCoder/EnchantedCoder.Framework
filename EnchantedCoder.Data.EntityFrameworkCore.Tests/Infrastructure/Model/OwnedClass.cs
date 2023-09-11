using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Model
{
	[Owned]
	public class OwnedClass
	{
		public string Value { get; set; }
	}
}
