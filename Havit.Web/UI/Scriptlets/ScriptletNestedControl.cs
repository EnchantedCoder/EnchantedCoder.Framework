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
		/// <summary>
		/// Zp��stup�uje Scriplet, ve kter�m je tento ClientScript obsa�en.
		/// </summary>
		public Scriptlet Scriptlet
		{
			get {
                if (Parent is IScriptletParameter)
                    return ((IScriptletParameter)Parent).Scriptlet;
                return (Scriptlet)Parent; 
            }
		}

	}
}
