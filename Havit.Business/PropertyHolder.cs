using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// T��da pro objekt, kter� nese hodnotu a vlastnosti jednotliv� property BusinessObjectu.
	/// </summary>
	/// <typeparam name="T">typ property, j� je PropertyHolder nosi�em</typeparam>
	public class PropertyHolder<T> : PropertyHolderBase
	{
		#region Constructors
		/// <summary>
		/// Zalo�� instanci PropertyHolderu.
		/// </summary>
		/// <param name="owner">objekt, kter�mu PropertyHolder pat��</param>
		public PropertyHolder(BusinessObjectBase owner)
			: base(owner)
		{
		}
		#endregion

		#region Value
		/// <summary>
		/// Hodnota, kterou PropertyHolder nese.
		/// </summary>
		public T Value
		{
			get
			{
				InitializationCheck();
				return _value;
			}
			set
			{
				if (!Object.Equals(_value, value))
				{
					IsDirty = true;
					
				}

				IsInitialized = true;

				// nen� pod podm�nkou !Object.Equals(), proto�e m��e doj�t k ulo�en� jin� instance, kter� je sice nyn� rovna,
				// ale pokud by se s n� d�le pracovalo, mohou se u� tyto instance rozch�zet
				// (nastaven� IsDirty mus�me v tom p��pad� sledovat p�es odb�r ud�losti "zm�na")
				_value = value;
			}
		}
		private T _value;
		#endregion
	}
}
