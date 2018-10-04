using System;

namespace Havit.Services.Ares
{
	/// <summary>
	/// Ob�lka pro data z odpov�di ze slu�by ARES - Obchodn� rejst��k.
	/// </summary>
	public class AresData
	{
		#region Ico
		/// <summary>
		/// I�O obchodn� firmy zapsan� v OR.
		/// </summary>
		public string Ico { get; set; }
		#endregion

		#region Dic
		/// <summary>
		/// DI� obchodn� firmy zapsan� v OR.
		/// </summary>
		public string Dic { get; set; }
		#endregion

		#region NazevObchodniFirmy
		/// <summary>
		/// N�zev pod kter�m je firma zapsan� v OR.
		/// </summary>
		public string NazevObchodniFirmy { get; set; }
		#endregion

		#region PravniForma
		/// <summary>
		/// Pr�vn� forma.
		/// </summary>
		public Classes.PravniForma PravniForma { get; set; }
		#endregion

		#region RegistraceOR
		/// <summary>
		/// Registrace do OR.
		/// </summary>
		public Classes.RegistraceOR RegistraceOR { get; set; }
		#endregion

		#region Sidlo
		/// <summary>
		/// S�dlo firmy.
		/// </summary>
		public Classes.Sidlo Sidlo { get; set; }
		#endregion

		#region StatutarniOrgan
		/// <summary>
		/// Statut�rn� org�n.
		/// </summary>
		public Classes.StatutarniOrgan StatutarniOrgan { get; set; }
		#endregion

		#region Nested classes
		/// <summary>
		/// T��dy (nested classes) pro odpov�d ARES OR.
		/// </summary>
		public class Classes
		{
			/// <summary>
			/// Registrace do OR.
			/// </summary>
			public class RegistraceOR
			{
				/// <summary>
				/// N�zev soudu kter�m je firma registrovan� v OR.
				/// </summary>
				public string NazevSoudu { get; set; }

				/// <summary>
				/// K�d soudu kter�m je firma registrovan� v OR.
				/// </summary>
				public string KodSoudu { get; set; }

				/// <summary>
				/// Spisov� zna�ka pod kterou je firma v OR vedena (odd�l + vlo�ka).
				/// </summary>
				public string SpisovaZnacka { get; set; }
			}

			/// <summary>
			/// Pr�vn� forma.
			/// </summary>
			public class PravniForma
			{
				/// <summary>
				/// Pr�vn� forma firmy.
				/// </summary>
				public string Nazev { get; set; }
			}

			/// <summary>
			/// S�dlo obchodn� firmy.
			/// </summary>
			public class Sidlo
			{
				/// <summary>
				/// Ulice s�dla firmy.
				/// </summary>
				public string Ulice { get; set; }

				/// <summary>
				/// ��slo do adresy.
				/// </summary>
				public string CisloDoAdresy { get; set; }
				
				/// <summary>
				/// Popisn� ��slo s�dla firmy.
				/// </summary>
				public string CisloPopisne { get; set; }

				/// <summary>
				/// Orienta�n� ��slo s�dla firmy.
				/// </summary>
				public string CisloOrientacni { get; set; }

				/// <summary>
				/// M�sto s�dla firmy.
				/// </summary>
				public string Mesto { get; set; }

				/// <summary>
				/// M�stk� ��st s�dla firmy.
				/// </summary>
				public string MestskaCast { get; set; }

				/// <summary>
				/// PS� s�dla firmy.
				/// </summary>
				public string Psc { get; set; }

				/// <summary>
				/// St�t s�dla firmy.
				/// </summary>
				public string Stat { get; set; }

				/// <summary>
				/// Adresa textov�. Mnoh� adresy nejsou strukturovan�, ale p�ed�ny jen jako nestrukturovan� text.
				/// </summary>
				public string AdresaTextem { get; set; }
			}

			/// <summary>
			/// Statutarni organ.
			/// </summary>
			public class StatutarniOrgan
			{
				/// <summary>
				/// Popis statut�rn�ho org�nu (obsahuje text, jak�m zp�sobem org�n statut�rn� org�n jedn�).
				/// </summary>
				public string Text { get; set; }
			}
		}
#endregion
	}
}
