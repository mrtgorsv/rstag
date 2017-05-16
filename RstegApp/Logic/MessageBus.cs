using System.Text;
using RstegApp.Models.Enums;
using RstegApp.Properties;

namespace RstegApp.Logic
{
    public class MessageBus
    {
        private readonly InitiatorType _initiatorType;

        public MessageBus(InitiatorType initiatorType = InitiatorType.Unknown)
        {
            _initiatorType = initiatorType;
        }

        public delegate void MessageRecieveEventHandler(object myObject, MessageEventArgs myArgs);

        public delegate void MessageSendEventHandler(object myObject, MessageEventArgs myArgs);

        public delegate void MessageEventHandler(object myObject, MessageEventArgs myArgs);

        public event MessageEventHandler Message;

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
                sb.Append(string.Format(Resources.InitiatorTemplate, InitiatorType));
            }
            if (!MessageType.Equals(MessageType.Default))
            {
                sb.Append(string.Format(Resources.MessageTypeTemplate, MessageType));
            }
            sb.Append(Message);
            return sb.ToString();
        }
    }
}