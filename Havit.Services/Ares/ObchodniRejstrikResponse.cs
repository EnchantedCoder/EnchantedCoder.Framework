using System;

namespace Havit.Services.Ares
{
	/// <summary>
	/// Strongtypov� ob�lko pro data z odpov�di ze slu�by ARES - Obchodn� rejst��k.
	/// </summary>
	public class ObchodniRejstrikResponse
	{
		/// <summary>
		/// I�O obchodn� firmy zapsan� v OR.
		/// </summary>
		public string Ico { get; set; }

		/// <summary>
		/// DI� obchodn� firmy zapsan� v OR.
		/// </summary>
		public string Dic { get; set; }

		/// <summary>
		/// N�zev pod kter�m je firma zapsan� v OR.
		/// </summary>
		public string NazevObchodniFirmy { get; set; }

		/// <summary>
		/// Den z�pisu firmy do OR.
		/// </summary>
		public DateTime DenZapisu { get; set; }

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

		/// <summary>
		/// Pr�vn� forma firmy.
		/// </summary>
		public string PravniForma { get; set; }

		/// <summary>
		/// Stav subjektu v OR.
		/// </summary>
		public string StavSubjektu { get; set; }

		/// <summary>
		/// Ulice s�dla firmy.
		/// </summary>
		public string SidloUlice { get; set; }

		/// <summary>
		/// Popisn� ��slo s�dla firmy.
		/// </summary>
		public string SidloCisloPopisne { get; set; }

		/// <summary>
		/// Orienta�n� ��slo s�dla firmy.
		/// </summary>
		public string SidloCisloOrientacni { get; set; }

		/// <summary>
		/// M�sto s�dla firmy.
		/// </summary>
		public string SidloMesto { get; set; }

		/// <summary>
		/// M�stk� ��st s�dla firmy.
		/// </summary>
		public string SidloMestskaCast { get; set; }

		/// <summary>
		/// PS� s�dla firmy.
		/// </summary>
		public string SidloPsc { get; set; }

		/// <summary>
		/// St�t s�dla firmy.
		/// </summary>
		public string SidloStat { get; set; }

		/// <summary>
		/// Chybov� zpr�va odpov�di slu�by ARES.
		/// </summary>
		public string ResponseErrorMessage { get; set; }

		/// <summary>
		/// Indikuje, zda-li se vyskytla v odpov�di slu�by ARES - Obchodn� rejst��k  chyba.
		/// </summary>
		public bool HasError
		{
			get { return !String.IsNullOrWhiteSpace(ResponseErrorMessage); }

		}
	}
}
