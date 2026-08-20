using System;
using System.Linq;
using System.Windows.Forms;
using SF3.Models.Files.X8PC;
using SF3.Models.Structs.X8PC;

namespace SF3.Win.Views.X8PC {
    public class X8PC_View : ArrayView<PolyChar, PolyCharView> {
        public X8PC_View(string name, IX8PC_File model): base(
            name,
            model.PolyCharTable.ToArray(),
            "Name",
            new PolyCharView(
                "PolyChar",
                null,
                model.NameGetterContext,
                tabAlignment: TabAlignment.Left
            )
        ) {
            _model = model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedPc = (PolyChar) DropdownList.SelectedValue;
            ElementView.Model = selectedPc;
        }

        private IX8PC_File _model = null;
        public IX8PC_File Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = value?.PolyCharTable?.ToArray();
                }
            }
        }
    }
}
