using System.Drawing;
using System.Windows.Forms;

namespace DFRLib.Win.Forms
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
        private void InitializeComponent() {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDFRTool));
            tabCommand = new TabControl();
            tabCommand_Apply = new TabPage();
            applyDFRControl1 = new Controls.ApplyDFRControl();
            tabCommand_Create = new TabPage();
            createDFRControl1 = new Controls.CreateDFRControl();
            tabCommand.SuspendLayout();
            tabCommand_Apply.SuspendLayout();
            tabCommand_Create.SuspendLayout();
            SuspendLayout();
            // 
            // tabCommand
            // 
            tabCommand.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabCommand.Controls.Add(tabCommand_Apply);
            tabCommand.Controls.Add(tabCommand_Create);
            tabCommand.Location = new Point(4, 3);
            tabCommand.Margin = new Padding(4, 3, 4, 3);
            tabCommand.Name = "tabCommand";
            tabCommand.SelectedIndex = 0;
            tabCommand.Size = new Size(580, 249);
            tabCommand.TabIndex = 13;
            // 
            // tabCommand_Apply
            // 
            tabCommand_Apply.BackColor = Color.FromArgb(  249,   249,   249);
            tabCommand_Apply.Controls.Add(applyDFRControl1);
            tabCommand_Apply.Location = new Point(4, 24);
            tabCommand_Apply.Margin = new Padding(4, 3, 4, 3);
            tabCommand_Apply.Name = "tabCommand_Apply";
            tabCommand_Apply.Padding = new Padding(4, 3, 4, 3);
            tabCommand_Apply.Size = new Size(572, 221);
            tabCommand_Apply.TabIndex = 0;
            tabCommand_Apply.Text = "Apply DFR";
            // 
            // applyDFRControl1
            // 
            applyDFRControl1.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            applyDFRControl1.ApplyInMemory = false;
            applyDFRControl1.BackColor = Color.Transparent;
            applyDFRControl1.InputData = null;
            applyDFRControl1.Location = new Point(0, 3);
            applyDFRControl1.Margin = new Padding(4, 3, 4, 3);
            applyDFRControl1.Name = "applyDFRControl1";
            applyDFRControl1.Size = new Size(572, 218);
            applyDFRControl1.TabIndex = 0;
            // 
            // tabCommand_Create
            // 
            tabCommand_Create.BackColor = Color.FromArgb(  249,   249,   249);
            tabCommand_Create.Controls.Add(createDFRControl1);
            tabCommand_Create.Location = new Point(4, 24);
            tabCommand_Create.Margin = new Padding(4, 3, 4, 3);
            tabCommand_Create.Name = "tabCommand_Create";
            tabCommand_Create.Padding = new Padding(4, 3, 4, 3);
            tabCommand_Create.Size = new Size(808, 221);
            tabCommand_Create.TabIndex = 1;
            tabCommand_Create.Text = "Create DFR";
            // 
            // createDFRControl1
            // 
            createDFRControl1.AlteredData = null;
            createDFRControl1.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            createDFRControl1.BackColor = Color.Transparent;
            createDFRControl1.Location = new Point(0, 3);
            createDFRControl1.Margin = new Padding(4, 3, 4, 3);
            createDFRControl1.Name = "createDFRControl1";
            createDFRControl1.Size = new Size(808, 218);
            createDFRControl1.TabIndex = 0;
            // 
            // frmDFRTool
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(588, 256);
            Controls.Add(tabCommand);
            Icon = (Icon) resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MaximumSize = new Size(2797, 295);
            MinimumSize = new Size(604, 295);
            Name = "frmDFRTool";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DFRTool v1.1.1 (Compatible with 25.1 patcher)";
            tabCommand.ResumeLayout(false);
            tabCommand_Apply.ResumeLayout(false);
            tabCommand_Create.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TabControl tabCommand;
        private TabPage tabCommand_Apply;
        private TabPage tabCommand_Create;
        private Controls.ApplyDFRControl applyDFRControl1;
        private Controls.CreateDFRControl createDFRControl1;
    }
}