using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class ModelsTabView : TabView {
        public ModelsTabView(string name, IMPD_File model) : base(name) {
            var allModels = model.ModelCollections.Where(x => x != null).Select(x => x.ModelTable).ToList();
            var allPDatas = model.ModelCollections.Where(x => x != null).Select(x => x.PDataTable).ToList();

            AllModelsTable = AllModelsTable.Create("AllModelInstances", allModels);
            AllPDatasTable = AllPDatasTable.Create("AllPDatas", allPDatas);
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new ModelTableView("Models", Model, AllModelsTable, ngc));
            CreateChild(new PDataTableView("PDATAs", Model, AllPDatasTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
        public AllModelsTable AllModelsTable { get; }
        public AllPDatasTable AllPDatasTable { get; }
    }
}
