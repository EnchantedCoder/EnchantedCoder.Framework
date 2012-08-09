using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Roz���en� verze <see cref="System.Web.UI.WebControls.ButtonField"/> (z�kladn� roz���en� spole�n� pro GridView i jin� pou�it� fieldu).
	/// Pro pou�it� v <see cref="GridViewExt"/> a odvozen�ch (nap�. <see cref="Havit.Web.UI.WebControls.EnterpriseGridView"/>)
	/// je doporu�eno pou��t bohat��ho potomka <see cref="GridViewCommandField"/>.
	/// </summary>
	public class CommandFieldExt : CommandField, IIdentifiableField
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
