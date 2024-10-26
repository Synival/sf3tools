using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using SF3.Models;
using static CommonLib.Win.Utils.MessageUtils;

namespace SF3.Editor.Extensions {
    public static class TabControlExtensions {
        /// <summary>
        /// Adds or removes tabs based on their 'value' in a key-value Dictionary.
        /// </summary>
        /// <param name="tabControl">The control whose tabs should be toggled.</param>
        /// <param name="tabPages">A key-value dictionary of tabs and whether or not they should be visible.</param>
        public static void ToggleTabs(this TabControl tabControl, Dictionary<TabPage, bool> tabPages) {
            var tabsToRemove = tabControl.TabPages
                .Cast<TabPage>()
                .Intersect(tabPages.Where(x => !x.Value).Select(x => x.Key))
                .ToArray();

            var tabsToAdd = tabPages
                .Where(x => x.Value)
                .Select(x => x.Key)
                .Except(tabControl.TabPages.Cast<TabPage>())
                .ToArray();

            // If there aren't any changes to be maid, abort early.
            if (tabsToRemove.Length == 0 && tabsToAdd.Length == 0)
                return;

            // Deselect the selected tab (it screws up rendering).
            var lastSelectedTab = tabControl.SelectedTab;
            tabControl.SelectedTab = null;

            // Prevent redraw/layout updates while modifying tabs.
            tabControl.SuspendLayout();

            // Remove all tabs, so we don't have to worry about adding them back in-order.
            tabsToRemove = tabControl.TabPages
                .Cast<TabPage>()
                .Intersect(tabPages.Select(x => x.Key))
                .ToArray();
            foreach (var tab in tabsToRemove)
                tabControl.TabPages.Remove(tab);

            // Add enabled tabs.
            tabsToAdd = tabPages
                .Where(x => x.Value)
                .Select(x => x.Key)
                .Except(tabControl.TabPages.Cast<TabPage>())
                .ToArray();
            tabControl.TabPages.AddRange(tabsToAdd);

            // Done - resume redrawing and re-select the tab, if possible.
            tabControl.ResumeLayout();
            if (lastSelectedTab?.Parent == tabControl)
                tabControl.SelectedTab = lastSelectedTab;
        }

        /// <summary>
        /// Configuration of a single tab for PopulateTabs().
        /// </summary>
        public class PopulateTabConfig {
            public PopulateTabConfig(TabPage tabPage, ObjectListView objectListView, IModelArray modelArray) {
                TabPage = tabPage;
                ObjectListView = objectListView;
                ModelArray = modelArray;
            }

            public TabPage TabPage { get; }
            public ObjectListView ObjectListView { get; }
            public IModelArray ModelArray { get; }
            public object[] ModelObjs => ModelArray.ModelObjs;
        }

        /// <summary>
        /// Populates contents of tabs.
        /// All ObjectListView's provided with the tab configuration will have their objects removed via ClearObjects(),
        /// regardless of whether or not they are visible.
        /// Any ModelArray's not loaded will be loaded automatically. If the TabPage is visible, its corresponding
        /// ObjectListView will be populated with its data.
        /// If a ModelArray could not be loaded, an error message will be shown and the method will return 'false'.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="tabConfigs"></param>
        /// <returns>'True' if the operation succeeded, 'false' if a ModelArray could not be loaded.</returns>
        public static bool PopulateTabs(this TabControl tabControl, IEnumerable<PopulateTabConfig> tabConfigs) {
            // Show and populate visible tabs.
            foreach (var tc in tabConfigs) {
                tc.ObjectListView.ClearObjects();
                if (!tc.ModelArray.IsLoaded && !tc.ModelArray.Load()) {
                    // TODO: we really should be throwing an exception here instead...
                    ErrorMessage("Could not load " + tc.ModelArray.ResourceFile);
                    return false;
                }
                tc.ObjectListView.AddObjects(tc.ModelObjs);
            }

            return true;
        }

        /// <summary>
        /// Shows/hides tabs in a collection based on their 'IsVisible' setting and populates their data.
        /// Also populates the contents of tabs.
        /// All ObjectListView's provided with the tab configuration will have their objects removed via ClearObjects(),
        /// regardless of whether or not they are visible.
        /// Any ModelArray's not loaded will be loaded automatically. If the TabPage is visible, its corresponding
        /// ObjectListView will be populated with its data.
        /// If a ModelArray could not be loaded, an error message will be shown and the method will return 'false'.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="tabConfigs"></param>
        /// <returns>'True' if the operation succeeded, 'false' if a ModelArray could not be loaded.</returns>
        public static bool PopulateAndToggleTabs(this TabControl tabControl, IEnumerable<PopulateTabConfig> tabConfigs) {
            tabControl.ToggleTabs(tabConfigs.ToDictionary(x => x.TabPage, x => x.ModelArray != null));
            var populateTabConfigs = tabConfigs
                .Where(x => x.ModelArray != null)
                .Cast<PopulateTabConfig>()
                .ToList();
            return tabControl.PopulateTabs(populateTabConfigs);
        }
    }
}
