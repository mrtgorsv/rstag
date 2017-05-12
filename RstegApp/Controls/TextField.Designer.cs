namespace RstegApp.Controls
{
    partial class TextField
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.FieldLabel = new System.Windows.Forms.Label();
            this.TextControl = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.FieldLabel);
            this.flowLayoutPanel1.Controls.Add(this.TextControl);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(280, 30);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // FieldLabel
            // 
            this.FieldLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.FieldLabel.AutoSize = true;
            this.FieldLabel.Location = new System.Drawing.Point(3, 6);
            this.FieldLabel.MaximumSize = new System.Drawing.Size(150, 0);
            this.FieldLabel.MinimumSize = new System.Drawing.Size(150, 0);
            this.FieldLabel.Name = "FieldLabel";
            this.FieldLabel.Size = new System.Drawing.Size(150, 13);
            this.FieldLabel.TabIndex = 0;
            this.FieldLabel.Text = "label1";
            this.FieldLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextControl
            // 
            this.TextControl.Location = new System.Drawing.Point(159, 3);
            this.TextControl.Name = "TextControl";
            this.TextControl.Size = new System.Drawing.Size(100, 20);
            this.TextControl.TabIndex = 1;
            // 
            // TextField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(0, 30);
            this.MinimumSize = new System.Drawing.Size(280, 30);
            this.Name = "TextField";
            this.Size = new System.Drawing.Size(280, 30);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label FieldLabel;
        private System.Windows.Forms.TextBox TextControl;
    }
}
