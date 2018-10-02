﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// Control extender pro jednoduché Controly (WebControls).
    /// Extender tvoří skript jen tak, že najde příslušný element
    /// ve stránce a použije jej jako hodnotu parametru.    
    /// </summary>
    public class SimpleControlExtender : IControlExtender
    {
		#region Private fields
		private Type controlType;
		private int priority;
		private string[] changeEvents;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří extender pro daný typ s danou prioritou.
		/// </summary>
		/// <param name="controlType">Typ, který bude tato instance umět řešit.</param>
		/// <param name="priority">Priorita, s jakou jej bude řešit.</param>
		/// <param name="changeEvents">Události, na které je potřeba se navázat pokud má být klientský skript vyvolán v případě změny. Null znamená, že pro tento typ controlu nejsou changeEvents podporovány.</param>
		public SimpleControlExtender(Type controlType, int priority, string[] changeEvents)
		{
			this.controlType = controlType;
			this.priority = priority;
			this.changeEvents = changeEvents;
		}
		#endregion
		
		#region GetPriority
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetPriority")]/*' />
		public virtual int? GetPriority(Control control)
		{
			return (this.controlType.IsAssignableFrom(control.GetType())) ? (int?)this.priority : null;
		}
		#endregion

		#region GetInitializeClientSideValueScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetInitializeClientSideValueScript")]/*' />
		public void GetInitializeClientSideValueScript(string parameterPrefix, IScriptletParameter parameter, Control control, ScriptBuilder scriptBuilder)
        {
            // vytvoříme objekt
			scriptBuilder.AppendFormat("{0}.{1} = document.getElementById(\"{2}\");\n", parameterPrefix, parameter.Name, control.ClientID);
        }
		#endregion

		#region GetAttachEventsScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetAttachEventsScript")]/*' />
		public void GetAttachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			GetEventsScript(BrowserHelper.GetAttachEventScript, parameterPrefix, parameter, control, scriptletFunctionCallDelegate, scriptBuilder);
		}
		
		#endregion

		#region GetDetachEventsScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetDetachEventsScript")]/*' />
		public void GetDetachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			GetEventsScript(BrowserHelper.GetDetachEventScript, parameterPrefix, parameter, control, scriptletFunctionCallDelegate, scriptBuilder);
		}
		#endregion
		
		#region GetEventsScript
		private void GetEventsScript(BrowserHelper.GetAttachDetachEventScriptEventHandler getEventScript, string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			// pokud se má volat klienský skript při změně hodnoty prvku
			if (((ControlParameter)parameter).StartOnChange)
			{
				// ověříme, zda jsou nastaveny události (prázdé pole stačí)
				if (changeEvents == null)
				{
					throw new HttpException("Parametr přikazuje spuštění při změně controlu, u extenderu však není uvedena žádná událost ke které bychom se měli navázat.");
				}

				// pro všechny událost
				foreach (string eventName in changeEvents)
				{
					// vytvoříme skript, který danou událost naváže k elementu
					scriptBuilder.AppendLine(getEventScript.Invoke(
						String.Format("{0}.{1}", parameterPrefix, parameter.Name),
						eventName,
						scriptletFunctionCallDelegate));
				}
			}	
		} 
		#endregion
	}
}