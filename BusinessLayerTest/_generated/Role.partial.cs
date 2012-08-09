﻿using System;
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
	/// <summary>
	/// Uživatelská role. Určuje oprávnění v systému.
	/// </summary>
	public partial class Role : RoleBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">RoleID (PK)</param>
		protected Role(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">RoleID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected Role(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region GetObject (static)
		
		private static object lockGetObjectCacheAccess = new object();
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">RoleID (PK)</param>
		/// <returns></returns>
		[System.Diagnostics.Contracts.ContractVerification(false)]
		public static Role GetObject(int id)
		{
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<Role>() != null);
			
			Role result;
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			if ((currentIdentityMap != null) && (currentIdentityMap.TryGet<Role>(id, out result)))
			{
				global::System.Diagnostics.Contracts.Contract.Assume(result != null);
				return result;
			}
			
			bool fromCache = true;
			string cacheKey = String.Format(CultureInfo.InvariantCulture, "Havit.BusinessLayerTest.Role.GetObject({0})", id);
			
			result = (Role)HttpRuntime.Cache.Get(cacheKey);
			if (result == null)
			{
				lock (lockGetObjectCacheAccess)
				{
					result = (Role)HttpRuntime.Cache.Get(cacheKey);
					if (result == null)
					{
						fromCache = false;
						result = new Role(id);
						
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
			
			if (fromCache && (currentIdentityMap != null))
			{
				currentIdentityMap.Store(result);
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		[System.Diagnostics.Contracts.ContractVerification(false)]
		internal static Role GetObject(DataRecord dataRecord)
		{
			global::System.Diagnostics.Contracts.Contract.Requires(dataRecord != null);
			global::System.Diagnostics.Contracts.Contract.Ensures(global::System.Diagnostics.Contracts.Contract.Result<Role>() != null);
			
			Role result = null;
			
			int id = dataRecord.Get<int>(Role.Properties.ID.FieldName);
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			if ((currentIdentityMap != null)
				&& ((dataRecord.DataLoadPower == DataLoadPower.Ghost)
					|| (dataRecord.DataLoadPower == DataLoadPower.FullLoad)))
			{
				if (currentIdentityMap.TryGet<Role>(id, out result))
				{
					if (!result.IsLoaded && (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
					{
						result.Load(dataRecord);
					}
				}
				else
				{
					if (dataRecord.DataLoadPower == DataLoadPower.Ghost)
					{
						result = Role.GetObject(id);
					}
					else
					{
						result = new Role(id, dataRecord);
					}
				}
			}
			else
			{
				result = new Role(id, dataRecord);
			}
			
			return result;
		}
		
		#endregion
		
		//------------------------------------------------------------------------------
		// <auto-generated>
		//     This code was generated by a tool.
		//     Changes to this file will be lost if the code is regenerated.
		// </auto-generated>
		//------------------------------------------------------------------------------
		
	}
}
