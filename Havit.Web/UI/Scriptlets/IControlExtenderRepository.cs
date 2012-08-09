using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// Repository extender�. 
    /// </summary>
    public interface IControlExtenderRepository
    {
		#region FindControlExtender
        /// <summary>
        /// Vrac� extender, kter� m� Control zpracov�vat.
        /// </summary>
        /// <param name="control">Control, kter� bude zpracov�v�n.</param>
        /// <returns>Nalezen� extender.</returns>
        IControlExtender FindControlExtender(Control control);
		#endregion
	}
}