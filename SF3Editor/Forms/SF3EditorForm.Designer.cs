using System.Drawing;

namespace SF3.Editor.Forms {
    partial class SF3EditorForm {
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SF3EditorForm));
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
            tsmiFile_SaveAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_CloseAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_SwapToPrev = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SwapToNext = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep3 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_ScanForErrors = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep4 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_RecentFiles = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_1 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_2 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_3 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_4 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_5 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_6 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_7 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_8 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_9 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_RecentFiles_10 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep5 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawSurfaceModel = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawModels = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawGround = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawSkyBox = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_RunAnimations = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_ApplyLighting = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawGradients = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiView_MPD_DrawWireframes = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawBoundaries = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawTerrainTypes = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawEventIDs = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawCollisionLines = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_HideModelsNotFacingCamera = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_RenderOnBlackBackground = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_DrawNormalMap = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_RotateSpritesUpToCamera = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiView_MPD_ShowHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_MPD_Sep3 = new System.Windows.Forms.ToolStripSeparator();
            tsmiView_MPD_EnableBlankFieldV2Controls = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_ImportTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_ExportTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiTools_ApplyDFR = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_CreateDFR = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiTools_MovePostEOFData = new System.Windows.Forms.ToolStripMenuItem();
            tsmiTools_InsertData = new System.Windows.Forms.ToolStripMenuItem();
            tsmiX019 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiX019_UnapplyMonsterEq = new System.Windows.Forms.ToolStripMenuItem();
            tsmiX019_ApplyMonsterEq = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_ImportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_ExportChunk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_DeleteChunk = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiMPD_Chunks_RecompressModifiedChunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_RecompressAllChunks = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Chunks_RebuildChunkTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures_ImportAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Textures_ExportAll = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_ModelSwitchGroups = new System.Windows.Forms.ToolStripMenuItem();
            tsmiMPD_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiMPD_RecalculateSurfaceModelNormals = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_UseDropdowns = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_EnableDebugSettings = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_ShowErrorsOnFileLoad = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiSettings_MPD = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_MPD_ImprovedNormalCalculations = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_MPD_UseFullHeightForNormals = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize = new System.Windows.Forms.ToolStripMenuItem();
            tsmiSettings_MPD_AutoRebuildMPDChunkTable = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            tsmiView_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiView_HighlightEndcodesInTextureViews = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile, tsmiView, tsmiTools, tsmiX019, tsmiMPD, tsmiSettings, tsmiHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(891, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_Open, tsmiFile_OpenScenario, tsmiFile_Save, tsmiFile_SaveAs, tsmiFile_SaveAll, tsmiFile_Sep1, tsmiFile_Close, tsmiFile_CloseAll, tsmiFile_Sep2, tsmiFile_SwapToPrev, tsmiFile_SwapToNext, tsmiFile_Sep3, tsmiFile_ScanForErrors, tsmiFile_Sep4, tsmiFile_RecentFiles, tsmiFile_Sep5, tsmiFile_Exit });
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
            // tsmiFile_SaveAll
            // 
            tsmiFile_SaveAll.Enabled = false;
            tsmiFile_SaveAll.Name = "tsmiFile_SaveAll";
            tsmiFile_SaveAll.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            tsmiFile_SaveAll.Size = new Size(259, 22);
            tsmiFile_SaveAll.Text = "Save A&ll";
            tsmiFile_SaveAll.Click += tsmiFile_SaveAll_Click;
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
            // tsmiFile_CloseAll
            // 
            tsmiFile_CloseAll.Enabled = false;
            tsmiFile_CloseAll.Name = "tsmiFile_CloseAll";
            tsmiFile_CloseAll.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.W;
            tsmiFile_CloseAll.Size = new Size(259, 22);
            tsmiFile_CloseAll.Text = "Close All";
            tsmiFile_CloseAll.Click += tsmiFile_CloseAll_Click;
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
            // tsmiFile_ScanForErrors
            // 
            tsmiFile_ScanForErrors.Enabled = false;
            tsmiFile_ScanForErrors.Name = "tsmiFile_ScanForErrors";
            tsmiFile_ScanForErrors.Size = new Size(259, 22);
            tsmiFile_ScanForErrors.Text = "Scan for &Errors...";
            tsmiFile_ScanForErrors.Click += tsmiFile_ScanForErrors_Click;
            // 
            // tsmiFile_Sep4
            // 
            tsmiFile_Sep4.Name = "tsmiFile_Sep4";
            tsmiFile_Sep4.Size = new Size(256, 6);
            // 
            // tsmiFile_RecentFiles
            // 
            tsmiFile_RecentFiles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_RecentFiles_1, tsmiFile_RecentFiles_2, tsmiFile_RecentFiles_3, tsmiFile_RecentFiles_4, tsmiFile_RecentFiles_5, tsmiFile_RecentFiles_6, tsmiFile_RecentFiles_7, tsmiFile_RecentFiles_8, tsmiFile_RecentFiles_9, tsmiFile_RecentFiles_10 });
            tsmiFile_RecentFiles.Name = "tsmiFile_RecentFiles";
            tsmiFile_RecentFiles.Size = new Size(259, 22);
            tsmiFile_RecentFiles.Text = "&Recent Files";
            // 
            // tsmiFile_RecentFiles_1
            // 
            tsmiFile_RecentFiles_1.Name = "tsmiFile_RecentFiles_1";
            tsmiFile_RecentFiles_1.Size = new Size(86, 22);
            tsmiFile_RecentFiles_1.Text = "&1";
            tsmiFile_RecentFiles_1.Click += tsmiFile_RecentFiles_1_Click;
            // 
            // tsmiFile_RecentFiles_2
            // 
            tsmiFile_RecentFiles_2.Name = "tsmiFile_RecentFiles_2";
            tsmiFile_RecentFiles_2.Size = new Size(86, 22);
            tsmiFile_RecentFiles_2.Text = "&2";
            tsmiFile_RecentFiles_2.Click += tsmiFile_RecentFiles_2_Click;
            // 
            // tsmiFile_RecentFiles_3
            // 
            tsmiFile_RecentFiles_3.Name = "tsmiFile_RecentFiles_3";
            tsmiFile_RecentFiles_3.Size = new Size(86, 22);
            tsmiFile_RecentFiles_3.Text = "&3";
            tsmiFile_RecentFiles_3.Click += tsmiFile_RecentFiles_3_Click;
            // 
            // tsmiFile_RecentFiles_4
            // 
            tsmiFile_RecentFiles_4.Name = "tsmiFile_RecentFiles_4";
            tsmiFile_RecentFiles_4.Size = new Size(86, 22);
            tsmiFile_RecentFiles_4.Text = "&4";
            tsmiFile_RecentFiles_4.Click += tsmiFile_RecentFiles_4_Click;
            // 
            // tsmiFile_RecentFiles_5
            // 
            tsmiFile_RecentFiles_5.Name = "tsmiFile_RecentFiles_5";
            tsmiFile_RecentFiles_5.Size = new Size(86, 22);
            tsmiFile_RecentFiles_5.Text = "&5";
            tsmiFile_RecentFiles_5.Click += tsmiFile_RecentFiles_5_Click;
            // 
            // tsmiFile_RecentFiles_6
            // 
            tsmiFile_RecentFiles_6.Name = "tsmiFile_RecentFiles_6";
            tsmiFile_RecentFiles_6.Size = new Size(86, 22);
            tsmiFile_RecentFiles_6.Text = "&6";
            tsmiFile_RecentFiles_6.Click += tsmiFile_RecentFiles_6_Click;
            // 
            // tsmiFile_RecentFiles_7
            // 
            tsmiFile_RecentFiles_7.Name = "tsmiFile_RecentFiles_7";
            tsmiFile_RecentFiles_7.Size = new Size(86, 22);
            tsmiFile_RecentFiles_7.Text = "&7";
            tsmiFile_RecentFiles_7.Click += tsmiFile_RecentFiles_7_Click;
            // 
            // tsmiFile_RecentFiles_8
            // 
            tsmiFile_RecentFiles_8.Name = "tsmiFile_RecentFiles_8";
            tsmiFile_RecentFiles_8.Size = new Size(86, 22);
            tsmiFile_RecentFiles_8.Text = "&8";
            tsmiFile_RecentFiles_8.Click += tsmiFile_RecentFiles_8_Click;
            // 
            // tsmiFile_RecentFiles_9
            // 
            tsmiFile_RecentFiles_9.Name = "tsmiFile_RecentFiles_9";
            tsmiFile_RecentFiles_9.Size = new Size(86, 22);
            tsmiFile_RecentFiles_9.Text = "&9";
            tsmiFile_RecentFiles_9.Click += tsmiFile_RecentFiles_9_Click;
            // 
            // tsmiFile_RecentFiles_10
            // 
            tsmiFile_RecentFiles_10.Name = "tsmiFile_RecentFiles_10";
            tsmiFile_RecentFiles_10.Size = new Size(86, 22);
            tsmiFile_RecentFiles_10.Text = "1&0";
            tsmiFile_RecentFiles_10.Click += tsmiFile_RecentFiles_10_Click;
            // 
            // tsmiFile_Sep5
            // 
            tsmiFile_Sep5.Name = "tsmiFile_Sep5";
            tsmiFile_Sep5.Size = new Size(256, 6);
            // 
            // tsmiFile_Exit
            // 
            tsmiFile_Exit.Name = "tsmiFile_Exit";
            tsmiFile_Exit.ShortcutKeys =  System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            tsmiFile_Exit.Size = new Size(259, 22);
            tsmiFile_Exit.Text = "E&xit";
            tsmiFile_Exit.Click += tsmiFile_Exit_Click;
            // 
            // tsmiView
            // 
            tsmiView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiView_HighlightEndcodesInTextureViews, tsmiView_Sep1, tsmiView_MPD });
            tsmiView.Name = "tsmiView";
            tsmiView.Size = new Size(44, 20);
            tsmiView.Text = "&View";
            // 
            // tsmiView_MPD
            // 
            tsmiView_MPD.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiView_MPD_DrawSurfaceModel, tsmiView_MPD_DrawModels, tsmiView_MPD_DrawGround, tsmiView_MPD_DrawSkyBox, tsmiView_MPD_RunAnimations, tsmiView_MPD_ApplyLighting, tsmiView_MPD_DrawGradients, tsmiView_MPD_Sep1, tsmiView_MPD_DrawWireframes, tsmiView_MPD_DrawBoundaries, tsmiView_MPD_DrawTerrainTypes, tsmiView_MPD_DrawEventIDs, tsmiView_MPD_DrawCollisionLines, tsmiView_MPD_HideModelsNotFacingCamera, tsmiView_MPD_RenderOnBlackBackground, tsmiView_MPD_DrawNormalMap, tsmiView_MPD_RotateSpritesUpToCamera, tsmiView_MPD_Sep2, tsmiView_MPD_ShowHelp, tsmiView_MPD_Sep3, tsmiView_MPD_EnableBlankFieldV2Controls });
            tsmiView_MPD.Name = "tsmiView_MPD";
            tsmiView_MPD.Size = new Size(265, 22);
            tsmiView_MPD.Text = "&MPD";
            // 
            // tsmiView_MPD_DrawSurfaceModel
            // 
            tsmiView_MPD_DrawSurfaceModel.Name = "tsmiView_MPD_DrawSurfaceModel";
            tsmiView_MPD_DrawSurfaceModel.Size = new Size(384, 22);
            tsmiView_MPD_DrawSurfaceModel.Text = "Draw Surface Model";
            tsmiView_MPD_DrawSurfaceModel.Click += tsmiView_MPD_DrawSurfaceModel_Click;
            // 
            // tsmiView_MPD_DrawModels
            // 
            tsmiView_MPD_DrawModels.Name = "tsmiView_MPD_DrawModels";
            tsmiView_MPD_DrawModels.Size = new Size(384, 22);
            tsmiView_MPD_DrawModels.Text = "Draw Models";
            tsmiView_MPD_DrawModels.Click += tsmiView_MPD_DrawModels_Click;
            // 
            // tsmiView_MPD_DrawGround
            // 
            tsmiView_MPD_DrawGround.Name = "tsmiView_MPD_DrawGround";
            tsmiView_MPD_DrawGround.Size = new Size(384, 22);
            tsmiView_MPD_DrawGround.Text = "Draw Ground/Water";
            tsmiView_MPD_DrawGround.Click += tsmiView_MPD_DrawGround_Click;
            // 
            // tsmiView_MPD_DrawSkyBox
            // 
            tsmiView_MPD_DrawSkyBox.Name = "tsmiView_MPD_DrawSkyBox";
            tsmiView_MPD_DrawSkyBox.Size = new Size(384, 22);
            tsmiView_MPD_DrawSkyBox.Text = "Draw Sky Box";
            tsmiView_MPD_DrawSkyBox.Click += tsmiView_MPD_DrawSkyBox_Click;
            // 
            // tsmiView_MPD_RunAnimations
            // 
            tsmiView_MPD_RunAnimations.Name = "tsmiView_MPD_RunAnimations";
            tsmiView_MPD_RunAnimations.Size = new Size(384, 22);
            tsmiView_MPD_RunAnimations.Text = "Run Animations";
            tsmiView_MPD_RunAnimations.Click += tsmiView_MPD_RunAnimations_Click;
            // 
            // tsmiView_MPD_ApplyLighting
            // 
            tsmiView_MPD_ApplyLighting.Name = "tsmiView_MPD_ApplyLighting";
            tsmiView_MPD_ApplyLighting.Size = new Size(384, 22);
            tsmiView_MPD_ApplyLighting.Text = "Apply Lighting";
            tsmiView_MPD_ApplyLighting.Click += tsmiView_MPD_ApplyLighting_Click;
            // 
            // tsmiView_MPD_DrawGradients
            // 
            tsmiView_MPD_DrawGradients.Name = "tsmiView_MPD_DrawGradients";
            tsmiView_MPD_DrawGradients.Size = new Size(384, 22);
            tsmiView_MPD_DrawGradients.Text = "Draw Gradients";
            tsmiView_MPD_DrawGradients.Click += tsmiView_MPD_DrawGradients_Click;
            // 
            // tsmiView_MPD_Sep1
            // 
            tsmiView_MPD_Sep1.Name = "tsmiView_MPD_Sep1";
            tsmiView_MPD_Sep1.Size = new Size(381, 6);
            // 
            // tsmiView_MPD_DrawWireframes
            // 
            tsmiView_MPD_DrawWireframes.Name = "tsmiView_MPD_DrawWireframes";
            tsmiView_MPD_DrawWireframes.Size = new Size(384, 22);
            tsmiView_MPD_DrawWireframes.Text = "Draw Wireframes";
            tsmiView_MPD_DrawWireframes.Click += tsmiView_MPD_DrawWireframes_Click;
            // 
            // tsmiView_MPD_DrawBoundaries
            // 
            tsmiView_MPD_DrawBoundaries.Name = "tsmiView_MPD_DrawBoundaries";
            tsmiView_MPD_DrawBoundaries.Size = new Size(384, 22);
            tsmiView_MPD_DrawBoundaries.Text = "Draw Boundaries";
            tsmiView_MPD_DrawBoundaries.Click += tsmiView_MPD_DrawBoundaries_Click;
            // 
            // tsmiView_MPD_DrawTerrainTypes
            // 
            tsmiView_MPD_DrawTerrainTypes.Name = "tsmiView_MPD_DrawTerrainTypes";
            tsmiView_MPD_DrawTerrainTypes.Size = new Size(384, 22);
            tsmiView_MPD_DrawTerrainTypes.Text = "Draw Terrain Types";
            tsmiView_MPD_DrawTerrainTypes.Click += tsmiView_MPD_DrawTerrainTypes_Click;
            // 
            // tsmiView_MPD_DrawEventIDs
            // 
            tsmiView_MPD_DrawEventIDs.Name = "tsmiView_MPD_DrawEventIDs";
            tsmiView_MPD_DrawEventIDs.Size = new Size(384, 22);
            tsmiView_MPD_DrawEventIDs.Text = "Draw Event IDs";
            tsmiView_MPD_DrawEventIDs.Click += tsmiView_MPD_DrawEventIDs_Click;
            // 
            // tsmiView_MPD_DrawCollisionLines
            // 
            tsmiView_MPD_DrawCollisionLines.Name = "tsmiView_MPD_DrawCollisionLines";
            tsmiView_MPD_DrawCollisionLines.Size = new Size(384, 22);
            tsmiView_MPD_DrawCollisionLines.Text = "Draw Collision Lines";
            tsmiView_MPD_DrawCollisionLines.Click += tsmiView_MPD_DrawCollisionLines_Click;
            // 
            // tsmiView_MPD_HideModelsNotFacingCamera
            // 
            tsmiView_MPD_HideModelsNotFacingCamera.Name = "tsmiView_MPD_HideModelsNotFacingCamera";
            tsmiView_MPD_HideModelsNotFacingCamera.Size = new Size(384, 22);
            tsmiView_MPD_HideModelsNotFacingCamera.Text = "Hide Models Not Facing Camera";
            tsmiView_MPD_HideModelsNotFacingCamera.Click += tsmiView_MPD_HideModelsNotFacingCamera_Click;
            // 
            // tsmiView_MPD_RenderOnBlackBackground
            // 
            tsmiView_MPD_RenderOnBlackBackground.Name = "tsmiView_MPD_RenderOnBlackBackground";
            tsmiView_MPD_RenderOnBlackBackground.Size = new Size(384, 22);
            tsmiView_MPD_RenderOnBlackBackground.Text = "Render on Black Background";
            tsmiView_MPD_RenderOnBlackBackground.Click += tsmiView_MPD_RenderOnBlackBackground_Click;
            // 
            // tsmiView_MPD_DrawNormalMap
            // 
            tsmiView_MPD_DrawNormalMap.Name = "tsmiView_MPD_DrawNormalMap";
            tsmiView_MPD_DrawNormalMap.Size = new Size(384, 22);
            tsmiView_MPD_DrawNormalMap.Text = "Draw Normal Map";
            tsmiView_MPD_DrawNormalMap.Click += tsmiView_MPD_DrawNormalMap_Click;
            // 
            // tsmiView_MPD_RotateSpritesUpToCamera
            // 
            tsmiView_MPD_RotateSpritesUpToCamera.Name = "tsmiView_MPD_RotateSpritesUpToCamera";
            tsmiView_MPD_RotateSpritesUpToCamera.Size = new Size(384, 22);
            tsmiView_MPD_RotateSpritesUpToCamera.Text = "Rotate Sprites Up to Camera";
            tsmiView_MPD_RotateSpritesUpToCamera.Click += tsmiView_MPD_RotateSpritesUpToCamera_Click;
            // 
            // tsmiView_MPD_Sep2
            // 
            tsmiView_MPD_Sep2.Name = "tsmiView_MPD_Sep2";
            tsmiView_MPD_Sep2.Size = new Size(381, 6);
            // 
            // tsmiView_MPD_ShowHelp
            // 
            tsmiView_MPD_ShowHelp.Name = "tsmiView_MPD_ShowHelp";
            tsmiView_MPD_ShowHelp.Size = new Size(384, 22);
            tsmiView_MPD_ShowHelp.Text = "Show Help";
            tsmiView_MPD_ShowHelp.Click += tsmiView_MPD_ShowHelp_Click;
            // 
            // tsmiView_MPD_Sep3
            // 
            tsmiView_MPD_Sep3.Name = "tsmiView_MPD_Sep3";
            tsmiView_MPD_Sep3.Size = new Size(381, 6);
            // 
            // tsmiView_MPD_EnableBlankFieldV2Controls
            // 
            tsmiView_MPD_EnableBlankFieldV2Controls.Name = "tsmiView_MPD_EnableBlankFieldV2Controls";
            tsmiView_MPD_EnableBlankFieldV2Controls.Size = new Size(384, 22);
            tsmiView_MPD_EnableBlankFieldV2Controls.Text = "E&XPERIMENTAL: Enable tile controls for BlankField_V2.MPD";
            tsmiView_MPD_EnableBlankFieldV2Controls.Click += tsmiView_MPD_EnableBlankFieldV2Controls_Click;
            // 
            // tsmiTools
            // 
            tsmiTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiTools_ImportTable, tsmiTools_ExportTable, tsmiTools_Sep1, tsmiTools_ApplyDFR, tsmiTools_CreateDFR, tsmiTools_Sep2, tsmiTools_MovePostEOFData, tsmiTools_InsertData });
            tsmiTools.Name = "tsmiTools";
            tsmiTools.Size = new Size(47, 20);
            tsmiTools.Text = "&Tools";
            // 
            // tsmiTools_ImportTable
            // 
            tsmiTools_ImportTable.Enabled = false;
            tsmiTools_ImportTable.Name = "tsmiTools_ImportTable";
            tsmiTools_ImportTable.Size = new Size(192, 22);
            tsmiTools_ImportTable.Text = "Copy Tables &From...";
            tsmiTools_ImportTable.Click += tsmiTools_ImportTable_Click;
            // 
            // tsmiTools_ExportTable
            // 
            tsmiTools_ExportTable.Enabled = false;
            tsmiTools_ExportTable.Name = "tsmiTools_ExportTable";
            tsmiTools_ExportTable.Size = new Size(192, 22);
            tsmiTools_ExportTable.Text = "Copy Tables &To...";
            tsmiTools_ExportTable.Click += tsmiTools_ExportTable_Click;
            // 
            // tsmiTools_Sep1
            // 
            tsmiTools_Sep1.Name = "tsmiTools_Sep1";
            tsmiTools_Sep1.Size = new Size(189, 6);
            // 
            // tsmiTools_ApplyDFR
            // 
            tsmiTools_ApplyDFR.Enabled = false;
            tsmiTools_ApplyDFR.Name = "tsmiTools_ApplyDFR";
            tsmiTools_ApplyDFR.Size = new Size(192, 22);
            tsmiTools_ApplyDFR.Text = "&Apply DFR File...";
            tsmiTools_ApplyDFR.Click += tsmiTools_ApplyDFR_Click;
            // 
            // tsmiTools_CreateDFR
            // 
            tsmiTools_CreateDFR.Enabled = false;
            tsmiTools_CreateDFR.Name = "tsmiTools_CreateDFR";
            tsmiTools_CreateDFR.Size = new Size(192, 22);
            tsmiTools_CreateDFR.Text = "&Create DFR File...";
            tsmiTools_CreateDFR.Click += tsmiTools_CreateDFR_Click;
            // 
            // tsmiTools_Sep2
            // 
            tsmiTools_Sep2.Name = "tsmiTools_Sep2";
            tsmiTools_Sep2.Size = new Size(189, 6);
            // 
            // tsmiTools_MovePostEOFData
            // 
            tsmiTools_MovePostEOFData.Enabled = false;
            tsmiTools_MovePostEOFData.Name = "tsmiTools_MovePostEOFData";
            tsmiTools_MovePostEOFData.Size = new Size(192, 22);
            tsmiTools_MovePostEOFData.Text = "&Move Post-EOF Data...";
            tsmiTools_MovePostEOFData.Click += tsmiTools_MovePostEOFData_Click;
            // 
            // tsmiTools_InsertData
            // 
            tsmiTools_InsertData.Enabled = false;
            tsmiTools_InsertData.Name = "tsmiTools_InsertData";
            tsmiTools_InsertData.Size = new Size(192, 22);
            tsmiTools_InsertData.Text = "&Insert Data...";
            tsmiTools_InsertData.Click += tsmiTools_InsertData_Click;
            // 
            // tsmiX019
            // 
            tsmiX019.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiX019_UnapplyMonsterEq, tsmiX019_ApplyMonsterEq });
            tsmiX019.Enabled = false;
            tsmiX019.Name = "tsmiX019";
            tsmiX019.Size = new Size(44, 20);
            tsmiX019.Text = "&X019";
            tsmiX019.Visible = false;
            // 
            // tsmiX019_UnapplyMonsterEq
            // 
            tsmiX019_UnapplyMonsterEq.Name = "tsmiX019_UnapplyMonsterEq";
            tsmiX019_UnapplyMonsterEq.Size = new Size(263, 22);
            tsmiX019_UnapplyMonsterEq.Text = "&Unapply Monster Equipment Stats...";
            tsmiX019_UnapplyMonsterEq.Click += tsmiX019_UnapplyMonsterEq_Click;
            // 
            // tsmiX019_ApplyMonsterEq
            // 
            tsmiX019_ApplyMonsterEq.Name = "tsmiX019_ApplyMonsterEq";
            tsmiX019_ApplyMonsterEq.Size = new Size(263, 22);
            tsmiX019_ApplyMonsterEq.Text = "&Apply Monster Equipment Stats...";
            tsmiX019_ApplyMonsterEq.Click += tsmiX019_ApplyMonsterEq_Click;
            // 
            // tsmiMPD
            // 
            tsmiMPD.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Chunks, tsmiMPD_Textures, tsmiMPD_ModelSwitchGroups, tsmiMPD_Sep1, tsmiMPD_RecalculateSurfaceModelNormals });
            tsmiMPD.Enabled = false;
            tsmiMPD.Name = "tsmiMPD";
            tsmiMPD.Size = new Size(45, 20);
            tsmiMPD.Text = "&MPD";
            tsmiMPD.Visible = false;
            // 
            // tsmiMPD_Chunks
            // 
            tsmiMPD_Chunks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Chunks_ImportChunk, tsmiMPD_Chunks_ExportChunk, tsmiMPD_Chunks_DeleteChunk, toolStripSeparator1, tsmiMPD_Chunks_RecompressModifiedChunks, tsmiMPD_Chunks_RecompressAllChunks, tsmiMPD_Chunks_RebuildChunkTable });
            tsmiMPD_Chunks.Name = "tsmiMPD_Chunks";
            tsmiMPD_Chunks.Size = new Size(261, 22);
            tsmiMPD_Chunks.Text = "&Chunks";
            // 
            // tsmiMPD_Chunks_ImportChunk
            // 
            tsmiMPD_Chunks_ImportChunk.Name = "tsmiMPD_Chunks_ImportChunk";
            tsmiMPD_Chunks_ImportChunk.Size = new Size(232, 22);
            tsmiMPD_Chunks_ImportChunk.Text = "&Import Chunk...";
            tsmiMPD_Chunks_ImportChunk.Click += tsmiMPD_Chunks_ImportChunk_Click;
            // 
            // tsmiMPD_Chunks_ExportChunk
            // 
            tsmiMPD_Chunks_ExportChunk.Name = "tsmiMPD_Chunks_ExportChunk";
            tsmiMPD_Chunks_ExportChunk.Size = new Size(232, 22);
            tsmiMPD_Chunks_ExportChunk.Text = "&Export Chunk...";
            tsmiMPD_Chunks_ExportChunk.Click += tsmiMPD_Chunks_ExportChunk_Click;
            // 
            // tsmiMPD_Chunks_DeleteChunk
            // 
            tsmiMPD_Chunks_DeleteChunk.Name = "tsmiMPD_Chunks_DeleteChunk";
            tsmiMPD_Chunks_DeleteChunk.Size = new Size(232, 22);
            tsmiMPD_Chunks_DeleteChunk.Text = "&Delete Chunk...";
            tsmiMPD_Chunks_DeleteChunk.Click += tsmiMPD_Chunks_DeleteChunk_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(229, 6);
            // 
            // tsmiMPD_Chunks_RecompressModifiedChunks
            // 
            tsmiMPD_Chunks_RecompressModifiedChunks.Name = "tsmiMPD_Chunks_RecompressModifiedChunks";
            tsmiMPD_Chunks_RecompressModifiedChunks.Size = new Size(232, 22);
            tsmiMPD_Chunks_RecompressModifiedChunks.Text = "Recompress Modified Chunks";
            tsmiMPD_Chunks_RecompressModifiedChunks.Click += tsmiMPD_Chunks_RecompressModifiedChunks_Click;
            // 
            // tsmiMPD_Chunks_RecompressAllChunks
            // 
            tsmiMPD_Chunks_RecompressAllChunks.Name = "tsmiMPD_Chunks_RecompressAllChunks";
            tsmiMPD_Chunks_RecompressAllChunks.Size = new Size(232, 22);
            tsmiMPD_Chunks_RecompressAllChunks.Text = "Recompress All Chunks";
            tsmiMPD_Chunks_RecompressAllChunks.Click += tsmiMPD_Chunks_RecompressAllChunks_Click;
            // 
            // tsmiMPD_Chunks_RebuildChunkTable
            // 
            tsmiMPD_Chunks_RebuildChunkTable.Name = "tsmiMPD_Chunks_RebuildChunkTable";
            tsmiMPD_Chunks_RebuildChunkTable.Size = new Size(232, 22);
            tsmiMPD_Chunks_RebuildChunkTable.Text = "Rebuild Chunk Table";
            tsmiMPD_Chunks_RebuildChunkTable.Click += tsmiMPD_Chunks_RebuildChunkTable_Click;
            // 
            // tsmiMPD_Textures
            // 
            tsmiMPD_Textures.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiMPD_Textures_ImportAll, tsmiMPD_Textures_ExportAll });
            tsmiMPD_Textures.Name = "tsmiMPD_Textures";
            tsmiMPD_Textures.Size = new Size(261, 22);
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
            // tsmiMPD_ModelSwitchGroups
            // 
            tsmiMPD_ModelSwitchGroups.Enabled = false;
            tsmiMPD_ModelSwitchGroups.Name = "tsmiMPD_ModelSwitchGroups";
            tsmiMPD_ModelSwitchGroups.Size = new Size(261, 22);
            tsmiMPD_ModelSwitchGroups.Text = "Model &Switch Groups";
            // 
            // tsmiMPD_Sep1
            // 
            tsmiMPD_Sep1.Name = "tsmiMPD_Sep1";
            tsmiMPD_Sep1.Size = new Size(258, 6);
            // 
            // tsmiMPD_RecalculateSurfaceModelNormals
            // 
            tsmiMPD_RecalculateSurfaceModelNormals.Name = "tsmiMPD_RecalculateSurfaceModelNormals";
            tsmiMPD_RecalculateSurfaceModelNormals.Size = new Size(261, 22);
            tsmiMPD_RecalculateSurfaceModelNormals.Text = "Recalculate Surface Model &Normals";
            tsmiMPD_RecalculateSurfaceModelNormals.Click += tsmiMPD_RecalculateSurfaceModelNormals_Click;
            // 
            // tsmiSettings
            // 
            tsmiSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiSettings_UseDropdowns, tsmiSettings_EnableDebugSettings, tsmiSettings_ShowErrorsOnFileLoad, tsmiSettings_Sep1, tsmiSettings_MPD });
            tsmiSettings.Name = "tsmiSettings";
            tsmiSettings.Size = new Size(61, 20);
            tsmiSettings.Text = "&Settings";
            // 
            // tsmiSettings_UseDropdowns
            // 
            tsmiSettings_UseDropdowns.Name = "tsmiSettings_UseDropdowns";
            tsmiSettings_UseDropdowns.Size = new Size(253, 22);
            tsmiSettings_UseDropdowns.Text = "Use &Dropdowns for Named Values";
            tsmiSettings_UseDropdowns.Click += tsmiSettings_UseDropdowns_Click;
            // 
            // tsmiSettings_EnableDebugSettings
            // 
            tsmiSettings_EnableDebugSettings.Name = "tsmiSettings_EnableDebugSettings";
            tsmiSettings_EnableDebugSettings.Size = new Size(253, 22);
            tsmiSettings_EnableDebugSettings.Text = "Show Debu&g Fields";
            tsmiSettings_EnableDebugSettings.Click += tsmiSettings_EnableDebugSettings_Click;
            // 
            // tsmiSettings_ShowErrorsOnFileLoad
            // 
            tsmiSettings_ShowErrorsOnFileLoad.Name = "tsmiSettings_ShowErrorsOnFileLoad";
            tsmiSettings_ShowErrorsOnFileLoad.Size = new Size(253, 22);
            tsmiSettings_ShowErrorsOnFileLoad.Text = "Show &Errors when Loading Files";
            tsmiSettings_ShowErrorsOnFileLoad.Click += tsmiSettings_ShowErrorsOnFileLoad_Click;
            // 
            // tsmiSettings_Sep1
            // 
            tsmiSettings_Sep1.Name = "tsmiSettings_Sep1";
            tsmiSettings_Sep1.Size = new Size(250, 6);
            // 
            // tsmiSettings_MPD
            // 
            tsmiSettings_MPD.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiSettings_MPD_ImprovedNormalCalculations, tsmiSettings_MPD_UseFullHeightForNormals, tsmiSettings_MPD_FixNormalOverflowUnderflowErrors, tsmiSettings_MPD_UpdateChunkTableOnChunkResize, tsmiSettings_MPD_AutoRebuildMPDChunkTable });
            tsmiSettings_MPD.Name = "tsmiSettings_MPD";
            tsmiSettings_MPD.Size = new Size(253, 22);
            tsmiSettings_MPD.Text = "&MPD";
            // 
            // tsmiSettings_MPD_ImprovedNormalCalculations
            // 
            tsmiSettings_MPD_ImprovedNormalCalculations.Name = "tsmiSettings_MPD_ImprovedNormalCalculations";
            tsmiSettings_MPD_ImprovedNormalCalculations.Size = new Size(454, 22);
            tsmiSettings_MPD_ImprovedNormalCalculations.Text = "Use Improved Surface Map Tile Normal Calculation Function";
            tsmiSettings_MPD_ImprovedNormalCalculations.Click += tsmiSettings_MPD_ImprovedNormalCalculations_Click;
            // 
            // tsmiSettings_MPD_UseFullHeightForNormals
            // 
            tsmiSettings_MPD_UseFullHeightForNormals.Name = "tsmiSettings_MPD_UseFullHeightForNormals";
            tsmiSettings_MPD_UseFullHeightForNormals.Size = new Size(454, 22);
            tsmiSettings_MPD_UseFullHeightForNormals.Text = "Use Full Instead of Half Height for Surface Map Tile Normal Calculations";
            tsmiSettings_MPD_UseFullHeightForNormals.Click += tsmiSettings_MPD_UseFullHeightForNormals_Click;
            // 
            // tsmiSettings_MPD_FixNormalOverflowUnderflowErrors
            // 
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Name = "tsmiSettings_MPD_FixNormalOverflowUnderflowErrors";
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Size = new Size(454, 22);
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Text = "Fix Surface Map Tile Normal Overflow/Underflow Errors";
            tsmiSettings_MPD_FixNormalOverflowUnderflowErrors.Click += tsmiSettings_MPD_FixNormalOverflowUnderflowErrors_Click;
            // 
            // tsmiSettings_MPD_UpdateChunkTableOnChunkResize
            // 
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Name = "tsmiSettings_MPD_UpdateChunkTableOnChunkResize";
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Size = new Size(454, 22);
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Text = "Update Chunk Table on Chunk Resize";
            tsmiSettings_MPD_UpdateChunkTableOnChunkResize.Click += tsmiSettings_MPD_UpdateChunkTableOnChunkResize_Click;
            // 
            // tsmiSettings_MPD_AutoRebuildMPDChunkTable
            // 
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Name = "tsmiSettings_MPD_AutoRebuildMPDChunkTable";
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Size = new Size(454, 22);
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Text = "Rebuild Chunk Table on Save";
            tsmiSettings_MPD_AutoRebuildMPDChunkTable.Click += tsmiSettings_MPD_AutoRebuildMPDChunkTable_Click;
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
            tsmiHelp_About.Size = new Size(180, 22);
            tsmiHelp_About.Text = "&About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // tsmiView_Sep1
            // 
            tsmiView_Sep1.Name = "tsmiView_Sep1";
            tsmiView_Sep1.Size = new Size(262, 6);
            // 
            // tsmiView_HighlightEndcodesInTextureViews
            // 
            tsmiView_HighlightEndcodesInTextureViews.Name = "tsmiView_HighlightEndcodesInTextureViews";
            tsmiView_HighlightEndcodesInTextureViews.Size = new Size(265, 22);
            tsmiView_HighlightEndcodesInTextureViews.Text = "&Highlight Endcodes in Texture Views";
            tsmiView_HighlightEndcodesInTextureViews.Click += tsmiView_HighlightEndcodesInTextureViews_Click;
            // 
            // SF3EditorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(891, 631);
            Controls.Add(menuStrip1);
            Icon = (Icon) resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Name = "SF3EditorForm";
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
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_UseDropdowns;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ApplyDFR;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_CreateDFR;
        private System.Windows.Forms.ToolStripSeparator tsmiTools_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ImportTable;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_ExportTable;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_EnableDebugSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures_ImportAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Textures_ExportAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_ExportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_ImportChunk;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_DeleteChunk;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_RecompressModifiedChunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_RebuildChunkTable;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep3;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SwapToPrev;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SwapToNext;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_CloseAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SaveAll;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_3;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_4;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_5;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_6;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_7;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_8;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_9;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_RecentFiles_10;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep4;
        private System.Windows.Forms.ToolStripMenuItem tsmiView;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawSurfaceModel;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawModels;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawGround;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawSkyBox;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_RunAnimations;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_ApplyLighting;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawGradients;
        private System.Windows.Forms.ToolStripSeparator tsmiView_MPD_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawWireframes;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawBoundaries;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawTerrainTypes;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawEventIDs;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawCollisionLines;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_DrawNormalMap;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_RotateSpritesUpToCamera;
        private System.Windows.Forms.ToolStripSeparator tsmiView_MPD_Sep2;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_EnableBlankFieldV2Controls;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_ShowHelp;
        private System.Windows.Forms.ToolStripSeparator tsmiView_MPD_Sep3;
        private System.Windows.Forms.ToolStripSeparator tsmiSettings_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD_ImprovedNormalCalculations;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD_UseFullHeightForNormals;
        private System.Windows.Forms.ToolStripSeparator tsmiMPD_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_RecalculateSurfaceModelNormals;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD_FixNormalOverflowUnderflowErrors;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_HideModelsNotFacingCamera;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_MPD_RenderOnBlackBackground;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_ModelSwitchGroups;
        private System.Windows.Forms.ToolStripMenuItem tsmiX019;
        private System.Windows.Forms.ToolStripMenuItem tsmiX019_UnapplyMonsterEq;
        private System.Windows.Forms.ToolStripMenuItem tsmiX019_ApplyMonsterEq;
        private System.Windows.Forms.ToolStripSeparator tsmiTools_Sep2;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_MovePostEOFData;
        private System.Windows.Forms.ToolStripMenuItem tsmiTools_InsertData;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD_AutoRebuildMPDChunkTable;
        private System.Windows.Forms.ToolStripMenuItem tsmiMPD_Chunks_RecompressAllChunks;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_MPD_UpdateChunkTableOnChunkResize;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings_ShowErrorsOnFileLoad;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_ScanForErrors;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep5;
        private System.Windows.Forms.ToolStripMenuItem tsmiView_HighlightEndcodesInTextureViews;
        private System.Windows.Forms.ToolStripSeparator tsmiView_Sep1;
    }
}
