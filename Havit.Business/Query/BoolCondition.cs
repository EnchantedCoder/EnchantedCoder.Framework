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
		public static Condition CreateEquals(PropertyInfo property, bool? value)
		{
			if (value == null)
			{
				return NullCondition.CreateIsNull(property);
			}
			else
			{
				return new BinaryCondition(BinaryCondition.EqualsPattern, property, ValueOperand.Create(value.Value));
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu true.
		/// </summary>
		public static Condition CreateTrue(PropertyInfo property)
		{
			return CreateEquals(property, true);
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu false.
		/// </summary>
		public static Condition CreateFalse(PropertyInfo property)
		{
			return CreateEquals(property, false);
		}
	}
}
