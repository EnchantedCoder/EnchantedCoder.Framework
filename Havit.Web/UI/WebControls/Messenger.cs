using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Havit.Web.UI.WebControls
{
    public class Messenger
    {
        #region Messages (private)
        /// <summary>
        /// Zpr�vy k zobrazen�.
        /// </summary>
        public List<MessengerMessage> Messages
        {
            get
            {
                HttpContext currentContext = HttpContext.Current;
                if (currentContext == null)
                {
                    throw new InvalidOperationException("HttpContext.Current je null.");
                }

                List<MessengerMessage> result = (List<MessengerMessage>)currentContext.Session["Messenger_Messages"];
                if (result == null)
                {
                    result = new List<MessengerMessage>();
                    currentContext.Session["Messenger_Messages"] = result;
                }

                return result;
            }
        }
        #endregion

        #region AddMessage
        /// <summary>
        /// P�id� zpr�vu
        /// </summary>
        /// <param name="format">form�tovac� �et�zec pro String.Format()</param>
        /// <param name="args">argumenty pro String.Format()</param>
        public void AddMessage(MessengerMessage message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// P�id� zpr�vu
        /// </summary>
        /// <param name="messageType">typ zpr�vy (information, error, ...)</param>
        /// <param name="text">text zpr�vy</param>
        public void AddMessage(MessageType messageType, string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Parametr nesm� b�t null ani String.Empty", "text");
            }
            AddMessage(new MessengerMessage(text, messageType));
        }

        /// <summary>
        /// P�id� zpr�vu pomoc� String.Format();
        /// </summary>
        /// <param name="messageType">typ zpr�vy (information, error, ...)</param>
        /// <param name="format">form�tovac� �et�zec pro String.Format()</param>
        /// <param name="args">argumenty pro String.Format()</param>
        public void AddMessage(MessageType messageType, string format, params object[] args)
        {
            AddMessage(messageType, String.Format(format, args));
        }

        /// <summary>
        /// P�id� zpr�vu typu Information pomoc� String.Format();
        /// </summary>
        /// <param name="format">form�tovac� �et�zec pro String.Format()</param>
        /// <param name="args">argumenty pro String.Format()</param>
        public void AddMessage(string format, params object[] args)
        {
            AddMessage(MessageType.Information, String.Format(format, args));
        }

        /// <summary>
        /// Prost� p�id�n� zpr�vy typu Information.
        /// </summary>
        /// <param name="message">zpr�va</param>
        public void AddMessage(string message)
        {
            AddMessage(MessageType.Information, message);
        }
        #endregion

        #region AddGlobalResourceMessage
        /// <summary>
        /// P�id� zpr�vu z App_GlobalResources.
        /// </summary>
        /// <param name="classKey">n�zev global-resource souboru</param>
        /// <param name="resourceKey">kl�� resourcu</param>
        public void AddGlobalResourceMessage(MessageType messageType, string classKey, string resourceKey)
        {
            AddMessage(messageType, (string)HttpContext.GetGlobalResourceObject(classKey, resourceKey));
        }

        /// <summary>
        /// P�id� zpr�vu z App_GlobalResources typu Information.
        /// </summary>
        /// <param name="classKey">n�zev global-resource souboru</param>
        /// <param name="resourceKey">kl�� resourcu</param>
        public void AddGlobalResourceMessage(string classKey, string resourceKey)
        {
            AddGlobalResourceMessage(MessageType.Information, classKey, resourceKey);
        }
        #endregion

        #region Clear
        /// <summary>
        /// Vy�ist� kolekci zpr�v.
        /// </summary>
        public void Clear()
        {
            Messages.Clear();
        }
        #endregion

        #region Default
        /// <summary>
        /// V�choz� instance messengera.
        /// </summary>
        public static Messenger Default
        {
            get
            {
                return _default;
            }
        }
        private static Messenger _default = new Messenger(); 
        #endregion

    }
}
