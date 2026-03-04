using System;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Utils;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : ModelInstancePropertiesControlBase {
        public ModelInstancePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbVisibleFrom.DataSource = Enum.GetValues<ModelDirectionType>().Select(x => OnlyVisibleFromToString(x)).ToArray();

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action action) {
                DoOnlyDirectly(() => {
                    action();
                    Viewer?.GLControl?.InvalidateModels();
                });
            }

            void UpdateXValues() {
                // TODO: support multiple selection!
                var eo   = EditingObjects[0];
                var posX = eo.PositionX;

                nudXWorld.Value = -posX;
                nudX.Value = (decimal) (-posX/ 32.0f);

                var bounds = eo.BoundingCube;
                // (Flipped because of the stupid 180 scene rotation thing)
                nudWest.Value = (decimal) (-(posX + bounds.RightBottomBack.X.Float) / 32.0f);
                nudEast.Value = (decimal) (-(posX + bounds.LeftTopFront.X.Float)    / 32.0f);
            }

            void UpdateYValues() {
                // TODO: support multiple selection!
                var eo   = EditingObjects[0];
                var posY = eo.PositionY;

                nudYWorld.Value = -posY;
                nudY.Value = (decimal) (-posY / 2.0f);

                var bounds = eo.BoundingCube;
                nudTop.Value    = (decimal) (-(posY + bounds.LeftTopFront.Y.Float)    / 2.0f);
                nudBottom.Value = (decimal) (-(posY + bounds.RightBottomBack.Y.Float) / 2.0f);
            }

            void UpdateZValues() {
                // TODO: support multiple selection!
                var eo   = EditingObjects[0];
                var posZ = eo.PositionZ;

                nudZWorld.Value = -posZ;
                nudZ.Value = (decimal) (-posZ / 32.0f);

                var bounds = eo.BoundingCube;
                // (Flipped because of the stupid 180 scene rotation thing)
                nudSouth.Value = (decimal) (-(posZ + bounds.RightBottomBack.Z.Float) / 32.0f);
                nudNorth.Value = (decimal) (-(posZ + bounds.LeftTopFront.Z.Float)    / 32.0f);
            }

            void UpdateBoundingBoxValues() {
                // TODO: support multiple selection!
                var eo     = EditingObjects[0];
                var bounds = eo.BoundingCube;

                var posX = eo.PositionX;
                var posY = eo.PositionY;
                var posZ = eo.PositionZ;

                // (X and Z are flipped because of the stupid 180 scene rotation thing)
                nudWest.Value   = (decimal) (-(posX + bounds.RightBottomBack.X.Float) / 32.0f);
                nudEast.Value   = (decimal) (-(posX + bounds.LeftTopFront.X.Float)    / 32.0f);
                nudTop.Value    = (decimal) (-(posY + bounds.LeftTopFront.Y.Float)    /  2.0f);
                nudBottom.Value = (decimal) (-(posY + bounds.RightBottomBack.Y.Float) /  2.0f);
                nudSouth.Value  = (decimal) (-(posZ + bounds.RightBottomBack.Z.Float) / 32.0f);
                nudNorth.Value  = (decimal) (-(posZ + bounds.LeftTopFront.Z.Float)    / 32.0f);
            }

            // ---------------------------------------
            // World coordinates
            // ---------------------------------------

            nudXWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -nudXWorld.Value;
                UpdateXValues();
            });

            nudYWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionY = (short) -nudYWorld.Value;
                UpdateYValues();
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -nudZWorld.Value;
                UpdateZValues();
            });

            // ---------------------------------------
            // Grid coordinates
            // ---------------------------------------

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -(nudX.Value * 32);
                UpdateXValues();
            });

            nudY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionY = (short) -((float) nudY.Value * 2);
                UpdateYValues();
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -(nudZ.Value * 32);
                UpdateZValues();
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

            nudRotationX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.AngleX = WrapAngleValue(nudRotationX);
                UpdateBoundingBoxValues();
            });

            nudRotationY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.AngleY = WrapAngleValue(nudRotationY);
                UpdateBoundingBoxValues();
            });

            nudRotationZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.AngleZ = WrapAngleValue(nudRotationZ);
                UpdateBoundingBoxValues();
            });

            // ---------------------------------------
            // Scale
            // ---------------------------------------

            nudScaleX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleX = (float) nudScaleX.Value / 100.0f;
                UpdateBoundingBoxValues();
            });

            nudScaleY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleY = (float) nudScaleY.Value / 100.0f;
                UpdateBoundingBoxValues();
            });

            nudScaleZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleZ = (float) nudScaleZ.Value / 100.0f;
                UpdateBoundingBoxValues();
            });

            // ---------------------------------------
            // Flags
            // ---------------------------------------

            // Only visible when facing direction
            cbVisibleFrom.SelectedValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                var dir = StringToOnlyVisibleFrom(cbVisibleFrom.Text);
                if (!dir.HasValue)
                    cbVisibleFrom.Text = OnlyVisibleFromToString(eo.OnlyVisibleFromDirection);
                else
                    eo.OnlyVisibleFromDirection = dir.Value;
            });

            // ---------------------------------------
            // Bounding box coordinates
            // ---------------------------------------

            nudWest.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -Math.Round((float) nudWest.Value * 32 + eo.BoundingCube.RightBottomBack.X.Float);
                UpdateXValues();
            });

            nudEast.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -Math.Round((float) nudEast.Value * 32 + eo.BoundingCube.LeftTopFront.X.Float);
                UpdateXValues();
            });

            nudTop.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionY = (short) -Math.Round((float) nudTop.Value * 2 + eo.BoundingCube.LeftTopFront.Y.Float);
                UpdateYValues();
            });

            nudBottom.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionY = (short) -Math.Round((float) nudBottom.Value * 2 + eo.BoundingCube.RightBottomBack.Y.Float);
                UpdateYValues();
            });

            nudSouth.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -Math.Round((float) nudSouth.Value * 32 + eo.BoundingCube.RightBottomBack.Z.Float);
                UpdateZValues();
            });

            nudNorth.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -Math.Round((float) nudNorth.Value * 32 + eo.BoundingCube.LeftTopFront.Z.Float);
                UpdateZValues();
            });
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];
            var bounds = eo.BoundingCube;

            var posX = eo.PositionX;
            var posY = eo.PositionY;
            var posZ = eo.PositionZ;

            labelModelInstanceEdited.Text = "Model Instance: " + (eo == null ? "(none)" : $"0x{eo.ID:X3}");

            InitNUD(nudXWorld, -eo.PositionX);
            InitNUD(nudYWorld, -eo.PositionY);
            InitNUD(nudZWorld, -eo.PositionZ);

            InitNUD(nudX, (decimal) (-posX / 32.0f));
            InitNUD(nudY, (decimal) (-posY /  2.0f));
            InitNUD(nudZ, (decimal) (-posZ / 32.0f));

            InitNUD(nudRotationX, (decimal) eo.AngleX);
            InitNUD(nudRotationY, (decimal) eo.AngleY);
            InitNUD(nudRotationZ, (decimal) eo.AngleZ);

            InitNUD(nudScaleX, (decimal) (eo.ScaleX * 100.0f));
            InitNUD(nudScaleY, (decimal) (eo.ScaleY * 100.0f));
            InitNUD(nudScaleZ, (decimal) (eo.ScaleZ * 100.0f));

            var dirStr = OnlyVisibleFromToString(eo.OnlyVisibleFromDirection);
            cbVisibleFrom.Text = dirStr;

            // (X and Z are flipped because of the stupid 180 scene rotation thing)
            nudWest.Value   = (decimal) (-(posX + bounds.RightBottomBack.X.Float) / 32.0f);
            nudEast.Value   = (decimal) (-(posX + bounds.LeftTopFront.X.Float)    / 32.0f);
            nudTop.Value    = (decimal) (-(posY + bounds.LeftTopFront.Y.Float)    /  2.0f);
            nudBottom.Value = (decimal) (-(posY + bounds.RightBottomBack.Y.Float) /  2.0f);
            nudSouth.Value  = (decimal) (-(posZ + bounds.RightBottomBack.Z.Float) / 32.0f);
            nudNorth.Value  = (decimal) (-(posZ + bounds.LeftTopFront.Z.Float)    / 32.0f);
        }

        private string OnlyVisibleFromToString(ModelDirectionType dir)
            => (dir == ModelDirectionType.Unset) ? "(Any Direction)" : dir.ToString();

        private ModelDirectionType? StringToOnlyVisibleFrom(string str) {
            return (str == "(Any Direction)") ? ModelDirectionType.Unset
                : (ModelDirectionType?) Enum.Parse<ModelDirectionType>(str);
        }
    }
}
