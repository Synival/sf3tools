using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.SGL;
using CommonLib.Utils;
using SF3.MPD.Interfaces;
using SF3.Types;
using static CommonLib.Extensions.VECTOR_Extensions;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : ModelInstancePropertiesControlBase {
        public ModelInstancePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbVisibleFrom.DataSource = Enum.GetValues<ModelDirectionType>().Select(x => OnlyVisibleFromToString(x)).ToArray();

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action<ISGL_ModelInstance> action) {
                DoOnlyDirectly(() => {
                    // TODO: support multiple selection!
                    var eo = EditingObjects[0];
                    action(eo);
                    Viewer?.GLControl?.InvalidateModels();
                });
            }

            // ---------------------------------------
            // World coordinates
            // ---------------------------------------

            nudXWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionX = (short) -nudXWorld.Value;
                UpdateCoordinateValues(eo);
            });

            nudYWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionY = (short) -nudYWorld.Value;
                UpdateCoordinateValues(eo);
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionZ = (short) -nudZWorld.Value;
                UpdateCoordinateValues(eo);
            });

            // ---------------------------------------
            // Grid coordinates
            // ---------------------------------------

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionX = (short) -(nudX.Value * 32);
                UpdateCoordinateValues(eo);
            });

            nudY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionY = (short) -((float) nudY.Value * 2);
                UpdateCoordinateValues(eo);
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionZ = (short) -(nudZ.Value * 32);
                UpdateCoordinateValues(eo);
            });

            // ---------------------------------------
            // Rotation
            // ---------------------------------------

            float WrapAngleValue(NumericUpDown nud) {
                var value = MathHelpers.ActualMod((float) nud.Value + 180.0f, 360.0f) - 180.0f;
                if ((float) nud.Value != value)
                    nud.Value = (decimal) value;
                return value;
            }

            nudRotationX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.AngleX = WrapAngleValue(nudRotationX);
                UpdateCoordinateValues(eo);
            });

            nudRotationY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.AngleY = WrapAngleValue(nudRotationY);
                UpdateCoordinateValues(eo);
            });

            nudRotationZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.AngleZ = WrapAngleValue(nudRotationZ);
                UpdateCoordinateValues(eo);
            });

            // ---------------------------------------
            // Scale
            // ---------------------------------------

            nudScaleX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ScaleX = (float) nudScaleX.Value / 100.0f;
                UpdateCoordinateValues(eo);
            });

            nudScaleY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ScaleY = (float) nudScaleY.Value / 100.0f;
                UpdateCoordinateValues(eo);
            });

            nudScaleZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ScaleZ = (float) nudScaleZ.Value / 100.0f;
                UpdateCoordinateValues(eo);
            });

            // ---------------------------------------
            // Flags
            // ---------------------------------------

            cbAlwaysFaceCamera.CheckedChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo =>
                eo.AlwaysFacesCamera = cbAlwaysFaceCamera.Checked);

            cbVisibleFrom.SelectedValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                if (eo is IMPD_ModelInstance mpdModelInst) {
                    var dir = StringToOnlyVisibleFrom(cbVisibleFrom.Text);
                    if (!dir.HasValue)
                        cbVisibleFrom.Text = OnlyVisibleFromToString(mpdModelInst.OnlyVisibleFromDirection);
                    else
                        mpdModelInst.OnlyVisibleFromDirection = dir.Value;
                }
            });

            // ---------------------------------------
            // Bounding box coordinates
            // ---------------------------------------

            nudWest.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionX = (short) -Math.Round((float) nudWest.Value * 32 + eo.BoundingBox.RightBottomBack.X.Float);
                UpdateCoordinateValues(eo);
            });

            nudEast.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionX = (short) -Math.Round((float) nudEast.Value * 32 + eo.BoundingBox.LeftTopFront.X.Float);
                UpdateCoordinateValues(eo);
            });

            nudTop.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionY = (short) -Math.Round((float) nudTop.Value * 2 + eo.BoundingBox.LeftTopFront.Y.Float);
                UpdateCoordinateValues(eo);
            });

            nudBottom.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionY = (short) -Math.Round((float) nudBottom.Value * 2 + eo.BoundingBox.RightBottomBack.Y.Float);
                UpdateCoordinateValues(eo);
            });

            nudSouth.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionZ = (short) -Math.Round((float) nudSouth.Value * 32 + eo.BoundingBox.RightBottomBack.Z.Float);
                UpdateCoordinateValues(eo);
            });

            nudNorth.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.PositionZ = (short) -Math.Round((float) nudNorth.Value * 32 + eo.BoundingBox.LeftTopFront.Z.Float);
                UpdateCoordinateValues(eo);
            });

            // ---------------------------------------
            // Bounding box size
            // ---------------------------------------

            void TransformSize(TransformSizeDelegate transformer) {
                DoOnlyDirectlyAndInvalidate(eo => {
                    var bounds = eo.BoundingBox;
                    transformer(ref bounds);

                    var updatedModelBounds = bounds.ToVECTORs().UnrotateXYZ(eo.AngleX, eo.AngleY, eo.AngleZ).CreateBoundingBox();
                    var originalModelBounds = eo.GetModel(0).Vertices.ToArray().CreateBoundingBox();

                    if (originalModelBounds.Width > 0.01f)
                        eo.ScaleX = Math.Max(0.01f, updatedModelBounds.Width  / originalModelBounds.Width);
                    if (originalModelBounds.Height > 0.01f)
                        eo.ScaleY = Math.Max(0.01f, updatedModelBounds.Height / originalModelBounds.Height);
                    if (originalModelBounds.Depth > 0.01f)
                        eo.ScaleZ = Math.Max(0.01f, updatedModelBounds.Depth  / originalModelBounds.Depth);

                    UpdateCoordinateValues(eo);
                });
            };

            void TransformSizeX(ref BoundingBox bounds) => bounds.Width  = (float) nudSizeX.Value * 32.0f;
            void TransformSizeY(ref BoundingBox bounds) => bounds.Height = (float) nudSizeY.Value *  2.0f;
            void TransformSizeZ(ref BoundingBox bounds) => bounds.Depth  = (float) nudSizeZ.Value * 32.0f;

            nudSizeX.ValueChanged += (s, e) => TransformSize(TransformSizeX);
            nudSizeY.ValueChanged += (s, e) => TransformSize(TransformSizeY);
            nudSizeZ.ValueChanged += (s, e) => TransformSize(TransformSizeZ);

            // ---------------------------------------
            // Model ID
            // ---------------------------------------

            nudModelID.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                // Try to set the model to a valid one. If this doesn't work, set it back to the original value.
                ISGL_Model model = null;
                try {
                    eo.ModelID = (int) nudModelID.Value;
                    model = eo.GetModel(0);
                }
                catch {
                    model = null;
                }
                if (model == null) {
                    SetNudValueAndText(nudModelID, eo.ModelID);
                    return;
                }

                UpdateCoordinateValues(eo);
            });
        }

        private delegate void TransformSizeDelegate(ref BoundingBox bounds);

        private void UpdateCoordinateValues(ISGL_ModelInstance eo) {
            var posX = eo.PositionX;
            var posY = eo.PositionY;
            var posZ = eo.PositionZ;

            nudXWorld.Value = -posX;
            nudYWorld.Value = -posY;
            nudZWorld.Value = -posZ;

            nudX.Value = (decimal) (-posX / 32.0f);
            nudY.Value = (decimal) (-posY /  2.0f);
            nudZ.Value = (decimal) (-posZ / 32.0f);

            nudRotationX.Value = (decimal) eo.AngleX;
            nudRotationY.Value = (decimal) eo.AngleY;
            nudRotationZ.Value = (decimal) eo.AngleZ;

            nudScaleX.Value = (decimal) (eo.ScaleX * 100.0f);
            nudScaleY.Value = (decimal) (eo.ScaleY * 100.0f);
            nudScaleZ.Value = (decimal) (eo.ScaleZ * 100.0f);

            // (X and Z are flipped because of the stupid 180 scene rotation thing)
            var bounds = eo.BoundingBox;

            nudWest.Value   = (decimal) (-(posX + bounds.RightBottomBack.X.Float) / 32.0f);
            nudEast.Value   = (decimal) (-(posX + bounds.LeftTopFront.X.Float)    / 32.0f);
            nudTop.Value    = (decimal) (-(posY + bounds.LeftTopFront.Y.Float)    /  2.0f);
            nudBottom.Value = (decimal) (-(posY + bounds.RightBottomBack.Y.Float) /  2.0f);
            nudSouth.Value  = (decimal) (-(posZ + bounds.RightBottomBack.Z.Float) / 32.0f);
            nudNorth.Value  = (decimal) (-(posZ + bounds.LeftTopFront.Z.Float)    / 32.0f);

            nudSizeX.Value = (decimal) (bounds.Width  / 32.0f);
            nudSizeY.Value = (decimal) (bounds.Height /  2.0f);
            nudSizeZ.Value = (decimal) (bounds.Depth  / 32.0f);
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            labelModelInstanceEdited.Text = "Model Instance: " + (eo == null ? "(none)" : $"0x{eo.ModelInstanceID:X3}");

            cbAlwaysFaceCamera.Checked = eo.AlwaysFacesCamera;

            if (eo is IMPD_ModelInstance mpdModelInst) {
                var dirStr = OnlyVisibleFromToString(mpdModelInst.OnlyVisibleFromDirection);
                cbVisibleFrom.Text = dirStr;
            }
            else
                cbVisibleFrom.Hide();

            UpdateCoordinateValues(eo);

            SetNudValueAndText(nudModelID, eo.ModelID);
        }

        private string OnlyVisibleFromToString(ModelDirectionType dir)
            => (dir == ModelDirectionType.Unset) ? "(Any Direction)" : dir.ToString();

        private ModelDirectionType? StringToOnlyVisibleFrom(string str) 
            => (str == "(Any Direction)") ? ModelDirectionType.Unset : Enum.Parse<ModelDirectionType>(str);
    }
}
