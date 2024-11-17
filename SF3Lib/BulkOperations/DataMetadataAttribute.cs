using System;

namespace SF3.BulkOperations {
    public enum EditorCapabilities {
        Hidden,
        Auto
    }

    public enum IntDisplayMode {
        Decimal,
        Hex
    }

    public class DataMetadataAttribute : Attribute {
        public DataMetadataAttribute(
            EditorCapabilities editorCapabilities = EditorCapabilities.Auto,
            string displayName = null,
            int displayOrder = 0,
            string displayFormat = null,
            bool isPointer = false,
            int minWidth = 0
        ) {
            EditorCapabilities = editorCapabilities;
            DisplayName        = displayName;
            DisplayOrder       = displayOrder;
            DisplayFormat      = displayFormat;
            IsPointer          = isPointer;
            MinWidth           = minWidth;
        }

        public EditorCapabilities EditorCapabilities { get; }
        public string DisplayName { get; }
        public int DisplayOrder { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
        public int MinWidth { get; }
    }
}
