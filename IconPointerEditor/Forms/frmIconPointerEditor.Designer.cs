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
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
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
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.helpToolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(764, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Enabled = false;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.aboutToolStripMenuItem.Text = "Version 0.08";
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItem5.Text = "Opens X011,X021, X026 files";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(219, 22);
            this.toolStripMenuItem6.Text = " x026 toggle";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click_1);
            // 
            // helpToolStripMenuItem1
            // 
            this.helpToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripSeparator1});
            this.helpToolStripMenuItem1.Name = "helpToolStripMenuItem1";
            this.helpToolStripMenuItem1.Size = new System.Drawing.Size(64, 20);
            this.helpToolStripMenuItem1.Text = "Scenario";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem1.Text = "Scenario 1";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem2.Text = "Scenario 2";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem3.Text = "Scenario 3";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(148, 22);
            this.toolStripMenuItem4.Text = "Premium Disk";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(145, 6);
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
            this.olvItemIcons.Location = new System.Drawing.Point(0, 0);
            this.olvItemIcons.MenuLabelGroupBy = "";
            this.olvItemIcons.Name = "olvItemIcons";
            this.olvItemIcons.ShowGroups = false;
            this.olvItemIcons.Size = new System.Drawing.Size(750, 474);
            this.olvItemIcons.TabIndex = 0;
            this.olvItemIcons.UseAlternatingBackColors = true;
            this.olvItemIcons.UseCompatibleStateImageBehavior = false;
            this.olvItemIcons.View = System.Windows.Forms.View.Details;
            this.olvItemIcons.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditFinishing);
            this.olvItemIcons.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcItemIconName
            // 
            this.lvcItemIconName.AspectName = "SizeName";
            this.lvcItemIconName.IsEditable = false;
            this.lvcItemIconName.Text = "Name";
            this.lvcItemIconName.Width = 80;
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
            this.tabMain.Controls.Add(this.tabItemIcons);
            this.tabMain.Controls.Add(this.tabSpellIcons);
            this.tabMain.Location = new System.Drawing.Point(0, 27);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(764, 506);
            this.tabMain.TabIndex = 0;
            this.tabMain.SelectedIndexChanged += new System.EventHandler(this.tabMain_SelectedIndexChanged);
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
            this.olvSpellIcons.Location = new System.Drawing.Point(0, 0);
            this.olvSpellIcons.MenuLabelGroupBy = "";
            this.olvSpellIcons.Name = "olvSpellIcons";
            this.olvSpellIcons.ShowGroups = false;
            this.olvSpellIcons.Size = new System.Drawing.Size(750, 474);
            this.olvSpellIcons.TabIndex = 1;
            this.olvSpellIcons.UseAlternatingBackColors = true;
            this.olvSpellIcons.UseCompatibleStateImageBehavior = false;
            this.olvSpellIcons.View = System.Windows.Forms.View.Details;
            this.olvSpellIcons.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditFinishing);
            this.olvSpellIcons.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcSpellIconName
            // 
            this.lvcSpellIconName.AspectName = "Name";
            this.lvcSpellIconName.IsEditable = false;
            this.lvcSpellIconName.Text = "Name";
            this.lvcSpellIconName.Width = 90;
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
            this.Text = "Sf3 Icon pointer Editor          X026 mode: Off";
            this.Resize += new System.EventHandler(this.frmIconPointerEditor_Resize);
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
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BrightIdeasSoftware.OLVColumn lvcItemIconHexID;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private BrightIdeasSoftware.OLVColumn lvcSpellIconViewOffset;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
    }
}
