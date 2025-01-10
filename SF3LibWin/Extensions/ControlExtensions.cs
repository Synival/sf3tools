using System.Windows.Forms;

namespace SF3.Win.Extensions {
    public static class ControlExtensions {
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
