using System;
using System.Threading.Tasks;
using RstegApp.Logic;
using RstegApp.Logic.Client;
using RstegApp.Logic.Server;
using RstegApp.Properties;

namespace RstegApp.Presenters
{
    public class MainFormPresenter : MessageBus, IDisposable
    {
        public string KeyWord { get; set; }
        public string StegWord { get; set; }

        public bool SendKey { get; set; }

        public MainFormPresenter()
        {
            short defPort = 2017;

            var defIp = "192.168.48.170";
            ClientIp =
                ServerIp = defIp;
            ServerPort =
                ClientPort = defPort;
            ClientMessage = "слово";

            KeyWord = Resources.KeyWord;
            StegWord = Resources.StegWord;
        }

        private void DoTask(Action action)
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
            DoTask(() =>
            {
                if (_server == null)
                {
                    return;
                }

                _server.Message -= OnServerMessage;
                _server.Dispose();
                _server = null;
            });
        }


        private void RunServer()
        {
            DoTask(() =>
            {
                _server = new Server(ServerIp, ServerPort);
                _server.Message += OnServerMessage;
                _server.Start();
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
            DoTask(() => { _client.Send(ClientMessage, SendKey); });
        }

        private void StopClient()
        {
            DoTask(() =>
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            });
        }


        private void RunClient()
        {
            DoTask(() =>
            {
                try
                {
                    _client = new Client(ClientIp, ClientPort);
                }
                catch (Exception e)
                {
                    OnMessage(e.Message);
                    return;
                }

                _client.Message += OnClientMessage;
            });
        }

        #endregion

        #region Event handlers

        private void OnClientMessage(object myobject, MessageEventArgs myargs)
        {
            OnMessage(myargs);
        }

        private void OnServerMessage(object myobject, MessageEventArgs myargs)
        {
            OnMessage(myargs);
        }

        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
            if (_client != null)
            {
                _client.Dispose();
            }
        }

        #endregion
    }
}