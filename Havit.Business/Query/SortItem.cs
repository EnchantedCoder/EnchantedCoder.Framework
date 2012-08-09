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

		/*
		/// <summary>
		/// Vytvo�� polo�du �azen� podle fieldName, vzestupn� �azen�.
		/// </summary>
		public SortItem(string fieldName)
			: this(fieldName, SortDirection.Ascending)
		{
#warning Chceme umo�nit stringov� zad�n� fieldName?
		}
		 */

		/// <summary>
		/// Vytvo�� polo�du �azen� podle fieldName a dan�ho po�ad�.
		/// </summary>
		protected SortItem(string fieldName, SortDirection direction)
			: this()
		{
#warning Chceme umo�nit stringov� zad�n� fieldName?
			this.fieldName = fieldName;
			this.direction = direction;
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle sloupce, vzestupn� po�ad�.
		/// </summary>
		public SortItem(FieldPropertyInfo property)
			: this(property.FieldName, SortDirection.Ascending)
		{
		}

		/// <summary>
		/// Vytvo�� polo�du �azen� podle sloupce a dan�ho po�ad�.
		/// </summary>
		public SortItem(FieldPropertyInfo property, SortDirection direction)
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
#warning Neb�l by SortItem pracovat je�t�  s PropertyInfo, sp� ne� se stringov�m FieldName?
			get { return fieldName; }
			set { fieldName = value; }
		}
		private string fieldName;

		/// <summary>
		/// Sm�r �azen�.
		/// </summary>
		public SortDirection Direction
		{
			get { return direction; }
			set { direction = value; }
		}
		private SortDirection direction = SortDirection.Ascending;

		#endregion

		#region GetSqlOrderBy
		/// <summary>
		/// Vr�t� v�raz pro �azen�.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
		public virtual string GetSqlOrderBy()
		{
			string result = fieldName;
			if (direction == SortDirection.Descending)
			{
				result = result + " DESC";
			}
			return result;
		}
		#endregion

	}
}
