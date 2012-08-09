using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// T��da reprezentuj�c� podm�nku o jednom operandu.
	/// </summary>
	public class UnaryCondition : ICondition
	{
		#region Patterns
		/// <summary>
		/// Vzor pro podm�nku IS NULL.
		/// </summary>
		public const string IsNullPattern = "({0} IS NULL)";

		/// <summary>
		/// Vzor pro podm�nku IS NOT NULL.
		/// </summary>
		public const string IsNotNullPattern = "({0} IS NOT NULL)";
		#endregion

		#region Protected fields
		/// <summary>
		/// Operand.
		/// </summary>
		protected IOperand Operand1;

		/// <summary>
		/// Vzor podm�nky SQL dotazu.
		/// N�sledn� je form�tov�n operandem (v potomc�ch operandy).
		/// </summary>
		protected string ConditionPattern;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci un�rn� podm�nky.
		/// </summary>
		/// <param name="conditionPattern"></param>
		/// <param name="operand"></param>
		public UnaryCondition(string conditionPattern, IOperand operand)
		{
			if (conditionPattern == null)
			{
				throw new ArgumentNullException("conditionPattern");
			}

			if (operand == null)
			{
				throw new ArgumentNullException("operand");
			}

			Operand1 = operand;
			ConditionPattern = conditionPattern;
		}
		#endregion

		#region ICondition Members
		/// <summary>
		/// P�id� ��st SQL p��kaz pro sekci WHERE.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="whereBuilder"></param>
		public virtual void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			whereBuilder.AppendFormat(ConditionPattern, Operand1.GetCommandValue(command));
		}
		#endregion
	}
}
