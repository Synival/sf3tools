﻿using System;
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
    }
}
