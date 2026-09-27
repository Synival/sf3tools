using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCBoneKeyframesTableView : ArrayView<PCBoneKeyframesStruct, PCBoneKeyframesStructView> {
        public PCBoneKeyframesTableView(string name, PCBoneKeyframesTable table, INameGetterContext ngc) : base(
            name,
            table?.ToArray(),
            "Name",
            new PCBoneKeyframesStructView(name + "_Table", null, ngc)
        ) {
            _table = table;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var model = (PCBoneKeyframesStruct) DropdownList.SelectedValue;
            ElementView.Model = model;
        }

        private PCBoneKeyframesTable _table = null;
        public PCBoneKeyframesTable Table {
            get => _table;
            set {
                if (_table != value) {
                    _table = value;
                    Elements = _table?.ToArray();
                }
            }
        }
    }
}
