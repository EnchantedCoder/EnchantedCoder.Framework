using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Havit.Web.UI.Scriptlets
{
    /// <summary>
    /// P�edek pro tvorbu klientsk�ch parametr�.
    /// </summary>
    [ControlBuilder(typeof(NoLiteralContolBuilder))]
    public abstract class ParameterBase : ScriptletNestedControl, IScriptletParameter
    {
		#region Name
		/// <summary>
		/// N�zev parametru, pod kter�m bude parametr p��stupn� v kliensk�m skriptu.
		/// </summary>
		public virtual string Name
		{
			get { return (string)ViewState["Name"]; }
			set { ViewState["Name"] = value; }
		}
		#endregion

		#region CheckProperties
		/// <summary>
		/// Zkontroluje, zda je parametr spr�vn� inicializov�n.
		/// </summary>
		public virtual void CheckProperties()
		{
			// zkontrolujeme property Name
			CheckNameProperty();
		}
		#endregion

		#region CheckNameProperty
		/// <summary>
		/// Testuje nastaven� hodnoty property Name.
		/// Pokud nen� hodnota nastavena, je vyhozena v�jimka.
		/// </summary>
		protected virtual void CheckNameProperty()
		{
			if (String.IsNullOrEmpty(Name))
			{
				throw new ArgumentException("Property Name nen� nastavena.");
			}
		}
		#endregion

		#region AddedControl
		/// <summary>
		/// Zavol�no, kdy� je do kolekce Controls p�id�n Control.
		/// Zaji��uje, aby nebyl p�id�n control neimplementuj�c� 
		/// IScriptletParameter.
		/// </summary>
		/// <param name="control">P�id�van� control.</param>
		/// <param name="index">Pozice v kolekci control�, kam je control p�id�v�n.</param>
		protected override void AddedControl(Control control, int index)
		{
			base.AddedControl(control, index);

			if (!(control is IScriptletParameter))
			{
				throw new ArgumentException(String.Format("Do parametru scriptletu je vkl�d�n nepodporovan� control {0}.", control));
			}
		}
		#endregion

		#region GetInitializeClientSideValueScript (abstract, IScriptletParameter)
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetInitializeClientSideValueScript")]/*' />
		public abstract void GetInitializeClientSideValueScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion

		#region GetAttachEventsScript (abstract, IScriptletParameter)
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetAttachEventsScript")]/*' />
		public abstract void GetAttachEventsScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion

		#region GetDetachEventsScript (abstract, IScriptletParameter)
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetDetachEventsScript")]/*' />
		public abstract void GetDetachEventsScript(string parameterPrefix, Control parentControl, ScriptBuilder scriptBuilder);
		#endregion
	}
}
