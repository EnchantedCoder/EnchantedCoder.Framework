using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Havit.Web.UI.Scriptlets
{
	public static class ScriptCacheHelper
	{
		#region FunctionCache
		/// <summary>
		/// Cache pro klientsk� skripty. Kl��em je skript a parametry funkce, hodnotou je n�zev funkce,
		/// ve kter� je skript registrov�n.
		/// Cache je ulo�ena v HttpContextu.
		/// </summary>
		private static Dictionary<string, string> FunctionCache
		{
			get
			{
				Dictionary<string, string> result = (Dictionary<string, string>)HttpContext.Current.Items[typeof(ScriptCacheHelper)];

				if (result == null)
				{
					// pokud cache je�t� nen�, vytvo��me ji a vr�t�me
					// ��dn� z�mky (lock { ... }) nejsou pot�eba, jsme st�le v jednom HttpContextu
					result = new Dictionary<string, string>();
					HttpContext.Current.Items[typeof(ScriptCacheHelper)] = result;
				}

				return result;
			}
		}
		#endregion

		#region AddFunctionToCache
		/// <summary>
		/// P�id� klientsk� skript do cache.
		/// </summary>
		/// <param name="functionName">N�zev funkce, ve kter� je skript registrov�n.</param>
		/// <param name="code">Klientsk� skript.</param>
		public static void AddFunctionToCache(string functionName, string[] parameters, string code)
		{
			FunctionCache.Add(GetCacheKey(parameters, code), functionName);
		}		
		#endregion

		#region GetFunctionNameFromCache
		/// <summary>
		/// Nalezne n�zev funkce, ve kter� je klientsk� skript registrov�n.
		/// </summary>
		/// <param name="code">Klientsk� skript.</param>
		/// <returns>Nalezne n�zev funkce, ve kter� je klientsk� skript 
		/// registrov�n. Pokud skript nen� registrov�n, vr�t� null.</returns>
		public static string GetFunctionNameFromCache(string[] parameters, string code)
		{
			string result;
			if (FunctionCache.TryGetValue(GetCacheKey(parameters, code), out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}
		#endregion

#warning Comment
		private static string GetCacheKey(string[] parameters, string code)
		{
			if (parameters == null)
			{
				return code;
			}
			else
			{
				return String.Join("|", parameters) + "|" + code;
			}
		}
	}
}
