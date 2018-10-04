using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Havit.Services.Ares
{
	/// <summary>
	/// T��da implementuj�c� na��t�n� dat z obchodn�ho rejst��ku (ARES).
	/// </summary>
	public class AresService
	{
		#region Const
		private const string AresBasicDataRequestUrl = "http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_bas.cgi?ico=";
		private const string AresObchodniRejstrikDataRequestUrl = "http://wwwinfo.mfcr.cz/cgi-bin/ares/darv_or.cgi?ico=";
		#endregion

		#region Private members
		/// <summary>
		/// I� subjektu, kter�ho �daje chceme z�skat.
		/// </summary>
		private string Ico { get; set; }

		#endregion

		#region Timeout
		/// <summary>
		/// Timeout (v milisekund�ch) jednoho requestu p�i na��t�n� dat z ARESu.
		/// Pokud nen� hodnota nastavena, nen� d�lka requestu omezov�na (resp. je pou�ito standardn� nastaven� .NETu).
		/// </summary>
		public int? Timeout
		{
			get; set;
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Konstruktor.
		/// </summary>
		/// <param name="ico">I�O spole�nosti.</param>
		public AresService(string ico)
		{
			Ico = ico;
		}
		#endregion

		#region GetData

		/// <summary>
		/// Vrac� strukturovanou odpov�d z obchodn�ho rejst��ku.
		/// </summary>
		public AresData GetData(AresRegistr rejstriky = AresRegistr.Basic | AresRegistr.ObchodniRejstrik)
		{
			AresData result = new AresData();
			List<Task> tasks = new List<Task>();

			if (rejstriky.HasFlag(AresRegistr.Basic))
			{
				tasks.Add(Task.Factory.StartNew(LoadBasicData, result));
			}

			if (rejstriky.HasFlag(AresRegistr.ObchodniRejstrik))
			{
				tasks.Add(Task.Factory.StartNew(LoadObchodniRejstrikData, result));
			}

			Task.WaitAll(tasks.ToArray());

			return result;
		}
		#endregion

		#region LoadBasicData, ParseBasicData
		private void LoadBasicData(object state)
		{
			AresData result = (AresData)state;

			string requestUrl = String.Format("{0}{1}", AresBasicDataRequestUrl, Ico);
			XDocument aresResponseXDocument = this.GetAresResponseXDocument(requestUrl);

			XNamespace aresDT = XNamespace.Get("http://wwwinfo.mfcr.cz/ares/xml_doc/schemas/ares/ares_datatypes/v_1.0.3");

			// Error
			XElement eElement = aresResponseXDocument.Root.Elements().Elements(aresDT + "E").SingleOrDefault();
			if (eElement != null)
			{
				if ((int)eElement.Elements(aresDT + "EK").SingleOrDefault() == 1 /* Nenalezen */)
				{
					return; // nehl�s�me chybu ani neparsujeme data
				}

				throw new AresException((string)eElement.Elements(aresDT + "ET").SingleOrDefault());
			}

			lock (result)
			{
				this.ParseBasicData(aresResponseXDocument, aresDT, result);
			}
		}

		private void ParseBasicData(XDocument aresResponse, XNamespace aresDT, AresData result)
		{
		// V�pis BASIC (element).
			XElement vypisOrElement = aresResponse.Descendants(aresDT + "VBAS").SingleOrDefault();

			if (vypisOrElement != null)
			{
				result.Ico = (string)vypisOrElement.Elements(aresDT + "ICO").SingleOrDefault();
				result.Dic = (string)vypisOrElement.Elements(aresDT + "DIC").SingleOrDefault();
				result.NazevObchodniFirmy = (string)vypisOrElement.Elements(aresDT + "OF").SingleOrDefault(); // obchodn� firma

				XElement npfElement = vypisOrElement.Elements(aresDT + "PF").Elements(aresDT + "NPF").SingleOrDefault();
				if (npfElement != null)
				{
					result.PravniForma = new AresData.Classes.PravniForma();
					result.PravniForma.Nazev = (string)npfElement;
				}

			}
		}
		#endregion

		#region LoadObchodniRejstrikData, ParseObchodniRejstrikData
		private void LoadObchodniRejstrikData(object state)
		{
			AresData result = (AresData)state;

			string requestUrl = String.Format("{0}{1}", AresObchodniRejstrikDataRequestUrl, Ico);
			XDocument aresResponseXDocument = this.GetAresResponseXDocument(requestUrl);

			XNamespace aresDT = XNamespace.Get("http://wwwinfo.mfcr.cz/ares/xml_doc/schemas/ares/ares_datatypes/v_1.0.3");

			// Error
			XElement eElement = aresResponseXDocument.Root.Elements().Elements(aresDT + "E").SingleOrDefault();
			if (eElement != null)
			{
				if ((int)eElement.Elements(aresDT + "EK").SingleOrDefault() == 1 /* Nenalezen */)
				{
					return; // nehl�s�me chybu ani neparsujeme data
				}

				throw new AresException((string)eElement.Elements(aresDT + "ET").SingleOrDefault());
			}

			lock (result)
			{
				this.ParseObchodniRejstrikData(aresResponseXDocument, aresDT, result);
			}
		}

		private void ParseObchodniRejstrikData(XDocument aresResponse, XNamespace aresDT, AresData result)
		{
			// V�pis OR (element).
			XElement vypisOrElement = aresResponse.Descendants(aresDT + "Vypis_OR").SingleOrDefault();

			if (vypisOrElement != null)
			{
				result.Ico = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "ICO").SingleOrDefault();
				result.NazevObchodniFirmy = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "OF").SingleOrDefault(); // obchodn� firma

				// Registrace OR
				XElement registraceElement = vypisOrElement.Elements(aresDT + "REG").SingleOrDefault();
				if (registraceElement != null)
				{
					result.RegistraceOR = new AresData.Classes.RegistraceOR();

					XElement szElement = registraceElement.Elements(aresDT + "SZ").SingleOrDefault();

					if (szElement != null)
					{
						XElement sdElement = szElement.Elements(aresDT + "SD").SingleOrDefault();

						if (sdElement != null)
						{
							result.RegistraceOR.NazevSoudu = (string)sdElement.Elements(aresDT + "T").SingleOrDefault();
							result.RegistraceOR.KodSoudu = (string)sdElement.Elements(aresDT + "K").SingleOrDefault();
						}

						result.RegistraceOR.SpisovaZnacka = (string)szElement.Elements(aresDT + "OV").SingleOrDefault();
					}
				}

				XElement npfElement = vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "PFO").Elements(aresDT + "NPF").SingleOrDefault();
				if (npfElement != null)
				{
					result.PravniForma = new AresData.Classes.PravniForma();
					result.PravniForma.Nazev = (string)npfElement;
				}

				//obchodniRejstrikResponse.StavSubjektu = (string)vbasElement.Descendants(aresDT + "SSU").SingleOrDefault();

				result.Sidlo = new AresData.Classes.Sidlo();
				result.Sidlo.Ulice = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "NU").SingleOrDefault();
				
				result.Sidlo.CisloDoAdresy = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "CA").SingleOrDefault();
				result.Sidlo.CisloPopisne = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "CD").SingleOrDefault();
				result.Sidlo.CisloOrientacni = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "CO").SingleOrDefault();

				result.Sidlo.Mesto = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "N").SingleOrDefault();
				result.Sidlo.MestskaCast = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "NCO").SingleOrDefault();
				result.Sidlo.Psc = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "PSC").SingleOrDefault();
				result.Sidlo.Stat = (string)vypisOrElement.Elements(aresDT + "ZAU").Elements(aresDT + "SI").Elements(aresDT + "NS").SingleOrDefault();

				// statut�rn� org�n
				var soElement = vypisOrElement.Elements(aresDT + "SO").SingleOrDefault();
				if (soElement != null)
				{
					result.StatutarniOrgan = new AresData.Classes.StatutarniOrgan();
					var statutartniOrganTextElement = soElement.Elements(aresDT + "T").SingleOrDefault();
					if (statutartniOrganTextElement != null)
					{
						result.StatutarniOrgan.Text = ((string)statutartniOrganTextElement).Trim();
					}
				}
			}
		}
		#endregion

		#region GetAresResponseXDocument
		/// <summary>
		/// Ode�le dotaz do obchodn�ho rejst��ku pro dan� I� a vr�t� odpov�d jako XDocument objekt.
		/// </summary>
		private XDocument GetAresResponseXDocument(string requestUrl)
		{
			XDocument aresResponseXDocument = null;

			try
			{
				WebRequest aresRequest = HttpWebRequest.Create(requestUrl);
				if (this.Timeout != null)
				{
					aresRequest.Timeout = this.Timeout.Value;
				}
				HttpWebResponse aresResponse = (HttpWebResponse)aresRequest.GetResponse();

				aresResponseXDocument = XDocument.Load(new StreamReader(aresResponse.GetResponseStream()));
			}
			catch (Exception e)
			{
				throw new AresLoadException(String.Format("Chyba \"{0}\" p�i pokusu o z�sk�n� dat ze slu�by ARES ({1}).", e.Message, requestUrl));
			}

			return aresResponseXDocument;
		}
		#endregion

	}
}
