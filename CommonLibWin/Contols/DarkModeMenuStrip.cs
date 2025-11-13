using System;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
    /// <summary>
    /// MenuStrip with dark mode support.
    /// </summary>
    public class DarkModeMenuStrip : MenuStrip {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeToolStripContext<DarkModeMenuStrip>(this, true);
                DarkModeContext.Init();
            }
        }

        private DarkModeToolStripContext<DarkModeMenuStrip> DarkModeContext { get; set; }
    }
}
