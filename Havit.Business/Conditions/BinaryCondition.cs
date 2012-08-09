using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Conditions
{
	/// <summary>
	/// T��da reprezentuj�c� podm�nku o dvou operandech.
	/// </summary>
	public class BinaryCondition : UnaryCondition, ICondition
	{
		#region Patterns
		/// <summary>
		/// Vzor pro podm�nku LIKE
		/// </summary>
		public const string LikePattern = "({0} LIKE {1})";

		/// <summary>
		/// Vzor pro podm�nku rovnosti.
		/// </summary>
		public const string EqualsPattern = "({0} = {1})";
		#endregion

		/// <summary>
		/// Druh� operand.
		/// </summary>
		protected IOperand Operand2;

		#region Constructors
		/// <summary>
		/// Vytvo�� bin�rn� (dvojoperandovou) podm�nku.
		/// </summary>
		public BinaryCondition(string conditionPattern, IOperand operand1, IOperand operand2) 
			: base(conditionPattern, operand1)
		{
			if (operand2 == null)
				throw new ArgumentNullException("operand2");

			Operand2 = operand2;
		}

		/// <summary>
		/// Vytvo�� bin�rn� (dvojoperandovou) podm�nku.
		/// </summary>
		public BinaryCondition(IOperand operand1, string conditionPattern, IOperand operand2)
			: this(conditionPattern, operand1, operand2)
		{
		}
		#endregion

		#region ICondition Members

		public override void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			whereBuilder.AppendFormat(ConditionPattern, Operand1.GetCommandValue(command), Operand2.GetCommandValue(command));
		}

		#endregion

		#region GetComparisonPattern
		/// <summary>
		/// Vr�t� vzor podm�nky pro b�n� porovn�n� dvou hodnot (vrac� nap�. "({0} = {1})").
		/// </summary>
		public static string GetComparisonPattern(ComparisonOperator comparisonOperator)
		{
			const string comparisonOperatorFormatPattern = "({{0}} {0} {{1}})";
			return String.Format(comparisonOperatorFormatPattern, ComparisonOperatorHelper.GetOperatorText(comparisonOperator));
		}
		#endregion
	}
}
