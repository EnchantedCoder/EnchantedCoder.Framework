using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Interface podm�nky dotazu.
	/// </summary>
	public abstract class Condition
	{
		/// <summary>
		/// P�id� ��st SQL p��kaz pro sekci WHERE. Je VELMI doporu�eno, aby byla podm�nka p�id�na v�etn� z�vorek.
		/// </summary>
		public abstract void GetWhereStatement(System.Data.Common.DbCommand command, StringBuilder whereBuilder);
	}
}
