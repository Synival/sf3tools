using System.Drawing.Text;

namespace SF3.X1_Editor.Forms
{
    partial class frmX1_Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmX1_Editor));
            this.tabTileData = new System.Windows.Forms.TabPage();
            this.olvTileData = new BrightIdeasSoftware.ObjectListView();
            this.lvcTileDataName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataNoEntry = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataAir = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataGrassland = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataDirt = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataDarkGrass = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataForest = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataBrownMountain = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataDesert = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataGreyMountain = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataWater = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataUnknownA = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataSand = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataUnknownC = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataUnknownD = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataUnknownE = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTileDataUnknownF = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabWarpTable = new System.Windows.Forms.TabPage();
            this.olvWarpTable = new BrightIdeasSoftware.ObjectListView();
            this.lvcWarpTableName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTableID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTableAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTablePlus0x00 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTablePlus0x01 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTableType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcWarpTableMap = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabBattlePointers = new System.Windows.Forms.TabPage();
            this.olvBattlePointers = new BrightIdeasSoftware.ObjectListView();
            this.lvcBattlePointersName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcBattlePointersID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcBattlePointersAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcBattlePointersPointer = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabArrows = new System.Windows.Forms.TabPage();
            this.olvArrows = new BrightIdeasSoftware.ObjectListView();
            this.lvcArrowsName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsPlus0x00 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsTextID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsPlus0x04 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsPointToWarpMPD = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsPlus0x08 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcArrowsPlus0x0A = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabNonBattleEnter = new System.Windows.Forms.TabPage();
            this.olvNonBattleEnter = new BrightIdeasSoftware.ObjectListView();
            this.lvcNonBattleEnterName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterSceneNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterPlus0x02 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterXPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterPlus0x06 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterZPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterDirection = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterCamera = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcNonBattleEnterPlus0x0E = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabTownNpcs = new System.Windows.Forms.TabPage();
            this.olvTownNpcs = new BrightIdeasSoftware.ObjectListView();
            this.lvcTownNpcsName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsSpriteID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x02 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsMonsterTable = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsXPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x0A = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x0C = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x0E = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsZPos = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsDirection = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsPlus0x16 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcTownNpcsTiedToEventNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabInteractables = new System.Windows.Forms.TabPage();
            this.olvInteractables = new BrightIdeasSoftware.ObjectListView();
            this.lvcInteractablesName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesDirectionSearched = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesEventNumber = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesFlagUsed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesUnknown1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesEventTypeCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesItemTextCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcInteractablesMPDTieInID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabBattle_Synbios = new System.Windows.Forms.TabPage();
            this.becBattle_Synbios = new SF3.X1_Editor.Controls.BattleEditorControl();
            this.tabBattle_Medion = new System.Windows.Forms.TabPage();
            this.becBattle_Medion = new SF3.X1_Editor.Controls.BattleEditorControl();
            this.tabBattle_Julian = new System.Windows.Forms.TabPage();
            this.becBattle_Julian = new SF3.X1_Editor.Controls.BattleEditorControl();
            this.tabBattle_Extra = new System.Windows.Forms.TabPage();
            this.becBattle_Extra = new SF3.X1_Editor.Controls.BattleEditorControl();
            this.tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiScenario_BTL99 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.tabTileData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvTileData)).BeginInit();
            this.tabWarpTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvWarpTable)).BeginInit();
            this.tabBattlePointers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvBattlePointers)).BeginInit();
            this.tabArrows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvArrows)).BeginInit();
            this.tabNonBattleEnter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvNonBattleEnter)).BeginInit();
            this.tabTownNpcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvTownNpcs)).BeginInit();
            this.tabInteractables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvInteractables)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabBattle_Synbios.SuspendLayout();
            this.tabBattle_Medion.SuspendLayout();
            this.tabBattle_Julian.SuspendLayout();
            this.tabBattle_Extra.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabTileData
            // 
            this.tabTileData.Controls.Add(this.olvTileData);
            this.tabTileData.Location = new System.Drawing.Point(4, 22);
            this.tabTileData.Name = "tabTileData";
            this.tabTileData.Padding = new System.Windows.Forms.Padding(3);
            this.tabTileData.Size = new System.Drawing.Size(909, 483);
            this.tabTileData.TabIndex = 14;
            this.tabTileData.Text = "TileData scn2,3,pd";
            this.tabTileData.UseVisualStyleBackColor = true;
            // 
            // olvTileData
            // 
            this.olvTileData.AllColumns.Add(this.lvcTileDataName);
            this.olvTileData.AllColumns.Add(this.lvcTileDataID);
            this.olvTileData.AllColumns.Add(this.lvcTileDataAddress);
            this.olvTileData.AllColumns.Add(this.lvcTileDataNoEntry);
            this.olvTileData.AllColumns.Add(this.lvcTileDataAir);
            this.olvTileData.AllColumns.Add(this.lvcTileDataGrassland);
            this.olvTileData.AllColumns.Add(this.lvcTileDataDirt);
            this.olvTileData.AllColumns.Add(this.lvcTileDataDarkGrass);
            this.olvTileData.AllColumns.Add(this.lvcTileDataForest);
            this.olvTileData.AllColumns.Add(this.lvcTileDataBrownMountain);
            this.olvTileData.AllColumns.Add(this.lvcTileDataDesert);
            this.olvTileData.AllColumns.Add(this.lvcTileDataGreyMountain);
            this.olvTileData.AllColumns.Add(this.lvcTileDataWater);
            this.olvTileData.AllColumns.Add(this.lvcTileDataUnknownA);
            this.olvTileData.AllColumns.Add(this.lvcTileDataSand);
            this.olvTileData.AllColumns.Add(this.lvcTileDataUnknownC);
            this.olvTileData.AllColumns.Add(this.lvcTileDataUnknownD);
            this.olvTileData.AllColumns.Add(this.lvcTileDataUnknownE);
            this.olvTileData.AllColumns.Add(this.lvcTileDataUnknownF);
            this.olvTileData.AllowColumnReorder = true;
            this.olvTileData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvTileData.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvTileData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcTileDataName,
            this.lvcTileDataID,
            this.lvcTileDataAddress,
            this.lvcTileDataNoEntry,
            this.lvcTileDataAir,
            this.lvcTileDataGrassland,
            this.lvcTileDataDirt,
            this.lvcTileDataDarkGrass,
            this.lvcTileDataForest,
            this.lvcTileDataBrownMountain,
            this.lvcTileDataDesert,
            this.lvcTileDataGreyMountain,
            this.lvcTileDataWater,
            this.lvcTileDataUnknownA,
            this.lvcTileDataSand,
            this.lvcTileDataUnknownC,
            this.lvcTileDataUnknownD,
            this.lvcTileDataUnknownE,
            this.lvcTileDataUnknownF});
            this.olvTileData.FullRowSelect = true;
            this.olvTileData.GridLines = true;
            this.olvTileData.HasCollapsibleGroups = false;
            this.olvTileData.HideSelection = false;
            this.olvTileData.Location = new System.Drawing.Point(3, 3);
            this.olvTileData.MenuLabelGroupBy = "";
            this.olvTileData.Name = "olvTileData";
            this.olvTileData.ShowGroups = false;
            this.olvTileData.Size = new System.Drawing.Size(903, 477);
            this.olvTileData.TabIndex = 2;
            this.olvTileData.UseAlternatingBackColors = true;
            this.olvTileData.UseCompatibleStateImageBehavior = false;
            this.olvTileData.View = System.Windows.Forms.View.Details;
            this.olvTileData.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcTileDataName
            // 
            this.lvcTileDataName.AspectName = "Name";
            this.lvcTileDataName.IsEditable = false;
            this.lvcTileDataName.Text = "Name";
            this.lvcTileDataName.Width = 90;
            // 
            // lvcTileDataID
            // 
            this.lvcTileDataID.AspectName = "ID";
            this.lvcTileDataID.AspectToStringFormat = "{0:X}";
            this.lvcTileDataID.IsEditable = false;
            this.lvcTileDataID.Text = "Hex ID";
            this.lvcTileDataID.Width = 50;
            // 
            // lvcTileDataAddress
            // 
            this.lvcTileDataAddress.AspectName = "Address";
            this.lvcTileDataAddress.AspectToStringFormat = "{0:X}";
            this.lvcTileDataAddress.IsEditable = false;
            this.lvcTileDataAddress.Text = "Address";
            this.lvcTileDataAddress.Width = 50;
            // 
            // lvcTileDataNoEntry
            // 
            this.lvcTileDataNoEntry.AspectName = "TileNoEntry";
            this.lvcTileDataNoEntry.AspectToStringFormat = "{0:X}";
            this.lvcTileDataNoEntry.Text = "NoEntry";
            this.lvcTileDataNoEntry.Width = 50;
            // 
            // lvcTileDataAir
            // 
            this.lvcTileDataAir.AspectName = "TileUnknown1";
            this.lvcTileDataAir.AspectToStringFormat = "{0:X}";
            this.lvcTileDataAir.Text = "Air";
            this.lvcTileDataAir.Width = 40;
            // 
            // lvcTileDataGrassland
            // 
            this.lvcTileDataGrassland.AspectName = "TileGrassland";
            this.lvcTileDataGrassland.AspectToStringFormat = "{0:X}";
            this.lvcTileDataGrassland.Text = "Grassland";
            this.lvcTileDataGrassland.Width = 65;
            // 
            // lvcTileDataDirt
            // 
            this.lvcTileDataDirt.AspectName = "TileDirt";
            this.lvcTileDataDirt.AspectToStringFormat = "{0:X}";
            this.lvcTileDataDirt.Text = "Dirt";
            this.lvcTileDataDirt.Width = 40;
            // 
            // lvcTileDataDarkGrass
            // 
            this.lvcTileDataDarkGrass.AspectName = "TileDarkGrass";
            this.lvcTileDataDarkGrass.AspectToStringFormat = "{0:X}";
            this.lvcTileDataDarkGrass.Text = "DarkGrass";
            this.lvcTileDataDarkGrass.Width = 65;
            // 
            // lvcTileDataForest
            // 
            this.lvcTileDataForest.AspectName = "TileForest";
            this.lvcTileDataForest.AspectToStringFormat = "{0:X}";
            this.lvcTileDataForest.Text = "Forest";
            this.lvcTileDataForest.Width = 45;
            // 
            // lvcTileDataBrownMountain
            // 
            this.lvcTileDataBrownMountain.AspectName = "TileBrownMountain";
            this.lvcTileDataBrownMountain.AspectToStringFormat = "{0:X}";
            this.lvcTileDataBrownMountain.Text = "BrownMountain";
            this.lvcTileDataBrownMountain.Width = 90;
            // 
            // lvcTileDataDesert
            // 
            this.lvcTileDataDesert.AspectName = "TileDesert";
            this.lvcTileDataDesert.AspectToStringFormat = "{0:X}";
            this.lvcTileDataDesert.Text = "Desert";
            this.lvcTileDataDesert.Width = 50;
            // 
            // lvcTileDataGreyMountain
            // 
            this.lvcTileDataGreyMountain.AspectName = "TileGreyMountain";
            this.lvcTileDataGreyMountain.AspectToStringFormat = "{0:X}";
            this.lvcTileDataGreyMountain.Text = "GreyMountain";
            this.lvcTileDataGreyMountain.Width = 80;
            // 
            // lvcTileDataWater
            // 
            this.lvcTileDataWater.AspectName = "TileUnknown9";
            this.lvcTileDataWater.AspectToStringFormat = "{0:X}";
            this.lvcTileDataWater.Text = "Water";
            this.lvcTileDataWater.Width = 65;
            // 
            // lvcTileDataUnknownA
            // 
            this.lvcTileDataUnknownA.AspectName = "TileUnknownA";
            this.lvcTileDataUnknownA.AspectToStringFormat = "{0:X}";
            this.lvcTileDataUnknownA.Text = "UnknownA";
            this.lvcTileDataUnknownA.Width = 65;
            // 
            // lvcTileDataSand
            // 
            this.lvcTileDataSand.AspectName = "TileUnknownB";
            this.lvcTileDataSand.AspectToStringFormat = "{0:X}";
            this.lvcTileDataSand.Text = "Sand";
            this.lvcTileDataSand.Width = 45;
            // 
            // lvcTileDataUnknownC
            // 
            this.lvcTileDataUnknownC.AspectName = "TileUnknownC";
            this.lvcTileDataUnknownC.AspectToStringFormat = "{0:X}";
            this.lvcTileDataUnknownC.Text = "UnknownC";
            this.lvcTileDataUnknownC.Width = 65;
            // 
            // lvcTileDataUnknownD
            // 
            this.lvcTileDataUnknownD.AspectName = "TileUnknownD";
            this.lvcTileDataUnknownD.AspectToStringFormat = "{0:X}";
            this.lvcTileDataUnknownD.Text = "UnknownD";
            this.lvcTileDataUnknownD.Width = 65;
            // 
            // lvcTileDataUnknownE
            // 
            this.lvcTileDataUnknownE.AspectName = "TileUnknownE";
            this.lvcTileDataUnknownE.AspectToStringFormat = "{0:X}";
            this.lvcTileDataUnknownE.Text = "UnknownE";
            this.lvcTileDataUnknownE.Width = 65;
            // 
            // lvcTileDataUnknownF
            // 
            this.lvcTileDataUnknownF.AspectName = "TileUnknownF";
            this.lvcTileDataUnknownF.AspectToStringFormat = "{0:X}";
            this.lvcTileDataUnknownF.Text = "UnknownF";
            this.lvcTileDataUnknownF.Width = 65;
            // 
            // tabWarpTable
            // 
            this.tabWarpTable.Controls.Add(this.olvWarpTable);
            this.tabWarpTable.Location = new System.Drawing.Point(4, 22);
            this.tabWarpTable.Name = "tabWarpTable";
            this.tabWarpTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabWarpTable.Size = new System.Drawing.Size(909, 483);
            this.tabWarpTable.TabIndex = 13;
            this.tabWarpTable.Text = "WarpTable scn2,3,pd";
            this.tabWarpTable.UseVisualStyleBackColor = true;
            // 
            // olvWarpTable
            // 
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTableName);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTableID);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTableAddress);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTablePlus0x00);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTablePlus0x01);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTableType);
            this.olvWarpTable.AllColumns.Add(this.lvcWarpTableMap);
            this.olvWarpTable.AllowColumnReorder = true;
            this.olvWarpTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvWarpTable.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvWarpTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcWarpTableName,
            this.lvcWarpTableID,
            this.lvcWarpTableAddress,
            this.lvcWarpTablePlus0x00,
            this.lvcWarpTablePlus0x01,
            this.lvcWarpTableType,
            this.lvcWarpTableMap});
            this.olvWarpTable.FullRowSelect = true;
            this.olvWarpTable.GridLines = true;
            this.olvWarpTable.HasCollapsibleGroups = false;
            this.olvWarpTable.HideSelection = false;
            this.olvWarpTable.Location = new System.Drawing.Point(3, 3);
            this.olvWarpTable.MenuLabelGroupBy = "";
            this.olvWarpTable.Name = "olvWarpTable";
            this.olvWarpTable.ShowGroups = false;
            this.olvWarpTable.Size = new System.Drawing.Size(903, 477);
            this.olvWarpTable.TabIndex = 5;
            this.olvWarpTable.UseAlternatingBackColors = true;
            this.olvWarpTable.UseCompatibleStateImageBehavior = false;
            this.olvWarpTable.View = System.Windows.Forms.View.Details;
            this.olvWarpTable.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcWarpTableName
            // 
            this.lvcWarpTableName.AspectName = "Name";
            this.lvcWarpTableName.IsEditable = false;
            this.lvcWarpTableName.Text = "WarpName";
            this.lvcWarpTableName.Width = 90;
            // 
            // lvcWarpTableID
            // 
            this.lvcWarpTableID.AspectName = "ID";
            this.lvcWarpTableID.AspectToStringFormat = "{0:X}";
            this.lvcWarpTableID.IsEditable = false;
            this.lvcWarpTableID.Text = "Warp ID";
            this.lvcWarpTableID.Width = 53;
            // 
            // lvcWarpTableAddress
            // 
            this.lvcWarpTableAddress.AspectName = "Address";
            this.lvcWarpTableAddress.AspectToStringFormat = "{0:X}";
            this.lvcWarpTableAddress.IsEditable = false;
            this.lvcWarpTableAddress.Text = "Address";
            this.lvcWarpTableAddress.Width = 50;
            // 
            // lvcWarpTablePlus0x00
            // 
            this.lvcWarpTablePlus0x00.AspectName = "WarpUnknown1";
            this.lvcWarpTablePlus0x00.AspectToStringFormat = "{0:X}";
            this.lvcWarpTablePlus0x00.Text = "+0x00";
            this.lvcWarpTablePlus0x00.Width = 110;
            // 
            // lvcWarpTablePlus0x01
            // 
            this.lvcWarpTablePlus0x01.AspectName = "WarpUnknown2";
            this.lvcWarpTablePlus0x01.AspectToStringFormat = "{0:X}";
            this.lvcWarpTablePlus0x01.Text = "+0x01";
            this.lvcWarpTablePlus0x01.Width = 82;
            // 
            // lvcWarpTableType
            // 
            this.lvcWarpTableType.AspectName = "WarpType";
            this.lvcWarpTableType.AspectToStringFormat = "{0:X}";
            this.lvcWarpTableType.Text = "Type";
            // 
            // lvcWarpTableMap
            // 
            this.lvcWarpTableMap.AspectName = "WarpMap";
            this.lvcWarpTableMap.AspectToStringFormat = "{0:X}";
            this.lvcWarpTableMap.Text = "Map";
            this.lvcWarpTableMap.Width = 70;
            // 
            // tabBattlePointers
            // 
            this.tabBattlePointers.Controls.Add(this.olvBattlePointers);
            this.tabBattlePointers.Location = new System.Drawing.Point(4, 22);
            this.tabBattlePointers.Name = "tabBattlePointers";
            this.tabBattlePointers.Padding = new System.Windows.Forms.Padding(3);
            this.tabBattlePointers.Size = new System.Drawing.Size(909, 483);
            this.tabBattlePointers.TabIndex = 10;
            this.tabBattlePointers.Text = "Battle Pointers";
            this.tabBattlePointers.UseVisualStyleBackColor = true;
            // 
            // olvBattlePointers
            // 
            this.olvBattlePointers.AllColumns.Add(this.lvcBattlePointersName);
            this.olvBattlePointers.AllColumns.Add(this.lvcBattlePointersID);
            this.olvBattlePointers.AllColumns.Add(this.lvcBattlePointersAddress);
            this.olvBattlePointers.AllColumns.Add(this.lvcBattlePointersPointer);
            this.olvBattlePointers.AllowColumnReorder = true;
            this.olvBattlePointers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvBattlePointers.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvBattlePointers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcBattlePointersName,
            this.lvcBattlePointersID,
            this.lvcBattlePointersAddress,
            this.lvcBattlePointersPointer});
            this.olvBattlePointers.FullRowSelect = true;
            this.olvBattlePointers.GridLines = true;
            this.olvBattlePointers.HasCollapsibleGroups = false;
            this.olvBattlePointers.HideSelection = false;
            this.olvBattlePointers.Location = new System.Drawing.Point(3, 3);
            this.olvBattlePointers.MenuLabelGroupBy = "";
            this.olvBattlePointers.Name = "olvBattlePointers";
            this.olvBattlePointers.ShowGroups = false;
            this.olvBattlePointers.Size = new System.Drawing.Size(903, 477);
            this.olvBattlePointers.TabIndex = 3;
            this.olvBattlePointers.UseAlternatingBackColors = true;
            this.olvBattlePointers.UseCompatibleStateImageBehavior = false;
            this.olvBattlePointers.View = System.Windows.Forms.View.Details;
            this.olvBattlePointers.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcBattlePointersName
            // 
            this.lvcBattlePointersName.AspectName = "Name";
            this.lvcBattlePointersName.IsEditable = false;
            this.lvcBattlePointersName.Text = "Name";
            this.lvcBattlePointersName.Width = 90;
            // 
            // lvcBattlePointersID
            // 
            this.lvcBattlePointersID.AspectName = "ID";
            this.lvcBattlePointersID.AspectToStringFormat = "{0:X}";
            this.lvcBattlePointersID.IsEditable = false;
            this.lvcBattlePointersID.Text = "Hex ID";
            this.lvcBattlePointersID.Width = 50;
            // 
            // lvcBattlePointersAddress
            // 
            this.lvcBattlePointersAddress.AspectName = "Address";
            this.lvcBattlePointersAddress.AspectToStringFormat = "{0:X}";
            this.lvcBattlePointersAddress.IsEditable = false;
            this.lvcBattlePointersAddress.Text = "Address";
            this.lvcBattlePointersAddress.Width = 50;
            // 
            // lvcBattlePointersPointer
            // 
            this.lvcBattlePointersPointer.AspectName = "BattlePointer";
            this.lvcBattlePointersPointer.AspectToStringFormat = "{0:X}";
            this.lvcBattlePointersPointer.Text = "Pointer";
            this.lvcBattlePointersPointer.Width = 86;
            // 
            // tabArrows
            // 
            this.tabArrows.Controls.Add(this.olvArrows);
            this.tabArrows.Location = new System.Drawing.Point(4, 22);
            this.tabArrows.Name = "tabArrows";
            this.tabArrows.Padding = new System.Windows.Forms.Padding(3);
            this.tabArrows.Size = new System.Drawing.Size(909, 483);
            this.tabArrows.TabIndex = 17;
            this.tabArrows.Text = "Arrows scn2,3,pd";
            this.tabArrows.UseVisualStyleBackColor = true;
            // 
            // olvArrows
            // 
            this.olvArrows.AllColumns.Add(this.lvcArrowsName);
            this.olvArrows.AllColumns.Add(this.lvcArrowsID);
            this.olvArrows.AllColumns.Add(this.lvcArrowsAddress);
            this.olvArrows.AllColumns.Add(this.lvcArrowsPlus0x00);
            this.olvArrows.AllColumns.Add(this.lvcArrowsTextID);
            this.olvArrows.AllColumns.Add(this.lvcArrowsPlus0x04);
            this.olvArrows.AllColumns.Add(this.lvcArrowsPointToWarpMPD);
            this.olvArrows.AllColumns.Add(this.lvcArrowsPlus0x08);
            this.olvArrows.AllColumns.Add(this.lvcArrowsPlus0x0A);
            this.olvArrows.AllowColumnReorder = true;
            this.olvArrows.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvArrows.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvArrows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcArrowsName,
            this.lvcArrowsID,
            this.lvcArrowsAddress,
            this.lvcArrowsPlus0x00,
            this.lvcArrowsTextID,
            this.lvcArrowsPlus0x04,
            this.lvcArrowsPointToWarpMPD,
            this.lvcArrowsPlus0x08,
            this.lvcArrowsPlus0x0A});
            this.olvArrows.FullRowSelect = true;
            this.olvArrows.GridLines = true;
            this.olvArrows.HasCollapsibleGroups = false;
            this.olvArrows.HideSelection = false;
            this.olvArrows.Location = new System.Drawing.Point(3, 3);
            this.olvArrows.MenuLabelGroupBy = "";
            this.olvArrows.Name = "olvArrows";
            this.olvArrows.ShowGroups = false;
            this.olvArrows.Size = new System.Drawing.Size(903, 477);
            this.olvArrows.TabIndex = 7;
            this.olvArrows.UseAlternatingBackColors = true;
            this.olvArrows.UseCompatibleStateImageBehavior = false;
            this.olvArrows.View = System.Windows.Forms.View.Details;
            this.olvArrows.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcArrowsName
            // 
            this.lvcArrowsName.AspectName = "Name";
            this.lvcArrowsName.IsEditable = false;
            this.lvcArrowsName.Text = "ArrowName";
            this.lvcArrowsName.Width = 70;
            // 
            // lvcArrowsID
            // 
            this.lvcArrowsID.AspectName = "ID";
            this.lvcArrowsID.AspectToStringFormat = "{0:X}";
            this.lvcArrowsID.IsEditable = false;
            this.lvcArrowsID.Text = "ArrowId";
            this.lvcArrowsID.Width = 50;
            // 
            // lvcArrowsAddress
            // 
            this.lvcArrowsAddress.AspectName = "Address";
            this.lvcArrowsAddress.AspectToStringFormat = "{0:X}";
            this.lvcArrowsAddress.IsEditable = false;
            this.lvcArrowsAddress.Text = "Address";
            this.lvcArrowsAddress.Width = 50;
            // 
            // lvcArrowsPlus0x00
            // 
            this.lvcArrowsPlus0x00.AspectName = "ArrowUnknown0";
            this.lvcArrowsPlus0x00.AspectToStringFormat = "{0:X}";
            this.lvcArrowsPlus0x00.Text = "+0x00";
            // 
            // lvcArrowsTextID
            // 
            this.lvcArrowsTextID.AspectName = "ArrowText";
            this.lvcArrowsTextID.AspectToStringFormat = "{0:X}";
            this.lvcArrowsTextID.Text = "TextID";
            this.lvcArrowsTextID.Width = 55;
            // 
            // lvcArrowsPlus0x04
            // 
            this.lvcArrowsPlus0x04.AspectName = "ArrowUnknown4";
            this.lvcArrowsPlus0x04.AspectToStringFormat = "{0:X}";
            this.lvcArrowsPlus0x04.Text = "+0x04";
            this.lvcArrowsPlus0x04.Width = 70;
            // 
            // lvcArrowsPointToWarpMPD
            // 
            this.lvcArrowsPointToWarpMPD.AspectName = "ArrowWarp";
            this.lvcArrowsPointToWarpMPD.AspectToStringFormat = "{0:X}";
            this.lvcArrowsPointToWarpMPD.Text = "PointToWarpMPD";
            this.lvcArrowsPointToWarpMPD.Width = 100;
            // 
            // lvcArrowsPlus0x08
            // 
            this.lvcArrowsPlus0x08.AspectName = "ArrowUnknown8";
            this.lvcArrowsPlus0x08.AspectToStringFormat = "{0:X}";
            this.lvcArrowsPlus0x08.Text = "+0x08";
            this.lvcArrowsPlus0x08.Width = 70;
            // 
            // lvcArrowsPlus0x0A
            // 
            this.lvcArrowsPlus0x0A.AspectName = "ArrowUnknownA";
            this.lvcArrowsPlus0x0A.AspectToStringFormat = "{0:X}";
            this.lvcArrowsPlus0x0A.Text = "+0x0A";
            // 
            // tabNonBattleEnter
            // 
            this.tabNonBattleEnter.Controls.Add(this.olvNonBattleEnter);
            this.tabNonBattleEnter.Location = new System.Drawing.Point(4, 22);
            this.tabNonBattleEnter.Name = "tabNonBattleEnter";
            this.tabNonBattleEnter.Padding = new System.Windows.Forms.Padding(3);
            this.tabNonBattleEnter.Size = new System.Drawing.Size(909, 483);
            this.tabNonBattleEnter.TabIndex = 16;
            this.tabNonBattleEnter.Text = "Non-battle Enter";
            this.tabNonBattleEnter.UseVisualStyleBackColor = true;
            // 
            // olvNonBattleEnter
            // 
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterName);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterID);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterAddress);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterSceneNumber);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterPlus0x02);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterXPos);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterPlus0x06);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterZPos);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterDirection);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterCamera);
            this.olvNonBattleEnter.AllColumns.Add(this.lvcNonBattleEnterPlus0x0E);
            this.olvNonBattleEnter.AllowColumnReorder = true;
            this.olvNonBattleEnter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvNonBattleEnter.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvNonBattleEnter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcNonBattleEnterName,
            this.lvcNonBattleEnterID,
            this.lvcNonBattleEnterAddress,
            this.lvcNonBattleEnterSceneNumber,
            this.lvcNonBattleEnterPlus0x02,
            this.lvcNonBattleEnterXPos,
            this.lvcNonBattleEnterPlus0x06,
            this.lvcNonBattleEnterZPos,
            this.lvcNonBattleEnterDirection,
            this.lvcNonBattleEnterCamera,
            this.lvcNonBattleEnterPlus0x0E});
            this.olvNonBattleEnter.FullRowSelect = true;
            this.olvNonBattleEnter.GridLines = true;
            this.olvNonBattleEnter.HasCollapsibleGroups = false;
            this.olvNonBattleEnter.HideSelection = false;
            this.olvNonBattleEnter.Location = new System.Drawing.Point(3, 3);
            this.olvNonBattleEnter.MenuLabelGroupBy = "";
            this.olvNonBattleEnter.Name = "olvNonBattleEnter";
            this.olvNonBattleEnter.ShowGroups = false;
            this.olvNonBattleEnter.Size = new System.Drawing.Size(903, 477);
            this.olvNonBattleEnter.TabIndex = 6;
            this.olvNonBattleEnter.UseAlternatingBackColors = true;
            this.olvNonBattleEnter.UseCompatibleStateImageBehavior = false;
            this.olvNonBattleEnter.View = System.Windows.Forms.View.Details;
            this.olvNonBattleEnter.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcNonBattleEnterName
            // 
            this.lvcNonBattleEnterName.AspectName = "Name";
            this.lvcNonBattleEnterName.IsEditable = false;
            this.lvcNonBattleEnterName.Text = "EnterName";
            this.lvcNonBattleEnterName.Width = 70;
            // 
            // lvcNonBattleEnterID
            // 
            this.lvcNonBattleEnterID.AspectName = "ID";
            this.lvcNonBattleEnterID.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterID.IsEditable = false;
            this.lvcNonBattleEnterID.Text = "EnterId";
            this.lvcNonBattleEnterID.Width = 50;
            // 
            // lvcNonBattleEnterAddress
            // 
            this.lvcNonBattleEnterAddress.AspectName = "Address";
            this.lvcNonBattleEnterAddress.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterAddress.IsEditable = false;
            this.lvcNonBattleEnterAddress.Text = "Address";
            this.lvcNonBattleEnterAddress.Width = 50;
            // 
            // lvcNonBattleEnterSceneNumber
            // 
            this.lvcNonBattleEnterSceneNumber.AspectName = "Entered";
            this.lvcNonBattleEnterSceneNumber.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterSceneNumber.Text = "Scene#";
            // 
            // lvcNonBattleEnterPlus0x02
            // 
            this.lvcNonBattleEnterPlus0x02.AspectName = "EnterUnknown2";
            this.lvcNonBattleEnterPlus0x02.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterPlus0x02.Text = "+0x02";
            this.lvcNonBattleEnterPlus0x02.Width = 55;
            // 
            // lvcNonBattleEnterXPos
            // 
            this.lvcNonBattleEnterXPos.AspectName = "EnterXPos";
            this.lvcNonBattleEnterXPos.AspectToStringFormat = "";
            this.lvcNonBattleEnterXPos.Text = "xPos(Dec)";
            this.lvcNonBattleEnterXPos.Width = 70;
            // 
            // lvcNonBattleEnterPlus0x06
            // 
            this.lvcNonBattleEnterPlus0x06.AspectName = "EnterUnknown6";
            this.lvcNonBattleEnterPlus0x06.AspectToStringFormat = "";
            this.lvcNonBattleEnterPlus0x06.Text = "+0x06";
            this.lvcNonBattleEnterPlus0x06.Width = 55;
            // 
            // lvcNonBattleEnterZPos
            // 
            this.lvcNonBattleEnterZPos.AspectName = "EnterZPos";
            this.lvcNonBattleEnterZPos.AspectToStringFormat = "";
            this.lvcNonBattleEnterZPos.Text = "zPos(Dec)";
            this.lvcNonBattleEnterZPos.Width = 70;
            // 
            // lvcNonBattleEnterDirection
            // 
            this.lvcNonBattleEnterDirection.AspectName = "EnterDirection";
            this.lvcNonBattleEnterDirection.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterDirection.Text = "Direction";
            // 
            // lvcNonBattleEnterCamera
            // 
            this.lvcNonBattleEnterCamera.AspectName = "EnterCamera";
            this.lvcNonBattleEnterCamera.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterCamera.Text = "Camera";
            this.lvcNonBattleEnterCamera.Width = 50;
            // 
            // lvcNonBattleEnterPlus0x0E
            // 
            this.lvcNonBattleEnterPlus0x0E.AspectName = "EnterUnknownE";
            this.lvcNonBattleEnterPlus0x0E.AspectToStringFormat = "{0:X}";
            this.lvcNonBattleEnterPlus0x0E.Text = "+0x0E";
            this.lvcNonBattleEnterPlus0x0E.Width = 55;
            // 
            // tabTownNpcs
            // 
            this.tabTownNpcs.Controls.Add(this.olvTownNpcs);
            this.tabTownNpcs.Location = new System.Drawing.Point(4, 22);
            this.tabTownNpcs.Name = "tabTownNpcs";
            this.tabTownNpcs.Padding = new System.Windows.Forms.Padding(3);
            this.tabTownNpcs.Size = new System.Drawing.Size(909, 483);
            this.tabTownNpcs.TabIndex = 15;
            this.tabTownNpcs.Text = "TownNpcs";
            this.tabTownNpcs.UseVisualStyleBackColor = true;
            // 
            // olvTownNpcs
            // 
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsName);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsID);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsAddress);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsSpriteID);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x02);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsMonsterTable);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsXPos);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x0A);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x0C);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x0E);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsZPos);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x12);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsDirection);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsPlus0x16);
            this.olvTownNpcs.AllColumns.Add(this.lvcTownNpcsTiedToEventNumber);
            this.olvTownNpcs.AllowColumnReorder = true;
            this.olvTownNpcs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvTownNpcs.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvTownNpcs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcTownNpcsName,
            this.lvcTownNpcsID,
            this.lvcTownNpcsAddress,
            this.lvcTownNpcsSpriteID,
            this.lvcTownNpcsPlus0x02,
            this.lvcTownNpcsMonsterTable,
            this.lvcTownNpcsXPos,
            this.lvcTownNpcsPlus0x0A,
            this.lvcTownNpcsPlus0x0C,
            this.lvcTownNpcsPlus0x0E,
            this.lvcTownNpcsZPos,
            this.lvcTownNpcsPlus0x12,
            this.lvcTownNpcsDirection,
            this.lvcTownNpcsPlus0x16,
            this.lvcTownNpcsTiedToEventNumber});
            this.olvTownNpcs.FullRowSelect = true;
            this.olvTownNpcs.GridLines = true;
            this.olvTownNpcs.HasCollapsibleGroups = false;
            this.olvTownNpcs.HideSelection = false;
            this.olvTownNpcs.Location = new System.Drawing.Point(3, 3);
            this.olvTownNpcs.MenuLabelGroupBy = "";
            this.olvTownNpcs.Name = "olvTownNpcs";
            this.olvTownNpcs.ShowGroups = false;
            this.olvTownNpcs.Size = new System.Drawing.Size(903, 477);
            this.olvTownNpcs.TabIndex = 5;
            this.olvTownNpcs.UseAlternatingBackColors = true;
            this.olvTownNpcs.UseCompatibleStateImageBehavior = false;
            this.olvTownNpcs.View = System.Windows.Forms.View.Details;
            this.olvTownNpcs.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcTownNpcsName
            // 
            this.lvcTownNpcsName.AspectName = "Name";
            this.lvcTownNpcsName.IsEditable = false;
            this.lvcTownNpcsName.Text = "NpcName";
            this.lvcTownNpcsName.Width = 70;
            // 
            // lvcTownNpcsID
            // 
            this.lvcTownNpcsID.AspectName = "ID";
            this.lvcTownNpcsID.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsID.IsEditable = false;
            this.lvcTownNpcsID.Text = "ID";
            this.lvcTownNpcsID.Width = 50;
            // 
            // lvcTownNpcsAddress
            // 
            this.lvcTownNpcsAddress.AspectName = "Address";
            this.lvcTownNpcsAddress.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsAddress.IsEditable = false;
            this.lvcTownNpcsAddress.Text = "Address";
            this.lvcTownNpcsAddress.Width = 50;
            // 
            // lvcTownNpcsSpriteID
            // 
            this.lvcTownNpcsSpriteID.AspectName = "SpriteID";
            this.lvcTownNpcsSpriteID.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsSpriteID.Text = "SpriteID";
            // 
            // lvcTownNpcsPlus0x02
            // 
            this.lvcTownNpcsPlus0x02.AspectName = "NpcUnknown";
            this.lvcTownNpcsPlus0x02.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x02.Text = "+0x02";
            this.lvcTownNpcsPlus0x02.Width = 55;
            // 
            // lvcTownNpcsMonsterTable
            // 
            this.lvcTownNpcsMonsterTable.AspectName = "NpcTable";
            this.lvcTownNpcsMonsterTable.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsMonsterTable.Text = "MovementTable?";
            this.lvcTownNpcsMonsterTable.Width = 95;
            // 
            // lvcTownNpcsXPos
            // 
            this.lvcTownNpcsXPos.AspectName = "NpcXPos";
            this.lvcTownNpcsXPos.AspectToStringFormat = "";
            this.lvcTownNpcsXPos.Text = "xPos(dec)";
            // 
            // lvcTownNpcsPlus0x0A
            // 
            this.lvcTownNpcsPlus0x0A.AspectName = "NpcUnknownA";
            this.lvcTownNpcsPlus0x0A.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x0A.Text = "+0x0A";
            this.lvcTownNpcsPlus0x0A.Width = 50;
            // 
            // lvcTownNpcsPlus0x0C
            // 
            this.lvcTownNpcsPlus0x0C.AspectName = "NpcUnknownC";
            this.lvcTownNpcsPlus0x0C.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x0C.Text = "+0x0C";
            this.lvcTownNpcsPlus0x0C.Width = 50;
            // 
            // lvcTownNpcsPlus0x0E
            // 
            this.lvcTownNpcsPlus0x0E.AspectName = "NpcUnknownE";
            this.lvcTownNpcsPlus0x0E.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x0E.Text = "+0x0E";
            this.lvcTownNpcsPlus0x0E.Width = 50;
            // 
            // lvcTownNpcsZPos
            // 
            this.lvcTownNpcsZPos.AspectName = "NpcZPos";
            this.lvcTownNpcsZPos.AspectToStringFormat = "";
            this.lvcTownNpcsZPos.Text = "zPos(dec)";
            // 
            // lvcTownNpcsPlus0x12
            // 
            this.lvcTownNpcsPlus0x12.AspectName = "NpcUnknown12";
            this.lvcTownNpcsPlus0x12.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x12.Text = "+0x12";
            this.lvcTownNpcsPlus0x12.Width = 50;
            // 
            // lvcTownNpcsDirection
            // 
            this.lvcTownNpcsDirection.AspectName = "NpcDirection";
            this.lvcTownNpcsDirection.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsDirection.Text = "Direction";
            // 
            // lvcTownNpcsPlus0x16
            // 
            this.lvcTownNpcsPlus0x16.AspectName = "NpcUnknown16";
            this.lvcTownNpcsPlus0x16.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsPlus0x16.Text = "+0x16";
            this.lvcTownNpcsPlus0x16.Width = 50;
            // 
            // lvcTownNpcsTiedToEventNumber
            // 
            this.lvcTownNpcsTiedToEventNumber.AspectName = "NpcTieIn";
            this.lvcTownNpcsTiedToEventNumber.AspectToStringFormat = "{0:X}";
            this.lvcTownNpcsTiedToEventNumber.IsEditable = false;
            this.lvcTownNpcsTiedToEventNumber.Text = "TiedToEventNumber";
            this.lvcTownNpcsTiedToEventNumber.Width = 120;
            // 
            // tabInteractables
            // 
            this.tabInteractables.Controls.Add(this.olvInteractables);
            this.tabInteractables.Location = new System.Drawing.Point(4, 22);
            this.tabInteractables.Name = "tabInteractables";
            this.tabInteractables.Padding = new System.Windows.Forms.Padding(3);
            this.tabInteractables.Size = new System.Drawing.Size(909, 483);
            this.tabInteractables.TabIndex = 11;
            this.tabInteractables.Text = "Interactables";
            this.tabInteractables.UseVisualStyleBackColor = true;
            // 
            // olvInteractables
            // 
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesName);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesID);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesAddress);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesDirectionSearched);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesEventNumber);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesFlagUsed);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesUnknown1);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesEventTypeCode);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesItemTextCode);
            this.olvInteractables.AllColumns.Add(this.lvcInteractablesMPDTieInID);
            this.olvInteractables.AllowColumnReorder = true;
            this.olvInteractables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvInteractables.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvInteractables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcInteractablesName,
            this.lvcInteractablesID,
            this.lvcInteractablesAddress,
            this.lvcInteractablesDirectionSearched,
            this.lvcInteractablesEventNumber,
            this.lvcInteractablesFlagUsed,
            this.lvcInteractablesUnknown1,
            this.lvcInteractablesEventTypeCode,
            this.lvcInteractablesItemTextCode,
            this.lvcInteractablesMPDTieInID});
            this.olvInteractables.FullRowSelect = true;
            this.olvInteractables.GridLines = true;
            this.olvInteractables.HasCollapsibleGroups = false;
            this.olvInteractables.HideSelection = false;
            this.olvInteractables.Location = new System.Drawing.Point(3, 3);
            this.olvInteractables.MenuLabelGroupBy = "";
            this.olvInteractables.Name = "olvInteractables";
            this.olvInteractables.ShowGroups = false;
            this.olvInteractables.Size = new System.Drawing.Size(903, 477);
            this.olvInteractables.TabIndex = 4;
            this.olvInteractables.UseAlternatingBackColors = true;
            this.olvInteractables.UseCompatibleStateImageBehavior = false;
            this.olvInteractables.View = System.Windows.Forms.View.Details;
            this.olvInteractables.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcInteractablesName
            // 
            this.lvcInteractablesName.AspectName = "Name";
            this.lvcInteractablesName.IsEditable = false;
            this.lvcInteractablesName.Text = "InteractableName";
            this.lvcInteractablesName.Width = 100;
            // 
            // lvcInteractablesID
            // 
            this.lvcInteractablesID.AspectName = "ID";
            this.lvcInteractablesID.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesID.IsEditable = false;
            this.lvcInteractablesID.Text = "Hex ID";
            this.lvcInteractablesID.Width = 50;
            // 
            // lvcInteractablesAddress
            // 
            this.lvcInteractablesAddress.AspectName = "Address";
            this.lvcInteractablesAddress.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesAddress.IsEditable = false;
            this.lvcInteractablesAddress.Text = "Address";
            this.lvcInteractablesAddress.Width = 50;
            // 
            // lvcInteractablesDirectionSearched
            // 
            this.lvcInteractablesDirectionSearched.AspectName = "Searched";
            this.lvcInteractablesDirectionSearched.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesDirectionSearched.Text = "Direction/Searched";
            this.lvcInteractablesDirectionSearched.Width = 110;
            // 
            // lvcInteractablesEventNumber
            // 
            this.lvcInteractablesEventNumber.AspectName = "EventNumber";
            this.lvcInteractablesEventNumber.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesEventNumber.Text = "EventNumber";
            this.lvcInteractablesEventNumber.Width = 82;
            // 
            // lvcInteractablesFlagUsed
            // 
            this.lvcInteractablesFlagUsed.AspectName = "FlagUse";
            this.lvcInteractablesFlagUsed.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesFlagUsed.Text = "FlagUsed";
            // 
            // lvcInteractablesUnknown1
            // 
            this.lvcInteractablesUnknown1.AspectName = "UnknownTreasure";
            this.lvcInteractablesUnknown1.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesUnknown1.Text = "Unknown1";
            this.lvcInteractablesUnknown1.Width = 70;
            // 
            // lvcInteractablesEventTypeCode
            // 
            this.lvcInteractablesEventTypeCode.AspectName = "EventType";
            this.lvcInteractablesEventTypeCode.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesEventTypeCode.Text = "EventType/Code";
            this.lvcInteractablesEventTypeCode.Width = 95;
            // 
            // lvcInteractablesItemTextCode
            // 
            this.lvcInteractablesItemTextCode.AspectName = "EventParameter";
            this.lvcInteractablesItemTextCode.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesItemTextCode.Text = "Item/Text/Code";
            this.lvcInteractablesItemTextCode.Width = 120;
            // 
            // lvcInteractablesMPDTieInID
            // 
            this.lvcInteractablesMPDTieInID.AspectName = "MPDTieIn";
            this.lvcInteractablesMPDTieInID.AspectToStringFormat = "{0:X}";
            this.lvcInteractablesMPDTieInID.IsEditable = false;
            this.lvcInteractablesMPDTieInID.Text = "MPDTieInID";
            this.lvcInteractablesMPDTieInID.Width = 80;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabInteractables);
            this.tabMain.Controls.Add(this.tabBattlePointers);
            this.tabMain.Controls.Add(this.tabTownNpcs);
            this.tabMain.Controls.Add(this.tabNonBattleEnter);
            this.tabMain.Controls.Add(this.tabWarpTable);
            this.tabMain.Controls.Add(this.tabArrows);
            this.tabMain.Controls.Add(this.tabTileData);
            this.tabMain.Controls.Add(this.tabBattle_Synbios);
            this.tabMain.Controls.Add(this.tabBattle_Medion);
            this.tabMain.Controls.Add(this.tabBattle_Julian);
            this.tabMain.Controls.Add(this.tabBattle_Extra);
            this.tabMain.Location = new System.Drawing.Point(0, 24);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(917, 509);
            this.tabMain.TabIndex = 0;
            // 
            // tabBattle_Synbios
            // 
            this.tabBattle_Synbios.Controls.Add(this.becBattle_Synbios);
            this.tabBattle_Synbios.Location = new System.Drawing.Point(4, 22);
            this.tabBattle_Synbios.Name = "tabBattle_Synbios";
            this.tabBattle_Synbios.Size = new System.Drawing.Size(909, 483);
            this.tabBattle_Synbios.TabIndex = 18;
            this.tabBattle_Synbios.Text = "Battle (Synbios)";
            this.tabBattle_Synbios.UseVisualStyleBackColor = true;
            // 
            // becBattle_Synbios
            // 
            this.becBattle_Synbios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.becBattle_Synbios.Location = new System.Drawing.Point(0, 0);
            this.becBattle_Synbios.Margin = new System.Windows.Forms.Padding(0);
            this.becBattle_Synbios.Name = "becBattle_Synbios";
            this.becBattle_Synbios.Size = new System.Drawing.Size(913, 487);
            this.becBattle_Synbios.TabIndex = 0;
            // 
            // tabBattle_Medion
            // 
            this.tabBattle_Medion.Controls.Add(this.becBattle_Medion);
            this.tabBattle_Medion.Location = new System.Drawing.Point(4, 22);
            this.tabBattle_Medion.Name = "tabBattle_Medion";
            this.tabBattle_Medion.Size = new System.Drawing.Size(909, 483);
            this.tabBattle_Medion.TabIndex = 19;
            this.tabBattle_Medion.Text = "Battle (Medion)";
            this.tabBattle_Medion.UseVisualStyleBackColor = true;
            // 
            // becBattle_Medion
            // 
            this.becBattle_Medion.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.becBattle_Medion.Location = new System.Drawing.Point(0, 0);
            this.becBattle_Medion.Margin = new System.Windows.Forms.Padding(0);
            this.becBattle_Medion.Name = "becBattle_Medion";
            this.becBattle_Medion.Size = new System.Drawing.Size(913, 487);
            this.becBattle_Medion.TabIndex = 1;
            // 
            // tabBattle_Julian
            // 
            this.tabBattle_Julian.Controls.Add(this.becBattle_Julian);
            this.tabBattle_Julian.Location = new System.Drawing.Point(4, 22);
            this.tabBattle_Julian.Name = "tabBattle_Julian";
            this.tabBattle_Julian.Size = new System.Drawing.Size(909, 483);
            this.tabBattle_Julian.TabIndex = 20;
            this.tabBattle_Julian.Text = "Battle (Julian)";
            this.tabBattle_Julian.UseVisualStyleBackColor = true;
            // 
            // becBattle_Julian
            // 
            this.becBattle_Julian.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.becBattle_Julian.Location = new System.Drawing.Point(0, 0);
            this.becBattle_Julian.Margin = new System.Windows.Forms.Padding(0);
            this.becBattle_Julian.Name = "becBattle_Julian";
            this.becBattle_Julian.Size = new System.Drawing.Size(913, 487);
            this.becBattle_Julian.TabIndex = 1;
            // 
            // tabBattle_Extra
            // 
            this.tabBattle_Extra.Controls.Add(this.becBattle_Extra);
            this.tabBattle_Extra.Location = new System.Drawing.Point(4, 22);
            this.tabBattle_Extra.Name = "tabBattle_Extra";
            this.tabBattle_Extra.Size = new System.Drawing.Size(909, 483);
            this.tabBattle_Extra.TabIndex = 21;
            this.tabBattle_Extra.Text = "Battle (Extra)";
            this.tabBattle_Extra.UseVisualStyleBackColor = true;
            // 
            // becBattle_Extra
            // 
            this.becBattle_Extra.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.becBattle_Extra.Location = new System.Drawing.Point(0, 0);
            this.becBattle_Extra.Margin = new System.Windows.Forms.Padding(0);
            this.becBattle_Extra.Name = "becBattle_Extra";
            this.becBattle_Extra.Size = new System.Drawing.Size(913, 487);
            this.becBattle_Extra.TabIndex = 1;
            // 
            // tsmiScenario
            // 
            this.tsmiScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsmiScenario_BTL99});
            this.tsmiScenario.Name = "tsmiScenario";
            this.tsmiScenario.Size = new System.Drawing.Size(64, 20);
            this.tsmiScenario.Text = "&Scenario";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // tsmiScenario_BTL99
            // 
            this.tsmiScenario_BTL99.Name = "tsmiScenario_BTL99";
            this.tsmiScenario_BTL99.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.tsmiScenario_BTL99.Size = new System.Drawing.Size(183, 22);
            this.tsmiScenario_BTL99.Text = "&BTL99 Toggle";
            this.tsmiScenario_BTL99.Click += new System.EventHandler(this.tsmiScenario_BTL99_Click);
            // 
            // menuStrip2
            // 
            this.menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiScenario});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(73, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // frmX1_Editor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 534);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmX1_Editor";
            this.Text = "SF3 X1 Editor";
            this.Controls.SetChildIndex(this.tabMain, 0);
            this.Controls.SetChildIndex(this.menuStrip2, 0);
            this.tabTileData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvTileData)).EndInit();
            this.tabWarpTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvWarpTable)).EndInit();
            this.tabBattlePointers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvBattlePointers)).EndInit();
            this.tabArrows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvArrows)).EndInit();
            this.tabNonBattleEnter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvNonBattleEnter)).EndInit();
            this.tabTownNpcs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvTownNpcs)).EndInit();
            this.tabInteractables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvInteractables)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabBattle_Synbios.ResumeLayout(false);
            this.tabBattle_Medion.ResumeLayout(false);
            this.tabBattle_Julian.ResumeLayout(false);
            this.tabBattle_Extra.ResumeLayout(false);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabPage tabTileData;
        private BrightIdeasSoftware.ObjectListView olvTileData;
        private BrightIdeasSoftware.OLVColumn lvcTileDataName;
        private BrightIdeasSoftware.OLVColumn lvcTileDataID;
        private BrightIdeasSoftware.OLVColumn lvcTileDataAddress;
        private BrightIdeasSoftware.OLVColumn lvcTileDataNoEntry;
        private BrightIdeasSoftware.OLVColumn lvcTileDataAir;
        private BrightIdeasSoftware.OLVColumn lvcTileDataGrassland;
        private BrightIdeasSoftware.OLVColumn lvcTileDataDirt;
        private BrightIdeasSoftware.OLVColumn lvcTileDataDarkGrass;
        private BrightIdeasSoftware.OLVColumn lvcTileDataForest;
        private BrightIdeasSoftware.OLVColumn lvcTileDataBrownMountain;
        private BrightIdeasSoftware.OLVColumn lvcTileDataDesert;
        private BrightIdeasSoftware.OLVColumn lvcTileDataGreyMountain;
        private BrightIdeasSoftware.OLVColumn lvcTileDataWater;
        private BrightIdeasSoftware.OLVColumn lvcTileDataUnknownA;
        private BrightIdeasSoftware.OLVColumn lvcTileDataSand;
        private BrightIdeasSoftware.OLVColumn lvcTileDataUnknownC;
        private BrightIdeasSoftware.OLVColumn lvcTileDataUnknownD;
        private BrightIdeasSoftware.OLVColumn lvcTileDataUnknownE;
        private BrightIdeasSoftware.OLVColumn lvcTileDataUnknownF;
        private System.Windows.Forms.TabPage tabWarpTable;
        private BrightIdeasSoftware.ObjectListView olvWarpTable;
        private BrightIdeasSoftware.OLVColumn lvcWarpTableName;
        private BrightIdeasSoftware.OLVColumn lvcWarpTableID;
        private BrightIdeasSoftware.OLVColumn lvcWarpTableAddress;
        private BrightIdeasSoftware.OLVColumn lvcWarpTablePlus0x00;
        private BrightIdeasSoftware.OLVColumn lvcWarpTablePlus0x01;
        private BrightIdeasSoftware.OLVColumn lvcWarpTableType;
        private BrightIdeasSoftware.OLVColumn lvcWarpTableMap;
        private System.Windows.Forms.TabPage tabBattlePointers;
        private BrightIdeasSoftware.ObjectListView olvBattlePointers;
        private BrightIdeasSoftware.OLVColumn lvcBattlePointersName;
        private BrightIdeasSoftware.OLVColumn lvcBattlePointersID;
        private BrightIdeasSoftware.OLVColumn lvcBattlePointersAddress;
        private BrightIdeasSoftware.OLVColumn lvcBattlePointersPointer;
        private System.Windows.Forms.TabPage tabArrows;
        private BrightIdeasSoftware.ObjectListView olvArrows;
        private BrightIdeasSoftware.OLVColumn lvcArrowsName;
        private BrightIdeasSoftware.OLVColumn lvcArrowsID;
        private BrightIdeasSoftware.OLVColumn lvcArrowsAddress;
        private BrightIdeasSoftware.OLVColumn lvcArrowsPlus0x00;
        private BrightIdeasSoftware.OLVColumn lvcArrowsTextID;
        private BrightIdeasSoftware.OLVColumn lvcArrowsPlus0x04;
        private BrightIdeasSoftware.OLVColumn lvcArrowsPointToWarpMPD;
        private BrightIdeasSoftware.OLVColumn lvcArrowsPlus0x08;
        private BrightIdeasSoftware.OLVColumn lvcArrowsPlus0x0A;
        private System.Windows.Forms.TabPage tabNonBattleEnter;
        private BrightIdeasSoftware.ObjectListView olvNonBattleEnter;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterName;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterID;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterAddress;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterSceneNumber;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterPlus0x02;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterXPos;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterPlus0x06;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterZPos;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterDirection;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterCamera;
        private BrightIdeasSoftware.OLVColumn lvcNonBattleEnterPlus0x0E;
        private System.Windows.Forms.TabPage tabTownNpcs;
        private BrightIdeasSoftware.ObjectListView olvTownNpcs;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsName;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsID;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsAddress;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsSpriteID;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x02;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsMonsterTable;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsXPos;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x0A;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x0C;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x0E;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsZPos;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x12;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsDirection;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsPlus0x16;
        private BrightIdeasSoftware.OLVColumn lvcTownNpcsTiedToEventNumber;
        private System.Windows.Forms.TabPage tabInteractables;
        private BrightIdeasSoftware.ObjectListView olvInteractables;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesName;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesID;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesAddress;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesDirectionSearched;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesEventNumber;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesFlagUsed;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesUnknown1;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesEventTypeCode;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesItemTextCode;
        private BrightIdeasSoftware.OLVColumn lvcInteractablesMPDTieInID;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabBattle_Synbios;
        private Controls.BattleEditorControl becBattle_Synbios;
        private System.Windows.Forms.TabPage tabBattle_Medion;
        private Controls.BattleEditorControl becBattle_Medion;
        private System.Windows.Forms.TabPage tabBattle_Julian;
        private Controls.BattleEditorControl becBattle_Julian;
        private System.Windows.Forms.TabPage tabBattle_Extra;
        private Controls.BattleEditorControl becBattle_Extra;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_BTL99;
        private System.Windows.Forms.MenuStrip menuStrip2;
    }
}


