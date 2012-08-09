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
		public static Condition CreateEquals(PropertyInfo property, int? id)
		{
			if (id == null || id < 0)
			{
				return NullCondition.CreateIsNull(property);
			}
			else
			{
				return NumberCondition.CreateEquals(property, id.Value);
			}
		}

		/// <summary>
		/// Vytvo�� podm�nku na rovnost.
		/// </summary>
		public static Condition CreateEquals(PropertyInfo property, BusinessObjectBase businessObject)
		{
			if (businessObject.IsNew)
			{
				throw new ArgumentException("Nelze vyhled�vat podle nov�ho neulo�en�ho objektu.", "businessObject");
			}

			return CreateEquals(property, businessObject.ID);
		}
		
	}
}
