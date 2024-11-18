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
        /// <param name="child">The child control to add.</param>
        /// <param name="autoFill">When true, the Control's Docking is automatically set to Full.</param>
        /// <returns>The control created, or 'null' if it was not possible.</returns>
        Control CreateChild(IEditorControl child, bool autoFill = true);

        /// <summary>
        /// Creates a child editor control.
        /// </summary>
        /// <param name="name">The name of the child control for display.</param>
        /// <param name="child">The child control to add.</param>
        /// <param name="autoFill">When true, the Control's Docking is automatically set to Full.</param>
        /// <returns>Returns 'child'.</returns>
        Control CreateChild(string name, Control child, bool autoFill = true);

        /// <summary>
        /// Collection of child controls.
        /// </summary>
        IEnumerable<Control> ChildControls { get; }
    }
}
