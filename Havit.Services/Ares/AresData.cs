using System;

namespace Havit.Services.Ares
{
	/// <summary>
	/// Obálka pro data z odpovìdi ze služby ARES - Obchodní rejstøík.
	/// </summary>
	public class AresData
	{
		/// <summary>
		/// Udává, zda subjekt zanikl (a jeho údaje se v registru nenachází).
		/// </summary>
		public bool? SubjektZanikl { get; set; }

		/// <summary>
		/// IÈO obchodní firmy zapsané v OR.
		/// </summary>
		public string Ico { get; set; }

		/// <summary>
		/// DIÈ obchodní firmy zapsané v OR.
		/// </summary>
		public string Dic { get; set; }

		/// <summary>
		/// Název pod kterým je firma zapsaná v OR.
		/// </summary>
		public string NazevObchodniFirmy { get; set; }

		/// <summary>
		/// Právní forma.
		/// </summary>
		public Classes.PravniForma PravniForma { get; set; }

		/// <summary>
		/// Registrace do OR.
		/// </summary>
		public Classes.RegistraceOR RegistraceOR { get; set; }

		/// <summary>
		/// Sídlo firmy.
		/// </summary>
		public Classes.Sidlo Sidlo { get; set; }

		/// <summary>
		/// Statutární orgán.
		/// </summary>
		public Classes.StatutarniOrgan StatutarniOrgan { get; set; }

		/// <summary>
		/// Tøídy (nested classes) pro odpovìd ARES OR.
		/// </summary>
		public static class Classes
		{
			/// <summary>
			/// Registrace do OR.
			/// </summary>
			public class RegistraceOR
			{
				/// <summary>
				/// Název soudu kterým je firma registrovaná v OR.
				/// </summary>
				public string NazevSoudu { get; set; }

				/// <summary>
				/// Kód soudu kterým je firma registrovaná v OR.
				/// </summary>
				public string KodSoudu { get; set; }

				/// <summary>
				/// Spisová znaèka pod kterou je firma v OR vedena (oddíl + vložka).
				/// </summary>
				public string SpisovaZnacka { get; set; }
			}

			/// <summary>
			/// Právní forma.
			/// </summary>
			public class PravniForma
			{
				/// <summary>
				/// Právní forma firmy.
				/// </summary>
				public string Nazev { get; set; }
			}

			/// <summary>
			/// Sídlo obchodní firmy.
			/// </summary>
			public class Sidlo
			{
				/// <summary>
				/// Ulice sídla firmy.
				/// </summary>
				public string Ulice { get; set; }

				/// <summary>
				/// Èíslo do adresy.
				/// </summary>
				public string CisloDoAdresy { get; set; }
				
				/// <summary>
				/// Popisné èíslo sídla firmy.
				/// </summary>
				public string CisloPopisne { get; set; }

				/// <summary>
				/// Orientaèní èíslo sídla firmy.
				/// </summary>
				public string CisloOrientacni { get; set; }

				/// <summary>
				/// Mìsto sídla firmy.
				/// </summary>
				public string Mesto { get; set; }

				/// <summary>
				/// Mìstká èást sídla firmy.
				/// </summary>
				public string MestskaCast { get; set; }

				/// <summary>
				/// PSÈ sídla firmy.
				/// </summary>
				public string Psc { get; set; }

				/// <summary>
				/// Stát sídla firmy.
				/// </summary>
				public string Stat { get; set; }

				/// <summary>
				/// Adresa textovì. Mnohé adresy nejsou strukturované, ale pøedány jen jako nestrukturovaný text.
				/// </summary>
				public string AdresaTextem { get; set; }
			}

			/// <summary>
			/// Statutarni organ.
			/// </summary>
			public class StatutarniOrgan
			{
				/// <summary>
				/// Popis statutárního orgánu (obsahuje text, jakým zpùsobem orgán statutární orgán jedná).
				/// </summary>
				public string Text { get; set; }

				/// <summary>
				/// Køstní jméno
				/// </summary>
				public string KrestniJmeno { get; set; }

				/// <summary>
				/// Pøíjmení
				/// </summary>
				public string Prijmeni { get; set; }
			}
		}
	}
}
