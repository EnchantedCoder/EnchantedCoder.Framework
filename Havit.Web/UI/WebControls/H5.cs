using System;
using System.Web.UI;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Reprezentuje label, kter� se renderuje jako HTML tag H5.
	/// </summary>
	public class H5 : System.Web.UI.WebControls.Label
	{
		/// <summary>
		/// Vrac� HtmlTextWriterTag.H5 zaji��uj�c� spr�vn� renderov�n�.
		/// </summary>
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.H5;
			}
		}
	}
}
