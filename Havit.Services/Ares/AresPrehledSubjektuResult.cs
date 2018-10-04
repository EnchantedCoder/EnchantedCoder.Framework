using System;
using System.Collections.Generic;

namespace Havit.Services.Ares
{
	/// <summary>
	/// V�sledek s daty z odpov�di ze slu�eb ARES - p�ehled ekonomick�ch subjekt�, p�ehled osob.
	/// </summary>
	public class AresPrehledSubjektuResult
	{
		#region PrilisMnohoVysledku
		/// <summary>
		/// Nalezeno p��li� mnoho v�sledk�
		/// </summary>
		public bool PrilisMnohoVysledku { get; set; }
		#endregion

		#region Data
		/// <summary>
		/// Data vyhledan�ch subjekt�
		/// </summary>
		public List<AresPrehledSubjektuItem> Data { get; set; }
		#endregion

		#region Constructor
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public AresPrehledSubjektuResult()
		{
			Data = new List<AresPrehledSubjektuItem>();
			PrilisMnohoVysledku = false;
		}
		#endregion
	}
}