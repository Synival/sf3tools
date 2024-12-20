using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface IContainerView : IView {
        /// <summary>
        /// Creates a child view inside the view IContainerView.
        /// The view may or may not be Create()'d immediately.
        /// </summary>
        /// <param name="child">The child view to add.</param>
        /// <param name="onCreate">Callback performed after attempting to Create() the view. The input Control parameter may be 'null' if Create() failed.</param>
        /// <param name="autoFill">When true, the Control's Docking is automatically set to Full.</param>
        void CreateChild(IView child, Action<Control> onCreate = null, bool autoFill = true);

        /// <summary>
        /// Collection of child views.
        /// </summary>
        IEnumerable<IView> ChildViews { get; }
    }
}
