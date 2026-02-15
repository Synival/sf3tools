using System.Windows.Forms;
using SF3.Actors;
using SF3.Win.App;

namespace SF3.Editor.Forms {
    public partial class SF3EditorForm {
        private void UpdateResourcesMenuActorCollections() {
            var items = tsmiResources_Actors.DropDown.Items;

            items.Clear();
            int itemIndex = 1;

            var registeredActorCollections = AppResources.Get().ActorCollections;
            var activeActors = AppResources.Get().ActiveActorCollection;

            foreach (var actors in registeredActorCollections) {
                var newItem = new ToolStripMenuItem(
                    $"&{itemIndex} - ({(actors.IsBattle ? "Battle" : "Town/Scene")}) (no name yet)",
                    null,
                    null,
                    $"tsmiResources_Actors_Item{itemIndex}"
                );

                newItem.Checked = (actors == activeActors);
                newItem.Click += (s, e) => SetActiveActorsCollection(actors, newItem);

                _ = items.Add(newItem);

                itemIndex++;
            }

            tsmiResources_Actors.Enabled = items.Count > 0;
        }

        private void SetActiveActorsCollection(IActorCollection actors, ToolStripMenuItem item) {
            AppResources.Get().ActiveActorCollection = actors;

            foreach (var iObj in tsmiResources_Actors.DropDown.Items) {
                var i = (ToolStripMenuItem) iObj;
                i.Checked = i == item;
            }
        }
    }
}
