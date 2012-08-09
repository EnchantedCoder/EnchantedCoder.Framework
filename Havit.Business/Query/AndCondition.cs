using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Kompozitn� podm�nka. V�sledek je pravdiv�, jsou-li pravdiv� v�echny �lensk� podm�nky.
	/// </summary>
	[Serializable]
	public class AndCondition: CompositeCondition
	{
		#region Constructor
		/// <summary>
		/// Vytvo�� kompozitn� podm�nku. Lze inicializovat sadou �lensk�ch podm�nek.
		/// </summary>		
		public AndCondition(params ICondition[] conditions)
			: base("AND", conditions)
		{
		}
		#endregion
	}
}
