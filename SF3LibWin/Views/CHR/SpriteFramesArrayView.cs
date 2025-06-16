using System;
using CommonLib.NamedValues;
using SF3.Models.Tables.CHR;

namespace SF3.Win.Views.CHR {
    public class SpriteFramesArrayView : ArrayView<SpriteFrameTable, SpriteFramesView> {
        public SpriteFramesArrayView(string name, SpriteFrameTable[] tables, INameGetterContext nameGetterContext) : base(
            name,
            tables,
            "Name",
            new SpriteFramesView("Table", null, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            ElementView.TableView.Table = (SpriteFrameTable) DropdownList.SelectedValue;
            ElementView.UpdateTexture();
        }
    }
}
