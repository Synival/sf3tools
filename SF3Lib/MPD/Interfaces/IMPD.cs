using System;
using System.Collections.Generic;
using CommonLib.Geometry;
using CommonLib.Imaging;
using SF3.MPD.Interfaces.Flags;
using SF3.Types;

namespace SF3.MPD.Interfaces {
    /// <summary>
    /// Abstract implementation of any kind of MPD, such as "in-place" editors like MPD_File or a fully deserialized
    /// resource.
    /// </summary>
    public interface IMPD {
        /// <summary>
        /// The flags for the MPD. Mostly technical information that should only be modified directly if you know what
        /// you're doing.
        /// </summary>
        IMPD_EditableFlags Flags { get; }

        /// <summary>
        /// The settings for the MPD. Contains various visual settings and determines what internal flags to set.
        /// </summary>
        IMPD_Settings Settings { get; }

        /// <summary>
        /// Settings that don't affect gameplay but are necessary for byte-for-byte reproduction of MPD files.
        /// </summary>
        IMPD_BinaryReproductionFlags BinaryReproductionFlags { get; }

        /// <summary>
        /// The "Surface" which contains the grid. Used for actor heights, battle grid data, and event IDs.
        /// </summary>
        IMPD_Surface Surface { get; }

        /// <summary>
        /// All models that exist in this MPD, sorted by their collection (model+surface, chests+barrel, extra model).
        /// </summary>
        Dictionary<MPD_CollectionType, IMPD_ModelCollection> ModelCollections { get; }

        /// <summary>
        /// Palette used for 8-bit textures on models and the surface model.
        /// </summary>
        Palette TexturePalette { get; }

        /// <summary>
        /// Palette and direction for lighting models and the surface model.
        /// </summary>
        IMPD_Lighting Lighting { get; }

        /// <summary>
        /// Collection of model switch groups.
        /// </summary>
        IReadOnlyList<IMPD_ModelSwitchGroup> ModelSwitchGroups { get; }

        /// <summary>
        /// Collection of specific plane types (ground, tiled ground, battle sky, scene sky, background, foreground).
        /// </summary>
        IMPD_Planes Planes { get; }

        /// <summary>
        /// Collection of all collision lines in the MPD.
        /// </summary>
        IMPD_Collisions Collisions { get; }

        /// <summary>
        /// 2D boundary box that the game camera will force itself into.
        /// </summary>
        IRectangleShort CameraBoundaries { get; }

        /// <summary>
        /// 2D boundary box that the free-movable cursor in battle will force itself into.
        /// </summary>
        IRectangleShort BattleCursorBoundaries { get; }

        /// <summary>
        /// Color gradient applied to the ground, sky, and models + surface model at different configurable
        /// intensities. Only present in Scenario 2+.
        /// </summary>
        IMPD_Gradient Gradient { get; }

        /// <summary>
        /// Table used to animate the ground plane, corresponding to the SGL sl1MapRA() function.
        /// (Not currently supported)
        /// </summary>
        IReadOnlyList<byte> GroundAnimationData { get; }

        /// <summary>
        /// First unknown Scenario 1 0xFFFF-terminated table.
        /// </summary>
        IReadOnlyList<ushort> Scenario1UnknownTable1 { get; }

        /// <summary>
        /// Second unknown Scenario 1 0xFFFF-terminated table.
        /// </summary>
        IReadOnlyList<ushort> Scenario1UnknownTable2 { get; }

        /// <summary>
        /// Triggered when models have been updated and something needs to be informed, like a viewer.
        /// </summary>
        event EventHandler ModelsUpdated;
    }
}
