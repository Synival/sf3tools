using SF3.Types;

namespace SF3.MPD {
    public interface IMPD_DerivedFlags {
        /// <summary>
        /// When set, specifies the chunk in which models should be located (either 1 or 20).
        /// </summary>
        int? ModelsChunkIndex { get; }

        /// <summary>
        /// When set, specifies the area of memory (low or high) in which it is accessed.
        /// </summary>
        MemoryLocationType? ModelsMemoryLocation { get; }

        /// <summary>
        /// When set, specifies the chunk in which the surface model is located (either 2 or 20).
        /// </summary>
        int? SurfaceModelChunkIndex { get; }

        /// <summary>
        /// When set, a skybox chunk is present for either battles (Scenario 1) or cutscenes (Scenario 2+).
        /// </summary>
        bool HasAnySkyBox { get; }
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
