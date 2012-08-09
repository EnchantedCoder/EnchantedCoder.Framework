using System;
using System.Collections.Generic;
using System.Text;
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
	/// <typeparam name="T">�lensk� typ kolekce</typeparam>
	[Serializable]
	public class BusinessObjectCollection<T> : Collection<T>
		where T : BusinessObjectBase
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
			get { return allowDuplicates; }
			set { allowDuplicates = value; }
		}
		private bool allowDuplicates = true;
		#endregion

		#region AddRange
		/// <summary>
		/// P�id� do kolekce prvky p�edan� kolekce.
		/// </summary>
		/// <param name="source">Kolekce, jej� prvky maj� b�t p�id�ny.</param>
		public void AddRange(IEnumerable<T> source)
		{
			foreach (T item in source)
			{
				this.Add(item);
			}
		}
		#endregion

		#region InsertItem (override)
		/// <summary>
		/// Inserts an element into the <see cref="T:System.Collections.ObjectModel.Collection`1"></see> at the specified index.
		/// When AllowDuplicates is false, checks whether item already is in the collection. If so, throws an ArgumentException.
		/// </summary>
		/// <param name="index">The zero-based index at which item should be inserted.</param>
		/// <param name="item">The object to insert. The value can be null for reference types.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">index is less than zero.-or-index is greater than <see cref="P:System.Collections.ObjectModel.Collection`1.Count"></see>.</exception>
		protected override void InsertItem(int index, T item)
		{
			if ((!allowDuplicates) && (this.Contains(item)))
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
		protected override void SetItem(int index, T item)
		{
			if (!allowDuplicates && (this.IndexOf(item) != index))
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
			: base(new List<T>())
		{
		}

		/// <summary>
		/// Vytvo�� novou instanci kolekce wrapnut�m Listu prvk� (neklonuje!).
		/// </summary>
		/// <remarks>
		/// Je to rychl�! Nikam se nic nekop�ruje, ale pozor, ani neklonuje!
		/// </remarks>
		/// <param name="list">List prvk�</param>
		public BusinessObjectCollection(List<T> list)
			: base(list)
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
		public T FindByID(int id)
		{
			List<T> innerList = (List<T>)Items;
			T result = null;
			result = innerList.Find(delegate(T item)
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
		public virtual T Find(Predicate<T> match)
		{
			List<T> innerList = (List<T>)Items;
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
		public virtual BusinessObjectCollection<T> FindAll(Predicate<T> match)
		{
			List<T> innerList = (List<T>)Items;
			return new BusinessObjectCollection<T>(innerList.FindAll(match));
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
		public void ForEach(Action<T> action)
		{
			List<T> innerList = (List<T>)Items;
			innerList.ForEach(action);
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
		public void Sort(string propertyName, bool ascending)
		{
			List<T> innerList = (List<T>)Items;
			innerList.Sort(new GenericPropertyComparer<T>(propertyName, ascending));
		}

		/// <summary>
		/// Se�ad� prvky kolekce dle zadan�ho srovn�n�. Publikuje metodu Sort(Generic Comparsion) inner-Listu.
		/// </summary>
		/// <param name="comparsion">srovn�n�, podle kter�ho maj� b�t prvky se�azeny</param>
		public void Sort(Comparison<T> comparsion)
		{
			List<T> innerList = (List<T>)Items;
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
			ForEach(delegate(T item)
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
			List<T> innerList = (List<T>)Items;
			for (int i = 0; i < innerList.Count; i++)
			{
				array[i] = innerList[i].ID;
			}
			return array;
		}
		#endregion
	}
}
