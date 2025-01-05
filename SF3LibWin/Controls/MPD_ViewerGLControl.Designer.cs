namespace SF3.Win.Controls {
    partial class MPD_ViewerGLControl {
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
            SuspendLayout();
            components = new System.ComponentModel.Container();

            API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            APIVersion = new System.Version(3, 3, 0, 0);
            Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            IsEventDriven = true;
            Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            SharedContext = null;
            ImeMode = System.Windows.Forms.ImeMode.Inherit;
            BackColor = System.Drawing.Color.FromArgb(  64,   64,   64);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "MPD_ViewerControl_GLControl";
            Size = new System.Drawing.Size(320, 240);
            MinimumSize = new System.Drawing.Size(320, 240);
            ResumeLayout();
        }

        #endregion
    }
}
