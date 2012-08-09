using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// T��da Highlighting dr�� data pro zv�razn�n� vybran� polo�ky podle hodnoty kl��e.
	/// </summary>
	[Serializable]
	public class Highlighting
	{
		/// <summary>
		/// Hodnota "kl��e" polo�ky, kter� m� b�t zv�razn�na.
		/// Nastavuje p��znak AutoPageChangeEnabled.
		/// </summary>
		public object HighlightValue
		{
			get
			{
				return highlightValue;
			}
			set
			{
				highlightValue = value;
				AutoPageChangeEnabled = true;
			}
		}
		private object highlightValue;

		/// <summary>
		/// Polo�ka dat, jej� hodnota se porovn�v� s HighlightValue.
		/// </summary>
		public string DataField
		{
			get
			{
				return dataField;
			}
			set
			{
				dataField = value;
			}
		}
		private string dataField;

		/// <summary>
		/// P��znak, zda m��e doj�t ke zm�n� str�nky pro zv�razn�n� polo�ky.
		/// P��znak je automaticky nastaven p�i nastaven� hodnoty HighlightValue
		/// a je po databindingu automaticky vypnut.
		/// </summary>
		internal bool AutoPageChangeEnabled
		{
			get
			{
				return autoPageChangeEnabled;
			}
			set
			{
				autoPageChangeEnabled = value;
			}
		}
		bool autoPageChangeEnabled = false;

	}
}
