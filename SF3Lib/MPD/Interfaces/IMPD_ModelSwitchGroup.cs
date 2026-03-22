using System.Collections.Generic;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Interface for models that have an on/off toggle.
    /// </summary>
    public interface IMPD_ModelSwitchGroup {
        /// <summary>
        /// Game flag that determines whether models are visible or hidden.
        /// </summary>
        int Flag { get; set; }

        /// <summary>
        /// IDs of model instances that are only visible when 'Flag' is off.
        /// </summary>
        IReadOnlyList<int> ModelInstancesVisibleWhenOff { get; }

        /// <summary>
        /// IDs of model instances that are only visible when 'Flag' is on.
        /// </summary>
        IReadOnlyList<int> ModelInstancesVisibleWhenOn { get; }

        /// <summary>
        /// This is the state of the toggle when editing. Only relevant to SF3Editor.
        /// </summary>
        bool StateInEditor { get; set; }
    }
}
