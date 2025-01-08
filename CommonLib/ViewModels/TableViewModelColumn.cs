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
    }
}
