using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAttackAnimChunksView : ArrayView<PCBoneKeyframesTable, PCBoneKeyframesTableView> {
        public PCAttackAnimChunksView(string name, PolyChar model, INameGetterContext ngc) : base(
            name,
            model?.AttackAnimBoneKeyframesTable?.ToArray(),
            "Name",
            new PCBoneKeyframesTableView(name + "_Table", null, ngc)
        ) {
            _model = Model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var table = (PCBoneKeyframesTable) DropdownList.SelectedValue;
            ElementView.Table = table;
        }

        private PolyChar _model = null;
        public PolyChar Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = _model?.AttackAnimBoneKeyframesTable?.ToArray();
                }
            }
        }
    }
}
