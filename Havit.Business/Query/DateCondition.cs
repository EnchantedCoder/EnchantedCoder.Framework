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
			return CreateEquals(operand, ValueOperand.Create(dateTime));			
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� rovnost dvou operand�.
		/// </summary>
		public static Condition CreateEquals(IOperand operand1, IOperand operand2)
		{
			return new BinaryCondition(operand1, BinaryCondition.EqualsPattern, operand2);
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnoty pomoc� zadan�ho oper�toru.
		/// </summary>
		public static Condition Create(IOperand operand, ComparisonOperator comparisonOperator, DateTime value)
		{
			return Create(operand, comparisonOperator, ValueOperand.Create(value));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnoty operand�.
		/// </summary>
		public static Condition Create(IOperand operand1, ComparisonOperator comparisonOperator, IOperand operand2)
		{
			return new BinaryCondition(operand1, BinaryCondition.GetComparisonPattern(comparisonOperator), operand2);
		}

	}
}
