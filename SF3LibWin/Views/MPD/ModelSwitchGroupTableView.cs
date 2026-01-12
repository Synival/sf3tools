using System;
using System.Linq;
using CommonLib.NamedValues;
using SF3.Models.Structs.MPD.Main;
using SF3.Models.Tables.MPD.Main;

namespace SF3.Win.Views.MPD {
    public class ModelSwitchGroupTableView : ArrayView<ModelSwitchGroup, ModelSwitchGroupView> {
        public ModelSwitchGroupTableView(string name, ModelSwitchGroupsTable table, INameGetterContext nameGetterContext) : base(
            name,
            table?.ToArray() ?? [],
            "DropdownName",
            new ModelSwitchGroupView(nameof(ModelSwitchGroup), null, nameGetterContext)
        ) {
            _table = table;
        }

        protected override void OnSelectValue(object sender, EventArgs args) {
            var selectedSwitchGroup = (ModelSwitchGroup) DropdownList.SelectedValue;
            ElementView.SwitchGroup = selectedSwitchGroup;
        }

        private ModelSwitchGroupsTable _table = null;
        public ModelSwitchGroupsTable Table {
            get => _table;
            set {
                if (_table != value) {
                    _table = value;
                    Elements = value?.ToArray() ?? [];
                    ElementView.SwitchGroup = null;
                }
            }
        }
    }
}
