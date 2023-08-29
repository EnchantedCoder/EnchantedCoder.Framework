﻿using EnchantedCoder.Data.EntityFrameworkCore.Patterns.DataSources.Fakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Data.EntityFrameworkCore.Patterns.Tests.Lookups.Infrastructure
{
	public class UzivatelDataSource : FakeDataSource<Uzivatel>
	{
		public UzivatelDataSource(Uzivatel[] uzivatele) : base(uzivatele)
		{

		}
	}
}
