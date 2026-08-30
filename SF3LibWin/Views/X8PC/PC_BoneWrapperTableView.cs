using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PC_BoneWrapperTableView : SGL_ModelCollectionTableView<PC_BoneWrapperStruct, PC_BoneWrapperTable> {
        public PC_BoneWrapperTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc)
        : base(name, texCollection, table, ngc, forceLighting: true, size: 1.5f, zoom: 1.25f) {
        }
    }
}
