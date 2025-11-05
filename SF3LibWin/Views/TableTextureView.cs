using CommonLib.NamedValues;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public abstract class TableTextureView<TTableItem, TTable> : TableImageViewBase<TTableItem, TTable, TextureView>
        where TTableItem : class, IStruct
        where TTable : class, ITable<TTableItem>
    {
        public TableTextureView(string name, TTable table, INameGetterContext ngc, float? textureScale = null)
        : base(name, table, ngc, textureScale) {}

        protected override TextureView CreateImageView(float? textureScale)
            => new TextureView("Texture", textureScale);

        protected override void SetImage(TTableItem item)
            => ImageView.Texture = GetTextureFromModel(item);

        protected abstract ITexture GetTextureFromModel(TTableItem item);
    }
}
