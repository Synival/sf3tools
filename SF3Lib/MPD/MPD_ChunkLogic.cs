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
            else if (flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures)
                return 20;
            else if (flags.Bit_0x0200_HasSurfaceModel && !flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel)
                return 20;
            else
                return 1;
        }
    }
}
