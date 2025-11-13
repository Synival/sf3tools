using System;
using System.Drawing;
using System.Windows.Forms;
using SF3.Win.DarkMode;

namespace SF3.Win.Controls {
    /// <summary>
    /// ToolStrip with dark mode support.
    /// </summary>
    public class DarkModeToolStrip : ToolStrip {
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (DarkModeContext == null) {
                DarkModeContext = new DarkModeToolStripContext<DarkModeToolStrip>(this, false);
                DarkModeContext.Init();
                DarkModeContext.OriginalBackColor = SystemColors.Control;
            }
        }

        private DarkModeToolStripContext<DarkModeToolStrip> DarkModeContext { get; set; }
    }
}
