using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

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
            tsbToggleNormals.Checked   = GLControl.DrawNormals;
        }

        public IMPD_File Model { get; set; }
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

        public void UpdateMap() {
            if (Model == null)
                return;

            var textureData = Model.TileSurfaceCharacterRows?.Make2DTextureData();
            GLControl.UpdateSurfaceModels(
                textureData,
                Model.TextureChunks,
                Model.TextureAnimations,
                Model.TileSurfaceHeightmapRows?.Rows,
                Model.TileSurfaceVertexNormalMeshBlocks?.Rows
            );
        }

        private void tsbRecalculateLightmapOriginalMath_Click(object sender, EventArgs e) {
            if (Model == null)
                return;

            Model.UpdateSurfaceVertexAbnormals(false);
            UpdateMap();
        }

        private void tsbUpdateLightmapUpdatedMath_Click(object sender, EventArgs e) {
            if (Model == null)
                return;

            Model.UpdateSurfaceVertexAbnormals(true);
            UpdateMap();
        }
    }
}
