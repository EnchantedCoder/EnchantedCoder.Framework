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
	/// parametr� Scriptletu nebo p�i na�t�n� str�nky (pokud je povoleno).
	/// </summary>
    //[DefaultProperty("Script")]
    [ParseChildren(true, "Script")]
	public class ClientScript : ScriptletNestedControl
    {
	
		/// <summary>
		/// Klientsk� skript. Okolo skriptu je vytvo�ena ob�lka a p��padn� je spu�t�n p�i na�ten� str�nky.
		/// </summary>		
        [PersistenceMode(PersistenceMode.InnerDefaultProperty)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string Script
        {
            get { return (string)ViewState["Script"] ?? String.Empty; }
			set { ViewState["Script"] = value; }
        }

		/// <summary>
		/// Ud�v�, zda m� po na�ten� str�nky doj�t k automatick�mu spu�t�n� skriptu. V�choz� hodnota je <b>false</b>.
		/// </summary>
		public bool StartOnLoad
		{
			get { return (bool)(ViewState["StartOnLoad"] ?? false); }
			set { ViewState["StartOnLoad"] = value; }
		}
	
		private string clientSideFunctionName;

		/// <summary>
		/// N�zev funkce, pod jakou je klientsk� skript zaregistrov�n.
		/// Hodnota je dostupn� a� po zavol�n� metody RegisterClientScript.
		/// </summary>
		public string ClientSideFunctionName
		{
			get { return clientSideFunctionName; }
		}

		/// <summary>
		/// Metoda vytvo�� funkci okolo kliensk�ho skriptu a zaregistruje ji do str�nky.
		/// Je zaji�t�no, aby nebyla funkce se shodn�m obsahem registrov�na v�ckr�t
		/// (nap�. p�i pou�it� controlu v repeateru).
		/// </summary>
		/// <param name="scriptBuilder">ScriptBuilder, do kter�ho je tvo�en klientsk� skript.</param>
		public virtual void CreateClientSideScript(ScriptBuilder scriptBuilder)
        {
			// na�teme klientsk� script
			string clientScript = DoSubstitutions();
			// pod�v�me se, zda ji� byl registrov�n
			string functionName = GetFunctionNameFromCache(clientScript);

			if (functionName == null)
			{
				// pokud tento skript je�t� nebyl registrov�n...				
				CreateFunctionName(null);
				// ulo��me jej do cache
				AddFunctionToCache(clientSideFunctionName, clientScript);
				// a zaregistrujeme
				scriptBuilder.Append(WrapClientSideScriptToFunction(clientScript));
			}
			else
				// pokud tento skript ji� byl registrov�n,
				// pou�ijeme tento registrovan� skript
				clientSideFunctionName = functionName;

            // zaregistrujeme spusteni pri startu (je-li nastaveno);
            CreateClientSideStartOnLoadEvent(scriptBuilder);
        }

        /// <summary>
        /// Vytvo�� n�zev klientsk� funkce. Pokud je parametr reuseFunction pr�zdn�,
        /// vytvo�� nov� n�zev, jinak pou�ije hodnotu tohoto parametru.
        /// </summary>
        /// <param name="reuseFunctionName"></param>
        protected virtual void CreateFunctionName(string reuseFunctionName)
        {
            if (String.IsNullOrEmpty(reuseFunctionName))
                clientSideFunctionName = "scriptlet" + this.Parent.ClientID;
            else
                clientSideFunctionName = reuseFunctionName;
        }

		/// <summary>
		/// Registruje spu�t�n� klientsk�ho skriptu p�i na�t�n� str�nky.
		/// </summary>
		/// <param name="scriptBuilder">ScriptBuilder, do kter�ho je tvo�en klientsk� skript.</param>
		protected virtual void CreateClientSideStartOnLoadEvent(ScriptBuilder scriptBuilder)
		{
            if (StartOnLoad)
            {
                scriptBuilder.AppendFormat(BrowserHelper.GetAttachEvent("window", "onload", Scriptlet.ClientSideFunctionCall));
                scriptBuilder.Append("\n");
            }
		}

		/// <summary>
		/// Vr�t� p�ipraven� klientsk� skript. Provede nad skriptem substituce.
		/// </summary>
		/// <returns>Klientsk� skript p�ipraven� k vytvo�en� ob�lky a registraci.</returns>
		protected virtual string DoSubstitutions()
		{
			return Scriptlet.ScriptSubstitution.Substitute(Script);
		}

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

		/// <summary>
		/// P�id� klientsk� skript do cache.
		/// </summary>
		/// <param name="functionName">N�zev funkce, ve kter� je skript registrov�n.</param>
		/// <param name="clientScript">Klientsk� skript.</param>
		protected virtual void AddFunctionToCache(string functionName, string clientScript)
		{
			FunctionCache.Add(clientScript, functionName);
		}

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
				return result;
			else
				return null;
		}

		/// <summary>
		/// Zabal� klientsk� skript do funkce.
		/// </summary>
		/// <param name="clientScript">N�zev funkce, kter� se tvo��.</param>
		/// <returns>Klientsk� skript jako funkce p�ipraven� k registraci do str�nky.</returns>
		protected string WrapClientSideScriptToFunction(string clientScript)
		{
			return String.Format("function {0}(parameters) {{\n{1}\n}}\n", clientSideFunctionName, clientScript);
		}
    }
}
