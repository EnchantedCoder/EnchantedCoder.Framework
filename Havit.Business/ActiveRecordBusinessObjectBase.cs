using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Havit.Data;
using Havit.Data.SqlClient;
using System.Data.SqlClient;

namespace Havit.Business
{
	/// <summary>
	/// B�zov� t��da pro v�echny business-objekty, kter� definuje jejich z�kladn� chov�n� (Layer Supertype),
	/// zejm�na ve vztahu k datab�zi jako Active Record [Fowler].
	/// </summary>
	/// <remarks>
	/// T��da je z�kladem pro v�echny business-objekty a implementuje z�kladn� pattern pro komunikaci s datab�z�.
	/// Na��t�n� z datab�ze je implementov�no jako Lazy Load, kdy je objekt nejprve vytvo�en pr�zdn� jako Ghost se sv�m ID a teprve
	/// p�i prvn� pot�eb� je iniciov�no jeho �pln� na�ten� z DB.<br/>
	/// Prost�ednictv�m constructoru BusinessObjectBase(DataRecord record) lze vytvo�it i ne�pln� na�tenou instanci objektu,
	/// samotn� funk�nost v�ak nen� �e�ena a ka�d� si mus� s�m ohl�dat, aby bylo na�teno v�e, co je pot�eba.
	/// </remarks>
	[Serializable]
	public abstract class ActiveRecordBusinessObjectBase : BusinessObjectBase
	{
		/*
		#region Properties - Stav objektu		
		/// <summary>
		/// Indikuje, zda-li byla data objektu na�tena z datab�ze ��ste�n�, tedy zda-li se jednalo o partial-load.
		/// </summary>
		public bool IsLoadedPartially
		{
			get { return _isLoadedPartially; }
			set { _isLoadedPartially = value; }
		}
		private bool _isLoadedPartially;
		#endregion
		*/
		#region Constructors
		/// <summary>
		/// Konstruktor pro nov� objekt (bez perzistence v datab�zi).
		/// </summary>
		protected ActiveRecordBusinessObjectBase()
			: base()
		{
		}

		/// <summary>
		/// Konstruktor pro objekt s obrazem v datab�zi (perzistentn�).
		/// </summary>
		/// <param name="id">prim�rn� kl�� objektu</param>
		protected ActiveRecordBusinessObjectBase(int id)
			: base(id)
		{
			if (IdentityMapScope.Current != null)
			{
				IdentityMapScope.Current.Store(this);
			}
		}

		/// <summary>
		/// Konstruktor pro objekt s obrazen v datab�zi, kter�m dojde rovnou k na�ten� dat z <see cref="Havit.Data.DataRecord"/>.
		/// Z�kladn� cesta vytvo�en� partially-loaded instance.
		/// Pokud se inicializuje Ghost nebo FullLoad objekt, je p�id�n do IdentityMapy, pokud existuje.
		/// </summary>
		/// <param name="id">ID na��tan�ho objektu</param>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu na�ten�mi z datab�ze</param>
		protected ActiveRecordBusinessObjectBase(int id, DataRecord record)
			: base(
			id,	// ID
			false,	// IsNew
			false,	// IsDirty
			false)	// IsLoaded

		{
			if (record == null)
			{
				throw new ArgumentNullException("record");
			}

/* nahradil implementa�n� constructor base(...)
			this.IsNew = false;
			this.IsLoaded = false;
*/
			if ((IdentityMapScope.Current != null)
				&& ((record.DataLoadPower == DataLoadPower.Ghost) || (record.DataLoadPower == DataLoadPower.FullLoad)))
			{
				IdentityMapScope.Current.Store(this);
			}

			Load(record);

			//this.Load_ParseDataRecord(record);

//			this._isLoadedPartially = !record.FullLoad;
			//this.IsLoaded = true;
			//this.IsDirty = false;
		}
		#endregion

		#region Load logika

		/// <summary>
		/// Nastav� objektu hodnoty z DataRecordu.
		/// Pokud je objekt ji� na�ten, vyhod� v�jimku.
		/// </summary>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu na�ten�mi z datab�ze.</param>
		public void Load(DataRecord record)
		{
			if (this.IsLoaded)
			{
				throw new InvalidOperationException("Nelze nastavit objektu hodnoty z DataRecordu, pokud objekt nen� ghostem.");
			}
			Load_ParseDataRecord(record);

			if (record.DataLoadPower != DataLoadPower.Ghost)
			{
				this.IsLoaded = true;
			}
			this.IsDirty = false;
		}

		/// <summary>
		/// V�konn� ��st nahr�n� objektu z perzistentn�ho ulo�i�t�.
		/// </summary>
		/// <remarks>
		/// Na�te objekt z datab�ze do <see cref="DataRecord"/> a parsuje z�skan� <see cref="DataRecord"/> do objektu.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt na�ten; null, pokud bez transakce</param>
		protected override void Load_Perform(DbTransaction transaction)
		{
			DataRecord record = Load_GetDataRecord(transaction);
			Load_ParseDataRecord(record);
		}
		/// <summary>
		/// Implementace metody na�te DataRecord objektu z datab�ze.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt na�ten; null, pokud bez transakce</param>
		/// <returns><see cref="Havit.Data.DataRecord"/> s daty objektu na�ten�mi z datab�ze; null, pokud nenalezeno</returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract DataRecord Load_GetDataRecord(DbTransaction transaction);

		/// <summary>
		/// Implemetace metody napln� hodnoty objektu z DataRecordu.
		/// </summary>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu na�ten�mi z datab�ze; null, pokud nenalezeno</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Load_ParseDataRecord(DataRecord record);
		#endregion

		#region Save logika
		/// <summary>
		/// Ulo�� objekt do datab�ze, s pou�it�m transakce. Nov� objekt je vlo�en INSERT, existuj�c� objekt je aktualizov�n UPDATE.
		/// </summary>
		/// <remarks>
		/// Metoda neprovede ulo�en� objektu, pokud nen� nahr�n (!IsLoaded), nen� toti� ani co ukl�dat,
		/// data nemohla b�t zm�n�na, kdy� nebyla ani jednou pou�ita.<br/>
		/// Metoda tak� neprovede ulo�en�, pokud objekt nebyl zm�n�n a sou�asn� nejde o nov� objekt (!IsDirty &amp;&amp; !IsNew)
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		public override void Save(DbTransaction transaction)
		{
			//if (IsLoadedPartially)
			//{
			//    throw new ApplicationException("Partially-loaded object cannot be saved.");
			//}

			// vynucen� transakce nad celou Save() operac� (BusinessObjectBase ji pouze o�ek�v�, ale nevynucuje).
			SqlDataAccess.ExecuteTransaction(delegate(SqlTransaction myTransaction)
				{
					Save_BaseInTransaction(myTransaction); // base.Save(myTransaction) hl�s� warning
				}, (SqlTransaction)transaction);
		}

		/// <summary>
		/// Vol�no z metody Save - �e�� warning p�i kompilaci p�i vol�n� base.Save(...) z anonymn� metody.
		/// </summary>
		private void Save_BaseInTransaction(SqlTransaction myTransaction)
		{
			base.Save(myTransaction);
		}

		/// <summary>
		/// V�konn� ��st ulo�en� objektu do perzistentn�ho ulo�i�t�.
		/// </summary>
		/// <remarks>
		/// Pokud je objekt nov�, vol� Save_Insert_SaveRequiredForFullInsert a Insert, jinak Update.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		protected override void Save_Perform(DbTransaction transaction)
		{
			if (IsNew)
			{
				Save_Insert_InsertRequiredForFullInsert(transaction);
			}

			if (IsNew)
			{
				Save_SaveMembers(transaction);
				Save_FullInsert(transaction);
				Save_SaveCollections(transaction);
			}
			else
			{
				Save_SaveMembers(transaction);
				Save_SaveCollections(transaction);
				if (IsDirty)
				{
					Save_Update(transaction);
				}
			}
		}

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

		/// <summary>
		/// Implementace metody vlo�� nov� objekt do datab�ze a nastav� nov� p�id�len� ID (prim�rn� kl��).
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Save_FullInsert(DbTransaction transaction);

		/// <summary>
		/// Implementace metody vlo�� jen not-null vlastnosti objektu do datab�ze a nastav� nov� p�id�len� ID (prim�rn� kl��).
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		public abstract void Save_MinimalInsert(DbTransaction transaction);

		/// <summary>
		/// Implementace metody aktualizuje data objektu v datab�zi.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Save_Update(DbTransaction transaction);

		/// <summary>
		/// Ukl�d� hodnoty pot�ebn� pro proveden� pln�ho insertu.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		protected virtual void Save_Insert_InsertRequiredForFullInsert(DbTransaction transaction)
		{
			Save_Insert_InsertRequiredForMinimalInsert(transaction);
			IsMinimalInserting = false;
		}
		
		/// <summary>
		/// Ukl�d� hodnoty pot�ebn� pro proveden� minim�ln�ho insertu. Vol� Save_Insert_SaveRequiredForMinimalInsert.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		protected virtual void Save_Insert_InsertRequiredForMinimalInsert(DbTransaction transaction)
		{
			if (IsMinimalInserting)
			{
				throw new InvalidOperationException("P�i ukl�d�n� objekt� se vyskytla ne�e�iteln� cyklick� z�vislost stylu 'Co vzniklo prvn�: zrno nebo klas?'");
			}

			IsMinimalInserting = true;
		}

		/// <summary>
		/// Identifikuje, zda prob�h� Save_Insert_InsertRequiredForMinimalInsert (nesm� se zacyklit).
		/// </summary>
		protected bool IsMinimalInserting
		{
			get { return isMinimalInserting; }
			set { isMinimalInserting = value; }
		}
		private bool isMinimalInserting = false;

		#endregion
	}
}
