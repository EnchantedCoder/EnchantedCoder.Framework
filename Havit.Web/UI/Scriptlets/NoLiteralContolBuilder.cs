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
	/// Control builder pro <see cref="Scriptlet">Scriptlet</see>.
	/// Omezuje chybn� pou�it� controlu <see cref="Scriptlet">Scriptlet</see>.
    /// </summary>
    internal class NoLiteralContolBuilder : ControlBuilder
    {
		#region AllowWhitespaceLiterals
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
		#endregion

		#region AppendLiteralString
		public override void AppendLiteralString(string s)
		{
			if (s.Trim().Length > 0)
			{
				throw new HttpException("Textov� liter�l je na nepovolen�m m�st�.");
			}
		}
		#endregion        
    }
}