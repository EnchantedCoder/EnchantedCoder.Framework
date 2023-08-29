﻿using EnchantedCoder.Data.EntityFrameworkCore.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnchantedCoder.EFCoreTests.Model
{
	[Cache]
	public class BusinessCase
	{
		public int Id { get; set; }

		public List<Modelation> Modelations { get; } = new List<Modelation>();
	}
}
