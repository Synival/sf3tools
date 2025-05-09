using System;
using CommonLib.NamedValues;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class TableArrayView<TTable> : ArrayView<TTable, TableView> where TTable : ITable {
        public TableArrayView(string name, TTable[] tables, INameGetterContext nameGetterContext) : base(
            name,
            tables,
            "Name",
            new TableView("Table", null, nameGetterContext, typeof(TTable).GetProperty("Rows").PropertyType.GetElementType())
        ) { }

        protected override void OnSelectValue(object sender, EventArgs args) {
            ElementView.Table = (TTable) DropdownList.SelectedValue;
        }
    }
}
