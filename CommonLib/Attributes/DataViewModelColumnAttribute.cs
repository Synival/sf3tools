using System;
using CommonLib.ViewModels;

namespace CommonLib.Attributes {
    public class DataViewModelColumnAttribute : Attribute {
        public DataViewModelColumnAttribute(
            string displayName   = null,
            int    displayOrder  = 0,
            string displayFormat = null,
            bool   isPointer     = false,
            int    minWidth      = 0,
            bool   isReadOnly    = false
        ) {
            Column = new DataViewModelColumn(
                displayName:   displayName,
                displayOrder:  displayOrder,
                displayFormat: displayFormat,
                isPointer:     isPointer,
                minWidth:      minWidth,
                isReadOnly:    isReadOnly
            );
        }

        public readonly DataViewModelColumn Column;
    }
}
