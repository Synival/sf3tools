using System.Linq;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.Model;
using SF3.Models.Tables;
using SF3.MPD.Interfaces;

namespace SF3.Win.Views.MPD {
    public class AttrTableView : TableTextureView<AttrStruct, Table<AttrStruct>> {
        public AttrTableView(string name, Table<AttrStruct> table, IMPD_ModelCollection modelCollection, INameGetterContext ngc, float? imageScale = null)
        : base(name, table, ngc, imageScale) {
            ModelCollection = modelCollection;
        }

        public IMPD_ModelCollection ModelCollection { get; }

        protected override ITextureData GetTextureFromModel(AttrStruct item) {
            if (item == null || !item.UseTexture || ModelCollection == null || ModelCollection.Textures == null)
                return null;
            return ModelCollection.Textures.FirstOrDefault(x => x.ID == item.TextureNo);
        }
    }
}
