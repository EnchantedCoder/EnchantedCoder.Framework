﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Havit.Collections;
using Havit.Business;
using Havit.Business.Query;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest
{
	/// <remarks>
	/// <code>
	/// CREATE TABLE [dbo].[Komunikace](
	/// 	[KomunikaceID] [int] IDENTITY(1,1) NOT NULL,
	/// 	[SubjektID] [int] NOT NULL,
	/// 	[ObjednavkaSepsaniID] [int] NULL,
	///  CONSTRAINT [PK_Komunikace] PRIMARY KEY CLUSTERED 
	/// (
	/// 	[KomunikaceID] ASC
	/// )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	/// ) ON [PRIMARY]
	/// ALTER TABLE [dbo].[Komunikace]  WITH CHECK ADD  CONSTRAINT [FK_Komunikace_ObjednavkaSepsani] FOREIGN KEY([ObjednavkaSepsaniID])
	/// REFERENCES [ObjednavkaSepsani] ([ObjednavkaSepsaniID])
	/// ALTER TABLE [dbo].[Komunikace] CHECK CONSTRAINT [FK_Komunikace_ObjednavkaSepsani]
	/// ALTER TABLE [dbo].[Komunikace]  WITH CHECK ADD  CONSTRAINT [FK_Komunikace_Subjekt] FOREIGN KEY([SubjektID])
	/// REFERENCES [Subjekt] ([SubjektID])
	/// ALTER TABLE [dbo].[Komunikace] CHECK CONSTRAINT [FK_Komunikace_Subjekt]
	/// </code>
	/// </remarks>
	[System.Diagnostics.Contracts.ContractVerification(false)]
	public abstract class KomunikaceBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static KomunikaceBase()
		{
			objectInfo = new ObjectInfo();
			properties = new KomunikaceProperties();
			objectInfo.Initialize("dbo", "Komunikace", "Komunikace", "Havit.BusinessLayerTest", false, Komunikace.CreateObject, Komunikace.GetObject, Komunikace.GetAll, null, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected KomunikaceBase()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">KomunikaceID (PK)</param>
		protected KomunikaceBase(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">KomunikaceID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected KomunikaceBase(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		///  [int, not-null]
		/// </summary>
		public virtual Havit.BusinessLayerTest.Subjekt Subjekt
		{
			get
			{
				EnsureLoaded();
				return _SubjektPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_SubjektPropertyHolder.Value = value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Subjekt.
		/// </summary>
		protected PropertyHolder<Havit.BusinessLayerTest.Subjekt> _SubjektPropertyHolder;
		
		/// <summary>
		///  [int, nullable]
		/// </summary>
		public virtual Havit.BusinessLayerTest.ObjednavkaSepsani ObjednavkaSepsani
		{
			get
			{
				EnsureLoaded();
				return _ObjednavkaSepsaniPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_ObjednavkaSepsaniPropertyHolder.Value = value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost ObjednavkaSepsani.
		/// </summary>
		protected PropertyHolder<Havit.BusinessLayerTest.ObjednavkaSepsani> _ObjednavkaSepsaniPropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_SubjektPropertyHolder = new PropertyHolder<Havit.BusinessLayerTest.Subjekt>(this);
			_ObjednavkaSepsaniPropertyHolder = new PropertyHolder<Havit.BusinessLayerTest.ObjednavkaSepsani>(this);
			
			if (IsNew)
			{
				_SubjektPropertyHolder.Value = null;
				_ObjednavkaSepsaniPropertyHolder.Value = null;
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
			
			if (_SubjektPropertyHolder.IsDirty && (_SubjektPropertyHolder.Value == null))
			{
				throw new ConstraintViolationException(this, "Vlastnost \"Subjekt\" nesmí nabývat hodnoty null.");
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
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.CommandText = "SELECT KomunikaceID, SubjektID, ObjednavkaSepsaniID FROM [dbo].[Komunikace] WHERE KomunikaceID = @KomunikaceID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterKomunikaceID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterKomunikaceID.DbType = DbType.Int32;
			dbParameterKomunikaceID.Direction = ParameterDirection.Input;
			dbParameterKomunikaceID.ParameterName = "KomunikaceID";
			dbParameterKomunikaceID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterKomunikaceID);
			
			result = DbConnector.Default.ExecuteDataRecord(dbCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		protected override sealed void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("KomunikaceID");
			
			int _tempSubjekt;
			if (record.TryGet<int>("SubjektID", out _tempSubjekt))
			{
				_SubjektPropertyHolder.Value = Havit.BusinessLayerTest.Subjekt.GetObject(_tempSubjekt);
			}
			
			int? _tempObjednavkaSepsani;
			if (record.TryGet<int?>("ObjednavkaSepsaniID", out _tempObjednavkaSepsani))
			{
				_ObjednavkaSepsaniPropertyHolder.Value = (_tempObjednavkaSepsani == null) ? null : Havit.BusinessLayerTest.ObjednavkaSepsani.GetObject(_tempObjednavkaSepsani.Value);
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete, Delete_Perform
		
		/// <summary>
		/// Ukládá member-objekty.
		/// </summary>
		protected override sealed void Save_SaveMembers(DbTransaction transaction)
		{
			base.Save_SaveMembers(transaction);
			
			if (_SubjektPropertyHolder.IsInitialized && (_SubjektPropertyHolder.Value != null))
			{
				_SubjektPropertyHolder.Value.Save(transaction);
			}
			
			if (_ObjednavkaSepsaniPropertyHolder.IsInitialized && (_ObjednavkaSepsaniPropertyHolder.Value != null))
			{
				_ObjednavkaSepsaniPropertyHolder.Value.Save(transaction);
			}
			
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		protected override sealed void Save_SaveCollections(DbTransaction transaction)
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
			
			DbCommand dbCommand;
			dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterSubjektID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterSubjektID.DbType = DbType.Int32;
			dbParameterSubjektID.Direction = ParameterDirection.Input;
			dbParameterSubjektID.ParameterName = "SubjektID";
			dbParameterSubjektID.Value = (_SubjektPropertyHolder.Value == null) ? DBNull.Value : (object)_SubjektPropertyHolder.Value.ID;
			dbCommand.Parameters.Add(dbParameterSubjektID);
			_SubjektPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @KomunikaceID INT; INSERT INTO [dbo].[Komunikace] (SubjektID) VALUES (@SubjektID); SELECT @KomunikaceID = SCOPE_IDENTITY(); SELECT @KomunikaceID; ";
			this.ID = (int)DbConnector.Default.ExecuteScalar(dbCommand);
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
			DbCommand dbCommand;
			dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterSubjektID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterSubjektID.DbType = DbType.Int32;
			dbParameterSubjektID.Direction = ParameterDirection.Input;
			dbParameterSubjektID.ParameterName = "SubjektID";
			dbParameterSubjektID.Value = (_SubjektPropertyHolder.Value == null) ? DBNull.Value : (object)_SubjektPropertyHolder.Value.ID;
			dbCommand.Parameters.Add(dbParameterSubjektID);
			_SubjektPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterObjednavkaSepsaniID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterObjednavkaSepsaniID.DbType = DbType.Int32;
			dbParameterObjednavkaSepsaniID.Direction = ParameterDirection.Input;
			dbParameterObjednavkaSepsaniID.ParameterName = "ObjednavkaSepsaniID";
			dbParameterObjednavkaSepsaniID.Value = (_ObjednavkaSepsaniPropertyHolder.Value == null) ? DBNull.Value : (object)_ObjednavkaSepsaniPropertyHolder.Value.ID;
			dbCommand.Parameters.Add(dbParameterObjednavkaSepsaniID);
			_ObjednavkaSepsaniPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @KomunikaceID INT; INSERT INTO [dbo].[Komunikace] (SubjektID, ObjednavkaSepsaniID) VALUES (@SubjektID, @ObjednavkaSepsaniID); SELECT @KomunikaceID = SCOPE_IDENTITY(); SELECT @KomunikaceID; ";
			this.ID = (int)DbConnector.Default.ExecuteScalar(dbCommand);
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
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE [dbo].[Komunikace] SET ");
			
			bool dirtyFieldExists = false;
			if (_SubjektPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("SubjektID = @SubjektID");
				
				DbParameter dbParameterSubjektID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterSubjektID.DbType = DbType.Int32;
				dbParameterSubjektID.Direction = ParameterDirection.Input;
				dbParameterSubjektID.ParameterName = "SubjektID";
				dbParameterSubjektID.Value = (_SubjektPropertyHolder.Value == null) ? DBNull.Value : (object)_SubjektPropertyHolder.Value.ID;
				dbCommand.Parameters.Add(dbParameterSubjektID);
				
				dirtyFieldExists = true;
			}
			
			if (_ObjednavkaSepsaniPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("ObjednavkaSepsaniID = @ObjednavkaSepsaniID");
				
				DbParameter dbParameterObjednavkaSepsaniID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterObjednavkaSepsaniID.DbType = DbType.Int32;
				dbParameterObjednavkaSepsaniID.Direction = ParameterDirection.Input;
				dbParameterObjednavkaSepsaniID.ParameterName = "ObjednavkaSepsaniID";
				dbParameterObjednavkaSepsaniID.Value = (_ObjednavkaSepsaniPropertyHolder.Value == null) ? DBNull.Value : (object)_ObjednavkaSepsaniPropertyHolder.Value.ID;
				dbCommand.Parameters.Add(dbParameterObjednavkaSepsaniID);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE KomunikaceID = @KomunikaceID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				DbParameter dbParameterKomunikaceID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterKomunikaceID.DbType = DbType.Int32;
				dbParameterKomunikaceID.Direction = ParameterDirection.Input;
				dbParameterKomunikaceID.ParameterName = "KomunikaceID";
				dbParameterKomunikaceID.Value = this.ID;
				dbCommand.Parameters.Add(dbParameterKomunikaceID);
				dbCommand.CommandText = commandBuilder.ToString();
				DbConnector.Default.ExecuteNonQuery(dbCommand);
			}
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
			if (_SubjektPropertyHolder.Value.IsNew)
			{
				_SubjektPropertyHolder.Value.Save_MinimalInsert(transaction);
			}
			
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForFullInsert(transaction);
			
			if (this.IsNew && (_ObjednavkaSepsaniPropertyHolder.Value != null) && (_ObjednavkaSepsaniPropertyHolder.Value.IsNew))
			{
				_ObjednavkaSepsaniPropertyHolder.Value.Save_MinimalInsert(transaction);
			}
			
		}
		
		/// <summary>
		/// Metoda vymaže objekt z perzistentního uložiště.
		/// </summary>
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			dbCommand.CommandText = "DELETE FROM [dbo].[Komunikace] WHERE KomunikaceID = @KomunikaceID";
			
			DbParameter dbParameterKomunikaceID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterKomunikaceID.DbType = DbType.Int32;
			dbParameterKomunikaceID.Direction = ParameterDirection.Input;
			dbParameterKomunikaceID.ParameterName = "KomunikaceID";
			dbParameterKomunikaceID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterKomunikaceID);
			
			DbConnector.Default.ExecuteNonQuery(dbCommand);
		}
		
		#endregion
		
		#region GetFirst, GetList, GetAll
		/// <summary>
		/// Vrátí první nalezený objekt typu Komunikace dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Komunikace GetFirst(QueryParams queryParams)
		{
			global::System.Diagnostics.Contracts.Contract.Requires(queryParams != null);
			
			return Komunikace.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu Komunikace dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static Komunikace GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			global::System.Diagnostics.Contracts.Contract.Requires(queryParams != null);
			
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			KomunikaceCollection getListResult = Komunikace.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Komunikace dle parametrů v queryParams.
		/// </summary>
		public static KomunikaceCollection GetList(QueryParams queryParams)
		{
			global::System.Diagnostics.Contracts.Contract.Requires(queryParams != null);
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<KomunikaceCollection>() != null);
			
			return Komunikace.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu Komunikace dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		public static KomunikaceCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			global::System.Diagnostics.Contracts.Contract.Requires(queryParams != null);
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<KomunikaceCollection>() != null);
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = Komunikace.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Komunikace.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand);
			return Komunikace.GetList(dbCommand, queryParams.GetDataLoadPower());
		}
		
		private static KomunikaceCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<KomunikaceCollection>() != null);
			
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			KomunikaceCollection result = new KomunikaceCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					result.Add(Komunikace.GetObject(dataRecord));
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí všechny objekty typu Komunikace.
		/// </summary>
		public static KomunikaceCollection GetAll()
		{
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<KomunikaceCollection>() != null);
			
			KomunikaceCollection collection = null;
			QueryParams queryParams = new QueryParams();
			collection = Komunikace.GetList(queryParams);
			return collection;
		}
		
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu Komunikace.
		/// </summary>
		public static ObjectInfo ObjectInfo
		{
			get
			{
				global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<ObjectInfo>() != null);
				
				return objectInfo;
			}
		}
		private static ObjectInfo objectInfo;
		#endregion
		
		#region Properties
		/// <summary>
		/// Objektová reprezentace metadat vlastností typu Komunikace.
		/// </summary>
		public static KomunikaceProperties Properties
		{
			get
			{
				global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<KomunikaceProperties>() != null);
				
				return properties;
			}
		}
		private static KomunikaceProperties properties;
		#endregion
		
	}
}
