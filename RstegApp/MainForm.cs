using System;
using System.Threading;
using System.Windows.Forms;
using RstegApp.Controls;
using RstegApp.Logic;
using RstegApp.Presenters;
using RstegApp.Properties;

namespace RstegApp
{
    public partial class MainForm : Form
    {

        public MainFormPresenter CurrentPresenter { get; set; }
        private SynchronizationContext synchronizationContext;

        public MainForm()
        {
            InitializeComponent();
            InitializePresenter();

            InitClientFields();
            InitServerFields();

            InitOutput();

            KeySendCheckBox.CheckedChanged += OnKeySendCheckedChanged;

            synchronizationContext = SynchronizationContext.Current;
        }

        private void OnKeySendCheckedChanged(object sender, EventArgs e)
        {
            CurrentPresenter.SendKey = KeySendCheckBox.Checked;
        }

        private void InitOutput()
        {
            CurrentPresenter.MessageRecieve += OnMessageRecieve;
        }

        private void InitializePresenter()
        {
            CurrentPresenter = new MainFormPresenter();
        }

        private void InitServerFields()
        {
            ServerIPField.SetLabel("Server IP address:");
            ServerPortField.SetLabel("Server port:");

            ServerIPField.SetValue(CurrentPresenter.ServerIp);
            ServerPortField.SetValue(CurrentPresenter.ServerPort);

            ServerIPField.FieldValueChanged += OnServerIpChanged;
            ServerPortField.FieldValueChanged += OnServerPortChanged;
        }

        private void InitClientFields()
        {
            ClientIpField.SetLabel("Client IP address:");
            CilentPortField.SetLabel("Client port:");
            ClientMessageField.SetLabel("Client message:");

            ClientIpField.SetValue(CurrentPresenter.ClientIp);
            CilentPortField.SetValue(CurrentPresenter.ClientPort);
            ClientMessageField.SetValue(CurrentPresenter.ClientMessage);

            ClientIpField.FieldValueChanged += OnClientIpChanged;
            CilentPortField.FieldValueChanged += OnClientPortChanged;
            ClientMessageField.FieldValueChanged += OnClientMessageChanged;
        }

        private void ClientRunButton_Click(object sender, EventArgs e)
        {
            CurrentPresenter.UpdateClient();
            UpdateClientControls();
        }


        private void RunServerBtn_Click(object sender, EventArgs e)
        {
            CurrentPresenter.UpdateServer();
            UpdateServerControls();
        }


        private void SendMessageButton_Click(object sender, EventArgs e)
        {
            CurrentPresenter.SendClientMessage();
        }
        private void UpdateClientControls()
        {
            ClientRunButton.Text = CurrentPresenter.ClientStarted ? Resources.Stop : Resources.Run;
            ClientIpField.Enabled = !CurrentPresenter.ClientStarted;
            CilentPortField.Enabled = !CurrentPresenter.ClientStarted;
            Refresh();
        }
        private void UpdateServerControls()
        {
            RunServerBtn.Text = CurrentPresenter.ServerStarted ? Resources.Stop : Resources.Run;
            ServerIPField.Enabled = !CurrentPresenter.ServerStarted;
            ServerPortField.Enabled = !CurrentPresenter.ServerStarted;
            Refresh();
        }

        #region Event handlers

        private void OnServerPortChanged(object myobject, ValueChangedEventArgs myargs)
        {
            CurrentPresenter.ServerPort = Convert.ToInt16(myargs.NewValue);
        }

        private void OnServerIpChanged(object myobject, ValueChangedEventArgs myargs)
        {
            CurrentPresenter.ServerIp = myargs.NewValue.ToString();
        }

        private void OnClientMessageChanged(object myobject, ValueChangedEventArgs myargs)
        {
            CurrentPresenter.ClientMessage = myargs.NewValue.ToString();
        }

        private void OnClientPortChanged(object myobject, ValueChangedEventArgs myargs)
        {
            CurrentPresenter.ClientPort = Convert.ToInt16(myargs.NewValue);
        }

        private void OnClientIpChanged(object myobject, ValueChangedEventArgs myargs)
        {
            CurrentPresenter.ClientIp = myargs.NewValue.ToString();
        }
        private void OnMessageRecieve(object myobject, MessageEventArgs myargs)
        {
            synchronizationContext.Send(_ =>
            {
                OutputTextBox.AppendText(myargs.Message + "\n");
            } , this);
        }

        #endregion
    }
}
