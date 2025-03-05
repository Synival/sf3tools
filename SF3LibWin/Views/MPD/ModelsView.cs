using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables.MPD.TextureCollection;

namespace SF3.Win.Views.MPD {
    public class ModelsView : TabView {
        public ModelsView(string name, IMPD_File model) : base(name) {
            var allPDatas = model.ModelCollections.Where(x => x != null).Select(x => x.PDataTable).ToList();
            AllPDatasTable = AllPDatasTable.Create("AllPDatas", allPDatas);
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new PDatasView("PDatas", Model, AllPDatasTable, ngc));

            return Control;
        }

        public IMPD_File Model { get; }
        public AllPDatasTable AllPDatasTable { get; }
    }
}
