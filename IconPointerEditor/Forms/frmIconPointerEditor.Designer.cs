namespace SF3.IconPointerEditor.Forms
{
    partial class frmIconPointerEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIconPointerEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_File2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp_Version = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp_OpenInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparator_Help1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiHelp_X026Toggle = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            this.tabItemIcons = new System.Windows.Forms.TabPage();
            this.olvItemIcons = new BrightIdeasSoftware.ObjectListView();
            this.lvcItemIconName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcItemIconHexID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcItemIconAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcItemIconOffset = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabSpellIcons = new System.Windows.Forms.TabPage();
            this.olvSpellIcons = new BrightIdeasSoftware.ObjectListView();
            this.lvcSpellIconName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconHexID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconOffset = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconViewOffset = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tsSeparator_File3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiFile_CopyTablesFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.tabItemIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvItemIcons)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabSpellIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvSpellIcons)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile,
            this.tsmiHelp,
            this.tsmiScenario});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(764, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            this.tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFile_Open,
            this.tsmiFile_SaveAs,
            this.tsSeparator_File1,
            this.tsmiFile_CopyTablesFrom,
            this.tsSeparator_File2,
            this.tsmiFile_Close,
            this.tsSeparator_File3,
            this.tsmiFile_Exit});
            this.tsmiFile.Name = "tsmiFile";
            this.tsmiFile.Size = new System.Drawing.Size(37, 20);
            this.tsmiFile.Text = "File";
            // 
            // tsmiFile_Open
            // 
            this.tsmiFile_Open.Name = "tsmiFile_Open";
            this.tsmiFile_Open.Size = new System.Drawing.Size(180, 22);
            this.tsmiFile_Open.Text = "Open...";
            this.tsmiFile_Open.Click += new System.EventHandler(this.tsmiFile_Open_Click);
            // 
            // tsmiFile_SaveAs
            // 
            this.tsmiFile_SaveAs.Enabled = false;
            this.tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            this.tsmiFile_SaveAs.Size = new System.Drawing.Size(180, 22);
            this.tsmiFile_SaveAs.Text = "Save As...";
            this.tsmiFile_SaveAs.Click += new System.EventHandler(this.tsmiFile_SaveAs_Click);
            // 
            // tsSeparator_File1
            // 
            this.tsSeparator_File1.Name = "tsSeparator_File1";
            this.tsSeparator_File1.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiFile_Close
            // 
            this.tsmiFile_Close.Enabled = false;
            this.tsmiFile_Close.Name = "tsmiFile_Close";
            this.tsmiFile_Close.Size = new System.Drawing.Size(180, 22);
            this.tsmiFile_Close.Text = "Close";
            this.tsmiFile_Close.Click += new System.EventHandler(this.tsmiFile_Close_Click);
            // 
            // tsSeparator_File2
            // 
            this.tsSeparator_File2.Name = "tsSeparator_File2";
            this.tsSeparator_File2.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiFile_Exit
            // 
            this.tsmiFile_Exit.Name = "tsmiFile_Exit";
            this.tsmiFile_Exit.Size = new System.Drawing.Size(180, 22);
            this.tsmiFile_Exit.Text = "Exit";
            this.tsmiFile_Exit.Click += new System.EventHandler(this.tsmiFile_Exit_Click);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelp_Version,
            this.tsmiHelp_OpenInfo,
            this.tsSeparator_Help1,
            this.tsmiHelp_X026Toggle});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsmiHelp.Text = "Help";
            // 
            // tsmiHelp_Version
            // 
            this.tsmiHelp_Version.Name = "tsmiHelp_Version";
            this.tsmiHelp_Version.Size = new System.Drawing.Size(222, 22);
            this.tsmiHelp_Version.Text = "Version (set internally)";
            // 
            // tsmiHelp_OpenInfo
            // 
            this.tsmiHelp_OpenInfo.Name = "tsmiHelp_OpenInfo";
            this.tsmiHelp_OpenInfo.Size = new System.Drawing.Size(222, 22);
            this.tsmiHelp_OpenInfo.Text = "Opens X011, X021, X026 files";
            // 
            // tsSeparator_Help1
            // 
            this.tsSeparator_Help1.Name = "tsSeparator_Help1";
            this.tsSeparator_Help1.Size = new System.Drawing.Size(219, 6);
            // 
            // tsmiHelp_X026Toggle
            // 
            this.tsmiHelp_X026Toggle.Name = "tsmiHelp_X026Toggle";
            this.tsmiHelp_X026Toggle.Size = new System.Drawing.Size(222, 22);
            this.tsmiHelp_X026Toggle.Text = "X026 toggle";
            this.tsmiHelp_X026Toggle.Click += new System.EventHandler(this.tsmiHelp_X026Toggle_Click);
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
            this.tsmiScenario.Text = "Scenario";
            // 
            // tsmiScenario_Scenario1
            // 
            this.tsmiScenario_Scenario1.Name = "tsmiScenario_Scenario1";
            this.tsmiScenario_Scenario1.Size = new System.Drawing.Size(148, 22);
            this.tsmiScenario_Scenario1.Text = "Scenario 1";
            this.tsmiScenario_Scenario1.Click += new System.EventHandler(this.tsmiScenario_Scenario1_Click);
            // 
            // tsmiScenario_Scenario2
            // 
            this.tsmiScenario_Scenario2.Name = "tsmiScenario_Scenario2";
            this.tsmiScenario_Scenario2.Size = new System.Drawing.Size(148, 22);
            this.tsmiScenario_Scenario2.Text = "Scenario 2";
            this.tsmiScenario_Scenario2.Click += new System.EventHandler(this.tsmiScenario_Scenario2_Click);
            // 
            // tsmiScenario_Scenario3
            // 
            this.tsmiScenario_Scenario3.Name = "tsmiScenario_Scenario3";
            this.tsmiScenario_Scenario3.Size = new System.Drawing.Size(148, 22);
            this.tsmiScenario_Scenario3.Text = "Scenario 3";
            this.tsmiScenario_Scenario3.Click += new System.EventHandler(this.tsmiScenario_Scenario3_Click);
            // 
            // tsmiScenario_PremiumDisk
            // 
            this.tsmiScenario_PremiumDisk.Name = "tsmiScenario_PremiumDisk";
            this.tsmiScenario_PremiumDisk.Size = new System.Drawing.Size(148, 22);
            this.tsmiScenario_PremiumDisk.Text = "Premium Disk";
            this.tsmiScenario_PremiumDisk.Click += new System.EventHandler(this.tsmiScenario_PremiumDisk_Click);
            // 
            // tabItemIcons
            // 
            this.tabItemIcons.Controls.Add(this.olvItemIcons);
            this.tabItemIcons.Location = new System.Drawing.Point(4, 22);
            this.tabItemIcons.Name = "tabItemIcons";
            this.tabItemIcons.Size = new System.Drawing.Size(756, 480);
            this.tabItemIcons.TabIndex = 3;
            this.tabItemIcons.Text = "Item Icons";
            this.tabItemIcons.UseVisualStyleBackColor = true;
            // 
            // olvItemIcons
            // 
            this.olvItemIcons.AllColumns.Add(this.lvcItemIconName);
            this.olvItemIcons.AllColumns.Add(this.lvcItemIconHexID);
            this.olvItemIcons.AllColumns.Add(this.lvcItemIconAddress);
            this.olvItemIcons.AllColumns.Add(this.lvcItemIconOffset);
            this.olvItemIcons.AllowColumnReorder = true;
            this.olvItemIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvItemIcons.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvItemIcons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcItemIconName,
            this.lvcItemIconHexID,
            this.lvcItemIconAddress,
            this.lvcItemIconOffset});
            this.olvItemIcons.FullRowSelect = true;
            this.olvItemIcons.GridLines = true;
            this.olvItemIcons.HasCollapsibleGroups = false;
            this.olvItemIcons.HideSelection = false;
            this.olvItemIcons.Location = new System.Drawing.Point(3, 3);
            this.olvItemIcons.MenuLabelGroupBy = "";
            this.olvItemIcons.Name = "olvItemIcons";
            this.olvItemIcons.ShowGroups = false;
            this.olvItemIcons.Size = new System.Drawing.Size(750, 474);
            this.olvItemIcons.TabIndex = 0;
            this.olvItemIcons.UseAlternatingBackColors = true;
            this.olvItemIcons.UseCompatibleStateImageBehavior = false;
            this.olvItemIcons.View = System.Windows.Forms.View.Details;
            this.olvItemIcons.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcItemIconName
            // 
            this.lvcItemIconName.AspectName = "SizeName";
            this.lvcItemIconName.IsEditable = false;
            this.lvcItemIconName.Text = "Name";
            this.lvcItemIconName.Width = 120;
            // 
            // lvcItemIconHexID
            // 
            this.lvcItemIconHexID.AspectName = "SizeID";
            this.lvcItemIconHexID.AspectToStringFormat = "{0:X}";
            this.lvcItemIconHexID.IsEditable = false;
            this.lvcItemIconHexID.Text = "Hex ID";
            // 
            // lvcItemIconAddress
            // 
            this.lvcItemIconAddress.AspectName = "SizeAddress";
            this.lvcItemIconAddress.AspectToStringFormat = "{0:X}";
            this.lvcItemIconAddress.IsEditable = false;
            this.lvcItemIconAddress.Text = "Address";
            // 
            // lvcItemIconOffset
            // 
            this.lvcItemIconOffset.AspectName = "TheItemIcon";
            this.lvcItemIconOffset.AspectToStringFormat = "{0:X}";
            this.lvcItemIconOffset.Text = "ItemIcon offset";
            this.lvcItemIconOffset.Width = 90;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabItemIcons);
            this.tabMain.Controls.Add(this.tabSpellIcons);
            this.tabMain.Location = new System.Drawing.Point(0, 27);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(764, 506);
            this.tabMain.TabIndex = 0;
            // 
            // tabSpellIcons
            // 
            this.tabSpellIcons.Controls.Add(this.olvSpellIcons);
            this.tabSpellIcons.Location = new System.Drawing.Point(4, 22);
            this.tabSpellIcons.Name = "tabSpellIcons";
            this.tabSpellIcons.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpellIcons.Size = new System.Drawing.Size(756, 480);
            this.tabSpellIcons.TabIndex = 4;
            this.tabSpellIcons.Text = "Spell Icons";
            this.tabSpellIcons.UseVisualStyleBackColor = true;
            // 
            // olvSpellIcons
            // 
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconName);
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconHexID);
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconAddress);
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconOffset);
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconViewOffset);
            this.olvSpellIcons.AllowColumnReorder = true;
            this.olvSpellIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvSpellIcons.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvSpellIcons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcSpellIconName,
            this.lvcSpellIconHexID,
            this.lvcSpellIconAddress,
            this.lvcSpellIconOffset,
            this.lvcSpellIconViewOffset});
            this.olvSpellIcons.FullRowSelect = true;
            this.olvSpellIcons.GridLines = true;
            this.olvSpellIcons.HasCollapsibleGroups = false;
            this.olvSpellIcons.HideSelection = false;
            this.olvSpellIcons.Location = new System.Drawing.Point(3, 3);
            this.olvSpellIcons.MenuLabelGroupBy = "";
            this.olvSpellIcons.Name = "olvSpellIcons";
            this.olvSpellIcons.ShowGroups = false;
            this.olvSpellIcons.Size = new System.Drawing.Size(750, 474);
            this.olvSpellIcons.TabIndex = 1;
            this.olvSpellIcons.UseAlternatingBackColors = true;
            this.olvSpellIcons.UseCompatibleStateImageBehavior = false;
            this.olvSpellIcons.View = System.Windows.Forms.View.Details;
            this.olvSpellIcons.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcSpellIconName
            // 
            this.lvcSpellIconName.AspectName = "Name";
            this.lvcSpellIconName.IsEditable = false;
            this.lvcSpellIconName.Text = "Name";
            this.lvcSpellIconName.Width = 120;
            // 
            // lvcSpellIconHexID
            // 
            this.lvcSpellIconHexID.AspectName = "ID";
            this.lvcSpellIconHexID.AspectToStringFormat = "{0:X}";
            this.lvcSpellIconHexID.IsEditable = false;
            this.lvcSpellIconHexID.Text = "Hex ID";
            this.lvcSpellIconHexID.Width = 50;
            // 
            // lvcSpellIconAddress
            // 
            this.lvcSpellIconAddress.AspectName = "Address";
            this.lvcSpellIconAddress.AspectToStringFormat = "{0:X}";
            this.lvcSpellIconAddress.IsEditable = false;
            this.lvcSpellIconAddress.Text = "Address";
            this.lvcSpellIconAddress.Width = 50;
            // 
            // lvcSpellIconOffset
            // 
            this.lvcSpellIconOffset.AspectName = "TheSpellIcon";
            this.lvcSpellIconOffset.AspectToStringFormat = "{0:X}";
            this.lvcSpellIconOffset.Text = "Spell Icon offset";
            this.lvcSpellIconOffset.Width = 90;
            // 
            // lvcSpellIconViewOffset
            // 
            this.lvcSpellIconViewOffset.AspectName = "RealOffset";
            this.lvcSpellIconViewOffset.AspectToStringFormat = "{0:X}";
            this.lvcSpellIconViewOffset.Text = "Offset in file for viewing";
            this.lvcSpellIconViewOffset.Width = 130;
            // 
            // tsSeparator_File3
            // 
            this.tsSeparator_File3.Name = "tsSeparator_File3";
            this.tsSeparator_File3.Size = new System.Drawing.Size(177, 6);
            // 
            // tsmiFile_CopyTablesFrom
            // 
            this.tsmiFile_CopyTablesFrom.Enabled = false;
            this.tsmiFile_CopyTablesFrom.Name = "tsmiFile_CopyTablesFrom";
            this.tsmiFile_CopyTablesFrom.Size = new System.Drawing.Size(180, 22);
            this.tsmiFile_CopyTablesFrom.Text = "Copy Tables From...";
            this.tsmiFile_CopyTablesFrom.Click += new System.EventHandler(this.tsmiFile_CopyTablesFrom_Click);
            // 
            // frmIconPointerEditor
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 534);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmIconPointerEditor";
            this.Text = "SF3 Icon Pointer Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabItemIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvItemIcons)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabSpellIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvSpellIcons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Open;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_Version;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario2;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_PremiumDisk;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario3;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario1;
        private System.Windows.Forms.TabPage tabItemIcons;
        private BrightIdeasSoftware.ObjectListView olvItemIcons;
        private BrightIdeasSoftware.OLVColumn lvcItemIconName;
        private BrightIdeasSoftware.OLVColumn lvcItemIconAddress;
        private BrightIdeasSoftware.OLVColumn lvcItemIconOffset;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabSpellIcons;
        private BrightIdeasSoftware.ObjectListView olvSpellIcons;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconName;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconHexID;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconAddress;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconOffset;
        private BrightIdeasSoftware.OLVColumn lvcItemIconHexID;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_OpenInfo;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconViewOffset;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_X026Toggle;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Close;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File2;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Exit;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_Help1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_CopyTablesFrom;
        private System.Windows.Forms.ToolStripSeparator tsSeparator_File3;
    }
}
