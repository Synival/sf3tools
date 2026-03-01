using System;
using System.Windows.Forms;
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
            void DoOnlyDirectlyAndInvalidate(Action action) {
                DoOnlyDirectly(() => {
                    action();
                    Viewer?.GLControl?.InvalidateActors();
                });
            }

            nudX.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.X = (int) nudX.Value;
                nudXWorld.Value = (decimal) EditingObject.ActorX;
            });

            nudZ.ValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => {
                EditingObject.Z = (int) nudZ.Value;
                nudZWorld.Value = (decimal) EditingObject.ActorZ;
            });

            cbDirection.SelectedValueChanged += (s, e) => DoOnlyDirectlyAndInvalidate(() => EditingObject.Facing = (SlotFacingType) cbDirection.SelectedValue);
        }

        protected override void PerformUpdateControls() {
            void InitNUD(NumericUpDown nud, decimal value) {
                nud.Value = value;
                nud.Text = (nud.Hexadecimal) ? ((int) value).ToString("X") : value.ToString();
            }

            labelActorEdited.Text = "Actor (Battle): " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X2}");

            InitNUD(nudXWorld, (decimal) EditingObject.ActorX);
            InitNUD(nudZWorld, (decimal) EditingObject.ActorZ);
            InitNUD(nudX, EditingObject.X);
            InitNUD(nudZ, EditingObject.Z);

            cbDirection.Text = EditingObject.Facing.ToString();
        }
    }
}
