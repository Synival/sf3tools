using System;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables;

namespace SF3.Win.Views.X8PC {
    public class BaseModelTablesView<TStruct, TTable> : ArrayView<TTable, TableView> where TTable : ITable {
        public BaseModelTablesView(string name, PolyChar model, INameGetterContext ngc, Func<PolyChar, TTable[]> fetcher) : base(
            name,
            fetcher(model),
            "Name",
            new TableView(name + "_Table", null, ngc, modelType: typeof(TStruct))
        ) {
            Fetcher = fetcher;
            _model = model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedPc = (TTable) DropdownList.SelectedValue;
            ElementView.Table = selectedPc;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = Fetcher(_model);
                }
            }
        }

        private Func<PolyChar, TTable[]> Fetcher { get; }
    }
}
