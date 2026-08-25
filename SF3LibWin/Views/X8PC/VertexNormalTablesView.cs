using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Win.Views.X8PC {
    public class VertexNormalTablesView : BaseModelTablesView<VertexNormalStruct, VertexNormalTable> {
        public VertexNormalTablesView(string name, PolyChar model, INameGetterContext ngc)
        : base(name, model, ngc, m => m?.VertexNormalTablesByOffset.Values?.ToArray())
        { }
    }
}
