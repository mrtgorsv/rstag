namespace RstegApp
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ClientIpField = new RstegApp.Controls.TextField();
            this.CilentPortField = new RstegApp.Controls.NumericField();
            this.ClientRunButton = new System.Windows.Forms.Button();
            this.ClientMessageField = new RstegApp.Controls.TextField();
            this.KeySendCheckBox = new System.Windows.Forms.CheckBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.ServerIPField = new RstegApp.Controls.TextField();
            this.ServerPortField = new RstegApp.Controls.NumericField();
            this.RunServerBtn = new System.Windows.Forms.Button();
            this.OutputTextBox = new System.Windows.Forms.RichTextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(3, 16);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(319, 293);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flowLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(311, 267);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Client";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.ClientIpField);
            this.flowLayoutPanel1.Controls.Add(this.CilentPortField);
            this.flowLayoutPanel1.Controls.Add(this.ClientRunButton);
            this.flowLayoutPanel1.Controls.Add(this.ClientMessageField);
            this.flowLayoutPanel1.Controls.Add(this.KeySendCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.SendMessageButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(305, 261);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // ClientIpField
            // 
            this.ClientIpField.Location = new System.Drawing.Point(3, 3);
            this.ClientIpField.MaximumSize = new System.Drawing.Size(0, 30);
            this.ClientIpField.MinimumSize = new System.Drawing.Size(280, 30);
            this.ClientIpField.Name = "ClientIpField";
            this.ClientIpField.Size = new System.Drawing.Size(280, 30);
            this.ClientIpField.TabIndex = 0;
            // 
            // CilentPortField
            // 
            this.CilentPortField.Location = new System.Drawing.Point(3, 39);
            this.CilentPortField.MaximumSize = new System.Drawing.Size(0, 30);
            this.CilentPortField.MinimumSize = new System.Drawing.Size(280, 30);
            this.CilentPortField.Name = "CilentPortField";
            this.CilentPortField.Size = new System.Drawing.Size(280, 30);
            this.CilentPortField.TabIndex = 1;
            // 
            // ClientRunButton
            // 
            this.ClientRunButton.Location = new System.Drawing.Point(3, 75);
            this.ClientRunButton.Name = "ClientRunButton";
            this.ClientRunButton.Size = new System.Drawing.Size(299, 23);
            this.ClientRunButton.TabIndex = 3;
            this.ClientRunButton.Text = "Run";
            this.ClientRunButton.UseVisualStyleBackColor = true;
            this.ClientRunButton.Click += new System.EventHandler(this.ClientRunButton_Click);
            // 
            // ClientMessageField
            // 
            this.ClientMessageField.Location = new System.Drawing.Point(3, 104);
            this.ClientMessageField.MaximumSize = new System.Drawing.Size(0, 30);
            this.ClientMessageField.MinimumSize = new System.Drawing.Size(280, 30);
            this.ClientMessageField.Name = "ClientMessageField";
            this.ClientMessageField.Size = new System.Drawing.Size(280, 30);
            this.ClientMessageField.TabIndex = 2;
            // 
            // KeySendCheckBox
            // 
            this.KeySendCheckBox.AutoSize = true;
            this.KeySendCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.KeySendCheckBox.Location = new System.Drawing.Point(3, 140);
            this.KeySendCheckBox.MinimumSize = new System.Drawing.Size(172, 0);
            this.KeySendCheckBox.Name = "KeySendCheckBox";
            this.KeySendCheckBox.Size = new System.Drawing.Size(172, 17);
            this.KeySendCheckBox.TabIndex = 5;
            this.KeySendCheckBox.Text = "Send Key Word";
            this.KeySendCheckBox.UseVisualStyleBackColor = true;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(3, 163);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(299, 23);
            this.SendMessageButton.TabIndex = 4;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flowLayoutPanel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(311, 267);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Server";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.ServerIPField);
            this.flowLayoutPanel2.Controls.Add(this.ServerPortField);
            this.flowLayoutPanel2.Controls.Add(this.RunServerBtn);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(305, 261);
            this.flowLayoutPanel2.TabIndex = 0;
            // 
            // ServerIPField
            // 
            this.ServerIPField.Location = new System.Drawing.Point(3, 3);
            this.ServerIPField.MaximumSize = new System.Drawing.Size(0, 30);
            this.ServerIPField.MinimumSize = new System.Drawing.Size(280, 30);
            this.ServerIPField.Name = "ServerIPField";
            this.ServerIPField.Size = new System.Drawing.Size(280, 30);
            this.ServerIPField.TabIndex = 0;
            // 
            // ServerPortField
            // 
            this.ServerPortField.Location = new System.Drawing.Point(3, 39);
            this.ServerPortField.MaximumSize = new System.Drawing.Size(0, 30);
            this.ServerPortField.MinimumSize = new System.Drawing.Size(280, 30);
            this.ServerPortField.Name = "ServerPortField";
            this.ServerPortField.Size = new System.Drawing.Size(280, 30);
            this.ServerPortField.TabIndex = 1;
            // 
            // RunServerBtn
            // 
            this.RunServerBtn.Location = new System.Drawing.Point(3, 75);
            this.RunServerBtn.Name = "RunServerBtn";
            this.RunServerBtn.Size = new System.Drawing.Size(299, 23);
            this.RunServerBtn.TabIndex = 2;
            this.RunServerBtn.Text = "Run";
            this.RunServerBtn.UseVisualStyleBackColor = true;
            this.RunServerBtn.Click += new System.EventHandler(this.RunServerBtn_Click);
            // 
            // OutputTextBox
            // 
            this.OutputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputTextBox.Location = new System.Drawing.Point(3, 16);
            this.OutputTextBox.Name = "OutputTextBox";
            this.OutputTextBox.Size = new System.Drawing.Size(524, 293);
            this.OutputTextBox.TabIndex = 1;
            this.OutputTextBox.Text = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(324, 312);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OutputTextBox);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(324, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(530, 312);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.menuStrip1.Size = new System.Drawing.Size(854, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.AboutMenuItem.Size = new System.Drawing.Size(48, 24);
            this.AboutMenuItem.Text = "About";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 336);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "RSteg Application";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox OutputTextBox;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Controls.TextField ClientIpField;
        private Controls.NumericField CilentPortField;
        private Controls.TextField ClientMessageField;
        private System.Windows.Forms.Button ClientRunButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private Controls.TextField ServerIPField;
        private Controls.NumericField ServerPortField;
        private System.Windows.Forms.Button RunServerBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.CheckBox KeySendCheckBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
    }
}

