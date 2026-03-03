using CommonLib.Win.Controls;

namespace SF3.Win.Controls {
    partial class ModelInstancePropertiesControl {
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
            labelModelInstanceEdited = new DarkModeLabel();
            gbOrientation = new DarkModeGroupBox();
            nudScaleZ = new DarkModeNumericUpDown();
            nudScaleY = new DarkModeNumericUpDown();
            nudScaleX = new DarkModeNumericUpDown();
            labelScale = new DarkModeLabel();
            nudRotationZ = new DarkModeNumericUpDown();
            nudRotationY = new DarkModeNumericUpDown();
            nudRotationX = new DarkModeNumericUpDown();
            cbVisibleFrom = new DarkModeComboBox();
            labelRotation = new DarkModeLabel();
            labelBoundingBox = new DarkModeLabel();
            labelVisibleFrom = new DarkModeLabel();
            labelBottom = new DarkModeLabel();
            nudBottom = new DarkModeNumericUpDown();
            labelTop = new DarkModeLabel();
            nudTop = new DarkModeNumericUpDown();
            labelCoordInfo = new DarkModeLabel();
            darkModeLabel3 = new DarkModeLabel();
            darkModeLabel1 = new DarkModeLabel();
            nudZ = new DarkModeNumericUpDown();
            nudZWorld = new DarkModeNumericUpDown();
            labelX = new DarkModeLabel();
            nudYWorld = new DarkModeNumericUpDown();
            nudXWorld = new DarkModeNumericUpDown();
            darkModeLabel2 = new DarkModeLabel();
            labelPosWorld = new DarkModeLabel();
            labelPosition = new DarkModeLabel();
            nudY = new DarkModeNumericUpDown();
            nudX = new DarkModeNumericUpDown();
            gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudScaleZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudTop).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudYWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudX).BeginInit();
            SuspendLayout();
            // 
            // labelModelInstanceEdited
            // 
            labelModelInstanceEdited.AutoSize = true;
            labelModelInstanceEdited.DisabledColor = System.Drawing.Color.Empty;
            labelModelInstanceEdited.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelModelInstanceEdited.IsSeparator = false;
            labelModelInstanceEdited.Location = new System.Drawing.Point(5, 5);
            labelModelInstanceEdited.Name = "labelModelInstanceEdited";
            labelModelInstanceEdited.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelModelInstanceEdited.Size = new System.Drawing.Size(134, 15);
            labelModelInstanceEdited.TabIndex = 3;
            labelModelInstanceEdited.Text = "Model Instance: (none)";
            // 
            // gbOrientation
            // 
            gbOrientation.Controls.Add(nudScaleZ);
            gbOrientation.Controls.Add(nudScaleY);
            gbOrientation.Controls.Add(nudScaleX);
            gbOrientation.Controls.Add(labelScale);
            gbOrientation.Controls.Add(nudRotationZ);
            gbOrientation.Controls.Add(nudRotationY);
            gbOrientation.Controls.Add(nudRotationX);
            gbOrientation.Controls.Add(cbVisibleFrom);
            gbOrientation.Controls.Add(labelRotation);
            gbOrientation.Controls.Add(labelBoundingBox);
            gbOrientation.Controls.Add(labelVisibleFrom);
            gbOrientation.Controls.Add(labelBottom);
            gbOrientation.Controls.Add(nudBottom);
            gbOrientation.Controls.Add(labelTop);
            gbOrientation.Controls.Add(nudTop);
            gbOrientation.Controls.Add(labelCoordInfo);
            gbOrientation.Controls.Add(darkModeLabel3);
            gbOrientation.Controls.Add(darkModeLabel1);
            gbOrientation.Controls.Add(nudZ);
            gbOrientation.Controls.Add(nudZWorld);
            gbOrientation.Controls.Add(labelX);
            gbOrientation.Controls.Add(nudYWorld);
            gbOrientation.Controls.Add(nudXWorld);
            gbOrientation.Controls.Add(darkModeLabel2);
            gbOrientation.Controls.Add(labelPosWorld);
            gbOrientation.Controls.Add(labelPosition);
            gbOrientation.Controls.Add(nudY);
            gbOrientation.Controls.Add(nudX);
            gbOrientation.Location = new System.Drawing.Point(3, 32);
            gbOrientation.Name = "gbOrientation";
            gbOrientation.Size = new System.Drawing.Size(200, 364);
            gbOrientation.TabIndex = 6;
            gbOrientation.TabStop = false;
            gbOrientation.Text = "Orientation";
            // 
            // nudScaleZ
            // 
            nudScaleZ.DecimalPlaces = 2;
            nudScaleZ.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            nudScaleZ.Location = new System.Drawing.Point(136, 298);
            nudScaleZ.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudScaleZ.Name = "nudScaleZ";
            nudScaleZ.Size = new System.Drawing.Size(58, 23);
            nudScaleZ.TabIndex = 13;
            // 
            // nudScaleY
            // 
            nudScaleY.DecimalPlaces = 2;
            nudScaleY.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            nudScaleY.Location = new System.Drawing.Point(73, 298);
            nudScaleY.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudScaleY.Name = "nudScaleY";
            nudScaleY.Size = new System.Drawing.Size(58, 23);
            nudScaleY.TabIndex = 12;
            // 
            // nudScaleX
            // 
            nudScaleX.DecimalPlaces = 2;
            nudScaleX.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
            nudScaleX.Location = new System.Drawing.Point(9, 298);
            nudScaleX.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudScaleX.Name = "nudScaleX";
            nudScaleX.Size = new System.Drawing.Size(58, 23);
            nudScaleX.TabIndex = 11;
            // 
            // labelScale
            // 
            labelScale.AutoSize = true;
            labelScale.DisabledColor = System.Drawing.Color.Empty;
            labelScale.IsSeparator = false;
            labelScale.Location = new System.Drawing.Point(6, 280);
            labelScale.Name = "labelScale";
            labelScale.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelScale.Size = new System.Drawing.Size(70, 15);
            labelScale.TabIndex = 27;
            labelScale.Text = "Scale X, Y, Z";
            // 
            // nudRotationZ
            // 
            nudRotationZ.DecimalPlaces = 2;
            nudRotationZ.Increment = new decimal(new int[] { 225, 0, 0, 65536 });
            nudRotationZ.Location = new System.Drawing.Point(136, 249);
            nudRotationZ.Maximum = new decimal(new int[] { 18000, 0, 0, 0 });
            nudRotationZ.Minimum = new decimal(new int[] { 18000, 0, 0, int.MinValue });
            nudRotationZ.Name = "nudRotationZ";
            nudRotationZ.Size = new System.Drawing.Size(58, 23);
            nudRotationZ.TabIndex = 10;
            // 
            // nudRotationY
            // 
            nudRotationY.DecimalPlaces = 2;
            nudRotationY.Increment = new decimal(new int[] { 225, 0, 0, 65536 });
            nudRotationY.Location = new System.Drawing.Point(73, 249);
            nudRotationY.Maximum = new decimal(new int[] { 18000, 0, 0, 0 });
            nudRotationY.Minimum = new decimal(new int[] { 18000, 0, 0, int.MinValue });
            nudRotationY.Name = "nudRotationY";
            nudRotationY.Size = new System.Drawing.Size(58, 23);
            nudRotationY.TabIndex = 9;
            // 
            // nudRotationX
            // 
            nudRotationX.DecimalPlaces = 2;
            nudRotationX.Increment = new decimal(new int[] { 225, 0, 0, 65536 });
            nudRotationX.Location = new System.Drawing.Point(9, 249);
            nudRotationX.Maximum = new decimal(new int[] { 18000, 0, 0, 0 });
            nudRotationX.Minimum = new decimal(new int[] { 18000, 0, 0, int.MinValue });
            nudRotationX.Name = "nudRotationX";
            nudRotationX.Size = new System.Drawing.Size(58, 23);
            nudRotationX.TabIndex = 8;
            // 
            // cbVisibleFrom
            // 
            cbVisibleFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cbVisibleFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbVisibleFrom.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cbVisibleFrom.FormattingEnabled = true;
            cbVisibleFrom.Location = new System.Drawing.Point(88, 334);
            cbVisibleFrom.Name = "cbVisibleFrom";
            cbVisibleFrom.Size = new System.Drawing.Size(109, 24);
            cbVisibleFrom.TabIndex = 14;
            cbVisibleFrom.Text = "(Any Direction)";
            cbVisibleFrom.UseWaitCursor = true;
            // 
            // labelRotation
            // 
            labelRotation.AutoSize = true;
            labelRotation.DisabledColor = System.Drawing.Color.Empty;
            labelRotation.IsSeparator = false;
            labelRotation.Location = new System.Drawing.Point(6, 231);
            labelRotation.Name = "labelRotation";
            labelRotation.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelRotation.Size = new System.Drawing.Size(149, 15);
            labelRotation.TabIndex = 7;
            labelRotation.Text = "Rotation X, Y, Z (in degees)";
            // 
            // labelBoundingBox
            // 
            labelBoundingBox.AutoSize = true;
            labelBoundingBox.DisabledColor = System.Drawing.Color.Empty;
            labelBoundingBox.IsSeparator = false;
            labelBoundingBox.Location = new System.Drawing.Point(6, 133);
            labelBoundingBox.Name = "labelBoundingBox";
            labelBoundingBox.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelBoundingBox.Size = new System.Drawing.Size(84, 15);
            labelBoundingBox.TabIndex = 26;
            labelBoundingBox.Text = "Bounding Box:";
            // 
            // labelVisibleFrom
            // 
            labelVisibleFrom.AutoSize = true;
            labelVisibleFrom.DisabledColor = System.Drawing.Color.Empty;
            labelVisibleFrom.IsSeparator = false;
            labelVisibleFrom.Location = new System.Drawing.Point(9, 337);
            labelVisibleFrom.Name = "labelVisibleFrom";
            labelVisibleFrom.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelVisibleFrom.Size = new System.Drawing.Size(73, 15);
            labelVisibleFrom.TabIndex = 7;
            labelVisibleFrom.Text = "Visible from:";
            // 
            // labelBottom
            // 
            labelBottom.AutoSize = true;
            labelBottom.DisabledColor = System.Drawing.Color.Empty;
            labelBottom.IsSeparator = false;
            labelBottom.Location = new System.Drawing.Point(82, 162);
            labelBottom.Name = "labelBottom";
            labelBottom.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelBottom.Size = new System.Drawing.Size(50, 15);
            labelBottom.TabIndex = 25;
            labelBottom.Text = "Bottom:";
            // 
            // nudBottom
            // 
            nudBottom.DecimalPlaces = 2;
            nudBottom.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudBottom.Location = new System.Drawing.Point(137, 160);
            nudBottom.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudBottom.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudBottom.Name = "nudBottom";
            nudBottom.Size = new System.Drawing.Size(58, 23);
            nudBottom.TabIndex = 7;
            // 
            // labelTop
            // 
            labelTop.AutoSize = true;
            labelTop.DisabledColor = System.Drawing.Color.Empty;
            labelTop.IsSeparator = false;
            labelTop.Location = new System.Drawing.Point(101, 133);
            labelTop.Name = "labelTop";
            labelTop.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelTop.Size = new System.Drawing.Size(30, 15);
            labelTop.TabIndex = 23;
            labelTop.Text = "Top:";
            // 
            // nudTop
            // 
            nudTop.DecimalPlaces = 2;
            nudTop.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudTop.Location = new System.Drawing.Point(136, 131);
            nudTop.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudTop.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudTop.Name = "nudTop";
            nudTop.Size = new System.Drawing.Size(58, 23);
            nudTop.TabIndex = 6;
            // 
            // labelCoordInfo
            // 
            labelCoordInfo.AutoSize = true;
            labelCoordInfo.DisabledColor = System.Drawing.Color.Empty;
            labelCoordInfo.IsSeparator = false;
            labelCoordInfo.Location = new System.Drawing.Point(2, 189);
            labelCoordInfo.Name = "labelCoordInfo";
            labelCoordInfo.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelCoordInfo.Size = new System.Drawing.Size(196, 30);
            labelCoordInfo.TabIndex = 21;
            labelCoordInfo.Text = "* World coordinate signs are flipped\r\n** Grid Y is in heightmap units";
            // 
            // darkModeLabel3
            // 
            darkModeLabel3.AutoSize = true;
            darkModeLabel3.DisabledColor = System.Drawing.Color.Empty;
            darkModeLabel3.IsSeparator = false;
            darkModeLabel3.Location = new System.Drawing.Point(50, 98);
            darkModeLabel3.Name = "darkModeLabel3";
            darkModeLabel3.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel3.Size = new System.Drawing.Size(17, 15);
            darkModeLabel3.TabIndex = 20;
            darkModeLabel3.Text = "Z:";
            // 
            // darkModeLabel1
            // 
            darkModeLabel1.AutoSize = true;
            darkModeLabel1.DisabledColor = System.Drawing.Color.Empty;
            darkModeLabel1.IsSeparator = false;
            darkModeLabel1.Location = new System.Drawing.Point(50, 69);
            darkModeLabel1.Name = "darkModeLabel1";
            darkModeLabel1.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel1.Size = new System.Drawing.Size(17, 15);
            darkModeLabel1.TabIndex = 19;
            darkModeLabel1.Text = "Y:";
            // 
            // nudZ
            // 
            nudZ.DecimalPlaces = 2;
            nudZ.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudZ.Location = new System.Drawing.Point(136, 96);
            nudZ.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudZ.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudZ.Name = "nudZ";
            nudZ.Size = new System.Drawing.Size(58, 23);
            nudZ.TabIndex = 5;
            // 
            // nudZWorld
            // 
            nudZWorld.DecimalPlaces = 2;
            nudZWorld.Hexadecimal = true;
            nudZWorld.Location = new System.Drawing.Point(73, 96);
            nudZWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudZWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudZWorld.Name = "nudZWorld";
            nudZWorld.Size = new System.Drawing.Size(58, 23);
            nudZWorld.TabIndex = 2;
            // 
            // labelX
            // 
            labelX.AutoSize = true;
            labelX.DisabledColor = System.Drawing.Color.Empty;
            labelX.IsSeparator = false;
            labelX.Location = new System.Drawing.Point(50, 40);
            labelX.Name = "labelX";
            labelX.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelX.Size = new System.Drawing.Size(17, 15);
            labelX.TabIndex = 15;
            labelX.Text = "X:";
            // 
            // nudYWorld
            // 
            nudYWorld.DecimalPlaces = 2;
            nudYWorld.Hexadecimal = true;
            nudYWorld.Location = new System.Drawing.Point(73, 67);
            nudYWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudYWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudYWorld.Name = "nudYWorld";
            nudYWorld.Size = new System.Drawing.Size(58, 23);
            nudYWorld.TabIndex = 1;
            // 
            // nudXWorld
            // 
            nudXWorld.DecimalPlaces = 2;
            nudXWorld.Hexadecimal = true;
            nudXWorld.Location = new System.Drawing.Point(73, 38);
            nudXWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudXWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudXWorld.Name = "nudXWorld";
            nudXWorld.Size = new System.Drawing.Size(58, 23);
            nudXWorld.TabIndex = 0;
            // 
            // darkModeLabel2
            // 
            darkModeLabel2.AutoSize = true;
            darkModeLabel2.DisabledColor = System.Drawing.Color.Empty;
            darkModeLabel2.IsSeparator = false;
            darkModeLabel2.Location = new System.Drawing.Point(136, 20);
            darkModeLabel2.Name = "darkModeLabel2";
            darkModeLabel2.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel2.Size = new System.Drawing.Size(47, 15);
            darkModeLabel2.TabIndex = 14;
            darkModeLabel2.Text = "(Grid**)";
            // 
            // labelPosWorld
            // 
            labelPosWorld.AutoSize = true;
            labelPosWorld.DisabledColor = System.Drawing.Color.Empty;
            labelPosWorld.IsSeparator = false;
            labelPosWorld.Location = new System.Drawing.Point(73, 20);
            labelPosWorld.Name = "labelPosWorld";
            labelPosWorld.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPosWorld.Size = new System.Drawing.Size(52, 15);
            labelPosWorld.TabIndex = 13;
            labelPosWorld.Text = "(World*)";
            // 
            // labelPosition
            // 
            labelPosition.AutoSize = true;
            labelPosition.DisabledColor = System.Drawing.Color.Empty;
            labelPosition.IsSeparator = false;
            labelPosition.Location = new System.Drawing.Point(6, 20);
            labelPosition.Name = "labelPosition";
            labelPosition.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPosition.Size = new System.Drawing.Size(53, 15);
            labelPosition.TabIndex = 12;
            labelPosition.Text = "Position:";
            // 
            // nudY
            // 
            nudY.DecimalPlaces = 1;
            nudY.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudY.Location = new System.Drawing.Point(136, 67);
            nudY.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudY.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudY.Name = "nudY";
            nudY.Size = new System.Drawing.Size(58, 23);
            nudY.TabIndex = 4;
            // 
            // nudX
            // 
            nudX.DecimalPlaces = 2;
            nudX.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudX.Location = new System.Drawing.Point(136, 38);
            nudX.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudX.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudX.Name = "nudX";
            nudX.Size = new System.Drawing.Size(58, 23);
            nudX.TabIndex = 3;
            // 
            // ModelInstancePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbOrientation);
            Controls.Add(labelModelInstanceEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ModelInstancePropertiesControl";
            Size = new System.Drawing.Size(207, 399);
            gbOrientation.ResumeLayout(false);
            gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudScaleZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudTop).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudYWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudX).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelModelInstanceEdited;
        private DarkModeGroupBox gbOrientation;
        private DarkModeNumericUpDown nudYWorld;
        private DarkModeNumericUpDown nudXWorld;
        private DarkModeLabel darkModeLabel2;
        private DarkModeLabel labelPosWorld;
        private DarkModeLabel labelPosition;
        private DarkModeNumericUpDown nudY;
        private DarkModeNumericUpDown nudX;
        private DarkModeLabel darkModeLabel1;
        private DarkModeLabel labelVisibleFrom;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudZWorld;
        private DarkModeLabel labelX;
        private DarkModeLabel darkModeLabel3;
        private DarkModeLabel labelCoordInfo;
        private DarkModeNumericUpDown nudBottom;
        private DarkModeLabel labelTop;
        private DarkModeNumericUpDown nudTop;
        private DarkModeLabel labelBottom;
        private DarkModeLabel labelBoundingBox;
        private DarkModeComboBox cbVisibleFrom;
        private DarkModeNumericUpDown nudRotationX;
        private DarkModeLabel labelRotation;
        private DarkModeNumericUpDown nudScaleZ;
        private DarkModeNumericUpDown nudScaleY;
        private DarkModeNumericUpDown nudScaleX;
        private DarkModeLabel labelScale;
        private DarkModeNumericUpDown nudRotationZ;
        private DarkModeNumericUpDown nudRotationY;
    }
}
