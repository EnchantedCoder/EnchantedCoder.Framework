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
using Havit.Data.SqlClient;
using Havit.Data.SqlServer;
using Havit.Data.SqlTypes;

namespace Havit.BusinessLayerTest
{
	/// <summary>
	/// Kolekce business objektů typu Havit.BusinessLayerTest.Subjekt.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public partial class SubjektCollectionBase : BusinessObjectCollection<Subjekt, SubjektCollection>
	{
		#region Constructors
		/// <summary>
		/// Vytvoří novou instanci kolekce.
		/// </summary>
		public SubjektCollectionBase() : base()
		{
		}
		
		/// <summary>
		/// Vytvoří novou instanci kolekce a zkopíruje do ní prvky z předané kolekce.
		/// </summary>
		public SubjektCollectionBase(IEnumerable<Subjekt> collection) : base(collection)
		{
		}
		#endregion
		
		#region Find & FindAll
		/// <summary>
		/// Prohledá kolekci a vrátí první nalezený prvek odpovídající kritériu match.
		/// </summary>
		public override Subjekt Find(Predicate<Subjekt> match)
		{
			LoadAll();
			return base.Find(match);
		}
		
		/// <summary>
		/// Prohledá kolekci a vrátí všechny prvky odpovídající kritériu match.
		/// </summary>
		public override SubjektCollection FindAll(Predicate<Subjekt> match)
		{
			LoadAll();
			return base.FindAll(match);
		}
		#endregion
		
		#region Sort
		/// <summary>
		/// Seřadí prvky kolekce dle požadované property, která implementuje IComparable.
		/// </summary>
		/// <remarks>
		/// Používá Havit.Collections.GenericPropertyComparer{T}. K porovnávání podle property
		/// tedy dochází pomocí reflexe - relativně pomalu. Pokud je potřeba vyšší výkon, je potřeba použít
		/// overload Sort(Generic Comparsion) s přímým přístupem k property.
		/// </remarks>
		/// <param name="propertyName">property, podle které se má řadit</param>
		/// <param name="ascending">true, pokud se má řadit vzestupně, false, pokud sestupně</param>
		[Obsolete]
		public override void Sort(string propertyName, bool ascending)
		{
			LoadAll();
			base.Sort(propertyName, ascending);
		}
		
		/// <summary>
		/// Seřadí prvky kolekce dle požadované property, která implementuje IComparable.
		/// Před řazením načtě všechny prvky metodou LoadAll.
		/// </summary>
		/// <remarks>
		/// Používá Havit.Collections.GenericPropertyComparer{T}. K porovnávání podle property
		/// tedy dochází pomocí reflexe - relativně pomalu. Pokud je potřeba vyšší výkon, je potřeba použít
		/// overload Sort(Generic Comparsion) s přímým přístupem k property.
		/// </remarks>
		/// <param name="propertyInfo">Property, podle které se má řadit.</param>
		/// <param name="sortDirection">Směr řazení.</param>
		public override void Sort(PropertyInfo propertyInfo, SortDirection sortDirection)
		{
			LoadAll();
			base.Sort(propertyInfo, sortDirection);
		}
		
		/// <summary>
		/// Seřadí prvky kolekce dle zadaného srovnání. Publikuje metodu Sort(Generic Comparsion) inner-Listu.
		/// Před řazením načtě všechny prvky metodou LoadAll.
		/// </summary>
		/// <param name="comparsion">srovnání, podle kterého mají být prvky seřazeny</param>
		public override void Sort(Comparison<Subjekt> comparsion)
		{
			LoadAll();
			base.Sort(comparsion);
		}
		#endregion
		
		#region LoadAll
		/// <summary>
		/// Načte všechny prvky kolekce.
		/// </summary>
		public void LoadAll()
		{
			LoadAll(null);
		}
		
		/// <summary>
		/// Načte všechny prvky kolekce.
		/// </summary>
		public void LoadAll(DbTransaction transaction)
		{
			if ((!LoadAllRequired) || (this.Count == 0))
			{
				return;
			}
			
			Dictionary<int, Subjekt> ghosts = new Dictionary<int, Subjekt>();
			
			for (int i = 0; i < this.Count; i++)
			{
				Subjekt currentObject = this[i];
				if ((currentObject != null) && (!currentObject.IsLoaded))
				{
					if (!ghosts.ContainsKey(currentObject.ID))
					{
						ghosts.Add(currentObject.ID, currentObject);
					}
				}
			}
			
			if (ghosts.Count > 0)
			{
				DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
				dbCommand.Transaction = transaction;
				
				QueryParams queryParams = new QueryParams();
				queryParams.ObjectInfo = Subjekt.ObjectInfo;
				queryParams.Conditions.Add(ReferenceCondition.CreateIn(Subjekt.Properties.ID, ghosts.Keys.ToArray()));
				queryParams.IncludeDeleted = true;
				queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
				
				using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
				{
					while (reader.Read())
					{
						DataRecord dataRecord = new DataRecord(reader, queryParams.GetDataLoadPower());
						int id = dataRecord.Get<int>(Subjekt.Properties.ID.FieldName);
						
						Subjekt ghost = ghosts[id];
						if (!ghost.IsLoaded)
						{
							ghost.Load(dataRecord);
						}
					}
				}
			}
			
			LoadAllRequired = false;
		}
		#endregion
		
	}
}
