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
            // ModelInstancePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelModelInstanceEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ModelInstancePropertiesControl";
            Size = new System.Drawing.Size(207, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelModelInstanceEdited;
    }
}
