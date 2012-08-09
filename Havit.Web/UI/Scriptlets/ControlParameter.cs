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
    /// Parametr Skriptletu reprezentuj�c� renderovan� control Control.
    /// </summary>
    public class ControlParameter : ParameterBase
    {
#warning Comment
		public Control Control
		{
			get
			{
				return _control;
			}
			set
			{
				_control = value;
			}
		}
		private Control _control;

		#region ControlName
		/// <summary>
		/// N�zev Controlu, kter� je zdrojem pro vytvo�en� klientsk�ho parametru.
		/// Pro vyhled�v�n� ve vno�en�m naming containeru lze n�zvy control� odd�lit te�kou.
		/// </summary>
		public string ControlName
		{
			get { return (string)ViewState["ControlName"]; }
			set { ViewState["ControlName"] = value; }
		}
		#endregion

		#region Name
		/// <summary>
		/// N�zev parametru, pod kter�m bude parametr p��stupn� v kliensk�m skriptu.
		/// Pokud nen� hodnota nastavena, pou�ije se hodnota <see cref="ControlName">ControlName</see>.
		/// Obsahuje-li <see cref="ControlName">ControlName</see> te�ku, je nahrazena podtr��tkem.
		/// </summary>
		public override string Name
		{
			get { return base.Name ?? ControlName.Replace(".", "_"); }
			set { base.Name = value; }
		}		
		#endregion

		#region StartOnChange
		/// <summary>
		/// Ud�v�, zda v p��pad� zm�ny hodnoty prvku (za�krtnut�, zm�na textu, apod.)
		/// dojde ke spu�t�n� skriptu.
		/// V�choz� hodnota je <c>false</c>.
		/// </summary>
		public bool StartOnChange
		{
			get { return (bool)(ViewState["StartOnChange"] ?? false); }
			set { ViewState["StartOnChange"] = value; }
		}
		#endregion

		#region CheckProperties (overriden)
		/// <summary>
		/// Zkontroluje, zda je parametr spr�vn� inicializov�n.
		/// </summary>
		public override void CheckProperties()
		{
			base.CheckProperties();
			// nav�c zkontrolujeme nastaven� ControlName
			CheckControlAndControlNameProperty();
		}		
		#endregion

		#region CheckNameProperty
		/// <summary>
		/// Testuje nastaven� hodnoty property Name.
		/// P�episuje chov�n� p�edka t�m zp�sobem, �e zde nen� property Name povinn�
		/// (tak�e se ani netestuje).
		/// </summary>
		protected override void CheckNameProperty()
		{
			// narozd�l od zde definujeme jm�no jako nepovinn�
			// nebudeme zde tedy jm�no kontrolovat
		}
		#endregion

		#region CheckControlAndControlNameProperty
		/// <summary>
		/// Zkontroluje nastaven� property <see cref="Control">Control</see> a <see cref="ControlName">ControlName</see>.
		/// Pokud nen� nastavena hodnota pr�v� jedn� vlastnosti, vyhod� v�jimku.
		/// </summary>
		protected virtual void CheckControlAndControlNameProperty()
		{
			if ((_control == null) && String.IsNullOrEmpty(ControlName))
			{
				throw new HttpException("Nen� ur�en control, nastavte vlastnost Control nebo ControlName.");
			}
			if ((_control != null) && !String.IsNullOrEmpty(ControlName))
			{
				throw new HttpException("Nen� mo�n� ur�it control vlastnostmi Control a ControlName z�rove�.");
			}
		}
		#endregion

		#region GetInitializeClientSideValueScript
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetInitializeClientSideValueScript")]/*' />        
		public override void GetInitializeClientSideValueScript(string parameterPrefix, Control parentControl, ScriptBuilder builder)
		{
			// najdeme control
			Control control = GetControl(parentControl);
			DoJobOnExtender(control, delegate(IControlExtender extender)
			{
				extender.GetInitializeClientSideValueScript(parameterPrefix, this, control, builder);
			});
		}
		#endregion

		#region GetAttachEventsScript
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetAttachEventsScript")]/*' />
		public override void GetAttachEventsScript(string parameterPrefix, Control parentControl, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			// najdeme control
			Control control = GetControl(parentControl);
			DoJobOnExtender(control, delegate(IControlExtender extender)
			{
				extender.GetAttachEventsScript(parameterPrefix, this, control, scriptletFunctionCallDelegate, scriptBuilder);
			});
		}
		#endregion

		#region GetDetachEventsScript
		/// <include file='..\\Dotfuscated\\Havit.Web.xml' path='doc/members/member[starts-with(@name,"M:Havit.Web.UI.Scriptlets.IControlParameter.GetDetachEventsScript")]/*' />
		public override void GetDetachEventsScript(string parameterPrefix, Control parentControl, string scriptletFunctionCallDelegate, ScriptBuilder scriptBuilder)
		{
			// najdeme control
			Control control = GetControl(parentControl);
			DoJobOnExtender(control, delegate(IControlExtender extender)
			{
				extender.GetDetachEventsScript(parameterPrefix, this, control, scriptletFunctionCallDelegate, scriptBuilder);
			});
		}
		#endregion

		#region DoJobOnExtender (ExtenderJobEventHandler)
		private void DoJobOnExtender(Control control, ExtenderJobEventHandler job)
		{
			// ak kdy� je viditeln�
			if (control.Visible)
			{
				// najdeme extender, kter� tento control bude �e�it
				IControlExtender extender = Scriptlet.ControlExtenderRepository.FindControlExtender(control);
				// a �ekneme, a� ho vy�e��
				job(extender);
			}
		}
		private delegate void ExtenderJobEventHandler(IControlExtender extender);
		
		#endregion		

		#region GetControl
		/// <summary>
		/// Nalezne Control, kter� m� b�t zpracov�n.
		/// Pokud nen� Control nalezen, vyhod� v�jimku HttpException.
		/// </summary>
		/// <param name="parentControl">Control v r�mci n�ho� se hled� (NamingContainer).</param>
		/// <returns>Control.</returns>
		protected virtual Control GetControl(Control parentControl)
		{
			if (_control != null)
			{
				return Control;
			}

			string controlName = ControlName.Replace(".", "$");

			Control result = parentControl.FindControl(controlName);

			if (result == null)
			{
				throw new HttpException(String.Format("Control {0} nebyl nalezen.", ControlName));
			}

			return result;
		}
		#endregion        
		
    }
}
