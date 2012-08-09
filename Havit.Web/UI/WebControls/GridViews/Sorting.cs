using System;
using Havit.Collections;
using Havit.Business.Query;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// T��da Sorting zaji��uje pr�ci s �azen�m polo�ek.
	/// </summary>
	/// 
	[Serializable]
	public class Sorting
	#warning Neum� rozumn� sestupn� �azen�
	{
		private SortItemCollection sortItems = new SortItemCollection();
		internal SortItemCollection SortItems
		{
			get
			{
				return sortItems;
			}
		}

		/// <summary>
		/// Vypr�zdn� seznam polo�ek �azen�.
		/// </summary>
		public void ClearSelection()
		{
			SortItems.Clear();
		}

		/// <summary>
		/// Vyhled� index polo�ky v seznamu polo�ek �azen�. Bere se ohled jen na Expression.
		/// </summary>
		protected int IndexOf(string expression)
		{
			for (int i = 0; i < SortItems.Count; i++)
				if (SortItems[i].Expression == expression)
					return i;
			return -1;
		}

		/// <summary>
		/// Odstran� polo�ku �azen� ze seznamu, pokud existuje.
		/// </summary>
		/// <param name="expression"></param>
		protected void RemoveField(string expression)
		{
			int i = IndexOf(expression);
			if (i >= 0)
				SortItems.RemoveAt(i);
		}

		/// <summary>
		/// P�id� polo�ku �azen� do seznamu na prvn� pozici.
		/// Pokud ji� v seznamu existuje, tak pokud existuje na prvn� pozici (tj. na pozici 0),
		/// zm�n� sm�r �azen�. Pokud existuje na jin� pozici, odstran� polo�ku ze seznamu
		/// a vlo�� ji na prvn� pozici (tj. zv��� se "priorita" a �ad� se vzestupn�.
		/// </summary>
		/// <param name="expression"></param>
		public void Add(string expression)
		{
			int i = IndexOf(expression);
			if (i == 0)
				Add(expression, (SortItems[0].Direction == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending);
			else
				Add(expression, SortDirection.Ascending);
		}

        /// <summary>
        /// P�id� polo�ky �azen� do seznamu na prvn� pozice (prvn� polo�ka kolekce bude na za��tku).
        /// Pokud ji� polo�ky v seznamu existuj� na prvn�ch pozic�ch zm�n� sm�r �azen�.
        /// Pokud existuje na jin� pozici, odstran� polo�ku ze seznamu
        /// a vlo�� ji na prvn� pozici (tj. zv��� se "priorita" a �ad� se vzestupn�.
        /// </summary>
		public void Add(string[] expression)
        {
            // �ad�me vzestupn�
			SortDirection newDirection = SortDirection.Ascending;

			if (SortItems.Count >= expression.Length)
            {
                // �ad�me sestupn�, ale d�le to v�t�inou zp�t zm�n�me
				newDirection = SortDirection.Descending;

                // projdeme v�echny polo�ky
				for (int i = 0; i < expression.Length; i++)
                {
                    // a testujeme, jestli jsou na za��tku seznamu a �azen� vzestupn�
                    // pokud ano, ned�l�me nic (z�stane sestupn� �azen�
                    // pokud ne, zm�n�me �azen� a kon��me test
					if (expression[i] != SortItems[i].Expression || SortItems[i].Direction != SortDirection.Ascending)
                    {
						newDirection = SortDirection.Ascending;
                        break;
                    }
                }
            }

            // p�id�me postupn� polo�ky �azen� (odzadu, proto�e posledn� p�idan� bude m�t nejvy��� prioritu)
			for (int i = expression.Length - 1; i >= 0; i--)
				Add(expression[i], newDirection);
        }

        /// <summary>
        /// P�id� polo�ku �azen� do seznamu na prvn� pozici. 
        /// Pokud ji� v seznamu existuje, je p�vodn� polo�ka odstran�na.
        /// </summary>
		/// <param name="expression"></param>
        /// <param name="sortDirection"></param>
		protected void Add(string expression, SortDirection sortDirection)
		{
			RemoveField(expression);
			SortItems.Insert(0, new SortItem(expression, sortDirection));
		}

		/// <summary>
		/// Prozkoum� expression od u�ivatele a rozebere ji na jednotliv� polo�ky �azen�.
		/// </summary>
		public static string[] ParseSortExpression(string expression)
		{
			string[] sortExpressionItems = expression.Split(',');

			// odstranime pripadne mezery
			for (int i = 0; i < sortExpressionItems.Length; i++)
				sortExpressionItems[i] = sortExpressionItems[i].Trim();

			return sortExpressionItems;
		}
	}
}