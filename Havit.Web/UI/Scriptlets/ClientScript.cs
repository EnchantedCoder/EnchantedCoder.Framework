using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// P�edstavuje klientsk� skript - funkci, kter� je vyvol�na p�i zm�n� vstupn�ch
	/// parametr� <see cref="Scriptlet">Scriptletu</see> nebo p�i na�t�n� str�nky �i callbacku (pokud je povoleno).
	/// </summary>
	//[DefaultProperty("Script")]
	[ParseChildren(true, "Script")]
	public class ClientScript : ScriptletNestedControl
	{

		/* PARAMETY CLIENTSCRIPTU *************** */

		#region Script
		/// <summary>
		/// Klientsk� skript. Okolo skriptu je vytvo�ena ob�lka a p��padn� je spu�t�n po na�ten� str�nky �i callbacku.
		/// </summary>
		[PersistenceMode(PersistenceMode.InnerDefaultProperty)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public string Script
		{
			get { return (string)ViewState["Script"] ?? String.Empty; }
			set { ViewState["Script"] = value; }
		}
		#endregion

		#region StartOnLoad
		/// <summary>
		/// Ud�v�, zda m� po na�ten� str�nky doj�t k automatick�mu spu�t�n� skriptu. V�choz� hodnota je <c>false</c>.
		/// </summary>				
		/// <remarks>
		/// Nem� vliv na zpracov�n� str�nky p�i asynchronn�m postbacku (callbacku). K tomu slou�� vlastnost <see cref="StartOnAjaxCallback"/>StartOnAjaxCallback</see>.
		/// </remarks>
		public bool StartOnLoad
		{
			get { return (bool)(ViewState["StartOnLoad"] ?? false); }
			set { ViewState["StartOnLoad"] = value; }
		}
		#endregion

		#region StartOnAjaxCallback
		/// <summary>
		/// Ud�v�, zda m� po doj�t k automatick�mu spu�t�n� skriptu po asynchronn�m postbacku (ajax callback). V�choz� hodnota je <b>false</b>.
		/// </summary>
		public bool StartOnAjaxCallback
		{
			get { return (bool)(ViewState["StartOnAjaxCallback"] ?? false); }
			set { ViewState["StartOnAjaxCallback"] = value; }
		}		
		#endregion		

		/*  *************** */

		#region GetAutoStart
		/// <summary>
		/// Vrac� true, pokud m� b�t renderov�n skript pro automatick� spu�t�n� funkce scriptletu.
		/// </summary>
		/// <returns></returns>
		public bool GetAutoStart()
		{
			if (this.Scriptlet.IsInAsyncPostBack)
			{
				return StartOnAjaxCallback;
			}
			else
			{
				return StartOnLoad;
			}
		}
		#endregion

		/* RENDEROV�N� *************** */
/*
		#region ClientSideScriptFunctionName (internal)
		/// <summary>
		/// N�zev funkce, pod jakou je klientsk� skript zaregistrov�n.
		/// Hodnota je dostupn� a� po zavol�n� metody RegisterClientScript.
		/// </summary>
		internal string ClientSideScriptFunctionName
		{
			get
			{
				if (clientSideScriptFunctionName == null)
				{
					throw new InvalidOperationException("��st vlastnost ClientSideFunctionName je�t� nen� dovoleno.");
				}

				return clientSideScriptFunctionName;
			}
		}
		private string clientSideScriptFunctionName;
		#endregion
*/
		#region GetClientSideScriptFunction
		/// <summary>
		/// Metoda vytvo�� funkci okolo kliensk�ho skriptu a zaregistruje ji do str�nky.
		/// Je zaji�t�no, aby nebyla funkce se shodn�m obsahem registrov�na v�ckr�t
		/// (nap�. p�i pou�it� controlu v repeateru).
		/// </summary>
		/// <param name="scriptBuilder">ScriptBuilder, do kter�ho je tvo�en klientsk� skript.</param>
		public virtual string GetClientSideFunctionCode()
		{
			// na�teme klientsk� script
			return GetSubstitutedScript();
//            // pod�v�me se, zda ji� byl registrov�n
//            string functionName = ScriptCacheHelper.GetFunctionNameFromCache(_clientScriptParameters, code);

//            if (functionName == null)
//            {
//                // pokud tento skript je�t� nebyl registrov�n...				
//                CreateFunctionName(null);
//                // ulo��me jej do cache
//                ScriptCacheHelper.AddFunctionToCache(clientSideScriptFunctionName, _clientScriptParameters, code);
//                // a zaregistrujeme
//                return WrapClientSideScriptToFunction(clientSideScriptFunctionName, _clientScriptParameters, code);
//            }
//            else
//            {
//                // pokud tento skript ji� byl registrov�n,
//                // pou�ijeme tento registrovan� skript
//                CreateFunctionName(functionName);
////				clientSideScriptFunctionName = functionName;
//                return null;
//            }
		}
		#endregion
	/*
		#region CreateFunctionName (private)
		/// <summary>
		/// Vytvo�� n�zev klientsk� funkce. Pokud je parametr reuseFunction pr�zdn�,
		/// vytvo�� nov� n�zev, jinak pou�ije hodnotu tohoto parametru.
		/// </summary>
		/// <param name="reuseFunctionName"></param>
		private void CreateFunctionName(string reuseFunctionName)
		{
			if (!String.IsNullOrEmpty(reuseFunctionName))
			{
				clientSideScriptFunctionName = reuseFunctionName;
			}
			else
			{
				clientSideScriptFunctionName = "scriptletFunction_" + this.Scriptlet.ClientID;
			}
		}		
		#endregion
		*/
		private static string[] _clientScriptParameters = new string[] { "parameters" };
		
		/* SUBSTITUCE *************** */

		#region GetSubstitutedScript (protected)
		/// <summary>
		/// Vr�t� p�ipraven� klientsk� skript. Provede nad skriptem substituce.
		/// </summary>
		/// <returns>Klientsk� skript p�ipraven� k vytvo�en� ob�lky a registraci.</returns>
		protected virtual string GetSubstitutedScript()
		{
			ClientScriptSubstituingEventArgs eventArgs = new ClientScriptSubstituingEventArgs(Script);
			OnClientScriptSubstituing(eventArgs);
			return Scriptlet.ScriptSubstitution.Substitute(eventArgs.ClientScript);
		}
		#endregion

		#region ClientScriptSubstituingEventArgs (protected)
		protected virtual void OnClientScriptSubstituing(ClientScriptSubstituingEventArgs eventArgs)
		{
			if (_clientScriptSubstituing != null)
			{
				_clientScriptSubstituing.Invoke(this, eventArgs);
			}
		}
		#endregion

		#region ClientScriptSubstituing
		public event ClientScriptSubstituingEventHandler ClientScriptSubstituing
		{
			add
			{
				_clientScriptSubstituing += value;
			}
			remove
			{
				_clientScriptSubstituing -= value;
			}
		}
		private ClientScriptSubstituingEventHandler _clientScriptSubstituing;
		
		#endregion

		/* STATIC  *************** */

		#region WrapClientSideScriptToFunction (static)
		/// <summary>
		/// Zabal� klientsk� skript do funkce.
		/// </summary>
		/// <returns>Klientsk� skript jako funkce p�ipraven� k registraci do str�nky.</returns>
		public static string WrapClientSideScriptToFunction(string functionName, string[] parameters, string clientScript)
		{
			return String.Format("function {0}({1}){3}{{{3}{2}{3}}}{3}",
				functionName,
				parameters == null ? String.Empty : String.Join(", ", parameters),
				clientScript.Trim(),
				Environment.NewLine);
		}
		#endregion

	}
}
