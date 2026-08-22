using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCModelChunkView : TabView {
        public PCModelChunkView(string name, PolyChar model, INameGetterContext ngc) : base(name) {
            NameGetterContext = ngc;
            HeaderView        = new DataModelView("Header", model?.ModelChunk, ngc, modelType: typeof(PCModelChunkHeader));
            XPDataListsView   = new TableView("XPData Pointers", model?.XPDataListTable, ngc, modelType: typeof(XPDataListStruct));
            XPDataTablesView  = new XPDataTablesView("XPDatas", model, ngc);
            Model             = model;
        }

        public override Control Create() {
            if (base.Create() == null)
                return null;

            var ngc = NameGetterContext;

            CreateChild(HeaderView);
            CreateChild(XPDataListsView);
            CreateChild(XPDataTablesView);

            return Control;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    HeaderView.Model       = _model?.ModelChunkHeader;
                    XPDataListsView.Table  = _model?.XPDataListTable;
                    XPDataTablesView.Model = _model;
                }
            }
        }

        public INameGetterContext NameGetterContext { get; }
        public DataModelView HeaderView { get; }
        public TableView XPDataListsView { get; }
        public XPDataTablesView XPDataTablesView { get; }
    }
}
