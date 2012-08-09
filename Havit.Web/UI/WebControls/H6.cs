using System;
using System.Web.UI;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Reprezentuje label, kter� se renderuje jako HTML tag H6.
	/// </summary>
	public class H6 : System.Web.UI.WebControls.Label
	{
		/// <summary>
		/// Vrac� HtmlTextWriterTag.H6 zaji��uj�c� spr�vn� renderov�n�.
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.H6;
			}
		}
	}
}
