﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Entity.Glimpse.DbCommandInterception
{
	/// <summary>
	/// Třída nesoucí informaci o počtu záznamů v DbDataReadu pro zobrazení v Glimpse (záložka EF).
	/// </summary>
	internal class DbDataReaderResult
	{
		#region RecordsCount 
		/// <summary>
		/// Počet záznamů v DbDataReaderu.
		/// Pokud je hodnota null, není hodnota dosud známá (DbDataReader nebyl dosud uzavřen).
		/// </summary>
		public int? RecordsCount { get; internal set; }
		#endregion
	}
}
