﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Collections
{
	/// <summary>
	/// Porovnává hodnoty vlastností dvou objektů. Názvy vlastností jsou dodány, porovnávají se v dodaném pořadí.
	/// Názvy vlastností mohou být složené: např. "Kniha.Autor.Prijmeni".
	/// Property musí implementovat IComparable.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericPropertyComparer<T> : IComparer<T>
	{
		#region Private fields
		private IList<SortItem> sortItems;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvoří instanci compareru pro řazení dle dané property.
		/// </summary>
		/// <param name="sortPropertyName">název property, podle které se má řadit</param>
		/// <param name="ascending">true, má-li se řadit vzestupně, false, pokud sestupně</param>
		[Obsolete]
		public GenericPropertyComparer(String sortPropertyName, bool ascending)
			: this(new SortItem(sortPropertyName, ascending ? SortDirection.Ascending : SortDirection.Descending))
		{			
		}

		/// <summary>
		/// Vytvoří instanci compareru pro řazení dle dané property.
		/// </summary>
		/// <param name="sortItem">Určuje parametr řazení.</param>
		public GenericPropertyComparer(SortItem sortItem): this( new SortItem[] { sortItem })
		{
		}

		/// <summary>
		/// Vytvoří instanci compareru pro řazení dle kolekce vlastností.
		/// </summary>
		/// <param name="sortItems">Určuje parametry řazení.</param>
		public GenericPropertyComparer(IList<SortItem> sortItems)
		{
			this.sortItems = sortItems;
		}
		#endregion

		#region Compare methods
		int IComparer<T>.Compare(T x, T y)
		{
			return Compare(x, y, 0);
		}

		/// <summary>
		/// Porovná vlastnosti instancí dvou objektů. Porovnávají se index-té vlastnosti uvedené ve fieldu sortItemCollection.
		/// </summary>
		/// <param name="x">První porovnávaný objekt.</param>
		/// <param name="y">Druhý porovnávaný objekt.</param>
		/// <param name="index">Index porovnávané vlastnosti.</param>
		/// <returns>-1, 0, 1 - jako Compare(T, T)</returns>
		protected int Compare(object x, object y, int index)
		{
			if (index >= sortItems.Count)
				return 0;

			/* napsáno trochu komplikovaněji - pro přehlednost */
			IComparable value1;
			IComparable value2;
			if (sortItems[index].Direction == SortDirection.Ascending)
			{
				value1 = (IComparable)GetValue(x, index);
				value2 = (IComparable)GetValue(y, index);
			}
			else
			{
				value2 = (IComparable)GetValue(x, index);
				value1 = (IComparable)GetValue(y, index);
			}

			int result = 0;

			if (value1 == null && value2 == null)
			{
				// oboji null -> stejne
				result = 0;
			}
			else if (value1 == null)
			{
				// value1 je null (value2 neni null), potom value1 < value2
				result = -1;
			}
			else if (value2 == null)
			{
				// value2 je null (value1 neni null), potom value2 < value1
				result = 1;
			}
			else /*if (value1 != null || value2 != null)*/
			{
				// ani jedno neni null -> porovname
				result = value1.CompareTo(value2);
			}

			return result == 0 ? Compare(x, y, index + 1) : result;
		}

		/// <summary>
		/// Vrátí hodnot index-té property objektu.
		/// Pokud je hodnota této property DBNull.Value, vrací null.
		/// </summary>
		private object GetValue(object obj, int index)
		{
			object result = DataBinderExt.GetValue(obj, sortItems[index].Expression);

			if (result == DBNull.Value) // pro účely srovnání budeme tvrdit, že null a DBNull.Value jsou shodné (tedy null).
			{
				return null;
			}
			return result;
		}

		#endregion
	}
}