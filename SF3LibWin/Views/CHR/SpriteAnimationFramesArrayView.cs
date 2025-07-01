using System;
using CommonLib.NamedValues;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationFramesArrayView : ArrayView<SpriteAnimationFramesViewContext, SpriteAnimationFramesView> {
        public SpriteAnimationFramesArrayView(string name, SpriteAnimationFramesViewContext[] tables, INameGetterContext nameGetterContext) : base(
            name, tables, "Name", new SpriteAnimationFramesView("Table", null, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selection = (SpriteAnimationFramesViewContext) DropdownList.SelectedValue;
            ElementView.Context = selection;
            ElementView.UpdateTexture();
        }
    }
}
