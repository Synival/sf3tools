using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface ITabView : IContainerView {
        /// <summary>
        /// When 'true', controls are Create()'d when the view is selected.
        /// </summary>
        bool LazyLoad { get; set; }

        /// <summary>
        /// Reference to the Control as a TabControl.
        /// </summary>
        TabControl TabControl { get; }
    }
}
