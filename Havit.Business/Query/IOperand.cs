using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;

namespace Havit.Business.Query
{
	/// <summary>
	/// Interface pro operandy SQL dotazu.
	/// Operandem m��e b�t v�raz, datab�zov� sloupec, sklal�rn� hodnota...
	/// </summary>
	public interface IOperand
	{
		/// <summary>
		/// Vrac� �et�zec, kter� reprezentuje hodnotu operandu v SQL dotazu.
		/// M��e p�id�vat datab�zov� parametry do commandu.
		/// </summary>
		/// <param name="command">Datab�zov� p��kaz. Je mo�n� do n�j p�id�vat datab�zov� parametry.</param>
		/// <returns>�et�zec reprezentuj�c� hodnotu operandu v SQL dotazu.</returns>
		string GetCommandValue(DbCommand command);
	}
}
