using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Oper�tor pro porovn�n� hodnot.
	/// </summary>
	public enum ComparisonOperator
	{
		/// <summary>
		/// Rovnost.
		/// </summary>
		Equals, 
		
		/// <summary>
		/// Nerovnost.
		/// </summary>
		NotEquals,
		
		/// <summary>
		/// Men��.
		/// </summary>
		Lower, 

		/// <summary>
		/// Men�� nebo rovno.
		/// </summary>
		LowerOrEquals,
		
		/// <summary>
		/// V�t��.
		/// </summary>
		Greater,
		
		/// <summary>
		/// V�t�� nebo rovno.
		/// </summary>
		GreaterOrEquals
	}
}
