using System.Drawing;

namespace SF3.Editor.Forms {
    partial class frmSF3Editor {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSF3Editor));
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario_Detect = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_OpenScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_SwapToPrev = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SwapToNext = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep3 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit_UseDropdowns = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit_EnableDebugSettings = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_Tools = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_ImportTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_ExportTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiTools_ApplyDFR = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_CreateDFR = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_ImportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_ExportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_DeleteChunk = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiMPD_Chunks_RecompressChunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_CorrectChunkPlacement = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiMPD_Chunks_MoveChunksAutomatically = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures_ImportAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures_ExportAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiMPD_EnableBlankFieldV2Controls = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile, tsmiEdit, tsmi_Tools, tsmiMPD, tsmiHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(891, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_Open, tsmiFile_OpenScenario, tsmiFile_Save, tsmiFile_SaveAs, tsmiFile_Sep1, tsmiFile_Close, tsmiFile_Sep2, tsmiFile_SwapToPrev, tsmiFile_SwapToNext, tsmiFile_Sep3, tsmiFile_Exit });
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiFile_Open
            // 
            tsmiFile_Open.Name = "tsmiFile_Open";
            tsmiFile_Open.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            tsmiFile_Open.Size = new Size(259, 22);
            tsmiFile_Open.Text = "&Open...";
            tsmiFile_Open.Click += tsmiFile_Open_Click;
            // 
            // tsmiFile_OpenScenario
            // 
            tsmiFile_OpenScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_OpenScenario_Detect, tsmiFile_OpenScenario_Sep1, tsmiFile_OpenScenario_Scenario1, tsmiFile_OpenScenario_Scenario2, tsmiFile_OpenScenario_Scenario3, tsmiFile_OpenScenario_PremiumDisk });
            tsmiFile_OpenScenario.Name = "tsmiFile_OpenScenario";
            tsmiFile_OpenScenario.Size = new Size(259, 22);
            tsmiFile_OpenScenario.Text = "Open Sce&nario";
            // 
            // tsmiFile_OpenScenario_Detect
            // 
            tsmiFile_OpenScenario_Detect.Name = "tsmiFile_OpenScenario_Detect";
            tsmiFile_OpenScenario_Detect.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0;
            tsmiFile_OpenScenario_Detect.Size = new Size(249, 22);
            tsmiFile_OpenScenario_Detect.Text = "&Detect or Based on Folder";
            tsmiFile_OpenScenario_Detect.Click += tsmiFile_OpenScenario_Detect_Click;
            // 
            // tsmiFile_OpenScenario_Sep1
            // 
            tsmiFile_OpenScenario_Sep1.Name = "tsmiFile_OpenScenario_Sep1";
            tsmiFile_OpenScenario_Sep1.Size = new Size(246, 6);
            // 
            // tsmiFile_OpenScenario_Scenario1
            // 
            tsmiFile_OpenScenario_Scenario1.Name = "tsmiFile_OpenScenario_Scenario1";
            tsmiFile_OpenScenario_Scenario1.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1;
            tsmiFile_OpenScenario_Scenario1.Size = new Size(249, 22);
            tsmiFile_OpenScenario_Scenario1.Text = "Scenario &1";
            tsmiFile_OpenScenario_Scenario1.Click += tsmiFile_OpenScenario_Scenario1_Click;
            // 
            // tsmiFile_OpenScenario_Scenario2
            // 
            tsmiFile_OpenScenario_Scenario2.Name = "tsmiFile_OpenScenario_Scenario2";
            tsmiFile_OpenScenario_Scenario2.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2;
            tsmiFile_OpenScenario_Scenario2.Size = new Size(249, 22);
            tsmiFile_OpenScenario_Scenario2.Text = "Scenario &2";
            tsmiFile_OpenScenario_Scenario2.Click += tsmiFile_OpenScenario_Scenario2_Click;
            // 
            // tsmiFile_OpenScenario_Scenario3
            // 
            tsmiFile_OpenScenario_Scenario3.Name = "tsmiFile_OpenScenario_Scenario3";
            tsmiFile_OpenScenario_Scenario3.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3;
            tsmiFile_OpenScenario_Scenario3.Size = new Size(249, 22);
            tsmiFile_OpenScenario_Scenario3.Text = "Scenario &3";
            tsmiFile_OpenScenario_Scenario3.Click += tsmiFile_OpenScenario_Scenario3_Click;
            // 
            // tsmiFile_OpenScenario_PremiumDisk
            // 
            tsmiFile_OpenScenario_PremiumDisk.Name = "tsmiFile_OpenScenario_PremiumDisk";
            tsmiFile_OpenScenario_PremiumDisk.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4;
            tsmiFile_OpenScenario_PremiumDisk.Size = new Size(249, 22);
            tsmiFile_OpenScenario_PremiumDisk.Text = "&Premium Disk";
            tsmiFile_OpenScenario_PremiumDisk.Click += tsmiFile_OpenScenario_PremiumDisk_Click;
            // 
            // tsmiFile_Save
            // 
            tsmiFile_Save.Enabled = false;
            tsmiFile_Save.Name = "tsmiFile_Save";
            tsmiFile_Save.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            tsmiFile_Save.Size = new Size(259, 22);
            tsmiFile_Save.Text = "&Save";
            tsmiFile_Save.Click += tsmiFile_Save_Click;
            // 
            // tsmiFile_SaveAs
            // 
            tsmiFile_SaveAs.Enabled = false;
            tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            tsmiFile_SaveAs.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            tsmiFile_SaveAs.Size = new Size(259, 22);
            tsmiFile_SaveAs.Text = "Save &As...";
            tsmiFile_SaveAs.Click += tsmiFile_SaveAs_Click;
            // 
            // tsmiFile_Sep1
            // 
            tsmiFile_Sep1.Name = "tsmiFile_Sep1";
            tsmiFile_Sep1.Size = new Size(256, 6);
            // 
            // tsmiFile_Close
            // 
            tsmiFile_Close.Enabled = false;
            tsmiFile_Close.Name = "tsmiFile_Close";
            tsmiFile_Close.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            tsmiFile_Close.Size = new Size(259, 22);
            tsmiFile_Close.Text = "&Close";
            tsmiFile_Close.Click += tsmiFile_Close_Click;
            // 
            // tsmiFile_Sep2
            // 
            tsmiFile_Sep2.Name = "tsmiFile_Sep2";
            tsmiFile_Sep2.Size = new Size(256, 6);
            // 
            // tsmiFile_SwapToPrev
            // 
            tsmiFile_SwapToPrev.Enabled = false;
            tsmiFile_SwapToPrev.Name = "tsmiFile_SwapToPrev";
            tsmiFile_SwapToPrev.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Oemcomma;
            tsmiFile_SwapToPrev.ShowShortcutKeys = false;
            tsmiFile_SwapToPrev.Size = new Size(259, 22);
            tsmiFile_SwapToPrev.Text = "Swap to Prev of Same Type in Folder";
            tsmiFile_SwapToPrev.Click += tsmiFile_SwapToPrev_Click;
            // 
            // tsmiFile_SwapToNext
            // 
            tsmiFile_SwapToNext.Enabled = false;
            tsmiFile_SwapToNext.Name = "tsmiFile_SwapToNext";
            tsmiFile_SwapToNext.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.OemPeriod;
            tsmiFile_SwapToNext.ShowShortcutKeys = false;
            tsmiFile_SwapToNext.Size = new Size(259, 22);
            tsmiFile_SwapToNext.Text = "Swap to Next of Same Type in Folder";
            tsmiFile_SwapToNext.Click += tsmiFile_SwapToNext_Click;
            // 
            // tsmiFile_Sep3
            // 
            tsmiFile_Sep3.Name = "tsmiFile_Sep3";
            tsmiFile_Sep3.Size = new Size(256, 6);
            // 
            // tsmiFile_Exit
            // 
            tsmiFile_Exit.Name = "tsmiFile_Exit";
            tsmiFile_Exit.ShortcutKeys =  System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            tsmiFile_Exit.Size = new Size(259, 22);
            tsmiFile_Exit.Text = "E&xit";
            tsmiFile_Exit.Click += tsmiFile_Exit_Click;
            // 
            // tsmiEdit
            // 
            tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiEdit_UseDropdowns, tsmiEdit_EnableDebugSettings });
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new Size(39, 20);
            tsmiEdit.Text = "&Edit";
            // 
            // tsmiEdit_UseDropdowns
            // 
            tsmiEdit_UseDropdowns.Name = "tsmiEdit_UseDropdowns";
            tsmiEdit_UseDropdowns.Size = new Size(253, 22);
            tsmiEdit_UseDropdowns.Text = "Use &Dropdowns for Named Values";
            tsmiEdit_UseDropdowns.Click += tsmiEdit_UseDropdowns_Click;
            // 
            // tsmiEdit_EnableDebugSettings
            // 
            tsmiEdit_EnableDebugSettings.Name = "tsmiEdit_EnableDebugSettings";
            tsmiEdit_EnableDebugSettings.Size = new Size(253, 22);
            tsmiEdit_EnableDebugSettings.Text = "Enable Debu&g Settings";
            tsmiEdit_EnableDebugSettings.Click += tsmiEdit_EnableDebugSettings_Click;
            // 
            // tsmi_Tools
            // 
            tsmi_Tools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiTools_ImportTable, tsmiTools_ExportTable, tsmiTools_Sep1, tsmiTools_ApplyDFR, tsmiTools_CreateDFR });
            tsmi_Tools.Name = "tsmi_Tools";
            tsmi_Tools.Size = new Size(47, 20);
            tsmi_Tools.Text = "&Tools";
            // 
            // tsmiTools_ImportTable
            // 
            tsmiTools_ImportTable.Enabled = false;
            tsmiTools_ImportTable.Name = "tsmiTools_ImportTable";
            tsmiTools_ImportTable.Size = new Size(178, 22);
            tsmiTools_ImportTable.Text = "Copy Tables &From...";
            tsmiTools_ImportTable.Click += tsmiTools_ImportTable_Click;
            // 
            // tsmiTools_ExportTable
            // 
            tsmiTools_ExportTable.Enabled = false;
            tsmiTools_ExportTable.Name = "tsmiTools_ExportTable";
            tsmiTools_ExportTable.Size = new Size(178, 22);
            tsmiTools_ExportTable.Text = "Copy Tables &To...";
            tsmiTools_ExportTable.Click += tsmiTools_ExportTable_Click;
            // 
            // tsmiTools_Sep1
            // 
            tsmiTools_Sep1.Name = "tsmiTools_Sep1";
            tsmiTools_Sep1.Size = new Size(175, 6);
            // 
            // tsmiTools_ApplyDFR
            // 
            tsmiTools_ApplyDFR.Enabled = false;
            tsmiTools_ApplyDFR.Name = "tsmiTools_ApplyDFR";
            tsmiTools_ApplyDFR.Size = new Size(178, 22);
            tsmiTools_ApplyDFR.Text = "&Apply DFR File...";
            tsmiTools_ApplyDFR.Click += tsmiTools_ApplyDFR_Click;
            // 
            // tsmiTools_CreateDFR
            // 
            tsmiTools_CreateDFR.Enabled = false;
            tsmiTools_CreateDFR.Name = "tsmiTools_CreateDFR";
            tsmiTools_CreateDFR.Size = new Size(178, 22);
            tsmiTools_CreateDFR.Text = "&Create DFR File...";
            tsmiTools_CreateDFR.Click += tsmiTools_CreateDFR_Click;
            // 
            // tsmiMPD
            // 
            tsmiMPD.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Chunks, tsmiMPD_Textures, tsmiMPD_Sep1, tsmiMPD_EnableBlankFieldV2Controls });
            tsmiMPD.Enabled = false;
            tsmiMPD.Name = "tsmiMPD";
            tsmiMPD.Size = new Size(45, 20);
            tsmiMPD.Text = "&MPD";
            tsmiMPD.Visible = false;
            // 
            // tsmiMPD_Chunks
            // 
            tsmiMPD_Chunks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Chunks_ImportChunk, tsmiMPD_Chunks_ExportChunk, tsmiMPD_Chunks_DeleteChunk, toolStripSeparator1, tsmiMPD_Chunks_RecompressChunks, tsmiMPD_Chunks_CorrectChunkPlacement, toolStripSeparator2, tsmiMPD_Chunks_MoveChunksAutomatically });
            tsmiMPD_Chunks.Name = "tsmiMPD_Chunks";
            tsmiMPD_Chunks.Size = new Size(384, 22);
            tsmiMPD_Chunks.Text = "&Chunks";
            tsmiMPD_Chunks.Visible = false;
            // 
            // tsmiMPD_Chunks_ImportChunk
            // 
            tsmiMPD_Chunks_ImportChunk.Name = "tsmiMPD_Chunks_ImportChunk";
            tsmiMPD_Chunks_ImportChunk.Size = new Size(224, 22);
            tsmiMPD_Chunks_ImportChunk.Text = "&Import Chunk...";
            tsmiMPD_Chunks_ImportChunk.Click += tsmiMPD_Chunks_ImportChunk_Click;
            // 
            // tsmiMPD_Chunks_ExportChunk
            // 
            tsmiMPD_Chunks_ExportChunk.Name = "tsmiMPD_Chunks_ExportChunk";
            tsmiMPD_Chunks_ExportChunk.Size = new Size(224, 22);
            tsmiMPD_Chunks_ExportChunk.Text = "&Export Chunk...";
            tsmiMPD_Chunks_ExportChunk.Click += tsmiMPD_Chunks_ExportChunk_Click;
            // 
            // tsmiMPD_Chunks_DeleteChunk
            // 
            tsmiMPD_Chunks_DeleteChunk.Name = "tsmiMPD_Chunks_DeleteChunk";
            tsmiMPD_Chunks_DeleteChunk.Size = new Size(224, 22);
            tsmiMPD_Chunks_DeleteChunk.Text = "&Delete Chunk...";
            tsmiMPD_Chunks_DeleteChunk.Click += tsmiMPD_Chunks_DeleteChunk_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(221, 6);
            // 
            // tsmiMPD_Chunks_RecompressChunks
            // 
            tsmiMPD_Chunks_RecompressChunks.Enabled = false;
            tsmiMPD_Chunks_RecompressChunks.Name = "tsmiMPD_Chunks_RecompressChunks";
            tsmiMPD_Chunks_RecompressChunks.Size = new Size(224, 22);
            tsmiMPD_Chunks_RecompressChunks.Text = "Recompress Chunks";
            // 
            // tsmiMPD_Chunks_CorrectChunkPlacement
            // 
            tsmiMPD_Chunks_CorrectChunkPlacement.Enabled = false;
            tsmiMPD_Chunks_CorrectChunkPlacement.Name = "tsmiMPD_Chunks_CorrectChunkPlacement";
            tsmiMPD_Chunks_CorrectChunkPlacement.Size = new Size(224, 22);
            tsmiMPD_Chunks_CorrectChunkPlacement.Text = "Correct Chunk Placement";
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(221, 6);
            // 
            // tsmiMPD_Chunks_MoveChunksAutomatically
            // 
            tsmiMPD_Chunks_MoveChunksAutomatically.Checked = true;
            tsmiMPD_Chunks_MoveChunksAutomatically.CheckState = System.Windows.Forms.CheckState.Checked;
            tsmiMPD_Chunks_MoveChunksAutomatically.Enabled = false;
            tsmiMPD_Chunks_MoveChunksAutomatically.Name = "tsmiMPD_Chunks_MoveChunksAutomatically";
            tsmiMPD_Chunks_MoveChunksAutomatically.Size = new Size(224, 22);
            tsmiMPD_Chunks_MoveChunksAutomatically.Text = "Move Chunks Automatically";
            // 
            // tsmiMPD_Textures
            // 
            tsmiMPD_Textures.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Textures_ImportAll, tsmiMPD_Textures_ExportAll });
            tsmiMPD_Textures.Name = "tsmiMPD_Textures";
            tsmiMPD_Textures.Size = new Size(384, 22);
            tsmiMPD_Textures.Text = "&Textures";
            // 
            // tsmiMPD_Textures_ImportAll
            // 
            tsmiMPD_Textures_ImportAll.Name = "tsmiMPD_Textures_ImportAll";
            tsmiMPD_Textures_ImportAll.Size = new Size(221, 22);
            tsmiMPD_Textures_ImportAll.Text = "&Import All (Replaces Only)...";
            tsmiMPD_Textures_ImportAll.Click += tsmiMPD_Textures_ImportAll_Click;
            // 
            // tsmiMPD_Textures_ExportAll
            // 
            tsmiMPD_Textures_ExportAll.Name = "tsmiMPD_Textures_ExportAll";
            tsmiMPD_Textures_ExportAll.Size = new Size(221, 22);
            tsmiMPD_Textures_ExportAll.Text = "&Export All...";
            tsmiMPD_Textures_ExportAll.Click += tsmiMPD_Textures_ExportAll_Click;
            // 
            // tsmiMPD_Sep1
            // 
            tsmiMPD_Sep1.Name = "tsmiMPD_Sep1";
            tsmiMPD_Sep1.Size = new Size(381, 6);
            // 
            // tsmiMPD_EnableBlankFieldV2Controls
            // 
            tsmiMPD_EnableBlankFieldV2Controls.Name = "tsmiMPD_EnableBlankFieldV2Controls";
            tsmiMPD_EnableBlankFieldV2Controls.Size = new Size(384, 22);
            tsmiMPD_EnableBlankFieldV2Controls.Text = "E&XPERIMENTAL: Enable tile controls for BlankField_V2.MPD";
            tsmiMPD_EnableBlankFieldV2Controls.Click += tsmiMPD_EnableBlankFieldV2Controls_Click;
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_About });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_About
            // 
            tsmiHelp_About.Name = "tsmiHelp_About";
            tsmiHelp_About.Size = new Size(116, 22);
            tsmiHelp_About.Text = "&About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // frmSF3Editor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(891, 631);
            Controls.Add(menuStrip1);
            Icon = (Icon) resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "frmSF3Editor";
            Text = "SF3 Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Open;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Save;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Close;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Exit;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario_Detect;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_OpenScenario_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario_Scenario1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario_Scenario2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario_Scenario3;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario_PremiumDisk;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_About;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit_UseDropdowns;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Tools;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ApplyDFR;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_CreateDFR;
        private System.Windows.Forms.ToolStripSeparator tsmiTools_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ImportTable;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ExportTable;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit_EnableDebugSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures_ImportAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures_ExportAll;
        private System.Windows.Forms.ToolStripSeparator tsmiMPD_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_EnableBlankFieldV2Controls;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_ExportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_ImportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_DeleteChunk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_RecompressChunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_CorrectChunkPlacement;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_MoveChunksAutomatically;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep3;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SwapToPrev;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SwapToNext;
    }
}
