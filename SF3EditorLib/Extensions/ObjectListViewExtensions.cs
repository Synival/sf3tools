using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using BrightIdeasSoftware;
using CommonLib.Attributes;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.Editor.Utils;

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
        /// Adds some extra functionality to all columns of an ObjectListView.
        /// </summary>
        /// <param name="olv"></param>
        public static void EnhanceColumns(this ObjectListView olv) {
            foreach (var lvc in olv.AllColumns) {
                lvc.AspectGetter = obj => {
                    var property = obj.GetType().GetProperty(lvc.AspectName);
                    if (property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null) {
                        // TODO: do something with this valuable info!!
                        return ((int) lvc.GetAspectByName(obj)).ToNamedValue(obj, attr);
                    }
                    else
                        return lvc.GetAspectByName(obj);
                };
            }

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

        private static void RegisterNamedValue<T>() {
            ObjectListView.EditorRegistry.Register(
                typeof(T),
                (object data, OLVColumn column, object value) => ControlUtils.MakeNamedValueComboBox((value as NamedValue).ComboBoxValues, value as NamedValue)
            );
        }

        /// <summary>
        /// A NumericUpDown designed to use with integer values that are displayed by the ObjectListView
        /// as a string.
        /// Its 'Value' takes (almost) any input and always outputs a T.
        /// This is to deal with ObjectListView using Value get/set via reflection.
        /// </summary>
        private class NumericUpDownFromAny : NumericUpDown
        {
            public NumericUpDownFromAny(Type getterType, int min = int.MinValue, int max = int.MaxValue) {
                Minimum = min;
                Maximum = max;
                DecimalPlaces = 0;
                Hexadecimal = true;
                GetterType = getterType;
            }

            private Type GetterType { get; }

            private static bool FromStringPred(char ch) => ch != ':' && ch != ' ';

            private int FromString(string value) {
                value = new string(value.TakeWhile(FromStringPred).ToArray());
                return Convert.ToInt32(value, 16);
            }

            public new object Value {
                get {
                    if (GetterType == typeof(byte))
                        return (byte) base.Value;
                    else if (GetterType == typeof(int))
                        return (int) base.Value;
                    else if (GetterType == typeof(decimal))
                        return base.Value;
                    else
                        throw new ArgumentException("Type 'T' is not a handled value getter type");
                }
                set {
                    switch (value) {
                        case string s:
                            base.Value = new decimal(FromString(s));
                            break;
                        case byte b:
                            base.Value = new decimal(b);
                            break;
                        case int i:
                            base.Value = new decimal(i);
                            break;
                        case decimal d:
                            base.Value = d;
                            break;
                        default:
                            throw new ArgumentException("'value' is not a handled value settertype");
                    }
                }
            }
        }

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterSF3Values() {
            // Get all classes derived from 'NamedValue'
            var namedValueTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type => type != typeof(NamedValue) && !type.ContainsGenericParameters && typeof(NamedValue).IsAssignableFrom(type))
                .OrderBy(x => x.Name)
                .ToList();

            // Invoke RegisterNamedValue<T> for all types
            // TODO: old method!! use the cooler one below!!
            var methodBase = typeof(ObjectListViewExtensions).GetMethod(nameof(RegisterNamedValue), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var nvt in namedValueTypes) {
                var method = methodBase.MakeGenericMethod(nvt);
                _ = method.Invoke(null, null);
            }

            ObjectListView.EditorRegistry.RegisterDefault((obj, model, value) => {
                var property = obj.GetType().GetProperty(model.AspectName);
                if (property.GetCustomAttribute<NameGetterAttribute>() is var attr && attr != null) {
                    // TODO: combo box!
                    return new NumericUpDownFromAny(property.PropertyType);
                }
                return null;
            });
        }
    }
}
