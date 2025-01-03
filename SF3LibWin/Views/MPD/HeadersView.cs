using System.Windows.Forms;
using SF3.Models.Files.MPD;

namespace SF3.Win.Views.MPD {
    public class HeadersView : TabView {
        public HeadersView(string name, IMPD_File model) : base(name) {
            Model = model;            
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Header",          Model.MPDHeader,           ngc));
            CreateChild(new TableView("Chunk Header",    Model.ChunkHeader,         ngc));
            CreateChild(new TableView("Light Direction", Model.LightDirectionTable, ngc));
            CreateChild(new TableView("Offset 3 Table",  Model.Offset3Table,        ngc));
            CreateChild(new TableView("Offset 4 Table",  Model.Offset4Table,        ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
