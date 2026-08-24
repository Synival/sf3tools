using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.Shared.SGL;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.Shared.SGL;

namespace SF3.Win.Views.X8PC {
    public class XPDataTablesView : ArrayView<XPDataTable, TableView> {
        public XPDataTablesView(string name, PolyChar model, INameGetterContext ngc) : base(
            name,
            model?.XPDataTables?.ToArray(),
            "Name",
            new TableView("XPDatas", null, ngc, modelType: typeof(XPDataStruct))
        ) {
            _model = model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedPc = (XPDataTable) DropdownList.SelectedValue;
            ElementView.Table = selectedPc;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = value?.XPDataTables?.ToArray();
                }
            }
        }
    }
}
