using System;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class MPD_ViewerControl : UserControl {
        public MPD_ViewerControl() {
            InitializeComponent();

            GLControl.TilePropertiesControl = tilePropertyControl1;
            Disposed += (s, e) => GLControl.Dispose();

            tsbToggleWireframe.Checked   = GLControl.DrawWireframe;
            tsbToggleBoundaries.Checked  = GLControl.DrawBoundaries;
            tsbToggleHelp.Checked        = GLControl.DrawHelp;
            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;

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

        private void tsbToggleWireframe_Click(object sender, EventArgs e) {
            GLControl.DrawWireframe = !GLControl.DrawWireframe;
            tsbToggleWireframe.Checked = GLControl.DrawWireframe;
        }

        private void tsbToggleBoundaries_Click(object sender, EventArgs e) {
            GLControl.DrawBoundaries = !GLControl.DrawBoundaries;
            tsbToggleBoundaries.Checked = GLControl.DrawBoundaries;
        }

        private void tsbToggleHelp_Click(object sender, EventArgs e) {
            GLControl.DrawHelp = !GLControl.DrawHelp;
            tsbToggleHelp.Checked = GLControl.DrawHelp;
        }

        private void tsbToggleNormals_Click(object sender, EventArgs e) {
            GLControl.DrawNormals = !GLControl.DrawNormals;

            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
        }

        private void tsbToggleTerrainType_Click(object sender, EventArgs e) {
            GLControl.DrawTerrainTypes = !GLControl.DrawTerrainTypes;

            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
        }

        private void tsbToggleEventID_Click(object sender, EventArgs e) {
            GLControl.DrawEventIDs = !GLControl.DrawEventIDs;

            tsbToggleNormals.Checked     = GLControl.DrawNormals;
            tsbToggleTerrainType.Checked = GLControl.DrawTerrainTypes;
            tsbToggleEventID.Checked     = GLControl.DrawEventIDs;
        }

        public void UpdateLighting() {
            if (MPD_File != null)
                GLControl.UpdateLighting();
        }

        public void UpdateModels() {
            if (MPD_File != null) {
                MPD_File.AssociateTilesWithTrees();
                GLControl.UpdateModels();
                GLControl.UpdateSurfaceModels();
                GLControl.UpdateGroundModel();
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
