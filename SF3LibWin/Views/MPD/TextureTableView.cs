using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;

namespace SF3.Win.Views.MPD {
    public class TextureTableView : TableTextureView<TextureModel, ITable<TextureModel>> {
        public TextureTableView(string name, ITable<TextureModel> table, INameGetterContext ngc, float? textureScale = null)
        : base(name, table, ngc, textureScale) {}

        protected override ITexture GetTextureFromModel(TextureModel item)
            => item?.Texture;
    }
}
