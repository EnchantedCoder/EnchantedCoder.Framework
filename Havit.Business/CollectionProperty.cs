using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Kolekce objekt� t��dy IProperty.
	/// </summary>
	public class CollectionProperty : IProperty
	{
		/// <summary>
		/// Vytvo�� instanci CollectionProperty.
		/// </summary>
		/// <param name="itemType">Typ prvk� kolekce.</param>
		/// <param name="collectionSelectFieldStatement">��st SQL dotazu pro vyta�en� hodnoty dan�ho sloupce.</param>
		public CollectionProperty(Type itemType, string collectionSelectFieldStatement)
		{
			this.itemType = itemType;
			this.collectionSelectFieldStatement = collectionSelectFieldStatement;
		}

		/// <summary>
		/// Typ prvk� kolekce.
		/// </summary>
		public Type ItemType
		{
			get { return itemType; }
		}
		private Type itemType;

		/// <summary>
		/// ��st SQL dotazu pro vyta�en� hodnoty dan�ho sloupce.
		/// </summary>
		public string CollectionSelectFieldStatement
		{
			get { return collectionSelectFieldStatement; }
		}
		private string collectionSelectFieldStatement;

		/// <summary>
		/// Vr�t� �et�zec pro vyta�en� dan�ho sloupce z datab�ze.
		/// </summary>
		public string GetSelectFieldStatement(System.Data.Common.DbCommand command)
		{
			return collectionSelectFieldStatement;
		}
	}
}
