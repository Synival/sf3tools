using SF3.MPD.Interfaces;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : PropertiesControlBase<IMPD_ModelInstance> {
        public ModelInstancePropertiesControl() {
            InitializeComponent();
        }

        protected override void PerformUpdateControls() {
            labelModelInstanceEdited.Text = "Model Instance: " + (EditingObject == null ? "(none)" : $"0x{EditingObject.ID:X3}");
        }
    }
}
