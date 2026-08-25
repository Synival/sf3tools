using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Win.Views.X8PC {
    public class VertexTablesView : BaseModelTablesView<VertexStruct, VertexTable> {
        public VertexTablesView(string name, PolyChar model, INameGetterContext ngc)
        : base(name, model, ngc, m => m?.VertexTablesByOffset.Values?.ToArray())
        { }
    }
}
