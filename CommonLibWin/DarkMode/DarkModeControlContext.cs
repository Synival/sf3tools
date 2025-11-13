using System.Drawing;
using System.Windows.Forms;

namespace CommonLib.Win.DarkMode {
    /// <summary>
    /// DarkModeContext with base behavior for any Control type.
    /// </summary>
    /// <typeparam name="T">Underlying type of the control.</typeparam>
    public class DarkModeControlContext<T> : DarkModeContext where T : Control {
        public DarkModeControlContext(T control) {
            Control = control;
            Control.Disposed += (s, e) => Dispose();
        }

        protected override void OnInit() {
            OriginalBackColor = Control.BackColor;
            OriginalForeColor = Control.ForeColor;
        }

        protected override void OnDarkModeEnabled() {
            Control.BackColor = DarkModeColors.BackColor;
            Control.ForeColor = DarkModeColors.ForeColor;
            Control.Invalidate();
        }

        protected override void OnDarkModeDisabled() {
            Control.BackColor = OriginalBackColor;
            Control.ForeColor = OriginalForeColor;
            Control.Invalidate();
        }

        protected override void OnDispose() {
            base.OnDispose();
            Control = null;
        }

        /// <summary>
        /// The contol associated with this context.
        /// </summary>
        public T Control { get; private set; }

        /// <summary>
        /// BackColor used when dark mode is disabled.
        /// </summary>
        public Color OriginalBackColor { get; set; }

        /// <summary>
        /// ForeColor used when dark mode is disabled.
        /// </summary>
        public Color OriginalForeColor { get; set; }
    }
}
