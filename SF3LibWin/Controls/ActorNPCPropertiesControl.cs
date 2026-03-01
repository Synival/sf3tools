using SF3.Models.Structs.X1.Town;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : ActorNPCPropertiesControlBase {
        public ActorNPCPropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);
        }

        protected override void PerformUpdateControls() {
            labelActorEdited.Text = "Actor (NPC): " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X2}");
        }
    }
}
