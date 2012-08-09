using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Ur�uje zp�sob tvorby dotazu na v�ce hodnot (IN, NOT IN).
	/// </summary>
	public enum MatchListMode
	{
		/// <summary>
		/// Podm�nka je tvo�ena zp�sobem: WHERE Field IN IN (SELECT Value FROM dbo.IntArrayToTable(@parameter).
		/// </summary>
		IntArray,

		/// <summary>
		/// Podm�nka je tvo�ena zp�sobem: WHERE Field IN (1, 2, 3, 4).
		/// </summary>
		CommaSeparatedList
	}
}
