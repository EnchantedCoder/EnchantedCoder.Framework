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
		
		#region ClientSideScriptFunctionName
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

		#region GetClientSideScriptFunction
		/// <summary>
		/// Metoda vytvo�� funkci okolo kliensk�ho skriptu a zaregistruje ji do str�nky.
		/// Je zaji�t�no, aby nebyla funkce se shodn�m obsahem registrov�na v�ckr�t
		/// (nap�. p�i pou�it� controlu v repeateru).
		/// </summary>
		/// <param name="scriptBuilder">ScriptBuilder, do kter�ho je tvo�en klientsk� skript.</param>
		public virtual void GetClientSideScriptFunction(ScriptBuilder scriptBuilder)
		{
			// na�teme klientsk� script
			string clientScript = GetSubstitutedScript();
			// pod�v�me se, zda ji� byl registrov�n
			string functionName = GetFunctionNameFromCache(clientScript);

			if (functionName == null)
			{
				// pokud tento skript je�t� nebyl registrov�n...				
				CreateFunctionName(null);
				// ulo��me jej do cache
				AddFunctionToCache(clientSideScriptFunctionName, clientScript);
				// a zaregistrujeme
				scriptBuilder.Append(WrapClientSideScriptToFunction(clientScript));
			}
			else
			{
				// pokud tento skript ji� byl registrov�n,
				// pou�ijeme tento registrovan� skript
				clientSideScriptFunctionName = functionName;
			}
		}
		#endregion
		
		#region CreateFunctionName
		/// <summary>
		/// Vytvo�� n�zev klientsk� funkce. Pokud je parametr reuseFunction pr�zdn�,
		/// vytvo�� nov� n�zev, jinak pou�ije hodnotu tohoto parametru.
		/// </summary>
		/// <param name="reuseFunctionName"></param>
		protected virtual void CreateFunctionName(string reuseFunctionName)
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

		#region GetClientSideStartupScript
		/// <summary>
		/// Registruje spu�t�n� klientsk�ho skriptu p�i na�t�n� str�nky.
		/// </summary>
		/// <param name="scriptBuilder">ScriptBuilder, do kter�ho je tvo�en klientsk� skript.</param>
		public virtual void GetClientSideStartupScript(ScriptBuilder scriptBuilder)
		{
			bool startOnLoad;

			ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
			if ((scriptManager != null) && scriptManager.IsInAsyncPostBack)
			{
				startOnLoad = this.StartOnAjaxCallback;
			}
			else
			{
				startOnLoad = this.StartOnLoad;
			}

			if (startOnLoad)
			{
				scriptBuilder.AppendLine(Scriptlet.ClientSideScriptFunctionCall);
			}
		}
		
		#endregion
		
		#region GetSubstitutedScript
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

		#region ClientScriptSubstituingEventArgs
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

		#region Functions cache
		
		#region FunctionCache
		/// <summary>
		/// Cache pro klientsk� skripty. Kl��em je skript, hodnotou je n�zev funkce,
		/// ve kter� je skript registrov�n.
		/// Cache je ulo�ena v HttpContextu.
		/// </summary>
		protected virtual Dictionary<string, string> FunctionCache
		{
			get
			{
				Dictionary<string, string> result = (Dictionary<string, string>)HttpContext.Current.Items[typeof(ClientScript)];

				if (result == null)
				{
					// pokud cache je�t� nen�, vytvo��me ji a vr�t�me
					// ��dn� z�mky (lock { ... }) nejsou pot�eba, jsme st�le v jednom HttpContextu
					result = new Dictionary<string, string>();
					HttpContext.Current.Items[typeof(ClientScript)] = result;
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
		/// <param name="clientScript">Klientsk� skript.</param>
		protected virtual void AddFunctionToCache(string functionName, string clientScript)
		{
			FunctionCache.Add(clientScript, functionName);
		}		
		#endregion

		#region GetFunctionNameFromCache
		/// <summary>
		/// Nalezne n�zev funkce, ve kter� je klientsk� skript registrov�n.
		/// </summary>
		/// <param name="clientScript">Klientsk� skript.</param>
		/// <returns>Nalezne n�zev funkce, ve kter� je klientsk� skript 
		/// registrov�n. Pokud skript nen� registrov�n, vr�t� null.</returns>
		protected virtual string GetFunctionNameFromCache(string clientScript)
		{
			string result;
			if (FunctionCache.TryGetValue(clientScript, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}
		#endregion
		
		#region WrapClientSideScriptToFunction
		/// <summary>
		/// Zabal� klientsk� skript do funkce.
		/// </summary>
		/// <param name="clientScript">N�zev funkce, kter� se tvo��.</param>
		/// <returns>Klientsk� skript jako funkce p�ipraven� k registraci do str�nky.</returns>
		protected string WrapClientSideScriptToFunction(string clientScript)
		{
			return String.Format("function {0}(parameters){2}{{{2}{1}{2}}}{2}", clientSideScriptFunctionName, clientScript, Environment.NewLine);
		}
		#endregion
		
		#endregion
	}
}
