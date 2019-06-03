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
using Havit.Business;
using Havit.Business.Query;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlServer;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Objednávka sepsání zápisu.
	/// </summary>
	/// <remarks>
	/// <code>
	/// CREATE TABLE [dbo].[ObjednavkaSepsani](
	/// 	[ObjednavkaSepsaniID] [int] IDENTITY(1,1) NOT NULL,
	/// 	[StornoKomunikaceID] [int] NULL,
	///  CONSTRAINT [PK_ObjednavkaSepsani] PRIMARY KEY CLUSTERED 
	/// (
	/// 	[ObjednavkaSepsaniID] ASC
	/// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	/// ) ON [PRIMARY]
	/// ALTER TABLE [dbo].[ObjednavkaSepsani]  WITH NOCHECK ADD  CONSTRAINT [FK_ObjednavkaSepsani_Komunikace] FOREIGN KEY([StornoKomunikaceID])
	/// REFERENCES [dbo].[Komunikace] ([KomunikaceID])
	/// ALTER TABLE [dbo].[ObjednavkaSepsani] CHECK CONSTRAINT [FK_ObjednavkaSepsani_Komunikace]
	/// </code>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public abstract class ObjednavkaSepsaniBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static ObjednavkaSepsaniBase()
		{
			objectInfo = new ObjectInfo();
			properties = new ObjednavkaSepsaniProperties();
			objectInfo.Initialize("dbo", "ObjednavkaSepsani", "ObjednavkaSepsani", "Havit.BusinessLayerTest", false, ObjednavkaSepsani.CreateObject, ObjednavkaSepsani.GetObject, ObjednavkaSepsani.GetAll, null, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected ObjednavkaSepsaniBase(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">ObjednavkaSepsaniID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		protected ObjednavkaSepsaniBase(int id, ConnectionMode connectionMode) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">ObjednavkaSepsaniID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		protected ObjednavkaSepsaniBase(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Odkaz na komunikaci, která stornuje tuto objednávku. [int, nullable]
		/// </summary>
		public virtual Havit.BusinessLayerTest.Komunikace StornoKomunikace
		{
			get
			{
				EnsureLoaded();
				return _StornoKomunikacePropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				
				if (!Object.Equals(_StornoKomunikacePropertyHolder.Value, value))
				{
					Havit.BusinessLayerTest.Komunikace oldValue = _StornoKomunikacePropertyHolder.Value;
					_StornoKomunikacePropertyHolder.Value = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(StornoKomunikace), oldValue, value));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost StornoKomunikace.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<Havit.BusinessLayerTest.Komunikace> _StornoKomunikacePropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_StornoKomunikacePropertyHolder = new PropertyHolder<Havit.BusinessLayerTest.Komunikace>(this);
			
			if (IsNew || IsDisconnected)
			{
				_StornoKomunikacePropertyHolder.Value = null;
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
			
			_StornoKomunikacePropertyHolder.IsDirty = false;
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
			dbCommand.CommandText = "SELECT [ObjednavkaSepsaniID], [StornoKomunikaceID] FROM [dbo].[ObjednavkaSepsani] WHERE [ObjednavkaSepsaniID] = @ObjednavkaSepsaniID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterObjednavkaSepsaniID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterObjednavkaSepsaniID.DbType = DbType.Int32;
			dbParameterObjednavkaSepsaniID.Direction = ParameterDirection.Input;
			dbParameterObjednavkaSepsaniID.ParameterName = "ObjednavkaSepsaniID";
			dbParameterObjednavkaSepsaniID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterObjednavkaSepsaniID);
			
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
			this.ID = record.Get<int>("ObjednavkaSepsaniID");
			
			int? _tempStornoKomunikace;
			if (record.TryGet<int?>("StornoKomunikaceID", out _tempStornoKomunikace))
			{
				_StornoKomunikacePropertyHolder.Value = (_tempStornoKomunikace == null) ? null : Havit.BusinessLayerTest.Komunikace.GetObject(_tempStornoKomunikace.Value);
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
			
			if (_StornoKomunikacePropertyHolder.IsInitialized && (_StornoKomunikacePropertyHolder.Value != null))
			{
				_StornoKomunikacePropertyHolder.Value.Save(transaction);
			}
			
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
			
			dbCommand.CommandText = "DECLARE @ObjednavkaSepsaniID INT; INSERT INTO [dbo].[ObjednavkaSepsani] DEFAULT VALUES; SELECT @ObjednavkaSepsaniID = SCOPE_IDENTITY(); SELECT @ObjednavkaSepsaniID; ";
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
			
			DbParameter dbParameterStornoKomunikaceID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterStornoKomunikaceID.DbType = DbType.Int32;
			dbParameterStornoKomunikaceID.Direction = ParameterDirection.Input;
			dbParameterStornoKomunikaceID.ParameterName = "StornoKomunikaceID";
			dbParameterStornoKomunikaceID.Value = (_StornoKomunikacePropertyHolder.Value == null) ? DBNull.Value : (object)_StornoKomunikacePropertyHolder.Value.ID;
			dbCommand.Parameters.Add(dbParameterStornoKomunikaceID);
			_StornoKomunikacePropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @ObjednavkaSepsaniID INT; INSERT INTO [dbo].[ObjednavkaSepsani] ([StornoKomunikaceID]) VALUES (@StornoKomunikaceID); SELECT @ObjednavkaSepsaniID = SCOPE_IDENTITY(); SELECT @ObjednavkaSepsaniID; ";
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
			commandBuilder.Append("UPDATE [dbo].[ObjednavkaSepsani] SET ");
			
			bool dirtyFieldExists = false;
			if (_StornoKomunikacePropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[StornoKomunikaceID] = @StornoKomunikaceID");
				
				DbParameter dbParameterStornoKomunikaceID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterStornoKomunikaceID.DbType = DbType.Int32;
				dbParameterStornoKomunikaceID.Direction = ParameterDirection.Input;
				dbParameterStornoKomunikaceID.ParameterName = "StornoKomunikaceID";
				dbParameterStornoKomunikaceID.Value = (_StornoKomunikacePropertyHolder.Value == null) ? DBNull.Value : (object)_StornoKomunikacePropertyHolder.Value.ID;
				dbCommand.Parameters.Add(dbParameterStornoKomunikaceID);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE [ObjednavkaSepsaniID] = @ObjednavkaSepsaniID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				DbParameter dbParameterObjednavkaSepsaniID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterObjednavkaSepsaniID.DbType = DbType.Int32;
				dbParameterObjednavkaSepsaniID.Direction = ParameterDirection.Input;
				dbParameterObjednavkaSepsaniID.ParameterName = "ObjednavkaSepsaniID";
				dbParameterObjednavkaSepsaniID.Value = this.ID;
				dbCommand.Parameters.Add(dbParameterObjednavkaSepsaniID);
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
			
			if (this.IsNew && (_StornoKomunikacePropertyHolder.Value != null) && (_StornoKomunikacePropertyHolder.Value.IsNew))
			{
				_StornoKomunikacePropertyHolder.Value.Save_MinimalInsert(transaction);
			}
			
		}
		
		/// <summary>
		/// Metoda vymaže objekt z perzistentního uložiště.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.AppendFormat("DELETE FROM [dbo].[ObjednavkaSepsani] WHERE [ObjednavkaSepsaniID] = @ObjednavkaSepsaniID");
			
			DbParameter dbParameterObjednavkaSepsaniID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterObjednavkaSepsaniID.DbType = DbType.Int32;
			dbParameterObjednavkaSepsaniID.Direction = ParameterDirection.Input;
			dbParameterObjednavkaSepsaniID.ParameterName = "ObjednavkaSepsaniID";
			dbParameterObjednavkaSepsaniID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterObjednavkaSepsaniID);
			
			dbCommand.CommandText = commandBuilder.ToString();
			DbConnector.Default.ExecuteNonQuery(dbCommand);
			
			InvalidateSaveCacheDependencyKey();
			InvalidateAnySaveCacheDependencyKey();
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
			
			string key = "BL|ObjednavkaSepsani|SaveDK|" + this.ID;
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(ObjednavkaSepsani), key);
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
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(ObjednavkaSepsani), GetSaveCacheDependencyKey(false));
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
			
			string key = "BL|ObjednavkaSepsani|AnySaveDK";
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(ObjednavkaSepsani), key);
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
				Havit.Business.BusinessLayerContext.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(ObjednavkaSepsani), GetAnySaveCacheDependencyKey(false));
			}
		}
		#endregion
		
		#region GetFirst, GetList, GetAll
		/// <summary>
		/// Vrátí první nalezený objekt typu ObjednavkaSepsani dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static ObjednavkaSepsani GetFirst(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return ObjednavkaSepsani.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu ObjednavkaSepsani dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static ObjednavkaSepsani GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			ObjednavkaSepsaniCollection getListResult = ObjednavkaSepsani.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu ObjednavkaSepsani dle parametrů v queryParams.
		/// </summary>
		internal static ObjednavkaSepsaniCollection GetList(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return ObjednavkaSepsani.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu ObjednavkaSepsani dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		internal static ObjednavkaSepsaniCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = ObjednavkaSepsani.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(ObjednavkaSepsani.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
			return ObjednavkaSepsani.GetList(dbCommand, queryParams.GetDataLoadPower());
		}
		
		private static ObjednavkaSepsaniCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			ObjednavkaSepsaniCollection result = new ObjednavkaSepsaniCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					ObjednavkaSepsani objednavkaSepsani = ObjednavkaSepsani.GetObject(dataRecord);
					result.Add(objednavkaSepsani);
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí všechny objekty typu ObjednavkaSepsani.
		/// </summary>
		public static ObjednavkaSepsaniCollection GetAll()
		{
			ObjednavkaSepsaniCollection collection = null;
			QueryParams queryParams = new QueryParams();
			collection = ObjednavkaSepsani.GetList(queryParams);
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
				return "ObjednavkaSepsani(New)";
			}
			
			return String.Format("ObjednavkaSepsani(ID={0})", this.ID);
		}
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu ObjednavkaSepsani.
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
		/// Objektová reprezentace metadat vlastností typu ObjednavkaSepsani.
		/// </summary>
		public static ObjednavkaSepsaniProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static ObjednavkaSepsaniProperties properties;
		#endregion
		
	}
}
