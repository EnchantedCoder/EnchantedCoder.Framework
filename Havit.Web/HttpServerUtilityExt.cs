using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace Havit.Web
{
	/// <summary>
	/// Poskytuje dal�� pomocn� metody pro ovl�d�n� webov�ho serveru.
	/// </summary>
	public sealed class HttpServerUtilityExt
	{
		#region ClearCache
		/// <summary>
		/// Vy�ist� cache webov� aplikace.
		/// </summary>
		public static void ClearCache()
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new InvalidOperationException("HttpContext.Current is null. Web server not available.");
			}
			if (context.Cache == null)
			{
				throw new InvalidOperationException("HttpContext.Current.Cache is null. Cache not available.");
			}
			foreach (DictionaryEntry de in context.Cache)
			{
				context.Cache.Remove(de.Key.ToString());
			}
		}
		#endregion

		#region ResolveUrl
		/// <summary>
		/// Converts a URL into one that is usable on the requesting client.
		/// </summary>
		/// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
		/// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
		/// <param name="appPath">The application path.</param>
		/// <param name="url">The URL, which might contain ~.</param>
		/// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
		/// value of the <b>appPath</b> parameter.</returns>
		public static string ResolveUrl(string appPath, string url)
		{
			if (url.Length == 0 || url[0] != '~')
			{
				return url;		// there is no ~ in the first character position, just return the url
			}
			else
			{
				if (url.Length == 1)
				{
					return appPath;  // there is just the ~ in the URL, return the appPath
				}
				if (url[1] == '/' || url[1] == '\\')
				{
					// url looks like ~/ or ~\
					if (appPath.Length > 1)
					{
						return appPath + "/" + url.Substring(2);
					}
					else
					{
						return "/" + url.Substring(2);
					}
				}
				else
				{
					// url looks like ~something
					if (appPath.Length > 1)
					{
						return appPath + "/" + url.Substring(1);
					}
					else
					{
						return appPath + url.Substring(1);
					}
				}
			}
		}

		/// <summary>
		/// Converts a URL into one that is usable on the requesting client.
		/// </summary>
		/// <remarks>Converts ~ to the requesting application path.  Mimics the behavior of the 
		/// <b>Control.ResolveUrl()</b> method, which is often used by control developers.</remarks>
		/// <param name="url">The URL, which might contain ~.</param>
		/// <returns>A resolved URL.  If the input parameter <b>url</b> contains ~, it is replaced with the
		/// value of the <see cref="System.Web.HttpRequest.ApplicationPath"/> parameter
		/// of <see cref="System.Web.HttpContext.Current"/>.</returns>
		public static string ResolveUrl(string url)
		{
			HttpContext context = HttpContext.Current;
			if (context == null)
			{
				throw new InvalidOperationException("HttpContext.Current is null.");
			}
			else if (context.Request == null)
			{
				throw new InvalidOperationException("HttpContext.Current.Request is null.");
			}
			
			return ResolveUrl(context.Request.ApplicationPath, url);
		}
		#endregion

		#region private constructor
		/// <summary>
		/// private constructor zabra�uje vytvo�en� instance t��dy
		/// </summary>
		private HttpServerUtilityExt() {}
		#endregion
	}
}
