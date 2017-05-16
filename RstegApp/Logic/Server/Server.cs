using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RstegApp.Logic.Capture;
using RstegApp.Models.Enums;
using RstegApp.Properties;

namespace RstegApp.Logic.Server
{
    class Server : MessageBus, IDisposable
    {
        private readonly PacketCapturer _packetCapturer;
        private readonly TcpListener _listener;

        private TcpClient _client;

        public Server(string ipAdress, short port) : base(InitiatorType.Server)
        {
            _packetCapturer = new PacketCapturer(port);

            _packetCapturer.Message += OnPacketCapturerMessage;

            IPAddress host = IPAddress.Parse(ipAdress);
            _listener = new TcpListener(host, port);
        }

        private void OnPacketCapturerMessage(object myobject, MessageEventArgs myargs)
        {
            OnMessage(myargs.Message);
        }

        private void ReadClient()
        {
            if (_client != null)
            {
                bool done = false;
                while (!done)
                {
                    string message = ReadMessage(_client);

                    if (message == null)
                    {
                        continue;
                    }

                    done = message.Equals(Resources.EndMessage);
                    if (message.Equals(Resources.KeyWord))
                        SendResponse(_client, Resources.KeyWord);
                    else if (message.Equals(Resources.EndMessage))
                        SendResponse(_client, Resources.OkMessage);
                }

                _client.Close();
            }
        }

        private string ReadMessage(TcpClient client)
        {
            byte[] buf = new byte[256];
            int totRead = 0;

            if (client.GetStream().DataAvailable)
            {
                do
                {
                    int read = client.GetStream().Read(buf, totRead, buf.Length - totRead);
                    totRead += read;
                } while (client.GetStream().DataAvailable);
            }
            else
            {
                return null;
            }
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
            _listener.Start();
            _packetCapturer.StartCapturing(true);

            _client = _listener.AcceptTcpClient();

            Task.Factory.StartNew(ReadClient);
        }

        public void Dispose()
        {
            _listener.Stop();
            if (_client != null)
            {
                ((IDisposable) _client).Dispose();
            }
            _packetCapturer.Dispose();
        }
    }
}