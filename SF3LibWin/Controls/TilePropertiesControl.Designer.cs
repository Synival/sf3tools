﻿namespace SF3.Win.Controls {
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
            cbMoveSlope = new System.Windows.Forms.CheckBox();
            nudMoveHeightmapBR = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapBL = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapTR = new System.Windows.Forms.NumericUpDown();
            nudMoveHeightmapTL = new System.Windows.Forms.NumericUpDown();
            nudMoveHeight = new System.Windows.Forms.NumericUpDown();
            labelMoveHeightmap = new System.Windows.Forms.Label();
            labelMoveHeight = new System.Windows.Forms.Label();
            cbMoveTerrain = new System.Windows.Forms.ComboBox();
            labelMoveTerrain = new System.Windows.Forms.Label();
            gbEvent = new System.Windows.Forms.GroupBox();
            nudEventID = new System.Windows.Forms.NumericUpDown();
            labelEventID = new System.Windows.Forms.Label();
            gbModel = new System.Windows.Forms.GroupBox();
            nudModelTextureID = new System.Windows.Forms.NumericUpDown();
            cbModelTileIsFlat = new System.Windows.Forms.CheckBox();
            cbModelRotate = new System.Windows.Forms.ComboBox();
            labelModelRotate = new System.Windows.Forms.Label();
            labelModelFlip = new System.Windows.Forms.Label();
            labelModelTextureID = new System.Windows.Forms.Label();
            cbModelFlip = new System.Windows.Forms.ComboBox();
            labelTileEdited = new System.Windows.Forms.Label();
            labelRealCoordinates = new System.Windows.Forms.Label();
            cbModelHasTree = new System.Windows.Forms.CheckBox();
            gbMovement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTR).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTL).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeight).BeginInit();
            gbEvent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudEventID).BeginInit();
            gbModel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudModelTextureID).BeginInit();
            SuspendLayout();
            // 
            // gbMovement
            // 
            gbMovement.Controls.Add(cbMoveSlope);
            gbMovement.Controls.Add(nudMoveHeightmapBR);
            gbMovement.Controls.Add(nudMoveHeightmapBL);
            gbMovement.Controls.Add(nudMoveHeightmapTR);
            gbMovement.Controls.Add(nudMoveHeightmapTL);
            gbMovement.Controls.Add(nudMoveHeight);
            gbMovement.Controls.Add(labelMoveHeightmap);
            gbMovement.Controls.Add(labelMoveHeight);
            gbMovement.Controls.Add(cbMoveTerrain);
            gbMovement.Controls.Add(labelMoveTerrain);
            gbMovement.Location = new System.Drawing.Point(3, 77);
            gbMovement.Name = "gbMovement";
            gbMovement.Size = new System.Drawing.Size(200, 193);
            gbMovement.TabIndex = 0;
            gbMovement.TabStop = false;
            gbMovement.Text = "Movement";
            // 
            // cbMoveSlope
            // 
            cbMoveSlope.AutoSize = true;
            cbMoveSlope.Location = new System.Drawing.Point(6, 83);
            cbMoveSlope.Name = "cbMoveSlope";
            cbMoveSlope.Size = new System.Drawing.Size(186, 19);
            cbMoveSlope.TabIndex = 2;
            cbMoveSlope.Text = "Reduced Height Move Penalty";
            cbMoveSlope.UseVisualStyleBackColor = true;
            // 
            // nudMoveHeightmapBR
            // 
            nudMoveHeightmapBR.DecimalPlaces = 4;
            nudMoveHeightmapBR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapBR.Location = new System.Drawing.Point(105, 159);
            nudMoveHeightmapBR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapBR.Name = "nudMoveHeightmapBR";
            nudMoveHeightmapBR.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapBR.TabIndex = 6;
            // 
            // nudMoveHeightmapBL
            // 
            nudMoveHeightmapBL.DecimalPlaces = 4;
            nudMoveHeightmapBL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapBL.Location = new System.Drawing.Point(6, 159);
            nudMoveHeightmapBL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapBL.Name = "nudMoveHeightmapBL";
            nudMoveHeightmapBL.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapBL.TabIndex = 5;
            // 
            // nudMoveHeightmapTR
            // 
            nudMoveHeightmapTR.DecimalPlaces = 4;
            nudMoveHeightmapTR.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapTR.Location = new System.Drawing.Point(106, 130);
            nudMoveHeightmapTR.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapTR.Name = "nudMoveHeightmapTR";
            nudMoveHeightmapTR.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapTR.TabIndex = 4;
            // 
            // nudMoveHeightmapTL
            // 
            nudMoveHeightmapTL.DecimalPlaces = 4;
            nudMoveHeightmapTL.Increment = new decimal(new int[] { 625, 0, 0, 262144 });
            nudMoveHeightmapTL.Location = new System.Drawing.Point(6, 130);
            nudMoveHeightmapTL.Maximum = new decimal(new int[] { 159375, 0, 0, 262144 });
            nudMoveHeightmapTL.Name = "nudMoveHeightmapTL";
            nudMoveHeightmapTL.Size = new System.Drawing.Size(88, 23);
            nudMoveHeightmapTL.TabIndex = 3;
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
            labelMoveHeightmap.Location = new System.Drawing.Point(6, 112);
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
            cbMoveTerrain.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cbMoveTerrain.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
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
            // gbEvent
            // 
            gbEvent.Controls.Add(nudEventID);
            gbEvent.Controls.Add(labelEventID);
            gbEvent.Location = new System.Drawing.Point(3, 277);
            gbEvent.Name = "gbEvent";
            gbEvent.Size = new System.Drawing.Size(200, 53);
            gbEvent.TabIndex = 1;
            gbEvent.TabStop = false;
            gbEvent.Text = "Event";
            // 
            // nudEventID
            // 
            nudEventID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            nudEventID.Hexadecimal = true;
            nudEventID.Location = new System.Drawing.Point(73, 23);
            nudEventID.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudEventID.Name = "nudEventID";
            nudEventID.Size = new System.Drawing.Size(120, 21);
            nudEventID.TabIndex = 7;
            // 
            // labelEventID
            // 
            labelEventID.AutoSize = true;
            labelEventID.Location = new System.Drawing.Point(6, 25);
            labelEventID.Name = "labelEventID";
            labelEventID.Size = new System.Drawing.Size(53, 15);
            labelEventID.TabIndex = 10;
            labelEventID.Text = "Event ID:";
            // 
            // gbModel
            // 
            gbModel.Controls.Add(cbModelHasTree);
            gbModel.Controls.Add(nudModelTextureID);
            gbModel.Controls.Add(cbModelTileIsFlat);
            gbModel.Controls.Add(cbModelRotate);
            gbModel.Controls.Add(labelModelRotate);
            gbModel.Controls.Add(labelModelFlip);
            gbModel.Controls.Add(labelModelTextureID);
            gbModel.Controls.Add(cbModelFlip);
            gbModel.Location = new System.Drawing.Point(3, 337);
            gbModel.Name = "gbModel";
            gbModel.Size = new System.Drawing.Size(200, 181);
            gbModel.TabIndex = 2;
            gbModel.TabStop = false;
            gbModel.Text = "Model";
            // 
            // nudModelTextureID
            // 
            nudModelTextureID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            nudModelTextureID.Hexadecimal = true;
            nudModelTextureID.Location = new System.Drawing.Point(73, 20);
            nudModelTextureID.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
            nudModelTextureID.Name = "nudModelTextureID";
            nudModelTextureID.Size = new System.Drawing.Size(120, 21);
            nudModelTextureID.TabIndex = 8;
            // 
            // cbModelTileIsFlat
            // 
            cbModelTileIsFlat.AutoSize = true;
            cbModelTileIsFlat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            cbModelTileIsFlat.Location = new System.Drawing.Point(6, 142);
            cbModelTileIsFlat.Name = "cbModelTileIsFlat";
            cbModelTileIsFlat.Size = new System.Drawing.Size(165, 34);
            cbModelTileIsFlat.TabIndex = 12;
            cbModelTileIsFlat.Text = "Tile is Flat and Unlinked to\r\nNeighboring Tiles";
            cbModelTileIsFlat.UseVisualStyleBackColor = true;
            // 
            // cbModelRotate
            // 
            cbModelRotate.FormattingEnabled = true;
            cbModelRotate.Location = new System.Drawing.Point(73, 80);
            cbModelRotate.Name = "cbModelRotate";
            cbModelRotate.Size = new System.Drawing.Size(121, 23);
            cbModelRotate.TabIndex = 10;
            cbModelRotate.Text = "90° CW";
            // 
            // labelModelRotate
            // 
            labelModelRotate.AutoSize = true;
            labelModelRotate.Location = new System.Drawing.Point(6, 83);
            labelModelRotate.Name = "labelModelRotate";
            labelModelRotate.Size = new System.Drawing.Size(44, 15);
            labelModelRotate.TabIndex = 13;
            labelModelRotate.Text = "Rotate:";
            // 
            // labelModelFlip
            // 
            labelModelFlip.AutoSize = true;
            labelModelFlip.Location = new System.Drawing.Point(6, 112);
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
            cbModelFlip.Location = new System.Drawing.Point(73, 109);
            cbModelFlip.Name = "cbModelFlip";
            cbModelFlip.Size = new System.Drawing.Size(121, 23);
            cbModelFlip.TabIndex = 11;
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
            labelRealCoordinates.Size = new System.Drawing.Size(137, 45);
            labelRealCoordinates.TabIndex = 4;
            labelRealCoordinates.Text = "Center Real Coordinates:\r\nAddress 1:\r\nAddress 2:";
            // 
            // cbModelHasTree
            // 
            cbModelHasTree.AutoSize = true;
            cbModelHasTree.Location = new System.Drawing.Point(7, 54);
            cbModelHasTree.Name = "cbModelHasTree";
            cbModelHasTree.Size = new System.Drawing.Size(71, 19);
            cbModelHasTree.TabIndex = 9;
            cbModelHasTree.Text = "Has Tree";
            cbModelHasTree.UseVisualStyleBackColor = true;
            // 
            // TilePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelRealCoordinates);
            Controls.Add(labelTileEdited);
            Controls.Add(gbModel);
            Controls.Add(gbEvent);
            Controls.Add(gbMovement);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 494);
            Name = "TilePropertiesControl";
            Size = new System.Drawing.Size(207, 521);
            gbMovement.ResumeLayout(false);
            gbMovement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapBL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTR).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeightmapTL).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudMoveHeight).EndInit();
            gbEvent.ResumeLayout(false);
            gbEvent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudEventID).EndInit();
            gbModel.ResumeLayout(false);
            gbModel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudModelTextureID).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gbMovement;
        private System.Windows.Forms.Label labelMoveHeight;
        private System.Windows.Forms.GroupBox gbEvent;
        private System.Windows.Forms.GroupBox gbModel;
        private System.Windows.Forms.Label labelMoveTerrain;
        private System.Windows.Forms.ComboBox cbMoveTerrain;
        private System.Windows.Forms.Label labelMoveHeightmap;
        private System.Windows.Forms.Label labelEventID;
        private System.Windows.Forms.Label labelModelFlip;
        private System.Windows.Forms.Label labelModelTextureID;
        private System.Windows.Forms.ComboBox cbModelFlip;
        private System.Windows.Forms.ComboBox cbModelRotate;
        private System.Windows.Forms.Label labelModelRotate;
        private System.Windows.Forms.CheckBox cbModelTileIsFlat;
        private System.Windows.Forms.Label labelTileEdited;
        private System.Windows.Forms.Label labelRealCoordinates;
        private System.Windows.Forms.NumericUpDown nudMoveHeight;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapBR;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapBL;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapTR;
        private System.Windows.Forms.NumericUpDown nudMoveHeightmapTL;
        private System.Windows.Forms.NumericUpDown nudEventID;
        private System.Windows.Forms.NumericUpDown nudModelTextureID;
        private System.Windows.Forms.CheckBox cbMoveSlope;
        private System.Windows.Forms.CheckBox cbModelHasTree;
    }
}
