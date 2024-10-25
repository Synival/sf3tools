namespace SF3.Editor.Forms
{
    partial class EditorForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_CopyTablesFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp_Version = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiScenario,
            this.tsmiHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(284, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile_Open,
            this.tsmiFile_SaveAs,
            this.tsSeparator_File1,
            this.tsmiFile_Close,
            this.tsSeparator_File2,
            this.tsmiFile_CopyTablesFrom,
            this.tsSeparator_File3,
            this.tsmiFile_Exit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "&File";
            // 
            // tsmiFile_Open
            // 
            this.tsmiFile_Open.Name = "tsmiFile_Open";
            this.tsmiFile_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiFile_Open.Size = new System.Drawing.Size(186, 22);
            this.tsmiFile_Open.Text = "&Open...";
            this.tsmiFile_Open.Click += new System.EventHandler(this.tsmiFile_Open_Click);
            // 
            // tsmiFile_SaveAs
            // 
            this.tsmiFile_SaveAs.Enabled = false;
            this.tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            this.tsmiFile_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.tsmiFile_SaveAs.Size = new System.Drawing.Size(186, 22);
            this.tsmiFile_SaveAs.Text = "Save &As...";
            this.tsmiFile_SaveAs.Click += new System.EventHandler(this.tsmiFile_SaveAs_Click);
            // 
            // tsSeparator_File1
            // 
            this.tsSeparator_File1.Name = "tsSeparator_File1";
            this.tsSeparator_File1.Size = new System.Drawing.Size(183, 6);
            // 
            // tsmiFile_Close
            // 
            this.tsmiFile_Close.Enabled = false;
            this.tsmiFile_Close.Name = "tsmiFile_Close";
            this.tsmiFile_Close.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.tsmiFile_Close.Size = new System.Drawing.Size(186, 22);
            this.tsmiFile_Close.Text = "&Close";
            this.tsmiFile_Close.Click += new System.EventHandler(this.tsmiFile_Close_Click);
            // 
            // tsSeparator_File2
            // 
            this.tsSeparator_File2.Name = "tsSeparator_File2";
            this.tsSeparator_File2.Size = new System.Drawing.Size(183, 6);
            // 
            // tsmiFile_CopyTablesFrom
            // 
            this.tsmiFile_CopyTablesFrom.Enabled = false;
            this.tsmiFile_CopyTablesFrom.Name = "tsmiFile_CopyTablesFrom";
            this.tsmiFile_CopyTablesFrom.Size = new System.Drawing.Size(186, 22);
            this.tsmiFile_CopyTablesFrom.Text = "Copy Tables From...";
            this.tsmiFile_CopyTablesFrom.Click += new System.EventHandler(this.tsmiFile_CopyTablesFrom_Click);
            // 
            // tsSeparator_File3
            // 
            this.tsSeparator_File3.Name = "tsSeparator_File3";
            this.tsSeparator_File3.Size = new System.Drawing.Size(183, 6);
            // 
            // tsmiFile_Exit
            // 
            this.tsmiFile_Exit.Name = "tsmiFile_Exit";
            this.tsmiFile_Exit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.tsmiFile_Exit.Size = new System.Drawing.Size(186, 22);
            this.tsmiFile_Exit.Text = "E&xit";
            this.tsmiFile_Exit.Click += new System.EventHandler(this.tsmiFile_Exit_Click);
            // 
            // tsmiScenario
            // 
            this.tsmiScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiScenario_Scenario1,
            this.tsmiScenario_Scenario2,
            this.tsmiScenario_Scenario3,
            this.tsmiScenario_PremiumDisk});
            this.tsmiScenario.Name = "tsmiScenario";
            this.tsmiScenario.Size = new System.Drawing.Size(64, 20);
            this.tsmiScenario.Text = "&Scenario";
            // 
            // tsmiScenario_Scenario1
            // 
            this.tsmiScenario_Scenario1.Name = "tsmiScenario_Scenario1";
            this.tsmiScenario_Scenario1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.tsmiScenario_Scenario1.Size = new System.Drawing.Size(189, 22);
            this.tsmiScenario_Scenario1.Text = "Scenario &1";
            this.tsmiScenario_Scenario1.Click += new System.EventHandler(this.tsmiScenario_Scenario1_Click);
            // 
            // tsmiScenario_Scenario2
            // 
            this.tsmiScenario_Scenario2.Name = "tsmiScenario_Scenario2";
            this.tsmiScenario_Scenario2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.tsmiScenario_Scenario2.Size = new System.Drawing.Size(189, 22);
            this.tsmiScenario_Scenario2.Text = "Scenario &2";
            this.tsmiScenario_Scenario2.Click += new System.EventHandler(this.tsmiScenario_Scenario2_Click);
            // 
            // tsmiScenario_Scenario3
            // 
            this.tsmiScenario_Scenario3.Name = "tsmiScenario_Scenario3";
            this.tsmiScenario_Scenario3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.tsmiScenario_Scenario3.Size = new System.Drawing.Size(189, 22);
            this.tsmiScenario_Scenario3.Text = "Scenario &3";
            this.tsmiScenario_Scenario3.Click += new System.EventHandler(this.tsmiScenario_Scenario3_Click);
            // 
            // tsmiScenario_PremiumDisk
            // 
            this.tsmiScenario_PremiumDisk.Name = "tsmiScenario_PremiumDisk";
            this.tsmiScenario_PremiumDisk.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.tsmiScenario_PremiumDisk.Size = new System.Drawing.Size(189, 22);
            this.tsmiScenario_PremiumDisk.Text = "&Premium Disk";
            this.tsmiScenario_PremiumDisk.Click += new System.EventHandler(this.tsmiScenario_PremiumDisk_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelp_Version});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_Version
            // 
            this.tsmiHelp_Version.Name = "tsmiHelp_Version";
            this.tsmiHelp_Version.Size = new System.Drawing.Size(299, 22);
            this.tsmiHelp_Version.Text = "Version (set internally)";
            // 
            // EditorForm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.menuStrip1);
            this.Name = "EditorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Open;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SaveAs;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Close;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_CopyTablesFrom;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File3;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Exit;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario1;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario2;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario3;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_PremiumDisk;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_Version;
    }
}
