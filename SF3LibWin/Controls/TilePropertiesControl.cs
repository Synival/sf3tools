using System;
using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Controls {
    public partial class TilePropertiesControl : UserControl {
        public TilePropertiesControl() {
            InitializeComponent();
            UpdateControls();

            // Enforce rounding to the nearest 16th.
            void RoundToNearest16th(object sender) {
                var nudControl = (NumericUpDown) sender;
                nudControl.Value = Math.Floor(nudControl.Value * 16) / 16;
            }
            var heightNumericUpDowns = new NumericUpDown[] {
                nudMoveHeight,
                nudMoveHeightmapTL,
                nudMoveHeightmapTR,
                nudMoveHeightmapBL,
                nudMoveHeightmapBR,
                nudModelVertexHeightmapTL,
                nudModelVertexHeightmapTR,
                nudModelVertexHeightmapBL,
                nudModelVertexHeightmapBR
            };
            foreach (var nc in heightNumericUpDowns)
                nc.Validating += (s, e) => RoundToNearest16th(s);

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls.
            void SetEnterKeyBehavior(Control control) {
                foreach (var cObj in control.Controls) {
                    if (cObj is not Control c)
                        continue;

                    c.KeyUp += (sender, e) => {
                        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                            SelectNextControl(c, true, true, true, true);
                    };

                    SetEnterKeyBehavior(c);
                }
            }
            SetEnterKeyBehavior(this);
        }

        private void UpdateControls() {
            if (_tile == null) {
                labelTileEdited.Text = "Tile: (no tile)";
                labelRealCoordinates.Text = "";

                gbMovement.Enabled = false;
                cbMoveTerrain.Text = "";
                nudMoveHeight.Value = 0;
                nudMoveHeightmapTL.Value = 0;
                nudMoveHeightmapTR.Value = 0;
                nudMoveHeightmapBL.Value = 0;
                nudMoveHeightmapBR.Value = 0;

                gbItem.Enabled = false;
                nudItemID.Value = 0;

                gbModel.Enabled = false;
                nudModelTextureID.Value = 0;
                cbModelFlip.Text = "";
                cbModelRotate.Text = "";
                cbModelUseMovementHeightmap.Checked = false;
                nudModelVertexHeightmapTL.Value = 0;
                nudModelVertexHeightmapTR.Value = 0;
                nudModelVertexHeightmapBL.Value = 0;
                nudModelVertexHeightmapBR.Value = 0;
            }
            else {
                labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";

                gbMovement.Enabled = true;
                cbMoveTerrain.Text = "";
                nudMoveHeight.Value = 0;
                nudMoveHeightmapTL.Value = 0;
                nudMoveHeightmapTR.Value = 0;
                nudMoveHeightmapBL.Value = 0;
                nudMoveHeightmapBR.Value = 0;

                gbItem.Enabled = true;
                nudItemID.Value = 0;

                gbModel.Enabled = Tile.MPD_File.SurfaceModel != null;
                nudModelTextureID.Value = 0;
                cbModelFlip.Text = "";
                cbModelRotate.Text = "";
                cbModelUseMovementHeightmap.Checked = false;
                nudModelVertexHeightmapTL.Value = 0;
                nudModelVertexHeightmapTR.Value = 0;
                nudModelVertexHeightmapBL.Value = 0;
                nudModelVertexHeightmapBR.Value = 0;
            }
        }

        public Tile _tile = null;

        public Tile Tile {
            get => _tile;
            set {
                if (value == _tile)
                    return;

                _tile = value;
                UpdateControls();
            }
        }
    }
}
