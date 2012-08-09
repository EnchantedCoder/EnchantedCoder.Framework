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
    [ToolboxData("<{0}:Scriptlet runat=\"server\"><{0}:ClientScript runat=\"server\"></{0}:ClientScript></{0}:Scriptlet>")]
    [ControlBuilder(typeof(NoLiteralContolBuilder))]
    public class Scriptlet : Control
    {
		/// <summary>
		/// Vytvo�� instanci scriptletu a nastav� v�choz� hodnoty (ControlExtenderRepository a ScriptSubstitution).
		/// </summary>
		public Scriptlet()
		{
            // vezmeme si v�choz� repository
			controlExtenderRepository = Havit.Web.UI.Scriptlets.ControlExtenderRepository.Default;
            scriptSubstitution = ScriptSubstitutionRepository.Default;
		}

        /// <summary>
        /// Vrac� n�zev kliensk�ho objektu, kter� je parametrem vol�n� kliensk� metody 
        /// generovan� v ClientScriptu.
        /// </summary>
        protected virtual string ClientSideObjectIdentifier
        {
            get
            {
                return "scriptletobject" + this.ClientID;
            }
        }

        /// <summary>
        /// N�zev funkce vygenerovan� ClientScriptem. Dostupn� a� po vygenerov�n� skriptu.
        /// Pokud nen� funkce generov�na opakovan� (v repeateru, apod.) vrac� n�zev sd�len�
        /// funkce.
        /// </summary>
        protected virtual string ClientSideFunctionName
        {
            get { return clientScript.ClientSideFunctionName; }
        }

        /// <summary>
        /// Vrac� klientsk� skript pro vol�n� klientsk� funkce s klientsk�m parametrem.
        /// </summary>
        public string ClientSideFunctionCall
        {
            get
            {
                return String.Format("{0}({1});", ClientSideFunctionName, ClientSideObjectIdentifier);
            }
        }

		ClientScript clientScript = null;
        List<IScriptletParameter> scriptletParameters = new List<IScriptletParameter>();

		private IControlExtenderRepository controlExtenderRepository;

        /// <summary>
        /// Vrac� nebo nastavuje repository extender� pro parametry.
        /// </summary>
		public IControlExtenderRepository ControlExtenderRepository
		{
			get { return controlExtenderRepository; }
			set { controlExtenderRepository = value; }
		}

        private IScriptSubstitution scriptSubstitution;
        /// <summary>
        /// Vrac� nebo nastavuje substituci pou�itou pro tvorbu kliensk�ho skriptu.
        /// </summary>
        public IScriptSubstitution ScriptSubstitution
        {
            get { return scriptSubstitution; }
            set { scriptSubstitution = value; }
        }

        /// <summary>
        /// Zajist� tvorbu klienstk�ho skriptu.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!DesignMode)
            {
                // vytvo��me klientsk� skript
                string clientScript = PrepareClientScript();

                // zaregistrujeme jej na konec str�nky, aby byly controly ji� dostupn�
                Page.ClientScript.RegisterStartupScript(
                    typeof(Scriptlet),
                    this.ClientID,
                    clientScript,
                    true
                );
            }
        }

        /// <summary>
        /// Sestav� kompletn� klientsk� skript seskl�d�n�m funkce, vytvo�en� objektu 
        /// a jeho parametr�.
        /// </summary>
        /// <returns>Kompletn� klientsk� skript.</returns>
        protected virtual string PrepareClientScript()
        {
			ScriptBuilder builder = new ScriptBuilder();

            CreateClientSideFunction(builder);
            CreateClientSideObject(builder);
			CreateClientSideParameters(builder);

            return builder.ToString();
        }

		/// <summary>
		/// Zajist�, aby se na scriptletu nepou�ilo klasick� renderov�n�.
		/// M�sto renderov�n� se registruj� klientsk� skripty v metod� OnPreRender.
		/// </summary>
		/// <param name="writer"></param>
        public override void RenderControl(HtmlTextWriter writer)
		{
			// nebudeme renderovat nic z vnit�ku controlu
		}

        /// <summary>
        /// Vytvo�� klientskou funkci z objektu typu ClientScript.
        /// </summary>
        /// <param name="builder">Script builder.</param>
        protected virtual void CreateClientSideFunction(ScriptBuilder builder)
        {
            if (clientScript != null)
                clientScript.CreateClientSideScript(builder);
            else
                throw new ArgumentException("ClientScript nebyl zad�n.");
        }

        /// <summary>
        /// Vytvo�� skript pro objekt na klientsk� stran�.
        /// </summary>
        /// <param name="builder">Script builder.</param>
        protected virtual void CreateClientSideObject(ScriptBuilder builder)
        {
			builder.AppendFormat("var {0} = new Object();\n", ClientSideObjectIdentifier);
        }

        /// <summary>
        /// Vytvo�� parametry klintsk�ho objektu.
        /// </summary>
        /// <param name="builder">Script builder.</param>
        protected virtual void CreateClientSideParameters(ScriptBuilder builder)
		{
            foreach (IScriptletParameter scriptletParameter in scriptletParameters)
			{
                scriptletParameter.CheckProperties();
                scriptletParameter.CreateParameter(ClientSideObjectIdentifier, this.NamingContainer, builder);
			}
        }

		/// <summary>
		/// Zavol�no, kdy� je do kolekce Controls p�id�n Control.
		/// Zaji��uje, aby nebyl p�id�n control neimplementuj�c� 
		/// IScriptletParameter nebo ClientScript.
		/// Z�rove� zajist�, aby nebyl p�id�n v�ce ne� jeden ClientScript.
		/// </summary>
		/// <param name="control">P�id�van� control.</param>
		/// <param name="index">Pozice v kolekci control�, kam je control p�id�v�n.</param>
		protected override void AddedControl(Control control, int index)
        {
            base.AddedControl(control, index);

            // zajist�me, aby n�m do scriptletu nep�i�el nezn�m� control
            if (!(control is ScriptletNestedControl))
                throw new ArgumentException(String.Format("Do Scriptletu je vkl�d�n nepodporovan� control {0}.", control.ID));

            if (control is ClientScript)
            {
                if (clientScript != null)
                    throw new ArgumentException("Scriptlet mus� obsahovat ClientScript pr�v� jednou.");

                clientScript = (ClientScript)control;
            }

            if (control is IScriptletParameter)
                scriptletParameters.Add((IScriptletParameter)control);
        }
    }
}
