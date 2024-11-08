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
            this.cbCombineAllAppendedData = new System.Windows.Forms.CheckBox();
            this.cbOpenWhenGenerated = new System.Windows.Forms.CheckBox();
            this.btnGenerateDFR = new System.Windows.Forms.Button();
            this.labelSeparator2 = new System.Windows.Forms.Label();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.tbAlteredFile = new System.Windows.Forms.TextBox();
            this.tbOriginalFile = new System.Windows.Forms.TextBox();
            this.labelOutputFile = new System.Windows.Forms.Label();
            this.labelSelectInputs = new System.Windows.Forms.Label();
            this.labelSeparator1 = new System.Windows.Forms.Label();
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.btnAlteredFile = new System.Windows.Forms.Button();
            this.btnOriginalFile = new System.Windows.Forms.Button();
            this.tabCommand_Apply = new System.Windows.Forms.TabPage();
            this.tabCommand.SuspendLayout();
            this.tabCommand_Create.SuspendLayout();
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
            this.tabCommand.Size = new System.Drawing.Size(699, 195);
            this.tabCommand.TabIndex = 13;
            // 
            // tabCommand_Create
            // 
            this.tabCommand_Create.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.tabCommand_Create.Controls.Add(this.cbCombineAllAppendedData);
            this.tabCommand_Create.Controls.Add(this.cbOpenWhenGenerated);
            this.tabCommand_Create.Controls.Add(this.btnGenerateDFR);
            this.tabCommand_Create.Controls.Add(this.labelSeparator2);
            this.tabCommand_Create.Controls.Add(this.tbOutputFile);
            this.tabCommand_Create.Controls.Add(this.tbAlteredFile);
            this.tabCommand_Create.Controls.Add(this.tbOriginalFile);
            this.tabCommand_Create.Controls.Add(this.labelOutputFile);
            this.tabCommand_Create.Controls.Add(this.labelSelectInputs);
            this.tabCommand_Create.Controls.Add(this.labelSeparator1);
            this.tabCommand_Create.Controls.Add(this.btnOutputFile);
            this.tabCommand_Create.Controls.Add(this.btnAlteredFile);
            this.tabCommand_Create.Controls.Add(this.btnOriginalFile);
            this.tabCommand_Create.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Create.Name = "tabCommand_Create";
            this.tabCommand_Create.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Create.Size = new System.Drawing.Size(691, 169);
            this.tabCommand_Create.TabIndex = 1;
            this.tabCommand_Create.Text = "Create DFR File";
            // 
            // cbCombineAllAppendedData
            // 
            this.cbCombineAllAppendedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCombineAllAppendedData.AutoSize = true;
            this.cbCombineAllAppendedData.Checked = true;
            this.cbCombineAllAppendedData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCombineAllAppendedData.Location = new System.Drawing.Point(201, 142);
            this.cbCombineAllAppendedData.Name = "cbCombineAllAppendedData";
            this.cbCombineAllAppendedData.Size = new System.Drawing.Size(155, 17);
            this.cbCombineAllAppendedData.TabIndex = 23;
            this.cbCombineAllAppendedData.Text = "Combine all appended data";
            this.cbCombineAllAppendedData.UseVisualStyleBackColor = true;
            // 
            // cbOpenWhenGenerated
            // 
            this.cbOpenWhenGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOpenWhenGenerated.AutoSize = true;
            this.cbOpenWhenGenerated.Checked = true;
            this.cbOpenWhenGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOpenWhenGenerated.Location = new System.Drawing.Point(360, 136);
            this.cbOpenWhenGenerated.Name = "cbOpenWhenGenerated";
            this.cbOpenWhenGenerated.Size = new System.Drawing.Size(233, 30);
            this.cbOpenWhenGenerated.TabIndex = 22;
            this.cbOpenWhenGenerated.Text = "Open automatically when generated\r\n(.DFR files can be opened in any text editor)";
            this.cbOpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // btnGenerateDFR
            // 
            this.btnGenerateDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateDFR.Location = new System.Drawing.Point(598, 141);
            this.btnGenerateDFR.Name = "btnGenerateDFR";
            this.btnGenerateDFR.Size = new System.Drawing.Size(87, 20);
            this.btnGenerateDFR.TabIndex = 21;
            this.btnGenerateDFR.Text = "Generate DFR";
            this.btnGenerateDFR.UseVisualStyleBackColor = true;
            // 
            // labelSeparator2
            // 
            this.labelSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator2.Location = new System.Drawing.Point(6, 127);
            this.labelSeparator2.Name = "labelSeparator2";
            this.labelSeparator2.Size = new System.Drawing.Size(679, 2);
            this.labelSeparator2.TabIndex = 20;
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOutputFile.Location = new System.Drawing.Point(99, 99);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(586, 20);
            this.tbOutputFile.TabIndex = 19;
            // 
            // tbAlteredFile
            // 
            this.tbAlteredFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAlteredFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAlteredFile.Location = new System.Drawing.Point(99, 47);
            this.tbAlteredFile.Name = "tbAlteredFile";
            this.tbAlteredFile.Size = new System.Drawing.Size(586, 20);
            this.tbAlteredFile.TabIndex = 18;
            // 
            // tbOriginalFile
            // 
            this.tbOriginalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOriginalFile.Location = new System.Drawing.Point(99, 22);
            this.tbOriginalFile.Name = "tbOriginalFile";
            this.tbOriginalFile.Size = new System.Drawing.Size(586, 20);
            this.tbOriginalFile.TabIndex = 17;
            // 
            // labelOutputFile
            // 
            this.labelOutputFile.AutoSize = true;
            this.labelOutputFile.Location = new System.Drawing.Point(6, 84);
            this.labelOutputFile.Name = "labelOutputFile";
            this.labelOutputFile.Size = new System.Drawing.Size(128, 13);
            this.labelOutputFile.TabIndex = 16;
            this.labelOutputFile.Text = "Select Output Destination";
            // 
            // labelSelectInputs
            // 
            this.labelSelectInputs.AutoSize = true;
            this.labelSelectInputs.Location = new System.Drawing.Point(6, 6);
            this.labelSelectInputs.Name = "labelSelectInputs";
            this.labelSelectInputs.Size = new System.Drawing.Size(88, 13);
            this.labelSelectInputs.TabIndex = 15;
            this.labelSelectInputs.Text = "Select Input Files";
            // 
            // labelSeparator1
            // 
            this.labelSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator1.Location = new System.Drawing.Point(6, 74);
            this.labelSeparator1.Name = "labelSeparator1";
            this.labelSeparator1.Size = new System.Drawing.Size(679, 2);
            this.labelSeparator1.TabIndex = 14;
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.Location = new System.Drawing.Point(6, 99);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(87, 20);
            this.btnOutputFile.TabIndex = 13;
            this.btnOutputFile.Text = "Output File...";
            this.btnOutputFile.UseVisualStyleBackColor = true;
            // 
            // btnAlteredFile
            // 
            this.btnAlteredFile.Location = new System.Drawing.Point(6, 47);
            this.btnAlteredFile.Name = "btnAlteredFile";
            this.btnAlteredFile.Size = new System.Drawing.Size(87, 20);
            this.btnAlteredFile.TabIndex = 12;
            this.btnAlteredFile.Text = "Altered File...";
            this.btnAlteredFile.UseVisualStyleBackColor = true;
            // 
            // btnOriginalFile
            // 
            this.btnOriginalFile.Location = new System.Drawing.Point(6, 21);
            this.btnOriginalFile.Name = "btnOriginalFile";
            this.btnOriginalFile.Size = new System.Drawing.Size(87, 20);
            this.btnOriginalFile.TabIndex = 11;
            this.btnOriginalFile.Text = "Original File...";
            this.btnOriginalFile.UseVisualStyleBackColor = true;
            // 
            // tabCommand_Apply
            // 
            this.tabCommand_Apply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.tabCommand_Apply.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Apply.Name = "tabCommand_Apply";
            this.tabCommand_Apply.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Apply.Size = new System.Drawing.Size(691, 169);
            this.tabCommand_Apply.TabIndex = 0;
            this.tabCommand_Apply.Text = "Apply DFR File";
            // 
            // frmDFRTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(706, 201);
            this.Controls.Add(this.tabCommand);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2400, 240);
            this.MinimumSize = new System.Drawing.Size(520, 240);
            this.Name = "frmDFRTool";
            this.Text = "DFRTool v1.1.1 (Compatible with 25.1 patcher)";
            this.tabCommand.ResumeLayout(false);
            this.tabCommand_Create.ResumeLayout(false);
            this.tabCommand_Create.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TabControl tabCommand;
        private TabPage tabCommand_Apply;
        private TabPage tabCommand_Create;
        private Label labelSeparator2;
        private TextBox tbOutputFile;
        private TextBox tbAlteredFile;
        private TextBox tbOriginalFile;
        private Label labelOutputFile;
        private Label labelSelectInputs;
        private Label labelSeparator1;
        private Button btnOutputFile;
        private Button btnAlteredFile;
        private Button btnOriginalFile;
        private CheckBox cbCombineAllAppendedData;
        private CheckBox cbOpenWhenGenerated;
        private Button btnGenerateDFR;
    }
}