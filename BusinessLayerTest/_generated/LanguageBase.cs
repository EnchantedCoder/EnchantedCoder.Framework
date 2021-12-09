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
	/// Jazyk (lokalizace). [cached, read-only]
	/// </summary>
	/// <remarks>
	/// <code>
	/// CREATE TABLE [dbo].[Language](
	/// 	[LanguageID] [int] NOT NULL,
	/// 	[UICulture] [varchar](6) COLLATE Czech_CI_AS NOT NULL,
	/// 	[Culture] [varchar](6) COLLATE Czech_CI_AS NOT NULL,
	/// 	[Name] [nvarchar](50) COLLATE Czech_CI_AS NULL,
	/// 	[Aktivni] [bit] NOT NULL,
	/// 	[EditacePovolena] [bit] NOT NULL,
	/// 	[Poradi] [int] NOT NULL,
	///  CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
	/// (
	/// 	[LanguageID] ASC
	/// )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	/// ) ON [PRIMARY]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_UICulture]  DEFAULT (N'') FOR [UICulture]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_Culture]  DEFAULT (N'') FOR [Culture]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_Name]  DEFAULT (N'') FOR [Name]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_Aktivni]  DEFAULT ((1)) FOR [Aktivni]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_EditacePovolena]  DEFAULT ((1)) FOR [EditacePovolena]
	/// ALTER TABLE [dbo].[Language] ADD  CONSTRAINT [DF_Language_Poradi]  DEFAULT ((0)) FOR [Poradi]
	/// </code>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
	public abstract class LanguageBase : ActiveRecordBusinessObjectBase, ILanguage
	{
		#region Static constructor
		static LanguageBase()
		{
			objectInfo = new ObjectInfo();
			properties = new LanguageProperties();
			objectInfo.Initialize("dbo", "Language", "Language", "Havit.BusinessLayerTest", true, null, Language.GetObject, Language.GetAll, null, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		/// <param name="connectionMode">Režim business objektu.</param>
		[System.CodeDom.Compiler.GeneratedCode("Havit.BusinessLayerGenerator", "1.0")]
		protected LanguageBase(ConnectionMode connectionMode) : base(connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">LanguageID (PK).</param>
		/// <param name="connectionMode">Režim business objektu.</param>
		protected LanguageBase(int id, ConnectionMode connectionMode) : base(id, connectionMode)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">LanguageID (PK).</param>
		/// <param name="record">DataRecord s daty objektu (i částečnými).</param>
		protected LanguageBase(int id, DataRecord record) : base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// CultrueName pro UICulture v podobě jako resources. Tedypro výchozí jazyk prázdné, &quot;en&quot; pro všechny angličtiny, &quot;en-US&quot; pro americkou angličtinu. [varchar(6), not-null, default N'']
		/// </summary>
		public virtual string UICulture
		{
			get
			{
				EnsureLoaded();
				return _UICulturePropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				string newValue = value ?? String.Empty;
				if (!Object.Equals(_UICulturePropertyHolder.Value, newValue))
				{
					string oldValue = _UICulturePropertyHolder.Value;
					_UICulturePropertyHolder.Value = newValue;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(UICulture), oldValue, newValue));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost UICulture.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<string> _UICulturePropertyHolder;
		
		/// <summary>
		/// CultureName v plné podobě, např. cs-CZ. Formát &quot;&lt;languagecode2&gt;-&lt;country/regioncode2&gt;&quot;. [varchar(6), not-null, default N'']
		/// </summary>
		public virtual string Culture
		{
			get
			{
				EnsureLoaded();
				return _CulturePropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				string newValue = value ?? String.Empty;
				if (!Object.Equals(_CulturePropertyHolder.Value, newValue))
				{
					string oldValue = _CulturePropertyHolder.Value;
					_CulturePropertyHolder.Value = newValue;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Culture), oldValue, newValue));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Culture.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<string> _CulturePropertyHolder;
		
		/// <summary>
		/// Název pro UI [nvarchar(50), nullable, default N'']
		/// </summary>
		public virtual string Name
		{
			get
			{
				EnsureLoaded();
				return _NamePropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				string newValue = value ?? String.Empty;
				if (!Object.Equals(_NamePropertyHolder.Value, newValue))
				{
					string oldValue = _NamePropertyHolder.Value;
					_NamePropertyHolder.Value = newValue;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name), oldValue, newValue));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Name.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<string> _NamePropertyHolder;
		
		/// <summary>
		/// Indikuje, zda je jazyk pro daného uživatele aktivní, či nikoliv. [bit, not-null, default 1]
		/// </summary>
		public virtual bool Aktivni
		{
			get
			{
				EnsureLoaded();
				return _AktivniPropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				if (!Object.Equals(_AktivniPropertyHolder.Value, value))
				{
					bool oldValue = _AktivniPropertyHolder.Value;
					_AktivniPropertyHolder.Value = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Aktivni), oldValue, value));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Aktivni.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<bool> _AktivniPropertyHolder;
		
		/// <summary>
		/// Indikuje, zda-li je povolena editace lokalizovaných hodnot pro jazyk. [bit, not-null, default 1]
		/// </summary>
		public virtual bool EditacePovolena
		{
			get
			{
				EnsureLoaded();
				return _EditacePovolenaPropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				if (!Object.Equals(_EditacePovolenaPropertyHolder.Value, value))
				{
					bool oldValue = _EditacePovolenaPropertyHolder.Value;
					_EditacePovolenaPropertyHolder.Value = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(EditacePovolena), oldValue, value));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost EditacePovolena.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<bool> _EditacePovolenaPropertyHolder;
		
		/// <summary>
		/// Pořadí při výpisu jazyků. [int, not-null, default 0]
		/// </summary>
		public virtual int Poradi
		{
			get
			{
				EnsureLoaded();
				return _PoradiPropertyHolder.Value;
			}
			private set
			{
				EnsureLoaded();
				
				if (!Object.Equals(_PoradiPropertyHolder.Value, value))
				{
					int oldValue = _PoradiPropertyHolder.Value;
					_PoradiPropertyHolder.Value = value;
					OnPropertyChanged(new PropertyChangedEventArgs(nameof(Poradi), oldValue, value));
				}
			}
		}
		/// <summary>
		/// PropertyHolder pro vlastnost Poradi.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected PropertyHolder<int> _PoradiPropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_UICulturePropertyHolder = new PropertyHolder<string>(this);
			_CulturePropertyHolder = new PropertyHolder<string>(this);
			_NamePropertyHolder = new PropertyHolder<string>(this);
			_AktivniPropertyHolder = new PropertyHolder<bool>(this);
			_EditacePovolenaPropertyHolder = new PropertyHolder<bool>(this);
			_PoradiPropertyHolder = new PropertyHolder<int>(this);
			
			if (IsNew || IsDisconnected)
			{
				_UICulturePropertyHolder.Value = String.Empty;
				_CulturePropertyHolder.Value = String.Empty;
				_NamePropertyHolder.Value = String.Empty;
				_AktivniPropertyHolder.Value = true;
				_EditacePovolenaPropertyHolder.Value = true;
				_PoradiPropertyHolder.Value = 0;
			}
			
			base.Init();
		}
		#endregion
		
		#region CleanDirty
		/// <summary>
		/// Nastaví property holderům IsDirty na false.
		/// </summary>
		protected override void CleanDirty()
		{
			base.CleanDirty();
			
			_UICulturePropertyHolder.IsDirty = false;
			_CulturePropertyHolder.IsDirty = false;
			_NamePropertyHolder.IsDirty = false;
			_AktivniPropertyHolder.IsDirty = false;
			_EditacePovolenaPropertyHolder.IsDirty = false;
			_PoradiPropertyHolder.IsDirty = false;
		}
		#endregion
		
		#region CheckConstraints
		/// <summary>
		/// Kontroluje konzistenci objektu jako celku.
		/// </summary>
		/// <remarks>
		/// Automaticky je voláno před ukládáním objektu Save(), pokud je objekt opravdu ukládán.
		/// </remarks>
		protected override void CheckConstraints()
		{
			base.CheckConstraints();
			
			if (_UICulturePropertyHolder.IsDirty && (_UICulturePropertyHolder.Value != null) && (_UICulturePropertyHolder.Value.Length > 6))
			{
				throw new ConstraintViolationException(this, "Vlastnost \"UICulture\" - řetězec přesáhl maximální délku 6 znaků.");
			}
			
			if (_CulturePropertyHolder.IsDirty && (_CulturePropertyHolder.Value != null) && (_CulturePropertyHolder.Value.Length > 6))
			{
				throw new ConstraintViolationException(this, "Vlastnost \"Culture\" - řetězec přesáhl maximální délku 6 znaků.");
			}
			
			if (_NamePropertyHolder.IsDirty && (_NamePropertyHolder.Value != null) && (_NamePropertyHolder.Value.Length > 50))
			{
				throw new ConstraintViolationException(this, "Vlastnost \"Name\" - řetězec přesáhl maximální délku 50 znaků.");
			}
			
		}
		#endregion
		
		#region Load: Load_GetDataRecord, Load_ParseDataRecord
		/// <summary>
		/// Načte data objektu z DB a vrátí je ve formě DataRecordu.
		/// </summary>
		/// <param name="transaction">Transakce.</param>
		/// <returns>Úplná data objektu.</returns>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed DataRecord Load_GetDataRecord(DbTransaction transaction)
		{
			DataRecord result;
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.CommandText = "SELECT [LanguageID], [UICulture], [Culture], [Name], [Aktivni], [EditacePovolena], [Poradi] FROM [dbo].[Language] WHERE [LanguageID] = @LanguageID";
			dbCommand.Transaction = transaction;
			
			DbParameter dbParameterLanguageID = DbConnector.Default.ProviderFactory.CreateParameter();
			dbParameterLanguageID.DbType = DbType.Int32;
			dbParameterLanguageID.Direction = ParameterDirection.Input;
			dbParameterLanguageID.ParameterName = "LanguageID";
			dbParameterLanguageID.Value = this.ID;
			dbCommand.Parameters.Add(dbParameterLanguageID);
			
			result = DbConnector.Default.ExecuteDataRecord(dbCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("LanguageID");
			
			string _tempUICulture;
			if (record.TryGet<string>("UICulture", out _tempUICulture))
			{
				_UICulturePropertyHolder.Value = _tempUICulture ?? String.Empty;
			}
			
			string _tempCulture;
			if (record.TryGet<string>("Culture", out _tempCulture))
			{
				_CulturePropertyHolder.Value = _tempCulture ?? String.Empty;
			}
			
			string _tempName;
			if (record.TryGet<string>("Name", out _tempName))
			{
				_NamePropertyHolder.Value = _tempName ?? String.Empty;
			}
			
			bool _tempAktivni;
			if (record.TryGet<bool>("Aktivni", out _tempAktivni))
			{
				_AktivniPropertyHolder.Value = _tempAktivni;
			}
			
			bool _tempEditacePovolena;
			if (record.TryGet<bool>("EditacePovolena", out _tempEditacePovolena))
			{
				_EditacePovolenaPropertyHolder.Value = _tempEditacePovolena;
			}
			
			int _tempPoradi;
			if (record.TryGet<int>("Poradi", out _tempPoradi))
			{
				_PoradiPropertyHolder.Value = _tempPoradi;
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete, Delete_Perform
		
		/// <summary>
		/// Ukládá member-objekty.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_SaveMembers(DbTransaction transaction)
		{
			base.Save_SaveMembers(transaction);
			
			// Neukládáme, jsme read-only třídou.
		}
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			// Neukládáme, jsme read-only třídou.
		}
		
		/// <summary>
		/// Implementace metody vloží jen not-null vlastnosti objektu do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		public override sealed void Save_MinimalInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Implementace metody vloží nový objekt do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_FullInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Implementace metody aktualizuje data objektu v databázi.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Update(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		/// <summary>
		/// Objekt je typu readonly. Metoda vyhazuje výjimku InvalidOperationException.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected override sealed void Delete_Perform(DbTransaction transaction)
		{
			throw new InvalidOperationException("Objekty třídy Havit.BusinessLayerTest.Language jsou určeny jen pro čtení.");
		}
		
		#endregion
		
		#region BusinessObject cache access methods
		/// <summary>
		/// Vrátí název klíče pro business object.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected static string GetBusinessObjectCacheKey(int id)
		{
			return "BL|Language|" + id;
		}
		
		/// <summary>
		/// Přidá business object do cache.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		protected static void AddBusinessObjectToCache(BusinessObjectBase businessObject)
		{
			Havit.Business.BusinessLayerContext.BusinessLayerCacheService.AddBusinessObjectToCache(typeof(Language), GetBusinessObjectCacheKey(businessObject.ID), businessObject);
		}
		
		/// <summary>
		/// Vyhledá v cache business object pro objekt daného ID a vrátí jej. Není-li v cache nalezen, vrací null.
		/// </summary>
		[System.ComponentModel.EditorBrowsable(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static BusinessObjectBase GetBusinessObjectFromCache(int id)
		{
			return Havit.Business.BusinessLayerContext.BusinessLayerCacheService.GetBusinessObjectFromCache(typeof(Language), GetBusinessObjectCacheKey(id));
		}
		
		#endregion
		
		#region GetAll IDs cache access methods
		/// <summary>
		/// Vrátí název klíče pro kolekci IDs metody GetAll.
		/// </summary>
		private static string GetAllIDsCacheKey()
		{
			return "BL|Language|GetAll";
		}
		
		/// <summary>
		/// Vyhledá v cache pole IDs metody GetAll a vrátí jej. Není-li v cache nalezena, vrací null.
		/// </summary>
		private static int[] GetAllIDsFromCache()
		{
			return Havit.Business.BusinessLayerContext.BusinessLayerCacheService.GetAllIDsFromCache(typeof(Language), GetAllIDsCacheKey());
		}
		
		/// <summary>
		/// Přidá pole IDs metody GetAll do cache.
		/// </summary>
		private static void AddAllIDsToCache(int[] ids)
		{
			Havit.Business.BusinessLayerContext.BusinessLayerCacheService.AddAllIDsToCache(typeof(Language), GetAllIDsCacheKey(), ids);
		}
		
		#endregion
		
		#region GetFirst, GetList, GetAll
		/// <summary>
		/// Vrátí první nalezený objekt typu Language dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Language GetFirst(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return Language.GetFirst(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí první nalezený objekt typu Language dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null. Data jsou načítána v předané transakci.
		/// </summary>
		public static Language GetFirst(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			int? originalTopRecords = queryParams.TopRecords;
			queryParams.TopRecords = 1;
			LanguageCollection getListResult = Language.GetList(queryParams, transaction);
			queryParams.TopRecords = originalTopRecords;
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Language dle parametrů v queryParams.
		/// </summary>
		internal static LanguageCollection GetList(QueryParams queryParams)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			return Language.GetList(queryParams, null);
		}
		
		/// <summary>
		/// Vrátí objekty typu Language dle parametrů v queryParams. Data jsou načítána v předané transakci.
		/// </summary>
		internal static LanguageCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(queryParams != null, "queryParams != null");
			
			DbCommand dbCommand = DbConnector.Default.ProviderFactory.CreateCommand();
			dbCommand.Transaction = transaction;
			
			queryParams.ObjectInfo = Language.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Language.Properties.ID);
			}
			
			queryParams.PrepareCommand(dbCommand, SqlServerPlatform.SqlServer2008, CommandBuilderOptions.None);
			return Language.GetList(dbCommand, queryParams.GetDataLoadPower());
		}
		
		private static LanguageCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			LanguageCollection result = new LanguageCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					Language language = Language.GetObject(dataRecord);
					result.Add(language);
				}
			}
			
			return result;
		}
		
		private static object lockGetAllCacheAccess = new object();
		
		/// <summary>
		/// Vrátí všechny objekty typu Language.
		/// </summary>
		public static LanguageCollection GetAll()
		{
			LanguageCollection collection = null;
			int[] ids = null;
			
			ids = GetAllIDsFromCache();
			if (ids == null)
			{
				lock (lockGetAllCacheAccess)
				{
					ids = GetAllIDsFromCache();
					if (ids == null)
					{
						QueryParams queryParams = new QueryParams();
						queryParams.OrderBy.Add(Language.Properties.Poradi, SortDirection.Ascending);
						collection = Language.GetList(queryParams);
						ids = collection.GetIDs();
						
						AddAllIDsToCache(ids);
					}
				}
			}
			if (collection == null)
			{
				collection = new LanguageCollection();
				collection.AddRange(Language.GetObjects(ids));
				collection.LoadAll();
			}
			
			return collection;
		}
		
		#endregion
		
		#region Localizations
		/// <summary>
		/// Dohledá objekt Language podle zadaného CultureName.
		/// </summary>
		/// <param name="cultureName">CultureName v podobě cs-CZ, en-US, atp.</param>
		/// <remarks>Dohledává dle logiky resources, tedy od nejspecifičtějšího (en-US) k obecnějšímu (en) až po invariant.</remarks>
		/// <returns>Nalezený objekt v případě úspěchu; jinak <c>null</c>.</returns>
		public static Language GetByUICulture(string cultureName)
		{
			global::Havit.Diagnostics.Contracts.Contract.Requires(cultureName != null, "cultureName != null");
			
			if (culturesDictionary == null)
			{
				lock (culturesDictionaryLock)
				{
					if (culturesDictionary == null)
					{
						LanguageCollection languages = Language.GetAll();
						culturesDictionary = new Dictionary<string, int>();
						foreach (Language language in languages)
						{
							culturesDictionary.Add(language.UICulture, language.ID);
							
							// pokud není nastaveno UiCulture, jedná se o výchozí jazyk (invariant)
							if (String.IsNullOrEmpty(language.UICulture))
							{
								_defaultUILanguageID = language.ID;
							}
						}
					}
				}
			}
			// nejprve zkusíme hledat podle plného názvu
			int? resultID = null;
			int tmp;
			if (culturesDictionary.TryGetValue(cultureName, out tmp))
			{
				resultID = tmp;
			}
			
			// pokud není nalezeno, hledáme podle samotného jazyka
			if ((resultID == null) && (cultureName.Length > 2))
			{
				if (culturesDictionary.TryGetValue(cultureName.Substring(0, 2), out tmp))
				{
					resultID = tmp;
				}
			}
			
			// pokud není nalezeno, použijeme výchozí jazyk (je-li stanoven).
			if (resultID == null)
			{
				resultID = _defaultUILanguageID;
			}
			return (resultID == null) ? null : Language.GetObject(resultID.Value);
			
		}
		private static Dictionary<string, int> culturesDictionary;
		private static object culturesDictionaryLock = new object();
		private static int? _defaultUILanguageID;
		
		/// <summary>
		/// Dohledá objekt Language podle zadaného Culture.
		/// </summary>
		/// <param name="culture">CultureName v podobě cs-CZ, en-US, atp.</param>
		/// <remarks>Dohledává dle logiky resources, tedy od nejspecifičtějšího (en-US) k obecnějšímu (en) až po invariant.</remarks>
		/// <returns>Nalezený objekt v případě úspěchu; jinak <c>null</c>.</returns>
		public static Language GetByUICulture(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			
			return Language.GetByUICulture(culture.Name);
		}
		
		/// <summary>
		/// Aktuální jazyk určený z System.Threading.Thread.CurrentThread.CurrentUICulture.
		/// </summary>
		public static Language Current
		{
			get
			{
				// pokud se ptáme na stále stejnou cultureName, v rámci stejné identity mapy...
				// raději si zapamatuju IdentityMap, než si pamatovat ID objektu a použít Language.GetObject,
				// protože to stejně použije identitymapu a navíc bude hledat ve slovníku
				if ((Thread.CurrentThread.CurrentUICulture.Name == _currentCultureName)
					&& (IdentityMapScope.Current == _currentIdentityMap))
				{
					return _currentLanguage;
				}
				
				_currentCultureName = Thread.CurrentThread.CurrentUICulture.Name;
				_currentIdentityMap = IdentityMapScope.Current;
				_currentLanguage = Language.GetByUICulture(Thread.CurrentThread.CurrentUICulture.Name);
				return _currentLanguage;
			}
		}
		[ThreadStatic]
		private static string _currentCultureName;
		[ThreadStatic]
		private static IdentityMap _currentIdentityMap;
		[ThreadStatic]
		private static Language _currentLanguage;
		
		#endregion
		
		#region ToString
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		public override string ToString()
		{
			return String.Format("Language(ID={0})", this.ID);
		}
		#endregion
		
		#region ObjectInfo
		/// <summary>
		/// Objektová reprezentace metadat typu Language.
		/// </summary>
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
		/// <summary>
		/// Objektová reprezentace metadat vlastností typu Language.
		/// </summary>
		public static LanguageProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static LanguageProperties properties;
		#endregion
		
	}
}
