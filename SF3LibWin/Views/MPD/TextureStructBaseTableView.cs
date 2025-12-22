using CommonLib.NamedValues;
using SF3.Images;
using SF3.Models.Structs.Shared;
using SF3.Models.Tables;

namespace SF3.Win.Views.MPD {
    public class TextureStructBaseTableView<TStruct> : TableTextureView<TStruct, ITable<TStruct>> where TStruct : TextureStructBase {
        public TextureStructBaseTableView(string name, ITable<TStruct> table, INameGetterContext ngc, float? imageScale = null)
        : base(name, table, ngc, imageScale) {}

        protected override ITextureData GetTextureFromModel(TStruct item)
            => item;
    }
}
