using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class ModelsTabView : TabView {
        public ModelsTabView(string name, IMPD_File model) : base(name) {
            var modelCollections = model.ModelCollections.Values
                .Select(x => x as ModelChunk)
                .Where(x => x != null)
                .OrderBy(x => x.Collection)
                .ToList();
            var allModelInstances = modelCollections.Select(x => x.ModelInstanceTable).ToList();
            var allPDatas = modelCollections.Select(x => x.PDataTable).ToList();

            Model             = model;
            AllModelsTable    = AllPDatasTable.Create("AllModels", allPDatas);
            AllInstancesTable = AllModelInstancesTable.Create("AllInstances", allModelInstances);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            CreateChild(new PDataTableView("Models", Model, AllModelsTable, ngc));
            CreateChild(new ModelTableView("Instances", Model, AllInstancesTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
        public AllPDatasTable AllModelsTable { get; }
        public AllModelInstancesTable AllInstancesTable { get; }
    }
}
