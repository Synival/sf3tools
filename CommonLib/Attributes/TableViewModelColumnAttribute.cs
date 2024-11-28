using System;
using CommonLib.ViewModels;

namespace CommonLib.Attributes {
    public class TableViewModelColumnAttribute : Attribute {
        public TableViewModelColumnAttribute(
            string displayName   = null,
            int    displayOrder  = 0,
            string displayFormat = null,
            bool   isPointer     = false,
            int    minWidth      = 0,
            bool   isReadOnly    = false
        ) {
            Column = new TableViewModelColumn(
                displayName:   displayName,
                displayOrder:  displayOrder,
                displayFormat: displayFormat,
                isPointer:     isPointer,
                minWidth:      minWidth,
                isReadOnly:    isReadOnly
            );
        }

        public readonly TableViewModelColumn Column;
    }
}
