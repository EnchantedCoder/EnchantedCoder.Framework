using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Collapser je ovl�dac� prvek, kter� zaji��uje funk�nost collapse/expand pro libovoln� jin� element str�nky.
	/// </summary>
	/// <example>
	/// Jednoduch� Collapser:
	/// <code>
	///	&lt;havit:Collapser
	///		Text="Prvn� mo�nost zad�n� textu"
	///		ContentElement="CollapseDiv"
	///		CssClass="normal"
	///		CssClassExpanded="expanded"
	///		CssClassCollapsed="collapsed"
	///		Runat="server"
	///	&gt;Text je mo�n� zadat i jako inner-text&lt;/havit:Collapser&gt;
	///	
	///	&lt;div id="CollapseDiv"&gt;
	///		Od:
	///		&lt;asp:TextBox ID="OdDateTB" Text="3.3.2004" Runat="server" /&gt;
	///	&lt;/div&gt;
	/// </code>
	/// </example>
	[ParseChildren(false)]
	public class Collapser : WebControl
	{
		#region Properties
		/// <summary>
		/// Text ovl�dac� prvku.
		/// </summary>
		public string Text
		{
			get
			{
				string tmp = (string)ViewState["Text"];
				if (tmp != null)
				{
					return tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// CssClass pro ovl�dac� prvek ve stavu expanded.
		/// </summary>
		/// <remarks>
		/// Pomoc� stylu m��eme nap��klad nastavit obr�zek pozad� na m�nus [-].
		/// </remarks>
		public string CssClassExpanded
		{
			get
			{
				string tmp = (string)ViewState["CssClassExpanded"];
				if (tmp != null)
				{
					return tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["CssClassExpanded"] = value;
			}
		}

		/// <summary>
		/// CssClass pro ovl�dac� prvek ve stavu collapsed.
		/// </summary>
		/// <remarks>
		/// Pomoc� stylu m��eme nap��klad nastavit obr�zek pozad� na m�nus [-].
		/// </remarks>
		public string CssClassCollapsed
		{
			get
			{
				string tmp = (string)ViewState["CssClassCollapsed"];
				if (tmp != null)
				{
					return tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["CssClassCollapsed"] = value;
			}
		}

		/// <summary>
		/// Odkaz (ID) na element, kter� m� b�t expandov�na/collapsov�na.<br/>
		/// </summary>
		/// <remarks>
		/// Nejprve se zkou��, jestli neexistuje control s t�mto ID.
		/// Pokud ano, pou�ije se jeho ClientID, pokud ne, pou�ije se p��mo ContentElement.
		/// </remarks>
		public string ContentElement
		{
			get
			{
				string tmp = (string)ViewState["ContentElement"];
				if (tmp != null)
				{
					return tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ContentElement"] = value;
			}
		}

		/// <summary>
		/// Indikuje, zda-li m� b�t <see cref="ContentElement"/> zobrazen sbalen�/rozbalen�.<br/>
		/// </summary>
		public bool Collapsed
		{
			get
			{
				object tmp = ViewState["Collapsed"];
				if (tmp != null)
				{
					return (bool)tmp;
				}
				return true;
			}
			set
			{
				ViewState["Collapsed"] = value;
			}
		}
		#endregion

		#region private properties
		/// <summary>
		/// �pln� CssClass pro stav Collapsed
		/// </summary>
		private string cssClassCollapsedFull
		{
			get
			{
				return (this.CssClass + " " + this.CssClassCollapsed).Trim();
			}
		}

		/// <summary>
		/// �pln� CssClass pro stav Expanded
		/// </summary>
		private string cssClassExpandedFull
		{
			get
			{
				return (this.CssClass + " " + this.CssClassExpanded).Trim();
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci controlu.
		/// </summary>
		public Collapser()
			: base(HtmlTextWriterTag.Span)
		{
		}
		#endregion

		#region AddParsedSubObject
		/// <summary>
		/// Zaji��uje pronesen� inner-textu controlu do property <see cref="Text"/>.
		/// </summary>
		/// <param name="obj"></param>
		protected override void AddParsedSubObject(object obj)
		{
			if (!(obj is LiteralControl))
			{
				throw new HttpException("Potomek mus� b�t Literal.");
			}
			this.Text = ((LiteralControl)obj).Text;
		}
		#endregion

		#region OnPreRender
		/// <summary>
		/// Vol�no p�ed renderov�n�m.
		/// </summary>
		protected override void OnPreRender(EventArgs e)
		{
			if (this.Enabled)
			{
				RegisterClientScript();
			}
			base.OnPreRender(e);
		}
		#endregion

		#region AddAttributesToRender
		/// <summary>
		/// Dopln� Attributes o hodnoty z properties.
		/// </summary>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			// nastaven� styl�
			if (this.Collapsed)
			{
				this.ControlStyle.CssClass = this.cssClassCollapsedFull;
			}
			else
			{
				this.ControlStyle.CssClass = this.cssClassExpandedFull;
			}

			// zaji�t�n� povinn�ho renderov�n� atributu ID
			if (this.ID == null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
			}
			base.AddAttributesToRender(writer);
		}
		#endregion

		#region RenderContents
		/// <summary>
		/// Renderuje obsahu elementu.
		/// </summary>
		protected override void RenderContents(HtmlTextWriter writer)
		{
			writer.Write(this.Text);
		}
		#endregion

		#region RegisterClientScript
		private void RegisterClientScript()
		{
			const string clientScriptKey = "Havit.Web.UI.WebControls.Collapser";
			const string toggleCollapser =
					  "<script type=\"text/javascript\" language=\"JScript\">\n"
					  + "function toggleCollapser(collapserElementId, contentElementId, cssClassCollapsed, cssClassExpanded)\n"
					  + "{\n"
					  + "\tvar collapser = document.getElementById(collapserElementId);\n"
					  + "\tvar content = document.getElementById(contentElementId);\n"
					  + "\tif (content.style.display != 'none')\n"
					  + "\t{\n"
					  + "\t\tcontent.setAttribute('previousDisplayStyle', content.style.display);\n"
					  + "\t\tcontent.style.display = 'none';\n"
					  + "\t\tcollapser.className = cssClassCollapsed;\n"
					  + "\t}\n"
					  + "\telse\n"
					  + "\t{\n"
					  + "\t\tcontent.style.display = content.getAttribute('previousDisplayStyle');\n"
					  + "\t\tcollapser.className = cssClassExpanded;\n"
					  + "\t}\n"
					  + "}\n"
					  + "</script>\n";

			if (!Page.IsClientScriptBlockRegistered(clientScriptKey))
			{
				Page.RegisterClientScriptBlock(clientScriptKey, toggleCollapser);
			}

			string toggleCall = String.Format("toggleCollapser('{0}','{1}','{2}','{3}');",
				this.ClientID,
				ResolveID(this.ContentElement),
				this.cssClassCollapsedFull,
				this.cssClassExpandedFull);

			this.Attributes.Add("onClick", toggleCall);

			if (this.Collapsed)
			{
				Page.RegisterStartupScript(clientScriptKey + ResolveID(this.ContentElement),  // zajist� jedin� vol�n� pro element
					"<script type=\"text/javascript\" language=\"JScript\">" + toggleCall + "</script>\n");
			}
		}
		#endregion

		#region ResolveID
		/// <summary>
		/// Pokud ID pat�� controlu, pak vr�t� jeho ClientID, jinak vr�t� zp�t p�vodn� ID.
		/// </summary>
		/// <param name="id">ID k resolvov�n�</param>
		/// <returns>c�lov� ID</returns>
		protected virtual string ResolveID(string id)
		{
			Control ctrl = this.NamingContainer.FindControl(id);
			if (ctrl != null)
			{
				return ctrl.ClientID;
			}
			return id;
		}
		#endregion
	}
}
