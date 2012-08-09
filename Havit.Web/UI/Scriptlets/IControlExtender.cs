using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// Control extender je zodpov�dn� za vyto�en� kliensk�ho skriptu pro 
    /// p�edan� control. D�ky t�to obecnosti je mo�n� p�id�vat vlastn� control
    /// extendery (�i m�nit existuj�c�) a t�m upravit chov�n� Scriptletu pro
    /// dal�� controly.
    /// </summary>
	public interface IControlExtender
	{
        /// <summary>
        /// Vrac� prioritu, s jakou je extender chodn� pro zpracov�n� controlu.
        /// Pokud je extender nevhodn� pro zpracov�n�, vr�c� se null.
        /// </summary>
        /// <param name="control">Control, kter� bude zpracov�v�n.</param>
        /// <returns>Priorita extenderu.</returns>
        int? GetPriority(Control control);

        /// <summary>
        /// Vytvo�� klientsk� parametr pro p�edan� control.
        /// </summary>
        /// <param name="parameterPrefix">N�zev objektu na klientsk� stran�.</param>
        /// <param name="parameter">Parametr p�ed�vaj�c� ��zen� extenderu.</param>
        /// <param name="control">Control ke zpracov�n�.</param>
        /// <param name="builder">Script builder.</param>
        void CreateParameter(string parameterPrefix, IScriptletParameter parameter, Control control, ScriptBuilder builder);
    }
}
