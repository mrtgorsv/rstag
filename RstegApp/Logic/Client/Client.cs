using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using RstegApp.Properties;

namespace RstegApp.Logic.Client
{
    class Client : MessageBus
    {
        private TcpClient _client;
        private int _maxMessages = 10;
        private Reader _reader;


        public Client(string ipAddress, short port)
        {
            _client = new TcpClient(ipAddress, port);
            try
            {
                _reader = new Reader(port);
                _reader.StartCapturing(false);
            }
            catch (Exception e)
            {
                //Console.WriteLine(Resources.ExceptionMessage, e.Message);
            }
        }

        public void Send(string stegWord, string message, bool sendKeyWord)
        {
            SendMess(_client, message , sendKeyWord);

            string res = ReadRes(_client);

            if (res.Equals(Resources.EndMessage))
                SendMess(_client, Resources.EndMessage);
            else
                SendMess(_client, Resources.OkMessage);
        }

        private void SendMess(TcpClient client, string mess , bool sendKeyWord = false)
        {

            byte[] bts = Encoding.Unicode.GetBytes(mess);
            if (sendKeyWord)
            {
                byte[] btsTmp = new byte[bts.Length];
                Array.Copy(bts, btsTmp,bts.Length);
                var keyBytes = Encoding.Unicode.GetBytes(Resources.KeyWord);

                bts = new byte[bts.Length + keyBytes.Length];
                btsTmp.CopyTo(bts, 0);
                keyBytes.CopyTo(bts, keyBytes.Length);
            }

            OnMessageSended(Encoding.Unicode.GetString(bts));
            client.GetStream().Write(bts, 0, bts.Length);
        }

        private string ReadRes(TcpClient client)
        {
            byte[] buf = new byte[256];
            int totread = 0;
            do
            {
                int read = client.GetStream().Read(buf, totread, buf.Length - totread);
                totread += read;
            } while (client.GetStream().DataAvailable);

            string message = Encoding.Unicode.GetString(buf, 0, totread);
            OnMessageRecieved(message);
            return message;
        }

        public void Stop()
        {
            _client = null;
        }
    }
}