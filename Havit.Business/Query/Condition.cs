using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Interface podm�nky dotazu.
	/// </summary>
	[Serializable]
	public abstract class Condition
	{
		#region GetWhereStatement
		/// <summary>
		/// P�id� ��st SQL p��kaz pro sekci WHERE. Je VELMI doporu�eno, aby byla podm�nka p�id�na v�etn� z�vorek.
		/// </summary>
		public abstract void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder); 
		#endregion

		#region IsEmptyCondition
		/// <summary>
		/// Ud�v�, zda podm�nka reprezentuje pr�zdnou podm�nku, kter� nebude renderov�na (nap�. pr�zdn� AndCondition).
		/// </summary>
		public abstract bool IsEmptyCondition(); 
		#endregion
	}
}
