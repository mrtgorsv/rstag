using System.Text;
using RstegApp.Models.Enums;
using RstegApp.Properties;

namespace RstegApp.Logic
{
    public class MessageBus
    {
        private readonly InitiatorType _initiatorType;

        public delegate void MessageEventHandler(object myObject, MessageEventArgs myArgs);
        public event MessageEventHandler Message;

        public MessageBus(InitiatorType initiatorType = InitiatorType.Unknown)
        {
            _initiatorType = initiatorType;
        }

        protected virtual void OnMessage(string msg, MessageType messageType = MessageType.Default)
        {
            SenMessageEvent(new MessageEventArgs(msg, messageType, _initiatorType));
        }

        protected virtual void OnMessage(MessageEventArgs msgArgs)
        {
            SenMessageEvent(msgArgs);
        }

        protected virtual void OnMessageRecieved(string msg)
        {
            OnMessage(msg, MessageType.Recieve);
        }

        protected virtual void OnMessageSended(string msg)
        {
            OnMessage(msg, MessageType.Send);
        }

        private void SenMessageEvent(MessageEventArgs messageEventArgs)
        {
            var handler = Message;
            if (handler != null)
            {
                handler(this, messageEventArgs);
            }
        }
    }

    public class MessageEventArgs
    {
        public string Message { get; set; }
        public MessageType MessageType { get; set; }
        public InitiatorType InitiatorType { get; set; }

        public MessageEventArgs(string msg, MessageType messageType = MessageType.Default,
            InitiatorType initiatorType = InitiatorType.Unknown)
        {
            Message = msg;
            MessageType = messageType;
            InitiatorType = initiatorType;
        }

        public string GetMessage()
        {
            StringBuilder sb = new StringBuilder();
            if (InitiatorType != InitiatorType.Unknown)
            {
                sb.Append(InitiatorType.Equals(InitiatorType.Client) ? Resources.Client : Resources.Server);
            }
            if (!MessageType.Equals(MessageType.Default))
            {
                sb.Append(string.Format(Resources.Template_MessageType, MessageType.Equals(MessageType.Send) ? Resources.MessageSended : Resources.MessageRecieved));
            }
            sb.Append(Message);
            return sb.ToString();
        }
    }
}