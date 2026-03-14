using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SF3.Win.App;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void UpdateActiveResourcesMenuActorCollections() {
            UpdateActiveResourcesMenuSubmenu(tsmiActiveResources_Actors, ar => ar.ActorCollections, ar => ar.ActiveActorCollection, (actors, item) => {
                _appScene.ActiveActorCollection = actors;
                UpdateActiveResourcesMenuSubmenuChecks(tsmiActiveResources_Actors, item);
            });
        }

        private void UpdateActiveResourcesMenuCHRs() {
            UpdateActiveResourcesMenuSubmenu(tsmiActiveResources_ActiveCHR, ar => ar.CHRs, ar => ar.ActiveCHR, (chr, item) => {
                _appScene.ActiveCHR = chr;
                UpdateActiveResourcesMenuSubmenuChecks(tsmiActiveResources_ActiveCHR, item);
            });
        }

        private void UpdateActiveResourcesMenuIconCollections() {
            UpdateActiveResourcesMenuSubmenu(tsmiActiveResources_ActiveIcons, ar => ar.IconCollections, ar => ar.ActiveIconCollection, (icons, item) => {
                _appScene.ActiveIconCollection = icons;
                UpdateActiveResourcesMenuSubmenuChecks(tsmiActiveResources_ActiveIcons, item);
            });
        }

        private void UpdateActiveResourcesMenuSubmenu<T>(
            ToolStripMenuItem parentItem,
            Func<AppScene, IEnumerable<T>> allGetter,
            Func<AppScene, T> activeGetter,
            Action<T, ToolStripMenuItem> onClick
        ) where T : class, AppScene.IResource {
            var items = parentItem.DropDown.Items;

            items.Clear();
            int itemIndex = 1;

            var allResources = allGetter(_appScene);
            var activeResource = activeGetter(_appScene);

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

        private void UpdateActiveResourcesMenuSubmenuChecks(ToolStripMenuItem parentItem, ToolStripMenuItem item) {
            foreach (var iObj in parentItem.DropDown.Items) {
                var i = (ToolStripMenuItem) iObj;
                i.Checked = i == item;
            }
        }
    }
}
