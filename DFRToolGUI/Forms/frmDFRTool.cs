using System;
using System.Linq;
using System.Windows.Forms;
using DFRLib.Types;

namespace DFRTool.GUI.Forms {
    public partial class frmDFRTool : Form {
        bool _isDialogMode = true;

        /// <summary>
        /// Initializes the DFRToolGUI as a standalone application or integrated dialog.
        /// </summary>
        public frmDFRTool(CommandType? command = null, bool dialogMode = false) {
            _isDialogMode = dialogMode;

            // Dialog mode starts in the middle of the screen, can't be minimized, and can be closed with the escape key.
            if (dialogMode) {
                this.DialogResult = DialogResult.Cancel;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.MinimizeBox = false;
                InitializeComponent();
                this.createDFRControl1.CreateDFR += CloseDialogWithSuccess;
                this.applyDFRControl1.ApplyDFR += CloseDialogWithSuccess;
            }
            else
                InitializeComponent();

            // If a command was supplied, for the tab.
            switch (command) {
                case CommandType.Apply:
                    tabCommand.SelectedTab = tabCommand_Apply;
                    break;
                case CommandType.Create:
                    tabCommand.SelectedTab = tabCommand_Create;
                    break;
                case null:
                    break;
                default:
                    throw new ArgumentException(nameof(command) + ": Unhandled DFRTool command");
            }

            // Move content from the tab to the main window, delete the tab, and resize accordingly.
            if (command != null) {
                var widthDiff  = tabCommand.Width  - tabCommand.DisplayRectangle.Width  + tabCommand.Margin.Horizontal;
                var heightDiff = tabCommand.Height - tabCommand.DisplayRectangle.Height + tabCommand.Margin.Vertical;

                // Unanchor the tab control so it won't resize itself (and its contents) when we reduce window sizes.
                tabCommand.Anchor = AnchorStyles.None;
                this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width - widthDiff, this.MinimumSize.Height - heightDiff);
                this.Width  -= widthDiff;
                this.Height -= heightDiff;
                this.MaximumSize = new System.Drawing.Size(this.MaximumSize.Width - widthDiff, this.MaximumSize.Height - heightDiff);

                this.Controls.AddRange(tabCommand.SelectedTab.Controls.Cast<Control>().ToArray());
                this.Controls.Remove(tabCommand);
                this.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void CloseDialogWithSuccess(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// When set, the "Create" command will use explicit "altered file" data instead of an actual file.
        /// </summary>
        public byte[] CreateDFRAlteredData {
            get => createDFRControl1.AlteredData;
            set => createDFRControl1.AlteredData = value;
        }

        /// <summary>
        /// When set, the "Apply" command will use explicit "original file" data instead of an actual file.
        /// </summary>
        public byte[] ApplyDFROriginalData {
            get => applyDFRControl1.OriginalData;
            set => applyDFRControl1.OriginalData = value;
        }

        /// <summary>
        /// When set, the "Apply" command will output its contents to ApplyDFRInMemoryOutput.
        /// </summary>
        public bool ApplyDFRInMemory {
            get => applyDFRControl1.ApplyInMemory;
            set => applyDFRControl1.ApplyInMemory = value;
        }

        /// <summary>
        /// Result of the "Apply" command when ApplyInMemory is set to 'true'.
        /// </summary>
        public byte[] ApplyDFRInMemoryOutput => applyDFRControl1.InMemoryOutput;

        protected override bool ProcessDialogKey(Keys keyData) {
            if (_isDialogMode && ModifierKeys == Keys.None && keyData == Keys.Escape) {
                Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }
    }
}
