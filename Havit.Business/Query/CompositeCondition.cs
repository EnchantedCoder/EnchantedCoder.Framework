using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Havit.Business.Query
{
	/// <summary>
	/// P�edek kompozitn�ch podm�nek.
	/// </summary>
	[Serializable]
	public abstract class CompositeCondition : Condition
	{
		private string operatorBetweenOperands = null;

		#region Conditions
		/// <summary>
		/// Podm�nky v kompozitu.
		/// </summary>
		public List<Condition> Conditions
		{
			get { return conditions; }
		}
		private List<Condition> conditions = new List<Condition>();
		#endregion

		#region Constructor
		/// <summary>
		/// Vytvo�� instanci.
		/// </summary>
		protected CompositeCondition(string operatorBetweenOperands, params Condition[] conditions)
		{
			this.operatorBetweenOperands = operatorBetweenOperands;
			this.Conditions.AddRange(conditions);
		}
		#endregion

		#region ICondition Members
		/// <summary>
		/// Poskl�d� �lensk� podm�nky. Mezi podm�nkami (operandy) je oper�tor zadan� v konstruktoru.
		/// </summary>
		public override void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder)
		{
			Debug.Assert(whereBuilder != null);

			if (Conditions.Count == 0)
			{
				return;
			}

			if (Conditions.Count == 1)
			{			
				Conditions[0].GetWhereStatement(command, whereBuilder);
				return;
			}

			whereBuilder.Append("(");
			for (int i = 0; i < Conditions.Count; i++)
			{
				if (i > 0)
				{
					whereBuilder.AppendFormat(" {0} ", operatorBetweenOperands);
				}
				Conditions[i].GetWhereStatement(command, whereBuilder);
			}
			whereBuilder.Append(")");
		}
		#endregion
	}
}
