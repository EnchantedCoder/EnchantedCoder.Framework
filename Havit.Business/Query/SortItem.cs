using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Havit.Business.Query
{
	/// <summary>
	/// Reprezentuje polo�ku �azen�.
	/// </summary>
	[Serializable]	
	public class SortItem
	{
		#region Constructors

		/// <summary>
		/// Vytvo�� pr�zdnou instanci po�ad�.
		/// </summary>
		public SortItem()
		{
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle fieldName, vzestupn� �azen�.
		/// </summary>
		public SortItem(string fieldName)
			: this(fieldName, ListSortDirection.Ascending)
		{			
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle fieldName a dan�ho po�ad�.
		/// </summary>
		public SortItem(string fieldName, ListSortDirection direction)
			: this()
		{
			this.fieldName = fieldName;
			this.direction = direction;
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle sloupce, vzestupn� po�ad�.
		/// </summary>
		public SortItem(PropertyInfo property)
			: this(property.FieldName, ListSortDirection.Ascending)
		{
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle sloupce a dan�ho po�ad�.
		/// </summary>
		public SortItem(PropertyInfo property, ListSortDirection direction)
			: this(property.FieldName, direction)
		{
		}

		#endregion

		#region Properties
		/// <summary>
		/// N�zev sloupce, dle kter�ho se �ad�.
		/// </summary>
		public string FieldName
		{
			get { return fieldName; }
			set { fieldName = value; }
		}
		private string fieldName;

		/// <summary>
		/// Sm�r �azen�.
		/// </summary>
		public ListSortDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		private ListSortDirection direction = ListSortDirection.Ascending;

		#endregion

		#region GetSqlOrderBy
		/// <summary>
		/// Vr�t� v�raz pro �azen�.
		/// </summary>
		public virtual string GetSqlOrderBy()
		{
			string result = fieldName;
			if (direction == ListSortDirection.Descending)
			{
				result = result + " DESC";
			}
			return result;
		}
		#endregion

	}
}
