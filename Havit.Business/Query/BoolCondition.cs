using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nku testuj�c� logickou hodnotu.
	/// </summary>	
	public static class BoolCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku pro vlastnost rovnou dan� hodnot�.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, bool? value)
		{
			if (value == null)
			{
				return NullCondition.CreateIsNull(operand);
			}
			else
			{
				return new BinaryCondition(BinaryCondition.EqualsPattern, operand, ValueOperand.Create(value.Value));
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu true.
		/// </summary>
		public static Condition CreateTrue(IOperand operand)
		{
			return CreateEquals(operand, true);
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu false.
		/// </summary>
		public static Condition CreateFalse(IOperand operand)
		{
			return CreateEquals(operand, false);
		}
	}
}
