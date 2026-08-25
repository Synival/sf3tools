using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PC_SGL_Model_XPDataTablesView : BaseModelTablesView<PC_SGL_Model_XPDataStruct, PC_SGL_Model_XPDataTable> {
        public PC_SGL_Model_XPDataTablesView(string name, PolyChar model, INameGetterContext ngc)
        : base(name, model, ngc, m => m?.XPDataTables?.ToArray())
        {}
    }
}
