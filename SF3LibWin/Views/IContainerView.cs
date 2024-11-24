using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface IContainerView : IView {
        /// <summary>
        /// Creates a child view inside the view IContainerView.
        /// </summary>
        /// <param name="child">The child view to add.</param>
        /// <param name="autoFill">When true, the Control's Docking is automatically set to Full.</param>
        /// <returns>The control created, or 'null' if it was not possible.</returns>
        Control CreateChild(IView child, bool autoFill = true);

        /// <summary>
        /// Collection of child views.
        /// </summary>
        IEnumerable<IView> ChildViews { get; }
    }
}
