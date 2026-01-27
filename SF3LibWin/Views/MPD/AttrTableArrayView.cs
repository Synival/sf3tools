using System;
using CommonLib.NamedValues;
using SF3.Models.Tables.MPD.Model;
using SF3.MPD.Interfaces;

namespace SF3.Win.Views.MPD {
    public class AttrTableArrayView : ArrayView<AttrTable, AttrTableView> {
        public AttrTableArrayView(string name, AttrTable[] tables, IMPD_ModelCollection modelCollection, INameGetterContext nameGetterContext) : base(
            name,
            tables,
            "Name",
            new AttrTableView("AttrTable", null, modelCollection, nameGetterContext)
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            ElementView.Table = (AttrTable) DropdownList.SelectedValue;
        }
    }
}
