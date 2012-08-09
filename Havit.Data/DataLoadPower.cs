using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Data
{
	/// <summary>
	/// Rozsah dat v datov�m zdroji.
	/// </summary>
	public enum DataLoadPower
	{

		/// <summary>
		/// Datov� zdroj obsahuje jen informace pro zalo�en� ghosta (prim�rn� kl��).
		/// </summary>
		Ghost = 0,

		/// <summary>
		/// Datov� zdroj obsahuje kompletn� ��dek dat (v�echny mo�n� sloupce).
		/// </summary>
		FullLoad = 1,

		/// <summary>
		/// Datov� zdroj obsahuje nekompletn� ��dek dat.
		/// </summary>
		PartialLoad = 2
	}
}
