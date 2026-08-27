using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PC_XPDataTablesView : ArrayView<PC_XPDataTable, PC_XPDataTableView> {
        public PC_XPDataTablesView(string name, PolyChar model, INameGetterContext ngc) : base(
            name,
            model?.XPDataTables?.ToArray(),
            "Name",
            new PC_XPDataTableView(name + "_Table", model, null, ngc)
        ) {
            _model = model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedPc = (PC_XPDataTable) DropdownList.SelectedValue;
            ElementView.SetTable(_model, selectedPc);
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = _model?.XPDataTables?.ToArray();
                }
            }
        }
    }
}
