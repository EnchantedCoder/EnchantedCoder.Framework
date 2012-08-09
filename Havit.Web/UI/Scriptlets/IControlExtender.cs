using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// Control extender je zodpov�dn� za vyto�en� kliensk�ho skriptu pro 
    /// p�edan� control. D�ky t�to obecnosti je mo�n� p�id�vat vlastn� control
	/// extendery (�i m�nit existuj�c�) a t�m upravit chov�n�
	/// <see cref="Scriptlet">Scriptletu</see> pro dal�� controly.
    /// </summary>
	public interface IControlExtender
	{
		#region GetPriority
		/// <summary>
		/// Vrac� prioritu, s jakou je extender vhodn� pro zpracov�n� controlu.
		/// Pokud je extender nevhodn� pro zpracov�n�, vr�c� se <c>null</c>.
		/// </summary>
		/// <param name="control">Control, kter� bude zpracov�v�n.</param>
		/// <returns>Priorita extenderu.</returns>
		int? GetPriority(Control control);
		#endregion

		#region GetInitializeClientSideValueScript
		/// <summary>
		/// Vr�t� skript pro inicializaci parametru hodnotou objektu na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">N�zev objektu na klientsk� stran�.</param>
		/// <param name="parameter">Parametr p�ed�vaj�c� ��zen� extenderu.</param>
		/// <param name="control">Control ke zpracov�n�.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetInitializeClientSideValueScript(string parameterPrefix, IScriptletParameter parameter, Control control, ScriptBuilder scriptBuilder);		
		#endregion

		#region GetAttachEventsScript
		/// <summary>
		/// Vr�t� skript pro nav�z�n� ud�lost� k objektu na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">N�zev objektu na klientsk� stran�.</param>
		/// <param name="parameter">Parametr p�ed�vaj�c� ��zen� extenderu.</param>
		/// <param name="control">Control ke zpracov�n�.</param>
		/// <param name="scriptletFunctionCallDelegate">Deleg�t vol�n� funkce scriptletu.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetAttachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder);		
		#endregion

		#region GetDetachEventsScript
		/// <summary>
		/// Vr�t� skript pro odpojen� ud�lost� od objektu na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">N�zev objektu na klientsk� stran�.</param>
		/// <param name="parameter">Parametr p�ed�vaj�c� ��zen� extenderu.</param>
		/// <param name="control">Control ke zpracov�n�.</param>
		/// <param name="scriptletFunctionCallDelegate">Deleg�t vol�n� funkce scriptletu.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetDetachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder); 
		#endregion
	}
}
