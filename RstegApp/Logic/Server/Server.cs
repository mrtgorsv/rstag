using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using RstegApp.Properties;

namespace RstegApp.Logic.Server
{
    class Server : MessageReciever
    {
        private Reader _reader;
        private TcpListener _listener;

        private bool _started;
        private void HandleClientThread(object obj)
        {
            TcpClient client = obj as TcpClient;
            if (client != null)
            {
                bool done = false;
                while (!done)
                {
                    string message = ReadMes(client);

                    done = message.Equals(Resources.EndMessage);
                    if (done)
                        SendResponse(client, Resources.EndMessage);
                    else
                        SendResponse(client, Resources.OkMessage);
                }
                client.Close();
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

        private static void SendResponse(TcpClient client, string mess)
        {
            byte[] byt = Encoding.Unicode.GetBytes(mess);
            client.GetStream().Write(byt, 0, byt.Length);
        }


        public Server(string ipAdress, int port)
        {
            _reader = new Reader();

            IPAddress host = IPAddress.Parse(ipAdress);
            _listener = new TcpListener(host, port);
        }

        public void Start()
        {
            Stop();
            _listener.Start();
            _listener.Pending();
            TcpClient client = _listener.AcceptTcpClient();
            Thread thread = new Thread(HandleClientThread);

            thread.Start(client);
            while (_started)
            {
                if (client.Connected)
                {
                    try
                    {
                        _reader.Read(Resources.KeyWord);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        public void Stop()
        {
            _started = false;
            _listener.Stop();
        }
    }
}