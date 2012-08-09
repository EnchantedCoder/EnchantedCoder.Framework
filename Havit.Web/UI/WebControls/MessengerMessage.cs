using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Web.UI.WebControls
{
    /// <summary>
    /// Zpr�va do messengera.
    /// </summary>
    [Serializable]
    public class MessengerMessage
    {
        #region Text
        /// <summary>
        /// Text zpr�vy.
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        private string _text;
        #endregion

        #region MessageType
        /// <summary>
        /// Typ zpr�vy.
        /// </summary>
        public MessageType MessageType
        {
            get
            {
                return _messageType;
            }
            set
            {
                _messageType = value;
            }
        }
        private MessageType _messageType;
        #endregion

        #region Constructors
        /// <summary>
        /// Vytvo�� instanci zpr�vy a nastav� typ zpr�vy na Information.
        /// </summary>
        public MessengerMessage()
        {
            this._messageType = MessageType.Information;
            this._text = String.Empty;
        }

        /// <summary>
        /// Vytvo�� instanci zpr�vy.
        /// </summary>
        /// <param name="text">text zpr�vy</param>
        /// <param name="messageType">typ zpr�vy</param>
        public MessengerMessage(string text, MessageType messageType)
        {
            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Parametr nesm� b�t null ani String.Empty", "messageType");
            }
            this._messageType = messageType;
            this._text = text;
        }
        #endregion
    }

}
