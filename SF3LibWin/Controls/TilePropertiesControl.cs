using System;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class TilePropertiesControl : UserControl {
        public TilePropertiesControl() {
            InitializeComponent();

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

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Bind controls to tile.
            void PerformUpdate(Action action) {
                if (!_updatingControls)
                    action();
            }

            cbMoveTerrain.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.MoveTerrain = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveHeight.ValueChanged                 += (s, e) => PerformUpdate(() => _tile.MoveHeight = (float) nudMoveHeight.Value);
            nudMoveHeightmapTL.ValueChanged            += (s, e) => PerformUpdate(() => _tile.SetMoveHeightmap(CornerType.TopLeft, (float) nudMoveHeightmapTL.Value));
            nudMoveHeightmapTR.ValueChanged            += (s, e) => PerformUpdate(() => _tile.SetMoveHeightmap(CornerType.TopRight, (float) nudMoveHeightmapTR.Value));
            nudMoveHeightmapBL.ValueChanged            += (s, e) => PerformUpdate(() => _tile.SetMoveHeightmap(CornerType.BottomLeft, (float) nudMoveHeightmapBL.Value));
            nudMoveHeightmapBR.ValueChanged            += (s, e) => PerformUpdate(() => _tile.SetMoveHeightmap(CornerType.BottomRight, (float) nudMoveHeightmapBR.Value));

            nudItemID.ValueChanged                     += (s, e) => PerformUpdate(() => _tile.ItemID = (byte) nudItemID.Value);

            nudModelTextureID.ValueChanged             += (s, e) => PerformUpdate(() => _tile.ModelTextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.ModelTextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged           += (s, e) => PerformUpdate(() => _tile.ModelTextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelUseMovementHeightmap.CheckedChanged += (s, e) => PerformUpdate(() => _tile.ModelUseMoveHeightmap = cbModelUseMovementHeightmap.Checked);
            nudModelVertexHeightmapTL.ValueChanged     += (s, e) => PerformUpdate(() => _tile.SetModelVertexHeightmap(CornerType.TopLeft, (float) nudModelVertexHeightmapTL.Value));
            nudModelVertexHeightmapTR.ValueChanged     += (s, e) => PerformUpdate(() => _tile.SetModelVertexHeightmap(CornerType.TopRight, (float) nudModelVertexHeightmapTR.Value));
            nudModelVertexHeightmapBL.ValueChanged     += (s, e) => PerformUpdate(() => _tile.SetModelVertexHeightmap(CornerType.BottomLeft, (float) nudModelVertexHeightmapBL.Value));
            nudModelVertexHeightmapBR.ValueChanged     += (s, e) => PerformUpdate(() => _tile.SetModelVertexHeightmap(CornerType.BottomRight, (float) nudModelVertexHeightmapBR.Value));

            // Update enabled status, visibility, and default values of controls.
            UpdateControls();
        }

        private bool _updatingControls = false;

        private void UpdateControls() {
            // Guard to prevent tiles from being edited while values are being initialized.
            if (_updatingControls)
                return;
            _updatingControls = true;

            // 'Current Tile' group
            if (_tile == null) {
                labelTileEdited.Text = "Tile: (no tile)";
                labelRealCoordinates.Text = "";
            }
            else {
                labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";
            }

            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = value.ToString();
            }

            // 'Movement' group
            gbMovement.Enabled = _tile != null;
            if (!gbMovement.Enabled) {
                cbMoveTerrain.SelectedItem = null;
                nudMoveHeight.Text = "";
                nudMoveHeightmapTL.Text = "";
                nudMoveHeightmapTR.Text = "";
                nudMoveHeightmapBL.Text = "";
                nudMoveHeightmapBR.Text = "";
            }
            else {
                cbMoveTerrain.SelectedItem = _tile.MoveTerrain;
                InitNUD(nudMoveHeight, (decimal) _tile.MoveHeight);
                InitNUD(nudMoveHeightmapTL, (decimal) _tile.GetMoveHeightmap(CornerType.TopLeft));
                InitNUD(nudMoveHeightmapTR, (decimal) _tile.GetMoveHeightmap(CornerType.TopRight));
                InitNUD(nudMoveHeightmapBL, (decimal) _tile.GetMoveHeightmap(CornerType.BottomLeft));
                InitNUD(nudMoveHeightmapBR, (decimal) _tile.GetMoveHeightmap(CornerType.BottomRight));
            }

            // 'Item' group
            gbItem.Enabled = _tile != null;
            if (!gbItem.Enabled)
                nudItemID.Text = "";
            else
                InitNUD(nudItemID, _tile.ItemID);

            // 'Model' group
            gbModel.Enabled = _tile != null && _tile.MPD_File.SurfaceModel != null;
            if (!gbModel.Enabled) {
                nudModelTextureID.Text = "";
                cbModelFlip.SelectedItem = null;
                cbModelRotate.SelectedItem = null;
                cbModelUseMovementHeightmap.Checked = false;
                nudModelVertexHeightmapTL.Text = "";
                nudModelVertexHeightmapTR.Text = "";
                nudModelVertexHeightmapBL.Text = "";
                nudModelVertexHeightmapBR.Text = "";
            }
            else {
                nudModelTextureID.Value = _tile.ModelTextureID;
                cbModelFlip.SelectedItem = _tile.ModelTextureFlip;
                cbModelRotate.SelectedItem = _tile.ModelTextureRotate;
                cbModelUseMovementHeightmap.Checked = _tile.ModelUseMoveHeightmap;
                InitNUD(nudModelVertexHeightmapTL, (decimal) _tile.GetModelVertexHeightmap(CornerType.TopLeft));
                InitNUD(nudModelVertexHeightmapTR, (decimal) _tile.GetModelVertexHeightmap(CornerType.TopRight));
                InitNUD(nudModelVertexHeightmapBL, (decimal) _tile.GetModelVertexHeightmap(CornerType.BottomLeft));
                InitNUD(nudModelVertexHeightmapBR, (decimal) _tile.GetModelVertexHeightmap(CornerType.BottomRight));
            }

            _updatingControls = false;
        }

        private Tile _tile = null;

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
