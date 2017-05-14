using System;
using System.Threading.Tasks;
using RstegApp.Logic;
using RstegApp.Logic.Client;
using RstegApp.Logic.Server;
using RstegApp.Properties;

namespace RstegApp.Presenters
{
    public class MainFormPresenter : MessageBus
    {
        public string KeyWord { get; set; }
        public string StegWord { get; set; }

        public bool SendKey { get; set; }

        public MainFormPresenter()
        {
            var defIp = "192.168.1.35";
            short defPort = 2017;
            ClientIp =
                ServerIp = defIp;
            ServerPort =
                ClientPort = defPort;
            ClientMessage = "test";

            KeyWord = Resources.KeyWord;
            StegWord = Resources.StegWord;
        }

        private void DoLockTask(Action action)
        {
            Task.Factory.StartNew(action);
        }

        #region Server

        private Server _server;
        private bool _serverStarted;

        public string ServerMessage { get; set; }
        public string ServerIp { get; set; }
        public short ServerPort { get; set; }

        public bool ServerStarted
        {
            get { return _serverStarted; }
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

        private void StopServer()
        {
            DoLockTask(() =>
            {
                if (_server == null)
                {
                    return;
                }

                _server.Stop();
                _server.MessageRecieve -= OnServerMessageRecieve;
                _server.MessageSend -= OnServerMessageSend;
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
                _server.MessageRecieve += OnServerMessageRecieve;
                _server.MessageSend += OnServerMessageSend;
            });
        }

        #endregion

        #region Client

        private Client _client;
        private bool _clientStarted;

        public string ClientIp { get; set; }
        public string ClientMessage { get; set; }
        public short ClientPort { get; set; }

        public bool ClientStarted
        {
            get { return _clientStarted; }
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
            DoLockTask(() => { _client.Send(StegWord, ClientMessage, SendKey); });
        }

        private void StopClient()
        {
            DoLockTask(() =>
            {
                _client.Stop();
                _client.MessageRecieve -= OnClientMessageRecieve;
                _client.MessageSend -= OnClientMessageSend;
                _client = null;
            });
        }

        private void RunClient()
        {
            DoLockTask(() =>
            {
                _client = new Client(ClientIp, ClientPort);
                _client.MessageRecieve += OnClientMessageRecieve;
                _client.MessageSend += OnClientMessageSend;
            });
        }

        #endregion

        #region Event handlers

        private void OnServerMessageRecieve(object myobject, MessageEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Server : Recieve {0}", myargs.Message));
        }

        private void OnClientMessageRecieve(object myobject, MessageEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Client : Recieve {0}", myargs.Message));
        }

        private void OnClientMessageSend(object myobject, MessageEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Client : Send {0}", myargs.Message));
        }

        private void OnServerMessageSend(object myobject, MessageEventArgs myargs)
        {
            OnMessageRecieved(string.Format("Server : Send {0}", myargs.Message));
        }

        #endregion
    }
}