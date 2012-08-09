using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Data.SqlTypes
{
    /// <summary>
    /// Reprezentuje pomocn� hodnoty pro datab�zov� typ smalldatetime.
    /// </summary>
    public class SqlSmallDateTime
    {
        /// <summary>
        /// Minim�ln� hodnota pou�iteln� pro datab�zov� typ smalldatetime.
        /// </summary>
        public static readonly SqlSmallDateTime MinValue = new SqlSmallDateTime(new DateTime(1900, 1, 1));

        /// <summary>
        /// Maxim�ln� hodnota pou�iteln� pro datab�zov� typ smalldatetime.
        /// </summary>
        public static readonly SqlSmallDateTime MaxValue = new SqlSmallDateTime(new DateTime(2079, 6, 6, 23, 59, 00));

        #region Constructor
        private SqlSmallDateTime(DateTime value)
        {
            _value = value;
        }
        #endregion

        #region Value
        /// <summary>
        /// Hodnota reprezentovan� jako DateTime.
        /// </summary>
        public DateTime Value
        {
            get
            {
                return _value;
            }
        }
        private DateTime _value;
        #endregion
    }
}
