using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Model
{
	[Keyless]
	public class KeylessClass
	{
		public string Value { get; set; }
	}
}
