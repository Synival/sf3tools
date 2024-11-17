using System.Drawing;

namespace SF3Editor {
    partial class frmSF3Editor {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            masterEditorControl1 = new SF3.Win.Controls.MasterEditorControl();
            SuspendLayout();
            // 
            // masterEditorControl1
            // 
            masterEditorControl1.Anchor =  System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            masterEditorControl1.BackColor = Color.MistyRose;
            masterEditorControl1.Location = new Point(12, 12);
            masterEditorControl1.Name = "masterEditorControl1";
            masterEditorControl1.Size = new Size(776, 426);
            masterEditorControl1.TabIndex = 0;
            // 
            // frmSF3Editor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(masterEditorControl1);
            Name = "frmSF3Editor";
            Text = "SF3 Editor";
            ResumeLayout(false);
        }

        #endregion

        private SF3.Win.Controls.MasterEditorControl masterEditorControl1;
    }
}
