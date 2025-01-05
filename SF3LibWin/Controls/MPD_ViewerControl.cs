using System;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            InitializeComponent();
            Disposed += (s, e) => GLControl.Dispose();

            tsbToggleWireframe.Checked = GLControl.DrawWireframe;
            tsbToggleHelp.Checked      = GLControl.DrawHelp;
            tsbToggleNormals.Checked   = GLControl.DrawNormals;
        }

        private IMPD_File _mpdFile = null;

        public IMPD_File MPD_File {
            get => _mpdFile;
            set {
                if (value != _mpdFile) {
                    _mpdFile = value;
                    GLControl.MPD_File = value;
                }
            }
        }

        public MPD_ViewerGLControl GLControl => mpdViewerGLControl1;

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

        public void UpdateModels() {
            if (MPD_File == null)
                return;

            GLControl.UpdateSurfaceModels();
        }

        private void tsbRecalculateLightmapOriginalMath_Click(object sender, EventArgs e) {
            MPD_File?.SurfaceModel?.UpdateVertexAbnormals(MPD_File.Surface?.HeightmapRowTable, POLYGON_NormalCalculationMethod.TopLeftTriangle);
            UpdateModels();
        }

        private void tsbUpdateLightmapUpdatedMath_Click(object sender, EventArgs e) {
            MPD_File?.SurfaceModel?.UpdateVertexAbnormals(MPD_File.Surface?.HeightmapRowTable, POLYGON_NormalCalculationMethod.WeightedVerticalTriangles);
            UpdateModels();
        }
    }
}
