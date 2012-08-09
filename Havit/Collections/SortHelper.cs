using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Havit.Collections
{
	/// <summary>
	/// Pomocn� t��da pro �azen�.
	/// </summary>
	public static class SortHelper
	{
		#region PropertySort
		/// <summary>
		/// Vr�t� data se�azen� podle properties v sortItemCollection.
		/// Pokud je sortItemCollection pr�zdn� kolekce, vrac� parametr data.
		/// </summary>
		/// <param name="data">Data k se�azen�.</param>
		/// <param name="sortItemCollection">Instrukce, jak se�adit.</param>
		/// <returns>Se�azen� data.</returns>
		public static IEnumerable PropertySort(IEnumerable data, SortItemCollection sortItemCollection)
		{
			if (sortItemCollection.Count == 0)
			{
				return data;
			}

			// p�ekop�rujeme data do jin� struktury
			List<object> dataList = new List<object>();
			foreach (object o in data)
			{
				dataList.Add(o);
			}
			// se�ed�me data
			dataList.Sort(new GenericPropertyComparer<object>(sortItemCollection));
			// provedeme databinding na se�azen�ch datech
			return dataList;
		}
		#endregion
	}
}
