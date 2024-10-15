using BrightIdeasSoftware;
using SF3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SF3.Editor.Extensions
{
    public static class TabControlExtensions
    {
        /// <summary>
        /// Configuration of a single tab in PopulateAndShowTabs.
        /// </summary>
        public class PopulateAndShowTabConfig
        {
            public PopulateAndShowTabConfig(bool isVisible, TabPage tabPage, ObjectListView objectListView, IModelArray modelArray)
            {
                IsVisible = isVisible;
                TabPage = tabPage;
                ObjectListView = objectListView;
                ModelArray = modelArray;
            }

            public bool IsVisible { get; }
            public TabPage TabPage { get; }
            public ObjectListView ObjectListView { get; }
            public IModelArray ModelArray { get; }
            public object[] ModelObjs => ModelArray.ModelObjs;
        }

        /// <summary>
        /// Shows/hides tabs in a collection based on their 'IsVisible' setting and populates their data.
        /// All ObjectListView's provided with the tab configuration will have their objects removed via ClearObjects(),
        /// regardless of whether or not they are visible.
        /// Any ModelArray's not loaded will be loaded automatically. If the TabPage is visible, its corresponding
        /// ObjectListView will be populated with its data.
        /// If a ModelArray could not be loaded, an error message will be shown and the method will return 'false'.
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="tabConfigs"></param>
        /// <returns>'True' if the operation succeeded, 'false' if a ModelArray could not be loaded.</returns>
        public static bool PopulateAndShowTabs(this TabControl tabControl, IEnumerable<PopulateAndShowTabConfig> tabConfigs)
        {
            tabControl.SuspendLayout();
            var lastSelectedTab = tabControl.SelectedTab;
            tabControl.SelectedTab = null;

            // Reset all tabs.
            foreach (var tc in tabConfigs)
            {
                tabControl.TabPages.Remove(tc.TabPage);
                tc.ObjectListView.ClearObjects();
            }

            // Show and populate visible tabs.
            foreach (var tc in tabConfigs)
            {
                if (!tc.IsVisible)
                {
                    continue;
                }

                if (!tc.ModelArray.IsLoaded && !tc.ModelArray.Load())
                {
                    // TODO: we really should be throwing an exception here instead...
                    MessageBox.Show("Could not load " + tc.ModelArray.ResourceFile);
                    return false;
                }

                tabControl.TabPages.Add(tc.TabPage);
                tc.ObjectListView.AddObjects(tc.ModelObjs);
            }

            if (lastSelectedTab?.Parent == tabControl)
            {
                tabControl.SelectedTab = lastSelectedTab;
            }

            tabControl.ResumeLayout();
            return true;
        }
    }
}
