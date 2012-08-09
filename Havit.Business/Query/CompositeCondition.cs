using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Havit.Business.Query
{
	/// <summary>
	/// P�edek kompozitn�ch podm�nek.
	/// </summary>
	[Serializable]
	public abstract class CompositeCondition : List<ICondition>, ICondition
	{
		string operatorBetweenOperands = null;

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci.
		/// </summary>
		public CompositeCondition(string operatorBetweenOperands, params ICondition[] conditions)
		{
			this.operatorBetweenOperands = operatorBetweenOperands;
			this.AddRange(conditions);
		}
		#endregion

		#region ICondition Members
		/// <summary>
		/// Poskl�d� �lensk� podm�nky. Mezi podm�nkami (operandy) je oper�tor zadan� v konstruktoru.
		/// </summary>
		public virtual void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			if (Count == 0)
				return;

			if (Count == 1)
			{			
				this[0].GetWhereStatement(command, whereBuilder);
				return;
			}

			whereBuilder.Append("(");
			for (int i = 0; i < Count; i++)
			{
				if (i > 0)
					whereBuilder.AppendFormat(" {0} ", operatorBetweenOperands);
				this[i].GetWhereStatement(command, whereBuilder);
			}
			whereBuilder.Append(")");
		}
		#endregion
	}
}
