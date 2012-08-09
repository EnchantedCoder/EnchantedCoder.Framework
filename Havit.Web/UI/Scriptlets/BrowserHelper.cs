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
    /// Pomocn�k pro identifikaci prohl�e�e.
    /// </summary>
    public static class BrowserHelper
    {
        /// <summary>
        /// Vrac� true, pokud byl aktu�ln� HttpRequest poch�z� z Internet Exploreru 
        /// (nebo shodn� se identifikuj�c�ho browseru).
        /// </summary>
        public static bool IsInternetExplorer
        {
            get
            {
                return HttpContext.Current.Request.Browser.Browser == "IE";
            }
        }

        /// <summary>
        /// Vr�t� p��kaz pro p�ipojen� ud�losti k objektu.
        /// Detekuji IE, kter� p�ipojuje ud�losti jinak ne� ostatn� prohl�e�e.
        /// </summary>
        /// <param name="attachingObject">Objekt, ke kter�mu je p�ipojov�na ud�lost.</param>
        /// <param name="eventName">N�zev ud�losti v� "on", nap� "onchange", "onclick".</param>
        /// <param name="function">Funkce, kter� je p�ipojov�na. Obsahuje-li �et�zev, mus� b�t uvozen uvozovkami a nikoliv apostrofy.</param>
        /// <returns>P��kaz p�ipojuj�c� ud�lost k objektu.</returns>
        public static string GetAttachEvent(string attachingObject, string eventName, string function)
        {
            if (function.Contains("("))
                function = String.Format("new Function(\'{0}\')", function);

            if (IsInternetExplorer)
                return String.Format("{0}.attachEvent(\"{1}\", {2});", attachingObject, eventName, function);
            else
                return String.Format("{0}.addEventListener(\"{1}\", {2}, false);", attachingObject, eventName.Substring(2), function);
        }
    }
}