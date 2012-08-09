using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Podm�nka, kter� je v�dy vyhodnocena stejn�.
	/// </summary>
	internal class StaticCondition: Condition
	{
		private const string TrueConditionText = "(0=0)";
		private const string FalseConditionText = "(0=1)";

		private string _conditionText;

		#region Constructor
		private StaticCondition(string conditionText)
		{
			_conditionText = conditionText;
		}
		#endregion

		#region GetWhereStatement
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

			whereBuilder.Append(_conditionText);
		}
		#endregion

		#region IsEmptyCondition
		public override bool IsEmptyCondition()
		{
			return false;
		} 
		#endregion

		#region CreateTrue (static)
		/// <summary>
		/// Vytv��� instanci podm�nky, kter� je v�dy true.
		/// </summary>
		internal static Condition CreateTrue()
		{
			return new StaticCondition(TrueConditionText);
		}
		#endregion

		#region CreateFalse (static)
		/// <summary>
		/// Vytv��� instanci podm�nky, kter� je v�dy false.
		/// </summary>
		internal static Condition CreateFalse()
		{
			return new StaticCondition(FalseConditionText);
		}
		#endregion
	}
}
