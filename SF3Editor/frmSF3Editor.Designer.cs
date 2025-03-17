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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSF3Editor));
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            tsmiFile = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Open = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_OpenScenario = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Detect = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiScenario_Scenario1 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Scenario2 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_Scenario3 = new System.Windows.Forms.ToolStripMenuItem();
            tsmiScenario_PremiumDisk = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Save = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_SaveAs = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep1 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Close = new System.Windows.Forms.ToolStripMenuItem();
            tsmiFile_Sep2 = new System.Windows.Forms.ToolStripSeparator();
            tsmiFile_Exit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            tsmiHelp_About = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit = new System.Windows.Forms.ToolStripMenuItem();
            tsmiEdit_UseDropdowns = new System.Windows.Forms.ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile, tsmiEdit, tsmiHelp });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(891, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // tsmiFile
            // 
            tsmiFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiFile_Open, tsmiFile_OpenScenario, tsmiFile_Save, tsmiFile_SaveAs, tsmiFile_Sep1, tsmiFile_Close, tsmiFile_Sep2, tsmiFile_Exit });
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
            tsmiFile_Open.Click += tsmiFile_Open_Click;
            // 
            // tsmiFile_OpenScenario
            // 
            tsmiFile_OpenScenario.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiScenario_Detect, tsmiScenario_Sep1, tsmiScenario_Scenario1, tsmiScenario_Scenario2, tsmiScenario_Scenario3, tsmiScenario_PremiumDisk });
            tsmiFile_OpenScenario.Name = "tsmiFile_OpenScenario";
            tsmiFile_OpenScenario.Size = new Size(195, 22);
            tsmiFile_OpenScenario.Text = "Open S&cenario";
            // 
            // tsmiScenario_Detect
            // 
            tsmiScenario_Detect.Name = "tsmiScenario_Detect";
            tsmiScenario_Detect.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D0;
            tsmiScenario_Detect.Size = new Size(249, 22);
            tsmiScenario_Detect.Text = "&Detect or Based on Folder";
            tsmiScenario_Detect.Click += tsmiScenario_Detect_Click;
            // 
            // tsmiScenario_Sep1
            // 
            tsmiScenario_Sep1.Name = "tsmiScenario_Sep1";
            tsmiScenario_Sep1.Size = new Size(246, 6);
            // 
            // tsmiScenario_Scenario1
            // 
            tsmiScenario_Scenario1.Name = "tsmiScenario_Scenario1";
            tsmiScenario_Scenario1.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1;
            tsmiScenario_Scenario1.Size = new Size(249, 22);
            tsmiScenario_Scenario1.Text = "Scenario &1";
            tsmiScenario_Scenario1.Click += tsmiScenario_Scenario1_Click;
            // 
            // tsmiScenario_Scenario2
            // 
            tsmiScenario_Scenario2.Name = "tsmiScenario_Scenario2";
            tsmiScenario_Scenario2.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2;
            tsmiScenario_Scenario2.Size = new Size(249, 22);
            tsmiScenario_Scenario2.Text = "Scenario &2";
            tsmiScenario_Scenario2.Click += tsmiScenario_Scenario2_Click;
            // 
            // tsmiScenario_Scenario3
            // 
            tsmiScenario_Scenario3.Name = "tsmiScenario_Scenario3";
            tsmiScenario_Scenario3.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3;
            tsmiScenario_Scenario3.Size = new Size(249, 22);
            tsmiScenario_Scenario3.Text = "Scenario &3";
            tsmiScenario_Scenario3.Click += tsmiScenario_Scenario3_Click;
            // 
            // tsmiScenario_PremiumDisk
            // 
            tsmiScenario_PremiumDisk.Name = "tsmiScenario_PremiumDisk";
            tsmiScenario_PremiumDisk.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4;
            tsmiScenario_PremiumDisk.Size = new Size(249, 22);
            tsmiScenario_PremiumDisk.Text = "&Premium Disk";
            tsmiScenario_PremiumDisk.Click += tsmiScenario_PremiumDisk_Click;
            // 
            // tsmiFile_Save
            // 
            tsmiFile_Save.Enabled = false;
            tsmiFile_Save.Name = "tsmiFile_Save";
            tsmiFile_Save.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
            tsmiFile_Save.Size = new Size(195, 22);
            tsmiFile_Save.Text = "&Save";
            tsmiFile_Save.Click += tsmiFile_Save_Click;
            // 
            // tsmiFile_SaveAs
            // 
            tsmiFile_SaveAs.Enabled = false;
            tsmiFile_SaveAs.Name = "tsmiFile_SaveAs";
            tsmiFile_SaveAs.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.S;
            tsmiFile_SaveAs.Size = new Size(195, 22);
            tsmiFile_SaveAs.Text = "Save &As...";
            tsmiFile_SaveAs.Click += tsmiFile_SaveAs_Click;
            // 
            // tsmiFile_Sep1
            // 
            tsmiFile_Sep1.Name = "tsmiFile_Sep1";
            tsmiFile_Sep1.Size = new Size(192, 6);
            // 
            // tsmiFile_Close
            // 
            tsmiFile_Close.Enabled = false;
            tsmiFile_Close.Name = "tsmiFile_Close";
            tsmiFile_Close.ShortcutKeys =  System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W;
            tsmiFile_Close.Size = new Size(195, 22);
            tsmiFile_Close.Text = "&Close";
            tsmiFile_Close.Click += tsmiFile_Close_Click;
            // 
            // tsmiFile_Sep2
            // 
            tsmiFile_Sep2.Name = "tsmiFile_Sep2";
            tsmiFile_Sep2.Size = new Size(192, 6);
            // 
            // tsmiFile_Exit
            // 
            tsmiFile_Exit.Name = "tsmiFile_Exit";
            tsmiFile_Exit.ShortcutKeys =  System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4;
            tsmiFile_Exit.Size = new Size(195, 22);
            tsmiFile_Exit.Text = "E&xit";
            tsmiFile_Exit.Click += tsmiFile_Exit_Click;
            // 
            // tsmiHelp
            // 
            tsmiHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiHelp_About });
            tsmiHelp.Name = "tsmiHelp";
            tsmiHelp.Size = new Size(44, 20);
            tsmiHelp.Text = "&Help";
            // 
            // tsmiHelp_About
            // 
            tsmiHelp_About.Name = "tsmiHelp_About";
            tsmiHelp_About.Size = new Size(180, 22);
            tsmiHelp_About.Text = "&About...";
            tsmiHelp_About.Click += tsmiHelp_About_Click;
            // 
            // tsmiEdit
            // 
            tsmiEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiEdit_UseDropdowns });
            tsmiEdit.Name = "tsmiEdit";
            tsmiEdit.Size = new Size(39, 20);
            tsmiEdit.Text = "&Edit";
            // 
            // tsmiEdit_UseDropdowns
            // 
            tsmiEdit_UseDropdowns.Name = "tsmiEdit_UseDropdowns";
            tsmiEdit_UseDropdowns.Size = new Size(253, 22);
            tsmiEdit_UseDropdowns.Text = "Use &Dropdowns for Named Values";
            tsmiEdit_UseDropdowns.Click += tsmiEdit_UseDropdowns_Click;
            // 
            // frmSF3Editor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new Size(891, 631);
            Controls.Add(menuStrip1);
            Icon = (Icon) resources.GetObject("$this.Icon");
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
        private System.Windows.Forms.ToolStripMenuItem tsmiFile_OpenScenario;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Detect;
        private System.Windows.Forms.ToolStripSeparator tsmiScenario_Sep1;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario1;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario2;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_Scenario3;
        private System.Windows.Forms.ToolStripMenuItem tsmiScenario_PremiumDisk;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp_About;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit;
        private System.Windows.Forms.ToolStripMenuItem tsmiEdit_UseDropdowns;
    }
}
