﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Data.Patterns.DataSeeds
{
	/// <summary>
	/// Rozhoduje, zda má dojít k spuštění seedování dat.
	/// Implementací lze například potlačit spuštění seedování dat, pokud již (třeba v dané verzi aplikace) bylo seedování spuštěno.
	/// </summary>
	public interface IDataSeedRunDecision
	{
		/// <summary>
		/// Indikuje, zda má dojík se spuštění seedování dat.
		/// </summary>
		bool ShouldSeedData(IEnumerable<Type> dataSeedTypes);

		/// <summary>
		/// Metoda je zavolána po dokončení seedování dat.
		/// </summary>
		void SeedDataCompleted(IEnumerable<Type> dataSeedTypes);
	}
}
