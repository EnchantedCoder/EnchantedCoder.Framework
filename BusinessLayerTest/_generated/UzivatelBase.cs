//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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
using Havit.Collections;
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
	public abstract class UzivatelBase : ActiveRecordBusinessObjectBase
	{
		#region Static constructor
		static UzivatelBase()
		{
			objectInfo = new ObjectInfo();
			properties = new UzivatelProperties();
			objectInfo.Initialize("dbo", "Uzivatel", false, Uzivatel.GetObject, Uzivatel.GetAll, properties.Deleted, properties.All);
			properties.Initialize(objectInfo);
		}
		#endregion
		
		#region Constructors
		/// <summary>
		/// Vytvoří instanci objektu jako nový prvek.
		/// </summary>
		protected UzivatelBase()
			: base()
		{
		}
		
		/// <summary>
		/// Vytvoří instanci existujícího objektu.
		/// </summary>
		/// <param name="id">UzivatelID (PK)</param>
		protected UzivatelBase(int id)
			: base(id)
		{
		}
		
		/// <summary>
		/// Vytvoří instanci objektu na základě dat (i částečných) načtených z databáze.
		/// </summary>
		/// <param name="id">UzivatelID (PK)</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu (i částečnými)</param>
		protected UzivatelBase(int id, DataRecord record)
			: base(id, record)
		{
		}
		#endregion
		
		#region Properties dle sloupců databázové tabulky
		/// <summary>
		/// Uživatelské jméno, kterým se uživatel přihlašuje.
		/// </summary>
		public virtual string Username
		{
			get
			{
				EnsureLoaded();
				return _UsernamePropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				if (value == null)
				{
					_UsernamePropertyHolder.Value = String.Empty;
				}
				else
				{
					_UsernamePropertyHolder.Value = value;
				}
			}
		}
		protected PropertyHolder<string> _UsernamePropertyHolder;
		
		/// <summary>
		/// Heslo uživatele.
		/// </summary>
		public virtual string Password
		{
			get
			{
				EnsureLoaded();
				return _PasswordPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				if (value == null)
				{
					_PasswordPropertyHolder.Value = String.Empty;
				}
				else
				{
					_PasswordPropertyHolder.Value = value;
				}
			}
		}
		protected PropertyHolder<string> _PasswordPropertyHolder;
		
		/// <summary>
		/// Jméno uživatele, jak se má zobrazovat pro přihlášení.
		/// </summary>
		public virtual string DisplayAs
		{
			get
			{
				EnsureLoaded();
				return _DisplayAsPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				if (value == null)
				{
					_DisplayAsPropertyHolder.Value = String.Empty;
				}
				else
				{
					_DisplayAsPropertyHolder.Value = value;
				}
			}
		}
		protected PropertyHolder<string> _DisplayAsPropertyHolder;
		
		/// <summary>
		/// Email uživatele.
		/// </summary>
		public virtual string Email
		{
			get
			{
				EnsureLoaded();
				return _EmailPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				if (value == null)
				{
					_EmailPropertyHolder.Value = String.Empty;
				}
				else
				{
					_EmailPropertyHolder.Value = value;
				}
			}
		}
		protected PropertyHolder<string> _EmailPropertyHolder;
		
		/// <summary>
		/// Indikuje, zda-li je uživatelský účet zablokován (nelze se přihlásit).
		/// </summary>
		public virtual bool Disabled
		{
			get
			{
				EnsureLoaded();
				return _DisabledPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_DisabledPropertyHolder.Value = value;
			}
		}
		protected PropertyHolder<bool> _DisabledPropertyHolder;
		
		/// <summary>
		/// Okamžik, kdy byl uživatelský účet uzamčen pro opakovaně neúspěšné pokusy o přihlášení.
		/// Indikuje uzamčený uživatelského účtu (NOT NULL).
		/// </summary>
		public virtual DateTime? LockedTime
		{
			get
			{
				EnsureLoaded();
				return _LockedTimePropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_LockedTimePropertyHolder.Value = value;
			}
		}
		protected PropertyHolder<DateTime?> _LockedTimePropertyHolder;
		
		/// <summary>
		/// Okamžik posledního úspěšného přihlášení uživatele.
		/// </summary>
		public virtual DateTime? LoginLast
		{
			get
			{
				EnsureLoaded();
				return _LoginLastPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_LoginLastPropertyHolder.Value = value;
			}
		}
		protected PropertyHolder<DateTime?> _LoginLastPropertyHolder;
		
		/// <summary>
		/// Počet úspěšných přihlášení uživatele od jeho založení.
		/// </summary>
		public virtual int LoginCount
		{
			get
			{
				EnsureLoaded();
				return _LoginCountPropertyHolder.Value;
			}
			set
			{
				EnsureLoaded();
				_LoginCountPropertyHolder.Value = value;
			}
		}
		protected PropertyHolder<int> _LoginCountPropertyHolder;
		
		/// <summary>
		/// Okamžik založení objektu v DB.
		/// </summary>
		public virtual DateTime Created
		{
			get
			{
				EnsureLoaded();
				return _CreatedPropertyHolder.Value;
			}
		}
		protected PropertyHolder<DateTime> _CreatedPropertyHolder;
		
		/// <summary>
		/// Indikuje smazaného uživatele.
		/// </summary>
		public virtual bool Deleted
		{
			get
			{
				EnsureLoaded();
				return _DeletedPropertyHolder.Value;
			}
			protected set
			{
				EnsureLoaded();
				_DeletedPropertyHolder.Value = value;
			}
		}
		protected PropertyHolder<bool> _DeletedPropertyHolder;
		
		public virtual Havit.BusinessLayerTest.RoleCollection Role
		{
			get
			{
				EnsureLoaded();
				return _RolePropertyHolder.Value;
			}
		}
		protected CollectionPropertyHolder<Havit.BusinessLayerTest.RoleCollection, Havit.BusinessLayerTest.Role> _RolePropertyHolder;
		
		#endregion
		
		#region Init
		/// <summary>
		/// Inicializuje třídu (vytvoří instance PropertyHolderů).
		/// </summary>
		protected override void Init()
		{
			_UsernamePropertyHolder = new PropertyHolder<string>(this);
			_PasswordPropertyHolder = new PropertyHolder<string>(this);
			_DisplayAsPropertyHolder = new PropertyHolder<string>(this);
			_EmailPropertyHolder = new PropertyHolder<string>(this);
			_DisabledPropertyHolder = new PropertyHolder<bool>(this);
			_LockedTimePropertyHolder = new PropertyHolder<DateTime?>(this);
			_LoginLastPropertyHolder = new PropertyHolder<DateTime?>(this);
			_LoginCountPropertyHolder = new PropertyHolder<int>(this);
			_CreatedPropertyHolder = new PropertyHolder<DateTime>(this);
			_DeletedPropertyHolder = new PropertyHolder<bool>(this);
			_RolePropertyHolder = new CollectionPropertyHolder<Havit.BusinessLayerTest.RoleCollection, Havit.BusinessLayerTest.Role>(this);
			
			if (IsNew)
			{
				_UsernamePropertyHolder.Value = String.Empty;
				_PasswordPropertyHolder.Value = String.Empty;
				_DisplayAsPropertyHolder.Value = String.Empty;
				_EmailPropertyHolder.Value = String.Empty;
				_DisabledPropertyHolder.Value = false;
				_LockedTimePropertyHolder.Value = null;
				_LoginLastPropertyHolder.Value = null;
				_LoginCountPropertyHolder.Value = 0;
				_DeletedPropertyHolder.Value = false;
				_RolePropertyHolder.Initialize();
			}
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
			
			if (_UsernamePropertyHolder.IsDirty && (_UsernamePropertyHolder.Value != null) && (_UsernamePropertyHolder.Value.Length > 50))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"Username\" přesáhl maximální délku 50 znaků.");
			}
			
			if (_PasswordPropertyHolder.IsDirty && (_PasswordPropertyHolder.Value != null) && (_PasswordPropertyHolder.Value.Length > 30))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"Password\" přesáhl maximální délku 30 znaků.");
			}
			
			if (_DisplayAsPropertyHolder.IsDirty && (_DisplayAsPropertyHolder.Value != null) && (_DisplayAsPropertyHolder.Value.Length > 50))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"DisplayAs\" přesáhl maximální délku 50 znaků.");
			}
			
			if (_EmailPropertyHolder.IsDirty && (_EmailPropertyHolder.Value != null) && (_EmailPropertyHolder.Value.Length > 100))
			{
				throw new ConstraintViolationException(this, "Řetězec v \"Email\" přesáhl maximální délku 100 znaků.");
			}
			
			if (_LockedTimePropertyHolder.IsDirty)
			{
				if (_LockedTimePropertyHolder.Value != null)
				{
					if ((_LockedTimePropertyHolder.Value.Value < SqlDateTime.MinValue.Value) || (_LockedTimePropertyHolder.Value.Value > SqlDateTime.MaxValue.Value))
					{
						throw new ConstraintViolationException(this, "Vlastnost \"LockedTime\" nesmí nabývat hodnoty mimo rozsah SqlDateTime.MinValue-SqlDateTime.MaxValue.");
					}
				}
			}
			
			if (_LoginLastPropertyHolder.IsDirty)
			{
				if (_LoginLastPropertyHolder.Value != null)
				{
					if ((_LoginLastPropertyHolder.Value.Value < SqlDateTime.MinValue.Value) || (_LoginLastPropertyHolder.Value.Value > SqlDateTime.MaxValue.Value))
					{
						throw new ConstraintViolationException(this, "Vlastnost \"LoginLast\" nesmí nabývat hodnoty mimo rozsah SqlDateTime.MinValue-SqlDateTime.MaxValue.");
					}
				}
			}
			
		}
		#endregion
		
		#region Load: Load_GetDataRecord, Load_ParseDataRecord
		/// <summary>
		/// Načte data objektu z DB a vrátí je ve formě DataRecordu.
		/// </summary>
		/// <param name="transaction">případná transakce</param>
		/// <returns>úplná data objektu</returns>
		protected override DataRecord Load_GetDataRecord(DbTransaction transaction)
		{
			DataRecord result;
			
			SqlCommand sqlCommand = new SqlCommand("SELECT UzivatelID, Username, Password, DisplayAs, Email, Disabled, LockedTime, LoginLast, LoginCount, Created, Deleted, (SELECT dbo.IntArrayAggregate(_items.RoleID) FROM dbo.Uzivatel_Role AS _items WHERE (_items.UzivatelID = @UzivatelID)) AS Role FROM dbo.Uzivatel WHERE UzivatelID = @UzivatelID");
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterUzivatelID = new SqlParameter("@UzivatelID", SqlDbType.Int);
			sqlParameterUzivatelID.Direction = ParameterDirection.Input;
			sqlParameterUzivatelID.Value = this.ID;
			sqlCommand.Parameters.Add(sqlParameterUzivatelID);
			
			result = DbConnector.Default.ExecuteDataRecord(sqlCommand);
			
			return result;
		}
		
		/// <summary>
		/// Vytahá data objektu z DataRecordu.
		/// </summary>
		/// <param name="record">DataRecord s daty objektu</param>
		protected override void Load_ParseDataRecord(DataRecord record)
		{
			this.ID = record.Get<int>("UzivatelID");
			
			string _tempUsername;
			if (record.TryGet<string>("Username", out _tempUsername))
			{
				_UsernamePropertyHolder.Value = _tempUsername ?? "";
			}
			
			string _tempPassword;
			if (record.TryGet<string>("Password", out _tempPassword))
			{
				_PasswordPropertyHolder.Value = _tempPassword ?? "";
			}
			
			string _tempDisplayAs;
			if (record.TryGet<string>("DisplayAs", out _tempDisplayAs))
			{
				_DisplayAsPropertyHolder.Value = _tempDisplayAs ?? "";
			}
			
			string _tempEmail;
			if (record.TryGet<string>("Email", out _tempEmail))
			{
				_EmailPropertyHolder.Value = _tempEmail ?? "";
			}
			
			bool _tempDisabled;
			if (record.TryGet<bool>("Disabled", out _tempDisabled))
			{
				_DisabledPropertyHolder.Value = _tempDisabled;
			}
			
			DateTime? _tempLockedTime;
			if (record.TryGet<DateTime?>("LockedTime", out _tempLockedTime))
			{
				_LockedTimePropertyHolder.Value = _tempLockedTime;
			}
			
			DateTime? _tempLoginLast;
			if (record.TryGet<DateTime?>("LoginLast", out _tempLoginLast))
			{
				_LoginLastPropertyHolder.Value = _tempLoginLast;
			}
			
			int _tempLoginCount;
			if (record.TryGet<int>("LoginCount", out _tempLoginCount))
			{
				_LoginCountPropertyHolder.Value = _tempLoginCount;
			}
			
			DateTime _tempCreated;
			if (record.TryGet<DateTime>("Created", out _tempCreated))
			{
				_CreatedPropertyHolder.Value = _tempCreated;
			}
			
			bool _tempDeleted;
			if (record.TryGet<bool>("Deleted", out _tempDeleted))
			{
				_DeletedPropertyHolder.Value = _tempDeleted;
			}
			
			SqlInt32Array _tempRole;
			if (record.TryGet<SqlInt32Array>("Role", out _tempRole))
			{
				_RolePropertyHolder.Initialize();
				if ((_tempRole != null) && (!_tempRole.IsNull))
				{
					for (int i = 0; i < _tempRole.Count; i++)
					{
						if (!_tempRole[i].IsNull)
						{
							_RolePropertyHolder.Value.Add(Havit.BusinessLayerTest.Role.GetObject(_tempRole[i].Value));
						}
					}
				}
			}
			
		}
		#endregion
		
		#region Save & Delete: Save_SaveMembers, Save_SaveCollections, Save_MinimalInsert, Save_FullInsert, Save_Update, Save_Insert_InsertRequiredForMinimalInsert, Save_Insert_InsertRequiredForFullInsert, Delete_Perform
		
		// Save_SaveMembers: Není co ukládat
		
		/// <summary>
		/// Ukládá member-kolekce objektu.
		/// </summary>
		protected override void Save_SaveCollections(DbTransaction transaction)
		{
			base.Save_SaveCollections(transaction);
			
			if (_RolePropertyHolder.IsInitialized)
			{
				_RolePropertyHolder.Value.SaveAll(transaction);
			}
			
		}
		
		/// <summary>
		/// Implementace metody vloží jen not-null vlastnosti objektu do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		public override void Save_MinimalInsert(DbTransaction transaction)
		{
			base.Save_MinimalInsert(transaction);
			Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterUsername = new SqlParameter("@Username", SqlDbType.VarChar, 50);
			sqlParameterUsername.Direction = ParameterDirection.Input;
			sqlParameterUsername.Value = _UsernamePropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterUsername);
			_UsernamePropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 30);
			sqlParameterPassword.Direction = ParameterDirection.Input;
			sqlParameterPassword.Value = _PasswordPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterPassword);
			_PasswordPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDisplayAs = new SqlParameter("@DisplayAs", SqlDbType.NVarChar, 50);
			sqlParameterDisplayAs.Direction = ParameterDirection.Input;
			sqlParameterDisplayAs.Value = _DisplayAsPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterDisplayAs);
			_DisplayAsPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
			sqlParameterEmail.Direction = ParameterDirection.Input;
			sqlParameterEmail.Value = _EmailPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterEmail);
			_EmailPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDisabled = new SqlParameter("@Disabled", SqlDbType.Bit);
			sqlParameterDisabled.Direction = ParameterDirection.Input;
			sqlParameterDisabled.Value = _DisabledPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDisabled);
			_DisabledPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLockedTime = new SqlParameter("@LockedTime", SqlDbType.SmallDateTime);
			sqlParameterLockedTime.Direction = ParameterDirection.Input;
			sqlParameterLockedTime.Value = (_LockedTimePropertyHolder.Value == null) ? DBNull.Value : (object)_LockedTimePropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLockedTime);
			_LockedTimePropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLoginLast = new SqlParameter("@LoginLast", SqlDbType.SmallDateTime);
			sqlParameterLoginLast.Direction = ParameterDirection.Input;
			sqlParameterLoginLast.Value = (_LoginLastPropertyHolder.Value == null) ? DBNull.Value : (object)_LoginLastPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLoginLast);
			_LoginLastPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLoginCount = new SqlParameter("@LoginCount", SqlDbType.Int);
			sqlParameterLoginCount.Direction = ParameterDirection.Input;
			sqlParameterLoginCount.Value = _LoginCountPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLoginCount);
			_LoginCountPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.Bit);
			sqlParameterDeleted.Direction = ParameterDirection.Input;
			sqlParameterDeleted.Value = _DeletedPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			sqlCommand.CommandText = "DECLARE @UzivatelID INT; INSERT INTO dbo.Uzivatel (Username, Password, DisplayAs, Email, Disabled, LockedTime, LoginLast, LoginCount, Deleted) VALUES (@Username, @Password, @DisplayAs, @Email, @Disabled, @LockedTime, @LoginLast, @LoginCount, @Deleted); SELECT @UzivatelID = SCOPE_IDENTITY(); SELECT @UzivatelID; ";
			
			this.ID = (int)DbConnector.Default.ExecuteScalar(sqlCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(this);
			}
		}
		
		/// <summary>
		/// Implementace metody vloží nový objekt do databáze a nastaví nově přidělené ID (primární klíč).
		/// </summary>
		protected override void Save_FullInsert(DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			SqlParameter sqlParameterUsername = new SqlParameter("@Username", SqlDbType.VarChar, 50);
			sqlParameterUsername.Direction = ParameterDirection.Input;
			sqlParameterUsername.Value = _UsernamePropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterUsername);
			_UsernamePropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 30);
			sqlParameterPassword.Direction = ParameterDirection.Input;
			sqlParameterPassword.Value = _PasswordPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterPassword);
			_PasswordPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDisplayAs = new SqlParameter("@DisplayAs", SqlDbType.NVarChar, 50);
			sqlParameterDisplayAs.Direction = ParameterDirection.Input;
			sqlParameterDisplayAs.Value = _DisplayAsPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterDisplayAs);
			_DisplayAsPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
			sqlParameterEmail.Direction = ParameterDirection.Input;
			sqlParameterEmail.Value = _EmailPropertyHolder.Value ?? String.Empty;
			sqlCommand.Parameters.Add(sqlParameterEmail);
			_EmailPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDisabled = new SqlParameter("@Disabled", SqlDbType.Bit);
			sqlParameterDisabled.Direction = ParameterDirection.Input;
			sqlParameterDisabled.Value = _DisabledPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDisabled);
			_DisabledPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLockedTime = new SqlParameter("@LockedTime", SqlDbType.SmallDateTime);
			sqlParameterLockedTime.Direction = ParameterDirection.Input;
			sqlParameterLockedTime.Value = (_LockedTimePropertyHolder.Value == null) ? DBNull.Value : (object)_LockedTimePropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLockedTime);
			_LockedTimePropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLoginLast = new SqlParameter("@LoginLast", SqlDbType.SmallDateTime);
			sqlParameterLoginLast.Direction = ParameterDirection.Input;
			sqlParameterLoginLast.Value = (_LoginLastPropertyHolder.Value == null) ? DBNull.Value : (object)_LoginLastPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLoginLast);
			_LoginLastPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterLoginCount = new SqlParameter("@LoginCount", SqlDbType.Int);
			sqlParameterLoginCount.Direction = ParameterDirection.Input;
			sqlParameterLoginCount.Value = _LoginCountPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterLoginCount);
			_LoginCountPropertyHolder.IsDirty = false;
			
			SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.Bit);
			sqlParameterDeleted.Direction = ParameterDirection.Input;
			sqlParameterDeleted.Value = _DeletedPropertyHolder.Value;
			sqlCommand.Parameters.Add(sqlParameterDeleted);
			_DeletedPropertyHolder.IsDirty = false;
			
			StringBuilder collectionCommandBuilder = new StringBuilder();
			
			if (_RolePropertyHolder.Value.Count > 0)
			{
				SqlParameter sqlParameterRole = new SqlParameter("@Role", SqlDbType.Udt);
				sqlParameterRole.UdtTypeName = "IntArray";
				sqlParameterRole.Value = new SqlInt32Array(this._RolePropertyHolder.Value.GetIDs());
				sqlCommand.Parameters.Add(sqlParameterRole);
				
				// OPTION (RECOMPILE): workaround pro http://connect.microsoft.com/SQLServer/feedback/ViewFeedback.aspx?FeedbackID=256717
				collectionCommandBuilder.Append("INSERT INTO dbo.Uzivatel_Role (UzivatelID, RoleID) SELECT @UzivatelID AS UzivatelID, Value AS RoleID FROM dbo.IntArrayToTable(@Role) OPTION (RECOMPILE); ");
			}
			
			sqlCommand.CommandText = "DECLARE @UzivatelID INT; INSERT INTO dbo.Uzivatel (Username, Password, DisplayAs, Email, Disabled, LockedTime, LoginLast, LoginCount, Deleted) VALUES (@Username, @Password, @DisplayAs, @Email, @Disabled, @LockedTime, @LoginLast, @LoginCount, @Deleted); SELECT @UzivatelID = SCOPE_IDENTITY(); " + collectionCommandBuilder.ToString() + "SELECT @UzivatelID; ";
			
			this.ID = (int)DbConnector.Default.ExecuteScalar(sqlCommand);
			this.IsNew = false; // uložený objekt není už nový, dostal i přidělené ID
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(this);
			}
		}
		
		/// <summary>
		/// Implementace metody aktualizuje data objektu v databázi.
		/// </summary>
		protected override void Save_Update(DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append("UPDATE dbo.Uzivatel SET ");
			
			bool dirtyFieldExists = false;
			if (_UsernamePropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Username = @Username");
				
				SqlParameter sqlParameterUsername = new SqlParameter("@Username", SqlDbType.VarChar, 50);
				sqlParameterUsername.Direction = ParameterDirection.Input;
				sqlParameterUsername.Value = _UsernamePropertyHolder.Value ?? String.Empty;
				sqlCommand.Parameters.Add(sqlParameterUsername);
				
				dirtyFieldExists = true;
			}
			
			if (_PasswordPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Password = @Password");
				
				SqlParameter sqlParameterPassword = new SqlParameter("@Password", SqlDbType.NVarChar, 30);
				sqlParameterPassword.Direction = ParameterDirection.Input;
				sqlParameterPassword.Value = _PasswordPropertyHolder.Value ?? String.Empty;
				sqlCommand.Parameters.Add(sqlParameterPassword);
				
				dirtyFieldExists = true;
			}
			
			if (_DisplayAsPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("DisplayAs = @DisplayAs");
				
				SqlParameter sqlParameterDisplayAs = new SqlParameter("@DisplayAs", SqlDbType.NVarChar, 50);
				sqlParameterDisplayAs.Direction = ParameterDirection.Input;
				sqlParameterDisplayAs.Value = _DisplayAsPropertyHolder.Value ?? String.Empty;
				sqlCommand.Parameters.Add(sqlParameterDisplayAs);
				
				dirtyFieldExists = true;
			}
			
			if (_EmailPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Email = @Email");
				
				SqlParameter sqlParameterEmail = new SqlParameter("@Email", SqlDbType.NVarChar, 100);
				sqlParameterEmail.Direction = ParameterDirection.Input;
				sqlParameterEmail.Value = _EmailPropertyHolder.Value ?? String.Empty;
				sqlCommand.Parameters.Add(sqlParameterEmail);
				
				dirtyFieldExists = true;
			}
			
			if (_DisabledPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Disabled = @Disabled");
				
				SqlParameter sqlParameterDisabled = new SqlParameter("@Disabled", SqlDbType.Bit);
				sqlParameterDisabled.Direction = ParameterDirection.Input;
				sqlParameterDisabled.Value = _DisabledPropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterDisabled);
				
				dirtyFieldExists = true;
			}
			
			if (_LockedTimePropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("LockedTime = @LockedTime");
				
				SqlParameter sqlParameterLockedTime = new SqlParameter("@LockedTime", SqlDbType.SmallDateTime);
				sqlParameterLockedTime.Direction = ParameterDirection.Input;
				sqlParameterLockedTime.Value = (_LockedTimePropertyHolder.Value == null) ? DBNull.Value : (object)_LockedTimePropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterLockedTime);
				
				dirtyFieldExists = true;
			}
			
			if (_LoginLastPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("LoginLast = @LoginLast");
				
				SqlParameter sqlParameterLoginLast = new SqlParameter("@LoginLast", SqlDbType.SmallDateTime);
				sqlParameterLoginLast.Direction = ParameterDirection.Input;
				sqlParameterLoginLast.Value = (_LoginLastPropertyHolder.Value == null) ? DBNull.Value : (object)_LoginLastPropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterLoginLast);
				
				dirtyFieldExists = true;
			}
			
			if (_LoginCountPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("LoginCount = @LoginCount");
				
				SqlParameter sqlParameterLoginCount = new SqlParameter("@LoginCount", SqlDbType.Int);
				sqlParameterLoginCount.Direction = ParameterDirection.Input;
				sqlParameterLoginCount.Value = _LoginCountPropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterLoginCount);
				
				dirtyFieldExists = true;
			}
			
			if (_DeletedPropertyHolder.IsDirty)
			{
				if (dirtyFieldExists)
				{
					commandBuilder.Append(", ");
				}
				commandBuilder.Append("Deleted = @Deleted");
				
				SqlParameter sqlParameterDeleted = new SqlParameter("@Deleted", SqlDbType.Bit);
				sqlParameterDeleted.Direction = ParameterDirection.Input;
				sqlParameterDeleted.Value = _DeletedPropertyHolder.Value;
				sqlCommand.Parameters.Add(sqlParameterDeleted);
				
				dirtyFieldExists = true;
			}
			
			if (dirtyFieldExists)
			{
				// objekt je sice IsDirty (volá se tato metoda), ale může být změněná jen kolekce
				commandBuilder.Append(" WHERE UzivatelID = @UzivatelID; ");
			}
			else
			{
				commandBuilder = new StringBuilder();
			}
			
			bool dirtyCollectionExists = false;
			if (_RolePropertyHolder.IsDirty)
			{
				dirtyCollectionExists = true;
				commandBuilder.AppendFormat("DELETE FROM dbo.Uzivatel_Role WHERE UzivatelID = @UzivatelID; ");
				if (_RolePropertyHolder.Value.Count > 0)
				{
					// OPTION (RECOMPILE): workaround pro http://connect.microsoft.com/SQLServer/feedback/ViewFeedback.aspx?FeedbackID=256717
					commandBuilder.AppendFormat("INSERT INTO dbo.Uzivatel_Role (UzivatelID, RoleID) SELECT @UzivatelID AS UzivatelID, Value AS RoleID FROM dbo.IntArrayToTable(@Role) OPTION (RECOMPILE); ");
					SqlParameter sqlParameterRole = new SqlParameter("@Role", SqlDbType.Udt);
					sqlParameterRole.UdtTypeName = "IntArray";
					sqlParameterRole.Value = new SqlInt32Array(this._RolePropertyHolder.Value.GetIDs());
					sqlCommand.Parameters.Add(sqlParameterRole);
				}
			}
			
			// pokud je objekt dirty, ale žádná property není dirty (Save_MinimalInsert poukládal všechno), neukládáme
			if (dirtyFieldExists || dirtyCollectionExists)
			{
				SqlParameter sqlParameterUzivatelID = new SqlParameter("@UzivatelID", SqlDbType.Int);
				sqlParameterUzivatelID.Direction = ParameterDirection.Input;
				sqlParameterUzivatelID.Value = this.ID;
				sqlCommand.Parameters.Add(sqlParameterUzivatelID);
				sqlCommand.CommandText = commandBuilder.ToString();
				DbConnector.Default.ExecuteNonQuery(sqlCommand);
			}
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení minimálního insertu. Volá Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		protected override void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForMinimalInsert(transaction);
			
		}
		
		/// <summary>
		/// Ukládá hodnoty potřebné pro provedení plného insertu.
		/// </summary>
		protected override void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			base.Save_Insert_InsertRequiredForFullInsert(transaction);
			
			foreach (Havit.BusinessLayerTest.Role roleBase in Role)
			{
				if (roleBase.IsNew)
				{
					roleBase.Save_MinimalInsert(transaction);
				}
			}
			
		}
		
		/// <summary>
		/// Metoda označí objekt jako smazaný a uloží jej.
		/// </summary>
		protected override void Delete_Perform(DbTransaction transaction)
		{
			if (IsNew)
			{
				throw new InvalidOperationException("Nelze smazat nový objekt.");
			}
			
			Deleted = true;
			Save(transaction);
		}
		
		#endregion
		
		#region GetFirst, GetList
		/// <summary>
		/// Vrátí první nalezený objekt typu Uzivatel dle parametrů v queryParams.
		/// Pokud není žádný objekt nalezen, vrací null.
		/// </summary>
		public static Uzivatel GetFirst(QueryParams queryParams)
		{
			queryParams.TopRecords = 1;
			UzivatelCollection getListResult = Uzivatel.GetList(queryParams);
			return (getListResult.Count == 0) ? null : getListResult[0];
		}
		
		/// <summary>
		/// Vrátí objekty typu Uzivatel dle parametrů v queryParams.
		/// </summary>
		public static UzivatelCollection GetList(QueryParams queryParams)
		{
			return Uzivatel.GetList(queryParams, null);
		}
		
		public static UzivatelCollection GetList(QueryParams queryParams, DbTransaction transaction)
		{
			SqlCommand sqlCommand = new SqlCommand();
			sqlCommand.Transaction = (SqlTransaction)transaction;
			
			queryParams.ObjectInfo = Uzivatel.ObjectInfo;
			if (queryParams.Properties.Count > 0)
			{
				queryParams.Properties.Add(Uzivatel.Properties.ID);
			}
			
			queryParams.PrepareCommand(sqlCommand);
			return Uzivatel.GetList(sqlCommand, queryParams.GetDataLoadPower());
		}
		
		private static UzivatelCollection GetList(DbCommand dbCommand, DataLoadPower dataLoadPower)
		{
			if (dbCommand == null)
			{
				throw new ArgumentNullException("dbCommand");
			}
			
			UzivatelCollection result = new UzivatelCollection();
			
			using (DbDataReader reader = DbConnector.Default.ExecuteReader(dbCommand))
			{
				while (reader.Read())
				{
					DataRecord dataRecord = new DataRecord(reader, dataLoadPower);
					result.Add(Uzivatel.GetObject(dataRecord));
				}
			}
			
			return result;
		}
		
		public static UzivatelCollection GetAll()
		{
			return Uzivatel.GetAll(false);
		}
		
		public static UzivatelCollection GetAll(bool includeDeleted)
		{
			UzivatelCollection collection = null;
			QueryParams queryParams = new QueryParams();
			queryParams.IncludeDeleted = includeDeleted;
			collection = Uzivatel.GetList(queryParams);
			return collection;
		}
		
		#endregion
		
		#region ObjectInfo
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
		/// Objektová reprezentace vlastností třídy Uzivatel.
		/// </summary>
		public static UzivatelProperties Properties
		{
			get
			{
				return properties;
			}
		}
		private static UzivatelProperties properties;
		#endregion
		
	}
}
