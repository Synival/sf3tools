using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.Types;
using SF3.Win.App;

namespace SF3.Win.Controls {
    public partial class SurfaceTilePropertiesControl : SurfaceTilePropertiesControlBase {
        public SurfaceTilePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            _nudVertexHeights = new Dictionary<CornerType, NumericUpDown>() {
                { CornerType.TopLeft,     nudMoveHeightmapTL },
                { CornerType.TopRight,    nudMoveHeightmapTR },
                { CornerType.BottomRight, nudMoveHeightmapBR },
                { CornerType.BottomLeft,  nudMoveHeightmapBL },
            };

            // Set up combo box values.
            cbMoveTerrain.DataSource = Enum.GetValues<TerrainType>();
            cbModelRotate.DataSource = Enum.GetValues<TextureRotateType>();
            cbModelFlip.DataSource   = Enum.GetValues<TextureFlipType>();

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.TerrainType = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveCenterHeight.ValueChanged   += (s, e) => DoOnlyDirectly(() => SetCenterHeight((byte) nudMoveCenterHeight.Value));
            cbMoveSlope.CheckedChanged         += (s, e) => DoOnlyDirectly(() => EditingObject.TerrainFlags ^= TerrainFlags.SteepSlope);

            foreach (var nud in _nudVertexHeights)
                nud.Value.ValueChanged += (s, e) => DoOnlyDirectly(() => SetVertexHeight(nud.Key, (byte) nud.Value.Value));

            // Event handling for 'Event' group.
            nudEventID.ValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.EventID = (byte) nudEventID.Value);

            // Event handling for 'Model' group.
            nudModelTextureID.ValueChanged     += (s, e) => DoOnlyDirectly(() => EditingObject.TextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.TextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged   += (s, e) => DoOnlyDirectly(() => EditingObject.TextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelTileIsFlat.CheckedChanged   += (s, e) => DoOnlyDirectly(() => SetIsFlat(cbModelTileIsFlat.Checked));
            cbModelHasTree.CheckedChanged      += (s, e) => DoOnlyDirectly(() => SetHasTree(cbModelHasTree.Checked));
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            // 'Current Tile' group
            labelTileEdited.Text = "Tile: (" + EditingObject.X + ", " + EditingObject.Y + ")\n";

            var heightTerrainAddress = 0x060B6000 + (EditingObject.Y * 64 + EditingObject.X) * 2;
            var eventIdAddress       = 0x060B8000 + (EditingObject.Y * 64 + EditingObject.X);

            labelRealCoordinates.Text =
                $"Center World Pos: (0x{(EditingObject.X * 32 + 16):X3}), 0x{(EditingObject.Y * 32 + 16):X3})\n" +
                "Height/Terrain Address: 0x" + heightTerrainAddress.ToString("X8") + "\n" +
                "Event ID Address: 0x" + eventIdAddress.ToString("X8");

            // 'Movement' group
            cbMoveTerrain.SelectedItem = EditingObject.TerrainType;
            InitNUD(nudMoveCenterHeight, (decimal) EditingObject.CenterHeight);
            cbMoveSlope.Checked = ((EditingObject.TerrainFlags & TerrainFlags.SteepSlope) != 0) ? true : false;
            foreach (var nud in _nudVertexHeights)
                InitNUD(nud.Value, (decimal) EditingObject.GetVertexHeight(nud.Key));

            // 'Event' group
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

        private void SetCenterHeight(byte value) {
            var diff = value - EditingObject.CenterHeight;
            var heights = EditingObject.GetVertexHeights()
                .Select(x => (byte) Math.Clamp(x + diff, 0, 255))
                .ToArray();

            EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
            EditingObject.SetVertexHeights(heights);
            UpdateVertexHeights();
        }

        private void SetVertexHeight(CornerType corner, byte value) {
            EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
            EditingObject.SetVertexHeight(corner, value);
            UpdateVertexHeights();
        }

        private void SetIsFlat(bool value) {
            EditingObject.Surface.NormalSettings = AppState.RetrieveAppState().MakeNormalCalculationSettings();
            EditingObject.IsFlat = cbModelTileIsFlat.Checked;
            UpdateVertexHeights();
            UpdateVertexHeightsEnabled();
        }

        private void SetHasTree(bool value) {
            // This only really applies to MPD_File tiles. We have a better way otherwise.
            var fileTile = EditingObject as SurfaceTile;
            if (fileTile == null)
                return;

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
