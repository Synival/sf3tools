using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    public class TabView : ViewBase, ITabView {
        private static int s_controlIndex = 1;

        public TabView(string name, bool lazyLoad = true) : base(name) {
            LazyLoad = lazyLoad;
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
            if (!IsCreated)
                return;

            Control?.Hide();

            if (_childViews != null) {
                foreach (var c in _childViews)
                    c.Destroy();
                _childViews.Clear();
                _childViews = null;
            }

            base.Destroy();
        }

        public void CreateChild(IView childView, Action<Control> onCreate = null, bool autoFill = true)
            => CreateCustomChild(childView, onCreate, autoFill, (name) => new TabPage(name) { AutoScroll = true });

        public void CreateCustomChild(IView childView, Action<Control> onCreate, bool autoFill, Func<string, Control> createTabDelegate) {
            if (childView == null)
                return;

            Control childControl = null;

            // TODO: the name should be internal, not used for display.
            var tabPage = createTabDelegate(childView.Name);
            TabControl.Controls.Add(tabPage);

            void ChildViewCreate() {
                if (childView.IsCreated)
                    return;

                if ((childControl = childView.Create()) == null) {
                    onCreate?.Invoke(null);
                    return;
                }

                TabControl.SuspendLayout();
                tabPage.SuspendLayout();

                if (autoFill)
                    childControl.Dock = DockStyle.Fill;

                tabPage.Controls.Add(childControl);
                tabPage.ResumeLayout();
                TabControl.ResumeLayout();

                onCreate?.Invoke(childControl);
            }

            if (LazyLoad) {
                void CreateIfConditionsAreRight() {
                    if (!childView.IsCreated && TabControl.Visible && TabControl.SelectedTab == tabPage && tabPage.Visible)
                        ChildViewCreate();
                }

                TabControl.Selected       += (s, e) => CreateIfConditionsAreRight();
                TabControl.VisibleChanged += (s, e) => CreateIfConditionsAreRight();
                tabPage.VisibleChanged    += (s, e) => CreateIfConditionsAreRight();

                CreateIfConditionsAreRight();
            }
            else
                ChildViewCreate();

            _childViews.Add(childView);
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            foreach (var child in ChildViews)
                child.RefreshContent();
        }

        public bool LazyLoad { get; set; }
        public TabControl TabControl => (TabControl) Control;

        private List<IView> _childViews = null;
        public IEnumerable<IView> ChildViews => _childViews;
    }
}
