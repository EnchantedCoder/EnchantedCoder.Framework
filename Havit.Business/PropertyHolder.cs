using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class PropertyHolder<T> : PropertyHolderBase
	{
		#region owner
		/// <summary>
		/// Objekt, kter�mu property pat��.
		/// </summary>
		private BusinessObjectBase owner;
#warning Pro� nen� Owner jako protected/internal propety u� v PropertyHolderBase?
		#endregion

		#region Constructors
		/// <summary>
		/// Zalo�� instanci PropertyHolderu.
		/// </summary>
		/// <param name="owner">objekt, kter�mu PropertyHolder pat��</param>
		public PropertyHolder(BusinessObjectBase owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}

			this.owner = owner;

			// zaregistrujeme se majiteli
			owner.RegisterPropertyHolder(this);
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
				if (!_isInitialized)
				{
					throw new InvalidOperationException("Hodnota nebyla inicializov�na.");
				}
				return _value;
			}
			set
			{
				if (!Object.Equals(_value, value))
				{
					_isDirty = true;
					owner.SetDirty();
				}

				_isInitialized = true;
				
				// nen� pod podm�nkou !Object.Equals(), proto�e m��e doj�t k ulo�en� jin� instance, kter� je sice nyn� rovna,
				// ale pokud by se s n� d�le pracovalo, mohou se u� tyto instance rozch�zet
				// (nastaven� IsDirty mus�me v tom p��pad� sledovat p�es odb�r ud�losti "zm�na")
				_value = value; 
			}
		}
		private T _value;
		#endregion

		#region IsDirty
		/// <summary>
		/// Indikuje, zda do�lo ke zm�n� hodnoty.
		/// </summary>
		public bool IsDirty
		{
#warning Pro� nen� u� v PropertyHolderBase?
			get
			{
				return _isDirty;
			}
		}
		private bool _isDirty = false;
		#endregion

		/// <summary>
		/// Indikuje, zda je hodnota property nastavena.
		/// </summary>
		public bool IsInitialized
		{
#warning Pro� nen� u� v PropertyHolderBase? Takto se nem��u objektu zeptat, jestli m� inicializovan� v�echny property...
			get
			{
				return _isInitialized;
			}
		}
		private bool _isInitialized = false;


		internal override void ClearDirty()
		{
#warning Pro� nesta�� m�sto ClearDirty() jen set-accesor?
			_isDirty = false;
		}
	}
}
