using System.Windows.Forms;
using SF3.Models.Files.X8;

namespace SF3.Win.Views.X8 {
    public class X8_View : TabView {
        public X8_View(string name, IX8_File model) : base(name) {
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

        public IX8_File Model { get; }
    }
}
