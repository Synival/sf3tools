using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SF3.Win.App;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void UpdateResourcesMenuActorCollections() {
            UpdateResourcesMenuSubmenu(tsmiResources_Actors, ar => ar.ActorCollections, ar => ar.ActiveActorCollection, (actors, item) => {
                AppResources.Get().ActiveActorCollection = actors;
                UpdateResourcesMenuSubmenuChecks(tsmiResources_Actors, item);
            });
        }

        private void UpdateResourcesMenuCHRs() {
            UpdateResourcesMenuSubmenu(tsmiResources_ActiveCHR, ar => ar.CHRs, ar => ar.ActiveCHR, (chr, item) => {
                AppResources.Get().ActiveCHR = chr;
                UpdateResourcesMenuSubmenuChecks(tsmiResources_ActiveCHR, item);
            });
        }

        private void UpdateResourcesMenuSubmenu<T>(
            ToolStripMenuItem parentItem,
            Func<AppResources, IEnumerable<T>> allGetter,
            Func<AppResources, T> activeGetter,
            Action<T, ToolStripMenuItem> onClick
        ) where T : class, AppResources.IResource {
            var items = parentItem.DropDown.Items;

            items.Clear();
            int itemIndex = 1;

            var allResources = allGetter(AppResources.Get());
            var activeResource = activeGetter(AppResources.Get());

            foreach (var resource in allResources) {
                var newItem = new ToolStripMenuItem(
                    $"&{itemIndex} - {resource.DisplayName}",
                    null,
                    null,
                    $"{parentItem.Name}_Item{itemIndex}"
                );

                newItem.Checked = (resource == activeResource);
                newItem.Click += (s, e) => onClick(resource, newItem);

                _ = items.Add(newItem);

                itemIndex++;
            }

            parentItem.Enabled = items.Count > 0;
        }

        private void UpdateResourcesMenuSubmenuChecks(ToolStripMenuItem parentItem, ToolStripMenuItem item) {
            foreach (var iObj in parentItem.DropDown.Items) {
                var i = (ToolStripMenuItem) iObj;
                i.Checked = i == item;
            }
        }
    }
}
