﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;

namespace Havit.Web.UI.Scriptlets
{
	internal class ListControlExtender : IControlExtender
	{
		#region Private fields
		/// <summary>
		/// Priorita control extenderu.
		/// </summary>
		private readonly int priority;

		/// <summary>
		/// Typ, ke kterému je extender registrován.
		/// </summary>
		private readonly Type controlType;				
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvoří instanci ListControlExtenderu.
		/// </summary>
		/// <param name="controlType">Typ, ke kterému je instance vytvářena.</param>
		/// <param name="priority">Priorita control extenderu.</param>
		public ListControlExtender(Type controlType, int priority)
		{
			this.priority = priority;
			this.controlType = controlType;
		}
		#endregion

		#region GetPriority
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetPriority")]/*' />		
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "Bráno z externího souboru.")]
		public int? GetPriority(Control control)
		{
			return (this.controlType.IsAssignableFrom(control.GetType())) ? (int?)this.priority : null;
		}
		#endregion

		#region GetInitializeClientSideValueScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetInitializeClientSideValueScript")]/*' />
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "Bráno z externího souboru.")]
		public void GetInitializeClientSideValueScript(string parameterPrefix, IScriptletParameter parameter, System.Web.UI.Control control, ScriptBuilder scriptBuilder)
		{
			if (!(control is ListControl))
			{
				throw new HttpException("ListControlExtender podporuje pouze controly typu ListControl.");
			}

			ListControl listControl = (ListControl)control;
			scriptBuilder.AppendLineFormat("{0}.{1} = new Array();", parameterPrefix, parameter.Name, control.ClientID);
			for (int i = 0; i < listControl.Items.Count; i++)
			{
				scriptBuilder.AppendLineFormat("{0}.{1}[{2}] = document.getElementById(\"{3}_{2}\");", parameterPrefix, parameter.Name, i, control.ClientID);
			}
		}
		#endregion
		
		#region GetAttachEventsScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetAttachEventsScript")]/*' />
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "Bráno z externího souboru.")]
		public void GetAttachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			GetEventsScript(BrowserHelper.GetAttachEventScript, parameterPrefix, parameter, control, scriptletFunctionCallDelegate, scriptBuilder);
		}
		#endregion

		#region GetDetachEventsScript
		/// <include file='IControlExtender.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlExtender.GetDetachEventsScript")]/*' />
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1604:ElementDocumentationMustHaveSummary", Justification = "Bráno z externího souboru.")]
		public void GetDetachEventsScript(string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			GetEventsScript(BrowserHelper.GetDetachEventScript, parameterPrefix, parameter, control, scriptletFunctionCallDelegate, scriptBuilder);
		}
		#endregion
		
		#region GetEventsScript
		private void GetEventsScript(BrowserHelper.GetAttachDetachEventScriptEventHandler getEventScript, string parameterPrefix, IScriptletParameter parameter, Control control, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			ListControl listControl = (ListControl)control;
			for (int i = 0; i < listControl.Items.Count; i++)
			{
				if (((ControlParameter)parameter).StartOnChange)
				{
					scriptBuilder.AppendLine(getEventScript.Invoke(
						String.Format("{0}.{1}[{2}]", parameterPrefix, parameter.Name, i),
						"onclick",
						scriptletFunctionCallDelegate));
				}
			}
		}
		#endregion		
	}
}
