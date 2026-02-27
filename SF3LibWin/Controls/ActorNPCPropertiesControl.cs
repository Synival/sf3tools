using System.ComponentModel;
using SF3.Models.Structs.X1.Town;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : PropertiesControlBase {
        public ActorNPCPropertiesControl() {
            InitializeComponent();
        }

        protected override void UpdateControls() {
            labelActorEdited.Text = "Actor (NPC): " + (_actor == null ? "(none)" : $"0x{_actor.ID:X2}");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Npc Actor {
            get => _actor;
            set => SetObject(ref _actor, value);
        }

        private Npc _actor = null;
    }
}
