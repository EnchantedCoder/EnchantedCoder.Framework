using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Havit.Data.SqlClient
{
	/// <summary>
	/// Reprezentuje metodu, kter� vykon�v� jednotliv� kroky transakce.
	/// </summary>
	/// <param name="transaction">transakce, v r�mci kter� maj� b�t jednotliv� kroky vykon�ny</param>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
	public delegate void SqlTransactionDelegate(SqlTransaction transaction);
}
