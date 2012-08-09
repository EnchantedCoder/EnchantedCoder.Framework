using System;
using System.Collections.Generic;
using System.Text;
using Havit.Business.Conditions;

namespace Havit.Business.Conditions
{
	/// <summary>
	/// Vytv��� podm�nku testuj�c� logickou hodnotu.
	/// </summary>	
	public static class BoolCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku pro vlastnost rovnou dan� hodnot�.
		/// </summary>
		public static ICondition CreateEquals(Property property, bool? value)
		{
			if (value == null)
				return NullCondition.CreateIsNull(property);
			else
				return new BinaryCondition(BinaryCondition.EqualsPattern, property, ValueOperand.FromBoolean(value.Value));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu true.
		/// </summary>
		public static ICondition CreateTrue(Property property)
		{
			return CreateEquals(property, true);
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu false.
		/// </summary>
		public static ICondition CreateFalse(Property property)
		{
			return CreateEquals(property, false);
		}
	}
}
