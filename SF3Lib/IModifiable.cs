using System;
using CommonLib;

namespace SF3 {
    /// <summary>
    /// Helper class for modifying any kind of data blob and tracking whether or not it's been edited.
    /// </summary>
    public interface IModifiable {
        /// <summary>
        /// While initialized, the IsModified flag will not be modified.
        /// Must be disposed of re-enable IsModified modifications.
        /// If multiple IsModifiedChangeBlockers are instantiated, IsModified changes will be blocked until all are disposed.
        /// </summary>
        /// <returns>A ScopeGuard that should be disposed of at some point.</returns>
        ScopeGuard IsModifiedChangeBlocker();

        /// <summary>
        /// 'True' when the data is modified.
        /// </summary>
        bool IsModified { get; set; }

        /// <summary>
        /// Event that occurs when the 'Modified' property is changed.
        /// </summary>
        event EventHandler IsModifiedChanged;
    }
}
