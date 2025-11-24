using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib;
using CommonLib.Types;
using SF3.Extensions;
using SF3.Models.Files.MPD;
using SF3.MPD;
using SF3.Types;
using SF3.Win.Extensions;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public partial class TilePropertiesControl : UserControl {
        public TilePropertiesControl() {
            InitializeComponent();

            _nudVisibleVertexHeights = new Dictionary<CornerType, NumericUpDown>() {
                { CornerType.TopLeft,     nudMoveHeightmapTL },
                { CornerType.TopRight,    nudMoveHeightmapTR },
                { CornerType.BottomRight, nudMoveHeightmapBR },
                { CornerType.BottomLeft,  nudMoveHeightmapBL },
            };

            // Enforce validation on height-related NumericUpDown's to round to nearest 16th.
            var heightNumericUpDowns = new List<NumericUpDown>() { nudMoveCenterHeight };
            heightNumericUpDowns.AddRange(_nudVisibleVertexHeights.Select(x => x.Value).ToList());
            foreach (var nc in heightNumericUpDowns)
                EnforceRoundingToNearest16th(nc);

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged += (s, e) => DoIfUserInput(() => _tile.TerrainType = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveCenterHeight.ValueChanged   += (s, e) => UserSetCenterHeight((float) nudMoveCenterHeight.Value);
            cbMoveSlope.CheckedChanged         += (s, e) => DoIfUserInput(() => _tile.TerrainFlags ^= TerrainFlags.SteepSlope);

            foreach (var nud in _nudVisibleVertexHeights)
                nud.Value.ValueChanged += (s, e) => UserSetVisualVertexHeight(nud.Key, (float) nud.Value.Value);

            // Event handling for 'Event' group.
            nudEventID.ValueChanged += (s, e) => DoIfUserInput(() => _tile.EventID = (byte) nudEventID.Value);

            // Event handling for 'Model' group.
            nudModelTextureID.ValueChanged     += (s, e) => DoIfUserInput(() => _tile.TextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged += (s, e) => DoIfUserInput(() => _tile.TextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged   += (s, e) => DoIfUserInput(() => _tile.TextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);

            cbModelTileIsFlat.CheckedChanged   += (s, e) => UserSetIsFlat(cbModelTileIsFlat.Checked);
            cbModelHasTree.CheckedChanged += (s, e) => DoIfUserInput(() => {
                // This only really applies to MPD_File tiles. We have a better way otherwise.
                var fileTile = _tile as Tile;
                if (fileTile == null)
                    return;

                using (IncrementNonUserInputGuard()) {
                    if (cbModelHasTree.Checked) {
                        if (!fileTile.AdoptTree())
                            cbModelHasTree.Checked = false;
                        else
                            fileTile.MPD_File.ModelsUpdated?.Invoke(this, EventArgs.Empty);
                    }
                    else {
                        if (!fileTile.OrphanTree())
                            cbModelHasTree.Checked = true;
                        else
                            fileTile.MPD_File.ModelsUpdated?.Invoke(this, EventArgs.Empty);
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
                    nudMoveCenterHeight.Text = "";
                    cbMoveSlope.Checked = false;
                    foreach (var nud in _nudVisibleVertexHeights.Values)
                        nud.Text = "";
                }
                else {
                    cbMoveTerrain.SelectedItem = _tile.TerrainType;
                    InitNUD(nudMoveCenterHeight, (decimal) _tile.CenterHeight);
                    cbMoveSlope.Checked = ((_tile.TerrainFlags & TerrainFlags.SteepSlope) != 0) ? true : false;
                    foreach (var nud in _nudVisibleVertexHeights)
                        InitNUD(nud.Value, (decimal) _tile.GetVisualVertexHeight(nud.Key));
                }

                // 'Event' group
                gbEvent.Enabled = _tile != null;
                if (!gbEvent.Enabled)
                    nudEventID.Text = "";
                else
                    InitNUD(nudEventID, _tile.EventID);

                // 'Model' group
                gbModel.Enabled = _tile?.HasModel == true;
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
                        UpdateVisibleVertexHeightsEnabled();
                    }
                }
                else {
                    var fileTile = _tile as Tile;

                    InitNUD(nudModelTextureID, _tile.TextureID);
                    cbModelHasTree.Checked = fileTile?.TreeModelID != null;
                    cbModelHasTree.Enabled = true;

                    if (_tile.HasRotatableTexture) {
                        cbModelRotate.SelectedItem = _tile.TextureRotate;
                        cbModelRotate.Enabled = true;
                    }
                    else {
                        var disabledMessage =
                            (fileTile != null && fileTile.MPD_File.Scenario >= ScenarioType.Scenario3) ? "(Enable in header)" :
                            (fileTile != null && fileTile.MPD_File.Scenario <  ScenarioType.Scenario3) ? "(Scenario 3+ only)" :
                            "(Disabled)";

                        cbModelRotate.SelectedItem = -1;
                        cbModelRotate.Text = disabledMessage;
                        cbModelRotate.Enabled = false;
                    }

                    cbModelFlip.SelectedItem = _tile.TextureFlip;

                    if (cbModelTileIsFlat.Checked != _tile.IsFlat) {
                        cbModelTileIsFlat.Checked = _tile.IsFlat;
                        UpdateVisibleVertexHeightsEnabled();
                    }
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

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

        private void UserSetCenterHeight(float value) {
            if (_nonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                var diff = value - _tile.CenterHeight;
                var heights = _tile.GetVisualVertexHeights()
                    .Select(x => Math.Clamp(x + diff, 0.00f, 15.9375f))
                    .ToArray();

                _tile.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                _tile.SetVisualVertexHeights(heights);
                UpdateVisibleVertexHeights();
            }
        }

        private void UserSetVisualVertexHeight(CornerType corner, float value) {
            if (_nonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                value = Math.Clamp(value, 0.00f, 15.9375f);
                _tile.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                _tile.SetVisualVertexHeight(corner, value);
                UpdateVisibleVertexHeights();
            }
        }

        private void UserSetIsFlat(bool value) {
            if (_nonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                _tile.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                _tile.IsFlat = cbModelTileIsFlat.Checked;
                UpdateVisibleVertexHeights();
                UpdateVisibleVertexHeightsEnabled();
            }
        }

        private void UpdateVisibleVertexHeights() {
            foreach (var corner in Enum.GetValues<CornerType>()) {
                var height = _tile.GetVisualVertexHeight(corner);
                _nudVisibleVertexHeights[corner].Value = (decimal) height;
            }
            nudMoveCenterHeight.Value = (decimal) _tile.CenterHeight;
        }

        private void UpdateVisibleVertexHeightsEnabled() {
            var isFlat = cbModelTileIsFlat.Checked;
            var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            foreach (var corner in corners)
                _nudVisibleVertexHeights[corner].Enabled = !isFlat;
        }

        private IMPD_Tile _tile = null;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD_Tile Tile {
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

        public event CmdKeyEventHandler CmdKey;

        private readonly Dictionary<CornerType, NumericUpDown> _nudVisibleVertexHeights;
    }
}
