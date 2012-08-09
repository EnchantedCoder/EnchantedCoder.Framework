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
	/// Scriptlet umo��uje snadnou tvorbu klientsk�ch skript�.
	/// </summary>
	[ControlBuilder(typeof(NoLiteralContolBuilder))]	
	public class Scriptlet : Control
	{
		#region Private fields
		private ClientScript clientScript = null;
		private List<IScriptletParameter> scriptletParameters = new List<IScriptletParameter>();
		#endregion

		#region ClientSideGetDataObjectFunctionName
		/// <summary>
		/// Vrac� skrit vol�n� kliensk� metody vracej�c� objekt nesouc� parametry scriptletu.
		/// </summary>
		protected virtual string ClientSideGetDataObjectFunctionName
		{
			get
			{
				return "scriptletGetDataObject_" + this.ClientID;
			}
		}		
		#endregion

		#region ClientSideGetDataObjectFunctionCall
		/// <summary>
		/// Vrac� skrit vol�n� kliensk� metody vracej�c� objekt nesouc� parametry scriptletu.
		/// </summary>
		protected virtual string ClientSideGetDataObjectFunctionCall
		{
			get
			{
				return ClientSideGetDataObjectFunctionName + "()";
			}
		}
		#endregion

		#region ClientSideScriptFunctionName
		///<summary>
		///N�zev funkce vygenerovan� ClientScriptem. Dostupn� a� po vygenerov�n� skriptu.
		///Pokud nen� funkce generov�na opakovan� (v repeateru, apod.) vrac� n�zev sd�len�
		///funkce.
		///</summary>
		protected virtual string ClientSideScriptFunctionName
		{
			get { return clientScript.ClientSideScriptFunctionName; }
		}
		#endregion

		#region ClientSideScriptFunctionCall
		/// <summary>
		/// Vrac� klientsk� skript pro vol�n� klientsk� funkce s klientsk�m parametrem.
		/// </summary>
		public string ClientSideScriptFunctionCall
		{
			get
			{
				return String.Format("{0}({1});", ClientSideScriptFunctionName, ClientSideGetDataObjectFunctionCall);
			}
		}
		#endregion
		
		#region ControlExtenderRepository
		/// <summary>
		/// Vrac� nebo nastavuje repository extender� pro parametry.
		/// </summary>
		public IControlExtenderRepository ControlExtenderRepository
		{
			get { return controlExtenderRepository; }
			set { controlExtenderRepository = value; }
		}
		private IControlExtenderRepository controlExtenderRepository;
		
		#endregion

		#region ScriptSubstitution
		/// <summary>
		/// Vrac� nebo nastavuje substituci pou�itou pro tvorbu kliensk�ho skriptu.
		/// </summary>
		public IScriptSubstitution ScriptSubstitution
		{
			get { return scriptSubstitution; }
			set { scriptSubstitution = value; }
		}
		private IScriptSubstitution scriptSubstitution;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci scriptletu a nastav� v�choz� hodnoty
		/// <see cref="ControlExtenderRepository">ControlExtenderRepository</see>
		/// (na <see cref="ControlExtenderRepository.Default">ControlExtenderRepository.Default</see>)
		/// a <see cref="ScriptSubstitution">ScriptSubstitution</see>
		/// (na <see cref="ScriptSubstitutionRepository.Default">ScriptSubstitutionRepository.Default</see>).
		/// </summary>
		public Scriptlet()
		{
			// vezmeme si v�choz� repository
			controlExtenderRepository = Havit.Web.UI.Scriptlets.ControlExtenderRepository.Default;
			scriptSubstitution = ScriptSubstitutionRepository.Default;
		}

		#endregion

		#region AddedControl
		/// <summary>
		/// Zavol�no, kdy� je do kolekce Controls p�id�n Control.
		/// Zaji��uje, aby nebyl p�id�n control neimplementuj�c� 
		/// <see cref="IScriptletParameter">IScriptletParameter</see>
		/// nebo <see cref="ClientScript">ClientScript</see>.
		/// Z�rove� zajist�, aby nebyl p�id�n v�ce ne� jeden <see cref="ClientScript">ClientScript</see>.
		/// </summary>
		/// <param name="control">P�id�van� control.</param>
		/// <param name="index">Pozice v kolekci control�, kam je control p�id�v�n.</param>
		protected override void AddedControl(Control control, int index)
		{
			base.AddedControl(control, index);

			// zajist�me, aby n�m do scriptletu nep�i�el nezn�m� control
			if (!(control is ScriptletNestedControl))
			{
				throw new ArgumentException(String.Format("Do Scriptletu je vkl�d�n nepodporovan� control {0}.", control.ID));
			}

			if (control is ClientScript)
			{
				if (clientScript != null)
				{
					throw new ArgumentException("Scriptlet mus� obsahovat ClientScript pr�v� jednou.");
				}

				clientScript = (ClientScript)control;
			}

			if (control is IScriptletParameter)
			{
				scriptletParameters.Add((IScriptletParameter)control);
			}
		}
		#endregion

		#region OnPreRender
		/// <summary>
		/// Zajist� tvorbu klienstk�ho skriptu.
		/// </summary>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			CheckControlConditions();
			PrepareAndRegisterClientScript();
		}
		#endregion
		
		#region CheckControlConditions
		/// <summary>
		/// Ov���, zda jsou spr�vn� zad�ny parametry scriptletu (testuje, zda byl zad�n ClientScript).
		/// </summary>
		protected virtual void CheckControlConditions()
		{
			if (clientScript == null)
			{
				throw new HttpException("ClientScript nebyl zad�n.");
			}
		}
		#endregion
		
		#region RenderControl
		///// <summary>
		///// Zajist� tvorbu klienstk�ho skriptu.
		///// </summary>
		//protected override void Render(HtmlTextWriter writer)
		//{
		//    base.Render(writer);

		//    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
		//    if ((scriptManager != null) && scriptManager.IsInAsyncPostBack)
		//    {
		//        PrepareAndRegisterClientScript();
		//    }
		//}

		/// <summary>
		/// Zajist�, aby se na scriptletu nepou�ilo klasick� renderov�n�.
		/// M�sto renderov�n� se registruj� klientsk� skripty v metod� OnPreRender.
		/// </summary>
		/// <param name="writer"></param>
		public override void RenderControl(HtmlTextWriter writer)
		{
			// nebudeme renderovat nic z vnit�ku controlu
		}		
		#endregion

		#region PrepareAndRegisterClientScript
		/// <summary>
		/// Sestav� kompletn� klientsk� skript seskl�d�n�m funkce, vytvo�en� objektu 
		/// a jeho parametr�. Zaregistruje skripty do str�nky
		/// </summary>
		/// <returns>Kompletn� klientsk� skript.</returns>
		protected virtual void PrepareAndRegisterClientScript()
		{
			if (DesignMode)
			{
				return;
			}

			ScriptBuilder builder = new ScriptBuilder();

			PrepareClientSideScripts(builder);

			// zaregistrujeme jej na konec str�nky, aby byly controly ji� dostupn�
			ScriptManager.RegisterStartupScript(
				this,
				typeof(Scriptlet),
				this.UniqueID,
				builder.ToString(),
				true
			);
		}
		#endregion
		
		#region PrepareClientSideScripts
		/// <summary>
		/// Vr�t� klientsk� skript scriptletu.
		/// </summary>
		/// <param name="builder">Script builder.</param>
		protected virtual void PrepareClientSideScripts(ScriptBuilder builder)
		{
			ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

			// nejd��ve mus�me vytvo�it funkci, abychom z�skali jm�no ob�lky
			// (nav�c m��e doj�t k reuse scriptu)
			
			// z�sk�me funkci reprezentuj�c� ClientScript	
			clientScript.GetClientSideScriptFunction(builder);

			// nyn� ji� se m��eme zeptat na jm�no klientsk� funkce se skriptem
			string attachEventsFunctionName = ClientSideScriptFunctionName + "_AttachEvents";
			string detachEventsFunctionName = ClientSideScriptFunctionName + "_DetachEvents";

			// vytvo��me funkci pro z�sk�n� objektu nesouc� parametry
			builder.AppendLineFormat("function {0}()", ClientSideGetDataObjectFunctionName);
			builder.AppendLine("{");
			builder.AppendLine("var result = new Object();");
			foreach (IScriptletParameter scriptletParameter in scriptletParameters)
			{
				scriptletParameter.CheckProperties();
				scriptletParameter.GetInitializeClientSideValueScript("result", this.NamingContainer, builder);
			}
			builder.AppendLine("return result;");
			builder.AppendLine("}");

			builder.AppendLineFormat("function {0}()", attachEventsFunctionName);
			builder.AppendLine("{");

			// pokud pou��v�me klientsk� ud�losti ASP.NET AJAXu, pot�ebujeme se odpojit od pageLoaded
			builder.AppendLine("if (!((typeof(Sys) == 'undefined') || (typeof(Sys.WebForms) == 'undefined')))");
			builder.AppendLine("{");
			builder.AppendLineFormat("Sys.WebForms.PageRequestManager.getInstance().remove_pageLoaded({0});", attachEventsFunctionName);
			builder.AppendLine("}");

			builder.AppendLineFormat("var data = {0};", ClientSideGetDataObjectFunctionCall);
			foreach (IScriptletParameter scriptletParameter in scriptletParameters)
			{
				scriptletParameter.GetAttachEventsScript("data", this.NamingContainer, builder);
			}
			
			builder.AppendLine("}");

			// pro neAJAXov� str�nky scriptlet nepot�ebuje odpojovat ud�losti
			if (scriptManager != null)
			{
				builder.AppendLineFormat("function {0}()", detachEventsFunctionName);
				builder.AppendLine("{");

				// pokud pou��v�me klientsk� ud�losti ASP.NET AJAXu, pot�ebujeme se odpojit od pageLoadingu
				builder.AppendLine("if (!((typeof(Sys) == 'undefined') || (typeof(Sys.WebForms) == 'undefined')))");
				builder.AppendLine("{");
				builder.AppendLineFormat("Sys.WebForms.PageRequestManager.getInstance().remove_pageLoading({0});", detachEventsFunctionName);
				builder.AppendLine("}");
				
				builder.AppendLineFormat("var data = {0};", ClientSideGetDataObjectFunctionCall);
				foreach (IScriptletParameter scriptletParameter in scriptletParameters)
				{
					scriptletParameter.GetDetachEventsScript("data", this.NamingContainer, builder);				
				}
				builder.AppendLine("}");
			}
		
			clientScript.GetClientSideStartupScript(builder);
						
			if (scriptManager == null)
			{
				builder.AppendLine(BrowserHelper.GetAttachEventScript("window", "onload", attachEventsFunctionName));
			}
			else
			{
				builder.AppendLine("if ((typeof(Sys) == 'undefined') || (typeof(Sys.WebForms) == 'undefined'))");
				builder.AppendLine("{");
				builder.AppendLine(BrowserHelper.GetAttachEventScript("window", "onload", attachEventsFunctionName));
				builder.AppendLine("}");
				builder.AppendLine("else");
				builder.AppendLine("{");
				//builder.AppendLine("if (typeof(document.scriptletEvents" + this.UniqueID + "Registered) == 'undefined')");
				//builder.AppendLine("{");				
				// pageLoading n�m zajist� odebr�n� ud�lost� je�t� p�ed v�m�nou element� v dokumentu
				builder.AppendLineFormat("Sys.WebForms.PageRequestManager.getInstance().add_pageLoading({0});", detachEventsFunctionName);
				// pageLoaded n�m zajist� nav�z�n� ud�lost� po v�m�n� element�
				builder.AppendLineFormat("Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded({0});", attachEventsFunctionName);
				builder.AppendLine("document.scriptletEvents" + this.UniqueID + "Registered = true;");				
				builder.AppendLine("}");
				//builder.AppendLine("}");
			}
		}
		#endregion
	}
}
