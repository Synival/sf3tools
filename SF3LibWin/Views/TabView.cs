using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.Win.Controls;

namespace SF3.Win.Views {
    public class TabView : ViewBase, ITabView {
        private static int s_controlIndex = 1;

        public TabView(string name, bool lazyLoad = true, TabAlignment tabAlignment = TabAlignment.Top) : base(name) {
            LazyLoad = lazyLoad;
            TabAlignment = tabAlignment;
        }

        private static bool s_inSelectCousinTabs = false;

        public override Control Create() {
            _childViews = new List<IView>();

            var newTabControl = new EnhancedTabControl(TabAlignment);

            newTabControl.SuspendLayout();
            newTabControl.Name = "tabsControlView" + (s_controlIndex++);
            newTabControl.TabIndex = 1;
            newTabControl.ResumeLayout();

            // Helper function to get a tab page with the same name.
            TabPage getTabPageByName(TabView tabView, string name) {
                var tabControl = tabView?.TabControl;
                if (tabControl == null)
                    return null;

                // We can't continue if there are zero tabs with that requested name, or two or more tabs with the same name.
                var tabsWithName = tabControl.Controls
                    .Cast<object>()
                    .Where(x => x is TabPage)
                    .Cast<TabPage>()
                    .Where(x => x.Text == name)
                    .ToArray();

                return tabsWithName.Length == 1 ? tabsWithName[0] : null;
            };

            // Recurses downward from a TabControl to make the "cousin" tab with name tabNameList[0] is selected
            // when generationsDown reaches 0.
            int selectCousinTabs(TabView ancestorTabView, List<string> tabNameList, TabView lastTabView, int generationsDown) {
                var similarTab = getTabPageByName(ancestorTabView, tabNameList[generationsDown]);
                if (similarTab == null)
                    return 0;

                // TODO: Create it!
                if (!ancestorTabView.IsCreated)
                    return 0;

                if (generationsDown == 0) {
                    if (ancestorTabView.TabControl.SelectedTab != similarTab)
                        ancestorTabView.TabControl.SelectedTab = similarTab;
                    return 1;
                }
                else {
                    var result = 0;
                    foreach (var ancestorChildView in ancestorTabView.ChildViews) {
                        TabView ancestorChildTabView = null;
                        if (ancestorChildView is TabView tv)
                            ancestorChildTabView = tv;
                        else if (ancestorChildView is FileView fv)
                            ancestorChildTabView = fv.ActualView as TabView;

                        if (ancestorChildTabView != null && ancestorChildTabView != lastTabView)
                            result += selectCousinTabs(ancestorChildTabView, tabNameList, ancestorTabView, generationsDown - 1);
                    }
                    return result;
                }
            };

            // This fancy block of code will make sure all "cousin" TabControl's have the same tab selected.
            // For example, when selecting a specific table in an editor for one file, similar editors for
            // other files will select the same tab as well automatically. This is useful for comparing one
            // table with another between files.
            newTabControl.Selected += (s, e) => {
                if (s_inSelectCousinTabs)
                    return;
                s_inSelectCousinTabs = true;

                int generationsRemoved = 0;
                var tabNameList = new List<string>();

                TabView lastTabView = null;
                for (var ancestorTabView = this; ancestorTabView?.TabControl?.SelectedTab != null; ancestorTabView = ancestorTabView.Parent as TabView) {
                    tabNameList.Add(ancestorTabView.TabControl.SelectedTab.Text);
                    if (generationsRemoved > 0)
                        selectCousinTabs(ancestorTabView, tabNameList, lastTabView, generationsRemoved);
                    generationsRemoved++;
                    lastTabView = ancestorTabView;
                }

                s_inSelectCousinTabs = false;
            };

            Control = newTabControl;
            return newTabControl;
        }

        public override void Destroy() {
            if (!IsCreated)
                return;

            Control?.Hide();

            if (_childViews != null) {
                foreach (var c in _childViews)
                    c.Destroy();
                _childViews.Clear();
                foreach (var c in _childViews)
                    c.Parent = null;
                _childViews = null;
            }

            var tabPages = new List<TabPage>();
            foreach (var tabPage in TabControl.TabPages)
                tabPages.Add((TabPage) tabPage);

            TabControl.TabPages.Clear();
            _tabsForChildren.Clear();

            foreach (var tabPage in tabPages)
                tabPage.Dispose();

            base.Destroy();
        }

        public delegate Control CreateCustomChildDelegate(string name, string text);

        public void CreateChild(IView childView, Action<Control> onCreate = null, bool autoFill = true)
            => CreateCustomChild(childView, onCreate, autoFill, (name, text) => new DarkModeTabPage(text) { Name = name, AutoScroll = true });

        public void CreateCustomChild(IView childView, Action<Control> onCreate, bool autoFill, CreateCustomChildDelegate createTabDelegate) {
            if (childView == null)
                return;

            Control childControl = null;

            var tabName = TabControl.Name + "_Tab_" + TabControl.TabPages.Count + 1;
            var tabPage = createTabDelegate(tabName, childView.Name);
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
            childView.Parent = this;
            _tabsForChildren[childView] = (TabPage) tabPage;
        }

        public bool RemoveChild(IView child) {
            if (!_childViews.Contains(child) || !_tabsForChildren.ContainsKey(child))
                return false;

            var tabPage = _tabsForChildren[child];
            TabControl.TabPages.Remove(tabPage);
            _tabsForChildren.Remove(child);

            child.Destroy();
            _childViews.Remove(child);
            child.Parent = null;

            tabPage.Dispose();
            return true;
        }

        public override void RefreshContent() {
            if (!IsCreated)
                return;

            foreach (var child in ChildViews)
                child.RefreshContent();
        }

        public IView GetChildViewForTabPage(TabPage page)
            => _tabsForChildren.FirstOrDefault(x => x.Value == page).Key;

        public bool LazyLoad { get; set; }
        public TabAlignment TabAlignment { get; set; }

        public TabControl TabControl => (TabControl) Control;

        private List<IView> _childViews = null;
        private Dictionary<IView, TabPage> _tabsForChildren = [];
        public IEnumerable<IView> ChildViews => _childViews;
    }
}
