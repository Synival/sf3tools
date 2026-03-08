using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SF3.Win.App;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void UpdateSceneMenuActorCollections() {
            UpdateSceneMenuSubmenu(tsmiScene_Actors, ar => ar.ActorCollections, ar => ar.ActiveActorCollection, (actors, item) => {
                AppScene.Get().ActiveActorCollection = actors;
                UpdateSceneMenuSubmenuChecks(tsmiScene_Actors, item);
            });
        }

        private void UpdateSceneMenuCHRs() {
            UpdateSceneMenuSubmenu(tsmiScene_ActiveCHR, ar => ar.CHRs, ar => ar.ActiveCHR, (chr, item) => {
                AppScene.Get().ActiveCHR = chr;
                UpdateSceneMenuSubmenuChecks(tsmiScene_ActiveCHR, item);
            });
        }

        private void UpdateSceneMenuIconCollections() {
            UpdateSceneMenuSubmenu(tsmiScene_ActiveIcons, ar => ar.IconCollections, ar => ar.ActiveIconCollection, (icons, item) => {
                AppScene.Get().ActiveIconCollection = icons;
                UpdateSceneMenuSubmenuChecks(tsmiScene_ActiveIcons, item);
            });
        }

        private void UpdateSceneMenuSubmenu<T>(
            ToolStripMenuItem parentItem,
            Func<AppScene, IEnumerable<T>> allGetter,
            Func<AppScene, T> activeGetter,
            Action<T, ToolStripMenuItem> onClick
        ) where T : class, AppScene.IResource {
            var items = parentItem.DropDown.Items;

            items.Clear();
            int itemIndex = 1;

            var allResources = allGetter(AppScene.Get());
            var activeResource = activeGetter(AppScene.Get());

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

        private void UpdateSceneMenuSubmenuChecks(ToolStripMenuItem parentItem, ToolStripMenuItem item) {
            foreach (var iObj in parentItem.DropDown.Items) {
                var i = (ToolStripMenuItem) iObj;
                i.Checked = i == item;
            }
        }
    }
}
