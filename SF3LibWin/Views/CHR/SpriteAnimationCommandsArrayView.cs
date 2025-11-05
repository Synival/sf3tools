using System;
using CommonLib.NamedValues;

namespace SF3.Win.Views.CHR {
    public class SpriteAnimationCommandsArrayView : ArrayView<SpriteAnimationCommandsViewContext, SpriteAnimationCommandsView> {
        public SpriteAnimationCommandsArrayView(string name, SpriteAnimationCommandsViewContext[] tables, INameGetterContext nameGetterContext) : base(
            name, tables, "Name", new SpriteAnimationCommandsView("Table", null, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selection = (SpriteAnimationCommandsViewContext) DropdownList.SelectedValue;
            ElementView.Context = selection;
            ElementView.UpdateImage();
        }
    }
}
