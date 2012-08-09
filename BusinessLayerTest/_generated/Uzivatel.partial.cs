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
using Havit.Data;
using Havit.Data.SqlClient;
using Havit.Data.SqlTypes;
using Havit.Business;
using Havit.Business.Query;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Uživatel.
	/// </summary>
	[Serializable]
	public partial class Uzivatel : UzivatelBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected Uzivatel()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">UzivatelID (PK)</param>
		protected Uzivatel(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected Uzivatel(DataRecord record)
			: base(record)
		{
		}
		#endregion
		
		#region CreateObject (static)
		/// <summary>
		/// Vrátí nový objekt.
		/// </summary>
		public static Uzivatel CreateObject()
		{
			Uzivatel result = new Uzivatel();
			return result;
		}
		#endregion
		
		#region GetObject (static)
		
		/// <summary>
		/// Vrátí existující objekt s daným ID.
		/// </summary>
		/// <param name="id">UzivatelID (PK)</param>
		/// <returns></returns>
		public static Uzivatel GetObject(int id)
		{
			Uzivatel result;
			
			if ((IdentityMapScope.Current != null) && (IdentityMapScope.Current.TryGet<Uzivatel>(id, out result)))
			{
				return result;
			}
			
			result = new Uzivatel(id);
			
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(result);
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		internal static Uzivatel GetObject(DataRecord dataRecord)
		{
			Uzivatel result = null;
			
			if ((IdentityMapScope.Current != null)
				&& ((dataRecord.DataLoadPower == DataLoadPower.Ghost)
					|| (dataRecord.DataLoadPower == DataLoadPower.FullLoad)))
			{
				int id = dataRecord.Get<int>(Uzivatel.Properties.ID.FieldName);
				
				if (IdentityMapScope.Current.TryGet<Uzivatel>(id, out result))
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
						result = Uzivatel.GetObject(id);
					}
					else
					{
						result = new Uzivatel(dataRecord);
						IdentityMapScope.Current.Store(result);
					}
				}
			}
			else
			{
				result = new Uzivatel(dataRecord);
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
