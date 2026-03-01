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

            void UpdateYValues() {
                // TODO: support multiple selection!
                var eo      = EditingObjects[0];
                var posY    = eo.PositionY;
                var topY    = eo.GetTopY(0);
                var bottomY = eo.GetBottomY(0);

                nudYWorld.Value = -posY;
                nudY.Value      = (decimal) (-posY / 2.0f);
                nudTop.Value    = (decimal) (-Math.Round(posY + topY) / 2.0f);
                nudBottom.Value = (decimal) (-Math.Round(posY + bottomY) / 2.0f);
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
                UpdateYValues();
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
                eo.PositionY = (short) -Math.Round((float) nudY.Value * 2);
                UpdateYValues();
            });

            nudTop.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                var topY = eo.GetTopY(0);
                eo.PositionY = (short) -Math.Round((float) nudTop.Value * 2 + topY);
                UpdateYValues();
            });

            nudBottom.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                var bottomY = eo.GetBottomY(0);
                eo.PositionY = (short) -Math.Round((float) nudBottom.Value * 2 + bottomY);
                UpdateYValues();
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
            InitNUD(nudX,      (decimal) (-(eo.PositionX + 16) / 32.0f));
            InitNUD(nudY,      (decimal) (-eo.PositionY / 2.0f));
            InitNUD(nudTop,    (decimal) (-Math.Round(eo.PositionY + eo.GetTopY(0)) / 2.0f));
            InitNUD(nudBottom, (decimal) (-Math.Round(eo.PositionY + eo.GetBottomY(0)) / 2.0f));
            InitNUD(nudZ,      (decimal) (-(eo.PositionZ + 16) / 32.0f));
        }
    }
}
