//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;
using Havit.Business;
using Havit.Business.Query;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Uživatelská role. Určuje oprávnění v systému.
	/// </summary>
	[Serializable]
	public abstract class RoleBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static RoleBase()
		{
			objectInfo = new ObjectInfo();
			properties = new RoleProperties();
			objectInfo.Initialize("dbo", "Role", true, Role.GetObject, Role.GetAll, null, properties.All);
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
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected RoleBase(DataRecord record)
			: base(record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Symbol role (název pro ASP.NET autrhorization)
		/// </summary>
		public virtual string Symbol
		{
			get
			{
				EnsureLoaded();
				return _SymbolPropertyHolder.Value;
			}
		}
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
				_SymbolPropertyHolder.Value = null;
			}
		}
		#endregion
		
		#region CheckConstraints
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
		protected override DataRecord Load_GetDataRecord(DbTransaction transaction)
		{
			DataRecord result;
			
			SqlCommand sqlCommand = new SqlCommand("SELECT RoleID, Symbol FROM dbo.Role WHERE RoleID = @RoleID");
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterID = new SqlParameter("@RoleID", SqlDbType.Int);
			sqlParameterID.Value = this.ID;
			sqlCommand.Parameters.Add(sqlParameterID);
			
			result = SqlDataAccess.ExecuteDataRecord(sqlCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		protected override void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("RoleID");
			
			string _tempSymbol;
			if (record.TryGet<string>("Symbol", out _tempSymbol))
			{
				_SymbolPropertyHolder.Value = _tempSymbol ?? "";
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete_Perform
		
		// Save_SaveMembers: Není co ukládat
		
		// Save_SaveCollections: Není co ukládat
		
		public override void Save_MinimalInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		protected override void Save_FullInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		protected override void Save_Update(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		protected override void Delete_Perform(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Role jsou určeny jen pro čtení.");
		}
		
		#endregion
		
		#region Enum members
		public static Role ZaporneID
		{
			get
			{
				return Role.GetObject(-1);
			}
		}
		
		public static Role NuloveID
		{
			get
			{
				return Role.GetObject(0);
			}
		}
		
		public static Role Administrator
		{
			get
			{
				return Role.GetObject(1);
			}
		}
		
		public static Role Editor
		{
			get
			{
				return Role.GetObject(2);
			}
		}
		
		public static Role Publisher
		{
			get
			{
				return Role.GetObject(3);
			}
		}
		
		public static Role Operator
		{
			get
			{
				return Role.GetObject(4);
			}
		}
		
		#endregion
		
		#region GetFirst, GetList
		/// <summary>
		/// Vrátí první nalezený objekt typu Role dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Role GetFirst(QueryParams queryParams)
		{
			queryParams.TopRecords = 1;
			RoleCollection getListResult = Role.GetList(queryParams);
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		internal static RoleCollection GetList(SqlCommand sqlCommand)
		{
			return GetList(sqlCommand, false);
		}
		
		private static RoleCollection GetList(SqlCommand sqlCommand, bool ghostsOnly)
		{
			if (sqlCommand == null)
			{
				throw new ArgumentNullException("sqlCommand");
			}
			
			RoleCollection result = new RoleCollection();
			
			using (SqlDataReader reader = SqlDataAccess.ExecuteReader(sqlCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, false);
					if (ghostsOnly)
					{
						result.Add(Role.GetObject(dataRecord.Get<int>(Role.Properties.ID.FieldName)));
					}
					else
					{
						result.Add(Role.GetObject(dataRecord));
					}
				}
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí objekty typu Role dle parametrů v queryParams.
		/// </summary>
		public static RoleCollection GetList(QueryParams queryParams)
		{
			return Role.GetList(queryParams, null);
		}
		
		public static RoleCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			queryParams.ObjectInfo = Role.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Role.Properties.ID);
			}
			
			queryParams.PrepareCommand(sqlCommand);
			return Role.GetList(sqlCommand, queryParams.Properties.Count == 1);
		}
		
		public static object lockGetAllCacheAccess = new object();
		
		public static RoleCollection GetAll()
		{
			RoleCollection result;
			string cacheKey = "Havit.BusinessLayerTest.Role.GetAll";
			
			result = (RoleCollection)HttpRuntime.Cache.Get(cacheKey);
			if (result == null)
			{
				lock (lockGetAllCacheAccess)
				{
					result = (RoleCollection)HttpRuntime.Cache.Get(cacheKey);
					if (result == null)
					{
						QueryParams queryParams = new QueryParams();
						queryParams.IncludeDeleted = true;
						result = Role.GetList(queryParams);
						
						HttpRuntime.Cache.Add(
							cacheKey,
							result,
							null, // dependencies
							Cache.NoAbsoluteExpiration,
							Cache.NoSlidingExpiration,
							CacheItemPriority.NotRemovable,
							null); // callback
					}
				}
			}
			return result;
		}
		
		#endregion
		
		#region ObjectInfo
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
