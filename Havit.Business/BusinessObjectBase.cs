using System;
using System.Collections.Generic;
using System.Text;
using Havit.Data;
using System.Data.Common;
using System.Collections.ObjectModel;

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
	[Serializable]
	public abstract class BusinessObjectBase
	{
		#region Consts
		/// <summary>
		/// Hodnota, kterou m� ID objektu neulo�en�ho v datab�zi (bez perzistence).
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase", MessageId = "Member")]
		public const int NoID = -1;
		#endregion

		#region Events
		/// <summary>
		/// Ud�lost, kter� se vol� p�ed ukl�d�n�m, je�t� p�ed constraint-checky.
		/// </summary>
		public event EventHandler<EventArgs> PreSave;
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
		public bool IsSaving
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
		internal protected Collection<PropertyHolderBase> PropertyHolders
		{
			get
			{
				return _propertyHolders;
			}
		}
		private Collection<PropertyHolderBase> _propertyHolders = new Collection<PropertyHolderBase>();
		#endregion

		#region Constructors
		/// <summary>
		/// Konstruktor pro nov� objekt (bez perzistence v datab�zi).
		/// </summary>
		protected BusinessObjectBase()
		{
			this._id = NoID;
			this._isNew = true;
			this._isDirty = false;
			this._isLoaded = true;

			Init();
		}

		/// <summary>
		/// Konstruktor pro objekt s obrazem v datab�zi (perzistentn�).
		/// </summary>
		/// <param name="id">prim�rn� kl�� objektu</param>
		protected BusinessObjectBase(int id)
		{
			this._id = id;
			this._isLoaded = false;
			this._isDirty = false;

			Init();
		}
		#endregion

		#region Load logika
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

			Load_Perform(transaction);

			this.IsLoaded = true;
			this.IsDirty = false; // na�ten� objekt nen� Dirty.
		}

		/// <summary>
		/// Nahraje objekt z perzistentn�ho ulo�i�t�, bez transakce.
		/// </summary>
		/// <remarks>
		/// Pozor, pokud je ji� objekt na�ten, znovu se nenahr�v�.
		/// </remarks>
		public virtual void Load()
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

		#region OnPreSave
		/// <summary>
		/// Spou�t� ud�lost <see cref="PreSave"/>.
		/// </summary>
		/// <param name="e">pr�dn�</param>
		public virtual void OnPreSave(EventArgs e)
		{
			if (PreSave != null)
			{
				PreSave(this, e);
			}
		}
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
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		public virtual void Save(DbTransaction transaction)
		{			
			if (!IsLoaded || IsSaving)
			{
				return;
			}

			IsSaving = true; // �e�� cyklick� reference p�i ukl�d�n� objektov�ch struktur

			OnPreSave(EventArgs.Empty);
			CheckConstraints();

			if (IsNew)
			{
				Save_SaveMembers(transaction);
				Save_Perform(transaction);
				Save_SaveCollections(transaction);
			}
			else
			{
				Save_SaveMembers(transaction);
				Save_SaveCollections(transaction);
				if (IsDirty)
				{
					Save_Perform(transaction);
				}
			}

			IsNew = false; // ulo�en� objekt nen� u� nov�, dostal i p�id�len� ID
			IsDirty = false; // ulo�en� objekt je aktu�ln�
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
		/// Ukl�d� member-objekty.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� maj� b�t member-objekty ulo�eny; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected virtual void Save_SaveMembers(DbTransaction transaction)
		{
			// NOOP
		}

		/// <summary>
		/// Ukl�d� member-kolekce objektu.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� maj� b�t member-kolekce ulo�eny; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected virtual void Save_SaveCollections(DbTransaction transaction)
		{
			// NOOP
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
			EnsureLoaded();

			if (IsDeleted)
			{
				return;
			}

			Delete_Perform(transaction);

			IsDeleted = true;
		}

		/// <summary>
		/// Sma�e objekt, nebo ho ozna�� jako smazan�, podle zvolen� logiky. Zm�nu ulo�� do datab�ze, bez transakce.
		/// </summary>
		/// <remarks>
		/// Neprovede se, pokud je ji� objekt smaz�n.
		/// </remarks>
		public virtual void Delete()
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
		/// </summary>
		/// <param name="obj">objekt k porovn�n�</param>
		/// <returns>true, pokud jsou si rovny; jinak false</returns>
		public virtual bool Equals(BusinessObjectBase obj)
		{
			if ((obj == null) || (this.GetType() != obj.GetType()))
			{
				return false;
			}
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
	}
}
