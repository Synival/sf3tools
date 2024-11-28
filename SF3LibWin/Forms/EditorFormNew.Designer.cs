namespace SF3.Win.Forms {
    partial class EditorFormNew {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_ApplyDFRFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_GenerateDFRFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_CopyTablesTo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_CopyTablesFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiEdit_UseDropdowns = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp_Version = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_OpenPrevious = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_OpenNext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiEdit,
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
            this.tsmiFile_OpenPrevious,
            this.tsmiFile_OpenNext,
            this.tsmiFile_Save,
            this.tsmiFile_SaveAs,
            this.tsSeparator_File1,
            this.tsmiFile_Close,
            this.tsSeparator_File2,
            this.tsmiFile_ApplyDFRFile,
            this.tsmiFile_GenerateDFRFile,
            this.tsSeparator_File3,
            this.tsmiFile_CopyTablesTo,
            this.tsmiFile_CopyTablesFrom,
            this.tsSeparator_File4,
            this.tsmiFile_Exit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "&File";
            // 
            // tsmiFile_Open
            // 
            this.tsmiFile_Open.Name = "tsmiFile_Open";
            this.tsmiFile_Open.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiFile_Open.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_Open.Text = "&Open...";
            this.tsmiFile_Open.Click += new System.EventHandler(this.tsmiFile_Open_Click);
            // 
            // tsmiFile_Save
            // 
            this.tsmiFile_Save.Enabled = false;
            this.tsmiFile_Save.Name = "tsmiFile_Save";
            this.tsmiFile_Save.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiFile_Save.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_Save.Text = "&Save";
            this.tsmiFile_Save.Click += new System.EventHandler(this.tsmiFile_Save_Click);
            // 
            // tsmiFile_SaveAs
            // 
            this.tsmiFile_SaveAs.Enabled = false;
            this.tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            this.tsmiFile_SaveAs.ShortcutKeys = ((System.Windows.Forms.Keys) (((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
            | System.Windows.Forms.Keys.S)));
            this.tsmiFile_SaveAs.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_SaveAs.Text = "Save &As...";
            this.tsmiFile_SaveAs.Click += new System.EventHandler(this.tsmiFile_SaveAs_Click);
            // 
            // tsSeparator_File1
            // 
            this.tsSeparator_File1.Name = "tsSeparator_File1";
            this.tsSeparator_File1.Size = new System.Drawing.Size(201, 6);
            // 
            // tsmiFile_Close
            // 
            this.tsmiFile_Close.Enabled = false;
            this.tsmiFile_Close.Name = "tsmiFile_Close";
            this.tsmiFile_Close.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.tsmiFile_Close.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_Close.Text = "&Close";
            this.tsmiFile_Close.Click += new System.EventHandler(this.tsmiFile_Close_Click);
            // 
            // tsSeparator_File2
            // 
            this.tsSeparator_File2.Name = "tsSeparator_File2";
            this.tsSeparator_File2.Size = new System.Drawing.Size(201, 6);
            // 
            // tsmiFile_ApplyDFRFile
            // 
            this.tsmiFile_ApplyDFRFile.Enabled = false;
            this.tsmiFile_ApplyDFRFile.Name = "tsmiFile_ApplyDFRFile";
            this.tsmiFile_ApplyDFRFile.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.tsmiFile_ApplyDFRFile.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_ApplyDFRFile.Text = "Apply DFR File...";
            this.tsmiFile_ApplyDFRFile.Click += new System.EventHandler(this.tsmiFile_applyDFRFile_Click);
            // 
            // tsmiFile_GenerateDFRFile
            // 
            this.tsmiFile_GenerateDFRFile.Enabled = false;
            this.tsmiFile_GenerateDFRFile.Name = "tsmiFile_GenerateDFRFile";
            this.tsmiFile_GenerateDFRFile.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.tsmiFile_GenerateDFRFile.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_GenerateDFRFile.Text = "Create DFR File...";
            this.tsmiFile_GenerateDFRFile.Click += new System.EventHandler(this.tsmiFile_generateDFRFile_Click);
            // 
            // tsSeparator_File3
            // 
            this.tsSeparator_File3.Name = "tsSeparator_File3";
            this.tsSeparator_File3.Size = new System.Drawing.Size(201, 6);
            // 
            // tsmiFile_CopyTablesTo
            // 
            this.tsmiFile_CopyTablesTo.Enabled = false;
            this.tsmiFile_CopyTablesTo.Name = "tsmiFile_CopyTablesTo";
            this.tsmiFile_CopyTablesTo.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_CopyTablesTo.Text = "Copy Tables To...";
            this.tsmiFile_CopyTablesTo.Click += new System.EventHandler(this.tsmiFile_CopyTablesTo_Click);
            // 
            // tsmiFile_CopyTablesFrom
            // 
            this.tsmiFile_CopyTablesFrom.Enabled = false;
            this.tsmiFile_CopyTablesFrom.Name = "tsmiFile_CopyTablesFrom";
            this.tsmiFile_CopyTablesFrom.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_CopyTablesFrom.Text = "Copy Tables From...";
            this.tsmiFile_CopyTablesFrom.Click += new System.EventHandler(this.tsmiFile_CopyTablesFrom_Click);
            // 
            // tsSeparator_File4
            // 
            this.tsSeparator_File4.Name = "tsSeparator_File4";
            this.tsSeparator_File4.Size = new System.Drawing.Size(201, 6);
            // 
            // tsmiFile_Exit
            // 
            this.tsmiFile_Exit.Name = "tsmiFile_Exit";
            this.tsmiFile_Exit.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.tsmiFile_Exit.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_Exit.Text = "E&xit";
            this.tsmiFile_Exit.Click += new System.EventHandler(this.tsmiFile_Exit_Click);
            // 
            // tsmiEdit
            // 
            this.tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEdit_UseDropdowns});
            this.tsmiEdit.Name = "tsmiEdit";
            this.tsmiEdit.Size = new System.Drawing.Size(39, 20);
            this.tsmiEdit.Text = "&Edit";
            // 
            // tsmiEdit_UseDropdowns
            // 
            this.tsmiEdit_UseDropdowns.Name = "tsmiEdit_UseDropdowns";
            this.tsmiEdit_UseDropdowns.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.tsmiEdit_UseDropdowns.Size = new System.Drawing.Size(295, 22);
            this.tsmiEdit_UseDropdowns.Text = "Use &Dropdowns for Named Values";
            this.tsmiEdit_UseDropdowns.Click += new System.EventHandler(this.tsmiEdit_UseDropdowns_Click);
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
            this.tsmiScenario_Scenario1.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.tsmiScenario_Scenario1.Size = new System.Drawing.Size(188, 22);
            this.tsmiScenario_Scenario1.Text = "Scenario &1";
            this.tsmiScenario_Scenario1.Click += new System.EventHandler(this.tsmiScenario_Scenario1_Click);
            // 
            // tsmiScenario_Scenario2
            // 
            this.tsmiScenario_Scenario2.Name = "tsmiScenario_Scenario2";
            this.tsmiScenario_Scenario2.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.tsmiScenario_Scenario2.Size = new System.Drawing.Size(188, 22);
            this.tsmiScenario_Scenario2.Text = "Scenario &2";
            this.tsmiScenario_Scenario2.Click += new System.EventHandler(this.tsmiScenario_Scenario2_Click);
            // 
            // tsmiScenario_Scenario3
            // 
            this.tsmiScenario_Scenario3.Name = "tsmiScenario_Scenario3";
            this.tsmiScenario_Scenario3.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.tsmiScenario_Scenario3.Size = new System.Drawing.Size(188, 22);
            this.tsmiScenario_Scenario3.Text = "Scenario &3";
            this.tsmiScenario_Scenario3.Click += new System.EventHandler(this.tsmiScenario_Scenario3_Click);
            // 
            // tsmiScenario_PremiumDisk
            // 
            this.tsmiScenario_PremiumDisk.Name = "tsmiScenario_PremiumDisk";
            this.tsmiScenario_PremiumDisk.ShortcutKeys = ((System.Windows.Forms.Keys) ((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.tsmiScenario_PremiumDisk.Size = new System.Drawing.Size(188, 22);
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
            this.tsmiHelp_Version.Size = new System.Drawing.Size(190, 22);
            this.tsmiHelp_Version.Text = "Version (set internally)";
            // 
            // tsmiFile_OpenPrevious
            // 
            this.tsmiFile_OpenPrevious.Enabled = false;
            this.tsmiFile_OpenPrevious.Name = "tsmiFile_OpenPrevious";
            this.tsmiFile_OpenPrevious.ShortcutKeys = ((System.Windows.Forms.Keys) (((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
            | System.Windows.Forms.Keys.Oemcomma)));
            this.tsmiFile_OpenPrevious.ShowShortcutKeys = false;
            this.tsmiFile_OpenPrevious.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_OpenPrevious.Text = "Open Previous";
            this.tsmiFile_OpenPrevious.Visible = false;
            this.tsmiFile_OpenPrevious.Click += new System.EventHandler(this.tsmiFile_OpenPrevious_Click);
            // 
            // tsmiFile_OpenNext
            // 
            this.tsmiFile_OpenNext.Enabled = false;
            this.tsmiFile_OpenNext.Name = "tsmiFile_OpenNext";
            this.tsmiFile_OpenNext.ShortcutKeys = ((System.Windows.Forms.Keys) (((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt)
            | System.Windows.Forms.Keys.OemPeriod)));
            this.tsmiFile_OpenNext.ShowShortcutKeys = false;
            this.tsmiFile_OpenNext.Size = new System.Drawing.Size(204, 22);
            this.tsmiFile_OpenNext.Text = "Open Next";
            this.tsmiFile_OpenNext.Visible = false;
            this.tsmiFile_OpenNext.Click += new System.EventHandler(this.tsmiFile_OpenNext_Click);
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
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_ApplyDFRFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_GenerateDFRFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_CopyTablesTo;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File4;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Save;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit_UseDropdowns;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenPrevious;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenNext;
    }
}
