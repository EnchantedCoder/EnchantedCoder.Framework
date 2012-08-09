using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Havit.Business.Query
{
	/// <summary>
	/// T��da reprezentuj�c� podm�nku o dvou operandech.
	/// </summary>
	[Serializable]
	public class BinaryCondition : UnaryCondition
	{
		#region Patterns
		/// <summary>
		/// Vzor pro podm�nku LIKE.
		/// </summary>
		public const string LikePattern = "({0} LIKE {1})";

		/// <summary>
		/// Vzor pro podm�nku rovnosti.
		/// </summary>
		public const string EqualsPattern = "({0} = {1})";

		/// <summary>
		/// Vzor pro podm�nku nerovnosti.
		/// </summary>
		public const string NotEqualsPattern = "({0} <> {1})";
		#endregion

		#region Operand2
		/// <summary>
		/// Druh� operand.
		/// </summary>
		protected IOperand Operand2
		{
			get { return _operand2; }
			set { _operand2 = value; }
		}
		private IOperand _operand2; 
		#endregion

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

		#region GetWhereStatement
		/// <summary>
		/// P�id� ��st SQL p��kaz pro sekci WHERE
		/// </summary>
		/// <param name="command"></param>
		/// <param name="whereBuilder"></param>
		public override void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}

			if (whereBuilder == null)
			{
				throw new ArgumentNullException("whereBuilder");
			}

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
			return String.Format(CultureInfo.InvariantCulture, comparisonOperatorFormatPattern, ComparisonOperatorHelper.GetOperatorText(comparisonOperator));
		}
		#endregion
	}
}
