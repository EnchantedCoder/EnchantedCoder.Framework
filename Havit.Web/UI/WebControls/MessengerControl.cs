using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace Havit.Web.UI.WebControls
{
    /// <summary>
    /// Zobrazuje zpr�vy Messengera.
    /// </summary>
    public class MessengerControl: Literal
    {
        #region Messenger
        /// <summary>
        /// Messenger pou�it� pro zpr�vy k zobrazen�.
        /// Nen�-li nastaven, vrac� Messenger.Default.
        /// </summary>
        public Messenger Messenger
        {
            get
            {
                return _messenger == null ? Messenger.Default : _messenger;                
            }
            set
            {
                _messenger = value;
            }
        }
        private Messenger _messenger; 
        #endregion

        #region OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            this.Text = this.GetText();
            this.Messenger.Clear();
        }
        #endregion

        #region GetText, GetMessageText
        /// <summary>
        /// Vr�t� obsah messengeru (HTML k�d) p�ipraven� k vyrenderov�n� do str�nky.
        /// </summary>
        protected virtual string GetText()
        {
            if (Messenger.Messages.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("<div class=\"tmessenger\">");
                foreach (MessengerMessage message in Messenger.Messages)
                {
                    AddMessageText(message, sb);
                }
                sb.AppendLine("</div>");

                return sb.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Vr�t� text zpr�vy messengeru (HTML k�d) p�ipraven� k vyrenderov�n� do str�nky.
        /// </summary>
        protected virtual void AddMessageText(MessengerMessage message, StringBuilder sb)
        {
            Debug.Assert(message != null);
            Debug.Assert(sb != null);

            string messageCssClass;
            switch (message.MessageType)
            {
                case MessageType.Information:
                    messageCssClass = "tmessageinformation";
                    break;
                case MessageType.Warning:
                    messageCssClass = "tmessagewarning";
                    break;
                case MessageType.Error:
                    messageCssClass = "tmessageerror";
                    break;
                default:
                    throw new InvalidOperationException("Nezn�m� typ zpr�vy.");
            }

            sb.Append("<div class=\"");
            sb.Append(messageCssClass);
            sb.Append("\">");

            sb.Append("<span class=\"tmessagetext\">");
            sb.Append(message.Text.Replace("\n", "<br/>\n"));
            sb.Append("</span>");

            sb.Append("</div>");
        }
        #endregion

    }
}
