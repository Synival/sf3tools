namespace SF3.Win.Controls {
    partial class ImagePanel {
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
            imageControl = new ImageControl();
            btnExport = new System.Windows.Forms.Button();
            btnImport = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // imageControl
            // 
            imageControl.BackColor = System.Drawing.Color.Transparent;
            imageControl.Image = null;
            imageControl.ImageScale = 4F;
            imageControl.Location = new System.Drawing.Point(4, 26);
            imageControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            imageControl.Name = "imageControl";
            imageControl.Size = new System.Drawing.Size(64, 64);
            imageControl.TabIndex = 0;
            // 
            // btnExport
            // 
            btnExport.Location = new System.Drawing.Point(4, 0);
            btnExport.Name = "btnExport";
            btnExport.Size = new System.Drawing.Size(64, 23);
            btnExport.TabIndex = 1;
            btnExport.Text = "Export...";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            btnImport.Location = new System.Drawing.Point(74, 0);
            btnImport.Name = "btnImport";
            btnImport.Size = new System.Drawing.Size(64, 23);
            btnImport.TabIndex = 2;
            btnImport.Text = "Import...";
            btnImport.UseVisualStyleBackColor = true;
            // 
            // ImagePanel
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(btnImport);
            Controls.Add(btnExport);
            Controls.Add(imageControl);
            MinimumSize = new System.Drawing.Size(138, 90);
            Name = "ImagePanel";
            Size = new System.Drawing.Size(138, 90);
            ResumeLayout(false);
        }

        #endregion

        private ImageControl imageControl;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
    }
}
