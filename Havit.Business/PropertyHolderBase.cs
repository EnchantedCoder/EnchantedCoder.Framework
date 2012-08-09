using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// P�edek generick�ho typu <see cref="PropertyHolder{T}"/>. 
	/// Pot�ebujeme kolekci PropertyHolder� a kolekci generick�ch typ� nelze ud�lat.
	/// </summary>
	public abstract class PropertyHolderBase
	{
		#region Owner
		/// <summary>
		/// Objekt, kter�mu property pat��.
		/// </summary>
		protected BusinessObjectBase Owner
		{
			get { return _owner; }
		}
		private BusinessObjectBase _owner;
		#endregion

		#region IsDirty
		/// <summary>
		/// Indikuje, zda do�lo ke zm�n� hodnoty.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
			internal set
			{
				_isDirty = value;
				if (_isDirty)
					Owner.IsDirty = true;
			}
		}
		private bool _isDirty = false;
		#endregion

		#region IsInitialized
		/// <summary>
		/// Indikuje, zda je hodnota property nastavena.
		/// </summary>
		public bool IsInitialized
		{
			get
			{
				return _isInitialized;
			}
			protected set
			{
				_isInitialized = value;
			}
		}
		private bool _isInitialized = false;
		#endregion

		#region CheckInitialization
		/// <summary>
		/// Pokud nebyla hodnota PropertyHolderu nastavena, vyhod� InvalidOperationException.
		/// Pokud byla hodnota PropertyHolderu nastavena, neud�l� nic (projde).
		/// </summary>
		protected void CheckInitialization()
		{
			if (!_isInitialized)
			{
				throw new InvalidOperationException("Hodnota nebyla inicializov�na.");
			}
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Zalo�� instanci PropertyHolderu.
		/// </summary>
		/// <param name="owner">objekt, kter�mu PropertyHolder pat��</param>
		public PropertyHolderBase(BusinessObjectBase owner)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}

			this._owner = owner;

			// zaregistrujeme se majiteli do kolekce PropertyHolders
			owner.RegisterPropertyHolder(this);
		}
		#endregion
	}
}
