using System;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;

namespace SF3.Win.Views.CHR {
    /// <summary>
    /// TODO: This is very poorly written!!! We need some kind of a multiview that doesn't have tabs.
    /// </summary>
    public class CHR_View : ArrayView<Sprite, SpriteView> {
        public CHR_View(string name, ICHR_File chrFile) : base(
            name,
            chrFile.SpriteTable.ToArray(),
            "DropdownName",
            new SpriteView("Sprite", null, chrFile.NameGetterContext, TabAlignment.Left)
        ) {
            Model = chrFile;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedSprite = (Sprite) DropdownList.SelectedValue;
            ElementView.Model = selectedSprite;
        }

        public ICHR_File Model { get; }
    }
}
