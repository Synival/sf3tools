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

            void UpdateYValues() {
                // TODO: support multiple selection!
                var eo     = EditingObjects[0];
                var posY   = eo.PositionY;

                nudYWorld.Value = -posY;
                nudY.Value      = (decimal) (-posY / 2.0f);

                UpdateBoundingBox();
            }

            void UpdateBoundingBox() {
                // TODO: support multiple selection!
                var eo     = EditingObjects[0];
                var posY   = eo.PositionY;
                var bounds = eo.BoundingCube;

                nudTop.Value    = (decimal) (-(posY + bounds.LeftTopFront.Y.Float)    / 2.0f);
                nudBottom.Value = (decimal) (-(posY + bounds.RightBottomBack.Y.Float) / 2.0f);
            }

            // World coordinates
            nudXWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -nudXWorld.Value;
                nudX.Value = (decimal) (-((float) eo.PositionX + 16) / 32);
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
                nudZ.Value = (decimal) (-((float) eo.PositionZ + 16) / 32);
            });

            // Grid coordinates
            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -(nudX.Value * 32 + 16);
                nudXWorld.Value = -eo.PositionX;
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
                eo.PositionZ = (short) -(nudZ.Value * 32 + 16);
                nudZWorld.Value = -eo.PositionZ;
            });

            // Bounding box coordinates
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

            // Rotation
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
                UpdateBoundingBox();
            });

            nudRotationY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.AngleY = WrapAngleValue(nudRotationY);
                UpdateBoundingBox();
            });

            nudRotationZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.AngleZ = WrapAngleValue(nudRotationZ);
                UpdateBoundingBox();
            });

            // Scale
            nudScaleX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleX = (float) nudScaleX.Value;
                UpdateBoundingBox();
            });

            nudScaleY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleY = (float) nudScaleY.Value;
                UpdateBoundingBox();
            });

            nudScaleZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.ScaleZ = (float) nudScaleZ.Value;
                UpdateBoundingBox();
            });

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
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];
            var boundingCube = eo.BoundingCube;

            labelModelInstanceEdited.Text = "Model Instance: " + (eo == null ? "(none)" : $"0x{eo.ID:X3}");

            InitNUD(nudXWorld, -eo.PositionX);
            InitNUD(nudYWorld, -eo.PositionY);
            InitNUD(nudZWorld, -eo.PositionZ);

            InitNUD(nudX,      (decimal) (-(eo.PositionX + 16) / 32.0f));
            InitNUD(nudY,      (decimal) (-eo.PositionY / 2.0f));
            InitNUD(nudZ,      (decimal) (-(eo.PositionZ + 16) / 32.0f));

            InitNUD(nudTop,    (decimal) (-(eo.PositionY + boundingCube.LeftTopFront.Y.Float)    / 2.0f));
            InitNUD(nudBottom, (decimal) (-(eo.PositionY + boundingCube.RightBottomBack.Y.Float) / 2.0f));

            InitNUD(nudRotationX, (decimal) eo.AngleX);
            InitNUD(nudRotationY, (decimal) eo.AngleY);
            InitNUD(nudRotationZ, (decimal) eo.AngleZ);

            InitNUD(nudScaleX, (decimal) eo.ScaleX);
            InitNUD(nudScaleY, (decimal) eo.ScaleY);
            InitNUD(nudScaleZ, (decimal) eo.ScaleZ);

            var dirStr = OnlyVisibleFromToString(eo.OnlyVisibleFromDirection);
            cbVisibleFrom.Text = dirStr;
        }

        private string OnlyVisibleFromToString(ModelDirectionType dir)
            => (dir == ModelDirectionType.Unset) ? "(Any Direction)" : dir.ToString();

        private ModelDirectionType? StringToOnlyVisibleFrom(string str) {
            return (str == "(Any Direction)") ? ModelDirectionType.Unset
                : (ModelDirectionType?) Enum.Parse<ModelDirectionType>(str);
        }
    }
}
