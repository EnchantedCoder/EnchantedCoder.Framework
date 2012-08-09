﻿using System;
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
	public partial class ObjednavkaSepsani : ObjednavkaSepsaniBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected ObjednavkaSepsani()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">ObjednavkaSepsaniID (PK)</param>
		protected ObjednavkaSepsani(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">ObjednavkaSepsaniID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected ObjednavkaSepsani(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region CreateObject (static)
		/// <summary>
		/// Vrátí nový objekt.
		/// </summary>
		public static ObjednavkaSepsani CreateObject()
		{
			ObjednavkaSepsani result = new ObjednavkaSepsani();
			return result;
		}
		#endregion
		
		#region GetObject (static)
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">ObjednavkaSepsaniID (PK)</param>
		/// <returns></returns>
		public static ObjednavkaSepsani GetObject(int id)
		{
			ObjednavkaSepsani result;
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			if ((currentIdentityMap != null) && (currentIdentityMap.TryGet<ObjednavkaSepsani>(id, out result)))
			{
				return result;
			}
			
			result = new ObjednavkaSepsani(id);
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		internal static ObjednavkaSepsani GetObject(DataRecord dataRecord)
		{
			ObjednavkaSepsani result = null;
			
			int id = dataRecord.Get<int>(ObjednavkaSepsani.Properties.ID.FieldName);
			
			IdentityMap currentIdentityMap = IdentityMapScope.Current;
			if ((currentIdentityMap != null)
				&& ((dataRecord.DataLoadPower == DataLoadPower.Ghost)
					|| (dataRecord.DataLoadPower == DataLoadPower.FullLoad)))
			{
				if (currentIdentityMap.TryGet<ObjednavkaSepsani>(id, out result))
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
						result = ObjednavkaSepsani.GetObject(id);
					}
					else
					{
						result = new ObjednavkaSepsani(id, dataRecord);
					}
				}
			}
			else
			{
				result = new ObjednavkaSepsani(id, dataRecord);
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
