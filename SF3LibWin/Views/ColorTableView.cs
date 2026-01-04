using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables.Shared;

namespace SF3.Win.Views {
    public class ColorTableView : ControlSpaceView {
        public ColorTableView(string name, ColorTable table, INameGetterContext nameGetterContext) : base(name) {
            TableView   = new TableView("Table", table, nameGetterContext, typeof(Models.Structs.Shared.Color));
            PaletteView = new ColorTableTextureView("Texture", table);
            Table       = table;
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            CreateChild(TableView, (c) => {});
            CreateChild(PaletteView, (c) => c.Dock = DockStyle.Right, false);

            return control;
        }

        public readonly TableView TableView = null;
        public readonly ColorTableTextureView PaletteView = null;

        public ColorTable Table {
            get => (ColorTable) TableView.Table;
            set {
                if (TableView.Table != value) {
                    TableView.Table   = value;
                    PaletteView.Table = value;
                }
            }
        }
    }
}
