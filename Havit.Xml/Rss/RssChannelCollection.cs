using System;
using System.Collections;

namespace Havit.Xml.Rss
{
	/// <summary>
	/// Kolekce RssChannel prvk�.
	/// </summary>
	public class RssChannelCollection : CollectionBase
	{
		/// <summary>
		/// P�id� nov� kan�l do kolekce
		/// </summary>
		/// <param name="channel">kan�l k p�id�n�</param>
		public virtual void Add(RssChannel channel)
		{
			this.List.Add(channel);
		}

		/// <summary>
		/// Zkop�ruje kan�ly do array, na pozici index
		/// </summary>
		/// <param name="array">c�lov� pole</param>
		/// <param name="index">pozice v c�lov�m poli</param>
		public void CopyTo(RssChannel[] array, int index)
		{
			((ICollection)this).CopyTo(array, index);
		}

		/// <summary>
		/// Vlo�� na ur�en� m�sto v kolekci kan�l.
		/// </summary>
		/// <param name="index">pozice</param>
		/// <param name="value">kan�l</param>
		public void Insert(int index, RssChannel value) 
		{
			((IList)this).Insert(index, value);
		}        

		/// <summary>
		/// Vyhled� pozici kan�lu v kolekci
		/// </summary>
		/// <param name="value">kan�k</param>
		/// <returns>pozice kan�lu v kolekci</returns>
		public int IndexOf(RssChannel value) 
		{
			return ((IList)this).IndexOf(value);
		}

		/// <summary>
		/// Odstran� kan�l z kolekce.
		/// </summary>
		/// <param name="value">kan�l k odstran�n�</param>
		public void Remove(RssChannel value) 
		{
			((IList)this).Remove( value);
		}

		/// <summary>
		/// Zjist�, zda-li se kan�l nach�z� v kolekci
		/// </summary>
		/// <param name="value">kan�l</param>
		/// <returns>true, pokud ji� kan�l v kolekci existuje, jinak false</returns>
		public bool Contains(RssChannel value) 
		{
			return ((IList)this).Contains( value);
		}

		/// <summary>
		/// Indexer zp��stup�uj�c� kan�l na dan� pozici kolekce.
		/// </summary>
		public virtual RssChannel this[int index]
		{
			get
			{
				return (RssChannel)this.List[index];
			}
		}
	}
}
