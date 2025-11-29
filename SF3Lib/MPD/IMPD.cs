using System.Collections.Generic;
using SF3.Types;

namespace SF3.MPD {
    /// <summary>
    /// Abstract implementation of any kind of MPD, such as "in-place" editors like MPD_File or a fully deserialized
    /// resource.
    /// </summary>
    public interface IMPD {
        /// <summary>
        /// The flags for the MPD. Mostly technical information that should only be modified directly if you know what
        /// you're doing.
        /// </summary>
        IMPD_AllFlags Flags { get; }

        /// <summary>
        /// The settings for the MPD. Contains various visual settings and determines what internal flags to set.
        /// </summary>
        IMPD_Settings Settings { get; }

        /// <summary>
        /// The "Surface" which contains the grid. Used for actor heights, battle grid data, and event IDs.
        /// </summary>
        IMPD_Surface Surface { get; }

        /// <summary>
        /// All models that exist in this MPD, sorted by their collection (model+surface, chests+barrel, extra model).
        /// </summary>
        Dictionary<CollectionType, IMPD_ModelCollection> ModelCollections { get; }
    }
}
