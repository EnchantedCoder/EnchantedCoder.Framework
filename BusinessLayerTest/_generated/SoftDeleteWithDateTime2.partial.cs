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
	/// Tabulka pro ověření generovaného kódu pro soft-delete dle DateTime2.
	/// </summary>
	public partial class SoftDeleteWithDateTime2 : SoftDeleteWithDateTime2Base
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SoftDeleteWithDateTime2() : this(ConnectionMode.Connected)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SoftDeleteWithDateTime2(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SoftDeleteWithDateTime2ID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SoftDeleteWithDateTime2(int id, ConnectionMode connectionMode = ConnectionMode.Connected) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">SoftDeleteWithDateTime2ID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected SoftDeleteWithDateTime2(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region CreateObject (static)
		/// <summary>
		/// Vrátí nový objekt.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static SoftDeleteWithDateTime2 CreateObject()
		{
			SoftDeleteWithDateTime2 result = new SoftDeleteWithDateTime2();
			return result;
		}
		#endregion
		
		#region GetObject (static)
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">SoftDeleteWithDateTime2ID (PK).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static SoftDeleteWithDateTime2 GetObject(int id)
		{
			SoftDeleteWithDateTime2 result;
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			global::Havit.Diagnostics.Contracts.Contract.Assert(currentIdentityMap != null, "currentIdentityMap != null");
			if (currentIdentityMap.TryGet<SoftDeleteWithDateTime2>(id, out result))
			{
				return result;
			}
			
			result = new SoftDeleteWithDateTime2(id);
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		internal static SoftDeleteWithDateTime2 GetObject(DataRecord dataRecord)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(dataRecord != null);
			
			SoftDeleteWithDateTime2 result = null;
			
			int id = dataRecord.Get<int>(SoftDeleteWithDateTime2.Properties.ID.FieldName);
			
			if ((dataRecord.DataLoadPower == DataLoadPower.Ghost) || (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
			{
				result = SoftDeleteWithDateTime2.GetObject(id);
				if (!result.IsLoaded && (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
				{
					result.Load(dataRecord);
				}
			}
			else
			{
				result = new SoftDeleteWithDateTime2(id, dataRecord);
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
		public static SoftDeleteWithDateTime2 GetObjectOrDefault(int? id, SoftDeleteWithDateTime2 defaultValue = null)
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
		public static SoftDeleteWithDateTime2Collection GetObjects(params int[] ids)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(ids != null, "ids != null");
			
			return new SoftDeleteWithDateTime2Collection(Array.ConvertAll<int, SoftDeleteWithDateTime2>(ids, id => SoftDeleteWithDateTime2.GetObject(id)));
		}
		
		#endregion
		
		#region CreateDisconnectedObject (static)
		/// <summary>
		/// Vrátí nový disconnected objekt. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static SoftDeleteWithDateTime2 CreateDisconnectedObject()
		{
			return new SoftDeleteWithDateTime2(ConnectionMode.Disconnected);
		}
		
		/// <summary>
		/// Vrátí nový disconnected objekt s daným Id. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static SoftDeleteWithDateTime2 CreateDisconnectedObject(int id)
		{
			return new SoftDeleteWithDateTime2(id, ConnectionMode.Disconnected);
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
