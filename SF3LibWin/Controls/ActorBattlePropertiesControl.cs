using System;
using System.Windows.Forms;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Win.Controls {
    public partial class ActorBattlePropertiesControl : ActorBattlePropertiesControlBase {
        public ActorBattlePropertiesControl() {
            InitializeComponent();

            // Recursively set behavior for pressing 'Enter'/'Return' on focusable controls, and other things.
            RecursivelyAttachedEventsToControls(this);

            // Set up combo box values.
            cbDirection.DataSource = Enum.GetValues<SlotFacingType>();

            // Event handling for 'Movement' group.
            nudX.ValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.X = (int) nudX.Value);
            nudX.ValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.Z = (int) nudY.Value);
            cbDirection.SelectedValueChanged += (s, e) => DoOnlyDirectly(() => EditingObject.Facing = (SlotFacingType) cbDirection.SelectedValue);
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
