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
            IntDisplayMode intDisplayMode = IntDisplayMode.Decimal,
            string displayFormat = "",
            bool isPointer = false
        ) {
            EditorCapabilities = editorCapabilities;
            DisplayName        = displayName;
            DisplayOrder       = displayOrder;
            IntDisplayMode     = intDisplayMode;
            DisplayFormat      = displayFormat;
            IsPointer          = isPointer;
        }

        public EditorCapabilities EditorCapabilities { get; }
        public string DisplayName { get; }
        public int DisplayOrder { get; }
        public IntDisplayMode IntDisplayMode { get; }
        public string DisplayFormat { get; }
        public bool IsPointer { get; }
    }
}
