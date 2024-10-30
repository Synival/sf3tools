using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.Editor.Utils;
using static SF3.Editor.Utils.ControlUtils;

namespace SF3.Editor.Extensions {
    public static class ObjectListViewExtensions {
        /// <summary>
        /// Runs RefreshItem() on all OLVListItem's in the property Items.
        /// </summary>
        /// <param name="olv">The ObjectListView to refresh.</param>
        public static void RefreshAllItems(this ObjectListView olv) {
            foreach (var item in olv.Items) {
                var olvItem = item as OLVListItem;
                olv.RefreshItem(olvItem);
            }
        }

        /// <summary>
        /// Applies some neat extensions to the ObjectListView.
        /// </summary>
        /// <param name=""></param>
        public static void Enhance(this ObjectListView olv) {
            foreach (var lvc in olv.AllColumns)
                lvc.Enhance();
        }

        /// <summary>
        /// Adds some extra functionality to a column of an ObjectListView.
        /// </summary>
        /// <param name="olv"></param>
        public static void Enhance(this OLVColumn lvc) {
            // Add a hook to each AspectGetter that will check for a named value.
            // If a name exists, hijack the AspectToStringConverter to use the name instead.
            // If no name exists, use the standard AspectToStringConverter.
            // (It would be nice if we could set one single AspectToStringConverter to check for this,
            // but alas, it only takes one paramter (value) and that's not enough to check for a name.)
            lvc.AspectGetter = obj => {
                var property = obj.GetType().GetProperty(lvc.AspectName);
                if (property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null) {
                    lvc.AspectToStringConverter = v => {
                        var val = ((int) lvc.GetAspectByName(obj)).ToNamedValue(obj, attr);
                        return val;
                    };
                }
                else
                    lvc.AspectToStringConverter = null;

                return lvc.GetAspectByName(obj);
            };
        }

        /// <summary>
        /// Adds some extra functionality to the Control created when editing an ObjectListView cell.
        /// </summary>
        /// <param name="olv">The ObjectListView whose control should be modified.</param>
        /// <param name="e">Arguments from the OlvCellEditControl event.</param>
        public static void EnhanceOlvCellEditControl(this ObjectListView olv, CellEditEventArgs e) {
            // Enhance ComboBox's so values are updated any time the dropdown is closed, unless from hitting "escape".
            if (e.Control is ComboBox) {
                var cb = e.Control as ComboBox;
                cb.KeyDown += (s, e2) => {
                    if (e2.KeyCode == Keys.Escape)
                        cb.SelectedValue = e.Column.GetValue(e.RowObject);
                };
                cb.DropDownClosed += (s, e2) => {
                    e.Column.PutValue(e.RowObject, cb.SelectedValue);
                    olv.RefreshItem(e.ListViewItem);
                };
            }
            // Ensure that strings displayed in hex format are edited in hex format.
            else if (e.Column.AspectToStringFormat == "{0:X}") {
                var control = (NumericUpDown)e.Control;
                control.Hexadecimal = true;
            }
        }

        /// <summary>
        /// Function to use for each EditorCreatorDelegate we're hijacking.
        /// Creates a combo box instead of the standard control if a named value is present.
        /// </summary>
        /// <param name="obj">The object bound to the ObjectListView row.</param>
        /// <param name="model">The column of the OLV.</param>
        /// <param name="value">The value fetched from the column.</param>
        /// <param name="oldDelegate">The EditorCreatorDelegate we're replacing to use as a fallback.</param>
        /// <returns>The control to use when editing - a ComboBox for named values, otherwise the return value of 'oldDelegate'.</returns>
        private static Control NamedValueEditorCreator(object obj, OLVColumn model, object value, EditorCreatorDelegate oldDelegate) {
            var property = obj.GetType().GetProperty(model.AspectName);
            if (property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null) {
                var intValue = (int) property.GetValue(obj);
                var nameAndValues = attr.GetNameAndInfo(obj, intValue);
                return MakeNamedValueComboBox(nameAndValues.Info, intValue);

                //TODO: use this for wacky new feature!
                //return new NumericUpDownFromAny(property.PropertyType);
            }

            return oldDelegate(obj, model, value);
        }

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterNamedValues() {
            /// BIG HACK to get existing editor delegates.
            var creatorMapField = ObjectListView.EditorRegistry.GetType().GetField(
                "creatorMap", BindingFlags.NonPublic | BindingFlags.Instance);
            var creatorMap = (Dictionary<Type, EditorCreatorDelegate>) creatorMapField.GetValue(ObjectListView.EditorRegistry);

            var typesToHijack = new Type[] {
                typeof(Int16),
                typeof(Int32),
                typeof(Int64),
                typeof(UInt16),
                typeof(UInt32),
                typeof(UInt64)
            };

            foreach (var type in typesToHijack) {
                var creator = creatorMap[type];
                ObjectListView.EditorRegistry.Register(type, (obj, model, value)
                    => NamedValueEditorCreator(obj, model, value, creator));
            }
        }
    }
}
