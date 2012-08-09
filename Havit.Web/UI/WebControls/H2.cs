using System;
using System.Web.UI;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Reprezentuje label, kter� se renderuje jako HTML tag H2.
	/// </summary>
	public class H2 : System.Web.UI.WebControls.Label
	{
		/// <summary>
		/// Vrac� HtmlTextWriterTag.H2 zaji��uj�c� spr�vn� renderov�n�.
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.H2;
			}
		}
	}
}
