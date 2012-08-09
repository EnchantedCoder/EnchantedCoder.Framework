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
		#region Initialize
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
		public void Initialize(
			string dbSchema,
			string dbTable,
			string className,
			string _namespace,
			bool readOnly,
			CreateObjectDelegate createObjectMethod,
			GetObjectDelegate getObjectMethod,
			GetAllDelegate getAllMethod,
			FieldPropertyInfo deletedProperty,
			PropertyInfoCollection properties)
		{
			this.dbSchema = dbSchema;
			this.dbTable = dbTable;
			this.className = className;
			this._namespace = _namespace;
			this.readOnly = readOnly;
			this.createObjectMethod = createObjectMethod;
			this.getObjectMethod = getObjectMethod;
			this.getAllMethod = getAllMethod;
			this.deletedProperty = deletedProperty;
			this.properties = properties;

			this.isInitialized = true;
		}
		private bool isInitialized = false; 
	#endregion

		#region ReadOnly
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
		#endregion

		#region DbSchema
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
		#endregion

		#region DbTable
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
		#endregion

		#region ClassName
		/// <summary>
		/// N�zev t��dy dle datab�zov� tabulky. Bez namespace.
		/// </summary>
		public string ClassName
		{
			get
			{
				CheckInitialization();
				return className;
			}
		}
		private string className;
		#endregion

		#region Namespace
		/// <summary>
		/// Namespace t��dy dle datab�zov� tabulky. Bez n�zvu samotn� t��dy.
		/// </summary>
		public string Namespace
		{
			get
			{
				CheckInitialization();
				return _namespace;
			}
		}
		private string _namespace;
		#endregion

		#region Properties
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
		#endregion

		#region DeletedProperty
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
		#endregion

		#region CreateObjectMethod
		/// <summary>
		/// Deleg�t metody (bez parametr�) vracuj�c� nov� objekt.
		/// </summary>
		public CreateObjectDelegate CreateObjectMethod
		{
			get
			{
				CheckInitialization();
				return createObjectMethod;
			}
		}
		private CreateObjectDelegate createObjectMethod; 
		#endregion

		#region GetObjectMethod
		/// <summary>
		/// Deleg�t metody vracuj�c� instanci objektu.
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
		#endregion

		#region GetAllMethod
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
		#endregion

		#region CheckInitialization
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
		#endregion

	}
}