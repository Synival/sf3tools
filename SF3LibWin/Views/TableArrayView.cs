using System.Windows.Forms;
using CommonLib.NamedValues;
using SF3.Models.Tables;

namespace SF3.Win.Views {
    public class TableArrayView<TTable> : ControlSpaceView {
        public TableArrayView(string name, TTable[] tables, INameGetterContext nameGetterContext) : base(name) {
            Tables = tables;
            var elementType = typeof(TTable).GetProperty("Rows").PropertyType.GetElementType();
            TableView = new TableView("Table", null, nameGetterContext, elementType);
        }

        public override Control Create() {
            var control = base.Create();
            if (control == null)
                return control;            

            DropdownList = new ComboBox();
            DropdownList.Width = 400;
            DropdownList.DataSource = new BindingSource(Tables, null);
            DropdownList.DisplayMember = "Name";
            DropdownList.SelectedValueChanged += (s, e) => TableView.Table = (ITable) DropdownList.SelectedValue;
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

        public TTable[] Tables { get; }

        public ComboBox DropdownList { get; private set; } = null;
        public TableView TableView { get; }
    }
}
