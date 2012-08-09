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
			if (id <= 0)
			{
				throw new ArgumentException("ID objektu mus� b�t kladn� ��slo nebo null.", "id");
			}

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

		/// <summary>
		/// Vytvo�� podm�nku na rovnost dvou operand�.
		/// </summary>
		public static Condition CreateEquals(IOperand operand1, IOperand operand2)
		{
			return NumberCondition.CreateEquals(operand1, operand2);
		}

		/// <summary>
		/// Vytvo�� podm�nku existence hodnoty v poli integer�.
		/// </summary>
		public static Condition CreateIn(IOperand operand, int[] ids)
		{
			return new BinaryCondition("{0} IN (SELECT Value FROM dbo.IntArrayToTable({1}))", operand, SqlInt32ArrayOperand.Create(ids));
		}
		
	}
}
