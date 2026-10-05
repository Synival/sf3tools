using SF3.Models.Files.X8AN;
using SF3.Win.Views.X8PC;

namespace SF3.Win.Views.X8AN {
    public class X8AN_View : PCAttackAnimChunksViewBase<IX8AN_File> {
        public X8AN_View(string name, IX8AN_File model) : base(name, model, model.NameGetterContext) {
        }
    }
}
