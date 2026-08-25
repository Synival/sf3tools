using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PC_SGL_Model_XPDataTableView : SGL_ModelTableView<PC_SGL_Model_XPDataStruct, PC_SGL_Model_XPDataTable> {
        public PC_SGL_Model_XPDataTableView(string name, ITextureMetaCollection texCollection, ITable table, INameGetterContext ngc)
        : base(name, texCollection, table, ngc, forceLighting: true) {
        }
    }
}
