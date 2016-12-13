﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Havit.Diagnostics.Contracts;
using Havit.Web.UI.ClientScripts;
using Havit.Web.UI.Scriptlets;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Auto complete textbox.
	/// Používá komponentu Devbridge jQuery-Autocomplete (https://github.com/devbridge/jQuery-Autocomplete)
	/// </summary>
	[ValidationProperty("SelectedValue")]
	[DefaultProperty("SelectedValue")]
	[ToolboxData("<{0}:AutoCompleteTextBox runat=server></{0}:AutoCompleteTextBox>")]
	[Themeable(true)]
	public class AutoCompleteTextBox : Control, INamingContainer, IPostBackDataHandler
	{
		#region OnValueChanged
		/// <summary>
		/// Událost nastane v okamžiku změny vybrané položky.
		/// </summary>
		public event EventHandler SelectedValueChanged;

		/// <summary>
		/// Raises theSelectedValueChanged event.
		/// </summary>
		protected void OnSelectedValueChanged(EventArgs e)
		{
			if (SelectedValueChanged != null)
			{
				SelectedValueChanged(this, e);
			}
		}
		#endregion

		#region Fields (private)
		private readonly TextBox valueTextBox;
		private readonly HiddenField valueHiddenField;
		#endregion

		#region AutoPostBack
		/// <summary>
		/// Udává, zda má po změně hodnoty V ui dojít k postbacku.
		/// </summary>
		public bool AutoPostBack
		{
			get { return (bool?)ViewState["AutoPostBack"] ?? false; }
			set { ViewState["AutoPostBack"] = value; }
		}
		#endregion

		#region ServiceUrl
		/// <summary>
		/// URL Služby poskytující data.
		/// </summary>
		public string ServiceUrl
		{
			get { return (string)ViewState["ServiceUrl"]; }
			set { ViewState["ServiceUrl"] = value; }
		}
		#endregion

		#region Context
		/// <summary>
		/// Kontext prvku. Předává se službě v parametru "context".
		/// </summary>
		public new string Context
		{
			get
			{
				return (string)ViewState["Context"];
			}
			set
			{
				ViewState["Context"] = value;
			}
		}
		#endregion

		#region MinSuggestedChars
		/// <summary>
		/// Minimální počet znaků při kterých se nabídnou položky. Default = 1
		/// </summary>
		public int MinSuggestedChars
		{
			get { return (int?)ViewState["MinSuggestedChars"] ?? 1; }
			set { ViewState["MinSuggestedChars"] = value; }
		}
		#endregion

		#region UseClientCache
		/// <summary>
		/// Udává, zda se používá klientská cache. Default = false.
		/// </summary>
		public bool UseClientCache
		{
			get { return (bool?)ViewState["UseClientCache"] ?? false; }
			set { ViewState["UseClientCache"] = value; }
		}
		#endregion

		#region MaxHeight
		/// <summary>
		/// Maximální výška našeptávače v px.
		/// </summary>
		public int MaxHeight
		{
			get { return (int?)ViewState["MaxHeight"] ?? 300; }
			set { ViewState["MaxHeight"] = value; }
		}
		#endregion

		#region DeferRequest
		/// <summary>
		/// Čas v milsekundách o který se zpozdí dotaz do služby.
		/// </summary>
		public int DeferRequest
		{
			get { return (int?)ViewState["DeferRequest"] ?? 0; }
			set { ViewState["DeferRequest"] = value; }
		}
		#endregion

		#region SelectedText
		/// <summary>
		/// Vyplněný text
		/// </summary>
		public string SelectedText
		{
			get { return valueTextBox.Text; }
			set { valueTextBox.Text = value; }
		}
		#endregion

		#region SelectedValue
		/// <summary>
		/// Vyplněná hodnota
		/// </summary>
		public string SelectedValue
		{
			get
			{
				return (string)ViewState["SelectedValue"] ?? String.Empty;
			}
			set
			{
				ViewState["SelectedValue"] = value;
			}
		}
		#endregion

		#region TextBoxStyle
		/// <summary>
		/// Styly vnitřního textboxu.
		/// </summary>
		public Style TextBoxStyle
		{
			get { return valueTextBox.ControlStyle; }
		}
		#endregion

		#region TabIndex
		/// <summary>
		/// Gets or sets the tab order of the control within its container.
		/// </summary>
		public short TabIndex
		{
			get
			{
				return valueTextBox.TabIndex;
			}
			set
			{
				valueTextBox.TabIndex = value;
			}
		}
		#endregion

		#region ToolTip
		/// <summary>
		/// Gets or sets the text displayed when the mouse pointer hovers over the Web server control.
		/// </summary>
		public string ToolTip
		{
			get
			{
				return valueTextBox.ToolTip;
			}
			set
			{
				valueTextBox.ToolTip = value;
			}
		}
		#endregion

		#region PlaceHolderText
		/// <summary>
		/// Set text, which is shown before text is entered
		/// </summary>
		public string PlaceHolderText
		{
			get { return valueTextBox.Attributes["placeholder"]; }
			set { valueTextBox.Attributes["placeholder"] = value; }
		}
		#endregion

		#region Enabled
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="AutoCompleteTextBox"/> is enabled.
		/// </summary>
		public bool Enabled
		{
			get
			{
				return valueTextBox.Enabled;
			}
			set
			{
				valueTextBox.Enabled = value;
			}
		}
		#endregion

		#region Orientation
		/// <summary>
		/// Orientace našeptávacího dialogu. Default = bottom
		/// </summary>
		public AutoCompleteTextBoxOrientation Orientation
		{
			get
			{
				return (AutoCompleteTextBoxOrientation?)ViewState["Orientation"] ?? AutoCompleteTextBoxOrientation.Bottom;
			}
			set
			{
				ViewState["Orientation"] = value;
			}
		}
		#endregion

		#region AllowInvalidSelection
		/// <summary>
		/// Pokud je hodnota false (default), při opuštění textového pole, se nevalidní hodnota vymaže.
		/// Pokud je hodnota true, nevymaže se v textovém poli nevalidní hodnota při opuštění editace. Určeno pro možnost zakládání nových položek.
		/// </summary>
		public bool AllowInvalidSelection
		{
			get
			{
				return (bool?)ViewState["AllowInvalidSelection"] ?? false;
			}
			set
			{
				ViewState["AllowInvalidSelection"] = value;
			}
		}
		#endregion

		/// <summary>
		/// Pokud je hodnota false (default), tak prázdná hodnota v textovém poli není považována za validní
		/// Pokud je hodnota true, tak je prázdná hodnota považována za validní
		/// </summary>
		public bool Nullable
		{
			get
			{
				return (bool?)ViewState["Nullable"] ?? false;
			}
			set
			{
				ViewState["Nullable"] = value;
			}
		}

		#region ShowNoSuggestionNotice
		/// <summary>
		/// Udává, zda se má zobrazit v nabídkovém pruhu informace o prázdné nabídce. (nejsou nabídnuty žádné položky)
		/// </summary>
		public bool ShowNoSuggestionNotice
		{
			get
			{
				return (bool?)ViewState["ShowNoSuggestionNotice"] ?? false;
			}
			set
			{
				ViewState["ShowNoSuggestionNotice"] = value;
			}
		}
		#endregion

		#region NoSuggestionNotice
		/// <summary>
		/// Informace, která se zobrazí v nabídkovém pruhu v případě, že žádná položka neodpovídá zadanému řetězci. (nejsou nabídnuty žádné položky)
		/// </summary>
		public string NoSuggestionNotice
		{
			get
			{
				return (string)ViewState["NoSuggestionNotice"];
			}
			set
			{
				ViewState["NoSuggestionNotice"] = value;
			}
		}
		#endregion

		#region ValidationGroup
		/// <summary>
		/// ValidationGroup pro validaci.
		/// </summary>
		public string ValidationGroup
		{
			get
			{
				return valueTextBox.ValidationGroup;
			}
			set
			{
				valueTextBox.ValidationGroup = value;
			}
		}
		#endregion

		#region CausesValidation
		/// <summary>
		/// Určuje, zda dochází k validaci při postbacku způsobeným tímto controlem (autopostback).
		/// </summary>
		public bool CausesValidation
		{
			get
			{
				return valueTextBox.CausesValidation;
			}
			set
			{
				valueTextBox.CausesValidation = value;
			}
		}
		#endregion

		#region OnClientSelectScript
		/// <summary>
		/// Skript vyvolaný v případě výběru hodnoty. Pokud script vrátí false, potom neproběhne AutoPostBack.
		/// </summary>
		public string OnClientSelectScript
		{
			get
			{
				return (string)(ViewState["OnClientSelectScript"] ?? String.Empty);
			}
			set
			{
				ViewState["OnClientSelectScript"] = value;
			}
		}
		#endregion

		#region ClientID (override)
		/// <summary>
		/// ClientID (overriden).
		/// Vrací ClientID obsaženého TextBoxu pro zadávání hodnoty.
		/// To řeší klientské validátory, které natrvdo předpokládají, že validovaný control (podle ClientID)
		/// obsahuje klientskou vlastnost "value". Tímto klientskému validátoru místo AutoCompleteTextBoxu podstrčíme nested TextBox.
		/// </summary>
		public override string ClientID
		{
			get
			{
				return valueTextBox.ClientID;
			}
		}

		/// <summary>
		/// ClientID hiddenfieldu, který obsahuje získanou hodnotu.
		/// </summary>
		public string ValueHiddenFieldClientID
		{
			get
			{
				return valueHiddenField.ClientID;
			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoCompleteTextBox"/> class.
		/// </summary>
		public AutoCompleteTextBox()
		{
			valueTextBox = new TextBox();
			valueTextBox.ID = "ValueTextBox";

			valueHiddenField = new HiddenField();
			valueHiddenField.ID = "ValueHiddenField";
			valueHiddenField.EnableViewState = false;
		}
		#endregion

		#region ClearSelection
		/// <summary>
		/// Vyčistí vybranou hodnotu.
		/// </summary>
		public void ClearSelection()
		{
			SelectedText = String.Empty;
			SelectedValue = String.Empty;
		}
		#endregion

		#region CreateChildControls (protected)
		/// <summary>
		/// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();

			Controls.Add(valueTextBox);
			Controls.Add(valueHiddenField);
		}
		#endregion

		#region RenderControl
		/// <summary>
		/// Outputs server control content to a provided <see cref="T:System.Web.UI.HtmlTextWriter" /> object and stores tracing information about the control if tracing is enabled.
		/// </summary>
		/// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> object that receives the control content.</param>
		public override void RenderControl(HtmlTextWriter writer)
		{
			writer.AddAttribute("class", "autocomplete-textbox");

			writer.AddAttribute("data-autocompletetextbox", null);
			writer.AddAttribute("data-serviceurl", ResolveClientUrl(ServiceUrl));
			writer.AddAttribute("data-minchars", MinSuggestedChars.ToString());
			writer.AddAttribute("data-deferRequest", DeferRequest.ToString());
			writer.AddAttribute("data-maxheight", MaxHeight.ToString());
			writer.AddAttribute("data-orientation", Orientation.ToString());
			writer.AddAttribute("data-allowInvalidSelection", AllowInvalidSelection.ToString());
			writer.AddAttribute("data-nullable", Nullable.ToString());
			writer.AddAttribute("data-showNoSuggestionNotice", ShowNoSuggestionNotice.ToString());
			writer.AddAttribute("data-selectedvalue", SelectedText);

			if (ShowNoSuggestionNotice)
			{
				writer.AddAttribute("data-noSuggestionNotice", NoSuggestionNotice);
			}

			if (!String.IsNullOrEmpty(OnClientSelectScript))
			{
				writer.AddAttribute("data-onselectscript", OnClientSelectScript);
			}

			if (!UseClientCache)
			{
				writer.AddAttribute("data-nocache", null);
			}
			if (!String.IsNullOrWhiteSpace(Context))
			{
				writer.AddAttribute("data-params", "{\"context\": \"" + Context + "\", \"query\": \"\"}");
			}

			if (AutoPostBack)
			{
				writer.AddAttribute("data-postbackscript", Page.ClientScript.GetPostBackEventReference(this, null));
			}
			writer.RenderBeginTag(HtmlTextWriterTag.Span);
			base.RenderControl(writer);
			writer.RenderEndTag();
		}
		#endregion

		#region OnInit
		/// <summary>
		/// Raises the <see cref="E:Init" /> event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			EnsureChildControls();
		}
		#endregion

		#region OnPreRender
		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.RegisterRequiresPostBack(this);

			valueHiddenField.Value = SelectedValue;

			HavitFrameworkClientScriptHelper.RegisterHavitFrameworkClientScript(this.Page);
			ScriptManager.ScriptResourceMapping.EnsureScriptRegistration(this.Page, HavitFrameworkClientScriptHelper.JQueryAutoCompleteResourceMappingName);
			ScriptManager.RegisterStartupScript(this, typeof(AutoCompleteTextBox), "InitScript", "havitAutoCompleteTextBoxExtensions.init();", true);
		}
		#endregion

		#region LoadPostData
		/// <summary>
		/// Loads the post data.
		/// </summary>
		/// <param name="postDataKey">The post data key.</param>
		/// <param name="postCollection">The post collection.</param>
		public virtual bool LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			string postedValue = postCollection[valueHiddenField.UniqueID];
			if (!SelectedValue.Equals(postedValue))
			{
				SelectedValue = postedValue;
				return true;
			}

			return false;
		}
		#endregion

		#region RaisePostDataChangedEvent
		/// <summary>
		/// When implemented by a class, signals the server control to notify the ASP.NET application that the state of the control has changed.
		/// </summary>
		public virtual void RaisePostDataChangedEvent()
		{
			OnSelectedValueChanged(EventArgs.Empty);
		}
		#endregion

		#region RegisterClientScripts (static)
		/// <summary>
		/// Zaregistruje klienské scripty, které control vyžaduje. Nutné volat před přidáním prvku do stránky v asynchronním volání.
		/// </summary>
		public static void RegisterClientScripts(Page page)
		{
			ScriptManager.ScriptResourceMapping.EnsureScriptRegistration(page, HavitFrameworkClientScriptHelper.JQueryAutoCompleteResourceMappingName);
		}
		#endregion
	}
}