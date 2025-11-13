using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace SF3.Win.Forms {
    /// <summary>
    /// Form with support for dark mode.
    /// </summary>
    public class DarkModeForm : Form {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<DarkModeForm>(this);
                DarkModeContext.Init();
            }
        }

        protected DarkModeControlContext<DarkModeForm> DarkModeContext { get; set; } = null;
    }
}
