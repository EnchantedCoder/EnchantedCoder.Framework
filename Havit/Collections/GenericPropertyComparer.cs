using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Collections
{
	/// <summary>
	/// Comparer pro �azen� dle libobovoln� IComparable property.
	/// </summary>
	public class GenericPropertyComparer<T> : IComparer<T>
	{
		private bool sortAscending = true;
		private string sortPropertyName = String.Empty;

		/// <summary>
		/// Vytvo�� instanci compareru pro �azen� dle dan� property.
		/// </summary>
		/// <param name="sortPropertyName">n�zev property, podle kter� se m� �adit</param>
		/// <param name="ascending">true, m�-li se �adit vzestupn�, false, pokud sestupn�</param>
		public GenericPropertyComparer(String sortPropertyName, bool ascending)
		{
			this.sortPropertyName = sortPropertyName;
			this.sortAscending = ascending;
		}

		/// <summary>
		/// Porovn� dva objekty.
		/// </summary>
		/// <param name="x">prvn� objekt</param>
		/// <param name="y">druh� objekt</param>
		/// <returns>v�sledek porovn�n�</returns>
		public int Compare(T x, T y)
		{
			if (String.IsNullOrEmpty(sortPropertyName))
			{
				return 0; // shoda
			}
			IComparable ic1 = (IComparable)x.GetType().GetProperty(sortPropertyName).GetValue(x, null);
			IComparable ic2 = (IComparable)y.GetType().GetProperty(sortPropertyName).GetValue(y, null);

			if ((ic1 == null) && (ic2 == null))
			{
				return 0; // shoda
			}
			else if (ic1 == null)
			{
				return sortAscending ? -1 : 1;
			}
			else if (ic2 == null)
			{
				return sortAscending ? 1 : -1;
			}

			if (sortAscending)
			{
				return ic1.CompareTo(ic2);
			}
			else
			{
				return ic2.CompareTo(ic1);
			}
		}
	}
}
