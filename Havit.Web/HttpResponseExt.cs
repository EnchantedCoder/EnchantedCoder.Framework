using System;
using System.Text;
using System.Web;
using Havit.Reflection;

namespace Havit.Web
{
	/// <summary>
	/// Obsahuje roz�i�uj�c� funk�nost k t��d� <see cref="System.Web.HttpResponse"/>.
	/// </summary>
	public class HttpResponseExt
	{
		#region MovedPermanently
		/// <summary>
		/// Provede redirect pomoc� HTTP status k�du 301 - Moved Permanently.
		/// Klasick� <see cref="System.Web.HttpResponse.Redirect(string)"/> prov�d� redirect p�es 302 - Found (Object Moved).
		/// </summary>
		/// <remarks>
		/// Zat�mco klasick� <see cref="System.Web.HttpResponse.Redirect(string)"/> prov�d� redirect p�es HTTP status k�d 302,
		/// co� je "temporarily moved", redirect p�es "301 - Moved Permanently" ��k� klientovi, �e URL po�adovan� str�nky
		/// se definitivn� zm�nilo na novou adresu.<br/>
		/// Klient by m�l teoreticky reagovat �pravou bookmarku, ale ��dn� to ned�l�. Smysl to m� v�ak pro indexovac� roboty
		/// vyhled�va��, kter� se t�m �dajn� docela ��d�.<br/>
		/// POZOR: Na rozd�l od <see cref="System.Web.HttpResponse.Redirect(string)"/> nekontroluje, jestli u� nebyly odesl�ny klientovi hlavi�ky.
		/// </remarks>
		/// <param name="url">C�lov� adresa.</param>
		/// <param name="endResponse">Indikuje, zda-li m� skon�it zpracov�n� vykon�v�n� str�nky.</param>
		public static void MovedPermanently(string url, bool endResponse)
		{
			if ((HttpContext.Current == null)
				|| (HttpContext.Current.Response == null))
			{
				throw new InvalidOperationException("HttpContext.Current.Response unavailable.");
			}
			HttpResponse response = HttpContext.Current.Response;

			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.IndexOf('\n') >= 0)
			{
				throw new ArgumentException("Cannot redirect to newline");
			}
			url = response.ApplyAppPathModifier(url);
			url = HttpUtilityExt.UrlEncodePathWithQueryString(url);
			
			response.Clear();
			response.StatusCode = 301;
			response.StatusDescription = "Moved Permanently";
			response.AddHeader("Location", url);
			response.Write("<html><head><title>Moved Permanently</title></head>\r\n");
			response.Write("<body><h2>301 Moved Permanently</h2>\r\n");
			response.Write("<p>Requested page permanently moved to <a href=\"" + HttpUtility.HtmlEncode(url) + "\">here</a>.</p>\r\n");
			response.Write("</body></html>\r\n");

			if (endResponse)
			{
				response.End();
			}
		}

		/// <summary>
		/// Provede redirect pomoc� HTTP status k�du 301 - Moved Permanently a ukon�� zpracov�n� str�nky.
		/// Klasick� <see cref="System.Web.HttpResponse.Redirect(string)"/> prov�d� redirect p�es 302 - Found (Object Moved).
		/// </summary>
		/// <remarks>
		/// Zat�mco klasick� <see cref="System.Web.HttpResponse.Redirect(string)"/> prov�d� redirect p�es HTTP status k�d 302,
		/// co� je "temporarily moved", redirect p�es "301 - Moved Permanently" ��k� klientovi, �e URL po�adovan� str�nky
		/// se definitivn� zm�nilo na novou adresu.<br/>
		/// Klient by m�l teoreticky reagovat �pravou bookmarku, ale ��dn� to ned�l�. Smysl to m� v�ak pro indexovac� roboty
		/// vyhled�va��, kter� se t�m �dajn� docela ��d�.
		/// </remarks>
		/// <param name="url">C�lov� adresa.</param>
		public static void MovedPermanently(string url)
		{
			HttpResponseExt.MovedPermanently(url, true);
		}
		#endregion

		#region Gone
		/// <summary>
		/// Ode�le klientovi odezvu se status k�dem 410 - Gone, tj. "str�nka byla zru�ena bez n�hrady".
		/// </summary>
		/// <param name="endResponse">Indikuje, zda-li m� skon�it zpracov�n� vykon�v�n� str�nky.</param>
		public static void Gone(bool endResponse)
		{
			if ((HttpContext.Current == null)
				|| (HttpContext.Current.Response == null))
			{
				throw new InvalidOperationException("HttpContext.Current.Response unavailable.");
			}
			HttpResponse response = HttpContext.Current.Response;
			
			response.Clear();
			response.StatusCode = 410;
			response.StatusDescription = "Gone";
			response.Write("<html><head><title>Gone</title></head>\r\n");
			response.Write("<body><h2>410 Gone</h2>\r\n");
			response.Write("<p>Requested page was permanently discontinued. Please remove all links here.</p>\r\n");
			response.Write("</body></html>\r\n");

			if (endResponse)
			{
				response.End();
			}
		}

		/// <summary>
		/// Ode�le klientovi odezvu se status k�dem 410 - Gone, tj. "str�nka byla zru�ena bez n�hrady"
		/// a ukon�� zpracov�n� str�nky.
		/// </summary>
		public static void Gone()
		{
			HttpResponseExt.Gone(true);
		}
		#endregion

		#region Private Constructor
		private HttpResponseExt() {}
		#endregion
	}
}
