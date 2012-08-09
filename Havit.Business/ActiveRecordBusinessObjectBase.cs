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
		}

		/// <summary>
		/// Konstruktor pro objekt s obrazen v datab�zi, kter�m dojde rovnou k na�ten� dat z <see cref="Havit.Data.DataRecord"/>.
		/// Z�kladn� cesta vytvo�en� partially-loaded instance.
		/// </summary>
		/// <param name="record"><see cref="Havit.Data.DataRecord"/> s daty objektu na�ten�mi z datab�ze</param>
		protected ActiveRecordBusinessObjectBase(DataRecord record)
			: base()
		{
			if (record == null)
			{
				throw new ArgumentNullException("record");
			}

			this.IsNew = false;
			this.IsLoaded = false;
		
			this.Load_ParseDataRecord(record);

//			this._isLoadedPartially = !record.FullLoad;
			this.IsLoaded = true;
			this.IsDirty = false;
		}
		#endregion

		#region Load logika
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
		/// Pokud je objekt nov�, vol� Insert, jinak Update.
		/// </remarks>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		protected override void Save_Perform(DbTransaction transaction)
		{
			if (this.IsNew)
			{
				Save_Insert(transaction);
			}
			else
			{
				Save_Update(transaction);
			}
		}

		/// <summary>
		/// Implementace metody vlo�� nov� objekt do datab�ze a nastav� nov� p�id�len� ID (prim�rn� kl��).
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Save_Insert(DbTransaction transaction);

		/// <summary>
		/// Implementace metody aktualizuje data objektu v datab�zi.
		/// </summary>
		/// <param name="transaction">transakce <see cref="DbTransaction"/>, v r�mci kter� m� b�t objekt ulo�en; null, pokud bez transakce</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", MessageId = "Member")]
		protected abstract void Save_Update(DbTransaction transaction);
		#endregion
	}
}
