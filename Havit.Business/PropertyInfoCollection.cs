using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Havit.Business
{
	/// <summary>
	/// Kolekce objekt� PropertyInfo.<br/>
	/// P�i opakovan�m p�id�n� property do kolekce se nic nestane (tj. 
	/// property nebude do kolekce p�id�na podruh� a nedojde k chyb�).
	/// </summary>
	[Serializable]
	public class PropertyInfoCollection : Collection<PropertyInfo>
	{
		#region Constructors
		/// <summary>
		/// Vyvo�� pr�zdnou kolekci.
		/// </summary>
		public PropertyInfoCollection()
		{
		}

		/// <summary>
		/// Vytvo�� kolekci a vlo�� do n� zadan� objekty PropertyInfo.
		/// </summary>
		public PropertyInfoCollection(params PropertyInfo[] properties)
			: this()
		{
			foreach (PropertyInfo propertyInfo in properties)
				this.Add(propertyInfo);
		} 
		#endregion

		#region InsertItem
		/// <summary>
		/// P�id� prvek do kolekce, pokud v kolekci ji� nen�.
		/// </summary>
		protected override void InsertItem(int index, PropertyInfo item)
		{
			if (this.Contains(item))
				return;

			base.InsertItem(index, item);
		} 
		#endregion
	}
}
