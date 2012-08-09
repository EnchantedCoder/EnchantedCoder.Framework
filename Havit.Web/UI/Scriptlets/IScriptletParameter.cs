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
    /// Parametr obsa�en� ve scripletu.
    /// </summary>
    public interface IScriptletParameter
    {
        /// <summary>
        /// Zp��stup�uje scriptlet, ve kter�m je parametr obsa�en.
        /// </summary>
        Scriptlet Scriptlet { get; }

        /// <summary>
        /// N�zev parametru, pod kter�m bude identifikov�n v klientsk�m skriptu.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Zkontroluje nastaven� parametru. Je-li n�jak� nastaven� chybn�,
        /// m� b�t vyhozena v�jimka.
        /// </summary>
        void CheckProperties();

        /// <summary>
        /// Vytvo�� klientsk� skript pro parametr.
        /// </summary>
        /// <param name="parameterPrefix">Prefix pro n�zev parametru. Controly mohou b�t slo�en� (nap�. TextBox v Repeateru).</param>
        /// <param name="parentControl">Rodi�ovsk� prvek, pro kter� je parametr renderov�n.</param>
		/// <param name="scriptBuilder">Script builder.</param>
        void CreateParameter(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
    }
}
