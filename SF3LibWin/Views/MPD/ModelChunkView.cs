using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Model;

namespace SF3.Win.Views.MPD {
    public class ModelChunkView : TabView {
        public ModelChunkView(string name, IMPD_File mpdFile, ModelCollection model) : base(name) {
            MPD_File = mpdFile;
            Model    = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            if (Model.ModelsHeader != null)
                CreateChild(new DataModelView("Header", Model.ModelsHeader, ngc));

            if (Model.ModelTable != null)
                CreateChild(new TableView("Models", Model.ModelTable, ngc));

            if (Model.MovableModelTable != null)
                CreateChild(new TableView("Movable Models", Model.MovableModelTable, ngc));

            CreateChild(new PDatasView("PDATAs", MPD_File, Model.PDataTable, ngc));
            CreateChild(new TableArrayView<VertexTable>("POINT[]s", Model.VertexTablesByMemoryAddress.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<PolygonTable>("POLYGON[]s", Model.PolygonTablesByMemoryAddress.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<AttrTable>("ATTR[]s", Model.AttrTablesByMemoryAddress.Values.ToArray(), ngc));

            if (Model.CollisionLinesHeader != null)
                CreateChild(new DataModelView("Collision Lines Header", Model.CollisionLinesHeader, ngc));

            if (Model.CollisionBlockTable != null)
                CreateChild(new TableView("Collision Blocks", Model.CollisionBlockTable, ngc));

            if (Model.CollisionLineIndexTablesByBlock != null)
                CreateChild(new TableArrayView<CollisionLineIndexTable>("Collision Block Line Indices", Model.CollisionLineIndexTablesByBlock.Values.ToArray(), ngc));

            return Control;
        }

        public IMPD_File MPD_File { get; }
        public ModelCollection Model { get; }
    }
}
