namespace RstegApp.Logic
{
    public class MessageBus
    {
        public delegate void MessageRecieveEventHandler(object myObject, MessageEventArgs myArgs);
        public delegate void MessageSendEventHandler(object myObject, MessageEventArgs myArgs);

        public event MessageRecieveEventHandler MessageRecieve;
        public event MessageSendEventHandler MessageSend;

        protected virtual void OnMessageRecieved(string msg)
        {
            var handler = MessageRecieve;
            if (handler != null)
            {
                handler(this, new MessageEventArgs(msg));
            }
        }
        protected virtual void OnMessageSended(string msg)
        {
            var handler = MessageSend;
            if (handler != null)
            {
                handler(this, new MessageEventArgs(msg));
            }
        }
    }
    public class MessageEventArgs
    {
        public string Message { get; set; }

        public MessageEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
