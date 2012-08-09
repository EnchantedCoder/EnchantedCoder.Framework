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
    /// Control builder pro Scriptlet.
    /// Zaji��uje omezuje chybn� pou�it� controlu Scriptlet.
    /// </summary>
    internal class NoLiteralContolBuilder : ControlBuilder
    {
        public override bool AllowWhitespaceLiterals()
        {
            return false;
        }

        public override void AppendLiteralString(string s)
        {
            if (s.Trim().Length > 0)
                throw new ArgumentException("Textov� liter�l je na nepovolen�m m�st�.");
        }
        
    }
}