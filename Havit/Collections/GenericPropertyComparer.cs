using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace Havit.Collections
{
	/// <summary>
	/// Porovn�v� hodnoty vlastnost� dvou objekt�. N�zvy vlastnost� jsou dod�ny, porovn�vaj� se v dodan�m po�ad�.
	/// N�zvy vlastnost� mohou b�t slo�en�: nap�. "Kniha.Autor.Prijmeni".
	/// Property mus� implementovat IComparable.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class GenericPropertyComparer<T> : IComparer<T>
	{
		#region Private fields
		private IList<SortItem> sortItems;
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� instanci compareru pro �azen� dle dan� property.
		/// </summary>
		/// <param name="sortPropertyName">n�zev property, podle kter� se m� �adit</param>
		/// <param name="ascending">true, m�-li se �adit vzestupn�, false, pokud sestupn�</param>
		[Obsolete]
		public GenericPropertyComparer(String sortPropertyName, bool ascending)
			: this(new SortItem(sortPropertyName, ascending ? SortDirection.Ascending : SortDirection.Descending))
		{			
		}

		/// <summary>
		/// Vytvo�� instanci compareru pro �azen� dle dan� property.
		/// </summary>
		/// <param name="sortItem">Ur�uje parametr �azen�.</param>
		public GenericPropertyComparer(SortItem sortItem): this( new SortItem[] { sortItem })
		{
		}

		/// <summary>
		/// Vytvo�� instanci compareru pro �azen� dle kolekce vlastnost�.
		/// </summary>
		/// <param name="sortItems">Ur�uje parametry �azen�.</param>
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
		/// Porovn� vlastnosti instanc� dvou objekt�. Porovn�vaj� se index-t� vlastnosti uveden� ve fieldu sortItemCollection.
		/// </summary>
		/// <param name="x">Prvn� porovn�van� objekt.</param>
		/// <param name="y">Druh� porovn�van� objekt.</param>
		/// <param name="index">Index porovn�van� vlastnosti.</param>
		/// <returns>-1, 0, 1 - jako Compare(T, T)</returns>
		protected int Compare(object x, object y, int index)
		{
			if (index >= sortItems.Count)
				return 0;

			/* naps�no trochu komplikovan�ji - pro p�ehlednost */
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
		/// Vr�t� hodnot index-t� property objektu.
		/// Pokud je hodnota t�to property DBNull.Value, vrac� null.
		/// </summary>
		private object GetValue(object obj, int index)
		{
			object result = DataBinder.Eval(obj, sortItems[index].Expression);
			if (result == DBNull.Value)
			{
				return null;
			}
			return result;
		}

		#endregion
	}
}