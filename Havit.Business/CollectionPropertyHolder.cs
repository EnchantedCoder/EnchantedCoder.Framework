using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// T��da pro objekt, kter� nese kolekci property BusinessObjectu.
	/// </summary>
	/// <typeparam name="CollectionType">typ kolekce, j� je CollectionPropertyHolder nosi�em</typeparam>
	/// <typeparam name="BusinessObjectType">typ prvku kolekce</typeparam>
	public class CollectionPropertyHolder<CollectionType, BusinessObjectType>: PropertyHolderBase
		where BusinessObjectType : BusinessObjectBase
		where CollectionType: BusinessObjectCollection<BusinessObjectType>, new()		
	{
		#region Constructors
		/// <summary>
		/// Zalo�� instanci CollectionPropertyHolderu.
		/// </summary>
		/// <param name="owner">objekt, kter�mu CollectionPropertyHolder pat��</param>
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
				InitializationCheck();
				return _value;
			}
		}
		private CollectionType _value;
		#endregion

		#region Initialize
		/// <summary>
		/// Inicializuje obsa�enou kolekci.
		/// </summary>
		public void Initialize()
		{
			_value = new CollectionType();
			IsInitialized = true;
			_value.CollectionChanged += delegate(object sender, EventArgs e)
			{
				IsDirty = true;
			};
		}
		#endregion
	}
}
