using EnchantedCoder.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnchantedCoder.Tests.Collections
{
	[TestClass]
	public class SortDirectionExtensionsTests
	{
		[TestMethod]
		public void SortDirectionExtensions_Reverse()
		{
			// Arrange + Assert
			Assert.AreEqual(SortDirection.Descending, SortDirection.Ascending.Reverse());
			Assert.AreEqual(SortDirection.Ascending, SortDirection.Descending.Reverse());
		}
	}
}
