using System;
using System.Windows.Forms;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            SuspendLayout();
            InitializeComponent();

            GLControl = new MPD_ViewerGLControl();
            GLControl.Dock = DockStyle.Fill;
            Controls.Add(GLControl);
            ResumeLayout();

            Disposed += (s, e) => GLControl.Dispose();

            tsbToggleWireframe.Checked = GLControl.DrawWireframe;
            tsbToggleHelp.Checked      = GLControl.DrawHelp;
        }

        public MPD_ViewerGLControl GLControl { get; }

        private void tsbToggleWireframe_Click(object sender, EventArgs e) {
            GLControl.DrawWireframe = !GLControl.DrawWireframe;
            tsbToggleWireframe.Checked = GLControl.DrawWireframe;
        }

        private void tsbToggleHelp_Click(object sender, EventArgs e) {
            GLControl.DrawHelp = !GLControl.DrawHelp;
            tsbToggleHelp.Checked = GLControl.DrawHelp;
        }

        private void tsbToggleNormals_Click(object sender, EventArgs e) {
            GLControl.DrawNormals = !GLControl.DrawNormals;
            tsbToggleNormals.Checked = GLControl.DrawNormals;
        }
    }
}
