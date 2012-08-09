using System.Collections;

namespace Havit.Xml.Rss
{
	/// <summary>
	/// Kolekce RssItem prvk�.
	/// </summary>
	public class RssItemCollection : CollectionBase
	{
		/// <summary>
		/// P�id� nov� item do kolekce
		/// </summary>
		/// <param name="newItem">nov� item</param>
		public virtual void Add(RssItem newItem)
		{
			this.List.Add(newItem);
		}

		/// <summary>
		/// Vykop�ruje prvky kolekce do pole.
		/// </summary>
		/// <param name="array">c�lov� pole</param>
		/// <param name="index">startovn� pozice v c�lov�m poli</param>
		public void CopyTo(RssItem[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		} 
       
		/// <summary>
		/// Vlo�� prvek do kolekce na zadanou pozici.
		/// </summary>
		/// <param name="index">pozice</param>
		/// <param name="value">prvek</param>
		public void Insert(int index, RssItem value) 
		{
			((IList)this).Insert(index, value);
		}

		/// <summary>
		/// Zjist� pozici prvku v kolekci
		/// </summary>
		/// <param name="value">hledn� prvek</param>
		/// <returns>pozice v kolekci</returns>
		public int IndexOf(RssItem value) 
		{
			return ((IList)this).IndexOf(value);
		}

		/// <summary>
		/// Odebere prvke z kolekce.
		/// </summary>
		/// <param name="value">prvek k odebr�n�</param>
		public void Remove(RssItem value) 
		{
			((IList)this).Remove(value);
		}

		/// <summary>
		/// Zjist�, zda-li je prvek v kolekci.
		/// </summary>
		/// <param name="value">prvek</param>
		/// <returns>true, je-li prvek v kolekci, jinak false</returns>
		public bool Contains(RssItem value) 
		{
			return ((IList)this).Contains(value);
		}

		/// <summary>
		/// Indexer na kolekci zp��stup�uj�c� prvek na po�adovan� pozici.
		/// </summary>
		public virtual RssItem this[int index]
		{
			get
			{
				return (RssItem)this.List[index];
			}
		}
	}
}
