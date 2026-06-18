using System.Windows.Forms;
using SF3.Models.Files.X012;
using SF3.Models.Tables.X012;

namespace SF3.Win.Views.X012 {
    public class X012_View : TabView {
        public X012_View(string name, IX012_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            if (Model.TileMovementTable != null)
                CreateChild(new TableView("Tile Data", Model.TileMovementTable, ngc));
            if (Model.ClassTargetPriorityTables != null)
                CreateChild(new TableArrayView<ClassTargetPriorityTable>("Class Move Target Priorities", Model.ClassTargetPriorityTables, ngc));
            if (Model.ClassAttackScoreBonusTables != null)
                CreateChild(new TableArrayView<ClassAttackScoreBonusTable>("Class Attack Score Bonuses", Model.ClassAttackScoreBonusTables, ngc));

            CreateChild(new TechnicalView("Technical Info", Model));

            return Control;
        }

        public IX012_File Model { get; }
    }
}
