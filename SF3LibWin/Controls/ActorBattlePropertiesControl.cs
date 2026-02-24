using System.ComponentModel;
using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;
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
            labelActorEdited.Text = "Actor (Battle): " + (_actor == null ? "(none)" : $"0x{_actor.ID:X2}");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Slot Actor {
            get => _actor;
            set {
                if (value == _actor)
                    return;

                _actor = value;
                UpdateControls();
            }
        }

        private Slot _actor = null;

        public event CmdKeyEventHandler CmdKey;
    }
}
