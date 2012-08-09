using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Kolekce objekt� t��dy IProperty.
	/// </summary>
	public class CollectionPropertyInfo : PropertyInfo, IFieldsBuilder
	{
		/// <summary>
		/// Vytvo�� instanci CollectionProperty.
		/// </summary>
		/// <param name="owner">Nad�azen� objectInfo.</param>
		/// <param name="itemType">Typ prvk� kolekce.</param>
		/// <param name="collectionSelectFieldStatement">��st SQL dotazu pro vyta�en� hodnoty dan�ho sloupce.</param>
		public void Initialize(ObjectInfo owner, Type itemType, string collectionSelectFieldStatement)
		{
			Initialize(owner);
			this.itemType = itemType;
			this.collectionSelectFieldStatement = collectionSelectFieldStatement;
		}

		/// <summary>
		/// Typ prvk� kolekce.
		/// </summary>
		public Type ItemType
		{
			get
			{
				CheckInitialization();
				return itemType;
			}
		}
		private Type itemType;

		/// <summary>
		/// ��st SQL dotazu pro vyta�en� hodnoty dan�ho sloupce.
		/// </summary>
		public string CollectionSelectFieldStatement
		{
			get
			{
				CheckInitialization();
				return collectionSelectFieldStatement;
			}
		}
		private string collectionSelectFieldStatement;

		/// <summary>
		/// Vr�t� �et�zec pro vyta�en� dan�ho sloupce z datab�ze.
		/// </summary>
		public string GetSelectFieldStatement(System.Data.Common.DbCommand command)
		{
			CheckInitialization();
			return collectionSelectFieldStatement;
		}
	}
}
