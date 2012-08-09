using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// T��da reprezentuj�c� podm�nku o t�ech operandech.
	/// </summary>
	public class TernaryCondition : BinaryCondition
	{
		#region Protected fields
		/// <summary>
		/// T�et� operand.
		/// </summary>
		protected IOperand Operand3;
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci tern�rn� podm�nky.
		/// </summary>
		public TernaryCondition(string conditionPattern, IOperand operand1, IOperand operand2, IOperand operand3):
			base(conditionPattern, operand1, operand2)
		{
			if (operand3 == null)
				throw new ArgumentNullException("operand3");

			this.Operand3 = operand3;
		}
		#endregion

		#region ICondition Members
		/// <summary>
		/// P�id� ��st SQL p��kaz pro sekci WHERE.
		/// </summary>
		/// <param name="command"></param>
		/// <param name="whereBuilder"></param>
		public override void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			whereBuilder.AppendFormat(ConditionPattern, Operand1.GetCommandValue(command), Operand2.GetCommandValue(command), Operand3.GetCommandValue(command));
		}
		#endregion
	}
}
