using System;

namespace Havit.Services.Ares
{
	/// <summary>
	/// Ob�lka pro data z odpov�di ze slu�eb ARES - p�ehled ekonomick�ch subjekt�, p�ehled osob.
	/// </summary>
	public class AresPrehledSubjektuItem
	{
		#region Ico
		/// <summary>
		/// I�O obchodn� firmy zapsan� v OR.
		/// </summary>
		public string Ico { get; set; }
		#endregion

		#region Nazev
		/// <summary>
		/// N�zev osoby nebo ekonomick�ho subjektu
		/// </summary>
		public string Nazev { get; set; }
		#endregion

		#region Kontakt
		/// <summary>
		/// Adresa/Kontaktn� osoba subjektu
		/// </summary>
		public string Kontakt { get; set; }
		#endregion
	}
}
