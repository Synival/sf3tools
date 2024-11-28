using System;
using System.Windows.Forms;

namespace SF3.Win.Views {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface IView : IDisposable {
        /// <summary>
        /// Called when a view is ready to be created, Creates the Control in its entirety.
        /// </summary>
        /// <returns>The new Control created, or 'null' on failure.</returns>
        Control Create();

        /// <summary>
        /// Called when a view is ready to be discarded. Called automatically on disposal.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Forces the view to re-examine its content and before a data and screen refresh.
        /// </summary>
        void RefreshContent();

        /// <summary>
        /// Name to identify this view.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The control instantiated upon Create().
        /// </summary>
        Control Control { get; }
    }
}
