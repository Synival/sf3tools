using System.Linq;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.MPD.Interfaces;

namespace SF3.Win.Views.MPD {
    public class IgnoredTextureTableView : TableTextureView<TextureIDStruct, IgnoredTextureTable> {
        public IgnoredTextureTableView(string name, IgnoredTextureTable table, INameGetterContext nameGetterContext, IMPD_ModelCollection collection)
        : base(name, table, nameGetterContext) {
            Collection = collection;
        }

        protected override ITextureData GetTextureFromModel(TextureIDStruct model)
            => (model == null) ? null : Collection.Textures.FirstOrDefault(x => x.TextureID == model.TextureID);

        public IMPD_ModelCollection Collection { get; }
    }
}