using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.EditorControls {
    public class EditorControlContainer : EditorControlBase, IEditorControlContainer {
        private static int s_controlIndex = 1;

        public EditorControlContainer(string name) : base(name) {
        }

        private static bool s_inSelectCousinTabs = false;

        public override Control Create() {
            var tabControl = new TabControl();

            tabControl.SuspendLayout();
            tabControl.Name = "tabsControlEditorControl" + (s_controlIndex++);
            tabControl.TabIndex = 1;
            tabControl.ResumeLayout();

            // Helper function to get a tab page with the same name.
            TabPage getTabPageByName(TabControl tabControl, string name) {
                foreach (var tabPageObj in tabControl.Controls)
                    if (tabPageObj is TabPage tabPage && tabPage.Text == name)
                        return tabPage;
                return null;
            };

            // Recurses downward from a TabControl to make the "cousin" tab with name tabNameList[0] is selected
            // when generationsDown reaches 0.
            int selectCousinTabs(TabControl ancestorTabControl, List<string> tabNameList, int generationsDown) {
                if (generationsDown == 0) {
                    var similarTab = getTabPageByName(ancestorTabControl, tabNameList[generationsDown]);
                    if (similarTab == null)
                        return 0;

                    if (ancestorTabControl.SelectedTab != similarTab)
                        ancestorTabControl.SelectedTab = similarTab;
                    return 1;
                }
                else {
                    var result = 0;
                    foreach (var ancestorTabPageObj in ancestorTabControl.Controls) {
                        var ancestorTabPage = ancestorTabPageObj as TabPage;
                        if (ancestorTabPage == null)
                            continue;

                        foreach (var ancestorTabPageControl in ancestorTabPage.Controls) {
                            var subAncestorTabControl = ancestorTabPageControl as TabControl;
                            if (subAncestorTabControl == null)
                                continue;
                            result += selectCousinTabs(subAncestorTabControl, tabNameList, generationsDown - 1);
                        }
                    }
                    return result;
                }
            };

            // This fancy block of code will make sure all "cousin" TabControl's have the same tab selected.
            // For example, when selecting a specific table in an editor for one file, similar editors for
            // other files will select the same tab as well automatically. This is useful for comparing one
            // table with another between files.
            // TODO: additional checks to make sure the TabControls are the "same kind of" TabControl
            tabControl.Selected += (s, e) => {
                if (s_inSelectCousinTabs)
                    return;
                s_inSelectCousinTabs = true;

                int generationsRemoved = 0;
                var ancestorTabControlPage = tabControl.SelectedTab;
                var tabNameList = new List<string>();

                while (ancestorTabControlPage != null) {
                    var ancestorTabControl = (TabControl) ancestorTabControlPage.Parent;
                    tabNameList.Add(ancestorTabControl.SelectedTab.Text);

                    selectCousinTabs(ancestorTabControl, tabNameList, generationsRemoved);

                    generationsRemoved++;
                    ancestorTabControlPage = ancestorTabControl.Parent as TabPage;
                }

                s_inSelectCousinTabs = false;
            };

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
