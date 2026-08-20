using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCTextureTableView : TableTextureView<PCTexture, PCTextureTable> {
        public PCTextureTableView(string name, PCTextureTable table, INameGetterContext ngc, float? imageScale = null)
        : base(name, table, ngc, imageScale) {
        }

        protected override ITextureData GetTextureFromModel(PCTexture item) => item;
    }
}
