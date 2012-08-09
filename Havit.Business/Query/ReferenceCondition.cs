using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nku testuj�c� referen�n� hodnotu (ciz� kl��).
	/// </summary>
	public static class ReferenceCondition
	{
		/// <summary>
		/// Vytvo�� podm�nku na rovnost.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, int? id)
		{
			if (id == null || id < 0)
			{
				return NullCondition.CreateIsNull(operand);
			}
			else
			{
				return NumberCondition.CreateEquals(operand, id.Value);
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku na rovnost.
		/// </summary>
		public static Condition CreateEquals(IOperand operand, BusinessObjectBase businessObject)
		{
			if (businessObject.IsNew)
			{
				throw new ArgumentException("Nelze vyhled�vat podle nov�ho neulo�en�ho objektu.", "businessObject");
			}

			return CreateEquals(operand, businessObject.ID);
		}
		
	}
}
