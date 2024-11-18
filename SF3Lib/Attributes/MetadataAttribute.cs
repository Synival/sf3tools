using System;

namespace SF3.Attributes {
    public class MetadataAttribute : Attribute {
        public MetadataAttribute(
            string displayName   = null,
            int    displayOrder  = 0,
            string displayFormat = null,
            bool   isPointer     = false,
            int    minWidth      = 0
        ) {
            DisplayName   = displayName;
            DisplayOrder  = displayOrder;
            DisplayFormat = displayFormat;
            IsPointer     = isPointer;
            MinWidth      = minWidth;
        }

        public string DisplayName { get; }
        public int DisplayOrder { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
        public int MinWidth { get; }
    }
}
