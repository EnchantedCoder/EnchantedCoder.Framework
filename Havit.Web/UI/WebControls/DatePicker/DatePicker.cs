using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.Calendar.js", "text/javascript")]
[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.Calendar-Setup.js", "text/javascript")]
[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.Calendar-cs.js", "text/javascript")]
[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.Calendar-en.js", "text/javascript")]

[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.Calendar.gif", "image/gif")]
[assembly: WebResource("Havit.Web.UI.WebControls.DatePicker.CalendarDisabled.gif", "image/gif")]

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Tla��tko s kalend���kem pro v�b�r data.
	/// </summary>
	public class DatePicker: Control
	{
		public const string DatePickerScript = @"<script type=""text/javascript"">
  Calendar.setup(
    {{
      inputField  : ""{1}"",         // ID of the input field
      ifFormat    : ""{2}"",    // the date format
      button      : ""{0}"",       // ID of the button
      firstDay    : 1,
      weekNumbers : false,
      electric    : false
    }}
  );
</script>";

		private TextBox associatedControl;

		/// <summary>
		/// Control, na kter� je DatePicker nav�z�n.
		/// Pokud nen� nastaven, hled� se control podle AssociatedControlID.
		/// Neukl�d� se do ViewState a nep�e��v� postback.
		/// </summary>
		public TextBox AssociatedControl
		{
			get { return (TextBox)(associatedControl ?? FindControl(AssociatedControlID)); }
			set { associatedControl = value; }
		}

		/// <summary>
		/// ID controlu, na kter� je DatePicker nav�z�n. Pou�ije se jen, pokud nen� nastavena 
		/// vlastnost AssociatedControl.
		/// </summary>
		public string AssociatedControlID
		{
			get { return (string)ViewState["AssociatedControlID"]; }
			set { ViewState["AssociatedControlID"] = value; }
		}

		public bool RegisterCalendarWhenDisabled
		{
			get { return (bool)(ViewState["RegisterCalendarWhenDisabled"] ?? false); }
			set { ViewState["RegisterCalendarWhenDisabled"] = value; }
		}

		public bool Enabled
		{
			get { return (bool)(ViewState["DatePicker_Enabled"] ?? true); }
			set { ViewState["DatePicker_Enabled"] = value; }
		}

/*		/// <summary>
		/// Cesta k obr�zku s ikonou kalend��i.
		/// </summary>
		public string ImageUrl
		{
			get { return (string)(ViewState["ImageUrl"] ?? String.Empty); }
			set { ViewState["ImageUrl"] = value; }
		}
*/
		/// <summary>
		/// Ud�v�, zda se m� zobrazit ikonka. Pokud ne, nav�e se DatePicker p��mo na AssociatedControl.
		/// </summary>
		public bool ShowImage
		{
			get { return (bool)(ViewState["ShowImage"] ?? false); }
			set { ViewState["ShowImage"] = value; }
		}

		/// <summary>
		/// Form�t data generovan� kalend���kem. V�choz� form�t je %d.%m.%Y.
		/// </summary>
		public string DateFormat
		{
			get { return (string)(ViewState["DateFormat"] ?? "%d.%m.%Y"); }
			set { ViewState["DateFormat"] = value; } 
		}

		private Image image;

		protected override void CreateChildControls()
		{
			image = new Image();
			Controls.Add(image);
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			image.Visible = ShowImage;
			image.ImageUrl = Page.ClientScript.GetWebResourceUrl(
				typeof(DatePicker),
				AssociatedControl.Enabled ? "Havit.Web.UI.WebControls.DatePicker.Calendar.gif" : "HavitWare.Web.UI.WebControls.DatePicker.CalendarDisabled.gif"
			);

			if (Visible)
			{
				// Registrace klientsk�ch script�.
				// Script s lokalizacemi se bere podle CurrentUICulture.
				Page.ClientScript.RegisterClientScriptResource(typeof(DatePicker), "Havit.Web.UI.WebControls.DatePicker.Calendar.js");
				Page.ClientScript.RegisterClientScriptResource(typeof(DatePicker),
					String.Format("Havit.Web.UI.WebControls.DatePicker.Calendar-{0}.js",
						CultureInfo.CurrentUICulture.TwoLetterISOLanguageName));
				Page.ClientScript.RegisterClientScriptResource(typeof(DatePicker), "Havit.Web.UI.WebControls.DatePicker.Calendar-Setup.js");

				// Registrace kalend���ku pro control
				Control control = AssociatedControl;
				if (control == null)
#warning Exception
					throw new Exception("DatePicker nem� nastaven AssociatedControl nebo AssociatedControlID.");

				if (Enabled || RegisterCalendarWhenDisabled)
				{
					image.Style.Add(HtmlTextWriterStyle.Cursor, "hand");
					Page.ClientScript.RegisterStartupScript(typeof(DatePicker), this.GetHashCode().ToString(), String.Format(DatePickerScript, ShowImage ? image.ClientID : control.ClientID, control.ClientID, DateFormat));
				}
			}
		}		
	}
}