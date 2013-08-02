//------------------------------------------------------------------------------
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

namespace Havit.BusinessLayerTest.Resources
{
	/// <summary>
	/// Kolekce business objektů typu Havit.BusinessLayerTest.Resources.ResourceItemLocalization.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public partial class ResourceItemLocalizationCollectionBase : BusinessObjectCollection<ResourceItemLocalization, ResourceItemLocalizationCollection>, ILocalizationCollection
	{
		#region Constructors
		/// <summary>
		/// Vytvoří novou instanci kolekce.
		/// </summary>
		public ResourceItemLocalizationCollectionBase() : base()
		{
		}
		
		/// <summary>
		/// Vytvoří novou instanci kolekce a zkopíruje do ní prvky z předané kolekce.
		/// </summary>
		public ResourceItemLocalizationCollectionBase(IEnumerable<ResourceItemLocalization> collection) : base(collection)
		{
		}
		#endregion
		
		#region Find & FindAll
		/// <summary>
		/// Prohledá kolekci a vrátí první nalezený prvek odpovídající kritériu match.
		/// </summary>
		public override ResourceItemLocalization Find(Predicate<ResourceItemLocalization> match)
		{
			LoadAll();
			return base.Find(match);
		}
		
		/// <summary>
		/// Prohledá kolekci a vrátí všechny prvky odpovídající kritériu match.
		/// </summary>
		public override ResourceItemLocalizationCollection FindAll(Predicate<ResourceItemLocalization> match)
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
		public override void Sort(Comparison<ResourceItemLocalization> comparsion)
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
			
			Dictionary<int, ResourceItemLocalization> ghosts = new Dictionary<int, ResourceItemLocalization>();
			
			for (int i = 0; i < this.Count; i++)
			{
				ResourceItemLocalization currentObject = this[i];
				if ((currentObject != null) && (!currentObject.IsLoaded))
				{
					DataRecord cachedDataRecord = ResourceItemLocalization.GetDataRecordFromCache(currentObject.ID);
					if (cachedDataRecord != null)
					{
						currentObject.Load(cachedDataRecord);
						continue;
					}
					
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
				queryParams.ObjectInfo = ResourceItemLocalization.ObjectInfo;
				queryParams.Conditions.Add(ReferenceCondition.CreateIn(ResourceItemLocalization.Properties.ID, ghosts.Keys.ToArray()));
				queryParams.IncludeDeleted = true;
				queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
				
				using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
				{
					while (reader.Read())
					{
						DataRecord dataRecord = new DataRecord(reader, queryParams.GetDataLoadPower());
						int id = dataRecord.Get<int>(ResourceItemLocalization.Properties.ID.FieldName);
						
						ResourceItemLocalization ghost = ghosts[id];
						if (!ghost.IsLoaded)
						{
							ghost.Load(dataRecord);
							ghost.AddDataRecordToCache(dataRecord);
						}
					}
				}
			}
			
			LoadAllRequired = false;
		}
		#endregion
		
		#region Localizations
		/// <summary>
		/// Vrací objekt s lokalizovanými daty na základě jazyka, který je předán.
		/// </summary>
		public ResourceItemLocalization this[Havit.BusinessLayerTest.Language language]
		{
			get
			{
				return this.Find(delegate(ResourceItemLocalization item)
					{
						return (item.Language == language);
					});
			}
		}
		
		/// <summary>
		/// Vrací objekt s lokalizovanými daty na základě aktuálního jazyka (aktuální jazyk se hledá na základě CurrentUICulture).
		/// </summary>
		public virtual ResourceItemLocalization Current
		{
			get
			{
				return this[Havit.BusinessLayerTest.Language.Current];
			}
		}
		
		/// <summary>
		/// Vrací objekt s lokalizovanými daty na základě jazyka, který je předán.
		/// </summary>
		BusinessObjectBase ILocalizationCollection.this[ILanguage language]
		{
			get
			{
				return this[(Havit.BusinessLayerTest.Language)language];
			}
		}
		
		/// <summary>
		/// Vrací objekt s lokalizovanými daty na základě aktuálního jazyka (aktuální jazyk se hledá na základě CurrentUICulture).
		/// </summary>
		BusinessObjectBase ILocalizationCollection.Current
		{
			get
			{
				return this.Current;
			}
		}
		#endregion
		
	}
}
