using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCBoneWrapperTableView : SGL_ModelInstanceCollectionTableView<PCBoneWrapperStruct, PCBoneWrapperTable> {
        public PCBoneWrapperTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc)
        : base(name, texCollection, table, ngc, forceLighting: true, size: 1.5f, zoom: 1.25f) {
        }
    }
}
