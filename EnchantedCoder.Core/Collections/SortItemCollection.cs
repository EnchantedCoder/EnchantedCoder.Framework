using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

namespace EnchantedCoder.Collections
{
	/// <summary>
	/// Kolekce objektů třídy SortItem.
	/// </summary>
	[Serializable]
	[DataContract]
	public class SortItemCollection : Collection<SortItem>
	{
	}
}
