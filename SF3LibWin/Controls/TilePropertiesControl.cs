using System;
using System.Linq;
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
            void PerformUpdate(Action action, bool updatesModel = true) {
                if (_updatingControls > 0)
                    return;

                action();
                if (updatesModel)
                    ModelUpdated?.Invoke(this, EventArgs.Empty);
            }

            void SetMoveHeightmap(CornerType corner, float value) {
                if (_updatingControls > 0)
                    return;

                _updatingControls++;
                var oldValue = _tile.GetMoveHeightmap(corner);
                _tile.SetMoveHeightmap(corner, value);
                SetMoveHeightToAverageMoveHeight();

                if (cbLinkHeightmaps.Checked) {
                    GetNUDModelVertexHeightmap(corner).Value = (decimal) value;
                    _tile.SetModelVertexHeightmap(corner, value);
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }
                _updatingControls--;

                if (cbLinkHeightmaps.Checked || _tile.ModelUseMoveHeightmap) {
                    _tile.UpdateAbnormals();
                    ModelUpdated?.Invoke(this, EventArgs.Empty);
                }
            }

            void SetModelVertexHeightmap(CornerType corner, float value) {
                if (_updatingControls > 0)
                    return;

                _updatingControls++;
                _tile.SetModelVertexHeightmap(corner, value);
                if (cbLinkHeightmaps.Checked) {
                    var oldValue = _tile.GetMoveHeightmap(corner);
                    GetNUDMoveHeightmap(corner).Value = (decimal) value;
                    _tile.SetMoveHeightmap(corner, value);
                    SetMoveHeightToAverageMoveHeight();
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }
                _updatingControls--;

                _tile.UpdateAbnormals();
                ModelUpdated?.Invoke(this, EventArgs.Empty);
            }

            cbMoveTerrain.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.MoveTerrain = (TerrainType) cbMoveTerrain.SelectedValue, false);
            nudMoveHeight.ValueChanged                 += (s, e) => PerformUpdate(() => _tile.MoveHeight = (float) nudMoveHeight.Value, false);
            foreach (var corner in Enum.GetValues<CornerType>()) {
                var nud = GetNUDMoveHeightmap(corner);
                nud.ValueChanged += (s, e) => PerformUpdate(() => SetMoveHeightmap(corner, (float) nud.Value));
            }
            nudMoveHeightmapTR.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.TopRight, (float) nudMoveHeightmapTR.Value));
            nudMoveHeightmapBL.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.BottomLeft, (float) nudMoveHeightmapBL.Value));
            nudMoveHeightmapBR.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.BottomRight, (float) nudMoveHeightmapBR.Value));

            nudItemID.ValueChanged                     += (s, e) => PerformUpdate(() => _tile.ItemID = (byte) nudItemID.Value, false);

            nudModelTextureID.ValueChanged             += (s, e) => PerformUpdate(() => _tile.ModelTextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.ModelTextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged           += (s, e) => PerformUpdate(() => _tile.ModelTextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelUseMovementHeightmap.CheckedChanged += (s, e) => PerformUpdate(() => {
                _tile.ModelUseMoveHeightmap = cbModelUseMovementHeightmap.Checked;
                UpdateLinkHeightmapsCheckbox();
            });
            foreach (var corner in Enum.GetValues<CornerType>()) {
                var nud = GetNUDModelVertexHeightmap(corner);
                nud.ValueChanged += (s, e) => PerformUpdate(() => SetModelVertexHeightmap(corner, (float) nud.Value));
            }

            // Update enabled status, visibility, and default values of controls.
            UpdateControls();
        }

        private int _updatingControls = 0;

        private void UpdateControls() {
            // Guard to prevent tiles from being edited while values are being initialized.
            if (_updatingControls > 0)
                return;

            _updatingControls++;

            // 'Current Tile' group
            if (_tile == null) {
                labelTileEdited.Text = "Tile: (no tile)";
                labelRealCoordinates.Text = "";
                cbLinkHeightmaps.Checked = false;
                cbLinkHeightmaps.Enabled = false;
            }
            else {
                labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";
                UpdateLinkHeightmapsCheckbox();
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
                foreach (var corner in Enum.GetValues<CornerType>())
                    GetNUDMoveHeightmap(corner).Text = "";
            }
            else {
                cbMoveTerrain.SelectedItem = _tile.MoveTerrain;
                InitNUD(nudMoveHeight, (decimal) _tile.MoveHeight);
                foreach (var corner in Enum.GetValues<CornerType>())
                    InitNUD(GetNUDMoveHeightmap(corner), (decimal) _tile.GetMoveHeightmap(corner));
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
                foreach (var corner in Enum.GetValues<CornerType>())
                    GetNUDModelVertexHeightmap(corner).Text = "";
            }
            else {
                nudModelTextureID.Value = _tile.ModelTextureID;
                cbModelFlip.SelectedItem = _tile.ModelTextureFlip;
                cbModelRotate.SelectedItem = _tile.ModelTextureRotate;
                cbModelUseMovementHeightmap.Checked = _tile.ModelUseMoveHeightmap;
                foreach (var corner in Enum.GetValues<CornerType>())
                    InitNUD(GetNUDModelVertexHeightmap(corner), (decimal) _tile.GetModelVertexHeightmap(corner));
            }

            _updatingControls--;
        }

        private void UpdateLinkHeightmapsCheckbox() {
            cbLinkHeightmaps.Enabled = _tile.MPD_File.SurfaceModel != null & !_tile.ModelUseMoveHeightmap;
            cbLinkHeightmaps.Checked = cbLinkHeightmaps.Enabled &&
                Enum.GetValues<CornerType>().All(x => _tile.GetMoveHeightmap(x) == _tile.GetModelVertexHeightmap(x));

            var nuds = Enum.GetValues<CornerType>().Select(x => GetNUDModelVertexHeightmap(x)).ToArray();
            foreach (var nud in nuds)
                nud.Enabled = cbLinkHeightmaps.Enabled;
        }

        private void SetMoveHeightToAverageMoveHeight() {
            _updatingControls++;
            var avgHeight = (float) Math.Round(Enum.GetValues<CornerType>().Average(x => _tile.GetMoveHeightmap(x) * 16f)) / 16f;
            nudMoveHeight.Value = (decimal) avgHeight;
            _tile.MoveHeight = avgHeight;
            _updatingControls--;
        }

        private NumericUpDown GetNUDMoveHeightmap(CornerType corner) {
            switch (corner) {
                case CornerType.TopLeft:
                    return nudMoveHeightmapTL;
                case CornerType.TopRight:
                    return nudMoveHeightmapTR;
                case CornerType.BottomLeft:
                    return nudMoveHeightmapBL;
                case CornerType.BottomRight:
                    return nudMoveHeightmapBR;
                default:
                    throw new ArgumentException(nameof(corner));
            }
        }

        private NumericUpDown GetNUDModelVertexHeightmap(CornerType corner) {
            switch (corner) {
                case CornerType.TopLeft:
                    return nudModelVertexHeightmapTL;
                case CornerType.TopRight:
                    return nudModelVertexHeightmapTR;
                case CornerType.BottomLeft:
                    return nudModelVertexHeightmapBL;
                case CornerType.BottomRight:
                    return nudModelVertexHeightmapBR;
                default:
                    throw new ArgumentException(nameof(corner));
            }
        }

        private struct TileAndCorner {
            public int X;
            public int Y;
            public CornerType Corner;
        }

        private TileAndCorner[] GetAdjacentTilesAtCorner(CornerType corner) {
            TileAndCorner[] GetUnfiltered() {
                switch (corner) {
                    case CornerType.TopLeft:
                        return [
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y - 1, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y - 0, Corner = CornerType.TopRight },
                            new TileAndCorner { X = _tile.X - 0, Y = _tile.Y - 1, Corner = CornerType.BottomLeft },
                        ];

                    case CornerType.TopRight:
                        return [
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y - 1, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y - 0, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = _tile.X + 0, Y = _tile.Y - 1, Corner = CornerType.BottomRight },
                        ];

                    case CornerType.BottomRight:
                        return [
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y + 1, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y + 0, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = _tile.X + 0, Y = _tile.Y + 1, Corner = CornerType.TopRight },
                        ];

                    case CornerType.BottomLeft:
                        return [
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y + 1, Corner = CornerType.TopRight },
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y + 0, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = _tile.X - 0, Y = _tile.Y + 1, Corner = CornerType.TopLeft },
                        ];

                    default:
                        throw new ArgumentException(nameof(corner));
                }
            }

            return GetUnfiltered()
                .Where(x => x.X >= 0 && x.Y >= 0 && x.X < 64 && x.Y < 64)
                .ToArray();
        }

        private void UpdateNeighboringIdenticalMoveHeightmapValues(CornerType corner, float oldValue, float newValue) {
            var adjacentCorners = GetAdjacentTilesAtCorner(corner);
            foreach (var ac in adjacentCorners) {
                var acTile = _tile.MPD_File.Tiles[ac.X, ac.Y];
                if (acTile.GetMoveHeightmap(ac.Corner) == oldValue) {
                    acTile.SetMoveHeightmap(ac.Corner, newValue);
                    var avgHeight = (float) Math.Round(Enum.GetValues<CornerType>().Average(x => acTile.GetMoveHeightmap(x) * 16f)) / 16f;
                    acTile.MoveHeight = avgHeight;
                }
            }
        }

        public EventHandler ModelUpdated;

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
