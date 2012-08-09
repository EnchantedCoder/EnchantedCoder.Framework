using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Havit.Business
{
	/// <summary>
	/// Argumenty ud�losti po ulo�en� objektu.
	/// </summary>
	public class AfterSaveEventArgs: DbTransactionEventArgs
	{
		#region WasNew
		/// <summary>
		/// Indikuje, zda byl objekt p�ed ulo�en�m nov�.
		/// </summary>
		public bool WasNew
		{
			get { return _wasNew; }
		}
		private bool _wasNew; 
		#endregion
	
		#region Constructors
		/// <summary>
		/// Konstruktor.
		/// </summary>
		public AfterSaveEventArgs(DbTransaction transaction, bool wasNew)
			: base(transaction)
		{
			_wasNew = wasNew;
		} 
		#endregion
	}
}
