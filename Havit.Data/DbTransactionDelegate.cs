using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Havit.Data
{
	/// <summary>
	/// Reprezentuje metodu, kter� vykon�v� jednotliv� kroky transakce.
	/// </summary>
	/// <param name="transaction">transakce, v r�mci kter� maj� b�t jednotliv� kroky vykon�ny</param>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public delegate void DbTransactionDelegate(DbTransaction transaction);
}
