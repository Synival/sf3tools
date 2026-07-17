using System.Windows.Forms;
using SF3.Models.Files.X8PC;

namespace SF3.Win.Views.X8PC {
    public class X8PC_View : TabView {
        public X8PC_View(string name, IX8PC_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model?.Header != null) {
                if (Model.Header.BattleModelChunkDefTable != null)
                    CreateChild(new TableView("Chunks", Model.Header.BattleModelChunkDefTable, ngc));
            }

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX8PC_File Model { get; }
    }
}
