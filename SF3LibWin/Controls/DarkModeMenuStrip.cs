using System;
using System.Windows.Forms;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    /// <summary>
    /// MenuStrip with dark mode support.
    /// </summary>
    public class DarkModeMenuStrip : MenuStrip {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeToolStripContext<DarkModeMenuStrip>(this);
                DarkModeContext.Init();
            }
        }

        private DarkModeToolStripContext<DarkModeMenuStrip> DarkModeContext { get; set; }
    }
}
