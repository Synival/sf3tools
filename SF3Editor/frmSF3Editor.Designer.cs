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
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_Open, tsmiFile_Save, tsmiFile_SaveAs, tsmiFile_Sep1, tsmiFile_Close, tsmiFile_Sep2, tsmiFile_Exit });
            tsmiFile.Name = "tsmiFile";
            tsmiFile.Size = new Size(37, 20);
            tsmiFile.Text = "&File";
            // 
            // tsmiFile_Open
            // 
            tsmiFile_Open.Name = "tsmiFile_Open";
            tsmiFile_Open.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
            tsmiFile_Open.Size = new Size(195, 22);
            tsmiFile_Open.Text = "&Open...";
            // 
            // tsmiFile_Save
            // 
            tsmiFile_Save.Enabled = false;
            tsmiFile_Save.Name = "tsmiFile_Save";
            tsmiFile_Save.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            tsmiFile_Save.Size = new Size(195, 22);
            tsmiFile_Save.Text = "&Save";
            // 
            // tsmiFile_SaveAs
            // 
            tsmiFile_SaveAs.Enabled = false;
            tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            tsmiFile_SaveAs.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            tsmiFile_SaveAs.Size = new Size(195, 22);
            tsmiFile_SaveAs.Text = "Save &As...";
            // 
            // tsmiFile_Close
            // 
            tsmiFile_Close.Enabled = false;
            tsmiFile_Close.Name = "tsmiFile_Close";
            tsmiFile_Close.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            tsmiFile_Close.Size = new Size(195, 22);
            tsmiFile_Close.Text = "&Close";
            // 
            // tsmiFile_Sep1
            // 
            tsmiFile_Sep1.Name = "tsmiFile_Sep1";
            tsmiFile_Sep1.Size = new Size(192, 6);
            // 
            // tsmiFile_Exit
            // 
            tsmiFile_Exit.Name = "tsmiFile_Exit";
            tsmiFile_Exit.ShortcutKeys =  System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            tsmiFile_Exit.Size = new Size(195, 22);
            tsmiFile_Exit.Text = "E&xit";
            // 
            // tsmiFile_Sep2
            // 
            tsmiFile_Sep2.Name = "tsmiFile_Sep2";
            tsmiFile_Sep2.Size = new Size(192, 6);
            // 
            // frmSF3Editor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "frmSF3Editor";
            Text = "SF3 Editor";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Open;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Save;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_SaveAs;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Close;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_Exit;
        private System.Windows.Forms.ToolStripSeparator tsmiFile_Sep2;
    }
}
