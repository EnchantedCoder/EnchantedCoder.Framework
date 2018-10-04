using System;
using System.Web;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Havit.Services.Ares
{
	/// <summary>
	/// T��da implementuj�c� na��t�n� dat z obchodn�ho rejst��ku (ARES).
	/// </summary>
	public class AresPrehledSubjektuService
	{
		#region Const
		private const string AresEkonomickySubjektRequestUrl = "http://wwwinfo.mfcr.cz/cgi-bin/ares/ares_es.cgi?obch_jm={0}&obec={1}&cestina=cestina&maxpoc=200";
		private const string AresFyzickaOsobaRequestUrl = "http://wwwinfo.mfcr.cz/cgi-bin/ares/ares_fo.cgi?jmeno={0}&obec={1}&cestina=cestina&maxpoc=200";
		#endregion

		#region Private members
		/// <summary>
		/// n�zev subjektu, kter�ho �daje chceme z�skat.
		/// </summary>
		private string Nazev { get; set; }

		/// <summary>
		/// Obec/M�sto, kde se subjekt nach�z�.
		/// </summary>
		private string Obec { get; set; }
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

		#region GetData

		/// <summary>
		/// Vrac� strukturovanou odpov�d z obchodn�ho rejst��ku.
		/// </summary>
		public AresPrehledSubjektuResult GetData(string nazev, string obec = null)
		{
			AresPrehledSubjektuResult result = new AresPrehledSubjektuResult();
			List<Task> tasks = new List<Task>();

			this.Nazev = nazev;
			this.Obec = obec;

			tasks.Add(Task.Factory.StartNew(LoadEkonomickySubjektData, result));
			tasks.Add(Task.Factory.StartNew(LoadFyzickaOsobaData, result));

			try
			{
				Task.WaitAll(tasks.ToArray());
			}
			catch (AggregateException exception)
			{
				// pokus o vybalen� v�jimky (chceme �e�it jen jedinou)
				if ((exception.InnerExceptions.Count > 0) && (exception.InnerExceptions[0] is AresBaseException))
				{
					throw exception.InnerExceptions[0];
				}
				else
				{
					throw;
				}
			}

			return result;
		}
		#endregion

		#region LoadEkonomickySubjektData
		private void LoadEkonomickySubjektData(object state)
		{
			string requestUrl = String.Format(AresEkonomickySubjektRequestUrl, System.Web.HttpUtility.UrlEncode(Nazev, Encoding.GetEncoding("windows-1250")), System.Web.HttpUtility.UrlEncode(Obec, Encoding.GetEncoding("windows-1250")));
			LoadData(requestUrl, state);
		}
		#endregion

		#region LoadFyzickaOsobaData
		private void LoadFyzickaOsobaData(object state)
		{
			string requestUrl = String.Format(AresFyzickaOsobaRequestUrl, Nazev, Obec);
			LoadData(requestUrl, state);
		}
		#endregion

		#region LoadData
		private void LoadData(String requestUrl, object state)
		{
			AresPrehledSubjektuResult result = (AresPrehledSubjektuResult)state;

			XDocument aresResponseXDocument = this.GetAresResponseXDocument(requestUrl);
			XNamespace aresDT = XNamespace.Get("http://wwwinfo.mfcr.cz/ares/xml_doc/schemas/ares/ares_datatypes/v_1.0.4");

			// Error
			XElement eElement = aresResponseXDocument.Root.Elements().Elements(aresDT + "Help").FirstOrDefault();
			if (eElement != null)
			{
				XElement rElement = eElement.Elements(aresDT + "R").FirstOrDefault();
				if (rElement != null && ((string)rElement).StartsWith("Nenalezen")  /* Nenalezen */)
				{
					return; // nehl�s�me chybu ani neparsujeme data
				}

				if (rElement != null && (((string)rElement).Contains("vede k v�b�ru v�ce ne�") || ((string)rElement).Contains("p�es�hl nastavenou mez"))   /* P��li� mnoho v�sledk� */)
				{
					result.PrilisMnohoVysledku = true;
					return;
				}

				throw new AresException((string)rElement);
			}

			lock (result)
			{
				this.ParseData(aresResponseXDocument, aresDT, result);
			}
		}
		#endregion

		#region ParseData
		private void ParseData(XDocument aresResponse, XNamespace aresDT, AresPrehledSubjektuResult result)
		{
			// V�pis BASIC (element).
			IEnumerable<XElement> vypisOrElements = aresResponse.Descendants(aresDT + "S");

			if ((vypisOrElements != null) && (vypisOrElements.Count() > 0))
			{
				foreach (XElement item in vypisOrElements)
				{
					AresPrehledSubjektuItem resultItem = new AresPrehledSubjektuItem();
					resultItem.Ico = (string)item.Elements(aresDT + "ico").FirstOrDefault();
					resultItem.Nazev = (string)item.Elements(aresDT + "ojm").FirstOrDefault();
					resultItem.Kontakt = (string)item.Elements(aresDT + "jmn").FirstOrDefault();
					result.Data.Add(resultItem);
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
			catch (WebException e)
			{
				throw new AresLoadException(String.Format("Chyba \"{0}\" p�i pokusu o z�sk�n� dat ze slu�by ARES ({1}).", e.Message, requestUrl));
			}

			return aresResponseXDocument;
		}
		#endregion

	}
}
