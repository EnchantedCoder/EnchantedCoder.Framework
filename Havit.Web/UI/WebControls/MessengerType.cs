using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
    #region MessageType (enum)
    /// <summary>
    /// Typ zpr�vy Messengeru.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Informace, potvrzen� operace.
        /// </summary>
        Information = 0,

        /// <summary>
        /// Varov�n�, nap�. upozorn�n� na dal�� nutn� kroky k dokon�en� operace.
        /// </summary>
        Warning = 1,

        /// <summary>
        /// Chyba.
        /// </summary>
        Error = 2
    }
    #endregion
}
