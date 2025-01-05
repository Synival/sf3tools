namespace SF3.Win.Controls {
    partial class TilePropertiesControl {
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
            gbMovement = new System.Windows.Forms.GroupBox();
            nudMoveHeightmapBR = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapBL = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapTR = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapTL = new System.Windows.Forms.NumericUpDown();
            nudMoveHeight = new System.Windows.Forms.NumericUpDown();
            labelMoveHeightmap = new System.Windows.Forms.Label();
            labelMoveHeight = new System.Windows.Forms.Label();
            cbMoveTerrain = new System.Windows.Forms.ComboBox();
            labelMoveTerrain = new System.Windows.Forms.Label();
            gbItem = new System.Windows.Forms.GroupBox();
            nudItemID = new System.Windows.Forms.NumericUpDown();
            labelItemID = new System.Windows.Forms.Label();
            gbModel = new System.Windows.Forms.GroupBox();
            nudModelVertexHeightmapBR = new System.Windows.Forms.NumericUpDown();
            nudModelVertexHeightmapBL = new System.Windows.Forms.NumericUpDown();
            nudModelVertexHeightmapTR = new System.Windows.Forms.NumericUpDown();
            nudModelVertexHeightmapTL = new System.Windows.Forms.NumericUpDown();
            nudModelTextureID = new System.Windows.Forms.NumericUpDown();
            cbModelUseMovementHeightmap = new System.Windows.Forms.CheckBox();
            cbModelRotate = new System.Windows.Forms.ComboBox();
            labelModelRotate = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            labelModelFlip = new System.Windows.Forms.Label();
            labelModelTextureID = new System.Windows.Forms.Label();
            cbModelFlip = new System.Windows.Forms.ComboBox();
            labelTileEdited = new System.Windows.Forms.Label();
            labelRealCoordinates = new System.Windows.Forms.Label();
            gbMovement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeight).BeginInit();
            gbItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudItemID).BeginInit();
            gbModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapBR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapBL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapTR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapTL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudModelTextureID).BeginInit();
            SuspendLayout();
            // 
            // gbMovement
            // 
            gbMovement.Controls.Add(nudMoveHeightmapBR);
            gbMovement.Controls.Add(nudMoveHeightmapBL);
            gbMovement.Controls.Add(nudMoveHeightmapTR);
            gbMovement.Controls.Add(nudMoveHeightmapTL);
            gbMovement.Controls.Add(nudMoveHeight);
            gbMovement.Controls.Add(labelMoveHeightmap);
            gbMovement.Controls.Add(labelMoveHeight);
            gbMovement.Controls.Add(cbMoveTerrain);
            gbMovement.Controls.Add(labelMoveTerrain);
            gbMovement.Location = new System.Drawing.Point(3, 53);
            gbMovement.Name = "gbMovement";
            gbMovement.Size = new System.Drawing.Size(200, 163);
            gbMovement.TabIndex = 0;
            gbMovement.TabStop = false;
            gbMovement.Text = "Movement";
            // 
            // nudMoveHeightmapBR
            // 
            nudMoveHeightmapBR.DecimalPlaces = 4;
            nudMoveHeightmapBR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapBR.Location = new System.Drawing.Point(105, 130);
            nudMoveHeightmapBR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapBR.Name = "nudMoveHeightmapBR";
            nudMoveHeightmapBR.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapBR.TabIndex = 5;
            // 
            // nudMoveHeightmapBL
            // 
            nudMoveHeightmapBL.DecimalPlaces = 4;
            nudMoveHeightmapBL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapBL.Location = new System.Drawing.Point(6, 130);
            nudMoveHeightmapBL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapBL.Name = "nudMoveHeightmapBL";
            nudMoveHeightmapBL.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapBL.TabIndex = 4;
            // 
            // nudMoveHeightmapTR
            // 
            nudMoveHeightmapTR.DecimalPlaces = 4;
            nudMoveHeightmapTR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapTR.Location = new System.Drawing.Point(106, 101);
            nudMoveHeightmapTR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapTR.Name = "nudMoveHeightmapTR";
            nudMoveHeightmapTR.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapTR.TabIndex = 3;
            // 
            // nudMoveHeightmapTL
            // 
            nudMoveHeightmapTL.DecimalPlaces = 4;
            nudMoveHeightmapTL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapTL.Location = new System.Drawing.Point(6, 101);
            nudMoveHeightmapTL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapTL.Name = "nudMoveHeightmapTL";
            nudMoveHeightmapTL.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapTL.TabIndex = 2;
            // 
            // nudMoveHeight
            // 
            nudMoveHeight.DecimalPlaces = 4;
            nudMoveHeight.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeight.Location = new System.Drawing.Point(73, 52);
            nudMoveHeight.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeight.Name = "nudMoveHeight";
            nudMoveHeight.Size = new System.Drawing.Size(120, 23);
            nudMoveHeight.TabIndex = 1;
            // 
            // labelMoveHeightmap
            // 
            labelMoveHeightmap.AutoSize = true;
            labelMoveHeightmap.Location = new System.Drawing.Point(6, 83);
            labelMoveHeightmap.Name = "labelMoveHeightmap";
            labelMoveHeightmap.Size = new System.Drawing.Size(70, 15);
            labelMoveHeightmap.TabIndex = 5;
            labelMoveHeightmap.Text = "Heightmap:";
            // 
            // labelMoveHeight
            // 
            labelMoveHeight.AutoSize = true;
            labelMoveHeight.Location = new System.Drawing.Point(6, 54);
            labelMoveHeight.Name = "labelMoveHeight";
            labelMoveHeight.Size = new System.Drawing.Size(46, 15);
            labelMoveHeight.TabIndex = 0;
            labelMoveHeight.Text = "Height:";
            // 
            // cbMoveTerrain
            // 
            cbMoveTerrain.FormattingEnabled = true;
            cbMoveTerrain.Location = new System.Drawing.Point(73, 22);
            cbMoveTerrain.Name = "cbMoveTerrain";
            cbMoveTerrain.Size = new System.Drawing.Size(121, 23);
            cbMoveTerrain.TabIndex = 0;
            cbMoveTerrain.Text = "Desert";
            // 
            // labelMoveTerrain
            // 
            labelMoveTerrain.AutoSize = true;
            labelMoveTerrain.Location = new System.Drawing.Point(6, 25);
            labelMoveTerrain.Name = "labelMoveTerrain";
            labelMoveTerrain.Size = new System.Drawing.Size(46, 15);
            labelMoveTerrain.TabIndex = 3;
            labelMoveTerrain.Text = "Terrain:";
            // 
            // gbItem
            // 
            gbItem.Controls.Add(nudItemID);
            gbItem.Controls.Add(labelItemID);
            gbItem.Location = new System.Drawing.Point(3, 222);
            gbItem.Name = "gbItem";
            gbItem.Size = new System.Drawing.Size(200, 53);
            gbItem.TabIndex = 1;
            gbItem.TabStop = false;
            gbItem.Text = "Item";
            // 
            // nudItemID
            // 
            nudItemID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            nudItemID.Hexadecimal = true;
            nudItemID.Location = new System.Drawing.Point(73, 23);
            nudItemID.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            nudItemID.Name = "nudItemID";
            nudItemID.Size = new System.Drawing.Size(120, 21);
            nudItemID.TabIndex = 6;
            // 
            // labelItemID
            // 
            labelItemID.AutoSize = true;
            labelItemID.Location = new System.Drawing.Point(6, 25);
            labelItemID.Name = "labelItemID";
            labelItemID.Size = new System.Drawing.Size(48, 15);
            labelItemID.TabIndex = 10;
            labelItemID.Text = "Item ID:";
            // 
            // gbModel
            // 
            gbModel.Controls.Add(nudModelVertexHeightmapBR);
            gbModel.Controls.Add(nudModelVertexHeightmapBL);
            gbModel.Controls.Add(nudModelVertexHeightmapTR);
            gbModel.Controls.Add(nudModelVertexHeightmapTL);
            gbModel.Controls.Add(nudModelTextureID);
            gbModel.Controls.Add(cbModelUseMovementHeightmap);
            gbModel.Controls.Add(cbModelRotate);
            gbModel.Controls.Add(labelModelRotate);
            gbModel.Controls.Add(label1);
            gbModel.Controls.Add(labelModelFlip);
            gbModel.Controls.Add(labelModelTextureID);
            gbModel.Controls.Add(cbModelFlip);
            gbModel.Location = new System.Drawing.Point(3, 281);
            gbModel.Name = "gbModel";
            gbModel.Size = new System.Drawing.Size(200, 212);
            gbModel.TabIndex = 2;
            gbModel.TabStop = false;
            gbModel.Text = "Model";
            // 
            // nudModelVertexHeightmapBR
            // 
            nudModelVertexHeightmapBR.DecimalPlaces = 4;
            nudModelVertexHeightmapBR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudModelVertexHeightmapBR.Location = new System.Drawing.Point(105, 182);
            nudModelVertexHeightmapBR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudModelVertexHeightmapBR.Name = "nudModelVertexHeightmapBR";
            nudModelVertexHeightmapBR.Size = new System.Drawing.Size(88, 23);
            nudModelVertexHeightmapBR.TabIndex = 14;
            // 
            // nudModelVertexHeightmapBL
            // 
            nudModelVertexHeightmapBL.DecimalPlaces = 4;
            nudModelVertexHeightmapBL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudModelVertexHeightmapBL.Location = new System.Drawing.Point(6, 182);
            nudModelVertexHeightmapBL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudModelVertexHeightmapBL.Name = "nudModelVertexHeightmapBL";
            nudModelVertexHeightmapBL.Size = new System.Drawing.Size(88, 23);
            nudModelVertexHeightmapBL.TabIndex = 13;
            // 
            // nudModelVertexHeightmapTR
            // 
            nudModelVertexHeightmapTR.DecimalPlaces = 4;
            nudModelVertexHeightmapTR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudModelVertexHeightmapTR.Location = new System.Drawing.Point(106, 153);
            nudModelVertexHeightmapTR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudModelVertexHeightmapTR.Name = "nudModelVertexHeightmapTR";
            nudModelVertexHeightmapTR.Size = new System.Drawing.Size(88, 23);
            nudModelVertexHeightmapTR.TabIndex = 12;
            // 
            // nudModelVertexHeightmapTL
            // 
            nudModelVertexHeightmapTL.DecimalPlaces = 4;
            nudModelVertexHeightmapTL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudModelVertexHeightmapTL.Location = new System.Drawing.Point(6, 153);
            nudModelVertexHeightmapTL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudModelVertexHeightmapTL.Name = "nudModelVertexHeightmapTL";
            nudModelVertexHeightmapTL.Size = new System.Drawing.Size(88, 23);
            nudModelVertexHeightmapTL.TabIndex = 11;
            // 
            // nudModelTextureID
            // 
            nudModelTextureID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            nudModelTextureID.Location = new System.Drawing.Point(73, 20);
            nudModelTextureID.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudModelTextureID.Name = "nudModelTextureID";
            nudModelTextureID.Size = new System.Drawing.Size(120, 21);
            nudModelTextureID.TabIndex = 7;
            // 
            // cbModelUseMovementHeightmap
            // 
            cbModelUseMovementHeightmap.AutoSize = true;
            cbModelUseMovementHeightmap.Location = new System.Drawing.Point(6, 107);
            cbModelUseMovementHeightmap.Name = "cbModelUseMovementHeightmap";
            cbModelUseMovementHeightmap.Size = new System.Drawing.Size(169, 19);
            cbModelUseMovementHeightmap.TabIndex = 10;
            cbModelUseMovementHeightmap.Text = "Use Movement Heightmap";
            cbModelUseMovementHeightmap.UseVisualStyleBackColor = true;
            // 
            // cbModelRotate
            // 
            cbModelRotate.FormattingEnabled = true;
            cbModelRotate.Location = new System.Drawing.Point(73, 78);
            cbModelRotate.Name = "cbModelRotate";
            cbModelRotate.Size = new System.Drawing.Size(121, 23);
            cbModelRotate.TabIndex = 9;
            cbModelRotate.Text = "90° CW";
            // 
            // labelModelRotate
            // 
            labelModelRotate.AutoSize = true;
            labelModelRotate.Location = new System.Drawing.Point(6, 81);
            labelModelRotate.Name = "labelModelRotate";
            labelModelRotate.Size = new System.Drawing.Size(44, 15);
            labelModelRotate.TabIndex = 13;
            labelModelRotate.Text = "Rotate:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(6, 134);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(104, 15);
            label1.TabIndex = 10;
            label1.Text = "Vertex Heightmap:";
            // 
            // labelModelFlip
            // 
            labelModelFlip.AutoSize = true;
            labelModelFlip.Location = new System.Drawing.Point(6, 52);
            labelModelFlip.Name = "labelModelFlip";
            labelModelFlip.Size = new System.Drawing.Size(29, 15);
            labelModelFlip.TabIndex = 12;
            labelModelFlip.Text = "Flip:";
            // 
            // labelModelTextureID
            // 
            labelModelTextureID.AutoSize = true;
            labelModelTextureID.Location = new System.Drawing.Point(6, 25);
            labelModelTextureID.Name = "labelModelTextureID";
            labelModelTextureID.Size = new System.Drawing.Size(62, 15);
            labelModelTextureID.TabIndex = 11;
            labelModelTextureID.Text = "Texture ID:";
            // 
            // cbModelFlip
            // 
            cbModelFlip.FormattingEnabled = true;
            cbModelFlip.Location = new System.Drawing.Point(73, 49);
            cbModelFlip.Name = "cbModelFlip";
            cbModelFlip.Size = new System.Drawing.Size(121, 23);
            cbModelFlip.TabIndex = 8;
            cbModelFlip.Text = "Horizontal";
            // 
            // labelTileEdited
            // 
            labelTileEdited.AutoSize = true;
            labelTileEdited.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelTileEdited.Location = new System.Drawing.Point(5, 5);
            labelTileEdited.Name = "labelTileEdited";
            labelTileEdited.Size = new System.Drawing.Size(69, 15);
            labelTileEdited.TabIndex = 3;
            labelTileEdited.Text = "Tile: (none)";
            // 
            // labelRealCoordinates
            // 
            labelRealCoordinates.AutoSize = true;
            labelRealCoordinates.Location = new System.Drawing.Point(5, 24);
            labelRealCoordinates.Name = "labelRealCoordinates";
            labelRealCoordinates.Size = new System.Drawing.Size(99, 15);
            labelRealCoordinates.TabIndex = 4;
            labelRealCoordinates.Text = "Real Coordinates:";
            // 
            // TilePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelRealCoordinates);
            Controls.Add(labelTileEdited);
            Controls.Add(gbModel);
            Controls.Add(gbItem);
            Controls.Add(gbMovement);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 498);
            Name = "TilePropertiesControl";
            Size = new System.Drawing.Size(207, 498);
            gbMovement.ResumeLayout(false);
            gbMovement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeight).EndInit();
            gbItem.ResumeLayout(false);
            gbItem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudItemID).EndInit();
            gbModel.ResumeLayout(false);
            gbModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapBR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapBL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapTR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudModelVertexHeightmapTL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudModelTextureID).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gbMovement;
        private System.Windows.Forms.Label labelMoveHeight;
        private System.Windows.Forms.GroupBox gbItem;
        private System.Windows.Forms.GroupBox gbModel;
        private System.Windows.Forms.Label labelMoveTerrain;
        private System.Windows.Forms.ComboBox cbMoveTerrain;
        private System.Windows.Forms.Label labelMoveHeightmap;
        private System.Windows.Forms.Label labelItemID;
        private System.Windows.Forms.Label labelModelFlip;
        private System.Windows.Forms.Label labelModelTextureID;
        private System.Windows.Forms.ComboBox cbModelFlip;
        private System.Windows.Forms.ComboBox cbModelRotate;
        private System.Windows.Forms.Label labelModelRotate;
        private System.Windows.Forms.CheckBox cbModelUseMovementHeightmap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTileEdited;
        private System.Windows.Forms.Label labelRealCoordinates;
        private System.Windows.Forms.NumericUpDown nudMoveHeight;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapBR;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapBL;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapTR;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapTL;
        private System.Windows.Forms.NumericUpDown nudItemID;
        private System.Windows.Forms.NumericUpDown nudModelVertexHeightmapBR;
        private System.Windows.Forms.NumericUpDown nudModelVertexHeightmapBL;
        private System.Windows.Forms.NumericUpDown nudModelVertexHeightmapTR;
        private System.Windows.Forms.NumericUpDown nudModelVertexHeightmapTL;
        private System.Windows.Forms.NumericUpDown nudModelTextureID;
    }
}
