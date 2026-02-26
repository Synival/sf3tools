using System;
using System.ComponentModel;
using System.Windows.Forms;
using CommonLib;
using SF3.Models.Structs.X1.Battle;
using SF3.Win.Extensions;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : UserControl {
        public ActorBattlePropertiesControl() {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            bool wasProcessed = false;
            CmdKey?.Invoke(this, ref msg, keyData, ref wasProcessed);
            if (wasProcessed)
                return wasProcessed;

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UpdateControls() {
            // Guard to prevent tiles from being edited while values are being initialized.
            if (_nonUserInputGuard > 0)
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

        private int _nonUserInputGuard = 0;
        private ScopeGuard IncrementNonUserInputGuard()
            => new ScopeGuard(() => _nonUserInputGuard++, () => _nonUserInputGuard--);

        private void DoIfUserInput(Action action) {
            if (_nonUserInputGuard > 0)
                return;
            action();
        }

        private Slot _actor = null;

        public event CmdKeyEventHandler CmdKey;
    }
}
