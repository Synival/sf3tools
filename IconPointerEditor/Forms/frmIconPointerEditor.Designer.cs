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
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp_OpenInfo = new System.Windows.Forms.ToolStripMenuItem();
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
            this.lvcSpellIconSpellName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconHexID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconOffset = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcSpellIconViewOffset = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.menuStrip2.SuspendLayout();
            this.tabItemIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvItemIcons)).BeginInit();
            this.tabMain.SuspendLayout();
            this.tabSpellIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvSpellIcons)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip2
            // 
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelp});
            this.menuStrip2.Location = new System.Drawing.Point(0, 24);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Size = new System.Drawing.Size(764, 24);
            this.menuStrip2.TabIndex = 1;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiHelp_OpenInfo});
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(44, 20);
            this.tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_OpenInfo
            // 
            this.tsmiHelp_OpenInfo.Name = "tsmiHelp_OpenInfo";
            this.tsmiHelp_OpenInfo.Size = new System.Drawing.Size(224, 22);
            this.tsmiHelp_OpenInfo.Text = "Opens X011, X021, X026 Files";
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
            this.lvcItemIconName.AspectName = "Name";
            this.lvcItemIconName.IsEditable = false;
            this.lvcItemIconName.Text = "Name";
            this.lvcItemIconName.Width = 120;
            // 
            // lvcItemIconHexID
            // 
            this.lvcItemIconHexID.AspectName = "ID";
            this.lvcItemIconHexID.AspectToStringFormat = "{0:X}";
            this.lvcItemIconHexID.IsEditable = false;
            this.lvcItemIconHexID.Text = "Hex ID";
            // 
            // lvcItemIconAddress
            // 
            this.lvcItemIconAddress.AspectName = "Address";
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
            this.olvSpellIcons.AllColumns.Add(this.lvcSpellIconSpellName);
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
            this.lvcSpellIconSpellName,
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
            this.lvcSpellIconName.Text = "Icon Name";
            this.lvcSpellIconName.Width = 120;
            // 
            // lvcSpellIconSpellName
            // 
            this.lvcSpellIconSpellName.AspectName = "SpellID";
            this.lvcSpellIconSpellName.IsEditable = false;
            this.lvcSpellIconSpellName.Text = "Spell Name";
            this.lvcSpellIconSpellName.Width = 120;
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
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.tabMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmIconPointerEditor";
            this.Text = "SF3 Icon Pointer Editor";
            this.Controls.SetChildIndex(this.tabMain, 0);
            this.Controls.SetChildIndex(this.menuStrip2, 0);
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.tabItemIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvItemIcons)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.tabSpellIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvSpellIcons)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
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
        private BrightIdeasSoftware.OLVColumn lvcSpellIconSpellName;
    }
}
