using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// ControlExtender ur�en� ke tvorb� potomk�, kte�� prov�d�j� substituci za jin� control.
	/// Nap�. pokud je pot�eba substituovat UserControl za n�kter� vno�en� control.
	/// </summary>
	public abstract class SubstitutionControlExtenderBase: IControlExtender
	{
		#region GetPriority
		/// <summary>
		/// Vrac� priotitu vhodnosti extenderu pro zpracov�n� controlu.
		/// Pokud extender nen� vhodn� pro zpracov�n� controlu, vrac� null.
		/// </summary>
		/// <param name="control">Ov��ovan� control.</param>
		/// <returns>Priorita.</returns>
		public int? GetPriority(Control control)
		{
			return (this.GetSupportedControlType().IsAssignableFrom(control.GetType())) ? (int?)this.GetPriorityValue: null;
		}
		#endregion
		
		#region IControlExtender Members
		/// <summary>
		/// Vytvo�� klientsk� parametr pro p�edan� control.
		/// </summary>
		/// <param name="parameterPrefix">N�zev objektu na klientsk� stran�.</param>
		/// <param name="parameter">Parametr p�ed�vaj�c� ��zen� extenderu.</param>
		/// <param name="control">Control ke zpracov�n�.</param>
		/// <param name="scriptBuilder">Script builder.</param>
		public void GetInitializeClientSideValueScript(string parameterPrefix, IScriptletParameter parameter, System.Web.UI.Control control, ScriptBuilder scriptBuilder)
		{
			Control substitutedControl = GetSubstitutedControl(control);
			parameter.Scriptlet.ControlExtenderRepository.FindControlExtender(substitutedControl).GetInitializeClientSideValueScript(parameterPrefix, parameter, substitutedControl, scriptBuilder);
		}

		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetAttachEventsScript")]/*' />
		public void GetAttachEventsScript(string parameterPrefix, IScriptletParameter parameter, System.Web.UI.Control control, ScriptBuilder scriptBuilder)
		{
			Control substitutedControl = GetSubstitutedControl(control);
			parameter.Scriptlet.ControlExtenderRepository.FindControlExtender(substitutedControl).GetAttachEventsScript(parameterPrefix, parameter, substitutedControl, scriptBuilder);
		}
		
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetDetachEventsScript")]/*' />
		public void GetDetachEventsScript(string parameterPrefix, IScriptletParameter parameter, System.Web.UI.Control control, ScriptBuilder scriptBuilder)
		{
			Control substitutedControl = GetSubstitutedControl(control);
			parameter.Scriptlet.ControlExtenderRepository.FindControlExtender(substitutedControl).GetDetachEventsScript(parameterPrefix, parameter, substitutedControl, scriptBuilder);
		}
		#endregion

		#region GetPriorityValue (virtual)
		/// <summary>
		/// Vr�t� hodnotu priority ControlExtenderu, kter� se pou�ije, pokud je 
		/// ControlExtender pou�iteln� pro zpracov�n� controlu.
		/// </summary>
		protected virtual int GetPriorityValue
		{
			get
			{
				return 100;
			}
		}
		#endregion

		#region GetSubstitutedControl (abstract)
		/// <summary>
		/// Vrac� substituovan� control.
		/// </summary>
		protected abstract Control GetSubstitutedControl(Control control);
		#endregion

		#region GetSupportedControlType (abstract)
		/// <summary>
		/// Vrac� typ, kter� je t��dou podporov�n k substituci.
		/// </summary>
		protected abstract Type GetSupportedControlType();
		#endregion
	}
}
