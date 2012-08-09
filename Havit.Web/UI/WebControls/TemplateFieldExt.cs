using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Roz���en� verze <see cref="System.Web.UI.WebControls.TemplateField"/>.
	/// </summary>
	public class TemplateFieldExt : TemplateField, IIdentifiableField
	{
		#region ID (IIdentifiableField Members)
		/// <summary>
		/// Identifik�tor fieldu na kter� se lze odkazovat pomoc� <see cref="GridViewExt.FindColumn(string)"/>.
		/// </summary>
		public string ID
		{
			get
			{
				object tmp = ViewState["ID"];
				if (tmp != null)
				{
					return (string)tmp;
				}
				return String.Empty;
			}
			set
			{
				ViewState["ID"] = value;
			}
		}
		#endregion
	}
}
