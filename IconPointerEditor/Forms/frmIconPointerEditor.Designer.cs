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
        private void InitializeComponent() {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIconPointerEditor));
            menuStrip2 = new System.Windows.Forms.MenuStrip();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_OpenInfo = new System.Windows.Forms.ToolStripMenuItem();
            tabItemIcons = new System.Windows.Forms.TabPage();
            olvItemIcons = new BrightIdeasSoftware.ObjectListView();
            lvcItemIconName = new BrightIdeasSoftware.OLVColumn();
            lvcItemIconHexID = new BrightIdeasSoftware.OLVColumn();
            lvcItemIconAddress = new BrightIdeasSoftware.OLVColumn();
            lvcItemIconOffset = new BrightIdeasSoftware.OLVColumn();
            tabMain = new System.Windows.Forms.TabControl();
            tabSpellIcons = new System.Windows.Forms.TabPage();
            olvSpellIcons = new BrightIdeasSoftware.ObjectListView();
            lvcSpellIconName = new BrightIdeasSoftware.OLVColumn();
            lvcSpellIconSpellName = new BrightIdeasSoftware.OLVColumn();
            lvcSpellIconHexID = new BrightIdeasSoftware.OLVColumn();
            lvcSpellIconAddress = new BrightIdeasSoftware.OLVColumn();
            lvcSpellIconOffset = new BrightIdeasSoftware.OLVColumn();
            lvcSpellIconViewOffset = new BrightIdeasSoftware.OLVColumn();
            menuStrip2.SuspendLayout();
            tabItemIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvItemIcons).BeginInit();
            tabMain.SuspendLayout();
            tabSpellIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvSpellIcons).BeginInit();
            SuspendLayout();
            // 
            // menuStrip2
            // 
            menuStrip2.Dock = System.Windows.Forms.DockStyle.None;
            menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp });
            menuStrip2.Location = new System.Drawing.Point(0, 0);
            menuStrip2.Name = "menuStrip2";
            menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            menuStrip2.Size = new System.Drawing.Size(53, 24);
            menuStrip2.TabIndex = 1;
            menuStrip2.Text = "menuStrip2";
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_OpenInfo });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new System.Drawing.Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_OpenInfo
            // 
            tsmiHelp_OpenInfo.Name = "tsmiHelp_OpenInfo";
            tsmiHelp_OpenInfo.Size = new System.Drawing.Size(224, 22);
            tsmiHelp_OpenInfo.Text = "Opens X011, X021, X026 Files";
            // 
            // tabItemIcons
            // 
            tabItemIcons.Controls.Add(olvItemIcons);
            tabItemIcons.Location = new System.Drawing.Point(4, 24);
            tabItemIcons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabItemIcons.Name = "tabItemIcons";
            tabItemIcons.Size = new System.Drawing.Size(883, 556);
            tabItemIcons.TabIndex = 3;
            tabItemIcons.Text = "Item Icons";
            tabItemIcons.UseVisualStyleBackColor = true;
            // 
            // olvItemIcons
            // 
            olvItemIcons.AllColumns.Add(lvcItemIconName);
            olvItemIcons.AllColumns.Add(lvcItemIconHexID);
            olvItemIcons.AllColumns.Add(lvcItemIconAddress);
            olvItemIcons.AllColumns.Add(lvcItemIconOffset);
            olvItemIcons.AllowColumnReorder = true;
            olvItemIcons.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvItemIcons.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvItemIcons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcItemIconName, lvcItemIconHexID, lvcItemIconAddress, lvcItemIconOffset });
            olvItemIcons.FullRowSelect = true;
            olvItemIcons.GridLines = true;
            olvItemIcons.HasCollapsibleGroups = false;
            olvItemIcons.Location = new System.Drawing.Point(4, 3);
            olvItemIcons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvItemIcons.MenuLabelGroupBy = "";
            olvItemIcons.Name = "olvItemIcons";
            olvItemIcons.ShowGroups = false;
            olvItemIcons.Size = new System.Drawing.Size(874, 546);
            olvItemIcons.TabIndex = 0;
            olvItemIcons.UseAlternatingBackColors = true;
            olvItemIcons.UseCompatibleStateImageBehavior = false;
            olvItemIcons.View = System.Windows.Forms.View.Details;
            // 
            // lvcItemIconName
            // 
            lvcItemIconName.AspectName = "Name";
            lvcItemIconName.IsEditable = false;
            lvcItemIconName.Text = "Name";
            lvcItemIconName.Width = 120;
            // 
            // lvcItemIconHexID
            // 
            lvcItemIconHexID.AspectName = "ID";
            lvcItemIconHexID.AspectToStringFormat = "{0:X}";
            lvcItemIconHexID.IsEditable = false;
            lvcItemIconHexID.Text = "Hex ID";
            // 
            // lvcItemIconAddress
            // 
            lvcItemIconAddress.AspectName = "Address";
            lvcItemIconAddress.AspectToStringFormat = "{0:X4}";
            lvcItemIconAddress.IsEditable = false;
            lvcItemIconAddress.Text = "Address";
            // 
            // lvcItemIconOffset
            // 
            lvcItemIconOffset.AspectName = "TheItemIcon";
            lvcItemIconOffset.AspectToStringFormat = "{0:X4}";
            lvcItemIconOffset.Text = "ItemIcon offset";
            lvcItemIconOffset.Width = 90;
            // 
            // tabMain
            // 
            tabMain.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabMain.Controls.Add(tabItemIcons);
            tabMain.Controls.Add(tabSpellIcons);
            tabMain.Location = new System.Drawing.Point(0, 31);
            tabMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(891, 584);
            tabMain.TabIndex = 0;
            // 
            // tabSpellIcons
            // 
            tabSpellIcons.Controls.Add(olvSpellIcons);
            tabSpellIcons.Location = new System.Drawing.Point(4, 24);
            tabSpellIcons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabSpellIcons.Name = "tabSpellIcons";
            tabSpellIcons.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabSpellIcons.Size = new System.Drawing.Size(883, 556);
            tabSpellIcons.TabIndex = 4;
            tabSpellIcons.Text = "Spell Icons";
            tabSpellIcons.UseVisualStyleBackColor = true;
            // 
            // olvSpellIcons
            // 
            olvSpellIcons.AllColumns.Add(lvcSpellIconName);
            olvSpellIcons.AllColumns.Add(lvcSpellIconSpellName);
            olvSpellIcons.AllColumns.Add(lvcSpellIconHexID);
            olvSpellIcons.AllColumns.Add(lvcSpellIconAddress);
            olvSpellIcons.AllColumns.Add(lvcSpellIconOffset);
            olvSpellIcons.AllColumns.Add(lvcSpellIconViewOffset);
            olvSpellIcons.AllowColumnReorder = true;
            olvSpellIcons.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvSpellIcons.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvSpellIcons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcSpellIconName, lvcSpellIconSpellName, lvcSpellIconHexID, lvcSpellIconAddress, lvcSpellIconOffset, lvcSpellIconViewOffset });
            olvSpellIcons.FullRowSelect = true;
            olvSpellIcons.GridLines = true;
            olvSpellIcons.HasCollapsibleGroups = false;
            olvSpellIcons.Location = new System.Drawing.Point(4, 3);
            olvSpellIcons.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvSpellIcons.MenuLabelGroupBy = "";
            olvSpellIcons.Name = "olvSpellIcons";
            olvSpellIcons.ShowGroups = false;
            olvSpellIcons.Size = new System.Drawing.Size(874, 546);
            olvSpellIcons.TabIndex = 1;
            olvSpellIcons.UseAlternatingBackColors = true;
            olvSpellIcons.UseCompatibleStateImageBehavior = false;
            olvSpellIcons.View = System.Windows.Forms.View.Details;
            // 
            // lvcSpellIconName
            // 
            lvcSpellIconName.AspectName = "Name";
            lvcSpellIconName.IsEditable = false;
            lvcSpellIconName.Text = "Icon Name";
            lvcSpellIconName.Width = 120;
            // 
            // lvcSpellIconSpellName
            // 
            lvcSpellIconSpellName.AspectName = "SpellID";
            lvcSpellIconSpellName.IsEditable = false;
            lvcSpellIconSpellName.Text = "Spell Name";
            lvcSpellIconSpellName.Width = 120;
            // 
            // lvcSpellIconHexID
            // 
            lvcSpellIconHexID.AspectName = "ID";
            lvcSpellIconHexID.AspectToStringFormat = "{0:X}";
            lvcSpellIconHexID.IsEditable = false;
            lvcSpellIconHexID.Text = "Hex ID";
            lvcSpellIconHexID.Width = 50;
            // 
            // lvcSpellIconAddress
            // 
            lvcSpellIconAddress.AspectName = "Address";
            lvcSpellIconAddress.AspectToStringFormat = "{0:X4}";
            lvcSpellIconAddress.IsEditable = false;
            lvcSpellIconAddress.Text = "Address";
            lvcSpellIconAddress.Width = 50;
            // 
            // lvcSpellIconOffset
            // 
            lvcSpellIconOffset.AspectName = "TheSpellIcon";
            lvcSpellIconOffset.AspectToStringFormat = "{0:X4}";
            lvcSpellIconOffset.Text = "Spell Icon offset";
            lvcSpellIconOffset.Width = 90;
            // 
            // lvcSpellIconViewOffset
            // 
            lvcSpellIconViewOffset.AspectName = "RealOffset";
            lvcSpellIconViewOffset.AspectToStringFormat = "{0:X4}";
            lvcSpellIconViewOffset.Text = "Offset in file for viewing";
            lvcSpellIconViewOffset.Width = 130;
            // 
            // frmIconPointerEditor
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(891, 616);
            Controls.Add(menuStrip2);
            Controls.Add(tabMain);
            Icon = (System.Drawing.Icon) resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "frmIconPointerEditor";
            Text = "SF3 Icon Pointer Editor";
            Controls.SetChildIndex(tabMain, 0);
            Controls.SetChildIndex(menuStrip2, 0);
            menuStrip2.ResumeLayout(false);
            menuStrip2.PerformLayout();
            tabItemIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvItemIcons).EndInit();
            tabMain.ResumeLayout(false);
            tabSpellIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvSpellIcons).EndInit();
            ResumeLayout(false);
            PerformLayout();
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
