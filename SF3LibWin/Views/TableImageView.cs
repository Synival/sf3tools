using System.Drawing;
using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public abstract class TableImageView<TTableItem, TTable> : TableImageViewBase<TTableItem, TTable, ImageView>
        where TTableItem : class, IStruct
        where TTable : class, ITable<TTableItem>
    {
        public TableImageView(string name, TTable table, INameGetterContext nameGetterContext, float? textureScale = null)
        : base(name, table, nameGetterContext, textureScale) {}

        protected override ImageView CreateImageView(float? textureScale)
            => new ImageView("Image", textureScale);

        protected override void SetImage(TTableItem item) {
            ImageView.Image = GetImageFromModel(item);
        }

        protected abstract Image GetImageFromModel(TTableItem item);
    }
}
