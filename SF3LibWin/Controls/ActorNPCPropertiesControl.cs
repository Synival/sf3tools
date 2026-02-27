using System.ComponentModel;
using SF3.Models.Structs.X1.Town;

namespace SF3.Win.Controls {
    public partial class ActorNPCPropertiesControl : PropertiesControlBase {
        public ActorNPCPropertiesControl() {
            InitializeComponent();
        }

        private void UpdateControls() {
            labelActorEdited.Text = "Actor (NPC): " + (_actor == null ? "(none)" : $"0x{_actor.ID:X2}");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Npc Actor {
            get => _actor;
            set {
                if (value == _actor)
                    return;

                _actor = value;
                UpdateControls();
            }
        }

        private Npc _actor = null;
    }
}
