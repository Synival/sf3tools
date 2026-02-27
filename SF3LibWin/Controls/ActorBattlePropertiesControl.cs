using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : PropertiesControlBase<Slot> {
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

                labelActorEdited.Text = "Actor (Battle): " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X2}");

                InitNUD(nudX, EditingObject.X);
                InitNUD(nudY, EditingObject.Z);
                cbDirection.Text = EditingObject.Facing.ToString();
            }
        }
    }
}
