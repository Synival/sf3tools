using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using OpenTK.Mathematics;
using SF3.Models.Files.MPD;
using SF3.Win.OpenGL.MPD_File;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            InitializeComponent();

            GLControl.TilePropertiesControl = tilePropertyControl1;
            Disposed += (s, e) => GLControl.Dispose();

            tsbDrawSurfaceModel.Checked  = GLControl.DrawSurfaceModel;
            tsbDrawModels.Checked        = GLControl.DrawModels;
            tsbDrawGround.Checked        = GLControl.DrawGround;
            tsbDrawSkyBox.Checked        = GLControl.DrawSkyBox;
            tsbRunAnimations.Checked     = GLControl.RunAnimations;
            tsbDrawGradients.Checked     = GLControl.DrawGradients;

            tsbToggleWireframe.Checked   = GLControl.DrawWireframe;
            tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
            tsbToggleNormals.Checked     = GLControl.DrawNormals;

            tsbRotateSpritesUp.Checked   = GLControl.RotateSpritesUp;

            tsbToggleHelp.Checked        = GLControl.DrawHelp;

            // Make sure certain key events make it to the GLControl.
            tilePropertyControl1.CmdKey += (object sender, ref Message msg, Keys keyData, ref bool wasProcessed) => {
                if (wasProcessed)
                    return;

                bool sendToGLControl = false;

                var keyPressed = (Keys) ((int) keyData & 0xFFFF);
                switch (keyPressed) {
                    case Keys.Up:
                    case Keys.Down:
                    case Keys.Left:
                    case Keys.Right:
                        if (keyData.HasFlag(Keys.Control))
                            sendToGLControl = true;
                        break;
                }

                if (sendToGLControl)
                    GLControl.RunCmdKeyEvent(sender, ref msg, keyData, ref wasProcessed);
            };
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

        private void tsbDrawSurfaceModel_Click (object sender, EventArgs e) => tsbDrawSurfaceModel.Checked  = GLControl.DrawSurfaceModel = !GLControl.DrawSurfaceModel;
        private void tsbDrawModels_Click       (object sender, EventArgs e) => tsbDrawModels.Checked        = GLControl.DrawModels       = !GLControl.DrawModels;
        private void tsbDrawGround_Click       (object sender, EventArgs e) => tsbDrawGround.Checked        = GLControl.DrawGround       = !GLControl.DrawGround;
        private void tsbDrawSkyBox_Click       (object sender, EventArgs e) => tsbDrawSkyBox.Checked        = GLControl.DrawSkyBox       = !GLControl.DrawSkyBox;
        private void tsbRunAnimations_Click    (object sender, EventArgs e) => tsbRunAnimations.Checked     = GLControl.RunAnimations    = !GLControl.RunAnimations;
        private void tsbDrawGradients_Click    (object sender, EventArgs e) => tsbDrawGradients.Checked     = GLControl.DrawGradients    = !GLControl.DrawGradients;

        private void tsbToggleWireframe_Click  (object sender, EventArgs e) => tsbToggleWireframe.Checked   = GLControl.DrawWireframe    = !GLControl.DrawWireframe;
        private void tsbToggleBoundaries_Click (object sender, EventArgs e) => tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries   = !GLControl.DrawBoundaries;
        private void tsbToggleTerrainType_Click(object sender, EventArgs e) => tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes = !GLControl.DrawTerrainTypes;
        private void tsbToggleEventID_Click    (object sender, EventArgs e) => tsbToggleEventID.Checked     = GLControl.DrawEventIDs     = !GLControl.DrawEventIDs;
        private void tsbToggleNormals_Click    (object sender, EventArgs e) => tsbToggleNormals.Checked     = GLControl.DrawNormals      = !GLControl.DrawNormals;

        private void tsbRotateSpritesUp_Click  (object sender, EventArgs e) => tsbRotateSpritesUp.Checked   = GLControl.RotateSpritesUp  = !GLControl.RotateSpritesUp;

        private void tsbToggleHelp_Click       (object sender, EventArgs e) => tsbToggleHelp.Checked        = GLControl.DrawHelp         = !GLControl.DrawHelp;

        public void UpdateLighting() {
            if (MPD_File != null)
                GLControl.UpdateLighting();
        }

        public void UpdateModels() {
            if (MPD_File != null) {
                MPD_File.AssociateTilesWithTrees();
                GLControl.UpdateModels();
            }
        }

        private void tsbRecalculateLightmapOriginalMath_Click(object sender, EventArgs e) {
            MPD_File?.SurfaceModel?.UpdateVertexNormals(MPD_File.Surface?.HeightmapRowTable, POLYGON_NormalCalculationMethod.TopRightTriangle);
            UpdateModels();
        }

        private void tsbUpdateLightmapUpdatedMath_Click(object sender, EventArgs e) {
            MPD_File?.SurfaceModel?.UpdateVertexNormals(MPD_File.Surface?.HeightmapRowTable, POLYGON_NormalCalculationMethod.WeightedVerticalTriangles);
            UpdateModels();
        }

        private struct CameraRefs {
            public float Width;
            public float Height;
            public Vector3 Center;
        }

        private CameraRefs CreateCameraRefs() {
            float width   = 64.0f;
            float height  = 64.0f;
            float centerX = 0.0f;
            float groundY = 0.0f;
            float centerZ = 0.0f;

            if (MPD_File?.BoundariesTable?.Length >= 2) {
                var bounds = MPD_File.BoundariesTable;
                var x1 = bounds.Min(x => x.X1);
                var y1 = bounds.Min(x => x.Y1);
                var x2 = bounds.Max(x => x.X2);
                var y2 = bounds.Max(x => x.Y2);

                width   = (x2 - x1) / 32.00f;
                height  = (y2 - y1) / 32.00f;
                centerX = (x1 + x2) / 2.0f /  32.00f + WorldResources.ModelOffsetX;
                centerZ = (y1 + y2) / 2.0f / -32.00f + WorldResources.ModelOffsetZ + 64.00f;
                groundY = (MPD_File?.MPDHeader?.GroundY ?? 0) / -32.0f;
            }

            return new CameraRefs {
                Width  = width,
                Height = height,
                Center = new Vector3(centerX, groundY, centerZ)
            };
        }

        private void tsbCameraReset_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();
            var size = Math.Max(refs.Width, refs.Height);
            GLControl.ResetCamera(refs.Center + (0, size * 0.071f, 0), size * 2f);
            GLControl.Invalidate();
        }

        private void tsbCameraTopView_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();
            GLControl.Position = (refs.Center.X, Math.Max(refs.Width * 0.80f, refs.Height) * 2.75f + refs.Center.Y, refs.Center.Z);
            GLControl.Pitch    = -90;
            GLControl.Yaw      = 0;
            GLControl.Invalidate();
        }

        private void tsbCameraLookAtCenter_Click(object sender, EventArgs e) {
            var refs = CreateCameraRefs();
            GLControl.LookAtTarget(refs.Center);
            GLControl.Invalidate();
        }

        // TODO: This kinda works, but not completely! Improve, refine, and ship it!!!
#if false
        private void WIP_CopyChnuk3FromOtherFile() {
            var thisMPD = (MPD_File) MPD_File;
            var otherMPD = Models.Files.MPD.MPD_File.Create(new ByteData.ByteData(new ByteArray(File.ReadAllBytes("E:/S2CHUR.MPD"))), null);

            // 16-bit to 32-bit
            var oldTableLoc = otherMPD.TextureAnimations.Address;
            int posIn = oldTableLoc;
            int posOut = 0x1200;
            var sizeIn = otherMPD.TextureAnimations.SizeInBytes + 2;
            int posInMax = sizeIn + oldTableLoc;
            while (posIn < posInMax) {
                ushort dataIn = (ushort) otherMPD.Data.GetWord(posIn);
                uint dataOut = dataIn;

                if (dataOut == 0xFFFF)
                    dataOut = 0xFFFF_FFFF;
                else if (dataOut == 0xFFFE)
                    dataOut = 0xFFFF_FFFE;

                thisMPD.Data.SetDouble(posOut, (int) dataOut);

                posIn += 2;
                posOut += 4;
            }
            thisMPD.MPDHeader[0].OffsetTextureAnimations = 0x291200;

            // Copy Chunk3.
            var endPos = thisMPD.Data.Length;
            var chunk3 = otherMPD.ChunkData[3];

            thisMPD.ChunkHeader[3].ChunkAddress = endPos + 0x290000;
            thisMPD.ChunkHeader[3].ChunkSize = chunk3.Length;
            thisMPD.Data.Data.SetDataAtTo(endPos, 0, chunk3.Data.GetDataCopy());

            MessageBox.Show("Chunk3 copied! (Don't do this again on the same file D: )");
        }
#endif
    }
}
