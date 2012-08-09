using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Havit.Business
{
	/// <summary>
	/// Kolekce objekt� implementuj�c�ch IProperty.
	/// P�i opakovan�m p�id�n� property do kolekce se nic nestane (tj. 
	/// property nebude do kolekce p�id�na podruh� a nedojde k chyb�).
	/// </summary>
	[Serializable]
	public class PropertyCollection : Collection<IProperty>
	{
		/// <summary>
		/// P�id� prvek do kolekce, pokud v kolekci ji� nen�.
		/// </summary>
		protected override void InsertItem(int index, IProperty item)
		{
			if (this.Contains(item))
				return;

			base.InsertItem(index, item);
		}
	}
}
