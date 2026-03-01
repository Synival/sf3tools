using CommonLib.Win.Controls;

namespace SF3.Win.Controls {
    partial class ActorNPCPropertiesControl {
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
            labelActorEdited = new DarkModeLabel();
            gbOrientation = new DarkModeGroupBox();
            nudZWorld = new DarkModeNumericUpDown();
            nudXWorld = new DarkModeNumericUpDown();
            darkModeLabel2 = new DarkModeLabel();
            labelPosWorld = new DarkModeLabel();
            labelPosition = new DarkModeLabel();
            nudZ = new DarkModeNumericUpDown();
            nudX = new DarkModeNumericUpDown();
            darkModeLabel1 = new DarkModeLabel();
            nudDirection = new DarkModeNumericUpDown();
            labelDirection = new DarkModeLabel();
            gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudX).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudDirection).BeginInit();
            SuspendLayout();
            // 
            // labelActorEdited
            // 
            labelActorEdited.AutoSize = true;
            labelActorEdited.DisabledColor = System.Drawing.Color.Empty;
            labelActorEdited.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelActorEdited.IsSeparator = false;
            labelActorEdited.Location = new System.Drawing.Point(5, 5);
            labelActorEdited.Name = "labelActorEdited";
            labelActorEdited.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelActorEdited.Size = new System.Drawing.Size(114, 15);
            labelActorEdited.TabIndex = 3;
            labelActorEdited.Text = "Actor (NPC): (none)";
            // 
            // gbOrientation
            // 
            gbOrientation.Controls.Add(nudZWorld);
            gbOrientation.Controls.Add(nudXWorld);
            gbOrientation.Controls.Add(darkModeLabel2);
            gbOrientation.Controls.Add(labelPosWorld);
            gbOrientation.Controls.Add(labelPosition);
            gbOrientation.Controls.Add(nudZ);
            gbOrientation.Controls.Add(nudX);
            gbOrientation.Controls.Add(darkModeLabel1);
            gbOrientation.Controls.Add(nudDirection);
            gbOrientation.Controls.Add(labelDirection);
            gbOrientation.Location = new System.Drawing.Point(3, 32);
            gbOrientation.Name = "gbOrientation";
            gbOrientation.Size = new System.Drawing.Size(200, 147);
            gbOrientation.TabIndex = 5;
            gbOrientation.TabStop = false;
            gbOrientation.Text = "Orientation";
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
            nudZWorld.TabIndex = 1;
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
            darkModeLabel2.Location = new System.Drawing.Point(28, 69);
            darkModeLabel2.Name = "darkModeLabel2";
            darkModeLabel2.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel2.Size = new System.Drawing.Size(40, 15);
            darkModeLabel2.TabIndex = 14;
            darkModeLabel2.Text = "(Grid:)";
            // 
            // labelPosWorld
            // 
            labelPosWorld.AutoSize = true;
            labelPosWorld.DisabledColor = System.Drawing.Color.Empty;
            labelPosWorld.IsSeparator = false;
            labelPosWorld.Location = new System.Drawing.Point(18, 40);
            labelPosWorld.Name = "labelPosWorld";
            labelPosWorld.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPosWorld.Size = new System.Drawing.Size(50, 15);
            labelPosWorld.TabIndex = 13;
            labelPosWorld.Text = "(World:)";
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
            // nudZ
            // 
            nudZ.DecimalPlaces = 2;
            nudZ.Location = new System.Drawing.Point(136, 67);
            nudZ.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudZ.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudZ.Name = "nudZ";
            nudZ.Size = new System.Drawing.Size(58, 23);
            nudZ.TabIndex = 3;
            // 
            // nudX
            // 
            nudX.DecimalPlaces = 2;
            nudX.Location = new System.Drawing.Point(73, 67);
            nudX.Maximum = new decimal(new int[] { 1024, 0, 0, 0 });
            nudX.Minimum = new decimal(new int[] { 1024, 0, 0, int.MinValue });
            nudX.Name = "nudX";
            nudX.Size = new System.Drawing.Size(58, 23);
            nudX.TabIndex = 2;
            // 
            // darkModeLabel1
            // 
            darkModeLabel1.AutoSize = true;
            darkModeLabel1.DisabledColor = System.Drawing.Color.Empty;
            darkModeLabel1.IsSeparator = false;
            darkModeLabel1.Location = new System.Drawing.Point(27, 126);
            darkModeLabel1.Name = "darkModeLabel1";
            darkModeLabel1.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel1.Size = new System.Drawing.Size(145, 15);
            darkModeLabel1.TabIndex = 7;
            darkModeLabel1.Text = "(NESW = 0, 90, (-)180, -90)";
            // 
            // nudDirection
            // 
            nudDirection.DecimalPlaces = 2;
            nudDirection.Increment = new decimal(new int[] { 225, 0, 0, 65536 });
            nudDirection.Location = new System.Drawing.Point(136, 100);
            nudDirection.Maximum = new decimal(new int[] { 18000, 0, 0, 0 });
            nudDirection.Minimum = new decimal(new int[] { 18000, 0, 0, int.MinValue });
            nudDirection.Name = "nudDirection";
            nudDirection.Size = new System.Drawing.Size(58, 23);
            nudDirection.TabIndex = 4;
            // 
            // labelDirection
            // 
            labelDirection.AutoSize = true;
            labelDirection.DisabledColor = System.Drawing.Color.Empty;
            labelDirection.IsSeparator = false;
            labelDirection.Location = new System.Drawing.Point(6, 103);
            labelDirection.Name = "labelDirection";
            labelDirection.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelDirection.Size = new System.Drawing.Size(123, 15);
            labelDirection.TabIndex = 3;
            labelDirection.Text = "Direction (in degrees):";
            // 
            // ActorNPCPropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbOrientation);
            Controls.Add(labelActorEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ActorNPCPropertiesControl";
            Size = new System.Drawing.Size(207, 183);
            gbOrientation.ResumeLayout(false);
            gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudX).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudDirection).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelActorEdited;
        private DarkModeGroupBox gbOrientation;
        private DarkModeLabel labelDirection;
        private DarkModeNumericUpDown nudDirection;
        private DarkModeLabel darkModeLabel1;
        private DarkModeNumericUpDown nudZWorld;
        private DarkModeNumericUpDown nudXWorld;
        private DarkModeLabel darkModeLabel2;
        private DarkModeLabel labelPosWorld;
        private DarkModeLabel labelPosition;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudX;
    }
}
