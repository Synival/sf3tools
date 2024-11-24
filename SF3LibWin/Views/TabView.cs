using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public class TabView : ViewBase, ITabView {
        private static int s_controlIndex = 1;

        public TabView(string name) : base(name) {
        }

        private static bool s_inSelectCousinTabs = false;

        public override Control Create() {
            _childViews = new List<IView>();

            var tabControl = new TabControl();

            tabControl.SuspendLayout();
            tabControl.Name = "tabsControlView" + (s_controlIndex++);
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
            return tabControl;
        }

        public override void Destroy() {
            Control?.Hide();

            if (_childViews != null) {
                foreach (var c in _childViews)
                    c.Destroy();
                _childViews.Clear();
                _childViews = null;
            }

            base.Destroy();
        }

        public Control CreateChild(IView childView, bool autoFill = true) {
            if (childView == null)
                return null;

            var childControl = childView.Create();
            if (childControl == null)
                return null;

            // TODO: the name should be internal, not used for display.
            var tabPage = new TabPage(childView.Name);

            TabControl.SuspendLayout();
            tabPage.SuspendLayout();

            if (autoFill)
                childControl.Dock = DockStyle.Fill;

            tabPage.AutoScroll = true;
            tabPage.Controls.Add(childControl);
            TabControl.Controls.Add(tabPage);

            tabPage.ResumeLayout();
            TabControl.ResumeLayout();

            _childViews.Add(childView);
            return childControl;
        }

        public TabControl TabControl => (TabControl) Control;

        private List<IView> _childViews = null;
        public IEnumerable<IView> ChildViews => _childViews;
    }
}
