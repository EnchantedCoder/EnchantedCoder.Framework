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

		/* Parametry ClientScriptu *************** */

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
		/// Nem� vliv na zpracov�n� str�nky p�i asynchronn�m postbacku (callbacku). K tomu slou�� vlastnost <see cref="StartOnAjaxCallback">StartOnAjaxCallback</see>.
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

		#region GetClientSideFunctionCode
		/// <summary>
		/// Vr�t� k�d pro hlavn� funkci skriptletu.
		/// </summary>
		public virtual string GetClientSideFunctionCode()
		{
			return GetSubstitutedScript();
		}
		#endregion
		
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
		/// <summary>
		/// Obslou�� ud�lost ClientScriptSubstituing.
		/// </summary>
		protected virtual void OnClientScriptSubstituing(ClientScriptSubstituingEventArgs eventArgs)
		{
			if (_clientScriptSubstituing != null)
			{
				_clientScriptSubstituing.Invoke(this, eventArgs);
			}
		}
		#endregion

		#region ClientScriptSubstituing
		/// <summary>
		/// Ud�lost pro proveden� substituce v klietsk�m skriptu.
		/// </summary>
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

	}
}
