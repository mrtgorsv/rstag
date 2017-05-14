using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RstegApp.Properties;

namespace RstegApp.Logic.Server
{
    class Server : MessageBus
    {
        private Reader _reader;
        private TcpListener _listener;
        private TcpClient _client;
        public Server(string ipAdress, short port)
        {
            _reader = new Reader(port);

            IPAddress host = IPAddress.Parse(ipAdress);
            _listener = new TcpListener(host, port);
        }

        private bool _started;
        private void ReadClient()
        {
            if (_client != null)
            {
                bool done = false;
                while (!done)
                {
                    string message = ReadMes(_client);

                    done = message.Equals(Resources.EndMessage);
                    if (done)
                        SendResponse(_client, Resources.EndMessage);
                    else
                        SendResponse(_client, Resources.OkMessage);
                }
            }
        }

        private string ReadMes(TcpClient client)
        {
            byte[] buf = new byte[256];
            int totRead = 0;

            do
            {
                int read = client.GetStream().Read(buf, totRead, buf.Length - totRead);
                totRead += read;

            } while (client.GetStream().DataAvailable);
            string message = Encoding.Unicode.GetString(buf, 0, totRead);
            OnMessageRecieved(message);
            return message;
        }

        private void SendResponse(TcpClient client, string mess)
        {
            byte[] byt = Encoding.Unicode.GetBytes(mess);

            OnMessageSended(mess);

            client.GetStream().Write(byt, 0, byt.Length);
        }

        public void Start()
        {
            Stop();
            _listener.Start();
            _reader.StartCapturing(true);
            _client = _listener.AcceptTcpClient();

            Task.Factory.StartNew(ReadClient);
        }

        public void Stop()
        {
            _started = false;
            _listener.Stop();
        }
    }
}