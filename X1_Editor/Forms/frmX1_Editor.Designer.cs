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
        private void InitializeComponent() {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmX1_Editor));
            tabTileData = new System.Windows.Forms.TabPage();
            olvTileData = new BrightIdeasSoftware.ObjectListView();
            lvcTileDataName = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataID = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataAddress = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataNoEntry = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataAir = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataGrassland = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataDirt = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataDarkGrass = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataForest = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataBrownMountain = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataDesert = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataGreyMountain = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataWater = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataCantStay = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataSand = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataEnemyOnly = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataPlayerOnly = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataUnknownE = new BrightIdeasSoftware.OLVColumn();
            lvcTileDataUnknownF = new BrightIdeasSoftware.OLVColumn();
            tabWarpTable = new System.Windows.Forms.TabPage();
            olvWarpTable = new BrightIdeasSoftware.ObjectListView();
            lvcWarpTableName = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTableID = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTableAddress = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTablePlus0x00 = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTablePlus0x01 = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTableType = new BrightIdeasSoftware.OLVColumn();
            lvcWarpTableMap = new BrightIdeasSoftware.OLVColumn();
            tabBattlePointers = new System.Windows.Forms.TabPage();
            olvBattlePointers = new BrightIdeasSoftware.ObjectListView();
            lvcBattlePointersName = new BrightIdeasSoftware.OLVColumn();
            lvcBattlePointersID = new BrightIdeasSoftware.OLVColumn();
            lvcBattlePointersAddress = new BrightIdeasSoftware.OLVColumn();
            lvcBattlePointersPointer = new BrightIdeasSoftware.OLVColumn();
            tabArrows = new System.Windows.Forms.TabPage();
            olvArrows = new BrightIdeasSoftware.ObjectListView();
            lvcArrowsName = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsID = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsAddress = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsPlus0x00 = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsTextID = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsPlus0x04 = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsPointToWarpMPD = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsPlus0x08 = new BrightIdeasSoftware.OLVColumn();
            lvcArrowsPlus0x0A = new BrightIdeasSoftware.OLVColumn();
            tabNonBattleEnter = new System.Windows.Forms.TabPage();
            olvNonBattleEnter = new BrightIdeasSoftware.ObjectListView();
            lvcNonBattleEnterName = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterID = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterAddress = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterSceneNumber = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterPlus0x02 = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterXPos = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterPlus0x06 = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterZPos = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterDirection = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterCamera = new BrightIdeasSoftware.OLVColumn();
            lvcNonBattleEnterPlus0x0E = new BrightIdeasSoftware.OLVColumn();
            tabTownNpcs = new System.Windows.Forms.TabPage();
            olvTownNpcs = new BrightIdeasSoftware.ObjectListView();
            lvcTownNpcsName = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsID = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsAddress = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsSpriteID = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x02 = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsMonsterTable = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsXPos = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x0A = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x0C = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x0E = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsZPos = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x12 = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsDirection = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsPlus0x16 = new BrightIdeasSoftware.OLVColumn();
            lvcTownNpcsTiedToEventNumber = new BrightIdeasSoftware.OLVColumn();
            tabInteractables = new System.Windows.Forms.TabPage();
            olvInteractables = new BrightIdeasSoftware.ObjectListView();
            lvcInteractablesName = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesID = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesAddress = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesDirectionSearched = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesEventNumber = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesFlagUsed = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesUnknown1 = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesEventTypeCode = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesItemTextCode = new BrightIdeasSoftware.OLVColumn();
            lvcInteractablesMPDTieInID = new BrightIdeasSoftware.OLVColumn();
            tabMain = new System.Windows.Forms.TabControl();
            tabBattle_Synbios = new System.Windows.Forms.TabPage();
            becBattle_Synbios = new Controls.BattleEditorControl();
            tabBattle_Medion = new System.Windows.Forms.TabPage();
            becBattle_Medion = new Controls.BattleEditorControl();
            tabBattle_Julian = new System.Windows.Forms.TabPage();
            becBattle_Julian = new Controls.BattleEditorControl();
            tabBattle_Extra = new System.Windows.Forms.TabPage();
            becBattle_Extra = new Controls.BattleEditorControl();
            tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiScenario_BTL99 = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            tabTileData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvTileData).BeginInit();
            tabWarpTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvWarpTable).BeginInit();
            tabBattlePointers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvBattlePointers).BeginInit();
            tabArrows.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvArrows).BeginInit();
            tabNonBattleEnter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvNonBattleEnter).BeginInit();
            tabTownNpcs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvTownNpcs).BeginInit();
            tabInteractables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvInteractables).BeginInit();
            tabMain.SuspendLayout();
            tabBattle_Synbios.SuspendLayout();
            tabBattle_Medion.SuspendLayout();
            tabBattle_Julian.SuspendLayout();
            tabBattle_Extra.SuspendLayout();
            menuStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // tabTileData
            // 
            tabTileData.Controls.Add(olvTileData);
            tabTileData.Location = new System.Drawing.Point(4, 24);
            tabTileData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabTileData.Name = "tabTileData";
            tabTileData.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabTileData.Size = new System.Drawing.Size(1062, 559);
            tabTileData.TabIndex = 14;
            tabTileData.Text = "TileData scn2,3,pd";
            tabTileData.UseVisualStyleBackColor = true;
            // 
            // olvTileData
            // 
            olvTileData.AllColumns.Add(lvcTileDataName);
            olvTileData.AllColumns.Add(lvcTileDataID);
            olvTileData.AllColumns.Add(lvcTileDataAddress);
            olvTileData.AllColumns.Add(lvcTileDataNoEntry);
            olvTileData.AllColumns.Add(lvcTileDataAir);
            olvTileData.AllColumns.Add(lvcTileDataGrassland);
            olvTileData.AllColumns.Add(lvcTileDataDirt);
            olvTileData.AllColumns.Add(lvcTileDataDarkGrass);
            olvTileData.AllColumns.Add(lvcTileDataForest);
            olvTileData.AllColumns.Add(lvcTileDataBrownMountain);
            olvTileData.AllColumns.Add(lvcTileDataDesert);
            olvTileData.AllColumns.Add(lvcTileDataGreyMountain);
            olvTileData.AllColumns.Add(lvcTileDataWater);
            olvTileData.AllColumns.Add(lvcTileDataCantStay);
            olvTileData.AllColumns.Add(lvcTileDataSand);
            olvTileData.AllColumns.Add(lvcTileDataEnemyOnly);
            olvTileData.AllColumns.Add(lvcTileDataPlayerOnly);
            olvTileData.AllColumns.Add(lvcTileDataUnknownE);
            olvTileData.AllColumns.Add(lvcTileDataUnknownF);
            olvTileData.AllowColumnReorder = true;
            olvTileData.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvTileData.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvTileData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcTileDataName, lvcTileDataID, lvcTileDataAddress, lvcTileDataNoEntry, lvcTileDataAir, lvcTileDataGrassland, lvcTileDataDirt, lvcTileDataDarkGrass, lvcTileDataForest, lvcTileDataBrownMountain, lvcTileDataDesert, lvcTileDataGreyMountain, lvcTileDataWater, lvcTileDataCantStay, lvcTileDataSand, lvcTileDataEnemyOnly, lvcTileDataPlayerOnly, lvcTileDataUnknownE, lvcTileDataUnknownF });
            olvTileData.FullRowSelect = true;
            olvTileData.GridLines = true;
            olvTileData.HasCollapsibleGroups = false;
            olvTileData.Location = new System.Drawing.Point(4, 3);
            olvTileData.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvTileData.MenuLabelGroupBy = "";
            olvTileData.Name = "olvTileData";
            olvTileData.ShowGroups = false;
            olvTileData.Size = new System.Drawing.Size(1053, 550);
            olvTileData.TabIndex = 2;
            olvTileData.UseAlternatingBackColors = true;
            olvTileData.UseCompatibleStateImageBehavior = false;
            olvTileData.View = System.Windows.Forms.View.Details;
            // 
            // lvcTileDataName
            // 
            lvcTileDataName.AspectName = "Name";
            lvcTileDataName.IsEditable = false;
            lvcTileDataName.Text = "Name";
            lvcTileDataName.Width = 90;
            // 
            // lvcTileDataID
            // 
            lvcTileDataID.AspectName = "ID";
            lvcTileDataID.AspectToStringFormat = "{0:X}";
            lvcTileDataID.IsEditable = false;
            lvcTileDataID.Text = "Hex ID";
            lvcTileDataID.Width = 50;
            // 
            // lvcTileDataAddress
            // 
            lvcTileDataAddress.AspectName = "Address";
            lvcTileDataAddress.AspectToStringFormat = "{0:X4}";
            lvcTileDataAddress.IsEditable = false;
            lvcTileDataAddress.Text = "Address";
            lvcTileDataAddress.Width = 50;
            // 
            // lvcTileDataNoEntry
            // 
            lvcTileDataNoEntry.AspectName = "NoEntry";
            lvcTileDataNoEntry.AspectToStringFormat = "{0:X}";
            lvcTileDataNoEntry.Text = "NoEntry";
            lvcTileDataNoEntry.Width = 50;
            // 
            // lvcTileDataAir
            // 
            lvcTileDataAir.AspectName = "Air";
            lvcTileDataAir.AspectToStringFormat = "{0:X}";
            lvcTileDataAir.Text = "Air";
            lvcTileDataAir.Width = 40;
            // 
            // lvcTileDataGrassland
            // 
            lvcTileDataGrassland.AspectName = "Grassland";
            lvcTileDataGrassland.AspectToStringFormat = "{0:X}";
            lvcTileDataGrassland.Text = "Grassland";
            lvcTileDataGrassland.Width = 65;
            // 
            // lvcTileDataDirt
            // 
            lvcTileDataDirt.AspectName = "Dirt";
            lvcTileDataDirt.AspectToStringFormat = "{0:X}";
            lvcTileDataDirt.Text = "Dirt";
            lvcTileDataDirt.Width = 40;
            // 
            // lvcTileDataDarkGrass
            // 
            lvcTileDataDarkGrass.AspectName = "DarkGrass";
            lvcTileDataDarkGrass.AspectToStringFormat = "{0:X}";
            lvcTileDataDarkGrass.Text = "DarkGrass";
            lvcTileDataDarkGrass.Width = 65;
            // 
            // lvcTileDataForest
            // 
            lvcTileDataForest.AspectName = "Forest";
            lvcTileDataForest.AspectToStringFormat = "{0:X}";
            lvcTileDataForest.Text = "Forest";
            lvcTileDataForest.Width = 45;
            // 
            // lvcTileDataBrownMountain
            // 
            lvcTileDataBrownMountain.AspectName = "BrownMountain";
            lvcTileDataBrownMountain.AspectToStringFormat = "{0:X}";
            lvcTileDataBrownMountain.Text = "BrownMountain";
            lvcTileDataBrownMountain.Width = 90;
            // 
            // lvcTileDataDesert
            // 
            lvcTileDataDesert.AspectName = "Desert";
            lvcTileDataDesert.AspectToStringFormat = "{0:X}";
            lvcTileDataDesert.Text = "Desert";
            lvcTileDataDesert.Width = 50;
            // 
            // lvcTileDataGreyMountain
            // 
            lvcTileDataGreyMountain.AspectName = "GreyMountain";
            lvcTileDataGreyMountain.AspectToStringFormat = "{0:X}";
            lvcTileDataGreyMountain.Text = "GreyMountain";
            lvcTileDataGreyMountain.Width = 80;
            // 
            // lvcTileDataWater
            // 
            lvcTileDataWater.AspectName = "Water";
            lvcTileDataWater.AspectToStringFormat = "{0:X}";
            lvcTileDataWater.Text = "Water";
            lvcTileDataWater.Width = 65;
            // 
            // lvcTileDataCantStay
            // 
            lvcTileDataCantStay.AspectName = "CantStay";
            lvcTileDataCantStay.AspectToStringFormat = "{0:X}";
            lvcTileDataCantStay.Text = "CantStay";
            lvcTileDataCantStay.Width = 70;
            // 
            // lvcTileDataSand
            // 
            lvcTileDataSand.AspectName = "Sand";
            lvcTileDataSand.AspectToStringFormat = "{0:X}";
            lvcTileDataSand.Text = "Sand";
            lvcTileDataSand.Width = 45;
            // 
            // lvcTileDataEnemyOnly
            // 
            lvcTileDataEnemyOnly.AspectName = "EnemyOnly";
            lvcTileDataEnemyOnly.AspectToStringFormat = "{0:X}";
            lvcTileDataEnemyOnly.Text = "EnemyOnly";
            lvcTileDataEnemyOnly.Width = 70;
            // 
            // lvcTileDataPlayerOnly
            // 
            lvcTileDataPlayerOnly.AspectName = "PlayerOnly";
            lvcTileDataPlayerOnly.AspectToStringFormat = "{0:X}";
            lvcTileDataPlayerOnly.Text = "PlayerOnly";
            lvcTileDataPlayerOnly.Width = 70;
            // 
            // lvcTileDataUnknownE
            // 
            lvcTileDataUnknownE.AspectName = "UnknownE";
            lvcTileDataUnknownE.AspectToStringFormat = "{0:X}";
            lvcTileDataUnknownE.Text = "UnknownE";
            lvcTileDataUnknownE.Width = 70;
            // 
            // lvcTileDataUnknownF
            // 
            lvcTileDataUnknownF.AspectName = "UnknownF";
            lvcTileDataUnknownF.AspectToStringFormat = "{0:X}";
            lvcTileDataUnknownF.Text = "UnknownF";
            lvcTileDataUnknownF.Width = 70;
            // 
            // tabWarpTable
            // 
            tabWarpTable.Controls.Add(olvWarpTable);
            tabWarpTable.Location = new System.Drawing.Point(4, 24);
            tabWarpTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabWarpTable.Name = "tabWarpTable";
            tabWarpTable.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabWarpTable.Size = new System.Drawing.Size(1062, 559);
            tabWarpTable.TabIndex = 13;
            tabWarpTable.Text = "WarpTable scn2,3,pd";
            tabWarpTable.UseVisualStyleBackColor = true;
            // 
            // olvWarpTable
            // 
            olvWarpTable.AllColumns.Add(lvcWarpTableName);
            olvWarpTable.AllColumns.Add(lvcWarpTableID);
            olvWarpTable.AllColumns.Add(lvcWarpTableAddress);
            olvWarpTable.AllColumns.Add(lvcWarpTablePlus0x00);
            olvWarpTable.AllColumns.Add(lvcWarpTablePlus0x01);
            olvWarpTable.AllColumns.Add(lvcWarpTableType);
            olvWarpTable.AllColumns.Add(lvcWarpTableMap);
            olvWarpTable.AllowColumnReorder = true;
            olvWarpTable.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvWarpTable.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvWarpTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcWarpTableName, lvcWarpTableID, lvcWarpTableAddress, lvcWarpTablePlus0x00, lvcWarpTablePlus0x01, lvcWarpTableType, lvcWarpTableMap });
            olvWarpTable.FullRowSelect = true;
            olvWarpTable.GridLines = true;
            olvWarpTable.HasCollapsibleGroups = false;
            olvWarpTable.Location = new System.Drawing.Point(4, 3);
            olvWarpTable.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvWarpTable.MenuLabelGroupBy = "";
            olvWarpTable.Name = "olvWarpTable";
            olvWarpTable.ShowGroups = false;
            olvWarpTable.Size = new System.Drawing.Size(1053, 550);
            olvWarpTable.TabIndex = 5;
            olvWarpTable.UseAlternatingBackColors = true;
            olvWarpTable.UseCompatibleStateImageBehavior = false;
            olvWarpTable.View = System.Windows.Forms.View.Details;
            // 
            // lvcWarpTableName
            // 
            lvcWarpTableName.AspectName = "Name";
            lvcWarpTableName.IsEditable = false;
            lvcWarpTableName.Text = "WarpName";
            lvcWarpTableName.Width = 90;
            // 
            // lvcWarpTableID
            // 
            lvcWarpTableID.AspectName = "ID";
            lvcWarpTableID.AspectToStringFormat = "{0:X}";
            lvcWarpTableID.IsEditable = false;
            lvcWarpTableID.Text = "Warp ID";
            lvcWarpTableID.Width = 53;
            // 
            // lvcWarpTableAddress
            // 
            lvcWarpTableAddress.AspectName = "Address";
            lvcWarpTableAddress.AspectToStringFormat = "{0:X4}";
            lvcWarpTableAddress.IsEditable = false;
            lvcWarpTableAddress.Text = "Address";
            lvcWarpTableAddress.Width = 50;
            // 
            // lvcWarpTablePlus0x00
            // 
            lvcWarpTablePlus0x00.AspectName = "WarpUnknown1";
            lvcWarpTablePlus0x00.AspectToStringFormat = "{0:X}";
            lvcWarpTablePlus0x00.Text = "+0x00";
            lvcWarpTablePlus0x00.Width = 110;
            // 
            // lvcWarpTablePlus0x01
            // 
            lvcWarpTablePlus0x01.AspectName = "WarpUnknown2";
            lvcWarpTablePlus0x01.AspectToStringFormat = "{0:X}";
            lvcWarpTablePlus0x01.Text = "+0x01";
            lvcWarpTablePlus0x01.Width = 82;
            // 
            // lvcWarpTableType
            // 
            lvcWarpTableType.AspectName = "WarpType";
            lvcWarpTableType.AspectToStringFormat = "{0:X}";
            lvcWarpTableType.Text = "Type";
            // 
            // lvcWarpTableMap
            // 
            lvcWarpTableMap.AspectName = "WarpMap";
            lvcWarpTableMap.AspectToStringFormat = "{0:X}";
            lvcWarpTableMap.Text = "Map";
            lvcWarpTableMap.Width = 70;
            // 
            // tabBattlePointers
            // 
            tabBattlePointers.Controls.Add(olvBattlePointers);
            tabBattlePointers.Location = new System.Drawing.Point(4, 24);
            tabBattlePointers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattlePointers.Name = "tabBattlePointers";
            tabBattlePointers.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattlePointers.Size = new System.Drawing.Size(1062, 559);
            tabBattlePointers.TabIndex = 10;
            tabBattlePointers.Text = "Battle Pointers";
            tabBattlePointers.UseVisualStyleBackColor = true;
            // 
            // olvBattlePointers
            // 
            olvBattlePointers.AllColumns.Add(lvcBattlePointersName);
            olvBattlePointers.AllColumns.Add(lvcBattlePointersID);
            olvBattlePointers.AllColumns.Add(lvcBattlePointersAddress);
            olvBattlePointers.AllColumns.Add(lvcBattlePointersPointer);
            olvBattlePointers.AllowColumnReorder = true;
            olvBattlePointers.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvBattlePointers.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvBattlePointers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcBattlePointersName, lvcBattlePointersID, lvcBattlePointersAddress, lvcBattlePointersPointer });
            olvBattlePointers.FullRowSelect = true;
            olvBattlePointers.GridLines = true;
            olvBattlePointers.HasCollapsibleGroups = false;
            olvBattlePointers.Location = new System.Drawing.Point(4, 3);
            olvBattlePointers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvBattlePointers.MenuLabelGroupBy = "";
            olvBattlePointers.Name = "olvBattlePointers";
            olvBattlePointers.ShowGroups = false;
            olvBattlePointers.Size = new System.Drawing.Size(1053, 550);
            olvBattlePointers.TabIndex = 3;
            olvBattlePointers.UseAlternatingBackColors = true;
            olvBattlePointers.UseCompatibleStateImageBehavior = false;
            olvBattlePointers.View = System.Windows.Forms.View.Details;
            // 
            // lvcBattlePointersName
            // 
            lvcBattlePointersName.AspectName = "Name";
            lvcBattlePointersName.IsEditable = false;
            lvcBattlePointersName.Text = "Name";
            lvcBattlePointersName.Width = 90;
            // 
            // lvcBattlePointersID
            // 
            lvcBattlePointersID.AspectName = "ID";
            lvcBattlePointersID.AspectToStringFormat = "{0:X}";
            lvcBattlePointersID.IsEditable = false;
            lvcBattlePointersID.Text = "Hex ID";
            lvcBattlePointersID.Width = 50;
            // 
            // lvcBattlePointersAddress
            // 
            lvcBattlePointersAddress.AspectName = "Address";
            lvcBattlePointersAddress.AspectToStringFormat = "{0:X4}";
            lvcBattlePointersAddress.IsEditable = false;
            lvcBattlePointersAddress.Text = "Address";
            lvcBattlePointersAddress.Width = 50;
            // 
            // lvcBattlePointersPointer
            // 
            lvcBattlePointersPointer.AspectName = "BattlePointer";
            lvcBattlePointersPointer.AspectToStringFormat = "{0:X}";
            lvcBattlePointersPointer.Text = "Pointer";
            lvcBattlePointersPointer.Width = 86;
            // 
            // tabArrows
            // 
            tabArrows.Controls.Add(olvArrows);
            tabArrows.Location = new System.Drawing.Point(4, 24);
            tabArrows.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabArrows.Name = "tabArrows";
            tabArrows.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabArrows.Size = new System.Drawing.Size(1062, 559);
            tabArrows.TabIndex = 17;
            tabArrows.Text = "Arrows scn2,3,pd";
            tabArrows.UseVisualStyleBackColor = true;
            // 
            // olvArrows
            // 
            olvArrows.AllColumns.Add(lvcArrowsName);
            olvArrows.AllColumns.Add(lvcArrowsID);
            olvArrows.AllColumns.Add(lvcArrowsAddress);
            olvArrows.AllColumns.Add(lvcArrowsPlus0x00);
            olvArrows.AllColumns.Add(lvcArrowsTextID);
            olvArrows.AllColumns.Add(lvcArrowsPlus0x04);
            olvArrows.AllColumns.Add(lvcArrowsPointToWarpMPD);
            olvArrows.AllColumns.Add(lvcArrowsPlus0x08);
            olvArrows.AllColumns.Add(lvcArrowsPlus0x0A);
            olvArrows.AllowColumnReorder = true;
            olvArrows.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvArrows.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvArrows.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcArrowsName, lvcArrowsID, lvcArrowsAddress, lvcArrowsPlus0x00, lvcArrowsTextID, lvcArrowsPlus0x04, lvcArrowsPointToWarpMPD, lvcArrowsPlus0x08, lvcArrowsPlus0x0A });
            olvArrows.FullRowSelect = true;
            olvArrows.GridLines = true;
            olvArrows.HasCollapsibleGroups = false;
            olvArrows.Location = new System.Drawing.Point(4, 3);
            olvArrows.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvArrows.MenuLabelGroupBy = "";
            olvArrows.Name = "olvArrows";
            olvArrows.ShowGroups = false;
            olvArrows.Size = new System.Drawing.Size(1053, 550);
            olvArrows.TabIndex = 7;
            olvArrows.UseAlternatingBackColors = true;
            olvArrows.UseCompatibleStateImageBehavior = false;
            olvArrows.View = System.Windows.Forms.View.Details;
            // 
            // lvcArrowsName
            // 
            lvcArrowsName.AspectName = "Name";
            lvcArrowsName.IsEditable = false;
            lvcArrowsName.Text = "ArrowName";
            lvcArrowsName.Width = 70;
            // 
            // lvcArrowsID
            // 
            lvcArrowsID.AspectName = "ID";
            lvcArrowsID.AspectToStringFormat = "{0:X}";
            lvcArrowsID.IsEditable = false;
            lvcArrowsID.Text = "ArrowId";
            lvcArrowsID.Width = 50;
            // 
            // lvcArrowsAddress
            // 
            lvcArrowsAddress.AspectName = "Address";
            lvcArrowsAddress.AspectToStringFormat = "{0:X4}";
            lvcArrowsAddress.IsEditable = false;
            lvcArrowsAddress.Text = "Address";
            lvcArrowsAddress.Width = 50;
            // 
            // lvcArrowsPlus0x00
            // 
            lvcArrowsPlus0x00.AspectName = "ArrowUnknown0";
            lvcArrowsPlus0x00.AspectToStringFormat = "{0:X}";
            lvcArrowsPlus0x00.Text = "+0x00";
            // 
            // lvcArrowsTextID
            // 
            lvcArrowsTextID.AspectName = "ArrowText";
            lvcArrowsTextID.AspectToStringFormat = "{0:X}";
            lvcArrowsTextID.Text = "TextID";
            lvcArrowsTextID.Width = 55;
            // 
            // lvcArrowsPlus0x04
            // 
            lvcArrowsPlus0x04.AspectName = "ArrowUnknown4";
            lvcArrowsPlus0x04.AspectToStringFormat = "{0:X}";
            lvcArrowsPlus0x04.Text = "+0x04";
            lvcArrowsPlus0x04.Width = 70;
            // 
            // lvcArrowsPointToWarpMPD
            // 
            lvcArrowsPointToWarpMPD.AspectName = "ArrowWarp";
            lvcArrowsPointToWarpMPD.AspectToStringFormat = "{0:X}";
            lvcArrowsPointToWarpMPD.Text = "PointToWarpMPD";
            lvcArrowsPointToWarpMPD.Width = 100;
            // 
            // lvcArrowsPlus0x08
            // 
            lvcArrowsPlus0x08.AspectName = "ArrowUnknown8";
            lvcArrowsPlus0x08.AspectToStringFormat = "{0:X}";
            lvcArrowsPlus0x08.Text = "+0x08";
            lvcArrowsPlus0x08.Width = 70;
            // 
            // lvcArrowsPlus0x0A
            // 
            lvcArrowsPlus0x0A.AspectName = "ArrowUnknownA";
            lvcArrowsPlus0x0A.AspectToStringFormat = "{0:X}";
            lvcArrowsPlus0x0A.Text = "+0x0A";
            // 
            // tabNonBattleEnter
            // 
            tabNonBattleEnter.Controls.Add(olvNonBattleEnter);
            tabNonBattleEnter.Location = new System.Drawing.Point(4, 24);
            tabNonBattleEnter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabNonBattleEnter.Name = "tabNonBattleEnter";
            tabNonBattleEnter.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabNonBattleEnter.Size = new System.Drawing.Size(1062, 559);
            tabNonBattleEnter.TabIndex = 16;
            tabNonBattleEnter.Text = "Non-battle Enter";
            tabNonBattleEnter.UseVisualStyleBackColor = true;
            // 
            // olvNonBattleEnter
            // 
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterName);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterID);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterAddress);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterSceneNumber);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterPlus0x02);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterXPos);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterPlus0x06);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterZPos);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterDirection);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterCamera);
            olvNonBattleEnter.AllColumns.Add(lvcNonBattleEnterPlus0x0E);
            olvNonBattleEnter.AllowColumnReorder = true;
            olvNonBattleEnter.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvNonBattleEnter.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvNonBattleEnter.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcNonBattleEnterName, lvcNonBattleEnterID, lvcNonBattleEnterAddress, lvcNonBattleEnterSceneNumber, lvcNonBattleEnterPlus0x02, lvcNonBattleEnterXPos, lvcNonBattleEnterPlus0x06, lvcNonBattleEnterZPos, lvcNonBattleEnterDirection, lvcNonBattleEnterCamera, lvcNonBattleEnterPlus0x0E });
            olvNonBattleEnter.FullRowSelect = true;
            olvNonBattleEnter.GridLines = true;
            olvNonBattleEnter.HasCollapsibleGroups = false;
            olvNonBattleEnter.Location = new System.Drawing.Point(4, 3);
            olvNonBattleEnter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvNonBattleEnter.MenuLabelGroupBy = "";
            olvNonBattleEnter.Name = "olvNonBattleEnter";
            olvNonBattleEnter.ShowGroups = false;
            olvNonBattleEnter.Size = new System.Drawing.Size(1053, 550);
            olvNonBattleEnter.TabIndex = 6;
            olvNonBattleEnter.UseAlternatingBackColors = true;
            olvNonBattleEnter.UseCompatibleStateImageBehavior = false;
            olvNonBattleEnter.View = System.Windows.Forms.View.Details;
            // 
            // lvcNonBattleEnterName
            // 
            lvcNonBattleEnterName.AspectName = "Name";
            lvcNonBattleEnterName.IsEditable = false;
            lvcNonBattleEnterName.Text = "EnterName";
            lvcNonBattleEnterName.Width = 70;
            // 
            // lvcNonBattleEnterID
            // 
            lvcNonBattleEnterID.AspectName = "ID";
            lvcNonBattleEnterID.AspectToStringFormat = "{0:X}";
            lvcNonBattleEnterID.IsEditable = false;
            lvcNonBattleEnterID.Text = "EnterId";
            lvcNonBattleEnterID.Width = 50;
            // 
            // lvcNonBattleEnterAddress
            // 
            lvcNonBattleEnterAddress.AspectName = "Address";
            lvcNonBattleEnterAddress.AspectToStringFormat = "{0:X4}";
            lvcNonBattleEnterAddress.IsEditable = false;
            lvcNonBattleEnterAddress.Text = "Address";
            lvcNonBattleEnterAddress.Width = 50;
            // 
            // lvcNonBattleEnterSceneNumber
            // 
            lvcNonBattleEnterSceneNumber.AspectName = "Entered";
            lvcNonBattleEnterSceneNumber.AspectToStringFormat = "{0:X}";
            lvcNonBattleEnterSceneNumber.Text = "Scene#";
            // 
            // lvcNonBattleEnterPlus0x02
            // 
            lvcNonBattleEnterPlus0x02.AspectName = "EnterUnknown2";
            lvcNonBattleEnterPlus0x02.AspectToStringFormat = "{0:X}";
            lvcNonBattleEnterPlus0x02.Text = "+0x02";
            lvcNonBattleEnterPlus0x02.Width = 55;
            // 
            // lvcNonBattleEnterXPos
            // 
            lvcNonBattleEnterXPos.AspectName = "EnterXPos";
            lvcNonBattleEnterXPos.AspectToStringFormat = "";
            lvcNonBattleEnterXPos.Text = "xPos";
            // 
            // lvcNonBattleEnterPlus0x06
            // 
            lvcNonBattleEnterPlus0x06.AspectName = "EnterUnknown6";
            lvcNonBattleEnterPlus0x06.AspectToStringFormat = "";
            lvcNonBattleEnterPlus0x06.Text = "+0x06";
            lvcNonBattleEnterPlus0x06.Width = 55;
            // 
            // lvcNonBattleEnterZPos
            // 
            lvcNonBattleEnterZPos.AspectName = "EnterZPos";
            lvcNonBattleEnterZPos.AspectToStringFormat = "";
            lvcNonBattleEnterZPos.Text = "zPos";
            // 
            // lvcNonBattleEnterDirection
            // 
            lvcNonBattleEnterDirection.AspectName = "EnterDirection";
            lvcNonBattleEnterDirection.AspectToStringFormat = "{0:X4}";
            lvcNonBattleEnterDirection.Text = "Direction";
            // 
            // lvcNonBattleEnterCamera
            // 
            lvcNonBattleEnterCamera.AspectName = "EnterCamera";
            lvcNonBattleEnterCamera.AspectToStringFormat = "{0:X4}";
            lvcNonBattleEnterCamera.Text = "Camera";
            lvcNonBattleEnterCamera.Width = 50;
            // 
            // lvcNonBattleEnterPlus0x0E
            // 
            lvcNonBattleEnterPlus0x0E.AspectName = "EnterUnknownE";
            lvcNonBattleEnterPlus0x0E.AspectToStringFormat = "{0:X}";
            lvcNonBattleEnterPlus0x0E.Text = "+0x0E";
            lvcNonBattleEnterPlus0x0E.Width = 55;
            // 
            // tabTownNpcs
            // 
            tabTownNpcs.Controls.Add(olvTownNpcs);
            tabTownNpcs.Location = new System.Drawing.Point(4, 24);
            tabTownNpcs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabTownNpcs.Name = "tabTownNpcs";
            tabTownNpcs.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabTownNpcs.Size = new System.Drawing.Size(1062, 559);
            tabTownNpcs.TabIndex = 15;
            tabTownNpcs.Text = "TownNpcs";
            tabTownNpcs.UseVisualStyleBackColor = true;
            // 
            // olvTownNpcs
            // 
            olvTownNpcs.AllColumns.Add(lvcTownNpcsName);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsID);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsAddress);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsSpriteID);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x02);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsMonsterTable);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsXPos);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x0A);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x0C);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x0E);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsZPos);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x12);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsDirection);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsPlus0x16);
            olvTownNpcs.AllColumns.Add(lvcTownNpcsTiedToEventNumber);
            olvTownNpcs.AllowColumnReorder = true;
            olvTownNpcs.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvTownNpcs.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvTownNpcs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcTownNpcsName, lvcTownNpcsID, lvcTownNpcsAddress, lvcTownNpcsSpriteID, lvcTownNpcsPlus0x02, lvcTownNpcsMonsterTable, lvcTownNpcsXPos, lvcTownNpcsPlus0x0A, lvcTownNpcsPlus0x0C, lvcTownNpcsPlus0x0E, lvcTownNpcsZPos, lvcTownNpcsPlus0x12, lvcTownNpcsDirection, lvcTownNpcsPlus0x16, lvcTownNpcsTiedToEventNumber });
            olvTownNpcs.FullRowSelect = true;
            olvTownNpcs.GridLines = true;
            olvTownNpcs.HasCollapsibleGroups = false;
            olvTownNpcs.Location = new System.Drawing.Point(4, 3);
            olvTownNpcs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvTownNpcs.MenuLabelGroupBy = "";
            olvTownNpcs.Name = "olvTownNpcs";
            olvTownNpcs.ShowGroups = false;
            olvTownNpcs.Size = new System.Drawing.Size(1053, 550);
            olvTownNpcs.TabIndex = 5;
            olvTownNpcs.UseAlternatingBackColors = true;
            olvTownNpcs.UseCompatibleStateImageBehavior = false;
            olvTownNpcs.View = System.Windows.Forms.View.Details;
            // 
            // lvcTownNpcsName
            // 
            lvcTownNpcsName.AspectName = "Name";
            lvcTownNpcsName.IsEditable = false;
            lvcTownNpcsName.Text = "NpcName";
            lvcTownNpcsName.Width = 70;
            // 
            // lvcTownNpcsID
            // 
            lvcTownNpcsID.AspectName = "ID";
            lvcTownNpcsID.AspectToStringFormat = "{0:X}";
            lvcTownNpcsID.IsEditable = false;
            lvcTownNpcsID.Text = "ID";
            lvcTownNpcsID.Width = 50;
            // 
            // lvcTownNpcsAddress
            // 
            lvcTownNpcsAddress.AspectName = "Address";
            lvcTownNpcsAddress.AspectToStringFormat = "{0:X4}";
            lvcTownNpcsAddress.IsEditable = false;
            lvcTownNpcsAddress.Text = "Address";
            lvcTownNpcsAddress.Width = 50;
            // 
            // lvcTownNpcsSpriteID
            // 
            lvcTownNpcsSpriteID.AspectName = "SpriteID";
            lvcTownNpcsSpriteID.AspectToStringFormat = "{0:X}";
            lvcTownNpcsSpriteID.Text = "SpriteID";
            // 
            // lvcTownNpcsPlus0x02
            // 
            lvcTownNpcsPlus0x02.AspectName = "NpcUnknown";
            lvcTownNpcsPlus0x02.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x02.Text = "+0x02";
            lvcTownNpcsPlus0x02.Width = 55;
            // 
            // lvcTownNpcsMonsterTable
            // 
            lvcTownNpcsMonsterTable.AspectName = "NpcTable";
            lvcTownNpcsMonsterTable.AspectToStringFormat = "{0:X}";
            lvcTownNpcsMonsterTable.Text = "MovementTable?";
            lvcTownNpcsMonsterTable.Width = 95;
            // 
            // lvcTownNpcsXPos
            // 
            lvcTownNpcsXPos.AspectName = "NpcXPos";
            lvcTownNpcsXPos.AspectToStringFormat = "";
            lvcTownNpcsXPos.Text = "xPos";
            // 
            // lvcTownNpcsPlus0x0A
            // 
            lvcTownNpcsPlus0x0A.AspectName = "NpcUnknownA";
            lvcTownNpcsPlus0x0A.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x0A.Text = "+0x0A";
            lvcTownNpcsPlus0x0A.Width = 50;
            // 
            // lvcTownNpcsPlus0x0C
            // 
            lvcTownNpcsPlus0x0C.AspectName = "NpcUnknownC";
            lvcTownNpcsPlus0x0C.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x0C.Text = "+0x0C";
            lvcTownNpcsPlus0x0C.Width = 50;
            // 
            // lvcTownNpcsPlus0x0E
            // 
            lvcTownNpcsPlus0x0E.AspectName = "NpcUnknownE";
            lvcTownNpcsPlus0x0E.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x0E.Text = "+0x0E";
            lvcTownNpcsPlus0x0E.Width = 50;
            // 
            // lvcTownNpcsZPos
            // 
            lvcTownNpcsZPos.AspectName = "NpcZPos";
            lvcTownNpcsZPos.AspectToStringFormat = "";
            lvcTownNpcsZPos.Text = "zPos";
            // 
            // lvcTownNpcsPlus0x12
            // 
            lvcTownNpcsPlus0x12.AspectName = "NpcUnknown12";
            lvcTownNpcsPlus0x12.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x12.Text = "+0x12";
            lvcTownNpcsPlus0x12.Width = 50;
            // 
            // lvcTownNpcsDirection
            // 
            lvcTownNpcsDirection.AspectName = "NpcDirection";
            lvcTownNpcsDirection.AspectToStringFormat = "{0:X}";
            lvcTownNpcsDirection.Text = "Direction";
            // 
            // lvcTownNpcsPlus0x16
            // 
            lvcTownNpcsPlus0x16.AspectName = "NpcUnknown16";
            lvcTownNpcsPlus0x16.AspectToStringFormat = "{0:X}";
            lvcTownNpcsPlus0x16.Text = "+0x16";
            lvcTownNpcsPlus0x16.Width = 50;
            // 
            // lvcTownNpcsTiedToEventNumber
            // 
            lvcTownNpcsTiedToEventNumber.AspectName = "NpcTieIn";
            lvcTownNpcsTiedToEventNumber.AspectToStringFormat = "{0:X}";
            lvcTownNpcsTiedToEventNumber.IsEditable = false;
            lvcTownNpcsTiedToEventNumber.Text = "TiedToEventNumber";
            lvcTownNpcsTiedToEventNumber.Width = 120;
            // 
            // tabInteractables
            // 
            tabInteractables.Controls.Add(olvInteractables);
            tabInteractables.Location = new System.Drawing.Point(4, 24);
            tabInteractables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabInteractables.Name = "tabInteractables";
            tabInteractables.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabInteractables.Size = new System.Drawing.Size(1062, 559);
            tabInteractables.TabIndex = 11;
            tabInteractables.Text = "Interactables";
            tabInteractables.UseVisualStyleBackColor = true;
            // 
            // olvInteractables
            // 
            olvInteractables.AllColumns.Add(lvcInteractablesName);
            olvInteractables.AllColumns.Add(lvcInteractablesID);
            olvInteractables.AllColumns.Add(lvcInteractablesAddress);
            olvInteractables.AllColumns.Add(lvcInteractablesDirectionSearched);
            olvInteractables.AllColumns.Add(lvcInteractablesEventNumber);
            olvInteractables.AllColumns.Add(lvcInteractablesFlagUsed);
            olvInteractables.AllColumns.Add(lvcInteractablesUnknown1);
            olvInteractables.AllColumns.Add(lvcInteractablesEventTypeCode);
            olvInteractables.AllColumns.Add(lvcInteractablesItemTextCode);
            olvInteractables.AllColumns.Add(lvcInteractablesMPDTieInID);
            olvInteractables.AllowColumnReorder = true;
            olvInteractables.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvInteractables.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvInteractables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcInteractablesName, lvcInteractablesID, lvcInteractablesAddress, lvcInteractablesDirectionSearched, lvcInteractablesEventNumber, lvcInteractablesFlagUsed, lvcInteractablesUnknown1, lvcInteractablesEventTypeCode, lvcInteractablesItemTextCode, lvcInteractablesMPDTieInID });
            olvInteractables.FullRowSelect = true;
            olvInteractables.GridLines = true;
            olvInteractables.HasCollapsibleGroups = false;
            olvInteractables.Location = new System.Drawing.Point(4, 3);
            olvInteractables.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvInteractables.MenuLabelGroupBy = "";
            olvInteractables.Name = "olvInteractables";
            olvInteractables.ShowGroups = false;
            olvInteractables.Size = new System.Drawing.Size(1053, 550);
            olvInteractables.TabIndex = 4;
            olvInteractables.UseAlternatingBackColors = true;
            olvInteractables.UseCompatibleStateImageBehavior = false;
            olvInteractables.View = System.Windows.Forms.View.Details;
            // 
            // lvcInteractablesName
            // 
            lvcInteractablesName.AspectName = "Name";
            lvcInteractablesName.IsEditable = false;
            lvcInteractablesName.Text = "InteractableName";
            lvcInteractablesName.Width = 100;
            // 
            // lvcInteractablesID
            // 
            lvcInteractablesID.AspectName = "ID";
            lvcInteractablesID.AspectToStringFormat = "{0:X}";
            lvcInteractablesID.IsEditable = false;
            lvcInteractablesID.Text = "Hex ID";
            lvcInteractablesID.Width = 50;
            // 
            // lvcInteractablesAddress
            // 
            lvcInteractablesAddress.AspectName = "Address";
            lvcInteractablesAddress.AspectToStringFormat = "{0:X4}";
            lvcInteractablesAddress.IsEditable = false;
            lvcInteractablesAddress.Text = "Address";
            lvcInteractablesAddress.Width = 50;
            // 
            // lvcInteractablesDirectionSearched
            // 
            lvcInteractablesDirectionSearched.AspectName = "Searched";
            lvcInteractablesDirectionSearched.AspectToStringFormat = "{0:X4}";
            lvcInteractablesDirectionSearched.Text = "Direction/Searched";
            lvcInteractablesDirectionSearched.Width = 110;
            // 
            // lvcInteractablesEventNumber
            // 
            lvcInteractablesEventNumber.AspectName = "EventNumber";
            lvcInteractablesEventNumber.AspectToStringFormat = "{0:X}";
            lvcInteractablesEventNumber.Text = "EventNumber";
            lvcInteractablesEventNumber.Width = 82;
            // 
            // lvcInteractablesFlagUsed
            // 
            lvcInteractablesFlagUsed.AspectName = "FlagUsed";
            lvcInteractablesFlagUsed.AspectToStringFormat = "{0:X4}";
            lvcInteractablesFlagUsed.Text = "FlagUsed";
            // 
            // lvcInteractablesUnknown1
            // 
            lvcInteractablesUnknown1.AspectName = "Unknown1";
            lvcInteractablesUnknown1.AspectToStringFormat = "{0:X}";
            lvcInteractablesUnknown1.Text = "Unknown1";
            lvcInteractablesUnknown1.Width = 70;
            // 
            // lvcInteractablesEventTypeCode
            // 
            lvcInteractablesEventTypeCode.AspectName = "EventType";
            lvcInteractablesEventTypeCode.AspectToStringFormat = "{0:X4}";
            lvcInteractablesEventTypeCode.Text = "EventType/Code";
            lvcInteractablesEventTypeCode.Width = 95;
            // 
            // lvcInteractablesItemTextCode
            // 
            lvcInteractablesItemTextCode.AspectName = "EventParameter";
            lvcInteractablesItemTextCode.AspectToStringFormat = "{0:X4}";
            lvcInteractablesItemTextCode.Text = "Item/Text/Code";
            lvcInteractablesItemTextCode.Width = 120;
            // 
            // lvcInteractablesMPDTieInID
            // 
            lvcInteractablesMPDTieInID.AspectName = "MPDTieInID";
            lvcInteractablesMPDTieInID.AspectToStringFormat = "{0:X}";
            lvcInteractablesMPDTieInID.IsEditable = false;
            lvcInteractablesMPDTieInID.Text = "MPDTieInID";
            lvcInteractablesMPDTieInID.Width = 80;
            // 
            // tabMain
            // 
            tabMain.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabMain.Controls.Add(tabInteractables);
            tabMain.Controls.Add(tabBattlePointers);
            tabMain.Controls.Add(tabTownNpcs);
            tabMain.Controls.Add(tabNonBattleEnter);
            tabMain.Controls.Add(tabWarpTable);
            tabMain.Controls.Add(tabArrows);
            tabMain.Controls.Add(tabTileData);
            tabMain.Controls.Add(tabBattle_Synbios);
            tabMain.Controls.Add(tabBattle_Medion);
            tabMain.Controls.Add(tabBattle_Julian);
            tabMain.Controls.Add(tabBattle_Extra);
            tabMain.Location = new System.Drawing.Point(0, 28);
            tabMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(1070, 587);
            tabMain.TabIndex = 0;
            // 
            // tabBattle_Synbios
            // 
            tabBattle_Synbios.Controls.Add(becBattle_Synbios);
            tabBattle_Synbios.Location = new System.Drawing.Point(4, 24);
            tabBattle_Synbios.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattle_Synbios.Name = "tabBattle_Synbios";
            tabBattle_Synbios.Size = new System.Drawing.Size(1062, 559);
            tabBattle_Synbios.TabIndex = 18;
            tabBattle_Synbios.Text = "Battle (Synbios)";
            tabBattle_Synbios.UseVisualStyleBackColor = true;
            // 
            // becBattle_Synbios
            // 
            becBattle_Synbios.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            becBattle_Synbios.BackColor = System.Drawing.Color.Transparent;
            becBattle_Synbios.Location = new System.Drawing.Point(-4, 1);
            becBattle_Synbios.Margin = new System.Windows.Forms.Padding(0);
            becBattle_Synbios.Name = "becBattle_Synbios";
            becBattle_Synbios.Size = new System.Drawing.Size(1070, 562);
            becBattle_Synbios.TabIndex = 0;
            // 
            // tabBattle_Medion
            // 
            tabBattle_Medion.Controls.Add(becBattle_Medion);
            tabBattle_Medion.Location = new System.Drawing.Point(4, 24);
            tabBattle_Medion.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattle_Medion.Name = "tabBattle_Medion";
            tabBattle_Medion.Size = new System.Drawing.Size(1062, 559);
            tabBattle_Medion.TabIndex = 19;
            tabBattle_Medion.Text = "Battle (Medion)";
            tabBattle_Medion.UseVisualStyleBackColor = true;
            // 
            // becBattle_Medion
            // 
            becBattle_Medion.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            becBattle_Medion.BackColor = System.Drawing.Color.Transparent;
            becBattle_Medion.Location = new System.Drawing.Point(-4, 1);
            becBattle_Medion.Margin = new System.Windows.Forms.Padding(0);
            becBattle_Medion.Name = "becBattle_Medion";
            becBattle_Medion.Size = new System.Drawing.Size(1070, 562);
            becBattle_Medion.TabIndex = 1;
            // 
            // tabBattle_Julian
            // 
            tabBattle_Julian.Controls.Add(becBattle_Julian);
            tabBattle_Julian.Location = new System.Drawing.Point(4, 24);
            tabBattle_Julian.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattle_Julian.Name = "tabBattle_Julian";
            tabBattle_Julian.Size = new System.Drawing.Size(1062, 559);
            tabBattle_Julian.TabIndex = 20;
            tabBattle_Julian.Text = "Battle (Julian)";
            tabBattle_Julian.UseVisualStyleBackColor = true;
            // 
            // becBattle_Julian
            // 
            becBattle_Julian.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            becBattle_Julian.BackColor = System.Drawing.Color.Transparent;
            becBattle_Julian.Location = new System.Drawing.Point(-4, 1);
            becBattle_Julian.Margin = new System.Windows.Forms.Padding(0);
            becBattle_Julian.Name = "becBattle_Julian";
            becBattle_Julian.Size = new System.Drawing.Size(1070, 562);
            becBattle_Julian.TabIndex = 1;
            // 
            // tabBattle_Extra
            // 
            tabBattle_Extra.Controls.Add(becBattle_Extra);
            tabBattle_Extra.Location = new System.Drawing.Point(4, 24);
            tabBattle_Extra.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBattle_Extra.Name = "tabBattle_Extra";
            tabBattle_Extra.Size = new System.Drawing.Size(1062, 559);
            tabBattle_Extra.TabIndex = 21;
            tabBattle_Extra.Text = "Battle (Extra)";
            tabBattle_Extra.UseVisualStyleBackColor = true;
            // 
            // becBattle_Extra
            // 
            becBattle_Extra.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            becBattle_Extra.BackColor = System.Drawing.Color.Transparent;
            becBattle_Extra.Location = new System.Drawing.Point(-4, 1);
            becBattle_Extra.Margin = new System.Windows.Forms.Padding(0);
            becBattle_Extra.Name = "becBattle_Extra";
            becBattle_Extra.Size = new System.Drawing.Size(1070, 562);
            becBattle_Extra.TabIndex = 1;
            // 
            // tsmiScenario
            // 
            tsmiScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripSeparator1, tsmiScenario_BTL99 });
            tsmiScenario.Name = "tsmiScenario";
            tsmiScenario.Size = new System.Drawing.Size(64, 20);
            tsmiScenario.Text = "&Scenario";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
            // 
            // tsmiScenario_BTL99
            // 
            tsmiScenario_BTL99.Name = "tsmiScenario_BTL99";
            tsmiScenario_BTL99.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B;
            tsmiScenario_BTL99.Size = new System.Drawing.Size(183, 22);
            tsmiScenario_BTL99.Text = "&BTL99 Toggle";
            tsmiScenario_BTL99.Click += tsmiScenario_BTL99_Click;
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiScenario });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(74, 24);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // frmX1_Editor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1070, 616);
            Controls.Add(menuStrip2);
            Controls.Add(tabMain);
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmX1_Editor";
            Text = "SF3 X1 Editor";
            Controls.SetChildIndex(tabMain, 0);
            Controls.SetChildIndex(menuStrip2, 0);
            tabTileData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvTileData).EndInit();
            tabWarpTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvWarpTable).EndInit();
            tabBattlePointers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvBattlePointers).EndInit();
            tabArrows.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvArrows).EndInit();
            tabNonBattleEnter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvNonBattleEnter).EndInit();
            tabTownNpcs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvTownNpcs).EndInit();
            tabInteractables.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvInteractables).EndInit();
            tabMain.ResumeLayout(false);
            tabBattle_Synbios.ResumeLayout(false);
            tabBattle_Medion.ResumeLayout(false);
            tabBattle_Julian.ResumeLayout(false);
            tabBattle_Extra.ResumeLayout(false);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private BrightIdeasSoftware.OLVColumn lvcTileDataCantStay;
        private BrightIdeasSoftware.OLVColumn lvcTileDataSand;
        private BrightIdeasSoftware.OLVColumn lvcTileDataEnemyOnly;
        private BrightIdeasSoftware.OLVColumn lvcTileDataPlayerOnly;
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


