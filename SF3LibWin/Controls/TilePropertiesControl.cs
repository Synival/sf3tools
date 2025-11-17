using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.Types;
using SF3.Win.Extensions;
using static SF3.Win.Utils.EventHandlers;

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

            // Enforce validation on height-related NumericUpDown's to round to nearest 16th.
            var heightNumericUpDowns = new List<NumericUpDown>() { nudMoveHeight };
            heightNumericUpDowns.AddRange(_nudMoveHeightmaps.Select(x => x.Value).ToList());
            foreach (var nc in heightNumericUpDowns)
                EnforceRoundingToNearest16th(nc);

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged         += (s, e) => DoIfUserInput(() => _tile.MoveTerrainType = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveHeight.ValueChanged                 += (s, e) => UserSetMoveHeight((float) nudMoveHeight.Value);
            cbMoveSlope.CheckedChanged                 += (s, e) => DoIfUserInput(() => _tile.MoveTerrainFlags ^= TerrainFlags.SteepSlope);
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

            cbModelTileIsFlat.CheckedChanged           += (s, e) => DoIfUserInput(() => {
                _tile.ModelIsFlat = cbModelTileIsFlat.Checked;
                UpdateMoveHeightsEnabled();
                if (_tile.ModelIsFlat)
                    FlattenTile();
                else
                    UnflattenTile();
            });

            cbModelHasTree.CheckedChanged += (s, e) => DoIfUserInput(() => {
                using (IncrementNonUserInputGuard()) {
                    if (cbModelHasTree.Checked) {
                        if (!_tile.AdoptTree())
                            cbModelHasTree.Checked = false;
                        else
                            _tile.MPD_File.ModelsUpdated?.Invoke(this, EventArgs.Empty);
                    }
                    else {
                        if (!_tile.OrphanTree())
                            cbModelHasTree.Checked = true;
                        else
                            _tile.MPD_File.ModelsUpdated?.Invoke(this, EventArgs.Empty);
                    }
                }
            });

            // Update enabled status, visibility, and default values of controls.
            UpdateControls();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
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
                    labelTileEdited.Text = "No Tile Selected";
                    labelRealCoordinates.Text = "";
                }
                else {
                    labelTileEdited.Text = "Tile: (" + _tile.X + ", " + _tile.Y + ")\n";

                    var heightTerrainAddress = 0x060B6000 + (_tile.Y * 64 + _tile.X) * 2;
                    var eventIdAddress       = 0x060B8000 + (_tile.Y * 64 + _tile.X);

                    labelRealCoordinates.Text =
                        "Center Real Coordinates: (" + (_tile.X * 32 + 16) + "," + (_tile.Y * 32 + 16) + ")\n" +
                        "Height/Terrain Address: 0x" + heightTerrainAddress.ToString("X8") + "\n" +
                        "Event ID Address: 0x" + eventIdAddress.ToString("X8");
                }

                // 'Movement' group
                gbMovement.Enabled = _tile != null;
                if (!gbMovement.Enabled) {
                    cbMoveTerrain.SelectedItem = null;
                    nudMoveHeight.Text = "";
                    cbMoveSlope.Checked = false;
                    foreach (var nud in _nudMoveHeightmaps.Values)
                        nud.Text = "";
                }
                else {
                    cbMoveTerrain.SelectedItem = _tile.MoveTerrainType;
                    InitNUD(nudMoveHeight, (decimal) _tile.MoveHeight);
                    cbMoveSlope.Checked = ((_tile.MoveTerrainFlags & TerrainFlags.SteepSlope) != 0) ? true : false;
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
                    cbModelHasTree.Checked = false;
                    cbModelHasTree.Enabled = false;
                    cbModelRotate.SelectedItem = null;
                    cbModelRotate.Text = "";
                    cbModelRotate.Enabled = true;
                    cbModelFlip.SelectedItem = null;

                    if (cbModelTileIsFlat.Checked != false) {
                        cbModelTileIsFlat.Checked = false;
                        UpdateMoveHeightsEnabled();
                    }
                }
                else {
                    InitNUD(nudModelTextureID, _tile.ModelTextureID);
                    cbModelHasTree.Checked = _tile.TreeModelID != null;
                    cbModelHasTree.Enabled = true;

                    var disabledMessage = _tile.MPD_File.Scenario >= ScenarioType.Scenario3 ? "(Enable in header)" : "(Scenario 3+ only)";
                    if (_tile.MPD_File.SurfaceModel.TileTextureRowTable.HasRotation && _tile.MPD_File.MPDFlags.HasSurfaceTextureRotation) {
                        cbModelRotate.SelectedItem = _tile.ModelTextureRotate;
                        cbModelRotate.Enabled = true;
                    }
                    else {
                        cbModelRotate.SelectedItem = -1;
                        cbModelRotate.Text = disabledMessage;
                        cbModelRotate.Enabled = false;
                    }

                    cbModelFlip.SelectedItem = _tile.ModelTextureFlip;

                    if (cbModelTileIsFlat.Checked != _tile.ModelIsFlat) {
                        cbModelTileIsFlat.Checked = _tile.ModelIsFlat;
                        UpdateMoveHeightsEnabled();
                    }
                }
            }
        }

        private void SetMoveHeightToAverageMoveHeight() {
            using (IncrementNonUserInputGuard()) {
                var avgHeight = _tile.GetAverageHeight();
                nudMoveHeight.Value = (decimal) avgHeight;
                _tile.MoveHeight = avgHeight;
            }
        }

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        private Dictionary<NumericUpDown, bool> _nudSelectAll = [];

        private void RecursivelyAttachedEventsToControls(Control control) {
            foreach (var cObj in control.Controls) {
                if (cObj is not Control c)
                    continue;

                // Pressing 'Enter' should move on to the next control.
                c.KeyUp += (s, e) => {
                    if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                        SelectNextControl(c, true, true, true, true);
                };

                // Selecting the control should highlight all text.
                if (c is NumericUpDown nud) {
                    // The NumericUpDown is frustrating. The 'enter' event *on tab* will select all the text, but not when clicking it.
                    // The selection is probably overridden on click for some reason. So: select text always, and on MouseDown (which
                    // happens after 'Enter'), select text again. If the 'KeyUp' event is received (which happens after 'Tab'), disrecard
                    // the '_nudSelectAll[nud]' flag.
                    nud.Enter += (s, e) => {
                        _nudSelectAll[nud] = true;
                        nud.Select(0, nud.Text.Length);
                    };
                    nud.KeyUp += (s, e) => _nudSelectAll[nud] = false;
                    nud.MouseDown += (s, e) => {
                        if (_nudSelectAll.TryGetValue(nud, out bool doSelectAll)) {
                            if (doSelectAll) {
                                nud.Select(0, nud.Text.Length);
                                _nudSelectAll[nud] = false;
                            }
                        }
                    };
                }
                if (c is TextBox tb)
                    tb.Enter += (s, e) => tb.SelectAll();
                else if (c is ComboBox cb) {
                    cb.Enter += (s, e) => cb.Select(0, cb.Text.Length);

                    // Hit 'enter' before leaving a combo box to select whatever item the highlighted text was referring to
                    cb.LostFocus += (s, e) => {
                        const int WM_KEYDOWN = 0x0100;
                        const int VK_RETURN  = 0x0D;
                        SendMessage(cb.Handle, WM_KEYDOWN, VK_RETURN, 0);
                    };
                }
                else
                    RecursivelyAttachedEventsToControls(c);
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

                if (_tile.MPD_File.SurfaceModel != null && !_tile.ModelIsFlat) {
                    _tile.SetModelVertexHeightmap(corner, value);
                    _tile.CopyMoveHeightToNonFlatNeighbors(corner);

                    // Technically we *could* recalculate normals because the normals of neighboring flat tiles are also included
                    // in the calculations, but the normals of flat tiles are constant, so neighboring tiles' normals never change
                    // when a flat tile moves up or down.
                    _tile.UpdateNormalsForCorner(corner, AppState.RetrieveAppState().MakeNormalCalculationSettings());
                }
            }
        }

        private void UpdateMoveHeightsEnabled() {
            var isFlat = cbModelTileIsFlat.Checked;
            var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            foreach (var corner in corners)
                _nudMoveHeightmaps[corner].Enabled = !isFlat;
        }

        private void FlattenTile() {
            using (IncrementNonUserInputGuard()) {
                var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));
                var height = _tile.MoveHeight;
                foreach (var nud in _nudMoveHeightmaps) {
                    _tile.SetMoveHeightmap(nud.Key, height);
                    nud.Value.Value = (decimal) height;
                }
                foreach (var corner in corners)
                    _tile.UpdateNormalsForCorner(corner, AppState.RetrieveAppState().MakeNormalCalculationSettings());
            }
        }

        private void UnflattenTile() {
            using (IncrementNonUserInputGuard()) {
                var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));
                foreach (var nud in _nudMoveHeightmaps) {
                    nud.Value.Enabled = true;
                    var height = _tile.GetMoveHeightmap(nud.Key);
                    _tile.SetModelVertexHeightmap(nud.Key, height);
                    _tile.CopyMoveHeightToNonFlatNeighbors(nud.Key);
                }
                foreach (var corner in corners)
                    _tile.UpdateNormalsForCorner(corner, AppState.RetrieveAppState().MakeNormalCalculationSettings());
            }
        }

        private Tile _tile = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Tile Tile {
            get => _tile;
            set {
                if (value == _tile)
                    return;

                Control lastFocused = null;
                if (ContainsFocus && !Focused) {
                    lastFocused = this.GetFocusedControl();
                    Focus();
                }

                _tile = value;
                UpdateControls();

                if (lastFocused != null) {
                    if (lastFocused is NumericUpDown nud)
                        nud.Select(0, nud.Text.Length);
                    else if (lastFocused is TextBox tb)
                        tb.SelectAll();
                    else if (lastFocused is ComboBox cb)
                        cb.Select(0, cb.Text.Length);
                }
            }
        }

        private readonly Dictionary<CornerType, NumericUpDown> _nudMoveHeightmaps;

        public event CmdKeyEventHandler CmdKey;
    }
}
