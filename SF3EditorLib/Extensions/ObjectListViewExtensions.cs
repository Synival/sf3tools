using BrightIdeasSoftware;
using SF3.Values;
using System;
using System.Windows.Forms;

namespace SF3.Editor.Extensions
{
    public static class ObjectListViewExtensions
    {
        /// <summary>
        /// Runs RefreshItem() on all OLVListItem's in the property Items.
        /// </summary>
        /// <param name="olv">The ObjectListView to refresh.</param>
        public static void RefreshAllItems(this ObjectListView olv)
        {
            foreach (var item in olv.Items)
            {
                var olvItem = item as OLVListItem;
                olv.RefreshItem(olvItem);
            }
        }

        /// <summary>
        /// Adds some extra functionality to the Control created when editing an ObjectListView cell.
        /// </summary>
        /// <param name="olv">The ObjectListView whose control should be modified.</param>
        /// <param name="e">Arguments from the OlvCellEditControl event.</param>
        public static void EnhanceOlvCellEditControl(this ObjectListView olv, CellEditEventArgs e)
        {
            // Enhance ComboBox's so values are updated any time the dropdown is closed, unless from hitting "escape".
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
            // Ensure that strings displayed in hex format are edited in hex format.
            else if (e.Column.AspectToStringFormat == "{0:X}")
            {
                NumericUpDown control = (NumericUpDown)e.Control;
                control.Hexadecimal = true;
            }
        }

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterSF3Values()
        {
            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(CharacterClassValue),
                (object model, OLVColumn column, object value) => Utils.MakeNamedValueComboBox((value as CharacterClassValue).ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(SexValue),
                (object model, OLVColumn column, object value) => Utils.MakeNamedValueComboBox((value as SexValue).ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(SpellValue),
                (object model, OLVColumn column, object value) => Utils.MakeNamedValueComboBox((value as SpellValue).ComboBoxValues)
            );

            // TODO: generic method to prevent copy + paste
            ObjectListView.EditorRegistry.Register(
                typeof(WeaponTypeValue),
                (object model, OLVColumn column, object value) => Utils.MakeNamedValueComboBox((value as WeaponTypeValue).ComboBoxValues)
            );
        }
    }
}
