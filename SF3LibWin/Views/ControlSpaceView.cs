using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public class ControlSpaceView : ViewBase, IControlSpaceView {
        public ControlSpaceView(string name) : base(name) {
        }

        public override Control Create() {
            ChildControls = new List<Control>();

            var container = new Control(null, Name);
            container.Padding = new Padding();
            container.Margin = new Padding();

            Control = container;
            return Control;
        }

        public override void Destroy() {
            foreach (var c in ChildControls)
                c.Dispose();
            ((List<Control>) ChildControls).Clear();
            ChildControls = null;
            base.Destroy();
        }

        public Control CreateChild(IView child, bool autoFill = true)
            => CreateChild(child.Name, child.Create(), autoFill);

        public Control CreateChild(string name, Control child, bool autoFill = true) {
            if (child == null)
                return null;

            Control.SuspendLayout();
            if (autoFill)
                child.Dock = DockStyle.Fill;
            Control.Controls.Add(child);
            Control.ResumeLayout();

            return child;
        }

        public IEnumerable<Control> ChildControls { get; private set; } = null;
    }
}
