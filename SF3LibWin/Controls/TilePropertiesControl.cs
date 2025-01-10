using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using CommonLib;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.Models.Files.MPD;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class TilePropertiesControl : UserControl {
        public TilePropertiesControl() {
            InitializeComponent();

            _nudMoveHeightmaps = new Dictionary<CornerType, NumericUpDown>() {
                { CornerType.TopLeft,     nudMoveHeightmapTL },
                { CornerType.TopRight,    nudMoveHeightmapTR },
                { CornerType.BottomRight, nudMoveHeightmapBR },
                { CornerType.BottomLeft,  nudMoveHeightmapBL },
            };                

            _nudModelVertexHeightmaps = new Dictionary<CornerType, NumericUpDown>() {
                { CornerType.TopLeft,     nudModelVertexHeightmapTL },
                { CornerType.TopRight,    nudModelVertexHeightmapTR },
                { CornerType.BottomRight, nudModelVertexHeightmapBR },
                { CornerType.BottomLeft,  nudModelVertexHeightmapBL },
            };                

            // Enforce validation on height-related NumericUpDown's to round to nearest 16th.
            var heightNumericUpDowns = new List<NumericUpDown>() { nudMoveHeight };
            heightNumericUpDowns.AddRange(_nudMoveHeightmaps.Select(x => x.Value).ToList());
            heightNumericUpDowns.AddRange(_nudModelVertexHeightmaps.Select(x => x.Value).ToList());
            foreach (var nc in heightNumericUpDowns)
                EnforceRoundingToNearest16th(nc);

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls.
            RecursivelyMakeEnterKeyMoveToNextControl(this);

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged         += (s, e) => DoIfUserInput(() => _tile.MoveTerrain = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveHeight.ValueChanged                 += (s, e) => UserSetMoveHeight((float) nudMoveHeight.Value);
            foreach (var nud in _nudMoveHeightmaps)
                nud.Value.ValueChanged                 += (s, e) => UserSetMoveHeightmap(nud.Key, (float) nud.Value.Value);
            nudMoveHeightmapTR.ValueChanged            += (s, e) => UserSetMoveHeightmap(CornerType.TopRight, (float) nudMoveHeightmapTR.Value);
            nudMoveHeightmapBL.ValueChanged            += (s, e) => UserSetMoveHeightmap(CornerType.BottomLeft, (float) nudMoveHeightmapBL.Value);
            nudMoveHeightmapBR.ValueChanged            += (s, e) => UserSetMoveHeightmap(CornerType.BottomRight, (float) nudMoveHeightmapBR.Value);

            // Event handling for 'Event' group.
            nudEventID.ValueChanged                    += (s, e) => DoIfUserInput(() => _tile.EventID = (byte) nudEventID.Value);

            // Event handling for 'Model' group.
            nudModelTextureID.ValueChanged             += (s, e) => DoIfUserInput(() => _tile.ModelTextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged         += (s, e) => DoIfUserInput(() => _tile.ModelTextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged           += (s, e) => DoIfUserInput(() => _tile.ModelTextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelUseMovementHeightmap.CheckedChanged += (s, e) => DoIfUserInput(() => {
                _tile.ModelUseMoveHeightmap = cbModelUseMovementHeightmap.Checked;
                UpdateLinkHeightmapsCheckbox();
            });
            foreach (var nud in _nudModelVertexHeightmaps)
                nud.Value.ValueChanged                 += (s, e) => UserSetModelVertexHeightmap(nud.Key, (float) nud.Value.Value);

            // Update enabled status, visibility, and default values of controls.
            UpdateControls();
        }

        private void UpdateControls() {
            // Guard to prevent tiles from being edited while values are being initialized.
            if (_nonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                void InitNUD(NumericUpDown nud, decimal value) {
                    nud.Value = value;
                    nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
                }

                // 'Current Tile' group
                if (_tile == null) {
                    labelTileEdited.Text = "Tile: (no tile)";
                    labelRealCoordinates.Text = "";
                    UpdateLinkHeightmapsCheckbox();
                }
                else {
                    labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                    labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";
                    UpdateLinkHeightmapsCheckbox();
                }

                // 'Movement' group
                gbMovement.Enabled = _tile != null;
                if (!gbMovement.Enabled) {
                    cbMoveTerrain.SelectedItem = null;
                    nudMoveHeight.Text = "";
                    foreach (var nud in _nudMoveHeightmaps.Values)
                        nud.Text = "";
                }
                else {
                    cbMoveTerrain.SelectedItem = _tile.MoveTerrain;
                    InitNUD(nudMoveHeight, (decimal) _tile.MoveHeight);
                    foreach (var nud in _nudMoveHeightmaps)
                        InitNUD(nud.Value, (decimal) _tile.GetMoveHeightmap(nud.Key));
                }

                // 'Event' group
                gbEvent.Enabled = _tile != null;
                if (!gbEvent.Enabled)
                    nudEventID.Text = "";
                else
                    InitNUD(nudEventID, _tile.EventID);

                // 'Model' group
                gbModel.Enabled = _tile != null && _tile.MPD_File.SurfaceModel != null;
                if (!gbModel.Enabled) {
                    nudModelTextureID.Text = "";
                    cbModelFlip.SelectedItem = null;
                    cbModelRotate.SelectedItem = null;
                    cbModelUseMovementHeightmap.Checked = false;
                    foreach (var nud in _nudModelVertexHeightmaps.Values)
                        nud.Text = "";
                }
                else {
                    InitNUD(nudModelTextureID, _tile.ModelTextureID);
                    cbModelFlip.SelectedItem = _tile.ModelTextureFlip;
                    cbModelRotate.SelectedItem = _tile.ModelTextureRotate;
                    cbModelUseMovementHeightmap.Checked = _tile.ModelUseMoveHeightmap;
                    foreach (var nud in _nudModelVertexHeightmaps)
                        InitNUD(nud.Value, (decimal) _tile.GetModelVertexHeightmap(nud.Key));
                }
            }
        }

        private void UpdateLinkHeightmapsCheckbox() {
            cbLinkHeightmaps.Enabled = _tile != null && _tile.MPD_File.SurfaceModel != null && !_tile.ModelUseMoveHeightmap;
            cbLinkHeightmaps.Checked = cbLinkHeightmaps.Enabled &&
                Enum.GetValues<CornerType>().All(x => _tile.GetMoveHeightmap(x) == _tile.GetModelVertexHeightmap(x));

            foreach (var nud in _nudModelVertexHeightmaps.Values)
                nud.Enabled = cbLinkHeightmaps.Enabled;
        }

        private void SetMoveHeightToAverageMoveHeight() {
            using (IncrementNonUserInputGuard()) {
                var avgHeight = _tile.GetAverageHeight();
                nudMoveHeight.Value = (decimal) avgHeight;
                _tile.MoveHeight = avgHeight;
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
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y + 1, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y + 0, Corner = CornerType.TopRight },
                            new TileAndCorner { X = _tile.X - 0, Y = _tile.Y + 1, Corner = CornerType.BottomLeft },
                        ];

                    case CornerType.TopRight:
                        return [
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y + 1, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y + 0, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = _tile.X + 0, Y = _tile.Y + 1, Corner = CornerType.BottomRight },
                        ];

                    case CornerType.BottomRight:
                        return [
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y - 1, Corner = CornerType.TopLeft },
                            new TileAndCorner { X = _tile.X + 1, Y = _tile.Y - 0, Corner = CornerType.BottomLeft },
                            new TileAndCorner { X = _tile.X + 0, Y = _tile.Y - 1, Corner = CornerType.TopRight },
                        ];

                    case CornerType.BottomLeft:
                        return [
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y - 1, Corner = CornerType.TopRight },
                            new TileAndCorner { X = _tile.X - 1, Y = _tile.Y - 0, Corner = CornerType.BottomRight },
                            new TileAndCorner { X = _tile.X - 0, Y = _tile.Y - 1, Corner = CornerType.TopLeft },
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
                    acTile.MoveHeight = acTile.GetAverageHeight();
                }
            }
        }

        private void UpdateAbnormalsForCorner(CornerType corner) {
            var surfaceModel = _tile.MPD_File.SurfaceModel;
            if (surfaceModel == null)
                return;

            var vxCenter = BlockHelpers.TileToVertexX(_tile.X, corner);
            var vyCenter = BlockHelpers.TileToVertexY(_tile.Y, corner);

            // Abnormals need to be updated in a 3x3 grid.
            for (var x = -1; x <= 1; x++) {
                for (var y = -1; y <= 1; y++) {
                    var vx = x + vxCenter;
                    var vy = y + vyCenter;
                    if (vx >= 0 && vy >= 0 && vx < 65 && vy < 65)
                        surfaceModel.UpdateVertexAbnormal(vx, vy, _tile.MPD_File.Surface.HeightmapRowTable, POLYGON_NormalCalculationMethod.WeightedVerticalTriangles);
                }
            }

            // Updating vertex normals in a 3x3 grid means tiles need to be re-rendered in a 4x4 grid.
            for (var x = -2; x < 2; x++) {
                for (var y = -2; y < 2; y++) {
                    var tx = x + vxCenter;
                    var ty = y + vyCenter;
                    if (tx >= 0 && ty >= 0 && tx < 64 && ty < 64) {
                        var tile = _tile.MPD_File.Tiles[tx, ty];
                        tile.Modified?.Invoke(tile, EventArgs.Empty);
                    }
                }
            }
        }

        private void RecursivelyMakeEnterKeyMoveToNextControl(Control control) {
            foreach (var cObj in control.Controls) {
                if (cObj is not Control c)
                    continue;

                c.KeyUp += (sender, e) => {
                    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                        SelectNextControl(c, true, true, true, true);
                };

                RecursivelyMakeEnterKeyMoveToNextControl(c);
            }
        }

        private void RoundToNearest16th(object sender, CancelEventArgs e) {
            var nudControl = (NumericUpDown) sender;
            nudControl.Value = Math.Floor(nudControl.Value * 16) / 16;
        }

        private void EnforceRoundingToNearest16th(NumericUpDown nud) {
            nud.Validating += RoundToNearest16th;
        }

        private int _nonUserInputGuard = 0;
        private ScopeGuard IncrementNonUserInputGuard()
            => new ScopeGuard(() => _nonUserInputGuard++, () => _nonUserInputGuard--);

        void DoIfUserInput(Action action) {
            if (_nonUserInputGuard > 0)
                return;
            action();
        }

        private void UserSetMoveHeight(float value) {
            if (_nonUserInputGuard > 0)
                return;

            value = Math.Clamp(value, 0.00f, 15.9375f);
            var diff = value - _tile.MoveHeight;

            foreach (var corner in Enum.GetValues<CornerType>()) {
                var height = Math.Clamp(_tile.GetMoveHeightmap(corner) + diff, 0.00f, 15.9375f);
                _nudMoveHeightmaps[corner].Value = (decimal) height;
            }
        }

        private void UserSetMoveHeightmap(CornerType corner, float value, bool adjustMoveHeight = true) {
            if (_nonUserInputGuard > 0)
                return;

            value = Math.Clamp(value, 0.00f, 15.9375f);
            using (IncrementNonUserInputGuard()) {
                var oldValue = _tile.GetMoveHeightmap(corner);
                _tile.SetMoveHeightmap(corner, value);
                if (adjustMoveHeight)
                    SetMoveHeightToAverageMoveHeight();

                if (cbLinkHeightmaps.Checked) {
                    _nudModelVertexHeightmaps[corner].Value = (decimal) value;
                    _tile.SetModelVertexHeightmap(corner, value);
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }

                if (cbLinkHeightmaps.Checked || (_tile.MPD_File.SurfaceModel != null && _tile.ModelUseMoveHeightmap))
                    UpdateAbnormalsForCorner(corner);
            }
        }

        private void UserSetModelVertexHeightmap(CornerType corner, float value) {
            if (_nonUserInputGuard > 0)
                return;

            value = Math.Clamp(value, 0.00f, 15.9375f);
            using (IncrementNonUserInputGuard()) {
                _tile.SetModelVertexHeightmap(corner, value);
                if (cbLinkHeightmaps.Checked) {
                    var oldValue = _tile.GetMoveHeightmap(corner);
                    _nudMoveHeightmaps[corner].Value = (decimal) value;
                    _tile.SetMoveHeightmap(corner, value);
                    SetMoveHeightToAverageMoveHeight();
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }

                UpdateAbnormalsForCorner(corner);
            }
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

        private readonly Dictionary<CornerType, NumericUpDown> _nudMoveHeightmaps;
        private readonly Dictionary<CornerType, NumericUpDown> _nudModelVertexHeightmaps;
    }
}
