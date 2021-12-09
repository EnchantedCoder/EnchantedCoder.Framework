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
	/// Jazyk (lokalizace).
	/// </summary>
	public partial class Language : LanguageBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Language() : this(ConnectionMode.Connected)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Language(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">LanguageID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Language(int id, ConnectionMode connectionMode = ConnectionMode.Connected) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">LanguageID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Language(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region GetObject (static)
		
		private static object lockGetObjectCacheAccess = new object();
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">LanguageID (PK).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Language GetObject(int id)
		{
			Language result;
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			global::Havit.Diagnostics.Contracts.Contract.Assert(currentIdentityMap != null, "currentIdentityMap != null");
			if (currentIdentityMap.TryGet<Language>(id, out result))
			{
				return result;
			}
			
			bool fromCache = true;
			
			result = (Language)GetBusinessObjectFromCache(id);
			if (result == null)
			{
				lock (lockGetObjectCacheAccess)
				{
					result = (Language)GetBusinessObjectFromCache(id);
					if (result == null)
					{
						fromCache = false;
						result = new Language(id);
						
						AddBusinessObjectToCache(result);
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
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		internal static Language GetObject(DataRecord dataRecord)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(dataRecord != null);
			
			Language result = null;
			
			int id = dataRecord.Get<int>(Language.Properties.ID.FieldName);
			
			if ((dataRecord.DataLoadPower == DataLoadPower.Ghost) || (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
			{
				result = Language.GetObject(id);
				if (!result.IsLoaded && (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
				{
					result.Load(dataRecord);
				}
			}
			else
			{
				result = new Language(id, dataRecord);
			}
			
			return result;
		}
		
		#endregion
		
		#region GetObjectOrDefault (static)
		
		/// <summary>
		/// Pokud je zadáno ID objektu (not-null), vrátí existující objekt s daným ID. Jinak vrací výchozí hodnotu (není-li zadána, pak vrací null).
		/// </summary>
		/// <param name="id">ID objektu.</param>
		/// <param name="defaultValue">Výchozí hodnota.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Language GetObjectOrDefault(int? id, Language defaultValue = null)
		{
			return (id != null) ? GetObject(id.Value) : defaultValue;
		}
		
		#endregion
		
		#region GetObjects (static)
		
		/// <summary>
		/// Vrátí kolekci obsahující objekty danými ID.
		/// </summary>
		/// <param name="ids">Identifikátory objektů.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static LanguageCollection GetObjects(params int[] ids)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(ids != null, "ids != null");
			
			return new LanguageCollection(Array.ConvertAll<int, Language>(ids, id => Language.GetObject(id)));
		}
		
		#endregion
		
		#region CreateDisconnectedObject (static)
		/// <summary>
		/// Vrátí nový disconnected objekt. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Language CreateDisconnectedObject()
		{
			return new Language(ConnectionMode.Disconnected);
		}
		
		/// <summary>
		/// Vrátí nový disconnected objekt s daným Id. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Language CreateDisconnectedObject(int id)
		{
			return new Language(id, ConnectionMode.Disconnected);
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
