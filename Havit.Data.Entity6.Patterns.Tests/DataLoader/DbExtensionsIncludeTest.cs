﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Data.Entity.Patterns.Tests.DataLoader.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Havit.Data.Entity.Patterns.DataLoaders
{
	[TestClass]
	public class DbExtensionsIncludeTest
	{
		[TestMethod]
		public void DbExtensionsInclude_CheckSupportechPatterns()
		{
			// Arrange
			DataLoaderTestDbContext dbContext = new DataLoaderTestDbContext();

			// Act

			// classic select
			dbContext.Set<Child>().Include(c => c.Parent.Children.Select(item => item.Parent)).FirstOrDefault();
			dbContext.Set<HiearchyItem>().Include(c1 => c1.Children.Select(c2 => c2.Children.Select(c3 => c3.Children))).FirstOrDefault();

			// another select
			dbContext.Set<Child>().Include(c => c.Parent.Children.Select((item, index) => item.Parent)).FirstOrDefault();
			dbContext.Set<HiearchyItem>().Include(c1 => c1.Children.Select((c2, index2) => c2.Children.Select((c3, index3) => c3.Children))).FirstOrDefault();

			// Assert
			// no exception was thrown
		}
	}
}
