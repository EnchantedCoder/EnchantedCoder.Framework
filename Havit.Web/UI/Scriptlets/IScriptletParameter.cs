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
	/// Parametr obsa�en� ve <see cref="Scriptlet">Scripletu</see>.
	/// </summary>
	public interface IScriptletParameter
	{
		#region Scriptlet
		/// <summary>
		/// Zp��stup�uje scriptlet, ve kter�m je parametr obsa�en.
		/// </summary>
		Scriptlet Scriptlet { get; }
		#endregion

		#region Name
		/// <summary>
		/// N�zev parametru, pod kter�m bude identifikov�n v klientsk�m skriptu.
		/// </summary>
		string Name { get; }
		#endregion

		#region CheckProperties
		/// <summary>
		/// Zkontroluje nastaven� parametru. Je-li n�jak� nastaven� chybn�,
		/// m� b�t vyhozena v�jimka.
		/// </summary>
		void CheckProperties();
		#endregion

		#region GetInitializeClientSideValueScript
		/// <summary>
		/// Vr�t� skript pro inicializaci hodnoty parametru na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">Prefix pro n�zev parametru. Parametry mohou b�t vno�en� (nap�. TextBox v Repeateru).</param>
		/// <param name="parentControl">Rodi�ovsk� prvek, pro kter� je parametr renderov�n.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetInitializeClientSideValueScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion

		#region GetAttachEventsScript
		/// <summary>
		/// Vr�t� skript pro nav�z�n� ud�lost� k objektu na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">Prefix pro n�zev parametru. Parametry mohou b�t vno�en� (nap�. TextBox v Repeateru).</param>
		/// <param name="parentControl">Rodi�ovsk� prvek, pro kter� je parametr renderov�n.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetAttachEventsScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion
		
		#region GetDetachEventsScript
		/// <summary>
		/// Vr�t� skript pro odpojen� ud�lost� od objektu na klientsk� stran�.
		/// </summary>
		/// <param name="parameterPrefix">Prefix pro n�zev parametru. Parametry mohou b�t vno�en� (nap�. TextBox v Repeateru).</param>
		/// <param name="parentControl">Rodi�ovsk� prvek, pro kter� je parametr renderov�n.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		void GetDetachEventsScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion
	}
}
