using System;

namespace Havit.Collections
{
	/// <summary>
	/// Abstraktn� t��da pro strong-typed collections s �azen�m dle jedn� libovoln� property.
	/// </summary>
	/// <remarks>
	/// Property, podle kter� m� b�t �azeno, mus� implementovat <see cref="System.IComparable"/>
	/// </remarks>
	[Obsolete]
	public abstract class SortableCollectionBase : System.Collections.CollectionBase
	{
		/// <summary>
		/// Se�ad� prvky dle po�adovan� property, kter� implementuje IComparable.
		/// </summary>
		/// <param name="propertyName">property, podle kter� se m� �adit</param>
		/// <param name="ascending">true, pokud se m� �adit vzestupn�, false, pokud sestupn�</param>
		public virtual void Sort(string propertyName, bool ascending) 
		{
			InnerList.Sort(new GenericPropertySort(propertyName, ascending));
		}

		/// <summary>
		/// Vr�t� polohu prvku v se�azen� collection.
		/// </summary>
		/// <param name="searchedValue">hodnota property prvku</param>
		/// <param name="propertyName">jm�no property</param>
		/// <returns>poloha prvku</returns>
		public int IndexOf(string propertyName, object searchedValue) 
		{
			for (int i=0; i < InnerList.Count; i++)
			{
				if (((IComparable)InnerList[i].GetType().GetProperty(propertyName).GetValue(InnerList[i], null)).CompareTo(searchedValue) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Comparer pro �azen� dle libobovoln� IComparable property.
		/// </summary>
		internal class GenericPropertySort : System.Collections.IComparer
		{
			private bool sortAscending = true;
			private string sortPropertyName = String.Empty;
		
			/// <summary>
			/// Vytvo�� instanci compareru pro �azen� dle dan� property.
			/// </summary>
			/// <param name="sortPropertyName">n�zev property, podle kter� se m� �adit</param>
			/// <param name="ascending">true, m�-li se �adit vzestupn�, false, pokud sestupn�</param>
			public GenericPropertySort(String sortPropertyName, bool ascending) 
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
			public int Compare(object x, object y)
			{
				if ((sortPropertyName == null) || (sortPropertyName.Length == 0))
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
}
