using System;
using System.Windows.Forms;
using CommonLib.Utils;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : ActorNPCPropertiesControlBase {
        public ActorNPCPropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Event handling for 'Movement' group.
            void DoOnlyDirectlyAndInvalidate(Action action) {
                DoOnlyDirectly(() => {
                    action();
                    Viewer?.GLControl?.InvalidateActors();
                });
            }

            nudXWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.ActorX = (float) nudXWorld.Value;
                nudX.Value = (decimal) ((EditingObject.ActorX - 16) / 32);
            });

            nudZWorld.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.ActorZ = (float) nudZWorld.Value;
                nudZ.Value = (decimal) ((EditingObject.ActorZ - 16) / 32);
            });

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.ActorX = (float) nudX.Value * 32 + 16;
                nudXWorld.Value = (decimal) EditingObject.ActorX;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.ActorZ = (float) nudZ.Value * 32 + 16;
                nudZWorld.Value = (decimal) EditingObject.ActorZ;
            });

            nudDirection.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                var value = MathHelpers.ActualMod((float) nudDirection.Value + 180.0f, 360.0f) - 180.0f;
                if ((float) nudDirection.Value != value)
                    nudDirection.Value = (decimal) value;
                EditingObject.ActorDirection = value;
            });
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            labelActorEdited.Text = "Actor (NPC): " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X2}");

            InitNUD(nudXWorld, (decimal) EditingObject.ActorX);
            InitNUD(nudZWorld, (decimal) EditingObject.ActorZ);
            InitNUD(nudX, (decimal) ((EditingObject.ActorX - 16) / 32.0f));
            InitNUD(nudZ, (decimal) ((EditingObject.ActorZ - 16) / 32.0f));

            InitNUD(nudDirection, (decimal) EditingObject.ActorDirection);
        }
    }
}
