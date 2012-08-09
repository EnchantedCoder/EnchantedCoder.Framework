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
		#region CreateEquals
		/// <summary>
		/// Vytvo�� podm�nku pro vlastnost rovnou dan� hodnot�.
		/// Je-li hodnota value null, testuje se operand na null (IS NULL).
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
		/// Vytvo�� podm�nku porovn�vaj�c� hodnoty dvou operand� na rovnost.
		/// </summary>
		public static Condition CreateEquals(IOperand operand1, IOperand operand2)
		{
			return new BinaryCondition(BinaryCondition.EqualsPattern, operand1, operand2);
		} 
		#endregion

		#region CreateNotEquals
		/// <summary>
		/// Vytvo�� podm�nku porovn�vaj�c� hodnoty dvou operand� na nerovnost.
		/// Hodnota null nen� ��dn�m zp�sobem zpracov�v�na, tj. pokud alespo� jeden operand m� hodnotu null, nen� ve v�sledku dotazu.
		/// </summary>
		public static Condition CreateNotEquals(IOperand operand1, IOperand operand2)
		{
			return new BinaryCondition(BinaryCondition.NotEqualsPattern, operand1, operand2);
		} 
		#endregion

		#region CreateTrue
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu true.
		/// </summary>
		public static Condition CreateTrue(IOperand operand)
		{
			return CreateEquals(operand, true);
		} 
		#endregion

		#region CreateFalse
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� vlastnost na hodnotu false.
		/// </summary>
		public static Condition CreateFalse(IOperand operand)
		{
			return CreateEquals(operand, false);
		} 
		#endregion
	}
}
