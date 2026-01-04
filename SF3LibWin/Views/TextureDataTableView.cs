using CommonLib.NamedValues;
using SF3.Imaging;
using SF3.Models.Structs;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class TextureDataTableView<TTableItem, TTable> : TableTextureView<TTableItem, TTable>
        where TTableItem : class, IStruct, ITextureData
        where TTable : class, ITable<TTableItem>
    {
        public TextureDataTableView(string name, TTable table, INameGetterContext ngc, float? imageScale = null) : base(name, table, ngc, imageScale) {}
        protected override ITextureData GetTextureFromModel(TTableItem item) => item;
    }
}
