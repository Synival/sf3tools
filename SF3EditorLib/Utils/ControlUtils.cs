using System.Collections.Generic;
using System.Windows.Forms;
using CommonLib;
using CommonLib.NamedValues;

namespace SF3.Editor.Utils {
    public static class ControlUtils {
        /// <summary>
        /// Creates a combo box that can be bound to a NamedValue.
        /// </summary>
        /// <param name="allPossibleValues">All values possible for the ComboBox (not just the valid named ones).</param>
        /// <returns>A new pre-configured ComboBox.</returns>
        public static ComboBox MakeNamedValueComboBox(Dictionary<NamedValue, string> allPossibleValues, NamedValue extraValue) {
            if (!allPossibleValues.ContainsKey(extraValue)) {
                allPossibleValues = new Dictionary<NamedValue, string>(allPossibleValues) {
                    { extraValue, extraValue.FullName }
                };
            }

            var comboBox = new ComboBox {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = new BindingSource(allPossibleValues, null),
                DisplayMember = "Value",
                ValueMember = "Key"
            };
            return comboBox;
        }

        /// <summary>
        /// Creates a combo box that can be bound to a named value.
        /// </summary>
        /// <param name="allPossibleValues">All values possible for the ComboBox (not just the valid named ones).</param>
        /// <returns>A new pre-configured ComboBox.</returns>
        public static ComboBox MakeNamedValueComboBox(INamedValueInfo info, int currentValue) {
            var values = new Dictionary<int, string>(info.NewComboBoxValues);
            if (!values.ContainsKey(currentValue))
                values.Add(currentValue, currentValue.ToString(info.FormatString));

            var dataSource = new BindingSource(values, null);
            var comboBox = new ComboBox {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dataSource,
                DisplayMember = "Value",
                ValueMember = "Key",
            };

            return comboBox;
        }
    }
}
