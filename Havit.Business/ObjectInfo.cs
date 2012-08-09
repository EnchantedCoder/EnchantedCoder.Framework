using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business
{
	/// <summary>
	/// Reprezentuje informace o objektu (t��d�).
	/// </summary>
	public class ObjectInfo
	{
		/// <summary>
		/// Vytvo�� instanci t��dy.
		/// </summary>
		/// <param name="dbSchema">N�zev sch�mata datab�zov� tabulky.</param>
		/// <param name="dbTable">N�zev datab�zov� tabulky.</param>
		/// <param name="readOnly">Ur�uje, zda je t��da jen ke �ten�.</param>
		/// <param name="deletedProperty">FieldPropertyInfo, kter� identifikuje p��znakem smazan� z�znamy.</param>
		/// <param name="getObjectMethod">Deleg�t na metodu vracej�c� objekt t��dy na z�klad� ID.</param>
		/// <param name="getAllMethod">Deleg�t na metodu vracej�c� v�echny (nesmazan�) objekty t��dy.</param>
		public ObjectInfo(string dbSchema, string dbTable, bool readOnly, FieldPropertyInfo deletedProperty, 
			GetObjectDelegate getObjectMethod, GetAllDelegate getAllMethod)
		{
			this.dbSchema = dbSchema;
			this.dbTable = dbTable;
			this.readOnly = readOnly;
			this.deletedProperty = deletedProperty;
			this.getObjectMethod = getObjectMethod;
			this.getAllMethod = getAllMethod;
		}

		/// <summary>
		/// Indikuje, zda je objekt ur�en jen ke �ten�.
		/// </summary>
		public bool ReadOnly
		{
			get { return readOnly; }
		}
		private bool readOnly;

		/// <summary>
		/// N�zev sch�matu datab�zov� tabulky.
		/// </summary>
		public string DbSchema
		{
			get { return dbSchema; }
		}
		private string dbSchema;

		/// <summary>
		/// N�zev datab�zov� tabulky.
		/// </summary>
		public string DbTable
		{
			get { return dbTable; }
		}
		private string dbTable;

		/// <summary>
		/// Property ve t��d�.
		/// </summary>
		public PropertyInfoCollection Properties
		{
			get { return properties; }
		}
		private PropertyInfoCollection properties;

		/// <summary>
		/// Property, kter� ozna�uje smazan� z�znamy.
		/// </summary>
		public FieldPropertyInfo DeletedProperty
		{
			get { return deletedProperty;  }
		}
		private FieldPropertyInfo deletedProperty;

		/// <summary>
		/// Metoda vracuj�c� instanci objektu.
		/// </summary>
		public GetObjectDelegate GetObjectMethod
		{
			get { return getObjectMethod; }
		}
		private GetObjectDelegate getObjectMethod;

		/// <summary>
		/// Metoda vracej�c� seznam v�ech instanc�.
		/// </summary>
		public GetAllDelegate GetAllMethod
		{
			get { return getAllMethod; }
		}
		private GetAllDelegate getAllMethod;

		/// <summary>
		/// Registruje kolekci properties.
		/// Ka�d� registrovan� property nastav� parenta na tuto instanci t��dy ObjectInfo.
		/// </summary>
		public void RegisterProperties(PropertyInfoCollection properties)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			this.properties = properties;

			foreach (PropertyInfo propertyInfo in properties)
			{
				propertyInfo.Parent = this;
			}
		}
	}
}