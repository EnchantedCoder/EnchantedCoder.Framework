//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading;
using System.Web;
using System.Web.Caching;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;
using Havit.Business;
using Havit.Business.Query;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Subjekt.
	/// </summary>
	[Serializable]
	public abstract class SubjektBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static SubjektBase()
		{
			objectInfo = new ObjectInfo();
			properties = new SubjektProperties();
			objectInfo.Initialize("dbo", "Subjekt", false, Subjekt.GetObject, Subjekt.GetAll, properties.Deleted, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected SubjektBase()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SubjektID (PK)</param>
		protected SubjektBase(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">SubjektID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected SubjektBase(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Název.
		/// </summary>
		public virtual string Nazev
		{
			get
			{
				EnsureLoaded();
				return _NazevPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				if (value == null)
				{
					_NazevPropertyHolder.Value = String.Empty;
				}
				else
				{
					_NazevPropertyHolder.Value = value;
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Nazev.
		/// </summary>
		protected PropertyHolder<string> _NazevPropertyHolder;
		
		/// <summary>
		/// Uživatel (login).
		/// </summary>
		public virtual Havit.BusinessLayerTest.Uzivatel Uzivatel
		{
			get
			{
				EnsureLoaded();
				return _UzivatelPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_UzivatelPropertyHolder.Value = value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Uzivatel.
		/// </summary>
		protected PropertyHolder<Havit.BusinessLayerTest.Uzivatel> _UzivatelPropertyHolder;
		
		/// <summary>
		/// Čas vytvoření objektu.
		/// </summary>
		public virtual DateTime Created
		{
			get
			{
				EnsureLoaded();
				return _CreatedPropertyHolder.Value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Created.
		/// </summary>
		protected PropertyHolder<DateTime> _CreatedPropertyHolder;
		
		/// <summary>
		/// Čas smazání objektu.
		/// </summary>
		public virtual DateTime? Deleted
		{
			get
			{
				EnsureLoaded();
				return _DeletedPropertyHolder.Value;
			}
			protected set
			{
				EnsureLoaded();
				_DeletedPropertyHolder.Value = value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Deleted.
		/// </summary>
		protected PropertyHolder<DateTime?> _DeletedPropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_NazevPropertyHolder = new PropertyHolder<string>(this);
			_UzivatelPropertyHolder = new PropertyHolder<Havit.BusinessLayerTest.Uzivatel>(this);
			_CreatedPropertyHolder = new PropertyHolder<DateTime>(this);
			_DeletedPropertyHolder = new PropertyHolder<DateTime?>(this);
			
			if (IsNew)
			{
				_NazevPropertyHolder.Value = String.Empty;
				_UzivatelPropertyHolder.Value = null;
				_CreatedPropertyHolder.Value = System.DateTime.Now;
				_DeletedPropertyHolder.Value = null;
			}
		}
		#endregion
		
		#region CheckConstraints
		/// <summary>
		/// Kontroluje konzistenci objektu jako celku.
		/// </summary>
		/// <remarks>
		/// Automaticky je voláno před ukládáním objektu Save(), pokud je objekt opravdu ukládán.
		/// </remarks>
		protected override void CheckConstraints()
		{
			base.CheckConstraints();
			
			if (_NazevPropertyHolder.IsDirty && (_NazevPropertyHolder.Value != null) && (_NazevPropertyHolder.Value.Length > 50))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"Nazev\" přesáhl maximální délku 50 znaků.");
			}
			
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (_DeletedPropertyHolder.Value != null)
				{
					if ((_DeletedPropertyHolder.Value.Value < SqlDateTime.MinValue.Value) || (_DeletedPropertyHolder.Value.Value > SqlDateTime.MaxValue.Value))
					{
						throw new ConstraintViolationException(this, "Vlastnost \"Deleted\" nesmí nabývat hodnoty mimo rozsah SqlDateTime.MinValue-SqlDateTime.MaxValue.");
					}
				}
			}
			
		}
		#endregion
		
		#region Load: Load_GetDataRecord, Load_ParseDataRecord
		/// <summary>
		/// Načte data objektu z DB a vrátí je ve formě DataRecordu.
		/// </summary>
		/// <param name="transaction">případná transakce</param>
		/// <returns>úplná data objektu</returns>
		protected override sealed DataRecord Load_GetDataRecord(DbTransaction transaction)
		{
			DataRecord result;
			
			SqlCommand sqlCommand = new SqlCommand("SELECT SubjektID, Nazev, UzivatelID, Created, Deleted FROM [dbo].[Subjekt] WHERE SubjektID = @SubjektID");
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterSubjektID = new SqlParameter("@SubjektID", SqlDbType.Int);
			sqlParameterSubjektID.Direction = ParameterDirection.Input;
			sqlParameterSubjektID.Value = this.ID;
			sqlCommand.Parameters.Add(sqlParameterSubjektID);
			
			result = DbConnector.Default.ExecuteDataRecord(sqlCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		protected override sealed void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("SubjektID");
			
			string _tempNazev;
			if (record.TryGet<string>("Nazev", out _tempNazev))
			{
				_NazevPropertyHolder.Value = _tempNazev ?? "";
			}
			
			int? _tempUzivatel;
			if (record.TryGet<int?>("UzivatelID", out _tempUzivatel))
			{
				_UzivatelPropertyHolder.Value = (_tempUzivatel == null) ? null : Havit.BusinessLayerTest.Uzivatel.GetObject(_tempUzivatel.Value);
			}
			
			DateTime _tempCreated;
			if (record.TryGet<DateTime>("Created", out _tempCreated))
			{
				_CreatedPropertyHolder.Value = _tempCreated;
			}
			
			DateTime? _tempDeleted;
			if (record.TryGet<DateTime?>("Deleted", out _tempDeleted))
			{
				_DeletedPropertyHolder.Value = _tempDeleted;
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete_Perform
		
		/// <summary>
		/// Ukládá member-objekty.
		/// </summary>
		protected override void Save_SaveMembers(DbTransaction transaction)
		{
			base.Save_SaveMembers(transaction);
			
			if (_UzivatelPropertyHolder.IsInitialized && (_UzivatelPropertyHolder.Value != null))
			{
				_UzivatelPropertyHolder.Value.Save(transaction);
			}
			
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		protected override void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			// Není co ukládat.
		}
		
		/// <summary>
		/// Implementace metody vloží jen not-null vlastnosti objektu do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		public override sealed void Save_MinimalInsert(DbTransaction transaction)
		{
			base.Save_MinimalInsert(transaction);
			Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterNazev = new SqlParameter("@Nazev", SqlDbType.NVarChar, 50);
			sqlParameterNazev.Direction = ParameterDirection.Input;
			sqlParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterNazev);
			_NazevPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.SmallDateTime);
			sqlParameterDeleted.Direction = ParameterDirection.Input;
			sqlParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			sqlCommand.CommandText = "DECLARE @SubjektID INT; INSERT INTO [dbo].[Subjekt] (Nazev, Deleted) VALUES (@Nazev, @Deleted); SELECT @SubjektID = SCOPE_IDENTITY(); SELECT @SubjektID; ";
			
			this.ID = (int)DbConnector.Default.ExecuteScalar(sqlCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(this);
			}
		}
		
		/// <summary>
		/// Implementace metody vloží nový objekt do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		protected override sealed void Save_FullInsert(DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterNazev = new SqlParameter("@Nazev", SqlDbType.NVarChar, 50);
			sqlParameterNazev.Direction = ParameterDirection.Input;
			sqlParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterNazev);
			_NazevPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterUzivatelID = new SqlParameter("@UzivatelID", SqlDbType.Int);
			sqlParameterUzivatelID.Direction = ParameterDirection.Input;
			sqlParameterUzivatelID.Value = (_UzivatelPropertyHolder.Value == null) ? DBNull.Value : (object)_UzivatelPropertyHolder.Value.ID;
			sqlCommand.Parameters.Add(sqlParameterUzivatelID);
			_UzivatelPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.SmallDateTime);
			sqlParameterDeleted.Direction = ParameterDirection.Input;
			sqlParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			sqlCommand.CommandText = "DECLARE @SubjektID INT; INSERT INTO [dbo].[Subjekt] (Nazev, UzivatelID, Deleted) VALUES (@Nazev, @UzivatelID, @Deleted); SELECT @SubjektID = SCOPE_IDENTITY(); SELECT @SubjektID; ";
			
			this.ID = (int)DbConnector.Default.ExecuteScalar(sqlCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(this);
			}
		}
		
		/// <summary>
		/// Implementace metody aktualizuje data objektu v databázi.
		/// </summary>
		protected override sealed void Save_Update(DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE [dbo].[Subjekt] SET ");
			
			bool dirtyFieldExists = false;
			if (_NazevPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Nazev = @Nazev");
				
				SqlParameter sqlParameterNazev = new SqlParameter("@Nazev", SqlDbType.NVarChar, 50);
				sqlParameterNazev.Direction = ParameterDirection.Input;
				sqlParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
				sqlCommand.Parameters.Add(sqlParameterNazev);
				
				dirtyFieldExists = true;
			}
			
			if (_UzivatelPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("UzivatelID = @UzivatelID");
				
				SqlParameter sqlParameterUzivatelID = new SqlParameter("@UzivatelID", SqlDbType.Int);
				sqlParameterUzivatelID.Direction = ParameterDirection.Input;
				sqlParameterUzivatelID.Value = (_UzivatelPropertyHolder.Value == null) ? DBNull.Value : (object)_UzivatelPropertyHolder.Value.ID;
				sqlCommand.Parameters.Add(sqlParameterUzivatelID);
				
				dirtyFieldExists = true;
			}
			
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Deleted = @Deleted");
				
				SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.SmallDateTime);
				sqlParameterDeleted.Direction = ParameterDirection.Input;
				sqlParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterDeleted);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE SubjektID = @SubjektID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				SqlParameter sqlParameterSubjektID = new SqlParameter("@SubjektID", SqlDbType.Int);
				sqlParameterSubjektID.Direction = ParameterDirection.Input;
				sqlParameterSubjektID.Value = this.ID;
				sqlCommand.Parameters.Add(sqlParameterSubjektID);
				sqlCommand.CommandText = commandBuilder.ToString();
				DbConnector.Default.ExecuteNonQuery(sqlCommand);
			}
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForFullInsert(transaction);
			
			if (this.IsNew && (_UzivatelPropertyHolder.Value != null) && (_UzivatelPropertyHolder.Value.IsNew))
			{
				_UzivatelPropertyHolder.Value.Save_MinimalInsert(transaction);
			}
			
		}
		
		/// <summary>
		/// Metoda označí objekt jako smazaný a uloží jej.
		/// </summary>
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			if (IsNew)
			{
				throw new InvalidOperationException("Nelze smazat nový objekt.");
			}
			
			Deleted = System.DateTime.Now;
			Save(transaction);
		}
		
		#endregion
		
		#region GetFirst, GetList
		/// <summary>
		/// Vrátí první nalezený objekt typu Subjekt dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Subjekt GetFirst(QueryParams queryParams)
		{
			return Subjekt.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu Subjekt dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static Subjekt GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			queryParams.TopRecords = 1;
			SubjektCollection getListResult = Subjekt.GetList(queryParams, transaction);
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Subjekt dle parametrů v queryParams.
		/// </summary>
		public static SubjektCollection GetList(QueryParams queryParams)
		{
			return Subjekt.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu Subjekt dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		public static SubjektCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			queryParams.ObjectInfo = Subjekt.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Subjekt.Properties.ID);
			}
			
			queryParams.PrepareCommand(sqlCommand);
			return Subjekt.GetList(sqlCommand, queryParams.GetDataLoadPower());
		}
		
		private static SubjektCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			SubjektCollection result = new SubjektCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					result.Add(Subjekt.GetObject(dataRecord));
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí všechny (příznakem) nesmazané objekty typu Subjekt.
		/// </summary>
		public static SubjektCollection GetAll()
		{
			return Subjekt.GetAll(false);
		}
		
		/// <summary>
		/// Vrátí všechny objekty typu Subjekt. Parametr udává, zda se mají vrátit i (příznakem) smazané záznamy.
		/// </summary>
		public static SubjektCollection GetAll(bool includeDeleted)
		{
			SubjektCollection collection = null;
			QueryParams queryParams = new QueryParams();
			queryParams.IncludeDeleted = includeDeleted;
			collection = Subjekt.GetList(queryParams);
			return collection;
		}
		
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu Subjekt.
		/// </summary>
		public static ObjectInfo ObjectInfo
		{
			get
			{
				return objectInfo;
			}
		}
		private static ObjectInfo objectInfo;
		#endregion
		
		#region Properties
		/// <summary>
		/// Objektová reprezentace metadat vlastností typu Subjekt.
		/// </summary>
		public static SubjektProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static SubjektProperties properties;
		#endregion
		
	}
}
