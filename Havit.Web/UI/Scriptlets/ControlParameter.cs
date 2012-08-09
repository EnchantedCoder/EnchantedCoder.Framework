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
        /// <summary>
        /// N�zev Controlu, kter� je zdrojem pro vytvo�en� klientsk�ho parametru.
        /// </summary>
		public string ControlName
		{
			get { return (string)ViewState["ControlName"]; }
			set { ViewState["ControlName"] = value; }		
		}

		/// <summary>
		/// N�zev parametru, pod kter�m bude parametr p��stupn� v kliensk�m skriptu.
		/// Pokud nen� hodnota nastavena, pou�ije se ControlName.
		/// </summary>
		public override string Name
        {
            get { return base.Name ?? ControlName; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Ud�v�, zda v p��pad� zm�ny hodnoty prvku (za�krtnut�, zm�na textu, apod.)
        /// dojde ke spu�t�n� skriptu.
        /// V�choz� hodnota je false.
        /// </summary>
        public bool StartOnChange
        {
            get { return (bool)(ViewState["StartOnChange"] ?? false); }
            set { ViewState["StartOnChange"] = value; }
        }

		/// <summary>
		/// Zkontroluje, zda je parametr spr�vn� inicializov�n.
		/// </summary>
		public override void CheckProperties()
		{
			base.CheckProperties();
            // nav�c zkontrolujeme nastaven� ControlName
            CheckControlNameProperty();
		}

		/// <summary>
		/// Testuje nastaven� hodnoty property Name.
		/// P�episuje chov�n� p�edka t�m zp�sobem, �e zde nen� property Name povinn�
		/// (tak�e se ani netestuje).
		/// </summary>
		protected override void CheckNameProperty()
        {
            // nebudeme zde jm�no kontrolovat
        }

        /// <summary>
        /// Zkontroluje nastaven� property ControlName.
        /// Pokud nen� hodnota nastavena, vyhod� v�jimku.
        /// </summary>
        protected virtual void CheckControlNameProperty()
        {
            if (String.IsNullOrEmpty(ControlName))
                throw new ArgumentException("Property ControlName nem� hodnotu.");
        }

        /// <summary>
        /// Vytvo�� klientsk� skript pro parametr.
        /// </summary>
        /// <param name="parameterPrefix">Prefix pro n�zev parametru. Controly mohou b�t slo�en� (nap�. TextBox v Repeateru).</param>
        /// <param name="parentControl">Rodi�ovsk� prvek, pro kter� je parametr renderov�n.</param>
        /// <param name="builder">Script builder.</param>
		public override void CreateParameter(string parameterPrefix, Control parentControl, ScriptBuilder builder)
		{
            // najdeme control
            Control control = GetControl(parentControl);

            // ak kdy� je viditeln�
            if (control.Visible)
            {
                // najdeme extender, kter� tento control bude �e�it
                IControlExtender extender = Scriptlet.ControlExtenderRepository.FindControlExtender(control);
                // a �ekneme, a� ho vy�e��
                extender.CreateParameter(parameterPrefix, this, control, builder);
            }
		}

        /// <summary>
        /// Nalezne Control, kter� m� b�t zpracov�n.
        /// Pokud nen� Control nalezen, vyhod� v�jimku.
        /// </summary>
        /// <param name="parentControl">Control v r�mci n�ho� se hled� (NamingContainer).</param>
        /// <returns>Control.</returns>
        protected virtual Control GetControl(Control parentControl)
        {            
            Control result = parentControl.FindControl(ControlName);
            
            if (result == null)
                throw new ArgumentException(String.Format("Control {0} nebyl nalezen.", ControlName));

            return result;
        }
    }
}
