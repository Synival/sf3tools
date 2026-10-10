using CommonLib.Arrays;
using SF3.Models.Structs.MPD.Main;
using SF3.Types;

namespace SF3.Tests.Models.Structs {
    [TestClass]
    public class MPD_FlagsFromHeader_Tests {
        [TestMethod]
        public void Scenario1_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPD_Header(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario1);
            var flags = new MPD_FlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                int expectedModelChunkIndex,
                int expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                MemoryLocationType expectedChunk1PointersMemoryLocation,
                MemoryLocationType expectedModelsMemoryLocation,
                MemoryLocationType expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                string PF(bool x) => x ? "P" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel = has0x8000;

                var resultsString =
                    PF(expectedModelChunkIndex              == flags.ModelsChunkIndex) +
                    PF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    PF(expectedChunk1Type                   == flags.Chunk1Type) +
                    PF(expectedChunk2Type                   == flags.Chunk2Type) +
                    PF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    PF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    PF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "PPPPPPP")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                // Some notes in Scenario 1:
                // - Surface models are always chunk[2]
                // - Surface models (chunk[2]) are always low memory
                // - Models are always chunk[1]
                // - Models (chunk[1]) can be either low or high memory depending on whether the surface model exists and bit 0x8000 is set
                // - Extra models (Titan) are in chunk[19]. Probaby always low memory? The Titan is special, so who knows!
                // - No rotatable textures

                RunTestCase(false, false, false, 1, 2, ChunkType.Unset,  ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase(false, false,  true, 1, 2, ChunkType.Unset,  ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase(false,  true, false, 1, 2, ChunkType.Unset,  ChunkType.SurfaceModel, MemoryLocationType.HighMemory, MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, 1, 2, ChunkType.Unset,  ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true, false, false, 1, 2, ChunkType.Models, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true, false,  true, 1, 2, ChunkType.Models, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true,  true, false, 1, 2, ChunkType.Models, ChunkType.SurfaceModel, MemoryLocationType.HighMemory, MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, 1, 2, ChunkType.Models, ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }

        [TestMethod]
        public void Scenario2_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPD_Header(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario2);
            var flags = new MPD_FlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                bool hasKraken,
                int expectedModelChunkIndex,
                int expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                ChunkType expectedChunk20Type,
                MemoryLocationType expectedChunk1PointersMemoryLocation,
                MemoryLocationType expectedModelsMemoryLocation,
                MemoryLocationType expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                string PF(bool x) => x ? "P" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}{TF(hasKraken)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel = has0x8000;
                flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures = hasKraken;

                var resultsString =
                    PF(expectedModelChunkIndex              == flags.ModelsChunkIndex) +
                    PF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    PF(expectedChunk1Type                   == flags.Chunk1Type) +
                    PF(expectedChunk2Type                   == flags.Chunk2Type) +
                    PF(expectedChunk20Type                  == flags.Chunk20Type) +
                    PF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    PF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    PF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "PPPPPPPP")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                // Some notes in Scenario 2:
                // - Chunk[1] is always low memory
                // - Chunk[2] is always low memory
                // - Chunk[20] is always high memory
                // - Surface models are in either chunk[2] (low) or chunk[20] (high) depending on bit 0x8000 and the existance of extra models (Kraken)
                // - Models are in either chunk[1] (low) or chunk[20] (high) depending on the existance of the surface model, bit 0x8000, and the existance of extra models (Kraken)
                // - Extra models (Kraken) are in chunk[1] (low memory)
                // - No rotatable textures

                // Without Kraken
                RunTestCase(false, false, false, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase(false, false,  true, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase(false,  true, false, false, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true, false, false, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true, false,  true, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true,  true, false, false, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),

                // With Kraken (bit 0x8000 is effectively ignored, AFAIK)
                RunTestCase(false, false, false,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true, false,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false, false,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true, false,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }

        [TestMethod]
        public void Scenario3_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPD_Header(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario3);
            var flags = new MPD_FlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                bool hasKraken,
                bool hasRotatableTextures,
                int? expectedModelChunkIndex,
                int? expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                ChunkType expectedChunk20Type,
                MemoryLocationType? expectedChunk1PointersMemoryLocation,
                MemoryLocationType? expectedModelsMemoryLocation,
                MemoryLocationType? expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                string PF(bool x) => x ? "P" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}{TF(hasKraken)}{TF(hasRotatableTextures)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_ModelsAreStillLowMemoryWithSurfaceModel = has0x8000;
                flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures = hasKraken;
                flags.Bit_0x0002_HasSurfaceTextureRotation = hasRotatableTextures;

                var resultsString =
                    PF(expectedModelChunkIndex              == flags.ModelsChunkIndex) +
                    PF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    PF(expectedChunk1Type                   == flags.Chunk1Type) +
                    PF(expectedChunk2Type                   == flags.Chunk2Type) +
                    PF(expectedChunk20Type                  == flags.Chunk20Type) +
                    PF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    PF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    PF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "PPPPPPPP")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                // Some notes in Scenario 3 + PD:
                // - Chunk[1] is always low memory
                // - Chunk[2] is always low memory
                // - Chunk[20] is always high memory
                // - Surface models are in either chunk[2] (low) or chunk[20] (high) depending on bit 0x8000, the existance of extra models (Kraken), and rotatable textures
                // - Models are in either chunk[1] (low) or chunk[20] (high) depending on the existance of the surface model, bit 0x8000, the existance of extra models (Kraken), and rotatable textures
                // - Extra models (Kraken) are in chunk[1] (low memory)
                // - Has rotatable textures; forces surface models to chunk[2] (low memory)

                // Without Kraken, no rotatable textures
                RunTestCase(false, false, false, false, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase(false, false,  true, false, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase(false,  true, false, false, false, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false, false,  1, 20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true, false, false, false, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true, false,  true, false, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),
                RunTestCase( true,  true, false, false, false, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false, false,  1, 20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),

                // With Kraken, no rotatable textures (bit 0x8000 is effectively ignored, AFAIK)
                RunTestCase(false, false, false,  true, false, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false, false,  true,  true, false, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true, false,  true, false, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true, false, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false, false,  true, false, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false,  true,  true, false, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true, false,  true, false, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true,  true, false, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),

                // -------------------------------------

                // Without Kraken, rotatable textures
                RunTestCase(false, false, false, false,  true,  1,  2, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase(false, false,  true, false,  true,  1,  2, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase(false,  true, false, false,  true, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false,  true,  1,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true, false, false, false,  true,  1,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true, false,  true, false,  true,  1,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
                RunTestCase( true,  true, false, false,  true, 20,  2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false,  true,  1,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),

                // With Kraken, rotatable textures (bit 0x8000 is effectively ignored, AFAIK)
                RunTestCase(false, false, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false, false,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true, false,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true, false,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true,  true,  true, 20,  2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }
    }
}
