using CommonLib.NamedValues;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;

namespace SF3.Win.Views.DAT {
    public class DAT_TableImageView : TableTextureView<TextureModelBase, Table<TextureModelBase>> {
        public DAT_TableImageView(string name, Table<TextureModelBase> table, INameGetterContext nameGetterContext, float? imageScale = null)
        : base(name, table, nameGetterContext, imageScale) {}

        protected override ITexture GetTextureFromModel(TextureModelBase frame)
            => frame?.Texture;
    }
}