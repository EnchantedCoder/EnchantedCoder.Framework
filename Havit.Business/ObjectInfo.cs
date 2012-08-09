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
		/// <param name="properties">Properties t��dy.</param>
		/// <param name="deletedProperty">FieldPropertyInfo, kter� identifikuje p��znakem smazan� z�znamy.</param>
		public ObjectInfo(string dbSchema, string dbTable, bool readOnly, PropertyInfoCollection properties, FieldPropertyInfo deletedProperty)
		{
			if (properties == null)
				throw new ArgumentNullException("properties");

			this.dbSchema = dbSchema;
			this.dbTable = dbTable;
			this.readOnly = readOnly;
			this.properties = properties;
			this.deletedProperty = deletedProperty;

			foreach (PropertyInfo propertyInfo in properties)
			{
				propertyInfo.Parent = this;
			}
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

	}
}