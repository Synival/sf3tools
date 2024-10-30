using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.NamedValues;
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
        /// <param name="olv">The ObjectListView to enhance.</param>
        /// <param name="nameContext">The context for NameGetterAttribute when fetching names.</param>
        public static void Enhance(this ObjectListView olv, INameGetterContext nameContext) {
            foreach (var lvc in olv.AllColumns)
                lvc.Enhance(nameContext);
        }

        /// <summary>
        /// Adds some extra functionality to a column of an ObjectListView.
        /// </summary>
        /// <param name="lvc">The ObjectListView column to enhance.</param>
        /// <param name="nameContext">The context for NameGetterAttribute when fetching names.</param>
        public static void Enhance(this OLVColumn lvc, INameGetterContext nameContext) {
            if (nameContext == null)
                return;

            // Add a hook to each AspectGetter that will check for a named value.
            // If a name exists, hijack the AspectToStringConverter to use the name instead.
            // If no name exists, use the standard AspectToStringConverter.
            // (It would be nice if we could set one single AspectToStringConverter to check for this,
            // but alas, it only takes one paramter (value) and that's not enough to check for a name.)
            lvc.AspectGetter = obj => {
                var property = obj.GetType().GetProperty(lvc.AspectName);
                lvc.AspectToStringConverter = property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null
                    ? (v => {
                        var val = ((int) lvc.GetAspectByName(obj)).ToNamedValue(nameContext, attr);
                        return val;
                    })
                    : (AspectToStringConverterDelegate) null;

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
        /// <param name="nameContext">The context for NameGetterAttribute when fetching names.</param>
        /// <param name="oldDelegate">The EditorCreatorDelegate we're replacing to use as a fallback.</param>
        /// <returns>The control to use when editing - a ComboBox for named values, otherwise the return value of 'oldDelegate'.</returns>
        private static Control NamedValueEditorCreator(object obj, OLVColumn model, object value, INameGetterContext nameContext, EditorCreatorDelegate oldDelegate) {
            if (nameContext != null && Globals.UseDropdowns) {
                var property = obj.GetType().GetProperty(model.AspectName);
                if (property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null) {
                    var intValue = (int) property.GetValue(obj);
                    var nameAndValues = attr.GetNameAndInfo(nameContext, intValue, false);
                    return MakeNamedValueComboBox(nameAndValues.Info, intValue);
                }
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
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(ushort),
                typeof(uint),
                typeof(ulong)
            };

            foreach (var type in typesToHijack) {
                var creator = creatorMap[type];
                ObjectListView.EditorRegistry.Register(type, (obj, model, value)
                    => NamedValueEditorCreator(obj, model, value, null /* TODO: actual context!! */, creator));
            }
        }
    }
}
