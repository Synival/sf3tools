using System.ComponentModel;
using SF3.MPD.Interfaces;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : PropertiesControlBase {
        public ModelInstancePropertiesControl() {
            InitializeComponent();
        }

        protected override void UpdateControls() {
            labelModelInstanceEdited.Text = "Model Instance: " + (_modelInstance == null ? "(none)" : $"0x{_modelInstance.ID:X3}");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD_ModelInstance ModelInstance {
            get => _modelInstance;
            set => SetObject(ref _modelInstance, value);
        }

        private IMPD_ModelInstance _modelInstance = null;
    }
}
