namespace Solution_Quality_Checker
{
    partial class SQCControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SQCControl));
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSample = new System.Windows.Forms.ToolStripButton();
            this.btnLoadSolutions = new System.Windows.Forms.ToolStripButton();
            this.btnCheckSolution = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstSolutions = new System.Windows.Forms.ListBox();
            this.btnSaveReport = new System.Windows.Forms.ToolStripButton();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClose,
            this.tssSeparator1,
            this.btnLoadSolutions,
            this.btnCheckSolution,
            this.tsbSample,
            this.btnSaveReport});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(981, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "Load Solutions";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(40, 22);
            this.tsbClose.Text = "Close";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSample
            // 
            this.tsbSample.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSample.Name = "tsbSample";
            this.tsbSample.Size = new System.Drawing.Size(48, 22);
            this.tsbSample.Text = "Try me";
            this.tsbSample.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // btnLoadSolutions
            // 
            this.btnLoadSolutions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLoadSolutions.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadSolutions.Image")));
            this.btnLoadSolutions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLoadSolutions.Name = "btnLoadSolutions";
            this.btnLoadSolutions.Size = new System.Drawing.Size(89, 22);
            this.btnLoadSolutions.Text = "Load Solutions";
            this.btnLoadSolutions.Click += new System.EventHandler(this.btnLoadSolutions_Click);
            // 
            // btnCheckSolution
            // 
            this.btnCheckSolution.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCheckSolution.Enabled = false;
            this.btnCheckSolution.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckSolution.Image")));
            this.btnCheckSolution.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCheckSolution.Name = "btnCheckSolution";
            this.btnCheckSolution.Size = new System.Drawing.Size(91, 22);
            this.btnCheckSolution.Text = "Check Solution";
            this.btnCheckSolution.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstSolutions);
            this.splitContainer1.Size = new System.Drawing.Size(981, 516);
            this.splitContainer1.SplitterDistance = 196;
            this.splitContainer1.TabIndex = 5;
            // 
            // lstSolutions
            // 
            this.lstSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSolutions.FormattingEnabled = true;
            this.lstSolutions.Location = new System.Drawing.Point(0, 0);
            this.lstSolutions.Name = "lstSolutions";
            this.lstSolutions.Size = new System.Drawing.Size(196, 516);
            this.lstSolutions.TabIndex = 0;
            this.lstSolutions.SelectedIndexChanged += new System.EventHandler(this.lstSolutions_SelectedIndexChanged);
            // 
            // btnSaveReport
            // 
            this.btnSaveReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveReport.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveReport.Image")));
            this.btnSaveReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveReport.Name = "btnSaveReport";
            this.btnSaveReport.Size = new System.Drawing.Size(73, 22);
            this.btnSaveReport.Text = "Save Report";
            this.btnSaveReport.ToolTipText = "Save Report";
            // 
            // SQCControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "SQCControl";
            this.Size = new System.Drawing.Size(981, 541);
            this.Load += new System.EventHandler(this.SQCControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripButton tsbSample;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripButton btnLoadSolutions;
        private System.Windows.Forms.ToolStripButton btnCheckSolution;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstSolutions;
        private System.Windows.Forms.ToolStripButton btnSaveReport;
    }
}
