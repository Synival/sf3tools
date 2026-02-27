using System.ComponentModel;
using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;
using SF3.Win.Extensions;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : PropertiesControlBase {
        public ActorBattlePropertiesControl() {
            InitializeComponent();
        }

        private void UpdateControls() {
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
            set {
                if (value == _actor)
                    return;

                Control lastFocused = null;
                if (ContainsFocus && !Focused) {
                    lastFocused = this.GetFocusedControl();
                    Focus();
                }

                _actor = value;
                UpdateControls();

                if (lastFocused != null) {
                    if (lastFocused is NumericUpDown nud)
                        nud.Select(0, nud.Text.Length);
                    else if (lastFocused is TextBox tb)
                        tb.SelectAll();
                    else if (lastFocused is ComboBox cb)
                        cb.Select(0, cb.Text.Length);
                }
            }
        }

        private Slot _actor = null;
    }
}
