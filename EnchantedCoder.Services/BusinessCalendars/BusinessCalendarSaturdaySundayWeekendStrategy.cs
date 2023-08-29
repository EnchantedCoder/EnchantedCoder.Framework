﻿using System;

namespace EnchantedCoder.Services.BusinessCalendars
{
	public class BusinessCalendarSaturdaySundayWeekendStrategy : IIsWeekendStrategy
	{
		public bool IsWeekend(DateTime date)
		{
			return (date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday);
		}
	}
}