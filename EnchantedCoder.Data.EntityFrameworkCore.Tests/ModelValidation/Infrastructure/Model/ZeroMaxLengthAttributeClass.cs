﻿using System.ComponentModel.DataAnnotations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.ModelValidation.Infrastructure.Model
{
	public class ZeroMaxLengthAttributeClass
	{
		public int Id { get; set; }

		[MaxLength(0)]
		public string Value { get; set; }
	}
}
