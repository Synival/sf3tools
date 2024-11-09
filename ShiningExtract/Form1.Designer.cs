namespace ShiningExtract
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            extractChunkButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            compressButton = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // extractChunkButton
            // 
            extractChunkButton.Location = new Point(12, 12);
            extractChunkButton.Name = "extractChunkButton";
            extractChunkButton.Size = new Size(97, 23);
            extractChunkButton.TabIndex = 0;
            extractChunkButton.Text = "Extract Chunks";
            extractChunkButton.UseVisualStyleBackColor = true;
            extractChunkButton.Click += extractChunkButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 41);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 1;
            label1.Text = "Log Folder:";
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Location = new Point(102, 38);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(403, 23);
            textBox1.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 3;
            label2.Text = "Output Folder:";
            // 
            // textBox2
            // 
            textBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox2.Location = new Point(102, 67);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(403, 23);
            textBox2.TabIndex = 4;
            // 
            // compressButton
            // 
            compressButton.Location = new Point(242, 12);
            compressButton.Name = "compressButton";
            compressButton.Size = new Size(113, 23);
            compressButton.TabIndex = 5;
            compressButton.Text = "Compress Chunk";
            compressButton.UseVisualStyleBackColor = true;
            compressButton.Click += compressButton_Click;
            // 
            // button1
            // 
            button1.Location = new Point(115, 12);
            button1.Name = "button1";
            button1.Size = new Size(121, 23);
            button1.TabIndex = 6;
            button1.Text = "Decompress Chunk";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(514, 141);
            Controls.Add(button1);
            Controls.Add(compressButton);
            Controls.Add(textBox2);
            Controls.Add(label2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(extractChunkButton);
            Name = "Form1";
            Text = "Shining Extract V1.0.0.1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button extractChunkButton;
        private OpenFileDialog openFileDialog1;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Button compressButton;
        private Button button1;
    }
}