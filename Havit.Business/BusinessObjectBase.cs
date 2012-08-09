using System;
using System.Collections.Generic;
using System.Text;
using Havit.Data;
using System.Data.Common;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Havit.Business
{
	/// <summary>
	/// B�zov� t��da pro v�echny business-objekty, kter� definuje jejich z�kladn� chov�n�, zejm�na ve vztahu k datab�zi (Layer Supertype).
	/// </summary>
	/// <remarks>
	/// T��da je z�kladem pro v�echny business-objekty a implementuje z�kladn� pattern pro komunikaci s perzistentn�mi ulo�i�ti.
	/// Na��t�n� je implementov�no jako lazy-load, kdy je objekt nejprve vytvo�en pr�zdn� jako ghost se sv�m ID a teprve
	/// p�i prvn� pot�eb� je iniciov�no jeho �pln� na�ten�.<br/>
	/// </remarks>
	[DebuggerDisplay("{GetType().FullName,nq} (ID={IsNew ? \"New\" : ID.ToString(),nq}, IsLoaded={IsLoaded,nq}, IsDirty={IsDirty,nq})")]
	public abstract class BusinessObjectBase
	{
		#region Consts
		/// <summary>
		/// Hodnota, kterou m� ID objektu neulo�en�ho v datab�zi (bez perzistence).
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
		public const int NoID = Int32.MinValue;
		#endregion

		#region Property - ID
		/// <summary>
		/// Prim�rn� kl�� objektu.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
		public int ID
		{
			get { return _id; }
			protected set { _id = value; }
		}
		private int _id;
		#endregion

		#region Properties - Stav objektu (IsDirty, IsLoaded, IsNew, IsDeleted)
		/// <summary>
		/// Indikuje, zda-li byla data objektu zm�n�na oproti dat�m v datab�zi.
		/// P�i nastaven� na false zru�� p��znak zm�n v�em PropertyHolder�m.
		/// </summary>
		public bool IsDirty
		{
			get { return _isDirty; }
			protected internal set
			{
				_isDirty = value;
				if (!value)
					foreach (PropertyHolderBase propertyHolder in PropertyHolders)
						propertyHolder.IsDirty = false;
			}
		}
		private bool _isDirty;

		/// <summary>
		/// Indikuje, zda-li byla data objektu na�tena z datab�ze,
		/// resp. zda-li je pot�eba objekt nahr�vat z datab�ze.
		/// </summary>
		public bool IsLoaded
		{
			get { return _isLoaded; }
			protected set { _isLoaded = value; }
		}
		private bool _isLoaded;

		/// <summary>
		/// Indikuje, zda-li jde o nov� objekt bez perzistence, kter� nebyl dosud ulo�en do datab�ze.
		/// �ek� na INSERT.
		/// </summary>
		public bool IsNew
		{
			get { return _isNew; }
			protected set { _isNew = value; }
		}
		private bool _isNew;

		/// <summary>
		/// Indikuje, zda-li je objekt smaz�n z datab�ze, p��padn� je v n� ozna�en jako smazan�.
		/// </summary>
		public bool IsDeleted
		{
			get { return _isDeleted; }
			protected set { _isDeleted = value; }
		}
		private bool _isDeleted;

		/// <summary>
		/// Indikuje, zda-li je objekt zrovna ukl�d�n (hl�d� cyklick� reference p�i ukl�d�n�).
		/// </summary>
		protected internal bool IsSaving
		{
			get { return _isSaving; }
			set { _isSaving = value; }
		}
		private bool _isSaving = false;
		#endregion

		#region PropertyHolders
		/// <summary>
		/// Kolekce referenc� na jednotliv� property-holder objekty.
		/// </summary>
		/// <remarks>
		/// Kolekce je ur�ena pro hromadn� operace s property-holdery. Jednotliv� property si reference na sv� property-holdery udr�uj� v private fieldu.
		/// </remarks>
		internal protected List<PropertyHolderBase> PropertyHolders
		{
			get
			{
				return _propertyHolders;
			}
		}
		private List<PropertyHolderBase> _propertyHolders = new List<PropertyHolderBase>(16);
		#endregion

		#region Constructors
		/// <summary>
		/// Implementa�n� konstruktor.
		/// </summary>
		/// <param name="id">ID objektu (PK)</param>
		/// <param name="isNew">indikuje nov� objekt</param>
		/// <param name="isDirty">indikuje objekt zm�n�n� v��i perzistentn�mu ulo�i�ti</param>
		/// <param name="isLoaded">indikuje na�ten� objekt</param>
		protected internal BusinessObjectBase(int id, bool isNew, bool isDirty, bool isLoaded)
		{
			this._id = id;
			this._isNew = isNew;
			this._isDirty = isDirty;
			this._isLoaded = isLoaded;

			Init();
		}

		/// <summary>
		/// Konstruktor pro nov� objekt (bez perzistence v datab�zi).
		/// </summary>
		protected BusinessObjectBase()
			: this(
			NoID,		// ID
			true,		// IsNew
			false,		// IsDirty
			true)		// IsLoaded

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
		/// Konstruktor pro objekt s obrazem v datab�zi (perzistentn�).
		/// </summary>
		/// <param name="id">prim�rn� kl�� objektu</param>
		protected BusinessObjectBase(int id)
			: this(
			id,		// ID
			false,	// IsNew
			false,	// IsDirty
			false)	// IsLoaded
		{
			if (id == NoID)
			{
				throw new InvalidOperationException("Nelze vytvo�it objekt, kter� by nebyl nov� a m�l NoID.");
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
		// z�mek pro na��t�n� objekt�
		private object loadLock = new object();

		/// <summary>
		/// Nahraje objekt z perzistentn�ho ulo�i�t�, bez transakce.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je ji� objekt na�ten a nen� ur�ena transakce (null), znovu se nenahr�v�.
		/// Pokud je transakce ur�ena, na�te se znovu.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt na�ten; null, pokud bez transakce</param>
		public virtual void Load(DbTransaction transaction)
		{
			if (this.IsLoaded && (transaction == null))
			{
				// pokud je ji� objekt na�ten, nena��t�me ho znovu
				return;
			}

			// na��t�n� se zamyk� kv�li cachovan�m readonly objekt�m
			// tam je sd�lena instance, kter� by mohla b�t na��t�na najednou ze dvou thread�
			lock (loadLock)
			{
				if (!this.IsLoaded)
				{
					Load_Perform(transaction);
					this.IsLoaded = true;
					this.IsDirty = false; // na�ten� objekt nen� Dirty.			
				}
			}
		}

		/// <summary>
		/// Nahraje objekt z perzistentn�ho ulo�i�t�, bez transakce.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je ji� objekt na�ten, znovu se nenahr�v�.
		/// </remarks>
		public void Load()
		{
			Load(null);
		}

		/// <summary>
		/// V�konn� ��st nahr�n� objektu z perzistentn�ho ulo�i�t�.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt na�ten; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Load_Perform(DbTransaction transaction);
		#endregion

		#region Save logika
		/// <summary>
		/// Ulo�� objekt do datab�ze, s p��padn�m pou�it�m VN�J�� transakce.
		/// </summary>
		/// <remarks>
		/// Metoda neprovede ulo�en� objektu, pokud nen� nahr�n (!IsLoaded), nen� toti� ani co ukl�dat,
		/// data nemohla b�t zm�n�na, kdy� nebyla ani jednou pou�ita.<br/>
		/// Metoda tak� neprovede ulo�en�, pokud objekt nebyl zm�n�n a sou�asn� nejde o nov� objekt (!IsDirty &amp;&amp; !IsNew).<br/>
		/// Metoda nezakl�d� vlastn� transakci, kter� by sdru�ovala ulo�en� kolekc�, �lensk�ch objekt� a vlastn�ch dat!!!
		/// P��slu�n� transakce mus� b�t p�ed�na (explicitn� transakci dopl�uje a� ActiveRecordBusinessObjectbase).<br/>
		/// Maz�n� objekt� rovn� ukl�d� p�es tuto metodu.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		public virtual void Save(DbTransaction transaction)
		{			
			if (!IsLoaded || IsSaving)
			{
				return;
			}

			IsSaving = true; // �e�� cyklick� reference p�i ukl�d�n� objektov�ch struktur

			bool wasNew = IsNew;
			bool callBeforeAfterSave = IsDirty; // pokr�v� i situaci, kdy je objekt nov�
			if (callBeforeAfterSave) // zavol�no pro zm�n�n� (a tedy i nov�) objekty
			{
				OnBeforeSave(new BeforeSaveEventArgs(transaction));
			}

			if (IsDirty && !IsDeleted)
			{
				CheckConstraints();
			}

			Save_Perform(transaction);
			// Ulo�en� objekt nen� u� nov�, dostal i p�id�len� ID.
			// Pro generovan� k�d BL je zbyte�n�, ten IsNew nastavuje i ve vygenerovan�ch
			// metod�ch pro MinimalInsert a FullInsert.
			IsNew = false; 
			IsDirty = false; // ulo�en� objekt je aktu�ln�

			if (callBeforeAfterSave)
			{
				OnAfterSave(new AfterSaveEventArgs(transaction, wasNew));
			}
			IsSaving = false;
		}

		/// <summary>
		/// Ulo�� objekt do datab�ze, bez transakce. Nov� objekt je vlo�en INSERT, existuj�c� objekt je aktualizov�n UPDATE.
		/// </summary>
		/// <remarks>
		/// Metoda neprovede ulo�en� objektu, pokud nen� nahr�n (!IsLoaded), nen� toti� ani co ukl�dat,
		/// data nemohla b�t zm�n�na, kdy� nebyla ani jednou pou�ita.<br/>
		/// Metoda tak� neprovede ulo�en�, pokud objekt nebyl zm�n�n a sou�asn� nejde o nov� objekt (!IsDirty &amp;&amp; !IsNew)
		/// </remarks>
		public void Save()
		{
			Save(null);
		}

		/// <summary>
		/// V�konn� ��st ulo�en� objektu do perzistentn�ho ulo�i�t�.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Save_Perform(DbTransaction transaction);

		/// <summary>
		/// Vol� se p�ed jak�mkoliv ulo�en�m objektu, tj. i p�ed smaz�n�m.
		/// V ka�d�m spu�t�n� ulo�en� grafu objekt� se metoda vol� pr�v� jednou, na rozd�l od Save, kter� m��e b�t (a je) spou�t�n opakovan� v p��pad� ukl�d�n� strom� objekt�.
		/// Metoda se vol� p�ed zavol�n�m valida�n� metody CheckConstrains.
		/// </summary>
		protected virtual void OnBeforeSave(BeforeSaveEventArgs transactionEventArgs)
		{
			// metoda vznikla jako reseni problemu opakovaneho volani Save a logiky, kterou jsme do Save vsude psali
			// az budeme potrebovat, implementujeme udalost oznamujici okamzik pred ulozeni objektu (a pred jeho validaci).
		}

		/// <summary>
		/// Vol� se po zak�mkoliv ulo�en� objektu, tj. i po smaz�n� objektu.
		/// V ka�d�m spu�t�n� ulo�en� grafu objekt� se metoda vol� pr�v� jednou, na rozd�l od Save, kter� m��e b�t (a je) spou�t�n opakovan� v p��pad� ukl�d�n� strom� objekt�.
		/// Metoda se vol� po nastaven� p��znak� IsDirty, IsNew, apod.
		/// </summary>        
		protected virtual void OnAfterSave(AfterSaveEventArgs transactionEventArgs)
		{
			// metoda vznikla jako reseni problemu opakovaneho volani Save a logiky, kterou jsme do Save vsude psali
			// az budeme potrebovat, implementujeme udalost oznamujici okamzik pred ulozeni objektu (a pred jeho validaci).
		}

		#endregion

		#region Delete logika
		/// <summary>
		/// Sma�e objekt, nebo ho ozna�� jako smazan�, podle zvolen� logiky. Zm�nu ulo�� do datab�ze, v transakci.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je ji� objekt smaz�n.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� se smaz�n� provede; null, pokud bez transakce</param>
		public virtual void Delete(DbTransaction transaction)
		{
			if (IsNew)
			{
				throw new InvalidOperationException("Nov� objekt nelze smazat.");
			}
			
			EnsureLoaded();

			if (IsDeleted)
			{
				return;
			}

			IsDirty = true;
			IsDeleted = true;
			Save(transaction);
		}

		/// <summary>
		/// Sma�e objekt, nebo ho ozna�� jako smazan�, podle zvolen� logiky. Zm�nu ulo�� do datab�ze, bez transakce.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je ji� objekt smaz�n.
		/// </remarks>
		public void Delete()
		{
			Delete(null);
		}

		/// <summary>
		/// Implementace metody vyma�e objekt z perzistentn�ho ulo�i�t� nebo ho ozna�� jako smazan�.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� se smaz�n� provede; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Delete_Perform(DbTransaction transaction);
		#endregion

		#region Implementa�n� metody - EnsureLoaded, CheckChange
		/// <summary>
		/// Ov���, jestli jsou data objektu na�tena z datab�ze (IsLoaded). Pokud nejsou, provede jejich na�ten�.
		/// </summary>
		/// <remarks>
		/// Metoda EnsureLoaded se vol� p�ed ka�dou operac�, kter� pot�ebuje data objektu. Zaji�tuje lazy-load.
		/// </remarks>
		protected void EnsureLoaded()
		{
			if (IsLoaded || IsNew)
			{
				return;
			}

			Load();
		}

		/// <summary>
		/// Metoda zkontroluje rovnost dvou objekt� - jestli�e nejsou stejn�, je objekt ozna�en jako zm�n�n� (IsDirty = true).
		/// </summary>
		/// <remarks>
		/// Metoda se pou��v� zejm�na v set-accesorech properties, kde hl�d�, jestli doch�z� ke zm�n�,
		/// kterou bude pot�eba ulo�it.
		/// </remarks>
		/// <param name="currentValue">dosavadn� hodnota</param>
		/// <param name="newValue">nov� hodnota</param>
		/// <returns>false, pokud jsou hodnoty stejn�; true, pokud doch�z� ke zm�n�</returns>
		protected bool CheckChange(object currentValue, object newValue)
		{
			if (!Object.Equals(currentValue, newValue))
			{
				IsDirty = true;
				return true;
			}
			return false;
		}
		#endregion

		#region Equals, GetHashCode, oper�tory == a != (override)
		/// <summary>
		/// Zjist� rovnost druh�ho objektu s instanc�. Z�kladn� implementace porovn� jejich ID.
		/// Nov� objekty jsou si rovny v p��pad� identity (stejn� reference).
		/// </summary>
		/// <param name="obj">objekt k porovn�n�</param>
		/// <returns>true, pokud jsou si rovny; jinak false</returns>
		public virtual bool Equals(BusinessObjectBase obj)
		{
			if ((obj == null) || (this.GetType() != obj.GetType()))
			{
				return false;
			}
			
			// nov� objekty jsou si rovny pouze v p��pad� identity (stejn� reference)
			if (this.IsNew || obj.IsNew)
			{
				return Object.ReferenceEquals(this, obj);
			}
			
			// b�n� objekty jsou si rovny, pokud maj� stejn� ID
			if (!Object.Equals(this.ID, obj.ID))
			{
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Zjist� rovnost druh�ho objektu s instanc�. Z�kladn� implementace porovn� jejich ID.
		/// </summary>
		/// <param name="obj">objekt k porovn�n�</param>
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
		/// Oper�tor ==, ov��uje rovnost ID.
		/// </summary>
		/// <param name="objA">prvn� objekt</param>
		/// <param name="objB">druh� objekt</param>
		/// <returns>true, pokud maj� objekty stejn� ID; jinak false</returns>
		public static bool operator ==(BusinessObjectBase objA, BusinessObjectBase objB)
		{
			return Object.Equals(objA, objB);
		}

		/// <summary>
		/// Oper�tor !=, ov��uje rovnost ID.
		/// </summary>
		/// <param name="objA">prvn� objekt</param>
		/// <param name="objB">druh� objekt</param>
		/// <returns>false, pokud maj� objekty stejn� ID; jinak true</returns>
		public static bool operator !=(BusinessObjectBase objA, BusinessObjectBase objB)
		{
			return !Object.Equals(objA, objB);
		}

		/// <summary>
		/// Vrac� ID jako HashCode.
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
		/// Automaticky je vol�no p�ed ukl�d�n�m objektu Save(), pokud je objekt opravdu ukl�d�n.
		/// </remarks>
		protected virtual void CheckConstraints()
		{
		}
		#endregion

		#region Init
		/// <summary>
		/// Inicializa�n� metoda, kter� je vol�na p�i vytvo�en� objektu (p��mo z konstruktor�).
		/// P�ipravena pro override potomky.
		/// </summary>
		/// <remarks>
		/// Metoda Init() je zam��lena mj. pro incializaci PropertyHolder� (vytvo�en� instance) a kolekc� (vytvo�en� instance, nav�z�n� ud�lost�).
		/// </remarks>
		protected virtual void Init()
		{
			// NOOP
		}
		#endregion

		#region RegisterPropertyHolder (internal)
		/// <summary>
		/// Zaregistruje PropertyHolder do kolekce PropertyHolders.
		/// </summary>
		/// <remarks>
		/// Touto metodou se k objektu registruj� sami PropertyHoldery ve sv�ch constructorech.
		/// </remarks>
		/// <param name="propertyHolder">PropertyHolder k zaregistrov�n�</param>
		internal void RegisterPropertyHolder(PropertyHolderBase propertyHolder)
		{
			_propertyHolders.Add(propertyHolder);
		}
		#endregion

		/**********************************************************************************/

		#region GetNullableID (static)
		/// <summary>
		/// Vr�t� ID objektu, nebo null, pokud je vstupn� objekt null.
		/// Ur�eno pro p�ehledn� z�sk�v�n� ID, obvykle p�i p�ed�v�n� do DB.
		/// </summary>
		/// <param name="businessObject">objekt, jeho� ID chceme</param>
		/// <returns>ID objektu, nebo null, pokud je vstupn� objekt null</returns>
		public static int? GetNullableID(BusinessObjectBase businessObject)
		{
			if (businessObject == null)
			{
				return null;
			}
			return businessObject.ID;
		}
		#endregion
	}
}
