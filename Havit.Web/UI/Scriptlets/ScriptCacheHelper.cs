using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// Cache skript�. Umo��uje sd�let skripty mezi instancemi skriptletu. Nap�. pro skriptlet v ��dc�ch repeateru, apod.
	/// </summary>
	internal static class ScriptCacheHelper
	{
		#region FunctionCache (private)
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
		/// P�id� k�d s parametry (kl��) do cache pod zadan� n�zev funkce (hodnota).
		/// </summary>
		/// <param name="functionName">N�zev funkce, ve kter� je skript registrov�n.</param>
		/// <param name="functionParameters">N�zvy parametr� funkce.</param>
		/// <param name="functionCode">K�d funkce.</param>
		public static void AddFunctionToCache(string functionName, string[] functionParameters, string functionCode)
		{
			FunctionCache.Add(GetCacheKey(functionParameters, functionCode), functionName);
		}		
		#endregion

		#region GetFunctionNameFromCache
		/// <summary>
		/// Vyhled� v cache a vr�t� n�zev funkce, se stejn�mi parametry a k�dem skriptu.
		/// Pokud nen� n�zev funkce nalezen, vrac� null.
		/// </summary>
		/// <param name="functionParameters">N�zvy parametr� funkce.</param>
		/// <param name="functionCode">K�d funkce.</param>
		public static string GetFunctionNameFromCache(string[] functionParameters, string functionCode)
		{
			string result;
			if (FunctionCache.TryGetValue(GetCacheKey(functionParameters, functionCode), out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}
		#endregion

		#region GetCacheKey (private)
		/// <summary>
		/// Vr�t� kl�� do cache z parametr�.
		/// </summary>
		private static string GetCacheKey(string[] parameters, string code)
		{
			return String.Join("|", parameters) + "|" + code;
		}
		#endregion
	}
}
