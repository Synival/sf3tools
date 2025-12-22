using System.Linq;
using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.MPD;

namespace SF3.Win.Views.MPD {
    public class TextureIDTableView : TableTextureView<TextureIDModel, TextureIDTable> {
        public TextureIDTableView(string name, TextureIDTable table, INameGetterContext nameGetterContext, IMPD_ModelCollection collection)
        : base(name, table, nameGetterContext) {
            Collection = collection;
        }

        protected override ITextureData GetTextureFromModel(TextureIDModel model)
            => (model == null) ? null : Collection.Textures.FirstOrDefault(x => x.ID == model.TextureID);

        public IMPD_ModelCollection Collection { get; }
    }
}