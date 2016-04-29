﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Havit.Data;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Havit.Business
{
	/// <summary>
	/// Bázová třída pro všechny business-objekty, která definuje jejich základní chování, zejména ve vztahu k databázi (Layer Supertype).
	/// </summary>
	/// <remarks>
	/// Třída je základem pro všechny business-objekty a implementuje základní pattern pro komunikaci s perzistentními uložišti.
	/// Načítání je implementováno jako lazy-load, kdy je objekt nejprve vytvořen prázdný jako ghost se svým ID a teprve
	/// při první potřebě je iniciováno jeho úplné načtení.<br/>
	/// </remarks>
	[DebuggerDisplay("{GetType().FullName,nq} (ID={IsNew ? \"New\" : ID.ToString(),nq}, IsLoaded={IsLoaded,nq}, IsDirty={IsDirty,nq}, IsDisconnected={IsDisconnected,nq})")]
	public abstract class BusinessObjectBase
	{
		#region Consts
		/// <summary>
		/// Hodnota, kterou má ID objektu neuloženého v databázi (bez perzistence).
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "Hodnotu ID povolujeme (a vyžadujeme).")]
		public const int NoID = Int32.MinValue;
		#endregion

		#region Property - ID
		/// <summary>
		/// Primární klíč objektu.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member", Justification = "Hodnotu ID povolujeme (a vyžadujeme).")]
		public int ID
		{
			get { return _id; }
			protected set { _id = value; }
		}
		private int _id;
		#endregion

		#region Properties - Stav objektu (IsDirty, IsLoaded, IsNew, IsDeleted, IsDisconnected)
		/// <summary>
		/// Indikuje, zdali byla data objektu změněna oproti datům v databázi.
		/// Při nastavení na false zruší zavolá CleanDirty.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
			protected internal set
			{
				_isDirty = value;
				if (!value)
				{
					CleanDirty();
				}
			}
		}
		private bool _isDirty;

		/// <summary>
		/// Indikuje, zdali byla data objektu načtena z databáze,
		/// resp. zdali je potřeba objekt nahrávat z databáze.
		/// </summary>
		public bool IsLoaded
		{
			get { return _isLoaded; }
			protected set { _isLoaded = value; }
		}
		private bool _isLoaded;

		/// <summary>
		/// Indikuje, zdali jde o nový objekt bez perzistence, který nebyl dosud uložen do databáze.
		/// Čeká na INSERT.
		/// </summary>
		public bool IsNew
		{
			get { return _isNew; }
			protected set { _isNew = value; }
		}
		private bool _isNew;

		/// <summary>
		/// Indikuje, zda lze objekt považovat za smazaný.
		/// Pro objekty mazané fyzickým odstraněním záznamu je true po zavolání metody Delete.
		/// Pro objekty mazané příznakem je true, pokud je nastaven příznak smazání (mj. po zavolání metody Delete), řešeno pomocí generovaného kódu.
		/// </summary>
		public virtual bool IsDeleted
		{
			get { return _isDeleted; }			
		}

		private bool _isDeleted = false;

		/// <summary>
		/// Indikuje, zdali byla nad objektem zavoláma metoda Deleted.
		/// </summary>
		internal bool IsDeleting
		{
			get { return this._isDeleting; }
			set { this._isDeleting = value; }
		}
		private bool _isDeleting;

		/// <summary>
		/// Indikuje, zdali je objekt zrovna ukládán (hlídá cyklické reference při ukládání).
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected internal bool IsSaving
		{
			get { return _isSaving; }
			set { _isSaving = value; }
		}
		private bool _isSaving = false;
		
		/// <summary>
		/// Indikuje, zda jde o disconnected business objekt.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool IsDisconnected
		{
			get { return _isDisconnected; }
		}
		private bool _isDisconnected = false;
		#endregion

		#region Constructors
		/// <summary>
		/// Implementační konstruktor.
		/// </summary>
		/// <param name="id">ID objektu (PK)</param>
		/// <param name="isNew">indikuje nový objekt</param>
		/// <param name="isDirty">indikuje objekt změněný vůči perzistentnímu uložišti</param>
		/// <param name="isLoaded">indikuje načtený objekt</param>
		/// <param name="isDisconnected">indikuje disconnected objekt</param>
		protected BusinessObjectBase(int id, bool isNew, bool isDirty, bool isLoaded, bool isDisconnected)
		{
			this._id = id;
			this._isNew = isNew;
			this._isDirty = isDirty;
			this._isLoaded = isLoaded;
			this._isDisconnected = isDisconnected;

			Init();
		}

		/// <summary>
		/// Konstruktor pro nový objekt (bez perzistence v databázi).
		/// </summary>
		protected BusinessObjectBase(ConnectionMode connectionMode) : this(
			NoID,		// ID
			true,		// IsNew
			false,		// IsDirty
			true,       // IsLoaded
			connectionMode == ConnectionMode.Disconnected)		// IsDisconnected
		{
			/*
			this._id = NoID;
			this._isNew = true;
			this._isDirty = false;
			this._isLoaded = true;

			Init();
			*/
		}

		/// <summary>
		/// Konstruktor pro objekt s obrazem v databázi (perzistentní).
		/// </summary>
		/// <param name="id">primární klíč objektu</param>
		/// <param name="connectionMode">režim vytvářeného objektu (connected/disconnected)</param>
		protected BusinessObjectBase(int id, ConnectionMode connectionMode)
			: this(
			id,		// ID
			false,	// IsNew
			false,	// IsDirty
			connectionMode == ConnectionMode.Disconnected,	// IsLoaded
			connectionMode == ConnectionMode.Disconnected)	// IsOffline
		{
			if (id == NoID)
			{
				throw new InvalidOperationException("Nelze vytvořit objekt, který by nebyl nový a měl NoID.");
			}

			/*
			this._id = id;
			this._isLoaded = false;
			this._isDirty = false;

			Init();
			 */
		}
		#endregion

		#region Load logika
		// zámek pro inicializaci zámku pro načítání objektů
		private static readonly object loadLockInitializerLock = new object();
		// zámek pro načítání objektů
		private volatile object loadLock;

		/// <summary>
		/// Nahraje objekt z perzistentního uložiště.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je již objekt načten a není určena transakce (null), znovu se nenahrává.
		/// Pokud je transakce určena, načte se znovu.
		/// Pokud se načtení podaří (nebo není načítání třeba, tj. není určena transakce a objekt je již načten), vrací true. Jinak (např. v případě neexistence dat pro objekt) vrací false.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které má být objekt načten; null, pokud bez transakce</param>
		public bool TryLoad(DbTransaction transaction)
		{
			if (this.IsLoaded && (transaction == null))
			{
				// pokud je již objekt načten, nenačítáme ho znovu
				return true;
			}

			// načítání se zamyká kvůli cachovaným readonly objektům
			// tam je sdílena instance, která by mohla být načítána najednou ze dvou threadů
			// protože ale nechceme mít vyrobeny instance loadLocků (žerou dost zbytečně paměť), vyrobíme je až při potřebě
			// a i toto vyrábění musíme nějak chránit - proto používáme loadLockInitializerLock
			if (loadLock == null)
			{
				lock (loadLockInitializerLock)
				{
					if (loadLock == null)
					{
						loadLock = new object();
					}
				}
			}

			lock (loadLock)
			{
				if (this.IsLoaded)
				{
					return true;
				}
				bool successful = TryLoad_Perform(transaction);
				if (successful)
				{
					this.IsLoaded = true;
					this.IsDirty = false; // načtený objekt není Dirty.			
				}
				return successful;

			}
		}

		/// <summary>
		/// Nahraje objekt z perzistentního uložiště.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je již objekt načten a není určena transakce (null), znovu se nenahrává.
		/// Pokud je transakce určena, načte se znovu.
		/// Pokud se načtení nedaří, je vyhozena výjimka.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které má být objekt načten; null, pokud bez transakce</param>
		public void Load(DbTransaction transaction)
		{
			if (!TryLoad(transaction))
			{
				// JK: Popis výjimky není přesný.
				throw new InvalidOperationException(String.Format("Pro objekt {0}.ID={1} se nepodařilo získat data z databáze.", this.GetType().FullName, this.ID));
			}
		}

		/// <summary>
		/// Nahraje objekt z perzistentního uložiště, bez transakce.
		/// Pokud se načtení nedaří, je vyhozena výjimka.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je již objekt načten, znovu se nenahrává.
		/// </remarks>
		public void Load()
		{
			Load(null);
		}

		/// <summary>
		/// Nahraje objekt z perzistentního uložiště, bez transakce.
		/// Pokud se načtení podaří, vrací true, jinak (např. v případě neexistence dat pro objekt) vrací false.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je již objekt načten, znovu se nenahrává.
		/// </remarks>
		public bool TryLoad()
		{
			return TryLoad(null);
		}

		/// <summary>
		/// Výkonná část nahrání objektu z perzistentního uložiště.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které má být objekt načten; null, pokud bez transakce</param>
		/// <returns>True, pokud se podařilo objekt načíst, jinak false.</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member", Justification = "Jde o template metodu volanou z metody TryLoad.")]
		protected abstract bool TryLoad_Perform(DbTransaction transaction);
		#endregion

		#region Save logika
		/// <summary>
		/// Uloží objekt do databáze, s případným použitím VNĚJŠÍ transakce.
		/// </summary>
		/// <remarks>
		/// Metoda neprovede uložení objektu, pokud není nahrán (!IsLoaded), není totiž ani co ukládat,
		/// data nemohla být změněna, když nebyla ani jednou použita.<br/>
		/// Metoda také neprovede uložení, pokud objekt nebyl změněn a současně nejde o nový objekt (!IsDirty &amp;&amp; !IsNew).<br/>
		/// Metoda nezakládá vlastní transakci, která by sdružovala uložení kolekcí, členských objektů a vlastních dat!!!
		/// Příslušná transakce musí být předána (explicitní transakci doplňuje až ActiveRecordBusinessObjectbase).<br/>
		/// Mazání objektů rovněž ukládá přes tuto metodu.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které má být objekt uložen; null, pokud bez transakce</param>
		public virtual void Save(DbTransaction transaction)
		{			
			if (!IsLoaded || IsSaving)
			{
				return;
			}

			IsSaving = true; // řeší cyklické reference při ukládání objektových struktur

			bool wasNew = IsNew;
			bool callBeforeAfterSave = IsDirty; // pokrývá i situaci, kdy je objekt nový
			if (callBeforeAfterSave) // zavoláno pro změněné (a tedy i nové) objekty
			{
				OnBeforeSave(new BeforeSaveEventArgs(transaction));
			}

			if (IsDirty && !IsDeleting)
			{
				CheckConstraints();
			}

			Save_Perform(transaction);
			// Uložený objekt není už nový, dostal i přidělené ID.
			// Pro generovaný kód BL je zbytečné, ten IsNew nastavuje i ve vygenerovaných
			// metodách pro MinimalInsert a FullInsert.
			IsNew = false; 
			IsDirty = false; // uložený objekt je aktuální

			if (callBeforeAfterSave)
			{
				OnAfterSave(new AfterSaveEventArgs(transaction, wasNew));
			}
			IsSaving = false;
		}

		/// <summary>
		/// Uloží objekt do databáze, bez transakce. Nový objekt je vložen INSERT, existující objekt je aktualizován UPDATE.
		/// </summary>
		/// <remarks>
		/// Metoda neprovede uložení objektu, pokud není nahrán (!IsLoaded), není totiž ani co ukládat,
		/// data nemohla být změněna, když nebyla ani jednou použita.<br/>
		/// Metoda také neprovede uložení, pokud objekt nebyl změněn a současně nejde o nový objekt (!IsDirty &amp;&amp; !IsNew)
		/// </remarks>
		public void Save()
		{
			Save(null);
		}

		/// <summary>
		/// Výkonná část uložení objektu do perzistentního uložiště.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které má být objekt uložen; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member", Justification = "Jde o template metodu volanou z metody Save.")]
		protected abstract void Save_Perform(DbTransaction transaction);

		/// <summary>
		/// Volá se před jakýmkoliv uložením objektu, tj. i před smazáním.
		/// V každém spuštění uložení grafu objektů se metoda volá právě jednou, na rozdíl od Save, který může být (a je) spouštěn opakovaně v případě ukládání stromů objektů.
		/// Metoda se volá před zavoláním validační metody CheckConstrains.
		/// </summary>
		protected virtual void OnBeforeSave(BeforeSaveEventArgs transactionEventArgs)
		{
			// metoda vznikla jako reseni problemu opakovaneho volani Save a logiky, kterou jsme do Save vsude psali
			// az budeme potrebovat, implementujeme udalost oznamujici okamzik pred ulozeni objektu (a pred jeho validaci).
		}

		/// <summary>
		/// Volá se po jakémkoliv uložení objektu, tj. i po smazání objektu.
		/// V každém spuštění uložení grafu objektů se metoda volá právě jednou, na rozdíl od Save, který může být (a je) spouštěn opakovaně v případě ukládání stromů objektů.
		/// Metoda se volá po nastavení příznaků IsDirty, IsNew, apod.
		/// </summary>        
		protected virtual void OnAfterSave(AfterSaveEventArgs transactionEventArgs)
		{
			// metoda vznikla jako reseni problemu opakovaneho volani Save a logiky, kterou jsme do Save vsude psali
			// az budeme potrebovat, implementujeme udalost oznamujici okamzik pred ulozeni objektu (a pred jeho validaci).
		}

		#endregion

		#region Delete logika
		/// <summary>
		/// Smaže objekt, nebo ho označí jako smazaný, podle zvolené logiky. Změnu uloží do databáze, v transakci.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je již objekt smazán.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které se smazání provede; null, pokud bez transakce</param>
		public virtual void Delete(DbTransaction transaction)
		{
			if (IsNew)
			{
				throw new InvalidOperationException("Nový objekt nelze smazat.");
			}
			
			EnsureLoaded(transaction);

			if (IsDeleting)
			{
				return;
			}

			IsDirty = true;
			IsDeleting = true;
			Save(transaction);
			IsDeleting = false;
			// nastavíme lokální proměnou, která se používá pro getter generátorem nepřepsané vlastnosti IsDeleted.
			_isDeleted = true;
		}

		/// <summary>
		/// Smaže objekt, nebo ho označí jako smazaný, podle zvolené logiky. Změnu uloží do databáze, bez transakce.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je již objekt smazán.
		/// </remarks>
		public void Delete()
		{
			Delete(null);			
		}

		/// <summary>
		/// Implementace metody vymaže objekt z perzistentního uložiště nebo ho označí jako smazaný.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v rámci které se smazání provede; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member", Justification = "Jde o template metodu volanou z metody Save.")]
		protected abstract void Delete_Perform(DbTransaction transaction);
		#endregion

		#region Implementační metody - EnsureLoaded

		/// <summary>
		/// Ověří, jestli jsou data objektu načtena z databáze (IsLoaded). Pokud nejsou, provede jejich načtení.
		/// </summary>
		/// <remarks>
		/// Metoda EnsureLoaded se volá před každou operací, která potřebuje data objektu. Zajištuje lazy-load.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void EnsureLoaded()
		{
			EnsureLoaded(null);
		}

		/// <summary>
		/// Ověří, jestli jsou data objektu načtena z databáze (IsLoaded). Pokud nejsou, provede jejich načtení.
		/// </summary>
		/// <remarks>
		/// Metoda EnsureLoaded se volá před každou operací, která potřebuje data objektu. Zajištuje lazy-load.
		/// </remarks>
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected void EnsureLoaded(DbTransaction transaction)
		{
			if (IsLoaded || IsNew)
			{
				return;
			}

			Load(transaction);
		}

		/*
		/// <summary>
		/// Metoda zkontroluje rovnost dvou objektů - jestliže nejsou stejné, je objekt označen jako změněný (IsDirty = true).
		/// </summary>
		/// <remarks>
		/// Metoda se používá zejména v set-accesorech properties, kde hlídá, jestli dochází ke změně,
		/// kterou bude potřeba uložit.
		/// </remarks>
		/// <param name="currentValue">dosavadní hodnota</param>
		/// <param name="newValue">nová hodnota</param>
		/// <returns>false, pokud jsou hodnoty stejné; true, pokud dochází ke změně</returns>
		protected bool CheckChange(object currentValue, object newValue)
		{
			if (!Object.Equals(currentValue, newValue))
			{
				IsDirty = true;
				return true;
			}
			return false;
		}
		 */
		#endregion

		#region Equals, GetHashCode, operátory == a != (override)
		/// <summary>
		/// Zjistí rovnost druhého objektu s instancí. Základní implementace porovná jejich ID.
		/// Nové objekty jsou si rovny v případě identity (stejná reference).
		/// </summary>
		/// <param name="obj">objekt k porovnání</param>
		/// <returns>true, pokud jsou si rovny; jinak false</returns>
		public virtual bool Equals(BusinessObjectBase obj)
		{
			if ((obj == null) || (this.GetType() != obj.GetType()))
			{
				return false;
			}
			
			// nové objekty jsou si rovny pouze v případě identity (stejná reference)
			if (this.IsNew || obj.IsNew)
			{
				return Object.ReferenceEquals(this, obj);
			}
			
			// běžné objekty jsou si rovny, pokud mají stejné ID
			if (!Object.Equals(this.ID, obj.ID))
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Zjistí rovnost druhého objektu s instancí. Základní implementace porovná jejich ID.
		/// </summary>
		/// <param name="obj">objekt k porovnání</param>
		/// <returns>true, pokud jsou si rovny; jinak false</returns>
		public override bool Equals(object obj)
		{
			BusinessObjectBase bob = obj as BusinessObjectBase;
			if (bob != null)
			{
				return this.Equals(bob);
			}
			return false;
		}

		/// <summary>
		/// Operátor ==, ověřuje rovnost ID.
		/// </summary>
		/// <param name="objA">první objekt</param>
		/// <param name="objB">druhý objekt</param>
		/// <returns>true, pokud mají objekty stejné ID; jinak false</returns>
		public static bool operator ==(BusinessObjectBase objA, BusinessObjectBase objB)
		{
			return Object.Equals(objA, objB);
		}

		/// <summary>
		/// Operátor !=, ověřuje rovnost ID.
		/// </summary>
		/// <param name="objA">první objekt</param>
		/// <param name="objB">druhý objekt</param>
		/// <returns>false, pokud mají objekty stejné ID; jinak true</returns>
		public static bool operator !=(BusinessObjectBase objA, BusinessObjectBase objB)
		{
			return !Object.Equals(objA, objB);
		}

		/// <summary>
		/// Vrací ID jako HashCode.
		/// </summary>
		public override int GetHashCode()
		{
			return this.ID;
		}
		#endregion

		#region CheckConstraints
		/// <summary>
		/// Kontroluje konzistenci objektu jako celku.
		/// </summary>
		/// <remarks>
		/// Automaticky je voláno před ukládáním objektu Save(), pokud je objekt opravdu ukládán.
		/// </remarks>
		protected virtual void CheckConstraints()
		{
		}
		#endregion

		#region Init
		/// <summary>
		/// Inicializační metoda, která je volána při vytvoření objektu (přímo z konstruktorů).
		/// Připravena pro override potomky.
		/// </summary>
		/// <remarks>
		/// Metoda Init() je zamýšlena mj. pro incializaci PropertyHolderů (vytvoření instance) a kolekcí (vytvoření instance, navázání událostí).
		/// </remarks>
		protected virtual void Init()
		{
			// NOOP
		}
		#endregion

		/// <summary>
		/// Metoda, která je volána při změně stavu objektu z Dirty na čisty (IsDirty: true -> false).
		/// Připravena pro override potomky.
		/// </summary>
		/// <remarks>
		/// Metoda CleanDirty() je zamýšlena k nastavení IsDirty=false property holderům business objektů.
		/// </remarks>
		protected virtual void CleanDirty()
		{
			// NOOP
		}

		/**********************************************************************************/

		#region GetNullableID (static)
		/// <summary>
		/// Vrátí ID objektu, nebo null, pokud je vstupní objekt null.
		/// Určeno pro přehledné získávání ID, obvykle při předávání do DB.
		/// </summary>
		/// <param name="businessObject">objekt, jehož ID chceme</param>
		/// <returns>ID objektu, nebo null, pokud je vstupní objekt null</returns>
		public static int? GetNullableID(BusinessObjectBase businessObject)
		{
			if (businessObject == null)
			{
				return null;
			}
			return businessObject.ID;
		}
		#endregion

		#region FastIntParse (static)
		/// <summary>
		/// Převede text na číslo.
		/// Předpokládá se korektnost hodnoty, neprovádí se žádná kontrola.
		/// Číslo musí být v "invariant formátu" - cifry bez oddělovačů, může být záporné.
		/// </summary>
		protected internal static int FastIntParse(string value)
		{
			unchecked
			{

				int result = 0;
				byte negative = (byte)(((value.Length > 0) && (value[0] == '-')) ? 1 : 0);

				byte l = (byte)value.Length;
				for (byte i = negative; i < l; i++)
				{
					//char letter = value[i];
					result = 10 * result + (value[i] - 48);
				}

				return negative == 0 ? result : -1 * result;
			}
		}
		#endregion

		/// <summary>
		/// Přepne objekt do stavu disconnected.
		/// Metoda je interní, aby nebyla dostupná na objektu přímo. Je pak přístupná pomocí extension metody z namespace TestExtensions.
		/// </summary>
		internal void SetDisconnected()
		{
			if (!IsDisconnected)
			{
				_isDisconnected = true;

				if (/*!IsNew && */!IsLoaded) // nový objekt je automaticky IsLoaded = true
				{
					// Novému objektu proběhne již proběhl Init() včetně nastavení hodnot vlastnostem.
					// Načtenému objektu byly hodnoty nastaveny v loadu.
					// Takže objektu, který není nový a není ani načtený (ghost), zavoláme (znovu) Init. Nastaví se mu nové instance property holderů, ale to nevadí.
					
					Init(); // Nyní je již objekt ve stavu IsDisconnected.
					_isLoaded = true;
				}
			}
		}
	}
}
