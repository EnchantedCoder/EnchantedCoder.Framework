using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Web;

namespace Havit.Web
{
	/// <summary>
	/// Pomocn�k pro sestaven� QueryStringu.
	/// </summary>
	public class QueryStringBuilder : NameValueCollection
	{
		#region ctor
		/// <summary>
		/// Vytvo�� instanci.
		/// </summary>
		/// <remarks>
		/// Pou��v� se StringComparer.OrdinalIgnoreCase po vzoru System.Web.HttpValueCollection.
		/// </remarks>
		public QueryStringBuilder()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}
		#endregion

		#region Add
		/// <summary>
		/// P�id� hodnotu do QueryStringu. Pokud ji� hodnota existuje, potom p�id� dal�� a QueryString bude obsahovat hodnot v�ce.
		/// Pokud chcete nastavit hodnoty bez mo�nosti duplicit, pou�ijte metodu Set().
		/// </summary>
		/// <exception cref="ArgumentException">pokud je argument name null nebo String.Empty</exception>
		/// <param name="name">n�zev hodnoty</param>
		/// <param name="value">hodnota</param>
		public override void Add(string name, string value)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Argument nesm� b�t null ani String.Empty.", "name");
			}

			base.Add(name, value);
		}
		#endregion

		#region Set
		/// <summary>
		/// Nastav� hodnotu do QueryStringu. Pokud ji� hodnota existuje, potom ji p�enastav� na novou hodnotu.
		/// Pokud hodnota neexistuje, zalo�� ji. Pokud chcete p�id�vat hodnoty s mo�nosti duplicit, pou�ijte metodu Add().
		/// </summary>
		/// <exception cref="ArgumentException">pokud je argument name null nebo String.Empty</exception>
		/// <param name="name">n�zev hodnoty</param>
		/// <param name="value">hodnota</param>
		public override void Set(string name, string value)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException("Argument nesm� b�t null ani String.Empty.", "name");
			}

			base.Set(name, value);
		}
		#endregion

		#region ToString
		/// <summary>
		/// P�evede na QueryString, neobsahuje �vodn� ? (otazn�k).
		/// </summary>
		/// <param name="urlEncoded">indikuje, zda-li m� b�t v�stup (n�zvy i hodnoty) UrlEncoded</param>
		/// <returns>QueryString bez �vodn�ho ? (otazn�ku)</returns>
		public virtual string ToString(bool urlEncoded)
		{
			int count = this.Count;
			if (count == 0)
			{
				return string.Empty;
			}
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				string key = this.GetKey(i);
				string value;
				if (urlEncoded)
				{
					key = HttpUtility.UrlEncode(key);
				}
				string keyEquals = key + "=";
				ArrayList values = (ArrayList)base.BaseGet(i);
				int valuesCount = (values != null) ? values.Count : 0;
				if (sb.Length > 0)
				{
					sb.Append('&');
				}
				if (valuesCount == 1)
				{
					sb.Append(keyEquals);
					value = (string)values[0];
					if (urlEncoded)
					{
						value = HttpUtility.UrlEncode(value);
					}
					sb.Append(value);
				}
				else if (valuesCount == 0)
				{
					sb.Append(keyEquals);
				}
				else
				{
					for (int j = 0; j < valuesCount; j++)
					{
						if (j > 0)
						{
							sb.Append('&');
						}
						sb.Append(keyEquals);
						value = (string)values[j];
						if (urlEncoded)
						{
							value = HttpUtility.UrlEncode(value);
						}
						sb.Append(value);
					}
				}
			}
			return sb.ToString();
		}

		/// <summary>
		/// P�evede na url-encoded QueryString bez �vodn�ho ? (otazn�ku).
		/// </summary>
		/// <returns>url-encoded QueryString bez �vodn�ho ? (otazn�ku)</returns>
		public override string ToString()
		{
			return this.ToString(true);
		}
		#endregion

		#region GetUrlWithQueryString
		/// <summary>
		/// Sestav� URL s QueryStringem na z�klad� zadan�ho URL (kter� ji� m��e n�jak� QueryString obsahovat).
		/// Pokud chcete z�skat samotn� QueryString, pou�ijte metodu ToString().
		/// </summary>
		/// <param name="url">vstupn� URL (kter� ji� m��e n�jak� QueryString obsahovat)</param>
		/// <returns>URL s QueryStringem</returns>
		public string GetUrlWithQueryString(string url)
		{
			if (url == null)
			{
				url = String.Empty;
			}

			if (url.Contains("?"))
			{
				char last = url[url.Length - 1];
				if ((last != '&') && (last != '?'))
				{
					url = url + "&";
				}
			}
			else
			{
				url = url + "?";
			}
			return url + this.ToString();
		}
		#endregion
	}
}
