using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Files.X8PC;
using SF3.Models.Tables.X8PC;

namespace SF3.Win.Views.X8PC {
    public class PCAttackAnimChunksViewBase<T> : ArrayView<PCBoneKeyframesTable, PCBoneKeyframesTableView> where T : class, IPCAttackAnimBoneKeyframesTableContainer {
        public PCAttackAnimChunksViewBase(string name, T model, INameGetterContext ngc) : base(
            name,
            model?.AttackAnimBoneKeyframesTables?.ToArray(),
            "Name",
            new PCBoneKeyframesTableView(name + "_Table", null, ngc)
        ) {
            _model = Model;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var table = (PCBoneKeyframesTable) DropdownList.SelectedValue;
            ElementView.Table = table;
        }

        private T _model = null;
        public T Model {
            get => _model;
            set {
                if (_model != value) {
                    _model = value;
                    Elements = _model?.AttackAnimBoneKeyframesTables?.ToArray();
                }
            }
        }
    }
}
