using System.Linq;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD;
using SF3.Models.Tables.MPD;
using SF3.MPD;

namespace SF3.Win.Views.MPD {
    public class IndexedTextureTableView : TableTextureView<TextureIDStruct, IndexedTextureTable> {
        public IndexedTextureTableView(string name, IndexedTextureTable table, INameGetterContext nameGetterContext, IMPD_ModelCollection collection)
        : base(name, table, nameGetterContext) {
            Collection = collection;
        }

        protected override ITextureData GetTextureFromModel(TextureIDStruct model)
            => (model == null) ? null : Collection.Textures.FirstOrDefault(x => x.ID == model.TextureID);

        public IMPD_ModelCollection Collection { get; }
    }
}