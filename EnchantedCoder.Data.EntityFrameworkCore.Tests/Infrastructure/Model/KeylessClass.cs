using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Model
{
	[Keyless]
	public class KeylessClass
	{
		public string Value { get; set; }
	}
}
