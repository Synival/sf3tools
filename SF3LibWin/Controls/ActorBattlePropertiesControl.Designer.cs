using CommonLib.Win.Controls;

namespace SF3.Win.Controls {
    partial class ActorBattlePropertiesControl {
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
            darkModeLabel1 = new DarkModeLabel();
            labelPosWorld = new DarkModeLabel();
            labelPosition = new DarkModeLabel();
            nudZ = new DarkModeNumericUpDown();
            nudX = new DarkModeNumericUpDown();
            cbDirection = new DarkModeComboBox();
            labelDirection = new DarkModeLabel();
            gbOrientation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).BeginInit();
            ((System.ComponentModel.ISupportInitialize) nudX).BeginInit();
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
            labelActorEdited.Size = new System.Drawing.Size(125, 15);
            labelActorEdited.TabIndex = 3;
            labelActorEdited.Text = "Actor (Battle): (none)";
            // 
            // gbOrientation
            // 
            gbOrientation.Controls.Add(nudZWorld);
            gbOrientation.Controls.Add(nudXWorld);
            gbOrientation.Controls.Add(darkModeLabel1);
            gbOrientation.Controls.Add(labelPosWorld);
            gbOrientation.Controls.Add(labelPosition);
            gbOrientation.Controls.Add(nudZ);
            gbOrientation.Controls.Add(nudX);
            gbOrientation.Controls.Add(cbDirection);
            gbOrientation.Controls.Add(labelDirection);
            gbOrientation.Location = new System.Drawing.Point(3, 32);
            gbOrientation.Name = "gbOrientation";
            gbOrientation.Size = new System.Drawing.Size(200, 133);
            gbOrientation.TabIndex = 4;
            gbOrientation.TabStop = false;
            gbOrientation.Text = "Orientation";
            // 
            // nudZWorld
            // 
            nudZWorld.Enabled = false;
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
            nudXWorld.Enabled = false;
            nudXWorld.Hexadecimal = true;
            nudXWorld.Location = new System.Drawing.Point(73, 38);
            nudXWorld.Maximum = new decimal(new int[] { 32768, 0, 0, 0 });
            nudXWorld.Minimum = new decimal(new int[] { 32768, 0, 0, int.MinValue });
            nudXWorld.Name = "nudXWorld";
            nudXWorld.Size = new System.Drawing.Size(58, 23);
            nudXWorld.TabIndex = 0;
            // 
            // darkModeLabel1
            // 
            darkModeLabel1.AutoSize = true;
            darkModeLabel1.DisabledColor = System.Drawing.Color.Empty;
            darkModeLabel1.IsSeparator = false;
            darkModeLabel1.Location = new System.Drawing.Point(28, 69);
            darkModeLabel1.Name = "darkModeLabel1";
            darkModeLabel1.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            darkModeLabel1.Size = new System.Drawing.Size(40, 15);
            darkModeLabel1.TabIndex = 7;
            darkModeLabel1.Text = "(Grid:)";
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
            labelPosWorld.TabIndex = 6;
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
            labelPosition.TabIndex = 5;
            labelPosition.Text = "Position:";
            // 
            // nudZ
            // 
            nudZ.Location = new System.Drawing.Point(136, 67);
            nudZ.Maximum = new decimal(new int[] { 63, 0, 0, 0 });
            nudZ.Name = "nudZ";
            nudZ.Size = new System.Drawing.Size(58, 23);
            nudZ.TabIndex = 3;
            // 
            // nudX
            // 
            nudX.Location = new System.Drawing.Point(73, 67);
            nudX.Maximum = new decimal(new int[] { 63, 0, 0, 0 });
            nudX.Name = "nudX";
            nudX.Size = new System.Drawing.Size(58, 23);
            nudX.TabIndex = 2;
            // 
            // cbDirection
            // 
            cbDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cbDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbDirection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cbDirection.FormattingEnabled = true;
            cbDirection.Location = new System.Drawing.Point(73, 100);
            cbDirection.Name = "cbDirection";
            cbDirection.Size = new System.Drawing.Size(121, 24);
            cbDirection.TabIndex = 4;
            cbDirection.Text = "North";
            // 
            // labelDirection
            // 
            labelDirection.AutoSize = true;
            labelDirection.DisabledColor = System.Drawing.Color.Empty;
            labelDirection.IsSeparator = false;
            labelDirection.Location = new System.Drawing.Point(6, 103);
            labelDirection.Name = "labelDirection";
            labelDirection.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelDirection.Size = new System.Drawing.Size(58, 15);
            labelDirection.TabIndex = 3;
            labelDirection.Text = "Direction:";
            // 
            // ActorBattlePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(gbOrientation);
            Controls.Add(labelActorEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ActorBattlePropertiesControl";
            Size = new System.Drawing.Size(207, 169);
            gbOrientation.ResumeLayout(false);
            gbOrientation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudZWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudXWorld).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudX).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelActorEdited;
        private DarkModeGroupBox gbOrientation;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudX;
        private DarkModeComboBox cbDirection;
        private DarkModeLabel labelDirection;
        private DarkModeLabel labelPosition;
        private DarkModeLabel darkModeLabel1;
        private DarkModeLabel labelPosWorld;
        private DarkModeNumericUpDown nudZWorld;
        private DarkModeNumericUpDown nudXWorld;
    }
}
