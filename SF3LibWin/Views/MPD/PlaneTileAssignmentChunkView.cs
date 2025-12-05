using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class PlaneTileAssignmentChunkView : TabView {
        public PlaneTileAssignmentChunkView(string name, PlaneTileAssignmentChunk model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Tile Assignment", Model.PlaneTileTextureRowTable, ngc));

            return Control;
        }

        public PlaneTileAssignmentChunk Model { get; }
    }
}
