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
            tabHeader = new System.Windows.Forms.TabPage();
            olvHeader = new BrightIdeasSoftware.ObjectListView();
            lvcHeaderName = new BrightIdeasSoftware.OLVColumn();
            lvcHeaderAddress = new BrightIdeasSoftware.OLVColumn();
            lvcHeaderNumTextures = new BrightIdeasSoftware.OLVColumn();
            lvcHeaderTextureIdStart = new BrightIdeasSoftware.OLVColumn();
            tabMain = new System.Windows.Forms.TabControl();
            tabTextures = new System.Windows.Forms.TabPage();
            textureControl = new SF3.Win.Controls.TextureControl();
            olvTextures = new BrightIdeasSoftware.ObjectListView();
            lvcTexturesID = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesName = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesAddress = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesWidth = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesHeight = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesImageDataOffset = new BrightIdeasSoftware.OLVColumn();
            lvcTexturesAssumedPixelFormat = new BrightIdeasSoftware.OLVColumn();
            highlightTextRenderer1 = new BrightIdeasSoftware.HighlightTextRenderer();
            highlightTextRenderer2 = new BrightIdeasSoftware.HighlightTextRenderer();
            tabHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvHeader).BeginInit();
            tabMain.SuspendLayout();
            tabTextures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) olvTextures).BeginInit();
            SuspendLayout();
            // 
            // tabHeader
            // 
            tabHeader.Controls.Add(olvHeader);
            tabHeader.Location = new System.Drawing.Point(4, 24);
            tabHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabHeader.Name = "tabHeader";
            tabHeader.Size = new System.Drawing.Size(1062, 588);
            tabHeader.TabIndex = 3;
            tabHeader.Text = "Header";
            tabHeader.UseVisualStyleBackColor = true;
            // 
            // olvHeader
            // 
            olvHeader.AllColumns.Add(lvcHeaderName);
            olvHeader.AllColumns.Add(lvcHeaderAddress);
            olvHeader.AllColumns.Add(lvcHeaderNumTextures);
            olvHeader.AllColumns.Add(lvcHeaderTextureIdStart);
            olvHeader.AllowColumnReorder = true;
            olvHeader.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvHeader.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvHeader.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcHeaderName, lvcHeaderAddress, lvcHeaderNumTextures, lvcHeaderTextureIdStart });
            olvHeader.FullRowSelect = true;
            olvHeader.GridLines = true;
            olvHeader.HasCollapsibleGroups = false;
            olvHeader.Location = new System.Drawing.Point(4, 3);
            olvHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvHeader.MenuLabelGroupBy = "";
            olvHeader.Name = "olvHeader";
            olvHeader.ShowGroups = false;
            olvHeader.Size = new System.Drawing.Size(1053, 579);
            olvHeader.TabIndex = 0;
            olvHeader.UseAlternatingBackColors = true;
            olvHeader.UseCompatibleStateImageBehavior = false;
            olvHeader.View = System.Windows.Forms.View.Details;
            // 
            // lvcHeaderName
            // 
            lvcHeaderName.AspectName = "Name";
            lvcHeaderName.IsEditable = false;
            lvcHeaderName.Text = "Name";
            // 
            // lvcHeaderAddress
            // 
            lvcHeaderAddress.AspectName = "Address";
            lvcHeaderAddress.AspectToStringFormat = "{0:X4}";
            lvcHeaderAddress.IsEditable = false;
            lvcHeaderAddress.Text = "Address";
            // 
            // lvcHeaderNumTextures
            // 
            lvcHeaderNumTextures.AspectName = "NumTextures";
            lvcHeaderNumTextures.Text = "# Textures";
            lvcHeaderNumTextures.Width = 65;
            // 
            // lvcHeaderTextureIdStart
            // 
            lvcHeaderTextureIdStart.AspectName = "TextureIdStart";
            lvcHeaderTextureIdStart.AspectToStringFormat = "{0:X2}";
            lvcHeaderTextureIdStart.Text = "Texture ID Start";
            lvcHeaderTextureIdStart.Width = 90;
            // 
            // tabMain
            // 
            tabMain.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabMain.Controls.Add(tabHeader);
            tabMain.Controls.Add(tabTextures);
            tabMain.Location = new System.Drawing.Point(0, 0);
            tabMain.Margin = new System.Windows.Forms.Padding(0);
            tabMain.Name = "tabMain";
            tabMain.SelectedIndex = 0;
            tabMain.Size = new System.Drawing.Size(1070, 616);
            tabMain.TabIndex = 0;
            // 
            // tabTextures
            // 
            tabTextures.Controls.Add(textureControl);
            tabTextures.Controls.Add(olvTextures);
            tabTextures.Location = new System.Drawing.Point(4, 24);
            tabTextures.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabTextures.Name = "tabTextures";
            tabTextures.Size = new System.Drawing.Size(1062, 588);
            tabTextures.TabIndex = 4;
            tabTextures.Text = "Textures";
            tabTextures.UseVisualStyleBackColor = true;
            // 
            // textureControl
            // 
            textureControl.Anchor =  System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            textureControl.BackColor = System.Drawing.Color.Transparent;
            textureControl.Location = new System.Drawing.Point(863, 391);
            textureControl.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            textureControl.Name = "textureControl";
            textureControl.Size = new System.Drawing.Size(175, 173);
            textureControl.TabIndex = 2;
            textureControl.TextureImage = null;
            // 
            // olvTextures
            // 
            olvTextures.AllColumns.Add(lvcTexturesID);
            olvTextures.AllColumns.Add(lvcTexturesName);
            olvTextures.AllColumns.Add(lvcTexturesAddress);
            olvTextures.AllColumns.Add(lvcTexturesWidth);
            olvTextures.AllColumns.Add(lvcTexturesHeight);
            olvTextures.AllColumns.Add(lvcTexturesImageDataOffset);
            olvTextures.AllColumns.Add(lvcTexturesAssumedPixelFormat);
            olvTextures.AllowColumnReorder = true;
            olvTextures.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            olvTextures.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.SingleClick;
            olvTextures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvcTexturesID, lvcTexturesName, lvcTexturesAddress, lvcTexturesWidth, lvcTexturesHeight, lvcTexturesImageDataOffset, lvcTexturesAssumedPixelFormat });
            olvTextures.FullRowSelect = true;
            olvTextures.GridLines = true;
            olvTextures.HasCollapsibleGroups = false;
            olvTextures.Location = new System.Drawing.Point(4, 3);
            olvTextures.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            olvTextures.MenuLabelGroupBy = "";
            olvTextures.Name = "olvTextures";
            olvTextures.ShowGroups = false;
            olvTextures.Size = new System.Drawing.Size(1053, 579);
            olvTextures.TabIndex = 1;
            olvTextures.UseAlternatingBackColors = true;
            olvTextures.UseCompatibleStateImageBehavior = false;
            olvTextures.View = System.Windows.Forms.View.Details;
            // 
            // lvcTexturesID
            // 
            lvcTexturesID.AspectName = "ID";
            lvcTexturesID.AspectToStringFormat = "{0:X2}";
            lvcTexturesID.Text = "ID";
            // 
            // lvcTexturesName
            // 
            lvcTexturesName.AspectName = "Name";
            lvcTexturesName.IsEditable = false;
            lvcTexturesName.Text = "Name";
            // 
            // lvcTexturesAddress
            // 
            lvcTexturesAddress.AspectName = "Address";
            lvcTexturesAddress.AspectToStringFormat = "{0:X4}";
            lvcTexturesAddress.IsEditable = false;
            lvcTexturesAddress.Text = "Address";
            // 
            // lvcTexturesWidth
            // 
            lvcTexturesWidth.AspectName = "Width";
            lvcTexturesWidth.Text = "Width";
            // 
            // lvcTexturesHeight
            // 
            lvcTexturesHeight.AspectName = "Height";
            lvcTexturesHeight.Text = "Height";
            // 
            // lvcTexturesImageDataOffset
            // 
            lvcTexturesImageDataOffset.AspectName = "ImageDataOffset";
            lvcTexturesImageDataOffset.AspectToStringFormat = "{0:X4}";
            lvcTexturesImageDataOffset.Text = "ImageDataOffset";
            lvcTexturesImageDataOffset.Width = 100;
            // 
            // lvcTexturesAssumedPixelFormat
            // 
            lvcTexturesAssumedPixelFormat.AspectName = "AssumedPixelFormat";
            lvcTexturesAssumedPixelFormat.IsEditable = false;
            lvcTexturesAssumedPixelFormat.Text = "(Assumed) Pixel Format";
            lvcTexturesAssumedPixelFormat.Width = 130;
            // 
            // highlightTextRenderer1
            // 
            highlightTextRenderer1.StringComparison = System.StringComparison.CurrentCultureIgnoreCase;
            highlightTextRenderer1.TextToHighlight = null;
            // 
            // highlightTextRenderer2
            // 
            highlightTextRenderer2.StringComparison = System.StringComparison.CurrentCultureIgnoreCase;
            highlightTextRenderer2.TextToHighlight = null;
            // 
            // TextureChunkControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Transparent;
            Controls.Add(tabMain);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "TextureChunkControl";
            Size = new System.Drawing.Size(1070, 616);
            tabHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvHeader).EndInit();
            tabMain.ResumeLayout(false);
            tabTextures.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize) olvTextures).EndInit();
            ResumeLayout(false);
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
        private BrightIdeasSoftware.ObjectListView olvTextures;
        private BrightIdeasSoftware.OLVColumn lvcTexturesName;
        private BrightIdeasSoftware.OLVColumn lvcTexturesAddress;
        private BrightIdeasSoftware.OLVColumn lvcTexturesWidth;
        private BrightIdeasSoftware.OLVColumn lvcTexturesHeight;
        private BrightIdeasSoftware.OLVColumn lvcTexturesImageDataOffset;
        private BrightIdeasSoftware.OLVColumn lvcTexturesID;
        private BrightIdeasSoftware.HighlightTextRenderer highlightTextRenderer1;
        private BrightIdeasSoftware.HighlightTextRenderer highlightTextRenderer2;
        private SF3.Win.Controls.TextureControl textureControl;
        private BrightIdeasSoftware.OLVColumn lvcTexturesAssumedPixelFormat;
    }
}
