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
            AllPDatasTable    = AllMPD_SGL_Model_PDatasTable.Create("AllModels", allPDatas);
            AllInstancesTable = AllMPD_ModelInstancesTable.Create("AllInstances", allModelInstances);
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;

            CreateChild(new MPD_SGL_Model_PDataTableView("Models", Model, AllPDatasTable, ngc));
            CreateChild(new MPD_ModelInstanceTableView("Instances", Model, AllInstancesTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
        public AllMPD_SGL_Model_PDatasTable AllPDatasTable { get; }
        public AllMPD_ModelInstancesTable AllInstancesTable { get; }
    }
}
