using SF3.MPD.Interfaces;
using SF3.Types;

namespace SF3.MPD {
    public static class MPD_ChunkLogic {
        /// <summary>
        /// Specifies the chunk in which models would be located (either 1 or 20).
        /// </summary>
        public static int GetModelsChunkIndex(IMPD_AllFlags flags, ScenarioType scenario) {
            if (scenario <= ScenarioType.Scenario1)
                return 1;
            else if (flags.HasExtraModel)
                return 20;
            else if (flags.Bit_0x0200_HasSurfaceModel && !flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                return 20;
            else
                return 1;
        }

        /// <summary>
        /// Specifies the chunk in which the surface model would be located (either 2 or 20).
        /// </summary>
        public static int GetSurfaceModelChunkIndex(IMPD_AllFlags flags, ScenarioType scenario) {
            if (scenario <= ScenarioType.Scenario1)
                return 2;
            else if (flags.Bit_0x0002_HasSurfaceTextureRotation)
                return 2;
            else if (GetModelsChunkIndex(flags, scenario) == 20)
                return 2;
            else
                return 20;
        }

        /// <summary>
        /// When set, specifies the type of chunk located in Chunk[1] (should always be 'Models' if non-null).
        /// </summary>
        public static ChunkType GetChunk1Type(IMPD_AllFlags flags, ScenarioType scenario) {
            if ((scenario >= ScenarioType.Scenario2 && flags.HasExtraModel) || (flags.Bit_0x0100_HasModels && GetModelsChunkIndex(flags, scenario) == 1))
                return ChunkType.Models;
            else
                return ChunkType.Unset;
        }

        /// <summary>
        /// When set, specifies the area of memory (low or high) the pointers the models in Chunk[1] should be
        /// pointing to (either 'Low' or 'High').
        /// </summary>
        public static MemoryLocationType? GetChunk1PointersMemoryLocation(IMPD_AllFlags flags, ScenarioType scenario) {
            if (GetChunk1Type(flags, scenario) != ChunkType.Models)
                return null;
            return (scenario >= ScenarioType.Scenario2 || !flags.Bit_0x0200_HasSurfaceModel || flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                ? MemoryLocationType.LowMemory
                : MemoryLocationType.HighMemory;
        }

        /// <summary>
        /// When set, specifies the type of chunk located in Chunk[20] (either 'Models' or 'SurfaceModel').
        /// </summary>
        public static ChunkType GetChunk2Type(IMPD_AllFlags flags, ScenarioType scenario) {
            return flags.Bit_0x0200_HasSurfaceModel && GetSurfaceModelChunkIndex(flags, scenario) == 2
                ? ChunkType.SurfaceModel
                : ChunkType.Unset;
        }

        /// <summary>
        /// When set, specifies the type of chunk located in Chunk[20] (either 'Models' or 'SurfaceModel').
        /// </summary>
        public static ChunkType GetChunk20Type(IMPD_AllFlags flags, ScenarioType scenario) {
            if (scenario < ScenarioType.Scenario1)
                return ChunkType.Unset;
            else if (flags.Bit_0x0100_HasModels && GetModelsChunkIndex(flags, scenario) == 20)
                return ChunkType.Models;
            else if (flags.Bit_0x0200_HasSurfaceModel && GetSurfaceModelChunkIndex(flags, scenario) == 20)
                return ChunkType.SurfaceModel;
            else
                return ChunkType.Unset;
        }

        /// <summary>
        /// Specifies the area of memory (low or high) from which models would be accessed.
        /// </summary>
        public static MemoryLocationType GetModelsMemoryLocation(IMPD_AllFlags flags, ScenarioType scenario) {
            return (GetModelsChunkIndex(flags, scenario) == 1)
                ? GetChunk1PointersMemoryLocation(flags, scenario).Value
                : MemoryLocationType.HighMemory;
        }

        /// <summary>
        /// Specifies the area of memory (low or high) from which the surface model would be accessed.
        /// </summary>
        public static MemoryLocationType GetSurfaceModelMemoryLocation(IMPD_AllFlags flags, ScenarioType scenario) {
            return (scenario >= ScenarioType.Scenario2 && flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel && !flags.Bit_0x0002_HasSurfaceTextureRotation)
                ? MemoryLocationType.HighMemory
                : MemoryLocationType.LowMemory;
        }
    }
}
