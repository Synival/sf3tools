using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.App;

namespace SF3.Win.Controls {
    public partial class SurfaceTilePropertiesControl : PropertiesControlBase<IMPD_SurfaceTile> {
        public SurfaceTilePropertiesControl() {
            InitializeComponent();

            _nudVertexHeights = new Dictionary<CornerType, NumericUpDown>() {
                { CornerType.TopLeft,     nudMoveHeightmapTL },
                { CornerType.TopRight,    nudMoveHeightmapTR },
                { CornerType.BottomRight, nudMoveHeightmapBR },
                { CornerType.BottomLeft,  nudMoveHeightmapBL },
            };

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged += (s, e) => DoIfUserInput(() => EditingObject.TerrainType = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveCenterHeight.ValueChanged   += (s, e) => UserSetCenterHeight((byte) nudMoveCenterHeight.Value);
            cbMoveSlope.CheckedChanged         += (s, e) => DoIfUserInput(() => EditingObject.TerrainFlags ^= TerrainFlags.SteepSlope);

            foreach (var nud in _nudVertexHeights)
                nud.Value.ValueChanged += (s, e) => UserSetVertexHeight(nud.Key, (byte) nud.Value.Value);

            // Event handling for 'Event' group.
            nudEventID.ValueChanged += (s, e) => DoIfUserInput(() => EditingObject.EventID = (byte) nudEventID.Value);

            // Event handling for 'Model' group.
            nudModelTextureID.ValueChanged     += (s, e) => DoIfUserInput(() => EditingObject.TextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged += (s, e) => DoIfUserInput(() => EditingObject.TextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged   += (s, e) => DoIfUserInput(() => EditingObject.TextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelTileIsFlat.CheckedChanged   += (s, e) => UserSetIsFlat(cbModelTileIsFlat.Checked);
            cbModelHasTree.CheckedChanged      += (s, e) => UserSetHasTree(cbModelHasTree.Checked);

            // Update enabled status, visibility, and default values of controls.
            UpdateControls();
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            // 'Current Tile' group
            if (EditingObject == null) {
                labelTileEdited.Text = "No Tile Selected";
                labelRealCoordinates.Text = "";
            }
            else {
                labelTileEdited.Text = "Tile: (" + EditingObject.X + ", " + EditingObject.Y + ")\n";

                var heightTerrainAddress = 0x060B6000 + (EditingObject.Y * 64 + EditingObject.X) * 2;
                var eventIdAddress       = 0x060B8000 + (EditingObject.Y * 64 + EditingObject.X);

                labelRealCoordinates.Text =
                    "Center Real Coordinates: (" + (EditingObject.X * 32 + 16) + "," + (EditingObject.Y * 32 + 16) + ")\n" +
                    "Height/Terrain Address: 0x" + heightTerrainAddress.ToString("X8") + "\n" +
                    "Event ID Address: 0x" + eventIdAddress.ToString("X8");
            }

            // 'Movement' group
            gbMovement.Enabled = EditingObject != null;
            if (!gbMovement.Enabled) {
                cbMoveTerrain.SelectedItem = null;
                nudMoveCenterHeight.Text = "";
                cbMoveSlope.Checked = false;
                foreach (var nud in _nudVertexHeights.Values)
                    nud.Text = "";
            }
            else {
                cbMoveTerrain.SelectedItem = EditingObject.TerrainType;
                InitNUD(nudMoveCenterHeight, (decimal) EditingObject.CenterHeight);
                cbMoveSlope.Checked = ((EditingObject.TerrainFlags & TerrainFlags.SteepSlope) != 0) ? true : false;
                foreach (var nud in _nudVertexHeights)
                    InitNUD(nud.Value, (decimal) EditingObject.GetVertexHeight(nud.Key));
            }

            // 'Event' group
            gbEvent.Enabled = EditingObject != null;
            if (!gbEvent.Enabled)
                nudEventID.Text = "";
            else
                InitNUD(nudEventID, EditingObject.EventID);

            // 'Model' group
            gbModel.Enabled = EditingObject?.Surface?.HasModel == true;
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
                    UpdateVertexHeightsEnabled();
                }
            }
            else {
                var fileTile = EditingObject as SurfaceTile;

                InitNUD(nudModelTextureID, EditingObject.TextureID);
                cbModelHasTree.Checked = fileTile?.TreeModelID != null;
                cbModelHasTree.Enabled = true;

                if (EditingObject?.Surface?.HasRotatableTextures == true) {
                    cbModelRotate.SelectedItem = EditingObject.TextureRotate;
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

                cbModelFlip.SelectedItem = EditingObject.TextureFlip;

                if (cbModelTileIsFlat.Checked != EditingObject.IsFlat) {
                    cbModelTileIsFlat.Checked = EditingObject.IsFlat;
                    UpdateVertexHeightsEnabled();
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

        private void UserSetCenterHeight(byte value) {
            if (NonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                var diff = value - EditingObject.CenterHeight;
                var heights = EditingObject.GetVertexHeights()
                    .Select(x => (byte) Math.Clamp(x + diff, 0, 255))
                    .ToArray();

                EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                EditingObject.SetVertexHeights(heights);
                UpdateVertexHeights();
            }
        }

        private void UserSetVertexHeight(CornerType corner, byte value) {
            if (NonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                EditingObject.SetVertexHeight(corner, value);
                UpdateVertexHeights();
            }
        }

        private void UserSetIsFlat(bool value) {
            if (NonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
                EditingObject.IsFlat = cbModelTileIsFlat.Checked;
                UpdateVertexHeights();
                UpdateVertexHeightsEnabled();
            }
        }

        private void UserSetHasTree(bool value) {
            if (NonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                // This only really applies to MPD_File tiles. We have a better way otherwise.
                var fileTile = EditingObject as SurfaceTile;
                if (fileTile == null)
                    return;

                using (IncrementNonUserInputGuard()) {
                    if (value) {
                        if (!fileTile.AdoptTree())
                            cbModelHasTree.Checked = false;
                        else
                            fileTile.MPD_File.TriggerModelsUpdated();
                    }
                    else {
                        if (!fileTile.OrphanTree())
                            cbModelHasTree.Checked = true;
                        else
                            fileTile.MPD_File.TriggerModelsUpdated();
                    }
                }
            }
        }

        private void UpdateVertexHeights() {
            foreach (var corner in Enum.GetValues<CornerType>()) {
                var height = EditingObject.GetVertexHeight(corner);
                _nudVertexHeights[corner].Value = (decimal) height;
            }
            nudMoveCenterHeight.Value = (decimal) EditingObject.CenterHeight;
        }

        private void UpdateVertexHeightsEnabled() {
            var isFlat = cbModelTileIsFlat.Checked;
            var corners = (CornerType[]) Enum.GetValues(typeof(CornerType));
            foreach (var corner in corners)
                _nudVertexHeights[corner].Enabled = !isFlat;
        }

        private readonly Dictionary<CornerType, NumericUpDown> _nudVertexHeights;
    }
}
