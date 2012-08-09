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
		public static ICondition CreateEquals(Property property, int? ID)
		{
			if (ID == null || ID < 0)
				return NullCondition.CreateIsNull(property);
			else
				return NumberCondition.CreateEquals(property, ID.Value);
		}

		/// <summary>
		/// Vytvo�� podm�nku na rovnost.
		/// </summary>
		public static ICondition CreateEquals(Property property, BusinessObjectBase businessObject)
		{
			if (businessObject.IsNew)
				throw new ArgumentException("Nelze vyhled�vat podle nov�ho neulo�en�ho objektu.", "businessObject");

			return CreateEquals(property, businessObject.ID);
		}
		
	}
}
