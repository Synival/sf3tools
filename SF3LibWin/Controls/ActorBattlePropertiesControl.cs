using System;
using SF3.Models.Structs.X1.Battle;
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
            void DoOnlyDirectlyAndInvalidate(Action<Slot> action) {
                DoOnlyDirectly(() => {
                    // TODO: support multiple selection!
                    var eo = EditingObjects[0];
                    action(eo);
                    Viewer?.GLControl?.InvalidateActors();
                });
            }

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.X = (int) nudX.Value;
                nudXWorld.Value = (decimal) eo.ActorX;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.Z = (int) nudZ.Value;
                nudZWorld.Value = (decimal) eo.ActorZ;
            });

            cbDirection.SelectedValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => eo.Facing = (SlotFacingType) cbDirection.SelectedValue);
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            labelActorEdited.Text = "Actor (Battle): " + (eo == null ? "(none)" : $"0x{eo.ID:X2}");

            SetNudValueAndText(nudXWorld, (decimal) eo.ActorX);
            SetNudValueAndText(nudZWorld, (decimal) eo.ActorZ);
            SetNudValueAndText(nudX, eo.X);
            SetNudValueAndText(nudZ, eo.Z);

            cbDirection.Text = eo.Facing.ToString();
        }
    }
}
