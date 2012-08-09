using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Options pro vol�n� metody <see cref="BusinessCalendar.CountBusinessDays"/>.
	/// </summary>
	public enum CountBusinessDaysOptions
	{
		/// <summary>
		/// Zahrne do po�tu dn� i koncov� datum (od pond�l� do p�tku bude 5 pracovn�ch dn�).
		/// </summary>
		IncludeEndDate = 0,

		/// <summary>
		/// Vylou�� z po�tu pracovn�ch dn� koncov� datum (standardn� rozd�l dvou dat; pokud jsou shodn�, rozd�l je 0).
		/// </summary>
		ExcludeEndDate = 1
	}
}
