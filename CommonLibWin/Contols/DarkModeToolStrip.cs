using System;
using System.Drawing;
using System.Windows.Forms;
using CommonLib.Win.DarkMode;

namespace CommonLib.Win.Controls {
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

        protected override void Dispose(bool disposing) {
            if (disposing) {
                // If the button tooltip was shown, this creates a strong reference to this control that remains after
                // disposal. Forcing ToolTipText to 'null' will remove any refs to any button a tooltip might have.
                //
                // (This was happening with a Form that had a ToolTip with a reference to a nested button that was
                // preventing a huge object from being garbage collected D:)
                //
                // There's also a nasty memory leak that occurs when the mouse is hovering over a button while the
                // control is removed... This doesn't seem to be fixed by this... C'mon, Microsoft!
                ShowItemToolTips = false;
                foreach (var item in Items) {
                    if (item is ToolStripButton button) {
                        button.ToolTipText = null;
                        button.AutoToolTip = false;
                    }
                }
            }
            base.Dispose(disposing);
        }

        private DarkModeToolStripContext<DarkModeToolStrip> DarkModeContext { get; set; }
    }
}
