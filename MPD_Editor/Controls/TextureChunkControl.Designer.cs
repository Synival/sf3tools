namespace SF3.X1_Editor.Controls {
    partial class TextureChunkControl {
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
            this.tabHeader = new System.Windows.Forms.TabPage();
            this.olvHeader = new BrightIdeasSoftware.ObjectListView();
            this.lvcHeaderName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcHeaderAddress = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcHeaderNumTextures = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.lvcHeaderTextureIdStart = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabTextures = new System.Windows.Forms.TabPage();
            this.tabHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvHeader)).BeginInit();
            this.tabMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabHeader
            // 
            this.tabHeader.Controls.Add(this.olvHeader);
            this.tabHeader.Location = new System.Drawing.Point(4, 22);
            this.tabHeader.Name = "tabHeader";
            this.tabHeader.Size = new System.Drawing.Size(909, 508);
            this.tabHeader.TabIndex = 3;
            this.tabHeader.Text = "Header";
            this.tabHeader.UseVisualStyleBackColor = true;
            // 
            // olvHeader
            // 
            this.olvHeader.AllColumns.Add(this.lvcHeaderName);
            this.olvHeader.AllColumns.Add(this.lvcHeaderAddress);
            this.olvHeader.AllColumns.Add(this.lvcHeaderNumTextures);
            this.olvHeader.AllColumns.Add(this.lvcHeaderTextureIdStart);
            this.olvHeader.AllowColumnReorder = true;
            this.olvHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvHeader.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            this.olvHeader.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.lvcHeaderName,
            this.lvcHeaderAddress,
            this.lvcHeaderNumTextures,
            this.lvcHeaderTextureIdStart});
            this.olvHeader.FullRowSelect = true;
            this.olvHeader.GridLines = true;
            this.olvHeader.HasCollapsibleGroups = false;
            this.olvHeader.HideSelection = false;
            this.olvHeader.Location = new System.Drawing.Point(3, 3);
            this.olvHeader.MenuLabelGroupBy = "";
            this.olvHeader.Name = "olvHeader";
            this.olvHeader.ShowGroups = false;
            this.olvHeader.Size = new System.Drawing.Size(903, 502);
            this.olvHeader.TabIndex = 0;
            this.olvHeader.UseAlternatingBackColors = true;
            this.olvHeader.UseCompatibleStateImageBehavior = false;
            this.olvHeader.View = System.Windows.Forms.View.Details;
            this.olvHeader.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.olvCellEditStarting);
            // 
            // lvcHeaderName
            // 
            this.lvcHeaderName.AspectName = "Name";
            this.lvcHeaderName.IsEditable = false;
            this.lvcHeaderName.Text = "SizeList";
            // 
            // lvcHeaderAddress
            // 
            this.lvcHeaderAddress.AspectName = "Address";
            this.lvcHeaderAddress.AspectToStringFormat = "{0:X4}";
            this.lvcHeaderAddress.IsEditable = false;
            this.lvcHeaderAddress.Text = "Address";
            // 
            // lvcHeaderNumTextures
            // 
            this.lvcHeaderNumTextures.Text = "# Textures";
            this.lvcHeaderNumTextures.Width = 65;
            // 
            // lvcHeaderTextureIdStart
            // 
            this.lvcHeaderTextureIdStart.Text = "Texture ID Start";
            this.lvcHeaderTextureIdStart.Width = 90;
            // 
            // tabMain
            // 
            this.tabMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMain.Controls.Add(this.tabHeader);
            this.tabMain.Controls.Add(this.tabTextures);
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Margin = new System.Windows.Forms.Padding(0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(917, 534);
            this.tabMain.TabIndex = 0;
            // 
            // tabTextures
            // 
            this.tabTextures.Location = new System.Drawing.Point(4, 22);
            this.tabTextures.Name = "tabTextures";
            this.tabTextures.Size = new System.Drawing.Size(909, 508);
            this.tabTextures.TabIndex = 4;
            this.tabTextures.Text = "Textures";
            this.tabTextures.UseVisualStyleBackColor = true;
            // 
            // TextureChunkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tabMain);
            this.Name = "TextureChunkControl";
            this.Size = new System.Drawing.Size(917, 534);
            this.tabHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvHeader)).EndInit();
            this.tabMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabHeader;
        private BrightIdeasSoftware.ObjectListView olvHeader;
        private BrightIdeasSoftware.OLVColumn lvcHeaderName;
        private BrightIdeasSoftware.OLVColumn lvcHeaderAddress;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabTextures;
        private BrightIdeasSoftware.OLVColumn lvcHeaderNumTextures;
        private BrightIdeasSoftware.OLVColumn lvcHeaderTextureIdStart;
    }
}
