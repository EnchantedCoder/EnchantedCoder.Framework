using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Security.Permissions;

namespace Havit.Business
{
	/// <summary>
	/// V�jimka reprezentuj�c� poru�en� business pravidla.
	/// </summary>
	[Serializable]
	public class ConstraintViolationException: Exception, ISerializable
	{
		#region BusinessObject
		/// <summary>
		/// BusinessObject, ve kter�m do�lo k poru�en� pravidla.
		/// </summary>
		public BusinessObjectBase BusinessObject
		{
			get { return _businessObject; }
		}
		private BusinessObjectBase _businessObject;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci v�jimky
		/// </summary>
		public ConstraintViolationException()
			: base()
		{
		}
		
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
		public ConstraintViolationException(BusinessObjectBase businessObject, string message, Exception innerException)
			: base(message, innerException)
		{
			this._businessObject = businessObject;
		}

		/// <summary>
		/// Vytvo�� instanci v�jimky.
		/// </summary>
		/// <param name="message">Popis v�jimky.</param>
		public ConstraintViolationException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Vytvo�� instanci v�jimky.
		/// </summary>
		/// <param name="message">Popis v�jimky.</param>
		/// <param name="innerException">Vno�en� v�jimka.</param>
		public ConstraintViolationException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Vytvo�� instanci v�jimky deserializac�.
		/// </summary>
		/// <param name="info">data v�jimky</param>
		/// <param name="context">context serializace</param>
		protected ConstraintViolationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			_businessObject = (BusinessObjectBase)info.GetValue("BusinessObject", typeof(BusinessObjectBase));
		}
		#endregion

		#region ISerializable
		/// <summary>
		/// Vr�t� data pro serializaci v�jimky.
		/// </summary>
		/// <param name="info">data v�jimky</param>
		/// <param name="context">context serializace</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}

			base.GetObjectData(info, context);

			info.AddValue("BusinessObject", _businessObject);
		}
		#endregion
	}
}
