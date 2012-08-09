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
	/// Subjekt.
	/// </summary>
	[Serializable]
	public partial class Subjekt : SubjektBase
	{
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected Subjekt()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">SubjektID (PK)</param>
		protected Subjekt(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected Subjekt(DataRecord record)
			: base(record)
		{
		}
		#endregion
		
		#region CreateObject (static)
		/// <summary>
		/// Vrátí nový objekt.
		/// </summary>
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
		/// <param name="id">SubjektID (PK)</param>
		/// <returns></returns>
		public static Subjekt GetObject(int id)
		{
			Subjekt result;
			
			if ((IdentityMapScope.Current != null) && (IdentityMapScope.Current.TryGet<Subjekt>(id, out result)))
			{
				return result;
			}
			
			result = new Subjekt(id);
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(result);
			}
			
			return result;
		}
		
		/// <summary>
		/// Vrátí existující objekt inicializovaný daty z DataReaderu.
		/// </summary>
		internal static Subjekt GetObject(DataRecord dataRecord)
		{
			return new Subjekt(dataRecord);
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
