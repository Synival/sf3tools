using System;

namespace SF3.Attributes {
    public class ViewModelDataAttribute : Attribute {
        public ViewModelDataAttribute(
            string displayName   = null,
            int    displayOrder  = 0,
            string displayFormat = null,
            bool   isPointer     = false,
            int    minWidth      = 0,
            bool   isReadOnly    = false
        ) {
            DisplayName   = displayName;
            DisplayOrder  = displayOrder;
            DisplayFormat = displayFormat;
            IsPointer     = isPointer;
            MinWidth      = minWidth;
            IsReadOnly    = isReadOnly;
        }

        public string DisplayName { get; }
        public int DisplayOrder { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
        public int MinWidth { get; }
        public bool IsReadOnly { get; }
    }
}
