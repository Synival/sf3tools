using CommonLib.NamedValues;
using SF3.Tables;

namespace SF3.Win.EditorControls {
    public interface ITableEditorControl : IEditorControl {
        /// <summary>
        /// The table that is to be edited.
        /// </summary>
        Table Table { get; }

        /// <summary>
        /// The name getter context used to fetch named values for viewing and editing.
        /// </summary>
        INameGetterContext NameGetterContext { get; }
    }
}
