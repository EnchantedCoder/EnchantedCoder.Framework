using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Havit.Business
{
    /// <summary>
    /// Argument nesouc� instanci datab�zov� transakce.
    /// </summary>
    public class DbTransactionEventArgs: EventArgs
    {
        #region Transaction
        /// <summary>
        /// Transakce.
		/// Pro OnBeforeSave a OnAfterSave nem��e b�t v p��pad� ActiveRecordBusinessObjectBase null, v p��pad� hol�ho BusinessObjectBase ano.
        /// </summary>
        public DbTransaction Transaction
        {
            get { return _transaction; }
        }
        private DbTransaction _transaction; 
        #endregion

        #region Constructors
        public DbTransactionEventArgs(DbTransaction transaction)
        {
            this._transaction = transaction;
        } 
        #endregion
    }
}
