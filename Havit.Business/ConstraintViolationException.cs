using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// V�jimka reprezentuj�c� poru�en� business pravidla.
	/// </summary>
	public class ConstraintViolationException: ApplicationException
	{
		/// <summary>
		/// Vytvo�� instanci v�jimky.
		/// </summary>
		/// <param name="businessObject">Business object, ve kter�m do�lo k poru�en� pravidla.</param>
		/// <param name="message">Popis v�jimky.</param>
		public ConstraintViolationException(BusinessObjectBase businessObject, string message)
			: this(businessObject, message, null)
		{
		}

		/// <summary>
		/// Vytvo�� instanci v�jimky.
		/// </summary>
		/// <param name="businessObject">Business object, ve kter�m do�lo k poru�en� pravidla.</param>
		/// <param name="message">Popis v�jimky.</param>
		/// <param name="innerException">Vno�en� v�jimka.</param>
		public ConstraintViolationException(BusinessObjectBase businessObject, string message, Exception innerException): base(message, innerException)
		{
			this.businessObject = businessObject;
		}

		/// <summary>
		/// BusinessObject, ve kter�m do�lo k poru�en� pravidla.
		/// </summary>
		public BusinessObjectBase BusinessObject
		{
			get { return businessObject; }
		}
		private BusinessObjectBase businessObject;

	}
}
