using System;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    /// <summary>
    /// TODO: This is very poorly written!!! We need some kind of a multiview that doesn't have tabs.
    /// </summary>
    public class CHR_View : ArrayView<Sprite, ControlSpaceView> {
        public CHR_View(string name, ICHR_File chrFile) : base(
            name,
            chrFile.SpriteTable.ToArray(),
            "DropdownName",
            new ControlSpaceView("Sprites")
        ) {
            Model = chrFile;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var selectedSprite = (Sprite) DropdownList.SelectedValue;
            foreach (var sprite in Model.SpriteTable) {
                ElementView.CreateChild(new SpriteView(sprite.Name, sprite, TabAlignment.Left), (c) => {
                    if (sprite != selectedSprite)
                        c.Hide();
                });
            }

            return Control;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedSprite = (Sprite) DropdownList.SelectedValue;
            foreach (var viewObj in ElementView.ChildViews) {
                var view = (SpriteView) viewObj;
                if (view.Model == selectedSprite) {
                    view.RefreshContent();
                    view.Control.Show();
                }
                else
                    view.Control.Hide();
            }
        }

        public ICHR_File Model { get; }
    }
}
