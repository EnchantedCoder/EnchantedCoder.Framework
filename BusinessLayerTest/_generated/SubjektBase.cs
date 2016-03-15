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
	/// Subjekt.
	/// </summary>
	/// <remarks>
	/// <code>
	/// CREATE TABLE [dbo].[Subjekt](
	/// 	[SubjektID] [int] IDENTITY(1,1) NOT NULL,
	/// 	[Nazev] [nvarchar](50) COLLATE Czech_CI_AS NULL CONSTRAINT [DF_Subjekt_Nazev]  DEFAULT (''),
	/// 	[UzivatelID] [int] NULL,
	/// 	[Created] [smalldatetime] NOT NULL CONSTRAINT [DF_Subjekt_Created]  DEFAULT (getdate()),
	/// 	[Deleted] [smalldatetime] NULL,
	///  CONSTRAINT [PK_Subjekt] PRIMARY KEY CLUSTERED 
	/// (
	/// 	[SubjektID] ASC
	/// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	/// ) ON [PRIMARY]
	/// ALTER TABLE [dbo].[Subjekt]  WITH NOCHECK ADD  CONSTRAINT [FK_Subjekt_Uzivatel] FOREIGN KEY([UzivatelID])
	/// REFERENCES [dbo].[Uzivatel] ([UzivatelID])
	/// ALTER TABLE [dbo].[Subjekt] CHECK CONSTRAINT [FK_Subjekt_Uzivatel]
	/// </code>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public abstract class SubjektBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static SubjektBase()
		{
			objectInfo = new ObjectInfo();
			properties = new SubjektProperties();
			objectInfo.Initialize("dbo", "Subjekt", "Subjekt", "Havit.BusinessLayerTest", false, Subjekt.CreateObject, Subjekt.GetObject, Subjekt.GetAll, properties.Deleted, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SubjektBase(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SubjektID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		protected SubjektBase(int id, ConnectionMode connectionMode) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">SubjektID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		protected SubjektBase(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Název. [nvarchar(50), nullable, default '']
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
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<string> _NazevPropertyHolder;
		
		/// <summary>
		/// Uživatel (login). [int, nullable]
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
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<Havit.BusinessLayerTest.Uzivatel> _UzivatelPropertyHolder;
		
		/// <summary>
		/// Čas vytvoření objektu. [smalldatetime, not-null, read-only, default getdate()]
		/// </summary>
		public virtual DateTime Created
		{
			get
			{
				EnsureLoaded();
				return _CreatedPropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				_CreatedPropertyHolder.Value = value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Created.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<DateTime> _CreatedPropertyHolder;
		
		/// <summary>
		/// Čas smazání objektu. [smalldatetime, nullable]
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
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<DateTime?> _DeletedPropertyHolder;
		
		/// <summary>
		/// Komunikace subjektu.
		/// </summary>
		public virtual Havit.BusinessLayerTest.KomunikaceCollection Komunikace
		{
			get
			{
				EnsureLoaded();
				return _KomunikacePropertyHolder.Value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Komunikace.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected CollectionPropertyHolder<Havit.BusinessLayerTest.KomunikaceCollection, Havit.BusinessLayerTest.Komunikace> _KomunikacePropertyHolder;
		private Havit.BusinessLayerTest.KomunikaceCollection _loadedKomunikaceValues;
		
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
			_NazevPropertyHolder = new PropertyHolder<string>(this);
			_UzivatelPropertyHolder = new PropertyHolder<Havit.BusinessLayerTest.Uzivatel>(this);
			_CreatedPropertyHolder = new PropertyHolder<DateTime>(this);
			_DeletedPropertyHolder = new PropertyHolder<DateTime?>(this);
			_KomunikacePropertyHolder = new CollectionPropertyHolder<Havit.BusinessLayerTest.KomunikaceCollection, Havit.BusinessLayerTest.Komunikace>(this);
			
			if (IsNew || IsDisconnected)
			{
				_NazevPropertyHolder.Value = String.Empty;
				_UzivatelPropertyHolder.Value = null;
				_CreatedPropertyHolder.Value = System.DateTime.Now;
				_DeletedPropertyHolder.Value = null;
				_KomunikacePropertyHolder.Initialize();
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
			
			_NazevPropertyHolder.IsDirty = false;
			_UzivatelPropertyHolder.IsDirty = false;
			_CreatedPropertyHolder.IsDirty = false;
			_DeletedPropertyHolder.IsDirty = false;
			_KomunikacePropertyHolder.IsDirty = false;
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
				throw new ConstraintViolationException(this, "Vlastnost \"Nazev\" - řetězec přesáhl maximální délku 50 znaků.");
			}
			
			if (_CreatedPropertyHolder.IsDirty)
			{
				if ((_CreatedPropertyHolder.Value < Havit.Data.SqlTypes.SqlSmallDateTime.MinValue.Value) || (_CreatedPropertyHolder.Value > Havit.Data.SqlTypes.SqlSmallDateTime.MaxValue.Value))
				{
					throw new ConstraintViolationException(this, "PropertyHolder \"_CreatedPropertyHolder\" nesmí nabývat hodnoty mimo rozsah SqlSmallDateTime.MinValue-SqlSmallDateTime.MaxValue.");
				}
			}
			
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (_DeletedPropertyHolder.Value != null)
				{
					if ((_DeletedPropertyHolder.Value.Value < Havit.Data.SqlTypes.SqlSmallDateTime.MinValue.Value) || (_DeletedPropertyHolder.Value.Value > Havit.Data.SqlTypes.SqlSmallDateTime.MaxValue.Value))
					{
						throw new ConstraintViolationException(this, "Vlastnost \"Deleted\" nesmí nabývat hodnoty mimo rozsah SqlSmallDateTime.MinValue-SqlSmallDateTime.MaxValue.");
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
			dbCommand.CommandText = "SELECT [SubjektID], [Nazev], [UzivatelID], [Created], [Deleted], (SELECT CAST([_items].[KomunikaceID] AS NVARCHAR(11)) + '|' FROM [dbo].[Komunikace] AS [_items] WHERE ([_items].[SubjektID] = @SubjektID) FOR XML PATH('')) AS [Komunikace] FROM [dbo].[Subjekt] WHERE [SubjektID] = @SubjektID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterSubjektID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterSubjektID.DbType = DbType.Int32;
			dbParameterSubjektID.Direction = ParameterDirection.Input;
			dbParameterSubjektID.ParameterName = "SubjektID";
			dbParameterSubjektID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterSubjektID);
			
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
			this.ID = record.Get<int>("SubjektID");
			
			string _tempNazev;
			if (record.TryGet<string>("Nazev", out _tempNazev))
			{
				_NazevPropertyHolder.Value = _tempNazev ?? String.Empty;
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
			
			string _tempKomunikace;
			if (record.TryGet<string>("Komunikace", out _tempKomunikace))
			{
				_KomunikacePropertyHolder.Initialize();
				_KomunikacePropertyHolder.Value.Clear();
				if (_tempKomunikace != null)
				{
					_KomunikacePropertyHolder.Value.AllowDuplicates = true; // Z výkonových důvodů. Víme, že duplicity nepřidáme.
					string[] _tempKomunikaceItems = _tempKomunikace.Split('|');
					int _tempKomunikaceItemsLength = _tempKomunikaceItems.Length - 1; // za každou (i za poslední) položkou je oddělovač
					for (int i = 0; i < _tempKomunikaceItemsLength; i++)
					{
						_KomunikacePropertyHolder.Value.Add(Havit.BusinessLayerTest.Komunikace.GetObject(BusinessObjectBase.FastIntParse(_tempKomunikaceItems[i])));
					}
					_KomunikacePropertyHolder.Value.AllowDuplicates = false;
					_loadedKomunikaceValues = new Havit.BusinessLayerTest.KomunikaceCollection(_KomunikacePropertyHolder.Value);
				}
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
			
			if (_UzivatelPropertyHolder.IsInitialized && (_UzivatelPropertyHolder.Value != null))
			{
				_UzivatelPropertyHolder.Value.Save(transaction);
			}
			
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			if (_KomunikacePropertyHolder.IsInitialized)
			{
				_KomunikacePropertyHolder.Value.SaveAll(transaction);
			}
			
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
			
			DbParameter dbParameterNazev = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterNazev.DbType = DbType.String;
			dbParameterNazev.Size = 50;
			dbParameterNazev.Direction = ParameterDirection.Input;
			dbParameterNazev.ParameterName = "Nazev";
			dbParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
			dbCommand.Parameters.Add(dbParameterNazev);
			_NazevPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterCreated = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterCreated.DbType = DbType.DateTime;
			dbParameterCreated.Direction = ParameterDirection.Input;
			dbParameterCreated.ParameterName = "Created";
			dbParameterCreated.Value = _CreatedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterCreated);
			_CreatedPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterDeleted.DbType = DbType.DateTime;
			dbParameterDeleted.Direction = ParameterDirection.Input;
			dbParameterDeleted.ParameterName = "Deleted";
			dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @SubjektID INT; INSERT INTO [dbo].[Subjekt] ([Nazev], [Created], [Deleted]) VALUES (@Nazev, @Created, @Deleted); SELECT @SubjektID = SCOPE_IDENTITY(); SELECT @SubjektID; ";
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
			
			DbParameter dbParameterNazev = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterNazev.DbType = DbType.String;
			dbParameterNazev.Size = 50;
			dbParameterNazev.Direction = ParameterDirection.Input;
			dbParameterNazev.ParameterName = "Nazev";
			dbParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
			dbCommand.Parameters.Add(dbParameterNazev);
			_NazevPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterUzivatelID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterUzivatelID.DbType = DbType.Int32;
			dbParameterUzivatelID.Direction = ParameterDirection.Input;
			dbParameterUzivatelID.ParameterName = "UzivatelID";
			dbParameterUzivatelID.Value = (_UzivatelPropertyHolder.Value == null) ? DBNull.Value : (object)_UzivatelPropertyHolder.Value.ID;
			dbCommand.Parameters.Add(dbParameterUzivatelID);
			_UzivatelPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterCreated = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterCreated.DbType = DbType.DateTime;
			dbParameterCreated.Direction = ParameterDirection.Input;
			dbParameterCreated.ParameterName = "Created";
			dbParameterCreated.Value = _CreatedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterCreated);
			_CreatedPropertyHolder.IsDirty = false;
			
			DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterDeleted.DbType = DbType.DateTime;
			dbParameterDeleted.Direction = ParameterDirection.Input;
			dbParameterDeleted.ParameterName = "Deleted";
			dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
			dbCommand.Parameters.Add(dbParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			dbCommand.CommandText = "DECLARE @SubjektID INT; INSERT INTO [dbo].[Subjekt] ([Nazev], [UzivatelID], [Created], [Deleted]) VALUES (@Nazev, @UzivatelID, @Created, @Deleted); SELECT @SubjektID = SCOPE_IDENTITY(); SELECT @SubjektID; ";
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
			commandBuilder.Append("UPDATE [dbo].[Subjekt] SET ");
			
			bool dirtyFieldExists = false;
			if (_NazevPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[Nazev] = @Nazev");
				
				DbParameter dbParameterNazev = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterNazev.DbType = DbType.String;
				dbParameterNazev.Size = 50;
				dbParameterNazev.Direction = ParameterDirection.Input;
				dbParameterNazev.ParameterName = "Nazev";
				dbParameterNazev.Value = _NazevPropertyHolder.Value ?? String.Empty;
				dbCommand.Parameters.Add(dbParameterNazev);
				
				dirtyFieldExists = true;
			}
			
			if (_UzivatelPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[UzivatelID] = @UzivatelID");
				
				DbParameter dbParameterUzivatelID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterUzivatelID.DbType = DbType.Int32;
				dbParameterUzivatelID.Direction = ParameterDirection.Input;
				dbParameterUzivatelID.ParameterName = "UzivatelID";
				dbParameterUzivatelID.Value = (_UzivatelPropertyHolder.Value == null) ? DBNull.Value : (object)_UzivatelPropertyHolder.Value.ID;
				dbCommand.Parameters.Add(dbParameterUzivatelID);
				
				dirtyFieldExists = true;
			}
			
			if (_CreatedPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[Created] = @Created");
				
				DbParameter dbParameterCreated = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterCreated.DbType = DbType.DateTime;
				dbParameterCreated.Direction = ParameterDirection.Input;
				dbParameterCreated.ParameterName = "Created";
				dbParameterCreated.Value = _CreatedPropertyHolder.Value;
				dbCommand.Parameters.Add(dbParameterCreated);
				
				dirtyFieldExists = true;
			}
			
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("[Deleted] = @Deleted");
				
				DbParameter dbParameterDeleted = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterDeleted.DbType = DbType.DateTime;
				dbParameterDeleted.Direction = ParameterDirection.Input;
				dbParameterDeleted.ParameterName = "Deleted";
				dbParameterDeleted.Value = (_DeletedPropertyHolder.Value == null) ? DBNull.Value : (object)_DeletedPropertyHolder.Value;
				dbCommand.Parameters.Add(dbParameterDeleted);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE [SubjektID] = @SubjektID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			if (_KomunikacePropertyHolder.IsDirty && (_loadedKomunikaceValues != null))
			{
				Havit.BusinessLayerTest.KomunikaceCollection _komunikaceToRemove = new Havit.BusinessLayerTest.KomunikaceCollection(_loadedKomunikaceValues.Except(_KomunikacePropertyHolder.Value).Where(item => !item.IsLoaded || (!item.IsDeleted && (item.Subjekt == this))));
				if (_komunikaceToRemove.Count > 0)
				{
					dirtyCollectionExists = true;
					commandBuilder.AppendFormat("DELETE FROM [dbo].[Komunikace] WHERE ([SubjektID] = @SubjektID) AND [KomunikaceID] IN (SELECT [Value] FROM @Komunikace);");
					SqlParameter dbParameterKomunikace = new SqlParameter("Komunikace", SqlDbType.Structured);
					dbParameterKomunikace.TypeName = "dbo.IntTable";
					dbParameterKomunikace.Value = IntTable.GetSqlParameterValue(_komunikaceToRemove.GetIDs());
					dbCommand.Parameters.Add(dbParameterKomunikace);
					_loadedKomunikaceValues = new Havit.BusinessLayerTest.KomunikaceCollection(_KomunikacePropertyHolder.Value);
				}
			}
			
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				DbParameter dbParameterSubjektID = DbConnector.Default.ProviderFactory.CreateParameter();
				dbParameterSubjektID.DbType = DbType.Int32;
				dbParameterSubjektID.Direction = ParameterDirection.Input;
				dbParameterSubjektID.ParameterName = "SubjektID";
				dbParameterSubjektID.Value = this.ID;
				dbCommand.Parameters.Add(dbParameterSubjektID);
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
			
			if (this.IsNew && (_UzivatelPropertyHolder.Value != null) && (_UzivatelPropertyHolder.Value.IsNew))
			{
				_UzivatelPropertyHolder.Value.Save_MinimalInsert(transaction);
			}
			
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
			
			if (!Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				throw new InvalidOperationException("Použitá BusinessLayerCacheService nepodporuje cache dependencies.");
			}
			
			string key = "Subjekt.SaveCacheDependencyKey|ID=" + this.ID.ToString();
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(Subjekt), key);
			}
			
			return key;
		}
		
		/// <summary>
		/// Odstraní z cache závislosti na klíči CacheDependencyKey.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected void InvalidateSaveCacheDependencyKey()
		{
			if (Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(Subjekt), GetSaveCacheDependencyKey(false));
			}
		}
		
		/// <summary>
		/// Vrátí klíč pro tvorbu závislostí cache. Po uložení jakéhokoliv objektu této třídy jsou závislosti invalidovány.
		/// </summary>
		public static string GetAnySaveCacheDependencyKey(bool ensureInCache = true)
		{
			if (!Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				throw new InvalidOperationException("Použitá BusinessLayerCacheService nepodporuje cache dependencies.");
			}
			
			string key = "Subjekt.AnySaveCacheDependencyKey";
			
			if (ensureInCache)
			{
				Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.EnsureCacheDependencyKey(typeof(Subjekt), key);
			}
			
			return key;
		}
		
		/// <summary>
		/// Odstraní z cache závislosti na klíči AnySaveCacheDependencyKey.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected static void InvalidateAnySaveCacheDependencyKey()
		{
			if (Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.SupportsCacheDependencies)
			{
				Havit.Business.BusinessLayerContexts.BusinessLayerCacheService.InvalidateCacheDependencies(typeof(Subjekt), GetAnySaveCacheDependencyKey(false));
			}
		}
		#endregion
		
		#region GetFirst, GetList, GetAll
		/// <summary>
		/// Vrátí první nalezený objekt typu Subjekt dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Subjekt GetFirst(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return Subjekt.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu Subjekt dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static Subjekt GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			SubjektCollection getListResult = Subjekt.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Subjekt dle parametrů v queryParams.
		/// </summary>
		internal static SubjektCollection GetList(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return Subjekt.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu Subjekt dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		internal static SubjektCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = Subjekt.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Subjekt.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
			return Subjekt.GetList(dbCommand, queryParams.GetDataLoadPower());
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
					Subjekt subjekt = Subjekt.GetObject(dataRecord);
					result.Add(subjekt);
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
		
		#region ToString
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		public override string ToString()
		{
			if (IsNew)
			{
				return "Subjekt(New)";
			}
			
			return String.Format("Subjekt(ID={0})", this.ID);
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
