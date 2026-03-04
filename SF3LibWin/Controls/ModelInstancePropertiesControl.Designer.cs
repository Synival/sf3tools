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
            labelVisibleFrom = new DarkModeLabel();
            nudZ = new DarkModeNumericUpDown();
            nudZWorld = new DarkModeNumericUpDown();
            nudYWorld = new DarkModeNumericUpDown();
            nudXWorld = new DarkModeNumericUpDown();
            labelPositionGrid = new DarkModeLabel();
            labelPositionWorld = new DarkModeLabel();
            nudY = new DarkModeNumericUpDown();
            nudX = new DarkModeNumericUpDown();
            labelBottom = new DarkModeLabel();
            nudBottom = new DarkModeNumericUpDown();
            labelTop = new DarkModeLabel();
            nudTop = new DarkModeNumericUpDown();
            labelCoordInfo = new DarkModeLabel();
            gbBoundingBox = new DarkModeGroupBox();
            labelWest = new DarkModeLabel();
            labelRight = new DarkModeLabel();
            labelSouth = new DarkModeLabel();
            labelNorth = new DarkModeLabel();
            nudSouth = new DarkModeNumericUpDown();
            nudEast = new DarkModeNumericUpDown();
            nudWest = new DarkModeNumericUpDown();
            nudNorth = new DarkModeNumericUpDown();
            gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudScaleZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudYWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudBottom).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudTop).BeginInit();
            gbBoundingBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudSouth).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudEast).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudWest).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudNorth).BeginInit();
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
            gbOrientation.Controls.Add(labelVisibleFrom);
            gbOrientation.Controls.Add(nudZ);
            gbOrientation.Controls.Add(nudZWorld);
            gbOrientation.Controls.Add(nudYWorld);
            gbOrientation.Controls.Add(nudXWorld);
            gbOrientation.Controls.Add(labelPositionGrid);
            gbOrientation.Controls.Add(labelPositionWorld);
            gbOrientation.Controls.Add(nudY);
            gbOrientation.Controls.Add(nudX);
            gbOrientation.Location = new System.Drawing.Point(3, 32);
            gbOrientation.Name = "gbOrientation";
            gbOrientation.Size = new System.Drawing.Size(200, 250);
            gbOrientation.TabIndex = 6;
            gbOrientation.TabStop = false;
            gbOrientation.Text = "Orientation";
            // 
            // nudScaleZ
            // 
            nudScaleZ.DecimalPlaces = 1;
            nudScaleZ.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudScaleZ.Location = new System.Drawing.Point(136, 185);
            nudScaleZ.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            nudScaleZ.Name = "nudScaleZ";
            nudScaleZ.Size = new System.Drawing.Size(58, 23);
            nudScaleZ.TabIndex = 13;
            // 
            // nudScaleY
            // 
            nudScaleY.DecimalPlaces = 1;
            nudScaleY.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudScaleY.Location = new System.Drawing.Point(73, 185);
            nudScaleY.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudScaleY.Name = "nudScaleY";
            nudScaleY.Size = new System.Drawing.Size(58, 23);
            nudScaleY.TabIndex = 12;
            // 
            // nudScaleX
            // 
            nudScaleX.DecimalPlaces = 1;
            nudScaleX.Increment = new decimal(new int[] { 5, 0, 0, 0 });
            nudScaleX.Location = new System.Drawing.Point(9, 185);
            nudScaleX.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            nudScaleX.Name = "nudScaleX";
            nudScaleX.Size = new System.Drawing.Size(58, 23);
            nudScaleX.TabIndex = 11;
            // 
            // labelScale
            // 
            labelScale.AutoSize = true;
            labelScale.DisabledColor = System.Drawing.Color.Empty;
            labelScale.IsSeparator = false;
            labelScale.Location = new System.Drawing.Point(6, 167);
            labelScale.Name = "labelScale";
            labelScale.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelScale.Size = new System.Drawing.Size(58, 15);
            labelScale.TabIndex = 27;
            labelScale.Text = "Scale (%):";
            // 
            // nudRotationZ
            // 
            nudRotationZ.DecimalPlaces = 2;
            nudRotationZ.Increment = new decimal(new int[] { 225, 0, 0, 65536 });
            nudRotationZ.Location = new System.Drawing.Point(136, 136);
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
            nudRotationY.Location = new System.Drawing.Point(73, 136);
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
            nudRotationX.Location = new System.Drawing.Point(9, 136);
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
            cbVisibleFrom.Location = new System.Drawing.Point(88, 219);
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
            labelRotation.Location = new System.Drawing.Point(6, 118);
            labelRotation.Name = "labelRotation";
            labelRotation.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelRotation.Size = new System.Drawing.Size(107, 15);
            labelRotation.TabIndex = 7;
            labelRotation.Text = "Rotation (degrees):";
            // 
            // labelVisibleFrom
            // 
            labelVisibleFrom.AutoSize = true;
            labelVisibleFrom.DisabledColor = System.Drawing.Color.Empty;
            labelVisibleFrom.IsSeparator = false;
            labelVisibleFrom.Location = new System.Drawing.Point(9, 222);
            labelVisibleFrom.Name = "labelVisibleFrom";
            labelVisibleFrom.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelVisibleFrom.Size = new System.Drawing.Size(73, 15);
            labelVisibleFrom.TabIndex = 7;
            labelVisibleFrom.Text = "Visible from:";
            // 
            // nudZ
            // 
            nudZ.DecimalPlaces = 2;
            nudZ.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudZ.Location = new System.Drawing.Point(136, 86);
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
            nudZWorld.Location = new System.Drawing.Point(136, 38);
            nudZWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudZWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudZWorld.Name = "nudZWorld";
            nudZWorld.Size = new System.Drawing.Size(58, 23);
            nudZWorld.TabIndex = 2;
            // 
            // nudYWorld
            // 
            nudYWorld.DecimalPlaces = 2;
            nudYWorld.Hexadecimal = true;
            nudYWorld.Location = new System.Drawing.Point(73, 38);
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
            nudXWorld.Location = new System.Drawing.Point(9, 38);
            nudXWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudXWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudXWorld.Name = "nudXWorld";
            nudXWorld.Size = new System.Drawing.Size(58, 23);
            nudXWorld.TabIndex = 0;
            // 
            // labelPositionGrid
            // 
            labelPositionGrid.AutoSize = true;
            labelPositionGrid.DisabledColor = System.Drawing.Color.Empty;
            labelPositionGrid.IsSeparator = false;
            labelPositionGrid.Location = new System.Drawing.Point(6, 68);
            labelPositionGrid.Name = "labelPositionGrid";
            labelPositionGrid.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPositionGrid.Size = new System.Drawing.Size(134, 15);
            labelPositionGrid.TabIndex = 14;
            labelPositionGrid.Text = "Position (grid coords**):";
            // 
            // labelPositionWorld
            // 
            labelPositionWorld.AutoSize = true;
            labelPositionWorld.DisabledColor = System.Drawing.Color.Empty;
            labelPositionWorld.IsSeparator = false;
            labelPositionWorld.Location = new System.Drawing.Point(6, 20);
            labelPositionWorld.Name = "labelPositionWorld";
            labelPositionWorld.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPositionWorld.Size = new System.Drawing.Size(138, 15);
            labelPositionWorld.TabIndex = 12;
            labelPositionWorld.Text = "Position (world coords*):";
            // 
            // nudY
            // 
            nudY.DecimalPlaces = 1;
            nudY.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudY.Location = new System.Drawing.Point(73, 86);
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
            nudX.Location = new System.Drawing.Point(9, 86);
            nudX.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudX.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudX.Name = "nudX";
            nudX.Size = new System.Drawing.Size(58, 23);
            nudX.TabIndex = 3;
            // 
            // labelBottom
            // 
            labelBottom.AutoSize = true;
            labelBottom.DisabledColor = System.Drawing.Color.Empty;
            labelBottom.IsSeparator = false;
            labelBottom.Location = new System.Drawing.Point(96, 125);
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
            nudBottom.Location = new System.Drawing.Point(99, 99);
            nudBottom.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudBottom.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudBottom.Name = "nudBottom";
            nudBottom.Size = new System.Drawing.Size(58, 23);
            nudBottom.TabIndex = 25;
            // 
            // labelTop
            // 
            labelTop.AutoSize = true;
            labelTop.DisabledColor = System.Drawing.Color.Empty;
            labelTop.IsSeparator = false;
            labelTop.Location = new System.Drawing.Point(99, 22);
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
            nudTop.Location = new System.Drawing.Point(102, 40);
            nudTop.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudTop.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudTop.Name = "nudTop";
            nudTop.Size = new System.Drawing.Size(58, 23);
            nudTop.TabIndex = 21;
            // 
            // labelCoordInfo
            // 
            labelCoordInfo.AutoSize = true;
            labelCoordInfo.DisabledColor = System.Drawing.Color.Empty;
            labelCoordInfo.IsSeparator = false;
            labelCoordInfo.Location = new System.Drawing.Point(5, 441);
            labelCoordInfo.Name = "labelCoordInfo";
            labelCoordInfo.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelCoordInfo.Size = new System.Drawing.Size(196, 30);
            labelCoordInfo.TabIndex = 21;
            labelCoordInfo.Text = "* World coordinate signs are flipped\r\n** Grid Y is in heightmap units";
            // 
            // gbBoundingBox
            // 
            gbBoundingBox.Controls.Add(labelWest);
            gbBoundingBox.Controls.Add(labelRight);
            gbBoundingBox.Controls.Add(labelSouth);
            gbBoundingBox.Controls.Add(labelNorth);
            gbBoundingBox.Controls.Add(nudSouth);
            gbBoundingBox.Controls.Add(nudEast);
            gbBoundingBox.Controls.Add(nudWest);
            gbBoundingBox.Controls.Add(nudNorth);
            gbBoundingBox.Controls.Add(nudTop);
            gbBoundingBox.Controls.Add(labelTop);
            gbBoundingBox.Controls.Add(nudBottom);
            gbBoundingBox.Controls.Add(labelBottom);
            gbBoundingBox.Location = new System.Drawing.Point(3, 289);
            gbBoundingBox.Name = "gbBoundingBox";
            gbBoundingBox.Size = new System.Drawing.Size(200, 147);
            gbBoundingBox.TabIndex = 7;
            gbBoundingBox.TabStop = false;
            gbBoundingBox.Text = "Bounding Box Positions (grid**):";
            // 
            // labelWest
            // 
            labelWest.AutoSize = true;
            labelWest.DisabledColor = System.Drawing.Color.Empty;
            labelWest.IsSeparator = false;
            labelWest.Location = new System.Drawing.Point(3, 52);
            labelWest.Name = "labelWest";
            labelWest.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelWest.Size = new System.Drawing.Size(36, 15);
            labelWest.TabIndex = 33;
            labelWest.Text = "West:";
            // 
            // labelRight
            // 
            labelRight.AutoSize = true;
            labelRight.DisabledColor = System.Drawing.Color.Empty;
            labelRight.IsSeparator = false;
            labelRight.Location = new System.Drawing.Point(165, 52);
            labelRight.Name = "labelRight";
            labelRight.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelRight.Size = new System.Drawing.Size(31, 15);
            labelRight.TabIndex = 32;
            labelRight.Text = "East:";
            // 
            // labelSouth
            // 
            labelSouth.AutoSize = true;
            labelSouth.DisabledColor = System.Drawing.Color.Empty;
            labelSouth.IsSeparator = false;
            labelSouth.Location = new System.Drawing.Point(35, 125);
            labelSouth.Name = "labelSouth";
            labelSouth.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelSouth.Size = new System.Drawing.Size(41, 15);
            labelSouth.TabIndex = 31;
            labelSouth.Text = "South:";
            // 
            // labelNorth
            // 
            labelNorth.AutoSize = true;
            labelNorth.DisabledColor = System.Drawing.Color.Empty;
            labelNorth.IsSeparator = false;
            labelNorth.Location = new System.Drawing.Point(36, 22);
            labelNorth.Name = "labelNorth";
            labelNorth.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelNorth.Size = new System.Drawing.Size(41, 15);
            labelNorth.TabIndex = 30;
            labelNorth.Text = "North:";
            // 
            // nudSouth
            // 
            nudSouth.DecimalPlaces = 2;
            nudSouth.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudSouth.Location = new System.Drawing.Point(38, 99);
            nudSouth.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudSouth.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudSouth.Name = "nudSouth";
            nudSouth.Size = new System.Drawing.Size(58, 23);
            nudSouth.TabIndex = 24;
            // 
            // nudEast
            // 
            nudEast.DecimalPlaces = 2;
            nudEast.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudEast.Location = new System.Drawing.Point(136, 70);
            nudEast.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudEast.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudEast.Name = "nudEast";
            nudEast.Size = new System.Drawing.Size(58, 23);
            nudEast.TabIndex = 23;
            // 
            // nudWest
            // 
            nudWest.DecimalPlaces = 2;
            nudWest.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudWest.Location = new System.Drawing.Point(6, 70);
            nudWest.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudWest.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudWest.Name = "nudWest";
            nudWest.Size = new System.Drawing.Size(58, 23);
            nudWest.TabIndex = 22;
            // 
            // nudNorth
            // 
            nudNorth.DecimalPlaces = 2;
            nudNorth.Increment = new decimal(new int[] { 5, 0, 0, 65536 });
            nudNorth.Location = new System.Drawing.Point(39, 40);
            nudNorth.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudNorth.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudNorth.Name = "nudNorth";
            nudNorth.Size = new System.Drawing.Size(58, 23);
            nudNorth.TabIndex = 20;
            // 
            // ModelInstancePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbBoundingBox);
            Controls.Add(gbOrientation);
            Controls.Add(labelModelInstanceEdited);
            Controls.Add(labelCoordInfo);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ModelInstancePropertiesControl";
            Size = new System.Drawing.Size(207, 476);
            gbOrientation.ResumeLayout(false);
            gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudScaleZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudScaleX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudRotationX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudYWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudBottom).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudTop).EndInit();
            gbBoundingBox.ResumeLayout(false);
            gbBoundingBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudSouth).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudEast).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudWest).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudNorth).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelModelInstanceEdited;
        private DarkModeGroupBox gbOrientation;
        private DarkModeNumericUpDown nudYWorld;
        private DarkModeNumericUpDown nudXWorld;
        private DarkModeLabel labelPositionGrid;
        private DarkModeLabel labelPositionWorld;
        private DarkModeNumericUpDown nudY;
        private DarkModeNumericUpDown nudX;
        private DarkModeLabel labelVisibleFrom;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudZWorld;
        private DarkModeLabel labelCoordInfo;
        private DarkModeNumericUpDown nudBottom;
        private DarkModeLabel labelTop;
        private DarkModeNumericUpDown nudTop;
        private DarkModeLabel labelBottom;
        private DarkModeComboBox cbVisibleFrom;
        private DarkModeNumericUpDown nudRotationX;
        private DarkModeLabel labelRotation;
        private DarkModeNumericUpDown nudScaleZ;
        private DarkModeNumericUpDown nudScaleY;
        private DarkModeNumericUpDown nudScaleX;
        private DarkModeLabel labelScale;
        private DarkModeNumericUpDown nudRotationZ;
        private DarkModeNumericUpDown nudRotationY;
        private DarkModeGroupBox gbBoundingBox;
        private DarkModeLabel labelWest;
        private DarkModeLabel labelRight;
        private DarkModeLabel labelSouth;
        private DarkModeLabel labelNorth;
        private DarkModeNumericUpDown nudSouth;
        private DarkModeNumericUpDown nudEast;
        private DarkModeNumericUpDown nudWest;
        private DarkModeNumericUpDown nudNorth;
    }
}
