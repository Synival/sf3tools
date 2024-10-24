﻿using BrightIdeasSoftware;
using SF3.Values;
using System;
using System.Linq;
using System.Reflection;
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

        private static void RegisterNamedValue<T>()
        {
            ObjectListView.EditorRegistry.Register(
                typeof(T),
                (object model, OLVColumn column, object value) => Utils.MakeNamedValueComboBox((value as NamedValue).ComboBoxValues)
            );
        }

        /// <summary>
        /// Performs ObjectListView.EditorRegistry.Register() for all SF3 NamedValues.
        /// </summary>
        public static void RegisterSF3Values()
        {
            // Get all classes derived from 'NamedValue'
            var namedValueTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(domainAssembly => domainAssembly.GetTypes())
                .Where(type => type != typeof(NamedValue) && !type.ContainsGenericParameters && typeof(NamedValue).IsAssignableFrom(type))
                .OrderBy(x => x.Name)
                .ToList();

            // Invoke RegisterNamedValue<T> for all types
            var methodBase = typeof(ObjectListViewExtensions).GetMethod(nameof(RegisterNamedValue), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var nvt in namedValueTypes)
            {
                var method = methodBase.MakeGenericMethod(nvt);
                method.Invoke(null, null);
            }
        }
    }
}
