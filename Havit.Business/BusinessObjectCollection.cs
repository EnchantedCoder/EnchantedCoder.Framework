using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Havit.Collections;
using System.Data.Common;

namespace Havit.Business
{
	/// <summary>
	/// B�zov� t��da pro v�echny kolekce BusinessObjectBase (Layer SuperType)
	/// </summary>
	/// <remarks>
	/// POZOR! Vnit�n� implementace je z�visl� na faktu, �e this.Items je List(Of T).
	/// To je v�choz� chov�n� Collection(Of T), ale pro jistotu si to je�t� vynucujeme
	/// pou�it�m wrappuj�c�ho constructoru.
	/// </remarks>
	/// <typeparam name="TItem">�lensk� typ kolekce</typeparam>
	/// <typeparam name="TCollection">typ pou��van� business object kolekce</typeparam>
	[Serializable]
	public class BusinessObjectCollection<TItem, TCollection> : Collection<TItem>
		where TItem : BusinessObjectBase
		where TCollection : BusinessObjectCollection<TItem, TCollection>, new()
	{
		#region Event - CollectionChanged
		/// <summary>
		/// Ud�lost vyvolan� po jak�koliv zm�n� kolekce (Insert, Remove, Set, Clear).
		/// </summary>
		public event EventHandler CollectionChanged;

		/// <summary>
		/// Prov�d� se jako vol�n� ud�losti <see cref="CollectionChanged"/>.
		/// </summary>
		/// <param name="e">pr�zdn�</param>
		protected void OnCollectionChanged(EventArgs e)
		{
			if (CollectionChanged != null)
			{
				CollectionChanged(this, e);
			}
		}
		#endregion

		#region AllowDuplicates
		/// <summary>
		/// Ur�uje, zda je mo�n� do kolekce vlo�it hodnotu, kter� ji� v kolekci je.
		/// Pokud je nastaveno na true, p�id�n� hodnoty, kter� v kolekci ji� je, vyvol� v�jimku.
		/// Pokud je nastaveno na false (v�choz�), je mo�n� hodnotu do kolekce p�idat v�cekr�t.
		/// </summary>
		public bool AllowDuplicates
		{
			get { return _allowDuplicates; }
			set {
				if (_allowDuplicates && !value)
				{
					if (CheckDuplicates())
					{
						throw new InvalidOperationException("Kolekce obsahuje duplicity.");
					}
				}
				_allowDuplicates = value; 
			}
		}

		private bool _allowDuplicates = true;
		#endregion

		#region InsertItem (override)
		/// <summary>
		/// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"></see> at the specified index.
		/// When AllowDuplicates is false, checks whether item already is in the collection. If so, throws an ArgumentException.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert. The value can be null for reference types.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
		protected override void InsertItem(int index, TItem item)
		{
			if ((!_allowDuplicates) && (this.Contains(item)))
			{
				throw new ArgumentException("Polo�ka v kolekci ji� existuje (a nen� povoleno vkl�d�n� duplicit).");
			}

			base.InsertItem(index, item);
			OnCollectionChanged(EventArgs.Empty);
		}
		#endregion

		#region RemoveItem (override)
		/// <summary>
		/// Removes the element at the specified index of the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is equal to or greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
			OnCollectionChanged(EventArgs.Empty);
		}
		#endregion

		#region SetItem (override)
		/// <summary>
		/// Replaces the element at the specified index.
		/// When AllowDuplicates is false, checks whether item already is in the collection. If so, throws an ArgumentException.
		/// </summary>
		/// <param name="index">The zero-based index of the element to replace.</param>
		/// <param name="item">The new value for the element at the specified index. The value can be null for reference types.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
		protected override void SetItem(int index, TItem item)
		{
			// je zaji�t�no, �e v re�imu !AllowDuplicates kolekce neobsahuje duplik�ty
			// potom m��eme pou��t IndexOf na hled�n� v�skytu (je garantov�no, �e prvek je v kolekci nejv��e jednou).
			if (!_allowDuplicates && (this.IndexOf(item) != index)) 
			{
				throw new ArgumentException("Polo�ka v kolekci ji� existuje (a nen� povoleno vkl�d�n� duplicit).");
			}
			base.SetItem(index, item);
			OnCollectionChanged(EventArgs.Empty);
		}
		#endregion

		#region ClearItems (override)
		/// <summary>
		/// Removes all elements from the <see cref="T:System.Collections.ObjectModel.Collection`1"></see>.
		/// </summary>
		protected override void ClearItems()
		{
			base.ClearItems();
			OnCollectionChanged(EventArgs.Empty);
		}
		#endregion

		#region Constructors
		/// <summary>
		/// Vytvo�� novou instanci kolekce bez prvk� - pr�zdnou.
		/// </summary>
		/// <remarks>
		/// Pou�it je wrappuj�c� constructor Collection(Of T), abychom si vynutili List(Of T).
		/// </remarks>
		public BusinessObjectCollection()
			: base(new List<TItem>())
		{
		}

		/// <summary>
		/// Vytvo�� novou instanci kolekce wrapnut�m Listu prvk� (neklonuje!).
		/// </summary>
		/// <remarks>
		/// Je to rychl�! Nikam se nic nekop�ruje, ale pozor, ani neklonuje!
		/// </remarks>
		/// <param name="list">List prvk�</param>
		public BusinessObjectCollection(List<TItem> list)
			: base(list)
		{
		}

		/// <summary>
		/// Vytvo�� novou instanci kolekce a zkop�ruje do n� prvky z p�edan� kolekce.
		/// </summary>
		/// <param name="collection">kolekce, jej� prvky se maj� do na�� kolekce zkop�rovat</param>
		public BusinessObjectCollection(IEnumerable<TItem> collection)
			: base(new List<TItem>(collection))
		{
		}
		#endregion

		#region FindByID
		/// <summary>
		/// Prohled� kolekci a vr�t� prvn� nalezen� prvek s po�adovan�m ID.
		/// </summary>
		/// <remarks>
		/// Vzhledem k tomu, �e jsou prvky v kolekci obvykle unik�tn�, najde prost� zadan� ID.
		/// </remarks>
		/// <param name="id">ID prvku</param>
		/// <returns>prvn� nalezen� prvek s po�adovan�m ID; null, pokud nic nenalezeno</returns>
		public TItem FindByID(int id)
		{
			List<TItem> innerList = (List<TItem>)Items;
			TItem result = null;
			result = innerList.Find(delegate(TItem item)
								  {
									  if (item.ID == id)
									  {
										  return true;
									  }
									  else
									  {
										  return false;
									  }
								  });

			return result;
		}
		#endregion

		#region Find
		/// <summary>
		/// Prohled� kolekci a vr�t� prvn� nalezen� prvek odpov�daj�c� krit�riu match.
		/// </summary>
		/// <remarks>
		/// Metoda pouze publikuje metodu Find() inner-Listu Items.
		/// </remarks>
		/// <param name="match">krit�rium ve form� predik�tu</param>
		/// <returns>kolekce v�ech prvk� odpov�daj�c�ch krit�riu match</returns>
		public virtual TItem Find(Predicate<TItem> match)
		{
			List<TItem> innerList = (List<TItem>)Items;
			return innerList.Find(match);
		}
		#endregion

		#region FindAll
		/// <summary>
		/// Prohled� kolekci a vr�t� v�echny prvky odpov�daj�c� krit�riu match.
		/// </summary>
		/// <remarks>
		/// Metoda pouze publikuje metodu FindAll() inner-listu Items.
		/// </remarks>
		/// <param name="match">krit�rium ve form� predik�tu</param>
		/// <returns>kolekce v�ech prvk� odpov�daj�c�ch krit�riu match</returns>
		public virtual TCollection FindAll(Predicate<TItem> match)
		{
			List<TItem> innerList = (List<TItem>)Items;
			List<TItem> found = innerList.FindAll(match);
			TCollection result = new TCollection();
			result.AllowDuplicates = this.AllowDuplicates; // ???
			result.AddRange(found);
			return result;
		}
		#endregion

		#region ForEach
		/// <summary>
		/// Spust� akci nad v�emi prvky kolekce.
		/// </summary>
		/// <example>
		/// orders.ForEach(delegate(Order item)
		///		{
		///			item.Delete();
		///		});
		/// </example>
		/// <remarks>
		/// Je rychlej��, ne� <c>foreach</c>, proto�e neproch�z� enumerator, ale iteruje prvky ve for cyklu podle indexu.
		/// </remarks>
		/// <param name="action">akce, kter� m� b�t spu�t�na</param>
		public void ForEach(Action<TItem> action)
		{
			List<TItem> innerList = (List<TItem>)Items;
			innerList.ForEach(action);
		}
		#endregion

		#region AddRange
		/// <summary>
		/// P�id� do kolekce prvky p�edan� kolekce.
		/// </summary>
		/// <param name="source">Kolekce, jej� prvky maj� b�t p�id�ny.</param>
		public void AddRange(IEnumerable<TItem> source)
		{
			List<TItem> innerList = (List<TItem>)Items;
			int originalItemsCount = innerList.Count;
			innerList.AddRange(source);

			// vyvol�me ud�lost informuj�c� o zm�n� kolekce, pokud se zm�nil po�et objekt� v kolekci
			if (originalItemsCount != innerList.Count)
			{
				OnCollectionChanged(EventArgs.Empty);
			}
		}
		#endregion

		#region RemoveAll
		/// <summary>
		/// Odstran� z kolekce v�echny prvky odpov�daj�c� krit�riu match.
		/// </summary>
		/// <remarks>
		/// Metoda pouze publikuje metodu RemoveAll() inner-listu Items.
		/// </remarks>
		/// <param name="match">krit�rium ve form� predik�tu</param>
		/// <returns>po�et odstran�n�ch prvk�</returns>
		public virtual int RemoveAll(Predicate<TItem> match)
		{
			List<TItem> innerList = (List<TItem>)Items;		
			int itemsRemovedCount = innerList.RemoveAll(match);
			
			// vyvol�me ud�lost informuj�c� o zm�n� kolekce, pokud n�jak� objekty byly z kolekce odstran�ny
			if (itemsRemovedCount != 0)
			{
				OnCollectionChanged(EventArgs.Empty);
			}

			return itemsRemovedCount;
		}
		#endregion

		#region RemoveRange
		/// <summary>
		/// Odstran� z kolekce po�adovan� prvky.
		/// </summary>
		/// <param name="items">prvky, kter� maj� b�t z kolekce odstran�ny</param>
		/// <returns>po�et prvk�, kter� byly skute�n� odstran�ny</returns>
		public virtual int RemoveRange(IEnumerable<TItem> items)
		{
			List<TItem> innerList = (List<TItem>)Items;		
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			int count = 0;
			foreach (TItem item in items)
			{
				if (this.Remove(item))
				{
					count++;
				}
			}
			return count;
		}
		#endregion

		#region Sort
		/// <summary>
		/// Se�ad� prvky kolekce dle po�adovan� property, kter� implementuje IComparable.
		/// </summary>
		/// <remarks>
		/// Pou��v� <see cref="Havit.Collections.GenericPropertyComparer{T}"/>. K porovn�v�n� podle property
		/// tedy doch�z� pomoc� reflexe - relativn� pomalu. Pokud je pot�eba vy��� v�kon, je pot�eba pou��t
		/// overload Sort(Generic Comparsion) s p��m�m p��stupem k property.
		/// </remarks>
		/// <param name="propertyName">property, podle kter� se m� �adit</param>
		/// <param name="ascending">true, pokud se m� �adit vzestupn�, false, pokud sestupn�</param>
		[Obsolete]
		public void Sort(string propertyName, bool ascending)
		{
			List<TItem> innerList = (List<TItem>)Items;
			innerList.Sort(new GenericPropertyComparer<TItem>(new SortItem(propertyName, ascending ? SortDirection.Ascending : SortDirection.Descending)));
		}
		
		/// <summary>
		/// Se�ad� prvky kolekce dle po�adovan� property, kter� implementuje IComparable.
		/// </summary>
		/// <remarks>
		/// Pou��v� <see cref="Havit.Collections.GenericPropertyComparer{T}"/>. K porovn�v�n� podle property
		/// tedy doch�z� pomoc� reflexe - relativn� pomalu. Pokud je pot�eba vy��� v�kon, je pot�eba pou��t
		/// overload Sort(Generic Comparsion) s p��m�m p��stupem k property.
		/// </remarks>
		/// <param name="propertyInfo">Property, podle kter� se m� �adit.</param>
		/// <param name="sortDirection">Sm�r �azen�.</param>
		public void Sort(PropertyInfo propertyInfo, SortDirection sortDirection)
		{
			List<TItem> innerList = (List<TItem>)Items;
			innerList.Sort(new GenericPropertyComparer<TItem>(new SortItem(propertyInfo.PropertyName, sortDirection)));
		}

		/// <summary>
		/// Se�ad� prvky kolekce dle zadan�ho srovn�n�. Publikuje metodu Sort(Generic Comparsion) inner-Listu.
		/// </summary>
		/// <param name="comparsion">srovn�n�, podle kter�ho maj� b�t prvky se�azeny</param>
		public void Sort(Comparison<TItem> comparsion)
		{
			List<TItem> innerList = (List<TItem>)Items;
			innerList.Sort(comparsion);
		}
		#endregion

		#region SaveAll
		/// <summary>
		/// Ulo�� v�echny prvky kolekce, v transakci.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v kter� maj� b�t prvky ulo�eny</param>
		public virtual void SaveAll(DbTransaction transaction)
		{
			ForEach(delegate(TItem item)
				{
					item.Save(transaction);
				});
		}

		/// <summary>
		/// Ulo�� v�echny prvky kolekce, bez transakce.
		/// </summary>
		public virtual void SaveAll()
		{
			SaveAll(null);
		}
		#endregion

		#region GetIDs
		/// <summary>
		/// Vr�t� pole hodnot ID v�ech prvk� kolekce.
		/// </summary>
		/// <returns>pole hodnot ID v�ech prvk� kolekce</returns>
		public int[] GetIDs()
		{
			int[] array = new int[this.Count];
			List<TItem> innerList = (List<TItem>)Items;
			for (int i = 0; i < innerList.Count; i++)
			{
				array[i] = innerList[i].ID;
			}
			return array;
		}
		#endregion

		/***********************************************************************/

		#region CheckDuplicates (private)
		/// <summary>
		/// Vrac� true, pokud kolekce obsahuje duplicity.
		/// </summary>		
		private bool CheckDuplicates()
		{			
			// obsahuje-li kolekce m�n� ne� dva prvky, nem��e obsahovat duplicity.
			if (Items.Count < 2)
			{
				return false;
			}

			//sem se dostaneme m�lokdy (pokud v�bec), tak�e nen� nutn� implementovat buhv� jak optimalizovan�
			List<TItem> innerList = (List<TItem>)Items;
			for (int i = 0; i < innerList.Count - 1; i++)
			{				
				// pokud posledn� v�skyt prvku je jin�, ne� aktu�ln�, jde o duplicitu
				// (M�sto LastIndexOf bychom mohli v klidu pou��t IndexOf, je to zcela jedno.)
				if (innerList.LastIndexOf(innerList[i]) != i)
				{
					return true;
				}
			}

			return false;			
		}
		#endregion

	}
}
