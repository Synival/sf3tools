using System;
using System.Reflection;

namespace CommonLib.ViewModels {
    public class TableViewModelColumn {
        public TableViewModelColumn(string displayName, float displayOrder, string displayFormat, bool isPointer, int minWidth, bool isReadOnly) {
            DisplayName   = displayName;
            DisplayOrder  = displayOrder;
            DisplayFormat = displayFormat;
            IsPointer     = isPointer;
            MinWidth      = minWidth;
            IsReadOnly    = isReadOnly;
        }

        public string DisplayName { get; }
        public float DisplayOrder { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
        public int MinWidth { get; }
        public bool IsReadOnly { get; }

        public bool GetColumnIsEditable(PropertyInfo prop)
            => !IsReadOnly && prop.GetSetMethod() != null;

        public string GetColumnText(PropertyInfo prop)
            => DisplayName ?? prop.Name;

        public string GetColumnAspectToStringFormat(PropertyInfo prop) {
            if (DisplayFormat != null && DisplayFormat != string.Empty)
                return "{0:" + DisplayFormat + "}";
            else if (IsPointer)
                return "{0:X6}";
            else
                return "";
        }

        public int GetColumnWidth()
            => Math.Max(30, MinWidth);
    }
}
