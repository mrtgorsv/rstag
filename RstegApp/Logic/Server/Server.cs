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
        private PacketCapturer _packetCapturer;
        private TcpListener _listener;

        private TcpClient _client;

        public Server(string ipAdress, short port)
        {
            _packetCapturer = new PacketCapturer(port);

            _packetCapturer.Message += OnPacketCapturerMessage;

            IPAddress host = IPAddress.Parse(ipAdress);
            _listener = new TcpListener(host, port);

            // Определим нужное максимальное количество потоков
            // Пусть будет по 4 на каждый процессор
            int maxThreadsCount = Environment.ProcessorCount * 4;
            Console.WriteLine(maxThreadsCount.ToString());
            // Установим максимальное количество рабочих потоков
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
            // Установим минимальное количество рабочих потоков
            ThreadPool.SetMinThreads(2, 2);
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
            Stop();
            _listener.Start();
            _packetCapturer.StartCapturing(true);

            _client = _listener.AcceptTcpClient();

            Task.Factory.StartNew(ReadClient);
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }
}