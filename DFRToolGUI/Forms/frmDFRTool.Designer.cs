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
            this.cbCreate_CombineAllAppendedData = new System.Windows.Forms.CheckBox();
            this.cbCreate_OpenWhenGenerated = new System.Windows.Forms.CheckBox();
            this.btnCreate_GenerateDFR = new System.Windows.Forms.Button();
            this.labelCreate_Separator2 = new System.Windows.Forms.Label();
            this.tbCreate_OutputFile = new System.Windows.Forms.TextBox();
            this.tbCreate_AlteredFile = new System.Windows.Forms.TextBox();
            this.tbCreate_OriginalFile = new System.Windows.Forms.TextBox();
            this.labelCreate_OutputFile = new System.Windows.Forms.Label();
            this.labelCreate_SelectInputs = new System.Windows.Forms.Label();
            this.labelCreate_Separator1 = new System.Windows.Forms.Label();
            this.btnCreate_OutputFile = new System.Windows.Forms.Button();
            this.btnCreate_AlteredFile = new System.Windows.Forms.Button();
            this.btnCreate_OriginalFile = new System.Windows.Forms.Button();
            this.tabCommand_Apply = new System.Windows.Forms.TabPage();
            this.cbApply_ApplyToOriginalFile = new System.Windows.Forms.CheckBox();
            this.btnApply_ApplyDFR = new System.Windows.Forms.Button();
            this.labelApply_Separator2 = new System.Windows.Forms.Label();
            this.tbApply_OutputFile = new System.Windows.Forms.TextBox();
            this.tbApply_DFRFile = new System.Windows.Forms.TextBox();
            this.labelApply_SelectOutputDestination = new System.Windows.Forms.Label();
            this.labelApply_Separator1 = new System.Windows.Forms.Label();
            this.btnApply_OutputFile = new System.Windows.Forms.Button();
            this.btnApply_DFRFile = new System.Windows.Forms.Button();
            this.tbApply_OriginalFile = new System.Windows.Forms.TextBox();
            this.btnApply_OriginalFile = new System.Windows.Forms.Button();
            this.labelApply_SelectInputFiles = new System.Windows.Forms.Label();
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
            this.tabCommand_Create.Controls.Add(this.cbCreate_CombineAllAppendedData);
            this.tabCommand_Create.Controls.Add(this.cbCreate_OpenWhenGenerated);
            this.tabCommand_Create.Controls.Add(this.btnCreate_GenerateDFR);
            this.tabCommand_Create.Controls.Add(this.labelCreate_Separator2);
            this.tabCommand_Create.Controls.Add(this.tbCreate_OutputFile);
            this.tabCommand_Create.Controls.Add(this.tbCreate_AlteredFile);
            this.tabCommand_Create.Controls.Add(this.tbCreate_OriginalFile);
            this.tabCommand_Create.Controls.Add(this.labelCreate_OutputFile);
            this.tabCommand_Create.Controls.Add(this.labelCreate_SelectInputs);
            this.tabCommand_Create.Controls.Add(this.labelCreate_Separator1);
            this.tabCommand_Create.Controls.Add(this.btnCreate_OutputFile);
            this.tabCommand_Create.Controls.Add(this.btnCreate_AlteredFile);
            this.tabCommand_Create.Controls.Add(this.btnCreate_OriginalFile);
            this.tabCommand_Create.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Create.Name = "tabCommand_Create";
            this.tabCommand_Create.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Create.Size = new System.Drawing.Size(691, 204);
            this.tabCommand_Create.TabIndex = 1;
            this.tabCommand_Create.Text = "Create DFR File";
            // 
            // cbCombineAllAppendedData
            // 
            this.cbCreate_CombineAllAppendedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCreate_CombineAllAppendedData.AutoSize = true;
            this.cbCreate_CombineAllAppendedData.Checked = true;
            this.cbCreate_CombineAllAppendedData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreate_CombineAllAppendedData.Location = new System.Drawing.Point(9, 144);
            this.cbCreate_CombineAllAppendedData.Name = "cbCreate_CombineAllAppendedData";
            this.cbCreate_CombineAllAppendedData.Size = new System.Drawing.Size(155, 17);
            this.cbCreate_CombineAllAppendedData.TabIndex = 9;
            this.cbCreate_CombineAllAppendedData.Text = "Combine all appended data";
            this.cbCreate_CombineAllAppendedData.UseVisualStyleBackColor = true;
            // 
            // cbOpenWhenGenerated
            // 
            this.cbCreate_OpenWhenGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbCreate_OpenWhenGenerated.AutoSize = true;
            this.cbCreate_OpenWhenGenerated.Checked = true;
            this.cbCreate_OpenWhenGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCreate_OpenWhenGenerated.Location = new System.Drawing.Point(9, 164);
            this.cbCreate_OpenWhenGenerated.Name = "cbCreate_OpenWhenGenerated";
            this.cbCreate_OpenWhenGenerated.Size = new System.Drawing.Size(233, 30);
            this.cbCreate_OpenWhenGenerated.TabIndex = 10;
            this.cbCreate_OpenWhenGenerated.Text = "Open automatically when generated\r\n(.DFR files can be opened in any text editor)";
            this.cbCreate_OpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // btnGenerateDFR
            // 
            this.btnCreate_GenerateDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate_GenerateDFR.Location = new System.Drawing.Point(598, 173);
            this.btnCreate_GenerateDFR.Name = "btnCreate_GenerateDFR";
            this.btnCreate_GenerateDFR.Size = new System.Drawing.Size(87, 25);
            this.btnCreate_GenerateDFR.TabIndex = 11;
            this.btnCreate_GenerateDFR.Text = "Generate DFR";
            this.btnCreate_GenerateDFR.UseVisualStyleBackColor = true;
            this.btnCreate_GenerateDFR.Click += new System.EventHandler(this.btnCreate_GenerateDFR_Click);
            // 
            // labelSeparator2
            // 
            this.labelCreate_Separator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCreate_Separator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCreate_Separator2.Location = new System.Drawing.Point(6, 133);
            this.labelCreate_Separator2.Name = "labelCreate_Separator2";
            this.labelCreate_Separator2.Size = new System.Drawing.Size(679, 2);
            this.labelCreate_Separator2.TabIndex = 8;
            // 
            // tbOutputFile
            // 
            this.tbCreate_OutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCreate_OutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbCreate_OutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCreate_OutputFile.Location = new System.Drawing.Point(99, 105);
            this.tbCreate_OutputFile.Name = "tbCreate_OutputFile";
            this.tbCreate_OutputFile.Size = new System.Drawing.Size(586, 20);
            this.tbCreate_OutputFile.TabIndex = 8;
            // 
            // tbAlteredFile
            // 
            this.tbCreate_AlteredFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCreate_AlteredFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCreate_AlteredFile.Location = new System.Drawing.Point(99, 50);
            this.tbCreate_AlteredFile.Name = "tbCreate_AlteredFile";
            this.tbCreate_AlteredFile.Size = new System.Drawing.Size(586, 20);
            this.tbCreate_AlteredFile.TabIndex = 4;
            // 
            // tbOriginalFile
            // 
            this.tbCreate_OriginalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCreate_OriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbCreate_OriginalFile.Location = new System.Drawing.Point(99, 25);
            this.tbCreate_OriginalFile.Name = "tbCreate_OriginalFile";
            this.tbCreate_OriginalFile.Size = new System.Drawing.Size(586, 20);
            this.tbCreate_OriginalFile.TabIndex = 2;
            // 
            // labelOutputFile
            // 
            this.labelCreate_OutputFile.AutoSize = true;
            this.labelCreate_OutputFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCreate_OutputFile.Location = new System.Drawing.Point(6, 87);
            this.labelCreate_OutputFile.Name = "labelCreate_OutputFile";
            this.labelCreate_OutputFile.Size = new System.Drawing.Size(153, 13);
            this.labelCreate_OutputFile.TabIndex = 6;
            this.labelCreate_OutputFile.Text = "Select Output Destination";
            // 
            // labelSelectInputs
            // 
            this.labelCreate_SelectInputs.AutoSize = true;
            this.labelCreate_SelectInputs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCreate_SelectInputs.Location = new System.Drawing.Point(6, 6);
            this.labelCreate_SelectInputs.Name = "labelCreate_SelectInputs";
            this.labelCreate_SelectInputs.Size = new System.Drawing.Size(106, 13);
            this.labelCreate_SelectInputs.TabIndex = 0;
            this.labelCreate_SelectInputs.Text = "Select Input Files";
            // 
            // labelSeparator1
            // 
            this.labelCreate_Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCreate_Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCreate_Separator1.Location = new System.Drawing.Point(6, 78);
            this.labelCreate_Separator1.Name = "labelCreate_Separator1";
            this.labelCreate_Separator1.Size = new System.Drawing.Size(679, 2);
            this.labelCreate_Separator1.TabIndex = 5;
            // 
            // btnOutputFile
            // 
            this.btnCreate_OutputFile.Location = new System.Drawing.Point(6, 104);
            this.btnCreate_OutputFile.Name = "btnCreate_OutputFile";
            this.btnCreate_OutputFile.Size = new System.Drawing.Size(87, 22);
            this.btnCreate_OutputFile.TabIndex = 7;
            this.btnCreate_OutputFile.Text = "Output File...";
            this.btnCreate_OutputFile.UseVisualStyleBackColor = true;
            this.btnCreate_OutputFile.Click += new System.EventHandler(this.btnCreate_OutputFile_Click);
            // 
            // btnAlteredFile
            // 
            this.btnCreate_AlteredFile.Location = new System.Drawing.Point(6, 49);
            this.btnCreate_AlteredFile.Name = "btnCreate_AlteredFile";
            this.btnCreate_AlteredFile.Size = new System.Drawing.Size(87, 22);
            this.btnCreate_AlteredFile.TabIndex = 3;
            this.btnCreate_AlteredFile.Text = "Altered File...";
            this.btnCreate_AlteredFile.UseVisualStyleBackColor = true;
            this.btnCreate_AlteredFile.Click += new System.EventHandler(this.btnCreate_AlteredFile_Click);
            // 
            // btnOriginalFile
            // 
            this.btnCreate_OriginalFile.Location = new System.Drawing.Point(6, 24);
            this.btnCreate_OriginalFile.Name = "btnCreate_OriginalFile";
            this.btnCreate_OriginalFile.Size = new System.Drawing.Size(87, 22);
            this.btnCreate_OriginalFile.TabIndex = 1;
            this.btnCreate_OriginalFile.Text = "Original File...";
            this.btnCreate_OriginalFile.UseVisualStyleBackColor = true;
            this.btnCreate_OriginalFile.Click += new System.EventHandler(this.btnCreate_OriginalFile_Click);
            // 
            // tabCommand_Apply
            // 
            this.tabCommand_Apply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(249)))), ((int)(((byte)(249)))));
            this.tabCommand_Apply.Controls.Add(this.cbApply_ApplyToOriginalFile);
            this.tabCommand_Apply.Controls.Add(this.btnApply_ApplyDFR);
            this.tabCommand_Apply.Controls.Add(this.labelApply_Separator2);
            this.tabCommand_Apply.Controls.Add(this.tbApply_OutputFile);
            this.tabCommand_Apply.Controls.Add(this.tbApply_DFRFile);
            this.tabCommand_Apply.Controls.Add(this.labelApply_SelectOutputDestination);
            this.tabCommand_Apply.Controls.Add(this.labelApply_Separator1);
            this.tabCommand_Apply.Controls.Add(this.btnApply_OutputFile);
            this.tabCommand_Apply.Controls.Add(this.btnApply_DFRFile);
            this.tabCommand_Apply.Controls.Add(this.tbApply_OriginalFile);
            this.tabCommand_Apply.Controls.Add(this.btnApply_OriginalFile);
            this.tabCommand_Apply.Controls.Add(this.labelApply_SelectInputFiles);
            this.tabCommand_Apply.Location = new System.Drawing.Point(4, 22);
            this.tabCommand_Apply.Name = "tabCommand_Apply";
            this.tabCommand_Apply.Padding = new System.Windows.Forms.Padding(3);
            this.tabCommand_Apply.Size = new System.Drawing.Size(691, 204);
            this.tabCommand_Apply.TabIndex = 0;
            this.tabCommand_Apply.Text = "Apply DFR File";
            // 
            // cbApplyToOriginalFile
            // 
            this.cbApply_ApplyToOriginalFile.AutoSize = true;
            this.cbApply_ApplyToOriginalFile.Checked = true;
            this.cbApply_ApplyToOriginalFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbApply_ApplyToOriginalFile.Location = new System.Drawing.Point(9, 107);
            this.cbApply_ApplyToOriginalFile.Name = "cbApply_ApplyToOriginalFile";
            this.cbApply_ApplyToOriginalFile.Size = new System.Drawing.Size(146, 17);
            this.cbApply_ApplyToOriginalFile.TabIndex = 7;
            this.cbApply_ApplyToOriginalFile.Text = "Apply DFR to Original File";
            this.cbApply_ApplyToOriginalFile.UseVisualStyleBackColor = true;
            // 
            // btnApplyDFR
            // 
            this.btnApply_ApplyDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply_ApplyDFR.Location = new System.Drawing.Point(598, 173);
            this.btnApply_ApplyDFR.Name = "btnApply_ApplyDFR";
            this.btnApply_ApplyDFR.Size = new System.Drawing.Size(87, 25);
            this.btnApply_ApplyDFR.TabIndex = 11;
            this.btnApply_ApplyDFR.Text = "Apply DFR";
            this.btnApply_ApplyDFR.UseVisualStyleBackColor = true;
            // 
            // labelSeparator2
            // 
            this.labelApply_Separator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApply_Separator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelApply_Separator2.Location = new System.Drawing.Point(6, 159);
            this.labelApply_Separator2.Name = "labelApply_Separator2";
            this.labelApply_Separator2.Size = new System.Drawing.Size(679, 2);
            this.labelApply_Separator2.TabIndex = 10;
            // 
            // tbOutputFile
            // 
            this.tbApply_OutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApply_OutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbApply_OutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbApply_OutputFile.Enabled = false;
            this.tbApply_OutputFile.Location = new System.Drawing.Point(99, 130);
            this.tbApply_OutputFile.Name = "tbApply_OutputFile";
            this.tbApply_OutputFile.Size = new System.Drawing.Size(586, 20);
            this.tbApply_OutputFile.TabIndex = 9;
            this.tbApply_OutputFile.Text = "(Same as Original File)";
            // 
            // tbDFRFile
            // 
            this.tbApply_DFRFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApply_DFRFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbApply_DFRFile.Location = new System.Drawing.Point(99, 50);
            this.tbApply_DFRFile.Name = "tbApply_DFRFile";
            this.tbApply_DFRFile.Size = new System.Drawing.Size(586, 20);
            this.tbApply_DFRFile.TabIndex = 4;
            // 
            // labelSelectOutputDestination
            // 
            this.labelApply_SelectOutputDestination.AutoSize = true;
            this.labelApply_SelectOutputDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApply_SelectOutputDestination.Location = new System.Drawing.Point(6, 87);
            this.labelApply_SelectOutputDestination.Name = "labelApply_SelectOutputDestination";
            this.labelApply_SelectOutputDestination.Size = new System.Drawing.Size(153, 13);
            this.labelApply_SelectOutputDestination.TabIndex = 6;
            this.labelApply_SelectOutputDestination.Text = "Select Output Destination";
            // 
            // labelSeparator1
            // 
            this.labelApply_Separator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelApply_Separator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelApply_Separator1.Location = new System.Drawing.Point(6, 78);
            this.labelApply_Separator1.Name = "labelApply_Separator1";
            this.labelApply_Separator1.Size = new System.Drawing.Size(679, 2);
            this.labelApply_Separator1.TabIndex = 5;
            // 
            // btnOutputFile
            // 
            this.btnApply_OutputFile.Enabled = false;
            this.btnApply_OutputFile.Location = new System.Drawing.Point(6, 129);
            this.btnApply_OutputFile.Name = "btnApply_OutputFile";
            this.btnApply_OutputFile.Size = new System.Drawing.Size(87, 22);
            this.btnApply_OutputFile.TabIndex = 8;
            this.btnApply_OutputFile.Text = "Output File...";
            this.btnApply_OutputFile.UseVisualStyleBackColor = true;
            // 
            // btnDFRFile
            // 
            this.btnApply_DFRFile.Location = new System.Drawing.Point(6, 49);
            this.btnApply_DFRFile.Name = "btnApply_DFRFile";
            this.btnApply_DFRFile.Size = new System.Drawing.Size(87, 22);
            this.btnApply_DFRFile.TabIndex = 3;
            this.btnApply_DFRFile.Text = "DFR File...";
            this.btnApply_DFRFile.UseVisualStyleBackColor = true;
            // 
            // tbOriginalFile
            // 
            this.tbApply_OriginalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbApply_OriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbApply_OriginalFile.Location = new System.Drawing.Point(99, 25);
            this.tbApply_OriginalFile.Name = "tbApply_OriginalFile";
            this.tbApply_OriginalFile.Size = new System.Drawing.Size(586, 20);
            this.tbApply_OriginalFile.TabIndex = 2;
            // 
            // btnOriginalFile
            // 
            this.btnApply_OriginalFile.Location = new System.Drawing.Point(6, 24);
            this.btnApply_OriginalFile.Name = "btnApply_OriginalFile";
            this.btnApply_OriginalFile.Size = new System.Drawing.Size(87, 22);
            this.btnApply_OriginalFile.TabIndex = 1;
            this.btnApply_OriginalFile.Text = "Original File...";
            this.btnApply_OriginalFile.UseVisualStyleBackColor = true;
            // 
            // labelSelectInputFiles
            // 
            this.labelApply_SelectInputFiles.AutoSize = true;
            this.labelApply_SelectInputFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApply_SelectInputFiles.Location = new System.Drawing.Point(6, 6);
            this.labelApply_SelectInputFiles.Name = "labelApply_SelectInputFiles";
            this.labelApply_SelectInputFiles.Size = new System.Drawing.Size(106, 13);
            this.labelApply_SelectInputFiles.TabIndex = 0;
            this.labelApply_SelectInputFiles.Text = "Select Input Files";
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
            this.tabCommand_Create.PerformLayout();
            this.tabCommand_Apply.ResumeLayout(false);
            this.tabCommand_Apply.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TabControl tabCommand;
        private TabPage tabCommand_Apply;
        private TabPage tabCommand_Create;
        private Label labelCreate_Separator2;
        private TextBox tbCreate_OutputFile;
        private TextBox tbCreate_AlteredFile;
        private TextBox tbCreate_OriginalFile;
        private Label labelCreate_OutputFile;
        private Label labelCreate_SelectInputs;
        private Label labelCreate_Separator1;
        private Button btnCreate_OutputFile;
        private Button btnCreate_AlteredFile;
        private Button btnCreate_OriginalFile;
        private CheckBox cbCreate_CombineAllAppendedData;
        private CheckBox cbCreate_OpenWhenGenerated;
        private Button btnCreate_GenerateDFR;
        private TextBox tbApply_OriginalFile;
        private Button btnApply_OriginalFile;
        private Label labelApply_SelectInputFiles;
        private Button btnApply_ApplyDFR;
        private Label labelApply_Separator2;
        private TextBox tbApply_DFRFile;
        private Label labelApply_SelectOutputDestination;
        private Label labelApply_Separator1;
        private Button btnApply_OutputFile;
        private Button btnApply_DFRFile;
        private TextBox tbApply_OutputFile;
        private CheckBox cbApply_ApplyToOriginalFile;
    }
}