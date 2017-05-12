using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using RstegApp.Logic;
using RstegApp.Logic.Client;
using RstegApp.Logic.Server;

namespace RstegApp
{
    public partial class Form1 : Form
    {
        private Reader _reader = new Reader();
        private Server _server;
        private bool _serverRunned = false;
        private object _syncRoot = new object();

        public Form1()
        {
            InitializeComponent();

            InitClientFields();
            InitServerFields();
        }

        private void InitServerFields()
        {
            ServerIPField.SetLabel("Server IP address:");
            ServerPortField.SetLabel("Server port:");

            ServerIPField.SetValue("192.168.48.170");
            ServerPortField.SetValue(2017);
        }

        private void InitClientFields()
        {
            ClientIpField.SetLabel("Client IP address:");
            CilentPortField.SetLabel("Client port:");
            ClientMessageField.SetLabel("Client message:");

            ClientIpField.SetValue("192.168.48.170");
            CilentPortField.SetValue(2017);
            ClientMessageField.SetValue("test");
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            var ipAddress = ClientIpField.GetValue() as string;
            var ipPort = Convert.ToInt16(CilentPortField.GetValue());
            Client client = new Client(ipAddress , ipPort);

            var message = ClientMessageField.GetValue() as string;

            client.Send(string.Empty , message , _reader);
        }

        private void RunServerBtn_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                DoLockedOperation(() =>
                {
                    if (_serverRunned)
                    {
                        StopServer();
                    }
                    else
                    {
                        RunServer();
                    }
                });
            });

        }

        private void DoLockedOperation(Action action)
        {
            lock (_syncRoot)
            {
                action();
            }
        }

        private void StopServer()
        {
            _server.Stop();
            _server = null;
        }

        private void RunServer()
        {
            var ipAddress = ServerIPField.GetValue() as string;
            var ipPort = Convert.ToInt16(ServerPortField.GetValue());
            _server = new Server(ipAddress , ipPort);
            _server.Start();
        }
    }
}
