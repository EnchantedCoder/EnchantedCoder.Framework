﻿//------------------------------------------------------------------------------
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
using System.Xml;
using Havit.Collections;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;
using Havit.Business;
using Havit.Business.Query;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Uživatelská role. Určuje oprávnění v systému. [cached, read-only]
	/// </summary>
	public abstract class RoleBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static RoleBase()
		{
			objectInfo = new ObjectInfo();
			properties = new RoleProperties();
			objectInfo.Initialize("dbo", "Role", true, null, Role.GetObject, Role.GetAll, null, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">RoleID (PK)</param>
		protected RoleBase(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">RoleID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected RoleBase(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Symbol role (název pro ASP.NET autrhorization) [varchar(50), nullable]
		/// </summary>
		public virtual string Symbol
		{
			get
			{
				EnsureLoaded();
				return _SymbolPropertyHolder.Value;
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Symbol.
		/// </summary>
		protected PropertyHolder<string> _SymbolPropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_SymbolPropertyHolder = new PropertyHolder<string>(this);
			
			if (IsNew)
			{
				_SymbolPropertyHolder.Value = String.Empty;
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
			
			if (_SymbolPropertyHolder.IsDirty && (_SymbolPropertyHolder.Value != null) && (_SymbolPropertyHolder.Value.Length > 50))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"Symbol\" přesáhl maximální délku 50 znaků.");
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
			dbCommand.CommandText = "SELECT RoleID, Symbol FROM [dbo].[Role] WHERE RoleID = @RoleID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterRoleID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterRoleID.DbType = DbType.Int32;
			dbParameterRoleID.Direction = ParameterDirection.Input;
			dbParameterRoleID.ParameterName = "RoleID";
			dbParameterRoleID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterRoleID);
			
			result = DbConnector.Default.ExecuteDataRecord(dbCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		protected override sealed void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("RoleID");
			
			string _tempSymbol;
			if (record.TryGet<string>("Symbol", out _tempSymbol))
			{
				_SymbolPropertyHolder.Value = _tempSymbol ?? "";
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
			
			// Neukládáme, jsme read-only třídou.
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		protected override sealed void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			// Neukládáme, jsme read-only třídou.
		}
		
		/// <summary>
		/// Implementace metody vloží jen not-null vlastnosti objektu do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		public override sealed void Save_MinimalInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Implementace metody vloží nový objekt do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		protected override sealed void Save_FullInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Implementace metody aktualizuje data objektu v databázi.
		/// </summary>
		protected override sealed void Save_Update(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		protected override sealed void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Objekt je typu readonly. Metoda vyhazuje výjimku InvalidOperationException.
		/// </summary>
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		#endregion
		
		#region Enum members
		public static Role ZaporneID
		{
			get
			{
				return Role.GetObject(EnumIDs.ZaporneID);
			}
		}
		
		public static Role NuloveID
		{
			get
			{
				return Role.GetObject(EnumIDs.NuloveID);
			}
		}
		
		public static Role Administrator
		{
			get
			{
				return Role.GetObject(EnumIDs.Administrator);
			}
		}
		
		public static Role Editor
		{
			get
			{
				return Role.GetObject(EnumIDs.Editor);
			}
		}
		
		public static Role Publisher
		{
			get
			{
				return Role.GetObject(EnumIDs.Publisher);
			}
		}
		
		public static Role Operator
		{
			get
			{
				return Role.GetObject(EnumIDs.Operator);
			}
		}
		
		#endregion
		
		#region EnumIDs (class)
		/// <summary>
		/// Konstanty ID objektů EnumClass.
		/// </summary>
		public static class EnumIDs
		{
			public const int ZaporneID = -1;
			public const int NuloveID = 0;
			public const int Administrator = 1;
			public const int Editor = 2;
			public const int Publisher = 3;
			public const int Operator = 4;
		}
		#endregion
		
		#region GetFirst, GetList
		/// <summary>
		/// Vrátí první nalezený objekt typu Role dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Role GetFirst(QueryParams queryParams)
		{
			return Role.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu Role dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static Role GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			RoleCollection getListResult = Role.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Role dle parametrů v queryParams.
		/// </summary>
		public static RoleCollection GetList(QueryParams queryParams)
		{
			return Role.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu Role dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		public static RoleCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = Role.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Role.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand);
			return Role.GetList(dbCommand, queryParams.GetDataLoadPower());
		}
		
		private static RoleCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			RoleCollection result = new RoleCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					result.Add(Role.GetObject(dataRecord));
				}
			}
			
			return result;
		}
		
		private static object lockGetAllCacheAccess = new object();
		
		/// <summary>
		/// Vrátí všechny objekty typu Role.
		/// </summary>
		public static RoleCollection GetAll()
		{
			RoleCollection collection = null;
			int[] ids = null;
			string cacheKey = "Havit.BusinessLayerTest.Role.GetAll()";
			
			ids = (int[])HttpRuntime.Cache.Get(cacheKey);
			if (ids == null)
			{
				lock (lockGetAllCacheAccess)
				{
					ids = (int[])HttpRuntime.Cache.Get(cacheKey);
					if (ids == null)
					{
						QueryParams queryParams = new QueryParams();
						collection = Role.GetList(queryParams);
						ids = collection.GetIDs();
						
						HttpRuntime.Cache.Add(
							cacheKey,
							ids,
							null, // dependencies
							Cache.NoAbsoluteExpiration,
							Cache.NoSlidingExpiration,
							CacheItemPriority.NotRemovable,
							null); // callback
					}
				}
			}
			if (collection == null)
			{
				collection = new RoleCollection();
				collection.AddRange(Array.ConvertAll<int, Role>(ids, delegate(int value)
					{
						return Role.GetObject(value);
						}));
				collection.LoadAll();
			}
			
			return collection;
		}
		
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu Role.
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
		/// Objektová reprezentace metadat vlastností typu Role.
		/// </summary>
		public static RoleProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static RoleProperties properties;
		#endregion
		
	}
}
