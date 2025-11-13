using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    /// <summary>
    /// TextBox with dark mode support.
    /// </summary>
    public class DarkModeTextBox : TextBox {
        public DarkModeTextBox() {
            BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeTextBox>(this);
                DarkModeContext.Init();
            }
        }

        private DarkModeControlContext<DarkModeTextBox> DarkModeContext { get; set; }
    }
}
