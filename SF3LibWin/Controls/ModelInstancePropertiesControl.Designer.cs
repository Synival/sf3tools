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
            gbOrientation.Size = new System.Drawing.Size(200, 128);
            gbOrientation.TabIndex = 6;
            gbOrientation.TabStop = false;
            gbOrientation.Text = "Orientation";
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
            darkModeLabel2.Size = new System.Drawing.Size(37, 15);
            darkModeLabel2.TabIndex = 14;
            darkModeLabel2.Text = "(Grid)";
            // 
            // labelPosWorld
            // 
            labelPosWorld.AutoSize = true;
            labelPosWorld.DisabledColor = System.Drawing.Color.Empty;
            labelPosWorld.IsSeparator = false;
            labelPosWorld.Location = new System.Drawing.Point(73, 20);
            labelPosWorld.Name = "labelPosWorld";
            labelPosWorld.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPosWorld.Size = new System.Drawing.Size(47, 15);
            labelPosWorld.TabIndex = 13;
            labelPosWorld.Text = "(World)";
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
            nudY.DecimalPlaces = 2;
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
            Size = new System.Drawing.Size(207, 163);
            gbOrientation.ResumeLayout(false);
            gbOrientation.PerformLayout();
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
        private DarkModeLabel labelDirection;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudZWorld;
        private DarkModeLabel labelX;
        private DarkModeLabel darkModeLabel3;
    }
}
