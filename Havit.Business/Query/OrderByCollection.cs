using System;
using System.Collections.Generic;
using System.Text;
using Havit.Collections;

namespace Havit.Business.Query
{
	/// <summary>
	/// Kolekce polo�ek SortItem.
	/// Ur�eno pro polo�ky ORDER BY skl�da�e SQL dotazu (QueryParameters).
	/// </summary>
	[Serializable]
	public class OrderByCollection: Havit.Collections.SortItemCollection
	{
		/// <summary>
		/// P�id� na konec kolekce polo�ku pro vzestupn� �azen�.
		/// </summary>
		public void Add(FieldPropertyInfo propertyInfo)
		{
			Add(propertyInfo, SortDirection.Ascending);
		}

		/// <summary>
		/// P�id� na konec kolekce polo�ku pro �azen�.
		/// </summary>
		public void Add(FieldPropertyInfo propertyInfo, SortDirection direction)
		{
			Add(new FieldPropertySortItem(propertyInfo, direction));						
		}

		/// <summary>
		/// P�id� do kolekce polo�ku pro vzestun� �azen�.
		/// </summary>
		public void Insert(int index, FieldPropertyInfo propertyInfo)
		{
			Insert(index, propertyInfo, SortDirection.Ascending);
		}

		/// <summary>
		/// P�id� do kolekce polo�ku pro �azen�.
		/// </summary>
		public void Insert(int index, FieldPropertyInfo propertyInfo, SortDirection direction)
		{
			Insert(index, new FieldPropertySortItem(propertyInfo, direction));			
		}
	}
}
