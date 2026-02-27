using System.ComponentModel;
using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : PropertiesControlBase {
        public ActorBattlePropertiesControl() {
            InitializeComponent();
        }

        protected override void UpdateControls() {
            // Guard to prevent tiles from being edited while values are being initialized.
            if (NonUserInputGuard > 0)
                return;

            using (IncrementNonUserInputGuard()) {
                void InitNUD(NumericUpDown nud, decimal value) {
                    nud.Value = value;
                    nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
                }

                labelActorEdited.Text = "Actor (Battle): " + (_actor == null ? "(none)" : $"0x{_actor.ID:X2}");

                InitNUD(nudX, _actor.X);
                InitNUD(nudY, _actor.Z);
                cbDirection.Text = _actor.Facing.ToString();
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Slot Actor {
            get => _actor;
            set => SetObject(ref _actor, value);
        }

        private Slot _actor = null;
    }
}
