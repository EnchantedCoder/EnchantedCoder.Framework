using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Vytv��� podm�nky, kter� nic netestuj�.
	/// Nyn� je takov� podm�nka reprezentov�na hodnotou null.
	/// </summary>
	public static class EmptyCondition
	{
		#region Create
		/// <summary>
		/// Vytvo�� podm�nku reprezentuj�c� pr�zdnou podm�nku (nic nen� testov�no). Nyn� vrac� null.
		/// </summary>
		/// <returns></returns>
		public static Condition Create()
		{
			return null;
		} 
		#endregion
	}
}
