using System.Windows.Forms;

namespace SF3.Win.Extensions {
    public static class ControlExtensions {
        /// <summary>
        /// Gets the bottom-most control that has user focus by checking ActiveControl and recrusing down.
        /// Returns 'null' if 'ContainsFocus' is 'false'.
        /// </summary>
        /// <param name="control">The control the check for focus.</param>
        /// <returns>A Control that has no 'ActiveControl' child set but is active itself.</returns>
        public static Control GetFocusedControl(this Control control) {
            if (!control.ContainsFocus)
                return null;

            var nextActiveControl = (control as IContainerControl)?.ActiveControl;
            while (true) {
                if (nextActiveControl == null)
                    return control;
                control = nextActiveControl;
                nextActiveControl = (control as IContainerControl)?.ActiveControl;
            }
        }
    }
}
