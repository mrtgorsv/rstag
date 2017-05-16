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

        private readonly SynchronizationContext _synchronizationContext;

        public MainForm()
        {
            _synchronizationContext = SynchronizationContext.Current;

            InitializeComponent();
            InitializePresenter();

            InitClientFields();
            InitServerFields();

            InitOutput();

            KeySendCheckBox.CheckedChanged += OnKeySendCheckedChanged;

            AboutMenuItem.Click += OnAboutMenuItemClick;

            Closed += OnClosed;
        }

        private void OnAboutMenuItemClick(object sender, EventArgs e)
        {
            MessageBox.Show(this, Resources.AboutMessage, Resources.About);
        }

        private void OnClosed(object sender, EventArgs e)
        {
            CurrentPresenter.Dispose();
        }

        private void OnKeySendCheckedChanged(object sender, EventArgs e)
        {
            CurrentPresenter.SendKey = KeySendCheckBox.Checked;
        }

        private void InitOutput()
        {
            CurrentPresenter.Message += OnMessage;
        }

        private void InitializePresenter()
        {
            CurrentPresenter = new MainFormPresenter();
        }

        private void InitServerFields()
        {
            ServerIPField.SetLabel(Resources.IPField);
            ServerPortField.SetLabel(Resources.PortField);

            ServerIPField.SetValue(CurrentPresenter.ServerIp);
            ServerPortField.SetValue(CurrentPresenter.ServerPort);

            ServerIPField.FieldValueChanged += OnServerIpChanged;
            ServerPortField.FieldValueChanged += OnServerPortChanged;
        }

        private void InitClientFields()
        {
            ClientIpField.SetLabel(Resources.IPField);
            CilentPortField.SetLabel(Resources.PortField);
            ClientMessageField.SetLabel(Resources.MessageField);

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

        private void OnMessage(object myobject, MessageEventArgs myargs)
        {
            _synchronizationContext.Send(_ => { OutputTextBox.AppendText(myargs.GetMessage() + "\n"); }, this);
        }

        #endregion
    }
}