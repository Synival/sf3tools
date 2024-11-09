using System.Drawing;
using System.Windows.Forms;

namespace DFRTool.GUI.Forms
{
    partial class frmDFRTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDFRTool));
            this.tabCommand = new System.Windows.Forms.TabControl();
            this.tabCommand_Create = new System.Windows.Forms.TabPage();
            this.tabCommand_Apply = new System.Windows.Forms.TabPage();
            this.createDFRControl1 = new DFRTool.GUI.Controls.CreateDFRControl();
            this.applyDFRControl1 = new DFRTool.GUI.Controls.ApplyDFRControl();
            this.tabCommand.SuspendLayout();
            this.tabCommand_Create.SuspendLayout();
            this.tabCommand_Apply.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabCommand
            // 
            this.tabCommand.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabCommand.Controls.Add(this.tabCommand_Create);
            this.tabCommand.Controls.Add(this.tabCommand_Apply);
            this.tabCommand.Location = new System.Drawing.Point(3, 3);
            this.tabCommand.Name = "tabCommand";
            this.tabCommand.SelectedIndex = 0;
            this.tabCommand.Size = new System.Drawing.Size(699, 230);
            this.tabCommand.TabIndex = 13;
            // 
            // tabCommand_Create
            // 
            this.tabCommand_Create.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.tabCommand_Create.Controls.Add(this.createDFRControl1);
            this.tabCommand_Create.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Create.Name = "tabCommand_Create";
            this.tabCommand_Create.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Create.Size = new System.Drawing.Size(691, 204);
            this.tabCommand_Create.TabIndex = 1;
            this.tabCommand_Create.Text = "Create DFR File";
            // 
            // tabCommand_Apply
            // 
            this.tabCommand_Apply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.tabCommand_Apply.Controls.Add(this.applyDFRControl1);
            this.tabCommand_Apply.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Apply.Name = "tabCommand_Apply";
            this.tabCommand_Apply.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Apply.Size = new System.Drawing.Size(691, 204);
            this.tabCommand_Apply.TabIndex = 0;
            this.tabCommand_Apply.Text = "Apply DFR File";
            // 
            // createDFRControl1
            // 
            this.createDFRControl1.AlteredData = null;
            this.createDFRControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.createDFRControl1.BackColor = System.Drawing.Color.Transparent;
            this.createDFRControl1.Location = new System.Drawing.Point(3, 6);
            this.createDFRControl1.Name = "createDFRControl1";
            this.createDFRControl1.Size = new System.Drawing.Size(685, 195);
            this.createDFRControl1.TabIndex = 0;
            // 
            // applyDFRControl1
            // 
            this.applyDFRControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyDFRControl1.BackColor = System.Drawing.Color.Transparent;
            this.applyDFRControl1.Location = new System.Drawing.Point(3, 6);
            this.applyDFRControl1.Name = "applyDFRControl1";
            this.applyDFRControl1.Size = new System.Drawing.Size(685, 195);
            this.applyDFRControl1.TabIndex = 0;
            // 
            // frmDFRTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(706, 236);
            this.Controls.Add(this.tabCommand);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2400, 275);
            this.MinimumSize = new System.Drawing.Size(520, 275);
            this.Name = "frmDFRTool";
            this.Text = "DFRTool v1.1 (Compatible with 25.1 patcher)";
            this.tabCommand.ResumeLayout(false);
            this.tabCommand_Create.ResumeLayout(false);
            this.tabCommand_Apply.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TabControl tabCommand;
        private TabPage tabCommand_Apply;
        private TabPage tabCommand_Create;
        private Controls.CreateDFRControl createDFRControl1;
        private Controls.ApplyDFRControl applyDFRControl1;
    }
}