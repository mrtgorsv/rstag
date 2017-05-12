using System;
using System.Threading.Tasks;
using RstegApp.Logic;
using RstegApp.Logic.Client;
using RstegApp.Logic.Server;
using RstegApp.Properties;

namespace RstegApp.Presenters
{
    public class MainFormPresenter : MessageReciever
    {
        private readonly Reader _reader = new Reader();
        private Server _server;
        private Client _client;
        private bool _serverStarted;
        private bool _clientStarted;
        private readonly object _syncRoot = new object();

        public string ClientIp { get; set; }
        public string ClientMessage { get; set; }
        public string ServerMessage { get; set; }
        public string ServerIp { get; set; }
        public short ServerPort { get; set; }
        public short ClientPort { get; set; }


        public string KeyWord { get; set; }
        public string StegWord { get; set; }

        public MainFormPresenter()
        {
            var defIp = "192.168.1.35";
            short defPort = 2017;
            ClientIp = ServerIp = defIp;
            ServerPort = ClientPort = defPort;
            ClientMessage = "test";

            KeyWord = Resources.KeyWord;
            StegWord = Resources.StegWord;

            _reader = new Reader();
        }


        private void DoLockedOperation(Action action)
        {
            lock (_syncRoot)
            {
                action();
            }
        }

        public bool ServerStarted
        {
            get { return _serverStarted; }
        }

        public bool ClientStarted
        {
            get { return _clientStarted; }
        }

        public void UpdateServer()
        {
            if (_serverStarted)
            {
                StopServer();
            }
            else
            {
                RunServer();
            }
            _serverStarted = !_serverStarted;
        }

        public void UpdateClient()
        {
            if (_clientStarted)
            {
                StopClient();
            }
            else
            {
                RunClient();
            }
            _clientStarted = !_clientStarted;
        }

        public void SendClientMessage()
        {
            DoLockTask(() =>
            {
                _client.Send(string.Empty, ClientMessage, _reader);
            });
        }

        private void StopClient()
        {
            DoLockTask(() =>
            {
                _client.Stop();
                _client.MessageRecieved -= OnClientMessageRecieved;
                _client = null;
            });
        }

        private void RunClient()
        {
            DoLockTask(() =>
            {
                _client = new Client(ClientIp, ClientPort);
                _client.MessageRecieved += OnClientMessageRecieved;
            });
        }

        private void StopServer()
        {
            DoLockTask(() =>
            {
                if (_server == null)
                {
                    return;
                }

                _server.Stop();
                _server.MessageRecieved -= OnServerMessageRecieved;
                _server = null;
            });
        }

        private void RunServer()
        {
            StopServer();
            DoLockTask(() =>
            {
                _server = new Server(ServerIp, ServerPort);
                _server.Start();
                _server.MessageRecieved += OnServerMessageRecieved;
            });
        }

        private void DoLockTask(Action action)
        {
            Task.Factory.StartNew(action);
        }

        #region Event handlers

        private void OnServerMessageRecieved(object myobject, MessageRecieveEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Client recieve: {0}", myargs.Message));
        }

        private void OnClientMessageRecieved(object myobject, MessageRecieveEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Server recieve: {0}", myargs.Message));
        }
        #endregion
    }
}