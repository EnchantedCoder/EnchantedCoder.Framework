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
    /// Pomocn�k pro identifikaci prohl�e�e a p��pravu browser-specific skript�.
    /// </summary>
    internal static class BrowserHelper
    {
		#region IsInternetExplorer
		/// <summary>
		/// Vrac� <c>true</c>, pokud byl aktu�ln� <see cref="System.Web.HttpRequest">HttpRequest</see> poch�z� z Internet Exploreru 
		/// (nebo shodn� se identifikuj�c�ho browseru).
		/// </summary>
		public static bool IsInternetExplorer
		{
			get
			{
				return HttpContext.Current.Request.Browser.Browser == "IE";
			}
		}
		#endregion

		#region GetAttachEventScript
		/// <summary>
		/// Vr�t� p��kaz pro p�ipojen� ud�losti k objektu.
		/// Detekuji IE, kter� p�ipojuje ud�losti jinak ne� ostatn� prohl�e�e.
		/// </summary>
		/// <param name="attachingObject">Objekt, ke kter�mu je p�ipojov�na ud�lost.</param>
		/// <param name="eventName">N�zev ud�losti v�etn� "on", nap��klad "onchange", "onclick", atp.</param>
		/// <param name="functionDelegateName">Deleg�t, kter� je p�ipojov�n.</param>
		/// <returns>P��kaz p�ipojuj�c� ud�lost k objektu.</returns>
		public static string GetAttachEventScript(string attachingObject, string eventName, string functionDelegateName)
		{
			if (functionDelegateName.Contains("("))
			{
				throw new ArgumentException("Je nutn� p�edat identifik�tor prom�nn� nesouc� hodnotu deleg�ta.", "functionDelegateName");
				//function = String.Format("new Function(\'{0}\')", function);
			}

			if (IsInternetExplorer)
			{
				return String.Format("{0}.attachEvent(\"{1}\", {2});", attachingObject, eventName, functionDelegateName);
			}
			else
			{
				return String.Format("{0}.addEventListener(\"{1}\", {2}, false);", attachingObject, eventName.Substring(2), functionDelegateName);
			}
		}
		#endregion

		#region GetDetachEventScript
		/// <summary>
		/// Vr�t� p��kaz pro odpojen� ud�losti od objektu.
		/// Detekuji IE, kter� odpojuje ud�losti jinak ne� ostatn� prohl�e�e.
		/// </summary>
		/// <param name="detachingObject">Objekt, od kter�ho je odpojov�na ud�lost.</param>
		/// <param name="eventName">N�zev ud�losti v� "on", nap� "onchange", "onclick".</param>
		/// <param name="functionDelegateName">Deleg�t, kter� je p�ipojov�n.</param>
		/// <returns>P��kaz odpojuj�c� ud�lost od objektu.</returns>
		public static string GetDetachEventScript(string detachingObject, string eventName, string functionDelegateName)
		{
			if (functionDelegateName.Contains("("))
			{
				throw new ArgumentException("Je nutn� p�edat identifik�tor prom�nn� nesouc� hodnotu deleg�ta.", "functionDelegateName");
				//				function = String.Format("new Function(\'{0}\')", function);
			}

			if (IsInternetExplorer)
			{
				return String.Format("{0}.detachEvent(\"{1}\", {2});", detachingObject, eventName, functionDelegateName);
			}
			else
			{
				return String.Format("{0}.removeEventListener(\"{1}\", {2}, false);", detachingObject, eventName.Substring(2), functionDelegateName);
			}
		}
		#endregion

		#region GetAttachDetachEventScriptEventHandler (internal)
		/// <summary>
		/// Deleg�t funkc� GetAttachEventScript a GetDetachEventScript.
		/// </summary>
		/// <param name="manipulatingObject">C�lov� objekt pro nav�z�n�/odv�z�n� ud�losti.</param>
		/// <param name="eventName">N�zev ud�losti v�etn� "on", nap��klad "onchange", "onclick", atp.</param>
		/// <param name="functionDelegateName">Deleg�t, kter� je p�ipojov�n.</param>
		/// <returns>P��kaz p�ipojuj�c� ud�lost k objektu.</returns>
		internal delegate string GetAttachDetachEventScriptEventHandler(string manipulatingObject, string eventName, string functionDelegateName);
		#endregion

	}
}