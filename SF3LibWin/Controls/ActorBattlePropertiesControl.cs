using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : PropertiesControlBase<Slot> {
        public ActorBattlePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);
        }

        protected override void PerformUpdateControls() {
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
