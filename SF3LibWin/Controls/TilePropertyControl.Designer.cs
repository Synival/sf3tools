namespace SF3.Win.Controls {
    partial class TilePropertyControl {
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
            tbMoveHeightmapBR = new System.Windows.Forms.TextBox();
            tbMoveHeightmapBL = new System.Windows.Forms.TextBox();
            tbMoveHeightmapTR = new System.Windows.Forms.TextBox();
            tbMoveHeightmapTL = new System.Windows.Forms.TextBox();
            labelMoveHeightmap = new System.Windows.Forms.Label();
            tbMoveHeight = new System.Windows.Forms.TextBox();
            labelMoveHeight = new System.Windows.Forms.Label();
            cbMoveTerrain = new System.Windows.Forms.ComboBox();
            labelMoveTerrain = new System.Windows.Forms.Label();
            gbItem = new System.Windows.Forms.GroupBox();
            labelItemID = new System.Windows.Forms.Label();
            tbItemID = new System.Windows.Forms.TextBox();
            gbModel = new System.Windows.Forms.GroupBox();
            textBox1 = new System.Windows.Forms.TextBox();
            cbModelUseMovementHeightmap = new System.Windows.Forms.CheckBox();
            textBox2 = new System.Windows.Forms.TextBox();
            cbModelRotate = new System.Windows.Forms.ComboBox();
            textBox3 = new System.Windows.Forms.TextBox();
            textBox4 = new System.Windows.Forms.TextBox();
            labelModelRotate = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            labelModelFlip = new System.Windows.Forms.Label();
            labelModelTextureID = new System.Windows.Forms.Label();
            tbModelTextureID = new System.Windows.Forms.TextBox();
            cbModelFlip = new System.Windows.Forms.ComboBox();
            labelTileEdited = new System.Windows.Forms.Label();
            gbMovement.SuspendLayout();
            gbItem.SuspendLayout();
            gbModel.SuspendLayout();
            SuspendLayout();
            // 
            // gbMovement
            // 
            gbMovement.Controls.Add(tbMoveHeightmapBR);
            gbMovement.Controls.Add(tbMoveHeightmapBL);
            gbMovement.Controls.Add(tbMoveHeightmapTR);
            gbMovement.Controls.Add(tbMoveHeightmapTL);
            gbMovement.Controls.Add(labelMoveHeightmap);
            gbMovement.Controls.Add(tbMoveHeight);
            gbMovement.Controls.Add(labelMoveHeight);
            gbMovement.Controls.Add(cbMoveTerrain);
            gbMovement.Controls.Add(labelMoveTerrain);
            gbMovement.Location = new System.Drawing.Point(3, 29);
            gbMovement.Name = "gbMovement";
            gbMovement.Size = new System.Drawing.Size(200, 163);
            gbMovement.TabIndex = 0;
            gbMovement.TabStop = false;
            gbMovement.Text = "Movement";
            // 
            // tbMoveHeightmapBR
            // 
            tbMoveHeightmapBR.Location = new System.Drawing.Point(106, 130);
            tbMoveHeightmapBR.Name = "tbMoveHeightmapBR";
            tbMoveHeightmapBR.Size = new System.Drawing.Size(88, 23);
            tbMoveHeightmapBR.TabIndex = 5;
            tbMoveHeightmapBR.Text = "6.125";
            // 
            // tbMoveHeightmapBL
            // 
            tbMoveHeightmapBL.Location = new System.Drawing.Point(6, 130);
            tbMoveHeightmapBL.Name = "tbMoveHeightmapBL";
            tbMoveHeightmapBL.Size = new System.Drawing.Size(88, 23);
            tbMoveHeightmapBL.TabIndex = 4;
            tbMoveHeightmapBL.Text = "5";
            // 
            // tbMoveHeightmapTR
            // 
            tbMoveHeightmapTR.Location = new System.Drawing.Point(106, 101);
            tbMoveHeightmapTR.Name = "tbMoveHeightmapTR";
            tbMoveHeightmapTR.Size = new System.Drawing.Size(88, 23);
            tbMoveHeightmapTR.TabIndex = 3;
            tbMoveHeightmapTR.Text = "8";
            // 
            // tbMoveHeightmapTL
            // 
            tbMoveHeightmapTL.Location = new System.Drawing.Point(6, 101);
            tbMoveHeightmapTL.Name = "tbMoveHeightmapTL";
            tbMoveHeightmapTL.Size = new System.Drawing.Size(88, 23);
            tbMoveHeightmapTL.TabIndex = 2;
            tbMoveHeightmapTL.Text = "3.75";
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
            // tbMoveHeight
            // 
            tbMoveHeight.Location = new System.Drawing.Point(73, 51);
            tbMoveHeight.Name = "tbMoveHeight";
            tbMoveHeight.Size = new System.Drawing.Size(121, 23);
            tbMoveHeight.TabIndex = 1;
            tbMoveHeight.Text = "6.75";
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
            gbItem.Controls.Add(labelItemID);
            gbItem.Controls.Add(tbItemID);
            gbItem.Location = new System.Drawing.Point(3, 198);
            gbItem.Name = "gbItem";
            gbItem.Size = new System.Drawing.Size(200, 53);
            gbItem.TabIndex = 1;
            gbItem.TabStop = false;
            gbItem.Text = "Item";
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
            // tbItemID
            // 
            tbItemID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbItemID.Location = new System.Drawing.Point(73, 22);
            tbItemID.Name = "tbItemID";
            tbItemID.Size = new System.Drawing.Size(121, 21);
            tbItemID.TabIndex = 6;
            tbItemID.Text = "007B";
            // 
            // gbModel
            // 
            gbModel.Controls.Add(textBox1);
            gbModel.Controls.Add(cbModelUseMovementHeightmap);
            gbModel.Controls.Add(textBox2);
            gbModel.Controls.Add(cbModelRotate);
            gbModel.Controls.Add(textBox3);
            gbModel.Controls.Add(textBox4);
            gbModel.Controls.Add(labelModelRotate);
            gbModel.Controls.Add(label1);
            gbModel.Controls.Add(labelModelFlip);
            gbModel.Controls.Add(labelModelTextureID);
            gbModel.Controls.Add(tbModelTextureID);
            gbModel.Controls.Add(cbModelFlip);
            gbModel.Location = new System.Drawing.Point(3, 257);
            gbModel.Name = "gbModel";
            gbModel.Size = new System.Drawing.Size(200, 212);
            gbModel.TabIndex = 2;
            gbModel.TabStop = false;
            gbModel.Text = "Model";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(106, 181);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(88, 23);
            textBox1.TabIndex = 14;
            textBox1.Text = "6.125";
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
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(6, 181);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(88, 23);
            textBox2.TabIndex = 13;
            textBox2.Text = "5";
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
            // textBox3
            // 
            textBox3.Location = new System.Drawing.Point(106, 152);
            textBox3.Name = "textBox3";
            textBox3.Size = new System.Drawing.Size(88, 23);
            textBox3.TabIndex = 12;
            textBox3.Text = "8";
            // 
            // textBox4
            // 
            textBox4.Location = new System.Drawing.Point(6, 152);
            textBox4.Name = "textBox4";
            textBox4.Size = new System.Drawing.Size(88, 23);
            textBox4.TabIndex = 11;
            textBox4.Text = "3.75";
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
            // tbModelTextureID
            // 
            tbModelTextureID.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point,  0);
            tbModelTextureID.Location = new System.Drawing.Point(73, 22);
            tbModelTextureID.Name = "tbModelTextureID";
            tbModelTextureID.Size = new System.Drawing.Size(121, 21);
            tbModelTextureID.TabIndex = 7;
            tbModelTextureID.Text = "1F";
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
            labelTileEdited.Size = new System.Drawing.Size(75, 15);
            labelTileEdited.TabIndex = 3;
            labelTileEdited.Text = "Tile (56, 31):";
            // 
            // TilePropertyControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelTileEdited);
            Controls.Add(gbModel);
            Controls.Add(gbItem);
            Controls.Add(gbMovement);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 473);
            Name = "TilePropertyControl";
            Size = new System.Drawing.Size(207, 473);
            gbMovement.ResumeLayout(false);
            gbMovement.PerformLayout();
            gbItem.ResumeLayout(false);
            gbItem.PerformLayout();
            gbModel.ResumeLayout(false);
            gbModel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.GroupBox gbMovement;
        private System.Windows.Forms.Label labelMoveHeight;
        private System.Windows.Forms.GroupBox gbItem;
        private System.Windows.Forms.GroupBox gbModel;
        private System.Windows.Forms.TextBox tbMoveHeight;
        private System.Windows.Forms.Label labelMoveTerrain;
        private System.Windows.Forms.ComboBox cbMoveTerrain;
        private System.Windows.Forms.Label labelMoveHeightmap;
        private System.Windows.Forms.TextBox tbMoveHeightmapBR;
        private System.Windows.Forms.TextBox tbMoveHeightmapBL;
        private System.Windows.Forms.TextBox tbMoveHeightmapTR;
        private System.Windows.Forms.TextBox tbMoveHeightmapTL;
        private System.Windows.Forms.TextBox tbItemID;
        private System.Windows.Forms.Label labelItemID;
        private System.Windows.Forms.Label labelModelFlip;
        private System.Windows.Forms.Label labelModelTextureID;
        private System.Windows.Forms.TextBox tbModelTextureID;
        private System.Windows.Forms.ComboBox cbModelFlip;
        private System.Windows.Forms.ComboBox cbModelRotate;
        private System.Windows.Forms.Label labelModelRotate;
        private System.Windows.Forms.CheckBox cbModelUseMovementHeightmap;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTileEdited;
    }
}
