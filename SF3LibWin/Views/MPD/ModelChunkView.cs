using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.Model;
using SF3.Types;

namespace SF3.Win.Views.MPD {
    public class ModelChunkView : TabView {
        public ModelChunkView(string name, IMPD_File mpdFile, ModelChunk model) : base(name) {
            MPD_File = mpdFile;
            Model    = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            var mc = (MPD_File.ModelCollections?.TryGetValue(Model.Collection, out var mcOut) == true) ? mcOut : null;

            if (Model.ModelsHeader != null)
                CreateChild(new DataModelView("Header", Model.ModelsHeader, ngc));

            if (Model.ModelInstanceTable != null)
                CreateChild(new ModelTableView("Model Instances", MPD_File, Model.ModelInstanceTable, ngc));

            if (Model.HeaderModelInstanceTable != null)
                CreateChild(new TableView("Model Instances", Model.HeaderModelInstanceTable, ngc));

            CreateChild(new PDataTableView("PDATAs", MPD_File, Model.PDataTable, ngc));
            CreateChild(new TableArrayView<VertexTable>("POINT[]s", Model.VertexTablesByMemoryAddress.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<PolygonTable>("POLYGON[]s", Model.PolygonTablesByMemoryAddress.Values.ToArray(), ngc));
            CreateChild(new AttrTableArrayView("ATTR[]s", Model.AttrTablesByMemoryAddress.Values.ToArray(), mc, ngc));

            if (Model.CollisionLinesHeader != null)
                CreateChild(new DataModelView("Collision Lines Header", Model.CollisionLinesHeader, ngc));

            if (Model.CollisionPointTable != null)
                CreateChild(new TableView("Collision Points", Model.CollisionPointTable, ngc));

            if (Model.CollisionLineTable != null)
                CreateChild(new TableView("Collision Lines", Model.CollisionLineTable, ngc));

            if (Model.CollisionBlockTable != null)
                CreateChild(new TableView("Collision Blocks", Model.CollisionBlockTable, ngc));

            if (Model.CollisionLineIndexTablesByBlock != null)
                CreateChild(new TableArrayView<CollisionLineIndexTable>("Collision Block Line Indices", Model.CollisionLineIndexTablesByBlock.Values.ToArray(), ngc));

            return Control;
        }

        public IMPD_File MPD_File { get; }
        public ModelChunk Model { get; }
    }
}
