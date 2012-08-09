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
		/// Nastav� instanci t��dy.
		/// </summary>
		/// <param name="dbSchema">N�zev sch�mata datab�zov� tabulky.</param>
		/// <param name="dbTable">N�zev datab�zov� tabulky.</param>
		/// <param name="readOnly">Ur�uje, zda je t��da jen ke �ten�.</param>
		/// <param name="getObjectMethod">Deleg�t na metodu vracej�c� objekt t��dy na z�klad� ID.</param>
		/// <param name="getAllMethod">Deleg�t na metodu vracej�c� v�echny (nesmazan�) objekty t��dy.</param>
		/// <param name="deletedProperty">FieldPropertyInfo, kter� identifikuje p��znakem smazan� z�znamy.</param>
		/// <param name="properties">Kolekce v�ech vlastnost� objektu.</param>
		public void Initialize(string dbSchema, string dbTable, bool readOnly, 
			GetObjectDelegate getObjectMethod, GetAllDelegate getAllMethod, FieldPropertyInfo deletedProperty, PropertyInfoCollection properties)
		{
			this.dbSchema = dbSchema;
			this.dbTable = dbTable;
			this.readOnly = readOnly;
			this.getObjectMethod = getObjectMethod;
			this.getAllMethod = getAllMethod;
			this.deletedProperty = deletedProperty;
			this.properties = properties;

			this.isInitialized = true;
		}
		private bool isInitialized = false;

		/// <summary>
		/// Indikuje, zda je objekt ur�en jen ke �ten�.
		/// </summary>
		public bool ReadOnly
		{
			get
			{
				CheckInitialization();
				return readOnly;
			}
		}
		private bool readOnly;

		/// <summary>
		/// N�zev sch�matu datab�zov� tabulky.
		/// </summary>
		public string DbSchema
		{
			get
			{
				CheckInitialization();
				return dbSchema;
			}
		}
		private string dbSchema;

		/// <summary>
		/// N�zev datab�zov� tabulky.
		/// </summary>
		public string DbTable
		{
			get
			{
				CheckInitialization();
				return dbTable;
			}
		}
		private string dbTable;

		/// <summary>
		/// Property ve t��d�.
		/// </summary>
		public PropertyInfoCollection Properties
		{
			get
			{
				CheckInitialization();
				return properties;
			}
		}
		private PropertyInfoCollection properties;

		/// <summary>
		/// Property, kter� ozna�uje smazan� z�znamy.
		/// </summary>
		public FieldPropertyInfo DeletedProperty
		{
			get
			{
				CheckInitialization();
				return deletedProperty;
			}
		}
		private FieldPropertyInfo deletedProperty;

		/// <summary>
		/// Metoda vracuj�c� instanci objektu.
		/// </summary>
		public GetObjectDelegate GetObjectMethod
		{
			get
			{
				CheckInitialization();
				return getObjectMethod;
			}
		}
		private GetObjectDelegate getObjectMethod;

		/// <summary>
		/// Metoda vracej�c� seznam v�ech instanc�.
		/// </summary>
		public GetAllDelegate GetAllMethod
		{
			get
			{
				CheckInitialization();
				return getAllMethod; 
			}
		}
		private GetAllDelegate getAllMethod;

		/// <summary>
		/// Ov���, �e byla instance inicializov�na. Pokud ne, vyhod� v�jimku.
		/// </summary>
		protected void CheckInitialization()
		{
			if (!isInitialized)
			{
				throw new InvalidOperationException("Instance nebyla inicializov�na.");
			}
		}

	}
}