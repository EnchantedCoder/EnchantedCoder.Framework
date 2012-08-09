using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Threading;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// NumericBox slou�� k zad�n� ��sla.
	/// </summary>
	[Themeable(true)]
	[ValidationProperty("NumberText")]	
	public class NumericBox : Control, INamingContainer
	{
		#region Constants
		private const string clientScriptBlockName = "Havit.DsvCommerce.WebBase.UI.WebControls.NumericBox_Script";
		private readonly Regex whitespaceremover = new Regex("\\s");
		private const string InvalidMemento = "invalid";
		#endregion

		#region Nested controls
		private TextBox valueTextBox;
		#endregion

		#region Behavior properties

		#region AutoPostBack
		/// <summary>
		/// Ud�v�, zda m� po zm�n� hodnoty v UI doj�t k postbacku.
		/// </summary>
		public bool AutoPostBack
		{
			get { return (bool)(ViewState["AutoPostBack"] ?? false); }
			set { ViewState["AutoPostBack"] = value; }
		}
		#endregion

		#region Enabled
		/// <summary>
		/// Ud�v�, zda je control pro zad�n� ��sla povolen.
		/// Pokud je zak�z�n, nen� mo�n� v UI zad�vat hodnotu.
		/// </summary>
		public bool Enabled
		{
			get { return (bool)(ViewState["_Enabled"] ?? true); }
			set { ViewState["_Enabled"] = value; }
		}
		#endregion

		#region Decimals
		/// <summary>
		/// Nastavuje po�et desetinn�ch m�st, kter� lze v UI zadat. Na tento po�et desetinn�ch m�st se ��slo form�tuje pro zobrazen�.
		/// V�choz� hodnota je 0.
		/// </summary>
		public int Decimals
		{
			get { return (int)(ViewState["Decimals"] ?? 0); }
			set { ViewState["Decimals"] = value; }
		}

		#endregion

		#region AllowNegativeNumber
		/// <summary>
		/// Ud�v�, zda je povoleno zad�vat v UI z�porn� ��sla (tj. znak "-").
		/// V�choz� hodnota je false.
		/// </summary>
		public bool AllowNegativeNumber
		{
			get { return (bool)(ViewState["AllowNegativeNumber"] ?? false); }
			set { ViewState["AllowNegativeNumber"] = value; }
		}

		#endregion

		#region ZeroAsEmpty
		/// <summary>
		/// Zobraz� editovac� okno jako pr�zdn�, pokud je v nastavena hodnota nula a naopak (pr�zdn� hodnota je vracena jako nula).
		/// V�choz� hodnota je false.
		/// </summary>
		public bool ZeroAsEmpty
		{
			get { return (bool)(ViewState["ZeroAsEmpty"] ?? false); }
			set { ViewState["ZeroAsEmpty"] = value; }
		}
		#endregion

		#region ReadOnly
		/// <summary>
		/// Ud�v�, zda lze hodnotu v textov�m pol��ku editovat.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				return valueTextBox.ReadOnly;
			}
			set
			{
				valueTextBox.ReadOnly = value;
			}
		}		
		#endregion

		#endregion

		#region Appereance properties
		#region Style
		/// <summary>
		/// Stylov�n� ValueTextBoxu.
		/// </summary>
		public Style ValueBoxStyle
		{
			get
			{
				return valueTextBox.ControlStyle;
			}
		}

		#endregion		#endregion
		#endregion

		#region Function properties

		#region Value
		/// <summary>
		/// Vrac� zadan� ��slo. Nen�-li zad�n ��dn� text, vrac� null (pokud je ZeroAsEmpty, vrac� nulu).
		/// Je-li zad�no neplatn� ��slo, vyhod� v�jimku.
		/// </summary>
		[Themeable(false)]
		public decimal? Value
		{
			get
			{
				if (String.IsNullOrEmpty(NumberText))
				{
					return ZeroAsEmpty ? (decimal?)0 : null;
				}

				return Decimal.Parse(NumberText, Thread.CurrentThread.CurrentCulture.NumberFormat);
			}
			set
			{
				if ((value == null) || ((value == 0) && ZeroAsEmpty))
				{
					valueTextBox.Text = "";
				}
				else
				{
					valueTextBox.Text = value.Value.ToString("N" + Decimals.ToString());
				}
			}
		}
		#endregion

		#region ValueAsInt
		/// <summary>
		/// Vrac� zadan� ��slo jako Int32. Nen�-li zad�n ��dn� text, vrac� null (pokud je ZeroAsEmpty, vrac� nulu).
		/// Je-li zad�no neplatn� ��slo, vyhod� v�jimku.
		/// </summary>
		public int? ValueAsInt
		{
			get
			{
				decimal? currentValue = Value;
				if (currentValue == null)
				{
					return null;
				}
				return Convert.ToInt32(currentValue.Value);
			}
		}
		#endregion

		#region IsValid
		/// <summary>
		/// Vrac� true, pokud obsahuje platn� ��slo (tj. pr�zdnou hodnotu NEBO "validn�" datum).
		/// </summary>
		public bool IsValid
		{
			get
			{
				string numberText = NumberText;

				if (numberText == String.Empty)
				{
					return true;
				}

				// pokud nem��eme ��slo p�ev�st na decimal, je to �patn�
				Decimal resultDecimal;
				return Decimal.TryParse(numberText, NumberStyles.Number, Thread.CurrentThread.CurrentCulture.NumberFormat, out resultDecimal);
			}
		}
		#endregion

		#region NumberText
		/// <summary>
		/// Hodnota zadan� v textov�m pol��ku o�ezanou o whitespaces (i z prost�edka textu, nikoliv prost� trim).
		/// Vlastnost nen� ur�ena pro zpracov�n�, slou�� pro valid�tory a pro parsov�n� hodnoty na ��slo.
		/// </summary>
		public string NumberText
		{
			get
			{
				return whitespaceremover.Replace(valueTextBox.Text, String.Empty);
			}
		}
		#endregion
		#endregion

		#region ValueChanged
		/// <summary>
		/// Ud�lost je vyvol�na, kdykoliv u�ivatel zm�n� editovanou hodnotu, resp. kdykoliv se po u�ivatelov� z�sahu zm�n� hodnota Value.
		/// (programov� zm�na Value nevyvol� ud�lost ValueChanged).
		/// Ud�lost je vyvol�na v situac�ch:
		/// <list>
		///		<item>hodnota 1 &lt;--^gt; hodnota 2 (nap�. "1" na "2")</item> 
		///		<item>hodnota 1 &lt;--^gt; chybn� hodnota (nap�. "1" na "xx")</item> 
		///		<item>hodnota 1 &lt;--^gt; ��dn� hodnota (nap�. "" na "xx")</item> 
		/// </list>
		/// Ud�lost NEN� vyvol�na p�i zm�n� form�tu hodnoty, nap�.
		/// <list>
		///		<item>Zm�na form�tu data: "1" na "01")</item> 
		///		<item>��ste�n� (ne�pln�) korekce chyby (nap�. "xx1" na "x1")</item> 
		///		<item>�prava pr�zdn� hodnoty (nap�. "" na "(mezera)" )</item>
		/// </list>
		/// </summary>
		public event EventHandler ValueChanged
		{
			add
			{
				_valueChanged += value;
			}
			remove
			{
				_valueChanged -= value;
			}
		}
		private EventHandler _valueChanged;
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

		#region ValidationGroup
		/// <summary>
		/// ValidationGroup pro validaci.
		/// </summary>
		public string ValidationGroup
		{
			get
			{
				return (string)ViewState["ValidationGroup"] ?? String.Empty;
			}
			set
			{
				ViewState["ValidationGroup"] = value;
			}
		}
		#endregion

		#region CausesValidation
		/// <summary>
		/// Ur�uje, zda doch�z� k validaci p�i postbacku zp�soben�m t�mto controlem (autopostback).
		/// </summary>
		public bool CausesValidation
		{
			get
			{
				return (bool)(ViewState["CausesValidation"] ?? false);
			}
			set
			{
				ViewState["CausesValidation"] = value;
			}
		}
		#endregion

		#region --------------------------------------------------------------------------------
		#endregion

		#region Constructor
		/// <summary>
		/// Kontruktor.
		/// </summary>
		public NumericBox()
		{
			valueTextBox = new TextBox();
			valueTextBox.ID = "ValueTextBox";
		}
		#endregion

		#region OnInit
		/// <summary>
		/// OnInit (overriden).
		/// </summary>
		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			valueTextBox.TextChanged += new EventHandler(ValueTextBox_TextChanged);
			EnsureChildControls();
		}
		#endregion

		#region Controls
		/// <summary>
		/// Controls (overriden).
		/// </summary>
		public override ControlCollection Controls
		{
			get
			{
				EnsureChildControls();
				return base.Controls;
			}
		}
		#endregion

		#region CreateChildControls
		/// <summary>
		/// CreateChildControls (overriden).
		/// </summary>
		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			this.Controls.Add(valueTextBox);
		}
		#endregion

		#region OnPreRender
		/// <summary>
		/// OnPreRender (overriden).
		/// </summary>
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			valueTextBox.Enabled = this.Enabled;
			valueTextBox.AutoPostBack = this.AutoPostBack;
			valueTextBox.ValidationGroup = this.ValidationGroup;
			valueTextBox.CausesValidation = this.CausesValidation;

			valueTextBox.Style.Add("text-align", "right");

			if (Enabled)
			{
				RegisterScripts();

				if (valueTextBox.MaxLength == 0)
				{
					valueTextBox.MaxLength = 12;
				}

				valueTextBox.Attributes.Add("onkeypress", String.Format("HavitNumericBox_KeyPress(event, {0}, {1});", AllowNegativeNumber.ToString().ToLower(), Decimals));
				valueTextBox.Attributes.Add("onchange", String.Format("HavitNumericBox_Fix(event, {0}, {1});", AllowNegativeNumber.ToString().ToLower(), Decimals));
				if (!ReadOnly)
				{
					valueTextBox.Attributes.Add("onfocus", "HavitNumericBox_Focus(event);");
				}
			}

			ViewState["ValueMemento"] = GetValueMemento();
		}		
		#endregion

		#region RegisterScripts
		/// <summary>
		/// Registruje klientsk� skripty omezuj�c� vstup z kl�vesnice.
		/// </summary>
		private void RegisterScripts()
		{
			char decimalSeparator = (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator[0]);

			string thousandsSeparator = (Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator);
			if (String.IsNullOrEmpty(thousandsSeparator) || (thousandsSeparator[0] == (char)160))
			{
				thousandsSeparator = " ";
			}
			else
			{
				thousandsSeparator = thousandsSeparator.Substring(0, 1);
			}

			// ((charCode >= 48) && (charCode <= 57)) ... znaky 0-9
			// (charCode < 31) .. speci�ln� symboly (Home, End, Enter, Backspace, apod.)
			// (allowNegativeNumber && (charCode == 45)) ... znam�nko m�nus, je-li povolena z�porn� ��sla
			// ((decimals > 0) && ... ) ... desetinn� odd�lova�, jsou-li desetinn� m�sta povolena a z�rove� je�t� desetinn� odd�lova� nen� uveden
			string javaScript = 
@"function HavitNumericBox_KeyPress(e, allowNegativeNumber, decimals)
{
	var charCode = (window.event) ? window.event.keyCode : e.charCode;
	var element = (e.target) ? e.target : window.event.srcElement;
    var validKey = (charCode == " + (byte)thousandsSeparator[0] + @")
		|| ((charCode >= 48) && (charCode <= 57)) 
		|| (charCode <= 31)
        || (allowNegativeNumber && (charCode == 45) && element.value.indexOf(String.fromCharCode(charCode)) == -1)
        || ((decimals > 0) && (charCode == " + (byte)decimalSeparator + @") && element.value.indexOf(String.fromCharCode(charCode)) == -1);
	if (!validKey)
	{
		if (window.event) { window.event.returnValue = null; } else { e.preventDefault(); }
	}
}

function HavitNumericBox_Fix(e, allowNegativeNumber, decimals)
{
	var element = (e.target) ? e.target : window.event.srcElement;
	var position;

	var value = element.value;
	position = value.indexOf('" + decimalSeparator + @"');
	if (position >= 0)
	{
		value = value.substr(0, position + decimals + 1);								
		element.value = value;
	}
}

function HavitNumericBox_Focus(e)
{
	var element = (e.target) ? e.target : window.event.srcElement;
	if ((element != null) && element.createTextRange)
	{
		element.createTextRange().select();
	}
}";

			ScriptManager.RegisterClientScriptBlock(this.Page, typeof(NumericBox), clientScriptBlockName, javaScript, true);
		}
		#endregion

		#region ClientID
		/// <summary>
		/// ClientID (overriden).
		/// Vrac� ClientID obsa�en�ho TextBoxu pro zad�v�n� hodnoty.
		/// To �e�� klientsk� valid�tory, kter� natrvdo p�edpokl�daj�, �e validovan� control (podle ClientID)
		/// obsahuje klientskou vlastnost "value". T�mto klientsk�mu valid�toru m�sto DateTimeBoxu podstr��me nested TextBox.
		/// </summary>
		public override string ClientID
		{
			get
			{
				return valueTextBox.ClientID;
			}
		}
		#endregion

		#region ValueTextBox_TextChanged
		/// <summary>
		/// Obsluha zm�ny textu v nested controlu.
		/// Ov��uje, zda do�lo ke zm�n� hodnoty a pokud ano, vyvol� prost�ednictv�m metody OnValueChanged ud�lost ValueChanged.
		/// </summary>
		private void ValueTextBox_TextChanged(object sender, EventArgs e)
		{
			if (ViewState["ValueMemento"] != GetValueMemento())
			{
				OnValueChanged(EventArgs.Empty);
			}
		}
		#endregion

		#region GetValueMemento
		/// <summary>
		/// Metoda vrac� editovanou hodnotu jako stav.
		/// Slou�� k detekci, zda do�lo ke zm�n� hodnoty mezi postbacky.
		/// </summary>
		private object GetValueMemento()
		{
			if (!IsValid)
			{
				return InvalidMemento;
			}
			else
			{
				return Value;
			}
		}
		#endregion

		#region OnValueChanged
		/// <summary>
		/// Vyvol�v� ud�lost ValueChanged. V�ce viz ValueChanged.
		/// </summary>
		private void OnValueChanged(EventArgs eventArgs)
		{
			if (_valueChanged != null)
			{
				_valueChanged(this, eventArgs);
			}
		}
		#endregion
	}
}