using System.Collections.Generic;
using System.Windows.Forms;

namespace SF3.Win.Views {
    /// <summary>
    /// Interface for a component that can dynamically create or destroy itself.
    /// </summary>
    public interface ITabView : IContainerView {
        /// <summary>
        /// Reference to the Control as a TabControl.
        /// </summary>
        TabControl TabControl { get; }
    }
}
