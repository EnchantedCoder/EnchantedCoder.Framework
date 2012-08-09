using System;
using System.Collections.Generic;
using System.Text;


namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky testuj�c� rozsah datum�.
	/// </summary>
	public static class DateRangeCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku testuj�c�, zda je datum v intervalu datum�.
		/// </summary>
		public static Condition Create(IOperand operand, DateTime date1, DateTime date2)
		{
			return new TernaryCondition("({0} >= {1} and {0} < {2})", operand, ValueOperand.Create(date1), ValueOperand.Create(date2));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c�, zda je den data (datumu) v intervalu dn� datum�.
		/// Zaji��uje, aby hodnota operandu byla v�t�� nebo rovna datu date1 a aby byla men�� ne� p�lnoc konce date2.
		/// Jin�mi slovy: Argumenty moho obsahovat datum a �as, ale testuje se jen datum bez �asu. Potom 
		/// je zaji��ov�no: DATUM(date1) &lt;= DATUM(operand) &lt; DATUM(date2).
		/// </summary>
		public static Condition CreateDays(IOperand operand, DateTime date1, DateTime date2)
		{
			return Create(operand, date1.Date, date2.Date.AddDays(1));
		}
	}
}
