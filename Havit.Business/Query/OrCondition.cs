using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Business.Query
{
	/// <summary>
	/// Kompozitn� podm�nka. V�sledek je pravdiv�, je-li pravdiv� alespo� jedna �lensk� podm�nka.
	/// </summary>
	[Serializable]
	public class OrCondition: CompositeCondition
	{
        #region Constructors
        /// <summary>
        /// Vytvo�� kompozitn� podm�nku. Lze inicializovat sadou �lensk�ch podm�nek.
        /// </summary>		
        public OrCondition(params Condition[] conditions)
            : base("OR", conditions)
        {
        }
        #endregion

        #region Create (static)
        /// <summary>
        /// Vytvo�� kompozitn� podm�nku. Lze inicializovat sadou �lensk�ch podm�nek.
        /// </summary>
        public static OrCondition Create(params Condition[] conditions)
        {
            return new OrCondition(conditions);
        }
        #endregion

	}
}
