using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAttackAnimChunksView : PCAttackAnimChunksViewBase<PolyChar> {
        public PCAttackAnimChunksView(string name, PolyChar model, INameGetterContext ngc) : base(name, model, ngc) {
        }
    }
}
