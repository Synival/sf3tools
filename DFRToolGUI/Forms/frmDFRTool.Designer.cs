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
            this.btnOutputFile = new System.Windows.Forms.Button();
            this.btnAlteredFile = new System.Windows.Forms.Button();
            this.btnOriginalFile = new System.Windows.Forms.Button();
            this.labelSeparator1 = new System.Windows.Forms.Label();
            this.labelSelectInputs = new System.Windows.Forms.Label();
            this.labelOutputFile = new System.Windows.Forms.Label();
            this.btnGenerateDFR = new System.Windows.Forms.Button();
            this.tbOriginalFile = new System.Windows.Forms.TextBox();
            this.tbAlteredFile = new System.Windows.Forms.TextBox();
            this.tbOutputFile = new System.Windows.Forms.TextBox();
            this.labelSeparator2 = new System.Windows.Forms.Label();
            this.cbOpenWhenGenerated = new System.Windows.Forms.CheckBox();
            this.cbCombineAllAppendedData = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnOutputFile
            // 
            this.btnOutputFile.Location = new System.Drawing.Point(10, 101);
            this.btnOutputFile.Name = "btnOutputFile";
            this.btnOutputFile.Size = new System.Drawing.Size(87, 20);
            this.btnOutputFile.TabIndex = 2;
            this.btnOutputFile.Text = "Output File...";
            this.btnOutputFile.UseVisualStyleBackColor = true;
            // 
            // btnAlteredFile
            // 
            this.btnAlteredFile.Location = new System.Drawing.Point(10, 49);
            this.btnAlteredFile.Name = "btnAlteredFile";
            this.btnAlteredFile.Size = new System.Drawing.Size(87, 20);
            this.btnAlteredFile.TabIndex = 1;
            this.btnAlteredFile.Text = "Altered File...";
            this.btnAlteredFile.UseVisualStyleBackColor = true;
            // 
            // btnOriginalFile
            // 
            this.btnOriginalFile.Location = new System.Drawing.Point(10, 23);
            this.btnOriginalFile.Name = "btnOriginalFile";
            this.btnOriginalFile.Size = new System.Drawing.Size(87, 20);
            this.btnOriginalFile.TabIndex = 0;
            this.btnOriginalFile.Text = "Original File...";
            this.btnOriginalFile.UseVisualStyleBackColor = true;
            // 
            // labelSeparator1
            // 
            this.labelSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator1.Location = new System.Drawing.Point(10, 76);
            this.labelSeparator1.Name = "labelSeparator1";
            this.labelSeparator1.Size = new System.Drawing.Size(484, 2);
            this.labelSeparator1.TabIndex = 3;
            // 
            // labelSelectInputs
            // 
            this.labelSelectInputs.AutoSize = true;
            this.labelSelectInputs.Location = new System.Drawing.Point(10, 8);
            this.labelSelectInputs.Name = "labelSelectInputs";
            this.labelSelectInputs.Size = new System.Drawing.Size(88, 13);
            this.labelSelectInputs.TabIndex = 4;
            this.labelSelectInputs.Text = "Select Input Files";
            // 
            // labelOutputFile
            // 
            this.labelOutputFile.AutoSize = true;
            this.labelOutputFile.Location = new System.Drawing.Point(10, 86);
            this.labelOutputFile.Name = "labelOutputFile";
            this.labelOutputFile.Size = new System.Drawing.Size(128, 13);
            this.labelOutputFile.TabIndex = 5;
            this.labelOutputFile.Text = "Select Output Destination";
            // 
            // btnGenerateDFR
            // 
            this.btnGenerateDFR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateDFR.Location = new System.Drawing.Point(407, 144);
            this.btnGenerateDFR.Name = "btnGenerateDFR";
            this.btnGenerateDFR.Size = new System.Drawing.Size(87, 20);
            this.btnGenerateDFR.TabIndex = 6;
            this.btnGenerateDFR.Text = "Generate DFR";
            this.btnGenerateDFR.UseVisualStyleBackColor = true;
            // 
            // tbOriginalFile
            // 
            this.tbOriginalFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOriginalFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOriginalFile.Location = new System.Drawing.Point(103, 24);
            this.tbOriginalFile.Name = "tbOriginalFile";
            this.tbOriginalFile.Size = new System.Drawing.Size(391, 20);
            this.tbOriginalFile.TabIndex = 7;
            // 
            // tbAlteredFile
            // 
            this.tbAlteredFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbAlteredFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAlteredFile.Location = new System.Drawing.Point(103, 49);
            this.tbAlteredFile.Name = "tbAlteredFile";
            this.tbAlteredFile.Size = new System.Drawing.Size(391, 20);
            this.tbAlteredFile.TabIndex = 8;
            // 
            // tbOutputFile
            // 
            this.tbOutputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutputFile.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutputFile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbOutputFile.Location = new System.Drawing.Point(103, 101);
            this.tbOutputFile.Name = "tbOutputFile";
            this.tbOutputFile.Size = new System.Drawing.Size(391, 20);
            this.tbOutputFile.TabIndex = 9;
            // 
            // labelSeparator2
            // 
            this.labelSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSeparator2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeparator2.Location = new System.Drawing.Point(10, 129);
            this.labelSeparator2.Name = "labelSeparator2";
            this.labelSeparator2.Size = new System.Drawing.Size(484, 2);
            this.labelSeparator2.TabIndex = 10;
            // 
            // cbOpenWhenGenerated
            // 
            this.cbOpenWhenGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOpenWhenGenerated.AutoSize = true;
            this.cbOpenWhenGenerated.Checked = true;
            this.cbOpenWhenGenerated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOpenWhenGenerated.Location = new System.Drawing.Point(169, 139);
            this.cbOpenWhenGenerated.Name = "cbOpenWhenGenerated";
            this.cbOpenWhenGenerated.Size = new System.Drawing.Size(233, 30);
            this.cbOpenWhenGenerated.TabIndex = 11;
            this.cbOpenWhenGenerated.Text = "Open automatically when generated\r\n(.DFR files can be opened in any text editor)";
            this.cbOpenWhenGenerated.UseVisualStyleBackColor = true;
            // 
            // cbCombineAllAppendedData
            // 
            this.cbCombineAllAppendedData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbCombineAllAppendedData.AutoSize = true;
            this.cbCombineAllAppendedData.Checked = true;
            this.cbCombineAllAppendedData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCombineAllAppendedData.Location = new System.Drawing.Point(10, 145);
            this.cbCombineAllAppendedData.Name = "cbCombineAllAppendedData";
            this.cbCombineAllAppendedData.Size = new System.Drawing.Size(155, 17);
            this.cbCombineAllAppendedData.TabIndex = 12;
            this.cbCombineAllAppendedData.Text = "Combine all appended data";
            this.cbCombineAllAppendedData.UseVisualStyleBackColor = true;
            // 
            // frmDFRTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 179);
            this.Controls.Add(this.cbCombineAllAppendedData);
            this.Controls.Add(this.cbOpenWhenGenerated);
            this.Controls.Add(this.labelSeparator2);
            this.Controls.Add(this.tbOutputFile);
            this.Controls.Add(this.tbAlteredFile);
            this.Controls.Add(this.tbOriginalFile);
            this.Controls.Add(this.btnGenerateDFR);
            this.Controls.Add(this.labelOutputFile);
            this.Controls.Add(this.labelSelectInputs);
            this.Controls.Add(this.labelSeparator1);
            this.Controls.Add(this.btnOutputFile);
            this.Controls.Add(this.btnAlteredFile);
            this.Controls.Add(this.btnOriginalFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(2400, 218);
            this.MinimumSize = new System.Drawing.Size(520, 218);
            this.Name = "frmDFRTool";
            this.Text = "DFRTool v0.1.1 (Compatible with 25.1 patcher)";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Button btnOutputFile;
        private Button btnAlteredFile;
        private Button btnOriginalFile;
        private Label labelSeparator1;
        private Label labelSelectInputs;
        private Label labelOutputFile;
        private Button btnGenerateDFR;
        private TextBox tbOriginalFile;
        private TextBox tbAlteredFile;
        private TextBox tbOutputFile;
        private Label labelSeparator2;
        private CheckBox cbOpenWhenGenerated;
        private CheckBox cbCombineAllAppendedData;
    }
}