using System;
using Havit.Collections;
using System.Web;

namespace Havit.Web.UI.WebControls
{
	/// <summary>
	/// T��da SortExpressions zaji��uje pr�ci s �azen�m polo�ek.
	/// </summary>
	/// 
	[Serializable]
	public class SortExpressions
	{
		/// <summary>
		/// Polo�ky �azen�.
		/// </summary>
		public SortItemCollection SortItems
		{
			get
			{
				return sortItems;
			}
		}
		private SortItemCollection sortItems = new SortItemCollection();

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
			{
				if (SortItems[i].Expression == expression)
				{
					return i;
				}
			}
			return -1;
		}

		/// <summary>
		/// Odstran� polo�ku �azen� ze seznamu, pokud existuje.
		/// </summary>
		/// <param name="expression"></param>
		protected void RemoveExpression(string expression)
		{
			int i = IndexOf(expression);
			if (i >= 0)
			{
				SortItems.RemoveAt(i);
			}
		}

		/// <summary>
		/// Rozebere sortExpression a p�id� polo�ky �azen� na prvn� m�sto.
		/// Pokud polo�ky po�ad� ji� jsou na za��tku seznamu, je jim oto�en sm�r �azen�.
		/// </summary>
		/// <param name="sortExpression">
		/// V�raz pro �azen�. Seznam odd�len� ��rkami. Sestupn� sm�r se vyj�d�� dopln�n�m mezery a DESC.<br/>
		/// Nap�. "Nazev", "Prijmeni, Jmeno", "Vek DESC", "Vek desc".
		/// </param>
		public void AddSortExpression(string sortExpression)
		{
			string[] expressions = sortExpression.Split(',');

			if (expressions.Length == 0)
			{
				// pokud nic nep�id�v�me, kon��me.
				return;
			}

			SortItemCollection newItems = new SortItemCollection();
			for (int i = 0; i < expressions.Length; i++)
			{
				string trimmedExpression = expressions[i].Trim();
				if (trimmedExpression.ToLower().EndsWith(" desc"))
				{
					// pokud m�me �edit sestupn�, vytvo��me polo�ku pro sestupn� �azen�
					newItems.Add(new SortItem(trimmedExpression.Substring(0, trimmedExpression.Length - 5), SortDirection.Descending));
				}
				else
				{
					// jinak vytvo��me polo�ku pro vzestupn� �azen�.
					newItems.Add(new SortItem(trimmedExpression, SortDirection.Ascending));
				}
			}

			if (StartsWith(newItems))
			{
				// pokud u� polo�ky v seznamu jsou a jsou na za��tku, oto��me jim sm�r �azen�.				
				for (int i = 0; i < newItems.Count; i++)
				{
					sortItems[i].Direction = (sortItems[i].Direction == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
				}
			}
			else
			{
				// jinak je p�id�me na za��tek seznamu (jsou-li v seznamu d�le, vyhod�me je a d�me na za��tek)
				for (int i = 0; i < newItems.Count; i++)
				{
					RemoveExpression(newItems[i].Expression);
					SortItems.Insert(i, newItems[i]);
				}
			}
		}

		/// <summary>
		/// Vrac� true, pokud kolekce za��n� stejn�mi polo�kami, jako jsou zde uveden�.
		/// Na sm�r �azen� se bere ohled v tom smyslu, �e sm�r �azen� v�ech st�vaj�c� polo�ek mus� b�t stejn� 
		/// jako sm�r �azen� nov�ch polo�ek nebo mus� b�t sm�r �azen� na v�ech polo�k�ch opa�n�.
		/// </summary>
		protected bool StartsWith(SortItemCollection referencingItems)
		{
			if (referencingItems.Count == 0)
			{
				return true;
			}

			if (referencingItems.Count > sortItems.Count)
			{
				return false;
			}

			bool sameDirection = referencingItems[0].Direction == sortItems[0].Direction;

			for (int i = 0; i < referencingItems.Count; i++)
			{
				if ((referencingItems[i].Expression != sortItems[i].Expression) // nejsou stejn� expression
				 || ((referencingItems[i].Direction == sortItems[i].Direction) ^ sameDirection)) // nebo nen� stejn� sm�r �azen�
				{
					return false;
				}
			}

			return true;
		}


		///// <summary>
		///// P�id� polo�ky �azen� do seznamu na prvn� pozice (prvn� polo�ka kolekce bude na za��tku).
		///// Pokud ji� polo�ky v seznamu existuj� na prvn�ch pozic�ch zm�n� sm�r �azen�.
		///// Pokud existuje na jin� pozici, odstran� polo�ku ze seznamu
		///// a vlo�� ji na prvn� pozici (tj. zv��� se "priorita" a �ad� se vzestupn�.
		///// </summary>
		//public void Add(string[] expression)
		//{
		//    // �ad�me vzestupn�
		//    SortDirection newDirection = SortDirection.Ascending;

		//    if (SortItems.Count >= expression.Length)
		//    {
		//        // �ad�me sestupn�, ale d�le to v�t�inou zp�t zm�n�me
		//        newDirection = SortDirection.Descending;

		//        // projdeme v�echny polo�ky
		//        for (int i = 0; i < expression.Length; i++)
		//        {
		//            // a testujeme, jestli jsou na za��tku seznamu a �azen� vzestupn�
		//            // pokud ano, ned�l�me nic (z�stane sestupn� �azen�
		//            // pokud ne, zm�n�me �azen� a kon��me test
		//            if (expression[i] != SortItems[i].Expression || SortItems[i].Direction != SortDirection.Ascending)
		//            {
		//                newDirection = SortDirection.Ascending;
		//                break;
		//            }
		//        }
		//    }

		//    // p�id�me postupn� polo�ky �azen� (odzadu, proto�e posledn� p�idan� bude m�t nejvy��� prioritu)
		//    for (int i = expression.Length - 1; i >= 0; i--)
		//        Add(expression[i], newDirection);
		//}

		///// <summary>
		///// P�id� polo�ku �azen� do seznamu na prvn� pozici. 
		///// Pokud ji� v seznamu existuje, je p�vodn� polo�ka odstran�na.
		///// </summary>
		///// <param name="expression"></param>
		///// <param name="sortDirection"></param>
		//protected void Add(string expression, SortDirection sortDirection)
		//{
		//    RemoveExpression(expression);
		//    SortItems.Insert(0, new SortItem(expression, sortDirection));
		//}

		///// <summary>
		///// Prozkoum� expression od u�ivatele a rozebere ji na jednotliv� polo�ky �azen�.
		///// </summary>
		//public static string[] ParseSortExpression(string expression)
		//{
		//    string[] sortExpressionItems = expression.Split(',');

		//    // odstranime pripadne mezery
		//    for (int i = 0; i < sortExpressionItems.Length; i++)
		//        sortExpressionItems[i] = sortExpressionItems[i].Trim();

		//    return sortExpressionItems;
		//}
	}
}