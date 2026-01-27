using SF3.Types;

namespace SF3.MPD.Interfaces {
    public interface IMPD_DerivedFlags {
        /// <summary>
        /// Specifies the area of memory (low or high) from which models would be accessed.
        /// </summary>
        MemoryLocationType ModelsMemoryLocation { get; }

        /// <summary>
        /// Specifies the area of memory (low or high) from which the surface model would be accessed.
        /// </summary>
        MemoryLocationType SurfaceModelMemoryLocation { get; }

        /// <summary>
        /// When set, a sky chunk is present for either battles (Scenario 1) or cutscenes (Scenario 2+).
        /// </summary>
        bool HasAnySky { get; }

        /// <summary>
        /// When set, an extra model is present like the Titan (Scenario 1) or the Kraken (Scenario 2+).
        /// </summary>
        bool HasExtraModel { get; }

        /// <summary>
        /// When set, specifies the type of chunk located in Chunk[2] (should always be 'SurfaceModel' if non-null).
        /// </summary>
        ChunkType Chunk2Type { get; }

        /// <summary>
        /// When set, specifies the type of chunk located in Chunk[20] (either 'Models' or 'SurfaceModel').
        /// </summary>
        ChunkType Chunk20Type { get; }
    }
}
