using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class TableDictionaryView<TKey, TTable> : ControlSpaceView {
        public TableDictionaryView(string name, Dictionary<TKey, TTable> tableDict, INameGetterContext nameGetterContext) : base(name) {
            TableDict = tableDict;
            var elementType = typeof(TTable).GetProperty("Rows").PropertyType.GetElementType();
            TableView = new TableView("Table", null, nameGetterContext, elementType);
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            DropdownList = new ComboBox();
            DropdownList.Width = 400;
            DropdownList.DataSource = new BindingSource(TableDict, null);
            DropdownList.DisplayMember = "Key";
            DropdownList.ValueMember = "Value";
            DropdownList.SelectedValueChanged += (s, e) => TableView.Table = (IBaseTable) DropdownList.SelectedValue;
            control.Controls.Add(DropdownList);

            CreateChild(TableView, (c) => {}, autoFill: false);

            Control.Resize += (s, e) => {
                var tableControl = TableView.Control;
                tableControl.SetBounds(0, DropdownList.Bottom + 8, Control.Width, Control.Height - DropdownList.Height - 8);
            };

            return control;
        }

        public override void Destroy() {
            Control?.Controls.Remove(DropdownList);
            DropdownList = null;
            base.Destroy();
        }

        public Dictionary<TKey, TTable> TableDict { get; }

        public ComboBox DropdownList { get; private set; } = null;
        public TableView TableView { get; }
    }
}
