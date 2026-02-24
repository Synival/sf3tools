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
            labelTileEdited = new DarkModeLabel();
            SuspendLayout();
            // 
            // labelTileEdited
            // 
            labelTileEdited.AutoSize = true;
            labelTileEdited.DisabledColor = System.Drawing.Color.Empty;
            labelTileEdited.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point,  0);
            labelTileEdited.IsSeparator = false;
            labelTileEdited.Location = new System.Drawing.Point(5, 5);
            labelTileEdited.Name = "labelTileEdited";
            labelTileEdited.OriginalBorderStyle = System.Windows.Forms.BorderStyle.None;
            labelTileEdited.Size = new System.Drawing.Size(92, 15);
            labelTileEdited.TabIndex = 3;
            labelTileEdited.Text = "Model Instance";
            // 
            // ModelInstancePropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(labelTileEdited);
            MaximumSize = new System.Drawing.Size(207, 10000);
            MinimumSize = new System.Drawing.Size(207, 50);
            Name = "ModelInstancePropertiesControl";
            Size = new System.Drawing.Size(207, 50);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private DarkModeLabel labelTileEdited;
    }
}
