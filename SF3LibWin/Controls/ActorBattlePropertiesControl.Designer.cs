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
            gbPosition = new DarkModeGroupBox();
            labelPosition = new DarkModeLabel();
            nudZ = new DarkModeNumericUpDown();
            nudX = new DarkModeNumericUpDown();
            cbDirection = new DarkModeComboBox();
            labelDirection = new DarkModeLabel();
            gbPosition.SuspendLayout();
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
            // gbPosition
            // 
            gbPosition.Controls.Add(labelPosition);
            gbPosition.Controls.Add(nudZ);
            gbPosition.Controls.Add(nudX);
            gbPosition.Controls.Add(cbDirection);
            gbPosition.Controls.Add(labelDirection);
            gbPosition.Location = new System.Drawing.Point(3, 32);
            gbPosition.Name = "gbPosition";
            gbPosition.Size = new System.Drawing.Size(200, 83);
            gbPosition.TabIndex = 4;
            gbPosition.TabStop = false;
            gbPosition.Text = "Movement";
            // 
            // labelPosition
            // 
            labelPosition.AutoSize = true;
            labelPosition.DisabledColor = System.Drawing.Color.Empty;
            labelPosition.IsSeparator = false;
            labelPosition.Location = new System.Drawing.Point(6, 25);
            labelPosition.Name = "labelPosition";
            labelPosition.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelPosition.Size = new System.Drawing.Size(53, 15);
            labelPosition.TabIndex = 5;
            labelPosition.Text = "Position:";
            // 
            // nudZ
            // 
            nudZ.Location = new System.Drawing.Point(136, 23);
            nudZ.Maximum = new decimal(new int[] { 63, 0, 0, 0 });
            nudZ.Name = "nudZ";
            nudZ.Size = new System.Drawing.Size(58, 23);
            nudZ.TabIndex = 4;
            // 
            // nudX
            // 
            nudX.Location = new System.Drawing.Point(73, 23);
            nudX.Maximum = new decimal(new int[] { 63, 0, 0, 0 });
            nudX.Name = "nudX";
            nudX.Size = new System.Drawing.Size(58, 23);
            nudX.TabIndex = 3;
            // 
            // cbDirection
            // 
            cbDirection.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            cbDirection.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cbDirection.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            cbDirection.FormattingEnabled = true;
            cbDirection.Location = new System.Drawing.Point(73, 51);
            cbDirection.Name = "cbDirection";
            cbDirection.Size = new System.Drawing.Size(121, 24);
            cbDirection.TabIndex = 0;
            cbDirection.Text = "North";
            // 
            // labelDirection
            // 
            labelDirection.AutoSize = true;
            labelDirection.DisabledColor = System.Drawing.Color.Empty;
            labelDirection.IsSeparator = false;
            labelDirection.Location = new System.Drawing.Point(6, 54);
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
            Controls.Add(gbPosition);
            Controls.Add(labelActorEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ActorBattlePropertiesControl";
            Size = new System.Drawing.Size(207, 119);
            gbPosition.ResumeLayout(false);
            gbPosition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize) nudZ).EndInit();
            ((System.ComponentModel.ISupportInitialize) nudX).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelActorEdited;
        private DarkModeGroupBox gbPosition;
        private DarkModeNumericUpDown nudZ;
        private DarkModeNumericUpDown nudX;
        private DarkModeComboBox cbDirection;
        private DarkModeLabel labelDirection;
        private DarkModeLabel labelPosition;
    }
}
