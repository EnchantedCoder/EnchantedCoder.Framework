﻿using System.ComponentModel.DataAnnotations;

namespace EnchantedCoder.Data.EntityFrameworkCore.Tests.Infrastructure.Model
{
	public class ModelClass
	{
		public int Id { get; set; }

		[MaxLength(100)]
		public string Name { get; set; }

		public OwnedClass OwnedClass { get; set; }
	}
}
