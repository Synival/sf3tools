using System.Windows.Forms;

namespace DFRTool.GUI.Forms {
    public partial class frmDFRTool : Form {
        bool _isDialogMode = true;

        /// <summary>
        /// Initializes the DFRToolGUI as a standalone application.
        /// </summary>
        public frmDFRTool() {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the DFRToolGUI for use inside an editor, with specific data for the "altered" file.
        /// </summary>
        /// <param name="data"></param>
        public frmDFRTool(byte[] data) {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            InitializeComponent();

            createDFRControl1.AlteredData = data;
            _isDialogMode = true;
        }

        protected override bool ProcessDialogKey(Keys keyData) {
            if (_isDialogMode && ModifierKeys == Keys.None && keyData == Keys.Escape) {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
