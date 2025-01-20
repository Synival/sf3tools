using System.Windows.Forms;

namespace SF3.Win.Views.MPD {
    public class ModelChunkView : TabView {
        public ModelChunkView(string name, Models.Files.MPD.Models model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Header", Model.ModelsHeaderTable, ngc));
            CreateChild(new TableView("Models", Model.ModelTable, ngc));
            CreateChild(new TableView("PDATAs", Model.PDataTable, ngc));

            return Control;
        }

        public Models.Files.MPD.Models Model { get; }
    }
}
