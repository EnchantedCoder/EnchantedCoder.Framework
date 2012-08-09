using System;
using System.Web.UI;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Reprezentuje label, kter� se renderuje jako HTML tag H4.
	/// </summary>
	public class H4 : System.Web.UI.WebControls.Label
	{
		/// <summary>
		/// Vrac� HtmlTextWriterTag.H4 zaji��uj�c� spr�vn� renderov�n�.
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.H4;
			}
		}
	}
}
