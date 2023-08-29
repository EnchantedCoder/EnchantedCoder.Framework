﻿using System;

namespace EnchantedCoder.Services.BusinessCalendars
{
	public class BusinessCalendarFridaySaturdayWeekendStrategy : IIsWeekendStrategy
	{
		public bool IsWeekend(DateTime date)
		{
			return (date.DayOfWeek == DayOfWeek.Friday) || (date.DayOfWeek == DayOfWeek.Saturday);
		}
	}
}