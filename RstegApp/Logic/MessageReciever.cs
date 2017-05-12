using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RstegApp.Logic
{
    public class MessageReciever
    {
        public delegate void MessageRecieveEventHandler(object myObject, MessageRecieveEventArgs myArgs);

        public event MessageRecieveEventHandler MessageRecieved;

        protected virtual void OnMessageRecieved(string msg)
        {
            var handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, new MessageRecieveEventArgs(msg));
            }
        }
    }
    public class MessageRecieveEventArgs
    {
        public string Message { get; set; }

        public MessageRecieveEventArgs(string msg)
        {
            Message = msg;
        }
    }
}
