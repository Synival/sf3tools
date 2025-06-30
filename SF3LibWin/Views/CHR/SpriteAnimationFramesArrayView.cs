using System;
using CommonLib.NamedValues;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationFramesArrayView : ArrayView<SpriteAnimationFramesViewItem, SpriteAnimationFramesView> {
        public SpriteAnimationFramesArrayView(string name, SpriteAnimationFramesViewItem[] tables, INameGetterContext nameGetterContext) : base(
            name, tables, "Name", new SpriteAnimationFramesView("Table", null, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selection = (SpriteAnimationFramesViewItem) DropdownList.SelectedValue;
            ElementView.Model = selection;
            ElementView.UpdateTexture();
        }
    }
}
