using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Havit.Business.Query
{
	/// <summary>
	/// Kompozitn� podm�nka. V�sledek je pravdiv�, jsou-li pravdiv� v�echny �lensk� podm�nky.
	/// </summary>
	[ComVisible(false)]
	[Serializable]
	public class AndCondition : CompositeCondition
	{
		#region Constructors
		/// <summary>
		/// Vytvo�� kompozitn� podm�nku. Lze inicializovat sadou �lensk�ch podm�nek.
		/// </summary>		
		public AndCondition(params Condition[] conditions)
			: base("AND", conditions)
		{
		}
		#endregion

        #region Create (static)
        /// <summary>
        /// Vytvo�� kompozitn� podm�nku. Lze inicializovat sadou �lensk�ch podm�nek.
        /// </summary>
        public static AndCondition Create(params Condition[] conditions)
        {
            return new AndCondition(conditions);
        } 
        #endregion
	}
}
