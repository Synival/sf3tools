using System;
using System.Reflection;

namespace CommonLib.ViewModels {
    public class TableViewModelColumn {
        public TableViewModelColumn(
            string addressField,
            string displayName,
            float displayOrder,
            string displayFormat,
            bool isPointer,
            int minWidth,
            bool isReadOnly,
            string visibilityProperty,
            string displayGroup
        ) {
            AddressProperty = addressField;
            DisplayName   = displayName;
            DisplayOrder  = displayOrder;
            DisplayFormat = displayFormat;
            IsPointer     = isPointer;
            MinWidth      = minWidth;
            IsReadOnly    = isReadOnly;
            VisibilityProperty = visibilityProperty;
            DisplayGroup  = displayGroup;
        }

        public string AddressProperty { get; }
        public string DisplayName { get; }
        public float DisplayOrder { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
        public int MinWidth { get; }
        public bool IsReadOnly { get; }
        public string VisibilityProperty { get; }
        public string DisplayGroup { get; }

        public bool GetColumnIsEditable(PropertyInfo prop)
            => !IsReadOnly && prop.GetSetMethod() != null;

        public string GetColumnText(PropertyInfo prop)
            => DisplayName ?? prop.Name;

        public string GetColumnAspectToStringFormat() {
            if (DisplayFormat != null && DisplayFormat != string.Empty)
                return "{0:" + DisplayFormat + "}";
            else if (IsPointer)
                return "{0:X2}";
            else
                return "";
        }

        public int GetColumnWidth()
            => Math.Max(30, MinWidth);

        public bool GetVisibility(object obj) {
            if (VisibilityProperty == null)
                return true;

            var visProp = obj.GetType().GetProperty(VisibilityProperty, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.GetProperty);
            if (visProp == null)
                return true;

            return (bool) visProp.GetValue(obj);
        }
    }
}
