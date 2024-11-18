using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.EditorControls {
    public class EditorControlContainer : EditorControlBase, IEditorControlContainer {
        private static int s_controlIndex = 1;

        public EditorControlContainer(string name) : base(name) {
        }

        public override Control Create() {
            var tabControl = new TabControl();

            tabControl.SuspendLayout();
            tabControl.Name = "tabsControlEditorControl" + (s_controlIndex++);
            tabControl.TabIndex = 1;
            tabControl.ResumeLayout();

            Control = tabControl;
            TabControl = tabControl;
            return tabControl;
        }

        public override void Destroy() {
            foreach (var c in ChildControls)
                c.Dispose();
            _childControls.Clear();

            base.Destroy();
            TabControl = null;
        }

        public Control CreateChild(IEditorControl child, bool autoFill = true)
            => CreateChild(child.Name, child.Create(), autoFill);

        public Control CreateChild(string name, Control child, bool autoFill = true) {
            if (child == null)
                return null;

            var tabPage = new TabPage(name);

            TabControl.SuspendLayout();
            tabPage.SuspendLayout();

            if (autoFill)
                child.Dock = DockStyle.Fill;

            tabPage.AutoScroll = true;
            tabPage.Controls.Add(child);
            TabControl.Controls.Add(tabPage);

            tabPage.ResumeLayout();
            TabControl.ResumeLayout();

            return child;
        }

        private TabControl TabControl { get; set; } = null;

        private List<Control> _childControls = new List<Control>();

        public IEnumerable<Control> ChildControls => _childControls;
    }
}
