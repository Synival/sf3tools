using System.Windows.Forms;
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

        public event CmdKeyEventHandler CmdKey;
    }
}
