using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.MPD;
using SF3.Models.Tables;

namespace SF3.Win.Views.MPD {
    public class ModelSwitchGroupsView : TabView {
        public ModelSwitchGroupsView(string name, IMPD_File model) : base(name) {
            Model = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = Model.NameGetterContext;
            CreateChild(new TableView("Groups", Model.ModelSwitchGroupsTable, ngc));
            CreateChild(new TableArrayView<ModelIDTable>("Enable IDs", Model.ModelsEnabledGroupsByAddr.Values.ToArray(), ngc));
            CreateChild(new TableArrayView<ModelIDTable>("Disable IDs", Model.ModelsDisabledGroupsByAddr.Values.ToArray(), ngc));

            return Control;
        }

        public IMPD_File Model { get; }
    }
}
