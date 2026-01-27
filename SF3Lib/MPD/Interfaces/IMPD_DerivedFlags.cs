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
        /// When set, specifies the type of chunk located in Chunk[1] (should always be 'Models' if non-null).
        /// </summary>
        ChunkType Chunk1Type { get; }

        /// <summary>
        /// When set, specifies the area of memory (low or high) the pointers the models in Chunk[1] should be
        /// pointing to (either 'Low' or 'High').
        /// </summary>
        MemoryLocationType? Chunk1PointersMemoryLocation { get; }

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
