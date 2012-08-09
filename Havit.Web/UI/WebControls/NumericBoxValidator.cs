using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// Valid�tor hodnoty controlu NumericBox.
	/// </summary>
	public class NumericBoxValidator: BaseValidator
	{
		#region MinValue
		/// <summary>
		/// Minim�ln� hodnota, kter� je pova�ov�na za platnou hodnotu.
		/// </summary>
		public decimal? MinValue
		{
			get { return (decimal?)ViewState["MinValue"]; }
			set { ViewState["MinValue"] = value; }
		}
		#endregion

		#region MaxValue
		/// <summary>
		/// Maxim�ln� hodnota, kter� je pova�ov�na za platnou hodnotu.
		/// </summary>
		public decimal? MaxValue
		{
			get { return (decimal?)ViewState["MaxValue"]; }
			set { ViewState["MaxValue"] = value; }
		}
		#endregion

		#region EvaluateIsValid (overriden)
		/// <summary>
		/// Testuje platnost ��sla.
		/// </summary>
		protected override bool EvaluateIsValid()
		{			
			Control control = FindControl(ControlToValidate);

			if (control == null)
			{
				throw new ArgumentException("ControlToValidate nebyl nalezen.", "ControlToValidate");
			}

			if (!(control is NumericBox))
			{
				throw new ArgumentException("ControlToValidate nen� NumericBox.", "ControlToValidate");
			}

			NumericBox numericBox = (NumericBox)control;

			// pr�zdn� hodnota je OK
			if (numericBox.NumberText == String.Empty)
			{
				return true;
			}

			// zept�me se, zda je ��slo v�bec ��slem a tud�, jestli smime ��hnout na vlastnost Value		
			if (!numericBox.IsValid)
			{
				return false;
			}

			decimal? numericValue = numericBox.Value;

			if (numericValue == null)
			{
				return true;
			}

			// otestujeme z�porn� ��sla
			if (!numericBox.AllowNegativeNumber && (numericValue < 0))
			{
				return false;
			}

			// testujeme minim�ln� hodnotu
			if ((MinValue != null) && (numericValue < MinValue.Value))
			{
				return false;
			}

			// testujeme maxim�ln� hodnotu
			if ((MaxValue != null) && (numericValue > MaxValue.Value))
			{
				return false;
			}

			// otestujeme desetinn� m�sta
			decimal tempValue = numericValue.Value;
			decimal tmpDecimals = numericBox.Decimals;
			while (tmpDecimals > 0)
			{
				tempValue *= 10;
				tmpDecimals -= 1;
			}			
			if (Math.Abs(tempValue) != Math.Floor(Math.Abs(tempValue))) // je v�ce desetinn�m m�st
			{
				return false;
			}

			return true;
		}
		#endregion
	}
}
