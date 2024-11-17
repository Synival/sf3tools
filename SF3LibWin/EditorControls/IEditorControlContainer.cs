using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.EditorControls {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface IEditorControlContainer : IEditorControl {
        /// <summary>
        /// Creates a child editor control.
        /// </summary>
        /// <returns>'true' on successful creation, otherwise 'false'.</returns>
        bool CreateChild(IEditorControl child);

        /// <summary>
        /// Collection of child controls.
        /// </summary>
        IEnumerable<Control> ChildControls { get; }
    }
}
