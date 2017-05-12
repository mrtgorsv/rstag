namespace RstegApp.Controls
{
    partial class NumericField
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
            this.NumericUpDownControl = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownControl)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.FieldLabel);
            this.flowLayoutPanel1.Controls.Add(this.NumericUpDownControl);
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
            // NumericUpDownControl
            // 
            this.NumericUpDownControl.Location = new System.Drawing.Point(159, 3);
            this.NumericUpDownControl.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.NumericUpDownControl.Name = "NumericUpDownControl";
            this.NumericUpDownControl.Size = new System.Drawing.Size(103, 20);
            this.NumericUpDownControl.TabIndex = 1;
            // 
            // NumericField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(0, 30);
            this.MinimumSize = new System.Drawing.Size(280, 30);
            this.Name = "NumericField";
            this.Size = new System.Drawing.Size(280, 30);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumericUpDownControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label FieldLabel;
        private System.Windows.Forms.NumericUpDown NumericUpDownControl;
    }
}
