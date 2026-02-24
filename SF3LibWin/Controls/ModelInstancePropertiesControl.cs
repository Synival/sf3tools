using System.ComponentModel;
using System.Windows.Forms;
using SF3.MPD.Interfaces;
using static SF3.Win.Utils.EventHandlers;

namespace SF3.Win.Controls {
    public partial class ModelInstancePropertiesControl : UserControl {
        public ModelInstancePropertiesControl() {
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

        public event CmdKeyEventHandler CmdKey;
    }
}
