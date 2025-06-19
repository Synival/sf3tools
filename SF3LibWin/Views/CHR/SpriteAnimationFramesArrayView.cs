using System;
using CommonLib.NamedValues;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationFramesArrayView : ArrayView<SpriteAnimationWithFrames, SpriteAnimationFramesView> {
        public SpriteAnimationFramesArrayView(string name, SpriteAnimationWithFrames[] tables, INameGetterContext nameGetterContext) : base(
            name,
            tables,
            "Name",
            new SpriteAnimationFramesView("Table", null, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selection = (SpriteAnimationWithFrames) DropdownList.SelectedValue;
            ElementView.Model      = selection.AnimationFrameTable;
            ElementView.FrameTable = selection.FrameTable;
            ElementView.UpdateTexture();
        }
    }
}
