using System.ComponentModel;
using SF3.MPD.Interfaces;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : PropertiesControlBase {
        public ModelInstancePropertiesControl() {
            InitializeComponent();
        }

        private void UpdateControls() {
            labelModelInstanceEdited.Text = "Model Instance: " + (_modelInstance == null ? "(none)" : $"0x{_modelInstance.ID:X3}");
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IMPD_ModelInstance ModelInstance {
            get => _modelInstance;
            set {
                if (value == _modelInstance)
                    return;

                _modelInstance = value;
                UpdateControls();
            }
        }

        private IMPD_ModelInstance _modelInstance = null;
    }
}
