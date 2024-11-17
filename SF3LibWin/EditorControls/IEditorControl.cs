using System;
using System.Windows.Forms;

namespace SF3.Win.EditorControls {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface IEditorControl : IDisposable {
        /// <summary>
        /// Called when the editor is ready to be instantiated.
        /// </summary>
        /// <returns>The new Control created, or 'null' on failure.</returns>
        Control Create();

        /// <summary>
        /// Called when an editor is ready to be discarded. Called automatically on disposal.
        /// </summary>
        void Destroy();

        /// <summary>
        /// The control instantiated upon Create().
        /// </summary>
        Control Control { get; }
    }
}
