﻿using Havit.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Havit.Services.Tests.Business
{
    [TestClass]
	public class BusinessCalendarWeekendStrategiesTests
	{
		[TestMethod]
		public void BusinessCalendarFridaySaturdayWeekendStrategy_IsWeekend_ReturnsTrueOnlyForFridaysAndSaturdays()
		{
			IIsWeekendStrategy weekendStrategy = BusinessCalendarWeekendStrategy.GetFridaySaturdayStrategy();
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 2)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 3)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 4)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 5)));
			Assert.IsTrue(weekendStrategy.IsWeekend(new DateTime(2015, 3, 6)));
			Assert.IsTrue(weekendStrategy.IsWeekend(new DateTime(2015, 3, 7)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 8)));
		}
		
		[TestMethod]
		public void BusinessCalendarSaturdaySundayWeekendStrategy_IsWeekend_ReturnsTrueOnlyForSaturdaysAndSundays()
		{
			IIsWeekendStrategy weekendStrategy = BusinessCalendarWeekendStrategy.GetSaturdaySundayStrategy();
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 2)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 3)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 4)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 5)));
			Assert.IsFalse(weekendStrategy.IsWeekend(new DateTime(2015, 3, 6)));
			Assert.IsTrue(weekendStrategy.IsWeekend(new DateTime(2015, 3, 7)));
			Assert.IsTrue(weekendStrategy.IsWeekend(new DateTime(2015, 3, 8)));
		}

	}
}
