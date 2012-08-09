﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Třída pro objekt, který nese kolekci property BusinessObjectu.
	/// </summary>
	/// <typeparam name="CollectionType">typ kolekce, jíž je CollectionPropertyHolder nosičem</typeparam>
	/// <typeparam name="BusinessObjectType">typ prvku kolekce</typeparam>
	[Serializable]
	public class CollectionPropertyHolder<CollectionType, BusinessObjectType>: PropertyHolderBase
		where BusinessObjectType : BusinessObjectBase
		where CollectionType: BusinessObjectCollection<BusinessObjectType, CollectionType>, new()		
	{
		#region Constructors
		/// <summary>
		/// Založí instanci CollectionPropertyHolderu.
		/// </summary>
		/// <param name="owner">objekt, kterému CollectionPropertyHolder patří</param>
		public CollectionPropertyHolder(BusinessObjectBase owner)
			: base(owner)
		{
		}
		#endregion

		#region Value
		/// <summary>
		/// Hodnota, kterou CollectionPropertyHolder nese.
		/// </summary>
		public CollectionType Value
		{
			get
			{
				CheckInitialization();
				return _value;
			}
		}
		private CollectionType _value;
		#endregion

		#region Initialize
		/// <summary>
		/// Inicializuje obsaženou kolekci.
		/// </summary>
		public void Initialize()
		{
			if (_value == null)
			{
				// první inicializace
				_value = new CollectionType();
				_value.AllowDuplicates = false;
				IsInitialized = true;
				_value.CollectionChanged += delegate(object sender, EventArgs e)
				{
					IsDirty = true;
				};
			}
		}
		#endregion
	}
}
