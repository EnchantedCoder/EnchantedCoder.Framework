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
	public partial class Subjekt : SubjektBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Subjekt() : this(ConnectionMode.Connected)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Subjekt(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SubjektID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Subjekt(int id, ConnectionMode connectionMode = ConnectionMode.Connected) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">SubjektID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected Subjekt(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region CreateObject (static)
		/// <summary>
		/// Vrátí nový objekt.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Subjekt CreateObject()
		{
			Subjekt result = new Subjekt();
			return result;
		}
		#endregion
		
		#region GetObject (static)
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">SubjektID (PK).</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Subjekt GetObject(int id)
		{
			Subjekt result;
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			global::Havit.Diagnostics.Contracts.Contract.Assert(currentIdentityMap != null, "currentIdentityMap != null");
			if (currentIdentityMap.TryGet<Subjekt>(id, out result))
			{
				return result;
			}
			
			result = new Subjekt(id);
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		internal static Subjekt GetObject(DataRecord dataRecord)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(dataRecord != null);
			
			Subjekt result = null;
			
			int id = dataRecord.Get<int>(Subjekt.Properties.ID.FieldName);
			
			if ((dataRecord.DataLoadPower == DataLoadPower.Ghost) || (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
			{
				result = Subjekt.GetObject(id);
				if (!result.IsLoaded && (dataRecord.DataLoadPower == DataLoadPower.FullLoad))
				{
					result.Load(dataRecord);
				}
			}
			else
			{
				result = new Subjekt(id, dataRecord);
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
		public static Subjekt GetObjectOrDefault(int? id, Subjekt defaultValue = null)
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
		public static SubjektCollection GetObjects(params int[] ids)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(ids != null, "ids != null");
			
			return new SubjektCollection(Array.ConvertAll<int, Subjekt>(ids, id => Subjekt.GetObject(id)));
		}
		
		#endregion
		
		#region CreateDisconnectedObject (static)
		/// <summary>
		/// Vrátí nový disconnected objekt. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Subjekt CreateDisconnectedObject()
		{
			return new Subjekt(ConnectionMode.Disconnected);
		}
		
		/// <summary>
		/// Vrátí nový disconnected objekt s daným Id. Určeno výhradně pro účely testů.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		public static Subjekt CreateDisconnectedObject(int id)
		{
			return new Subjekt(id, ConnectionMode.Disconnected);
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
