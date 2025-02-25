using System;
using CommonLib.ViewModels;

namespace CommonLib.Attributes {
    public class TableViewModelColumnAttribute : Attribute {
        public TableViewModelColumnAttribute(
            string displayName   = null,
            float  displayOrder  = 0f,
            string displayFormat = null,
            bool   isPointer     = false,
            int    minWidth      = 0,
            bool   isReadOnly    = false,
            string visibilityProperty = null,
            string displayGroup  = null
        ) {
            Column = new TableViewModelColumn(
                displayName:   displayName,
                displayOrder:  displayOrder,
                displayFormat: displayFormat,
                isPointer:     isPointer,
                minWidth:      minWidth,
                isReadOnly:    isReadOnly,
                visibilityProperty: visibilityProperty,
                displayGroup:  displayGroup
            );
        }

        public readonly TableViewModelColumn Column;
    }
}
