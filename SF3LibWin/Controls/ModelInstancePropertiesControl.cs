using System;
using System.Windows.Forms;

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
                EditingObject.PositionX = (short) -nudXWorld.Value;
                nudX.Value = (decimal) (-((float) EditingObject.PositionX + 16) / 32);
            });

            nudYWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.PositionY = (short) -nudYWorld.Value;
                nudY.Value = (decimal) (-(float) EditingObject.PositionY / 32);
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.PositionZ = (short) -nudZWorld.Value;
                nudZ.Value = (decimal) (-((float) EditingObject.PositionZ + 16) / 32);
            });

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.PositionX = (short) -(nudX.Value * 32 + 16);
                nudXWorld.Value = -EditingObject.PositionX;
            });

            nudY.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.PositionY = (short) -(nudY.Value * 32);
                nudYWorld.Value = -EditingObject.PositionY;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.PositionZ = (short) -(nudZ.Value * 32 + 16);
                nudZWorld.Value = -EditingObject.PositionZ;
            });
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            labelModelInstanceEdited.Text = "Model Instance: " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X3}");

            InitNUD(nudXWorld, -EditingObject.PositionX);
            InitNUD(nudYWorld, -EditingObject.PositionY);
            InitNUD(nudZWorld, -EditingObject.PositionZ);
            InitNUD(nudX, (decimal) (-(EditingObject.PositionX + 16) / 32.0f));
            InitNUD(nudY, (decimal) (-EditingObject.PositionY / 32.0f));
            InitNUD(nudZ, (decimal) (-(EditingObject.PositionZ + 16) / 32.0f));
        }
    }
}
