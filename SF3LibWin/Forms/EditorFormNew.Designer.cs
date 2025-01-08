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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenPrevious = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenNext = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            tsSeparator_File1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            tsSeparator_File2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_ApplyDFRFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_GenerateDFRFile = new System.Windows.Forms.ToolStripMenuItem();
            tsSeparator_File3 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_CopyTablesTo = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_CopyTablesFrom = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_CopyTablesSeparator = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit_UseDropdowns = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile, tsmiEdit, tsmiScenario, tsmiHelp });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new System.Drawing.Size(284, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_Open, tsmiFile_OpenPrevious, tsmiFile_OpenNext, tsmiFile_Save, tsmiFile_SaveAs, tsSeparator_File1, tsmiFile_Close, tsSeparator_File2, tsmiFile_ApplyDFRFile, tsmiFile_GenerateDFRFile, tsSeparator_File3, tsmiFile_CopyTablesTo, tsmiFile_CopyTablesFrom, tsmiFile_CopyTablesSeparator, tsmiFile_Exit });
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new System.Drawing.Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiFile_Open
            // 
            tsmiFile_Open.Name = "tsmiFile_Open";
            tsmiFile_Open.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            tsmiFile_Open.Size = new System.Drawing.Size(339, 22);
            tsmiFile_Open.Text = "&Open...";
            tsmiFile_Open.Click += tsmiFile_Open_Click;
            // 
            // tsmiFile_OpenPrevious
            // 
            tsmiFile_OpenPrevious.Enabled = false;
            tsmiFile_OpenPrevious.Name = "tsmiFile_OpenPrevious";
            tsmiFile_OpenPrevious.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Oemcomma;
            tsmiFile_OpenPrevious.Size = new System.Drawing.Size(339, 22);
            tsmiFile_OpenPrevious.Text = "Open Previous in Directory";
            tsmiFile_OpenPrevious.Click += tsmiFile_OpenPrevious_Click;
            // 
            // tsmiFile_OpenNext
            // 
            tsmiFile_OpenNext.Enabled = false;
            tsmiFile_OpenNext.Name = "tsmiFile_OpenNext";
            tsmiFile_OpenNext.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.OemPeriod;
            tsmiFile_OpenNext.Size = new System.Drawing.Size(339, 22);
            tsmiFile_OpenNext.Text = "Open Next in Directory";
            tsmiFile_OpenNext.Click += tsmiFile_OpenNext_Click;
            // 
            // tsmiFile_Save
            // 
            tsmiFile_Save.Enabled = false;
            tsmiFile_Save.Name = "tsmiFile_Save";
            tsmiFile_Save.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            tsmiFile_Save.Size = new System.Drawing.Size(339, 22);
            tsmiFile_Save.Text = "&Save";
            tsmiFile_Save.Click += tsmiFile_Save_Click;
            // 
            // tsmiFile_SaveAs
            // 
            tsmiFile_SaveAs.Enabled = false;
            tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            tsmiFile_SaveAs.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.S;
            tsmiFile_SaveAs.Size = new System.Drawing.Size(339, 22);
            tsmiFile_SaveAs.Text = "Save &As...";
            tsmiFile_SaveAs.Click += tsmiFile_SaveAs_Click;
            // 
            // tsSeparator_File1
            // 
            tsSeparator_File1.Name = "tsSeparator_File1";
            tsSeparator_File1.Size = new System.Drawing.Size(336, 6);
            // 
            // tsmiFile_Close
            // 
            tsmiFile_Close.Enabled = false;
            tsmiFile_Close.Name = "tsmiFile_Close";
            tsmiFile_Close.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            tsmiFile_Close.Size = new System.Drawing.Size(339, 22);
            tsmiFile_Close.Text = "&Close";
            tsmiFile_Close.Click += tsmiFile_Close_Click;
            // 
            // tsSeparator_File2
            // 
            tsSeparator_File2.Name = "tsSeparator_File2";
            tsSeparator_File2.Size = new System.Drawing.Size(336, 6);
            // 
            // tsmiFile_ApplyDFRFile
            // 
            tsmiFile_ApplyDFRFile.Enabled = false;
            tsmiFile_ApplyDFRFile.Name = "tsmiFile_ApplyDFRFile";
            tsmiFile_ApplyDFRFile.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.A;
            tsmiFile_ApplyDFRFile.Size = new System.Drawing.Size(339, 22);
            tsmiFile_ApplyDFRFile.Text = "Apply DFR File...";
            tsmiFile_ApplyDFRFile.Click += tsmiFile_applyDFRFile_Click;
            // 
            // tsmiFile_GenerateDFRFile
            // 
            tsmiFile_GenerateDFRFile.Enabled = false;
            tsmiFile_GenerateDFRFile.Name = "tsmiFile_GenerateDFRFile";
            tsmiFile_GenerateDFRFile.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.C;
            tsmiFile_GenerateDFRFile.Size = new System.Drawing.Size(339, 22);
            tsmiFile_GenerateDFRFile.Text = "Create DFR File...";
            tsmiFile_GenerateDFRFile.Click += tsmiFile_generateDFRFile_Click;
            // 
            // tsSeparator_File3
            // 
            tsSeparator_File3.Name = "tsSeparator_File3";
            tsSeparator_File3.Size = new System.Drawing.Size(336, 6);
            // 
            // tsmiFile_CopyTablesTo
            // 
            tsmiFile_CopyTablesTo.Enabled = false;
            tsmiFile_CopyTablesTo.Name = "tsmiFile_CopyTablesTo";
            tsmiFile_CopyTablesTo.Size = new System.Drawing.Size(339, 22);
            tsmiFile_CopyTablesTo.Text = "Copy Tables To...";
            tsmiFile_CopyTablesTo.Click += tsmiFile_CopyTablesTo_Click;
            // 
            // tsmiFile_CopyTablesFrom
            // 
            tsmiFile_CopyTablesFrom.Enabled = false;
            tsmiFile_CopyTablesFrom.Name = "tsmiFile_CopyTablesFrom";
            tsmiFile_CopyTablesFrom.Size = new System.Drawing.Size(339, 22);
            tsmiFile_CopyTablesFrom.Text = "Copy Tables From...";
            tsmiFile_CopyTablesFrom.Click += tsmiFile_CopyTablesFrom_Click;
            // 
            // tsmiFile_CopyTablesSeparator
            // 
            tsmiFile_CopyTablesSeparator.Name = "tsmiFile_CopyTablesSeparator";
            tsmiFile_CopyTablesSeparator.Size = new System.Drawing.Size(336, 6);
            // 
            // tsmiFile_Exit
            // 
            tsmiFile_Exit.Name = "tsmiFile_Exit";
            tsmiFile_Exit.ShortcutKeys =  System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            tsmiFile_Exit.Size = new System.Drawing.Size(339, 22);
            tsmiFile_Exit.Text = "E&xit";
            tsmiFile_Exit.Click += tsmiFile_Exit_Click;
            // 
            // tsmiEdit
            // 
            tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiEdit_UseDropdowns });
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new System.Drawing.Size(39, 20);
            tsmiEdit.Text = "&Edit";
            // 
            // tsmiEdit_UseDropdowns
            // 
            tsmiEdit_UseDropdowns.Name = "tsmiEdit_UseDropdowns";
            tsmiEdit_UseDropdowns.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.D;
            tsmiEdit_UseDropdowns.Size = new System.Drawing.Size(327, 22);
            tsmiEdit_UseDropdowns.Text = "Use &Dropdowns for Named Values";
            tsmiEdit_UseDropdowns.Click += tsmiEdit_UseDropdowns_Click;
            // 
            // tsmiScenario
            // 
            tsmiScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiScenario_Scenario1, tsmiScenario_Scenario2, tsmiScenario_Scenario3, tsmiScenario_PremiumDisk });
            tsmiScenario.Name = "tsmiScenario";
            tsmiScenario.Size = new System.Drawing.Size(64, 20);
            tsmiScenario.Text = "&Scenario";
            // 
            // tsmiScenario_Scenario1
            // 
            tsmiScenario_Scenario1.Name = "tsmiScenario_Scenario1";
            tsmiScenario_Scenario1.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1;
            tsmiScenario_Scenario1.Size = new System.Drawing.Size(188, 22);
            tsmiScenario_Scenario1.Text = "Scenario &1";
            tsmiScenario_Scenario1.Click += tsmiScenario_Scenario1_Click;
            // 
            // tsmiScenario_Scenario2
            // 
            tsmiScenario_Scenario2.Name = "tsmiScenario_Scenario2";
            tsmiScenario_Scenario2.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2;
            tsmiScenario_Scenario2.Size = new System.Drawing.Size(188, 22);
            tsmiScenario_Scenario2.Text = "Scenario &2";
            tsmiScenario_Scenario2.Click += tsmiScenario_Scenario2_Click;
            // 
            // tsmiScenario_Scenario3
            // 
            tsmiScenario_Scenario3.Name = "tsmiScenario_Scenario3";
            tsmiScenario_Scenario3.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3;
            tsmiScenario_Scenario3.Size = new System.Drawing.Size(188, 22);
            tsmiScenario_Scenario3.Text = "Scenario &3";
            tsmiScenario_Scenario3.Click += tsmiScenario_Scenario3_Click;
            // 
            // tsmiScenario_PremiumDisk
            // 
            tsmiScenario_PremiumDisk.Name = "tsmiScenario_PremiumDisk";
            tsmiScenario_PremiumDisk.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4;
            tsmiScenario_PremiumDisk.Size = new System.Drawing.Size(188, 22);
            tsmiScenario_PremiumDisk.Text = "&Premium Disk";
            tsmiScenario_PremiumDisk.Click += tsmiScenario_PremiumDisk_Click;
            // 
            // tsmiHelp
            // 
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new System.Drawing.Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // EditorFormNew
            // 
            ClientSize = new System.Drawing.Size(284, 261);
            Controls.Add(menuStrip1);
            Name = "EditorFormNew";
            FormClosing += EditorForm_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_ApplyDFRFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_GenerateDFRFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_CopyTablesTo;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_CopyTablesSeparator;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Save;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit_UseDropdowns;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenPrevious;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenNext;
    }
}
