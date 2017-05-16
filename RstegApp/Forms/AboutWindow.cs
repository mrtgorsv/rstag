using System;
using System.Reflection;
using System.Windows.Forms;
using RstegApp.Properties;

namespace RstegApp.Forms
{
    partial class AboutWindow : Form
    {
        public AboutWindow()
        {
            InitializeComponent();
            Text = string.Format(Resources.AboutTemplate, Resources.ApplicationTitle);
            labelProductName.Text = Resources.ApplicationTitle;
            labelVersion.Text = String.Format(Resources.Template_Version, AssemblyVersion);
            InfoLabel.Text = Resources.AdditionalInfo;

            string email = Resources.EmailLink;
            EmailLinkLabel.Text = email;
            EmailLinkLabel.LinkClicked += EmailLinkLabel_LinkClicked;

            okButton.Click += OkButton_Click;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void EmailLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Specify that the link was visited.
            this.EmailLinkLabel.LinkVisited = true;

            try
            {
                // Navigate to a URL.
                System.Diagnostics.Process.Start(string.Format("mailto:{0}", Resources.EmailLink));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #region Методы доступа к атрибутам сборки

        public string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute) attributes[0]).Description;
            }
        }

        #endregion
    }
}