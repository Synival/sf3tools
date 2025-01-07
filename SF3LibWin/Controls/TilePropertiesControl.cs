using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
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

            // Enforce rounding to the nearest 16th.
            void RoundToNearest16th(object sender) {
                var nudControl = (NumericUpDown) sender;
                nudControl.Value = Math.Floor(nudControl.Value * 16) / 16;
            }

            var heightNumericUpDowns = new List<NumericUpDown>() { nudMoveHeight };
            heightNumericUpDowns.AddRange(_nudMoveHeightmaps.Select(x => x.Value).ToList());
            heightNumericUpDowns.AddRange(_nudModelVertexHeightmaps.Select(x => x.Value).ToList());
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
            }

            void SetMoveHeight(float value) {
                value = Math.Clamp(value, 0.00f, 15.9375f);
                if (_updatingControls > 0)
                    return;

                var diff = value - _tile.MoveHeight;
                _tile.MoveHeight = value;

                foreach (var corner in Enum.GetValues<CornerType>()) {
                    var height = Math.Clamp(_tile.GetMoveHeightmap(corner) + diff, 0.00f, 15.9375f);
                    SetMoveHeightmap(corner, height, false);
                    _updatingControls++;
                    _nudMoveHeightmaps[corner].Value = (decimal) height;
                    _updatingControls--;
                }
            }

            void SetMoveHeightmap(CornerType corner, float value, bool adjustMoveHeight = true) {
                value = Math.Clamp(value, 0.00f, 15.9375f);
                if (_updatingControls > 0)
                    return;

                _updatingControls++;
                var oldValue = _tile.GetMoveHeightmap(corner);
                _tile.SetMoveHeightmap(corner, value);
                if (adjustMoveHeight)
                    SetMoveHeightToAverageMoveHeight();

                if (cbLinkHeightmaps.Checked) {
                    _nudModelVertexHeightmaps[corner].Value = (decimal) value;
                    _tile.SetModelVertexHeightmap(corner, value);
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }
                _updatingControls--;

                if (cbLinkHeightmaps.Checked || (_tile.MPD_File.SurfaceModel != null && _tile.ModelUseMoveHeightmap))
                    UpdateAbnormalsForCorner(corner);
            }

            void SetModelVertexHeightmap(CornerType corner, float value) {
                value = Math.Clamp(value, 0.00f, 15.9375f);
                if (_updatingControls > 0)
                    return;

                _updatingControls++;
                _tile.SetModelVertexHeightmap(corner, value);
                if (cbLinkHeightmaps.Checked) {
                    var oldValue = _tile.GetMoveHeightmap(corner);
                    _nudMoveHeightmaps[corner].Value = (decimal) value;
                    _tile.SetMoveHeightmap(corner, value);
                    SetMoveHeightToAverageMoveHeight();
                    UpdateNeighboringIdenticalMoveHeightmapValues(corner, oldValue, value);
                }
                _updatingControls--;

                UpdateAbnormalsForCorner(corner);
            }

            cbMoveTerrain.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.MoveTerrain = (TerrainType) cbMoveTerrain.SelectedValue, false);
            nudMoveHeight.ValueChanged                 += (s, e) => PerformUpdate(() => SetMoveHeight((float) nudMoveHeight.Value), true);
            foreach (var nud in _nudMoveHeightmaps)
                nud.Value.ValueChanged += (s, e) => PerformUpdate(() => SetMoveHeightmap(nud.Key, (float) nud.Value.Value));
            nudMoveHeightmapTR.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.TopRight, (float) nudMoveHeightmapTR.Value));
            nudMoveHeightmapBL.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.BottomLeft, (float) nudMoveHeightmapBL.Value));
            nudMoveHeightmapBR.ValueChanged            += (s, e) => PerformUpdate(() => SetMoveHeightmap(CornerType.BottomRight, (float) nudMoveHeightmapBR.Value));

            nudEventID.ValueChanged                    += (s, e) => PerformUpdate(() => _tile.EventID = (byte) nudEventID.Value, false);

            nudModelTextureID.ValueChanged             += (s, e) => PerformUpdate(() => _tile.ModelTextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged         += (s, e) => PerformUpdate(() => _tile.ModelTextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged           += (s, e) => PerformUpdate(() => _tile.ModelTextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelUseMovementHeightmap.CheckedChanged += (s, e) => PerformUpdate(() => {
                _tile.ModelUseMoveHeightmap = cbModelUseMovementHeightmap.Checked;
                UpdateLinkHeightmapsCheckbox();
            });
            foreach (var nud in _nudModelVertexHeightmaps)
                nud.Value.ValueChanged += (s, e) => PerformUpdate(() => SetModelVertexHeightmap(nud.Key, (float) nud.Value.Value));

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
                UpdateLinkHeightmapsCheckbox();
            }
            else {
                labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";
                labelRealCoordinates.Text = "Real coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")";
                UpdateLinkHeightmapsCheckbox();
            }

            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
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

            _updatingControls--;
        }

        private void UpdateLinkHeightmapsCheckbox() {
            cbLinkHeightmaps.Enabled = _tile != null && _tile.MPD_File.SurfaceModel != null && !_tile.ModelUseMoveHeightmap;
            cbLinkHeightmaps.Checked = cbLinkHeightmaps.Enabled &&
                Enum.GetValues<CornerType>().All(x => _tile.GetMoveHeightmap(x) == _tile.GetModelVertexHeightmap(x));

            foreach (var nud in _nudModelVertexHeightmaps.Values)
                nud.Enabled = cbLinkHeightmaps.Enabled;
        }

        private void SetMoveHeightToAverageMoveHeight() {
            _updatingControls++;
            var avgHeight = _tile.GetAverageHeight();
            nudMoveHeight.Value = (decimal) avgHeight;
            _tile.MoveHeight = avgHeight;
            _updatingControls--;
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
