using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky testuj�c� null hodnoty.
	/// </summary>
	public static class NullCondition
	{
		#region CreateIsNull
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnotu na NULL.
		/// </summary>
		public static Condition CreateIsNull(IOperand operand)
		{
			return new UnaryCondition(UnaryCondition.IsNullPattern, operand);
		} 
		#endregion

		#region CreateIsNotNull
		/// <summary>
		/// Vytvo�� podm�nku testuj�c� hodnotu na NOT NULL.
		/// </summary>
		public static Condition CreateIsNotNull(IOperand operand)
		{
			return new UnaryCondition(UnaryCondition.IsNotNullPattern, operand);
		} 
		#endregion
	}
}
