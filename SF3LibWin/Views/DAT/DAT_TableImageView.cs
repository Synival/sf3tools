using System.Drawing;
using CommonLib.NamedValues;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Win.Extensions;

namespace SF3.Win.Views.DAT {
    public class DAT_TableImageView : TableImageView<TextureModelBase, Table<TextureModelBase>> {
        public DAT_TableImageView(string name, Table<TextureModelBase> table, INameGetterContext nameGetterContext, float? imageScale = null)
        : base(name, table, nameGetterContext, imageScale) {}

        protected override Image GetImageFromModel(TextureModelBase frame)
            => frame?.Texture?.CreateBitmapARGB1555();
    }
}