using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    /// <summary>
    /// Any UserControl with dark mode support. Can be used as a base for most custom controls.
    /// </summary>
    public class DarkModeUserControl : UserControl {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeControlContext<UserControl>(this);
                DarkModeContext.Init();
            }
        }

        private DarkModeControlContext<UserControl> DarkModeContext { get; set; }
    }
}
