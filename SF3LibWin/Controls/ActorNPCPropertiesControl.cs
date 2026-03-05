using System;
using CommonLib.Utils;
using SF3.Models.Structs.X1.Town;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : ActorNPCPropertiesControlBase {
        public ActorNPCPropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action<Npc> action) {
                DoOnlyDirectly(() => {
                    // TODO: support multiple selection!
                    action(EditingObjects[0]);
                    Viewer?.GLControl?.InvalidateActors();
                });
            }

            nudXWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ActorX = (float) nudXWorld.Value;
                nudX.Value = (decimal) (eo.ActorX / 32);
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ActorZ = (float) nudZWorld.Value;
                nudZ.Value = (decimal) (eo.ActorZ / 32);
            });

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ActorX = (float) nudX.Value * 32;
                nudXWorld.Value = (decimal) eo.ActorX;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                eo.ActorZ = (float) nudZ.Value * 32;
                nudZWorld.Value = (decimal) eo.ActorZ;
            });

            nudDirection.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(eo => {
                var value = MathHelpers.ActualMod((float) nudDirection.Value + 180.0f, 360.0f) - 180.0f;
                if ((float) nudDirection.Value != value)
                    nudDirection.Value = (decimal) value;
                eo.ActorDirection = value;
            });
        }

        protected override void PerformUpdateControls() {
            // TODO: support multiple selection!
            var eo = EditingObjects[0];

            labelActorEdited.Text = "Actor (NPC): " + (eo == null ? "(none)" : $"0x{eo.ID:X2}");

            InitNUD(nudXWorld, (decimal) eo.ActorX);
            InitNUD(nudZWorld, (decimal) eo.ActorZ);
            InitNUD(nudX, (decimal) (eo.ActorX / 32.0f));
            InitNUD(nudZ, (decimal) (eo.ActorZ / 32.0f));

            InitNUD(nudDirection, (decimal) eo.ActorDirection);
        }
    }
}
