namespace SlimSolution
{
    partial class ValidationSettingsForm
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
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lstSettings = new System.Windows.Forms.CheckedListBox();
            this.chkBoxAlwaysPublish = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.Location = new System.Drawing.Point(256, 225);
            this.btnSaveSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(100, 34);
            this.btnSaveSettings.TabIndex = 6;
            this.btnSaveSettings.Text = "Save";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.BtnSaveSettings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select your criteria";
            // 
            // lstSettings
            // 
            this.lstSettings.FormattingEnabled = true;
            this.lstSettings.Items.AddRange(new object[] {
            "Entity Components",
            "Processes"});
            this.lstSettings.Location = new System.Drawing.Point(4, 23);
            this.lstSettings.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstSettings.Name = "lstSettings";
            this.lstSettings.Size = new System.Drawing.Size(353, 72);
            this.lstSettings.TabIndex = 4;
            // 
            // chkBoxAlwaysPublish
            // 
            this.chkBoxAlwaysPublish.AutoSize = true;
            this.chkBoxAlwaysPublish.Checked = true;
            this.chkBoxAlwaysPublish.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBoxAlwaysPublish.Location = new System.Drawing.Point(4, 108);
            this.chkBoxAlwaysPublish.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkBoxAlwaysPublish.Name = "chkBoxAlwaysPublish";
            this.chkBoxAlwaysPublish.Size = new System.Drawing.Size(123, 21);
            this.chkBoxAlwaysPublish.TabIndex = 7;
            this.chkBoxAlwaysPublish.Text = "Always Publish";
            this.chkBoxAlwaysPublish.UseVisualStyleBackColor = true;
            // 
            // ValidationSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 265);
            this.Controls.Add(this.chkBoxAlwaysPublish);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstSettings);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "ValidationSettingsForm";
            this.Text = "Validation Settings Form";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox lstSettings;
        private System.Windows.Forms.CheckBox chkBoxAlwaysPublish;
    }
}