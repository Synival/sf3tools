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
            // ActorNPCPropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelActorEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ActorNPCPropertiesControl";
            Size = new System.Drawing.Size(207, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelActorEdited;
    }
}
