using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SF3.Editor
{
    public static class Utils
    {
        /// <summary>
        /// Creates a combo box that can be bound to a NamedValue.
        /// </summary>
        /// <param name="allPossibleValues">All values possible for the ComboBox (not just the valid named ones).</param>
        /// <returns>A new pre-configured ComboBox.</returns>
        public static ComboBox MakeNamedValueComboBox(Dictionary<NamedValue, string> allPossibleValues)
        {
            var comboBox = new ComboBox();
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.DataSource = new BindingSource(allPossibleValues, null);
            comboBox.DisplayMember = "Value";
            comboBox.ValueMember = "Key";
            return comboBox;
        }

        /// <summary>
        /// Adds some extra functionality to the Control created when editing an ObjectListView cell.
        /// </summary>
        /// <param name="olv">ObjectListView reference</param>
        /// <param name="e">CellEditEventArgs reference</param>
        public static void EnhanceOlvCellEditControl(ObjectListView olv, CellEditEventArgs e)
        {
            if (e.Control is ComboBox)
            {
                ComboBox cb = e.Control as ComboBox;
                cb.KeyDown += (s, e2) =>
                {
                    if (e2.KeyCode == Keys.Escape)
                    {
                        cb.SelectedValue = e.Column.GetValue(e.RowObject);
                    }
                };
                cb.DropDownClosed += (s, e2) =>
                {
                    e.Column.PutValue(e.RowObject, cb.SelectedValue);
                    olv.RefreshItem(e.ListViewItem);
                };
            }
        }
    }
}
