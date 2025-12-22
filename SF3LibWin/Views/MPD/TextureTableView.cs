using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.TextureChunk;
using SF3.Models.Tables;

namespace SF3.Win.Views.MPD {
    public class TextureTableView : TextureStructBaseTableView<TextureModel> {
        public TextureTableView(string name, ITable<TextureModel> table, INameGetterContext ngc, float? imageScale = null)
        : base(name, table, ngc, imageScale) {}
    }
}
