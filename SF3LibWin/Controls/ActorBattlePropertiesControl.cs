using System;
using System.Windows.Forms;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : ActorBattlePropertiesControlBase {
        public ActorBattlePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbDirection.DataSource = Enum.GetValues<SlotFacingType>();

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action action) {
                DoOnlyDirectly(() => {
                    action();
                    Viewer?.GLControl?.InvalidateActors();
                });
            }

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.X = (int) nudX.Value;
                nudXWorld.Value = (decimal) eo.ActorX;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.Z = (int) nudZ.Value;
                nudZWorld.Value = (decimal) eo.ActorZ;
            });

            cbDirection.SelectedValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                // TODO: support multiple selection!
                var eo = EditingObjects[0];
                eo.Facing = (SlotFacingType) cbDirection.SelectedValue;
            });
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            labelActorEdited.Text = "Actor (Battle): " + (eo == null ? "(none)" : $"0x{eo.ID:X2}");

            InitNUD(nudXWorld, (decimal) eo.ActorX);
            InitNUD(nudZWorld, (decimal) eo.ActorZ);
            InitNUD(nudX, eo.X);
            InitNUD(nudZ, eo.Z);

            cbDirection.Text = eo.Facing.ToString();
        }
    }
}
