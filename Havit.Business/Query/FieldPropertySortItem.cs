using System;
using System.Collections.Generic;
using System.Text;
using Havit.Collections;

namespace Havit.Business.Query
{
	/// <summary>
	/// Reprezentuje polo�ku �azen�.
	/// </summary>
	[Serializable]
	public class FieldPropertySortItem: SortItem
	{
		#region Constructor (obsolete)
		/// <summary>
		/// Vytvo�� nenastavenou polo�ku �azen� podle.
		/// </summary>
		[Obsolete]
		public FieldPropertySortItem()
			: base()
		{
		}
		
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� polo�ku �azen� podle sloupce, vzestupn� po�ad�.
		/// </summary>
		public FieldPropertySortItem(FieldPropertyInfo property)
			: this(property, SortDirection.Ascending)
		{
		}
		
		/// <summary>
		/// Vytvo�� polo�ku �azen� podle sloupce a dan�ho po�ad�.
		/// </summary>
		public FieldPropertySortItem(FieldPropertyInfo property, SortDirection direction)
			: base(property.FieldName, direction)
		{
		}
		#endregion
	}
}
