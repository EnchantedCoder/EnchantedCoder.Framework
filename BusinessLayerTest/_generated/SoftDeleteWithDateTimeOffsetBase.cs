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
using System.Xml;
using Havit.Business;
using Havit.Business.Query;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlServer;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Tabulka pro ověření generovaného kódu pro soft-delete dle DateTimeOffset.
	/// </summary>
	/// <remarks>
	/// <code>
	/// CREATE TABLE [dbo].[SoftDeleteWithDateTimeOffset](
	/// 	[SoftDeleteWithDateTimeOffsetID] [int] IDENTITY(1,1) NOT NULL,
	/// 	[Deleted] [datetimeoffset](7) NULL,
	///  CONSTRAINT [PK_SoftDeleteWithDateTimeOffset] PRIMARY KEY CLUSTERED 
	/// (
	/// 	[SoftDeleteWithDateTimeOffsetID] ASC
	/// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	/// ) ON [PRIMARY]
	/// </code>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public abstract class SoftDeleteWithDateTimeOffsetBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static SoftDeleteWithDateTimeOffsetBase()
		{
			objectInfo = new ObjectInfo();
			properties = new SoftDeleteWithDateTimeOffsetProperties();
			objectInfo.Initialize("dbo", "SoftDeleteWithDateTimeOffset", "SoftDeleteWithDateTimeOffset", "Havit.BusinessLayerTest", false, SoftDeleteWithDateTimeOffset.CreateObject, SoftDeleteWithDateTimeOffset.GetObject, SoftDeleteWithDateTimeOffset.GetAll, properties.Deleted, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SoftDeleteWithDateTimeOffsetBase(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SoftDeleteWithDateTimeOffsetID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		protected SoftDeleteWithDateTimeOffsetBase(int id, ConnectionMode connectionMode) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">SoftDeleteWithDateTimeOffsetID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		protected SoftDeleteWithDateTimeOffsetBase(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Čas smazání objektu. [datetimeoffset, nullable]
		/// </summary>
		public virtual DateTimeOffset? Deleted
		{
			get
			{
				EnsureLoaded();
				return _DeletedPropertyHolder.Value;
			}
			protected set
			{
				EnsureLoaded();
				
				if (!Object.Equals(_DeletedPropertyHolder.Value, value))
				{
					DateTimeOffset? oldValue = _DeletedPropertyHolder.Value;
					_DeletedPropertyHolder.Value = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Deleted), oldValue, value));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Deleted.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<DateTimeOffset?> _DeletedPropertyHolder;
		
		#endregion
		
		#region IsDeleted
		/// <summary>
		/// Indikuje, zda je nastaven příznak smazaného záznamu.
		/// </summary>
		public override bool IsDeleted
		{
			get
			{
				return Deleted != null;
			}
		}
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_DeletedPropertyHolder = new PropertyHolder<DateTimeOffset?>(this);
			
			if (IsNew || IsDisconnected)
			{
				_DeletedPropertyHolder.Value = null;
			}
			
			base.Init();
		}
		#endregion
		
		#region CleanDirty
		/// <summary>
		/// Nastaví property holderům IsDirty na false.
		/// </summary>
		protected override void CleanDirty()
		{
			base.CleanDirty();
			
			_DeletedPropertyHolder.IsDirty = false;
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
		/// <param name="transaction">Transakce.</param>
		/// <returns>Úplná data objektu.</returns>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed DataRecord Load_GetDataRecord(DbTransaction transaction)
		{
			DataRecord result;
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.CommandText = "SELECT [SoftDeleteWithDateTimeOffsetID], [Deleted] FROM [dbo].[SoftDeleteWithDateTimeOffset] WHERE [SoftDeleteWithDateTimeOffsetID] = @SoftDeleteWithDateTimeOffsetID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterSoftDeleteWithDateTimeOffsetID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterSoftDeleteWithDateTimeOffsetID.DbType = DbType.Int32;
			dbParameterSoftDeleteWithDateTimeOffsetID.Direction = ParameterDirection.Input;
			dbParameterSoftDeleteWithDateTimeOffsetID.ParameterName = "SoftDeleteWithDateTimeOffsetID";
			dbParameterSoftDeleteWithDateTimeOffsetID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterSoftDeleteWithDateTimeOffsetID);
			
			result = DbConnector.Default.ExecuteDataRecord(dbCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("SoftDeleteWithDateTimeOffsetID");
			
			DateTimeOffset? _tempDeleted;
			if (record.TryGet<DateTimeOffset?>("Deleted", out _tempDeleted))
			{
				_DeletedPropertyHolder.Value = _tempDeleted;
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete, Delete_Perform
		
		/// <summary>
		/// Ukládá member-objekty.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_SaveMembers(DbTransaction transaction)
		{
			base.Save_SaveMembers(transaction);
			
			// Není co ukládat.
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			// Není co ukládat.
		}
		
		/// <summary>
		/// Implementace metody vloží jen not-null vlastnosti objektu do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		public override sealed void Save_MinimalInsert(DbTransaction transaction)
		{
			base.Save_MinimalInsert(transaction);
			Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
			DbCommand dbCommand;
			dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterDeleted.DbType = DbType.DateTimeOffset;
			dbParameterDeleted.Direction = ParameterDirection.Input;
			dbParameterDeleted.ParameterName = "Deleted";
			dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @SoftDeleteWithDateTimeOffsetID INT; INSERT INTO [dbo].[SoftDeleteWithDateTimeOffset] ([Deleted]) VALUES (@Deleted); SELECT @SoftDeleteWithDateTimeOffsetID = SCOPE_IDENTITY(); SELECT @SoftDeleteWithDateTimeOffsetID; ";
			this.ID = (int)DbConnector.Default.ExecuteScalar(dbCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			global::Havit.Diagnostics.Contracts.Contract.Assert(currentIdentityMap != null, "currentIdentityMap != null");
			currentIdentityMap.Store(this);
		}
		
		/// <summary>
		/// Implementace metody vloží nový objekt do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_FullInsert(DbTransaction transaction)
		{
			DbCommand dbCommand;
			dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterDeleted.DbType = DbType.DateTimeOffset;
			dbParameterDeleted.Direction = ParameterDirection.Input;
			dbParameterDeleted.ParameterName = "Deleted";
			dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @SoftDeleteWithDateTimeOffsetID INT; INSERT INTO [dbo].[SoftDeleteWithDateTimeOffset] ([Deleted]) VALUES (@Deleted); SELECT @SoftDeleteWithDateTimeOffsetID = SCOPE_IDENTITY(); SELECT @SoftDeleteWithDateTimeOffsetID; ";
			this.ID = (int)DbConnector.Default.ExecuteScalar(dbCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			global::Havit.Diagnostics.Contracts.Contract.Assert(currentIdentityMap != null, "currentIdentityMap != null");
			currentIdentityMap.Store(this);
			
			InvalidateAnySaveCacheDependencyKey();
		}
		
		/// <summary>
		/// Implementace metody aktualizuje data objektu v databázi.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Update(DbTransaction transaction)
		{
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE [dbo].[SoftDeleteWithDateTimeOffset] SET ");
			
			bool dirtyFieldExists = false;
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[Deleted] = @Deleted");
				
				DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterDeleted.DbType = DbType.DateTimeOffset;
				dbParameterDeleted.Direction = ParameterDirection.Input;
				dbParameterDeleted.ParameterName = "Deleted";
				dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
				dbCommand.Parameters.Add(dbParameterDeleted);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE [SoftDeleteWithDateTimeOffsetID] = @SoftDeleteWithDateTimeOffsetID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				DbParameter dbParameterSoftDeleteWithDateTimeOffsetID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterSoftDeleteWithDateTimeOffsetID.DbType = DbType.Int32;
				dbParameterSoftDeleteWithDateTimeOffsetID.Direction = ParameterDirection.Input;
				dbParameterSoftDeleteWithDateTimeOffsetID.ParameterName = "SoftDeleteWithDateTimeOffsetID";
				dbParameterSoftDeleteWithDateTimeOffsetID.Value = this.ID;
				dbCommand.Parameters.Add(dbParameterSoftDeleteWithDateTimeOffsetID);
				dbCommand.CommandText = commandBuilder.ToString();
				DbConnector.Default.ExecuteNonQuery(dbCommand);
			}
			
			InvalidateSaveCacheDependencyKey();
			InvalidateAnySaveCacheDependencyKey();
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForFullInsert(transaction);
			
		}
		
		/// <summary>
		/// Smaže objekt, nebo ho označí jako smazaný, podle zvolené logiky. Změnu uloží do databáze, v transakci.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je již objekt smazán.
		/// </remarks>
		/// <param name="transaction">Transakce DbTransaction, v rámci které se smazání provede; null, pokud bez transakce.</param>
		public override void Delete(DbTransaction transaction)
		{
			EnsureLoaded(transaction);
			if (Deleted == null)
			{
				Deleted = System.DateTime.Now;
			}
			base.Delete(transaction);
		}
		
		/// <summary>
		/// Metoda označí objekt jako smazaný a uloží jej.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			Save_Update(transaction);
		}
		
		#endregion
		
		#region Cache dependencies methods
		/// <summary>
		/// Vrátí klíč pro tvorbu závislostí cache na objektu. Při uložení objektu jsou závislosti invalidovány.
		/// </summary>
		public string GetSaveCacheDependencyKey(bool ensureInCache = true)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(!this.IsNew, "!this.IsNew");
			
			if (!Havit.Business.BusinessLayerContext.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				throw new InvalidOperationException("Použitá BusinessLayerCacheService nepodporuje cache dependencies.");
			}
			
			string key = "BL|SoftDeleteWithDateTimeOffset|SaveDK|" + this.ID;
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(SoftDeleteWithDateTimeOffset), key);
			}
			
			return key;
		}
		
		/// <summary>
		/// Odstraní z cache závislosti na klíči CacheDependencyKey.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected void InvalidateSaveCacheDependencyKey()
		{
			if (Havit.Business.BusinessLayerContext.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(SoftDeleteWithDateTimeOffset), GetSaveCacheDependencyKey(false));
			}
		}
		
		/// <summary>
		/// Vrátí klíč pro tvorbu závislostí cache. Po uložení jakéhokoliv objektu této třídy jsou závislosti invalidovány.
		/// </summary>
		public static string GetAnySaveCacheDependencyKey(bool ensureInCache = true)
		{
			if (!Havit.Business.BusinessLayerContext.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				throw new InvalidOperationException("Použitá BusinessLayerCacheService nepodporuje cache dependencies.");
			}
			
			string key = "BL|SoftDeleteWithDateTimeOffset|AnySaveDK";
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(SoftDeleteWithDateTimeOffset), key);
			}
			
			return key;
		}
		
		/// <summary>
		/// Odstraní z cache závislosti na klíči AnySaveCacheDependencyKey.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected static void InvalidateAnySaveCacheDependencyKey()
		{
			if (Havit.Business.BusinessLayerContext.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(SoftDeleteWithDateTimeOffset), GetAnySaveCacheDependencyKey(false));
			}
		}
		#endregion
		
		#region GetFirst, GetList, GetAll
		/// <summary>
		/// Vrátí první nalezený objekt typu SoftDeleteWithDateTimeOffset dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static SoftDeleteWithDateTimeOffset GetFirst(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return SoftDeleteWithDateTimeOffset.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu SoftDeleteWithDateTimeOffset dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static SoftDeleteWithDateTimeOffset GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			SoftDeleteWithDateTimeOffsetCollection getListResult = SoftDeleteWithDateTimeOffset.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu SoftDeleteWithDateTimeOffset dle parametrů v queryParams.
		/// </summary>
		internal static SoftDeleteWithDateTimeOffsetCollection GetList(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return SoftDeleteWithDateTimeOffset.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu SoftDeleteWithDateTimeOffset dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		internal static SoftDeleteWithDateTimeOffsetCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = SoftDeleteWithDateTimeOffset.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(SoftDeleteWithDateTimeOffset.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
			return SoftDeleteWithDateTimeOffset.GetList(dbCommand, queryParams.GetDataLoadPower());
		}
		
		private static SoftDeleteWithDateTimeOffsetCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			SoftDeleteWithDateTimeOffsetCollection result = new SoftDeleteWithDateTimeOffsetCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					SoftDeleteWithDateTimeOffset softDeleteWithDateTimeOffset = SoftDeleteWithDateTimeOffset.GetObject(dataRecord);
					result.Add(softDeleteWithDateTimeOffset);
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí všechny (příznakem) nesmazané objekty typu SoftDeleteWithDateTimeOffset.
		/// </summary>
		public static SoftDeleteWithDateTimeOffsetCollection GetAll()
		{
			return SoftDeleteWithDateTimeOffset.GetAll(false);
		}
		
		/// <summary>
		/// Vrátí všechny objekty typu SoftDeleteWithDateTimeOffset. Parametr udává, zda se mají vrátit i (příznakem) smazané záznamy.
		/// </summary>
		public static SoftDeleteWithDateTimeOffsetCollection GetAll(bool includeDeleted)
		{
			SoftDeleteWithDateTimeOffsetCollection collection = null;
			QueryParams queryParams = new QueryParams();
			queryParams.IncludeDeleted = includeDeleted;
			collection = SoftDeleteWithDateTimeOffset.GetList(queryParams);
			return collection;
		}
		
		#endregion
		
		#region ToString
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		public override string ToString()
		{
			if (IsNew)
			{
				return "SoftDeleteWithDateTimeOffset(New)";
			}
			
			return String.Format("SoftDeleteWithDateTimeOffset(ID={0})", this.ID);
		}
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu SoftDeleteWithDateTimeOffset.
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
		/// Objektová reprezentace metadat vlastností typu SoftDeleteWithDateTimeOffset.
		/// </summary>
		public static SoftDeleteWithDateTimeOffsetProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static SoftDeleteWithDateTimeOffsetProperties properties;
		#endregion
		
	}
}
