using System;
using System.Text;
using System.Net.Sockets;
using RstegApp.Logic.Capture;
using RstegApp.Models.Enums;
using RstegApp.Properties;

namespace RstegApp.Logic.Client
{
    class Client : MessageBus, IDisposable
    {
        private readonly TcpClient _client;
        private readonly PacketCapturer _packetCapturer;

        public Client(string ipAddress, short port) : base(InitiatorType.Client)
        {
            _client = new TcpClient(ipAddress, port);

            _packetCapturer = new PacketCapturer(port);
            _packetCapturer.StartCapturing(false);
            _packetCapturer.Message += OnPacketCapturerMessage;
        }

        private void OnPacketCapturerMessage(object myobject, MessageEventArgs myargs)
        {
            OnMessage(myargs.Message);
        }

        public void Send( string message, bool sendKeyWord)
        {
            SendMessage(_client, message, sendKeyWord);

            string res = ReadResponse(_client);

            if (res.Equals(Resources.EndMessage))
            {
                SendMessage(_client, Resources.EndMessage);
            }
        }

        private void SendMessage(TcpClient client, string mess, bool sendKeyWord = false)
        {
            byte[] bts = Encoding.Unicode.GetBytes(mess);
            if (sendKeyWord)
            {
                bts = Encoding.Unicode.GetBytes(Resources.KeyWord);
            }

            OnMessageSended(Encoding.Unicode.GetString(bts));
            client.GetStream().Write(bts, 0, bts.Length);
        }

        private string ReadResponse(TcpClient client)
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

        public void Dispose()
        {
            if (_client != null)
            {
                ((IDisposable) _client).Dispose();
            }
            _packetCapturer.Dispose();
        }
    }
}