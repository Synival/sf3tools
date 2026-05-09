using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Types;
using SF3.Models.Files.MPD;
using SF3.MPD.Interfaces;
using SF3.Types;
using SF3.Win.App;
using SF3.MPD.Extensions;

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

            void DoOnlyDirectly(Action<IMPD_SurfaceTile> action) {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                this.DoOnlyDirectly(() => action(eo));
            }

            // Event handling for 'Movement' group.
            cbMoveTerrain.SelectedValueChanged += (s, e) => DoOnlyDirectly(eo => eo.TerrainType = (TerrainType) cbMoveTerrain.SelectedValue);
            nudMoveCenterHeight.ValueChanged   += (s, e) => DoOnlyDirectly(eo => SetCenterHeight((byte) nudMoveCenterHeight.Value));
            cbMoveSlope.CheckedChanged         += (s, e) => DoOnlyDirectly(eo => eo.TerrainFlags ^= TerrainFlags.IgnoreHeightCost);

            foreach (var nud in _nudVertexHeights)
                nud.Value.ValueChanged += (s, e) => DoOnlyDirectly(eo => SetVertexHeight(nud.Key, (byte) nud.Value.Value));

            // Event handling for 'Event' group.
            nudEventID.ValueChanged            += (s, e) => DoOnlyDirectly(eo => eo.EventID = (byte) nudEventID.Value);

            // Event handling for 'Model' group.
            nudModelTextureID.ValueChanged     += (s, e) => DoOnlyDirectly(eo => eo.TextureID = (byte) nudModelTextureID.Value);
            cbModelRotate.SelectedValueChanged += (s, e) => DoOnlyDirectly(eo => eo.TextureRotate = (TextureRotateType) cbModelRotate.SelectedValue);
            cbModelFlip.SelectedValueChanged   += (s, e) => DoOnlyDirectly(eo => eo.TextureFlip = (TextureFlipType) cbModelFlip.SelectedValue);
            cbModelTileIsFlat.CheckedChanged   += (s, e) => DoOnlyDirectly(eo => SetIsFlat(cbModelTileIsFlat.Checked));
            cbModelHasTree.CheckedChanged      += (s, e) => DoOnlyDirectly(eo => SetHasTree(cbModelHasTree.Checked));
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            // 'Current Tile' group
            labelTileEdited.Text = "Tile: (" + eo.X + ", " + eo.Y + ")\n";

            var heightTerrainAddress = 0x060B6000 + (eo.Y * 64 + eo.X) * 2;
            var eventIdAddress       = 0x060B8000 + (eo.Y * 64 + eo.X);

            labelRealCoordinates.Text =
                $"Center World Pos: (0x{(eo.X * 32 + 16):X3}), 0x{(eo.Y * 32 + 16):X3})\n" +
                "Height/Terrain Address: 0x" + heightTerrainAddress.ToString("X8") + "\n" +
                "Event ID Address: 0x" + eventIdAddress.ToString("X8");

            // 'Movement' group
            cbMoveTerrain.SelectedItem = eo.TerrainType;
            SetNudValueAndText(nudMoveCenterHeight, (decimal) eo.CenterHeight);
            cbMoveSlope.Checked = ((eo.TerrainFlags & TerrainFlags.IgnoreHeightCost) != 0) ? true : false;
            foreach (var nud in _nudVertexHeights)
                SetNudValueAndText(nud.Value, (decimal) eo.GetVertexHeight(nud.Key));

            // 'Event' group
            SetNudValueAndText(nudEventID, eo.EventID);

            // 'Model' group
            gbModel.Enabled = eo?.Surface?.HasModel == true;
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
                var fileTile = eo as SurfaceTile;

                SetNudValueAndText(nudModelTextureID, eo.TextureID);
                cbModelHasTree.Checked = fileTile?.TreeModelID != null;
                cbModelHasTree.Enabled = true;

                if (eo?.Surface?.HasRotatableTextures == true) {
                    cbModelRotate.SelectedItem = eo.TextureRotate;
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

                cbModelFlip.SelectedItem = eo.TextureFlip;

                if (cbModelTileIsFlat.Checked != eo.IsFlat) {
                    cbModelTileIsFlat.Checked = eo.IsFlat;
                    UpdateVertexHeightsEnabled();
                }
            }
        }

        private void SetCenterHeight(byte value) {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            var diff = value - eo.CenterHeight;
            var heights = eo.GetVertexHeights()
                .Select(x => (byte) Math.Clamp(x + diff, 0, 255))
                .ToArray();

            eo.Surface.NormalSettings = AppSettings.Get().MakeNormalCalculationSettings();
            eo.SetVertexHeights(heights);
            UpdateVertexHeights();
        }

        private void SetVertexHeight(CornerType corner, byte value) {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            eo.Surface.NormalSettings = AppSettings.Get().MakeNormalCalculationSettings();
            eo.SetVertexHeight(corner, value);
            UpdateVertexHeights();
        }

        private void SetIsFlat(bool value) {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            eo.Surface.NormalSettings = AppSettings.Get().MakeNormalCalculationSettings();
            eo.SetFlatAndUpdateHeights(cbModelTileIsFlat.Checked);
            UpdateVertexHeights();
            UpdateVertexHeightsEnabled();
        }

        private void SetHasTree(bool value) {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            // This only really applies to MPD_File tiles. We have a better way otherwise.
            var fileTile = eo as SurfaceTile;
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
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            foreach (var corner in Enum.GetValues<CornerType>()) {
                var height = eo.GetVertexHeight(corner);
                _nudVertexHeights[corner].Value = (decimal) height;
            }
            nudMoveCenterHeight.Value = (decimal) eo.CenterHeight;
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
