using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Win.Views.X8PC {
    public class PolygonTablesView : BaseModelTablesView<PolygonStruct, PolygonTable> {
        public PolygonTablesView(string name, PolyChar model, INameGetterContext ngc)
        : base(name, model, ngc, m => m?.PolygonTablesByOffset.Values?.ToArray())
        { }
    }
}
