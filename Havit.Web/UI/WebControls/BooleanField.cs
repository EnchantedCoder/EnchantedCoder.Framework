using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Reflection;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Sloupec pro zobrazen� boolean hodnoty.
	/// </summary>
	public class BooleanField : BoundFieldExt
	{
		#region Properties
		/// <summary>
		/// Text zobrazen�, pokud je hodnota true.
		/// </summary>
		public string TrueText
		{
			get
			{
				return (string)ViewState["TrueText"];
			}
			set
			{
				ViewState["TrueText"] = value;
			}
		}

		/// <summary>
		/// Text zobrazen�, pokud je hodnota false.
		/// </summary>
		public string FalseText
		{
			get
			{
				return (string)ViewState["FalseText"];
			}
			set
			{ 
				ViewState["FalseText"] = value; 
			}
		}
		#endregion

		#region FormatDataValue

		/// <summary>
		/// Zajist� transformaci boolean hodnoty na text s pou�it�m vlastnost� TrueText, FalseText.
		/// </summary>
		/// <param name="value">Hodnota ke zform�tov�n�.</param>
		/// <returns>Text k zobrazen�.</returns>
		public override string FormatDataValue(object value)
		{
			if (value != null)
			{
				return (bool)value ? TrueText : FalseText;
			}
			else
			{
				return String.Empty;
			}
		}
		#endregion

	}
}
