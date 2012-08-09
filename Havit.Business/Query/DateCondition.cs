using System;
using System.Collections.Generic;
using System.Text;


namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky testuj�c� datumy.
	/// </summary>
	public static class DateCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� rovnost datum�.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, DateTime dateTime)
		{
			return new BinaryCondition(operand, BinaryCondition.EqualsPattern, ValueOperand.Create(dateTime));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnoty pomoc� zadan�ho oper�toru.
		/// </summary>
		public static Condition Create(IOperand operand, ComparisonOperator comparisonOperator, DateTime value)
		{
			return new BinaryCondition(operand, BinaryCondition.GetComparisonPattern(comparisonOperator), ValueOperand.Create(value));
		}

	}
}
