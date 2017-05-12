using System;
using System.Text;
using System.Net.Sockets;
using RstegApp.Properties;

namespace RstegApp.Logic.Client
{
    class Client
    {
        private readonly TcpClient _client;

        public Client(string ipAddress, short port)
        {
            _client = new TcpClient(ipAddress, port);
        }

        public void Send(string stegWord, string message, Reader reader)
        {
            try
            {
                reader.Read(Resources.KeyWord, stegWord);
            }
            catch (Exception e)
            {
                //Console.WriteLine(Resources.ExceptionMessage, e.Message);
            }
            //Console.WriteLine(Resources.ClienEndMessage, Resources.EndMessage);
            SendMess(_client, message);

            bool done = false;

            while (!done)
            {
                string res = ReadRes(_client);

                done = res.Equals(Resources.EndMessage);

                if (done)
                    SendMess(_client, Resources.EndMessage);
                else
                    SendMess(_client, Resources.OkMessage);
            }
        }

        private static void SendMess(TcpClient client, string mess)
        {
            byte[] bts = Encoding.Unicode.GetBytes(mess);
            client.GetStream().Write(bts, 0, bts.Length);
        }

        private static string ReadRes(TcpClient client)
        {
            byte[] buf = new byte[256];
            int totread = 0;
            do
            {
                int read = client.GetStream().Read(buf, totread, buf.Length - totread);
                totread += read;
            } while (client.GetStream().DataAvailable);
            return Encoding.Unicode.GetString(buf, 0, totread);
        }
    }
}