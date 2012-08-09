using System;
using System.Collections.Generic;
using System.Text;
using Havit.Business;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky testuj�c� ��seln� hodnoty.
	/// </summary>
	public static class NumberCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� rovnost hodnoty.
		/// </summary>
		public static ICondition CreateEquals(Property property, int value)
		{
			return new BinaryCondition(BinaryCondition.EqualsPattern, property, ValueOperand.Create(value));
		}

		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnoty pomoc� zadan�ho oper�toru.
		/// </summary>
		public static ICondition Create(Property property, ComparisonOperator comparisonOperator, int value)
		{
			return new BinaryCondition(property, BinaryCondition.GetComparisonPattern(comparisonOperator), ValueOperand.Create(value));			
		}

	}
}
