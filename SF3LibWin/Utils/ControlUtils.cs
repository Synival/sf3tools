using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CommonLib.NamedValues;
using CommonLib.Win.Controls;

namespace SF3.Win.Utils {
    public static class ControlUtils {
        /// <summary>
        /// Creates a combo box that can be bound to a named value.
        /// </summary>
        /// <returns>A new pre-configured ComboBox.</returns>
        public static DarkModeComboBox MakeNamedValueComboBox(INamedValueInfo info, int currentValue, Type keyType) {
            var values = new Dictionary<int, string>(info.ComboBoxValues);
            if (!values.ContainsKey(currentValue))
                values.Add(currentValue, currentValue.ToString(info.FormatString));

            var dataSource = new BindingSource(values.ToDictionary(x => Convert.ChangeType(x.Key, keyType), x => x.Value), null);
            var comboBox = new DarkModeComboBox {
                DropDownStyle = ComboBoxStyle.DropDownList,
                DataSource = dataSource,
                DisplayMember = "Value",
                ValueMember = "Key",
            };

            return comboBox;
        }
    }
}
