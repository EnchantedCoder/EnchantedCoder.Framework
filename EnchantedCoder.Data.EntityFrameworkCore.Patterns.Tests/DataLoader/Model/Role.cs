﻿using EnchantedCoder.Data.EntityFrameworkCore.Attributes;
using System;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.DataLoader.Model
{
	public class Role
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public DateTime? Deleted { get; set; }
	}
}
