using System;
using System.Windows.Forms;
using SF3.MPD.Extensions;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : ModelInstancePropertiesControlBase {
        public ModelInstancePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action action) {
                DoOnlyDirectly(() => {
                    action();
                    Viewer?.GLControl?.InvalidateModels();
                });
            }

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
                nudY.Value = (decimal) (-Math.Round((float) eo.PositionY + eo.GetBottomY(0)) / 2);
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -nudZWorld.Value;
                nudZ.Value = (decimal) (-((float) eo.PositionZ + 16) / 32);
            });

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionX = (short) -(nudX.Value * 32 + 16);
                nudXWorld.Value = -eo.PositionX;
            });

            nudY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                var bottomY = eo.GetBottomY(0);

                eo.PositionY = (short) -Math.Round((float) nudY.Value * 2 + bottomY);
                nudYWorld.Value = -eo.PositionY;
                nudY.Value = (decimal) (-Math.Round(eo.PositionY + bottomY) / 2.0f);
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.PositionZ = (short) -(nudZ.Value * 32 + 16);
                nudZWorld.Value = -eo.PositionZ;
            });
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            labelModelInstanceEdited.Text = "Model Instance: " + (eo == null ? "(none)" : $"0x{eo.ID:X3}");

            InitNUD(nudXWorld, -eo.PositionX);
            InitNUD(nudYWorld, -eo.PositionY);
            InitNUD(nudZWorld, -eo.PositionZ);
            InitNUD(nudX, (decimal) (-(eo.PositionX + 16) / 32.0f));
            InitNUD(nudY, (decimal) (-Math.Round(eo.PositionY + eo.GetBottomY(0)) / 2.0f));
            InitNUD(nudZ, (decimal) (-(eo.PositionZ + 16) / 32.0f));
        }
    }
}
