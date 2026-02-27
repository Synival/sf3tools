using SF3.Models.Structs.X1.Town;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : PropertiesControlBase<Npc> {
        public ActorNPCPropertiesControl() {
            InitializeComponent();
        }

        protected override void PerformUpdateControls() {
            labelActorEdited.Text = "Actor (NPC): " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X2}");
        }
    }
}
