namespace CommonLib.ViewModels {
    public class TableViewModelColumn {
        public TableViewModelColumn(string displayName, int displayOrder, string displayFormat, bool isPointer, int minWidth, bool isReadOnly) {
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
