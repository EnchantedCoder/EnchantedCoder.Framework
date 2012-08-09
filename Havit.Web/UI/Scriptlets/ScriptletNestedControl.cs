using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Web.UI.Scriptlets
{
	/// <summary>
	/// �ist� form�ln� t��da zp��stup�uj�c� Scriptlet pro prvky vkl�dan� do Scritletu.
	/// </summary>
	public abstract class ScriptletNestedControl: Control
	{
		#region Scriptlet
		/// <include file='IScriptletParameter.xml' path='doc/members/member[starts-with(@name,"P:Havit.Web.UI.Scriptlets.IScriptletParameter.Scriptlet")]/*' />
		public Scriptlet Scriptlet
		{
			get
			{
				if (Parent is IScriptletParameter)
				{
					return ((IScriptletParameter)Parent).Scriptlet;
				}
				
				if (!(Parent is Scriptlet))
				{
					throw new InvalidOperationException("Scriptlet nebyl nalezen - control se nach�z� na nezn�m�m m�st� ve stromu control�.");
				}
				
				return (Scriptlet)Parent;
			}
		}
		#endregion
	}
}
