using CommonLib.Arrays;
using SF3.Models.Structs.MPD;
using SF3.Types;

namespace SF3.Tests.Models.Structs {
    [TestClass]
    public class MPDFlagsFromHeader_Tests {
        [TestMethod]
        public void Scenario1_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPDHeaderModel(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario1);
            var flags = new MPDFlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                int? expectedModelsChunkIndex,
                int? expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                MemoryLocationType? expectedChunk1PointersMemoryLocation,
                MemoryLocationType? expectedModelsMemoryLocation,
                MemoryLocationType? expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_Chunk1IsStillLoadedFromLowMemoryIfSurfaceModelExists = has0x8000;

                var resultsString =
                    TF(expectedModelsChunkIndex             == flags.ModelsChunkIndex) +
                    TF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    TF(expectedChunk1Type                   == flags.Chunk1Type) +
                    TF(expectedChunk2Type                   == flags.Chunk2Type) +
                    TF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    TF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    TF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "TTTTTTT")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                RunTestCase(false, false, false, null, null, ChunkType.Unset,  ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false, false,  true, null, null, ChunkType.Unset,  ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false,  true, false, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase( true, false, false,    1, null, ChunkType.Models, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true, false,  true,    1, null, ChunkType.Models, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true,  true, false,    1,    2, ChunkType.Models, ChunkType.SurfaceModel, MemoryLocationType.HighMemory, MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true,    1,    2, ChunkType.Models, ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }

        [TestMethod]
        public void Scenario2_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPDHeaderModel(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario2);
            var flags = new MPDFlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                bool hasKraken,
                int? expectedModelsChunkIndex,
                int? expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                ChunkType expectedChunk20Type,
                MemoryLocationType? expectedChunk1PointersMemoryLocation,
                MemoryLocationType? expectedModelsMemoryLocation,
                MemoryLocationType? expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}{TF(hasKraken)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_Chunk20IsSurfaceModelIfExists = has0x8000;
                flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures = hasKraken;

                var resultsString =
                    TF(expectedModelsChunkIndex             == flags.ModelsChunkIndex) +
                    TF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    TF(expectedChunk1Type                   == flags.Chunk1Type) +
                    TF(expectedChunk2Type                   == flags.Chunk2Type) +
                    TF(expectedChunk20Type                  == flags.Chunk20Type) +
                    TF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    TF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    TF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "TTTTTTTT")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                // Without Kraken
                RunTestCase(false, false, false, false, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false, false,  true, false, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false,  true, false, false, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false, null,   20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.SurfaceModel, null,                          null,                          MemoryLocationType.HighMemory),
                RunTestCase( true, false, false, false,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true, false,  true, false,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true,  true, false, false,   20,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       null,                          MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false,    1,   20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),

                // With Kraken
                RunTestCase(false, false, false,  true, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false, false,  true,  true, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false,  true, false,  true, null,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true, null,   20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  null,                          MemoryLocationType.HighMemory),
                RunTestCase( true, false, false,  true,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true, false,  true,  true,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true,  true, false,  true,   20,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),

                // (Contradictory case)
                //RunTestCase( true,  true,  true,  true, null, null, ChunkType.Models, ChunkType.Unknown,      ChunkType.Unknown,      MemoryLocationType.LowMemory,  null,                          null),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }

        [TestMethod]
        public void Scenario3_ModelSurfaceFlagSettings_AllPossibilitiesLookCorrect() {
            var header = new MPDHeaderModel(new SF3.ByteData.ByteData(new ByteArray(100)), 0, "Header", 0, ScenarioType.Scenario3);
            var flags = new MPDFlagsFromHeader(header);

            string? RunTestCase(
                bool hasModels,
                bool hasSurfaceModel,
                bool has0x8000,
                bool hasKraken,
                bool hasRotatableTextures,
                int? expectedModelsChunkIndex,
                int? expectedSurfaceModelChunkIndex,
                ChunkType expectedChunk1Type,
                ChunkType expectedChunk2Type,
                ChunkType expectedChunk20Type,
                MemoryLocationType? expectedChunk1PointersMemoryLocation,
                MemoryLocationType? expectedModelsMemoryLocation,
                MemoryLocationType? expectedSurfaceModelMemoryLocation
            ) {
                string TF(bool x) => x ? "T" : "F";
                var settingsString = $"{TF(hasModels)}{TF(hasSurfaceModel)}{TF(has0x8000)}{TF(hasKraken)}{TF(hasRotatableTextures)}";

                flags.Bit_0x0100_HasModels = hasModels;
                flags.Bit_0x0200_HasSurfaceModel = hasSurfaceModel;
                flags.Bit_0x8000_Chunk20IsSurfaceModelIfExists = has0x8000;
                flags.Bit_0x4000_HasExtraChunk1ModelWithChunk21Textures = hasKraken;
                flags.Bit_0x0002_HasSurfaceTextureRotation = hasRotatableTextures;

                var resultsString =
                    TF(expectedModelsChunkIndex             == flags.ModelsChunkIndex) +
                    TF(expectedSurfaceModelChunkIndex       == flags.SurfaceModelChunkIndex) +
                    TF(expectedChunk1Type                   == flags.Chunk1Type) +
                    TF(expectedChunk2Type                   == flags.Chunk2Type) +
                    TF(expectedChunk20Type                  == flags.Chunk20Type) +
                    TF(expectedChunk1PointersMemoryLocation == flags.Chunk1PointersMemoryLocation) +
                    TF(expectedModelsMemoryLocation         == flags.ModelsMemoryLocation) +
                    TF(expectedSurfaceModelMemoryLocation   == flags.SurfaceModelMemoryLocation);

                if (resultsString != "TTTTTTTT")
                    return $"{settingsString}: {resultsString}";
                else
                    return null;
            }

            var results = new List<string?>() {
                // No rotatable textures
                // -------------------------
                // Without Kraken
                RunTestCase(false, false, false, false, false, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false, false,  true, false, false, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false,  true, false, false, false, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false, false, null,   20, ChunkType.Unset,  ChunkType.Unset,        ChunkType.SurfaceModel, null,                          null,                          MemoryLocationType.HighMemory),
                RunTestCase( true, false, false, false, false,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true, false,  true, false, false,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true,  true, false, false, false,   20,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       null,                          MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false, false,    1,   20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory),

                // With Kraken
                RunTestCase(false, false, false,  true, false, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false, false,  true,  true, false, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false,  true, false,  true, false, null,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true, false, null,   20, ChunkType.Models, ChunkType.Unset,        ChunkType.SurfaceModel, MemoryLocationType.LowMemory,  null,                          MemoryLocationType.HighMemory),
                RunTestCase( true, false, false,  true, false,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true, false,  true,  true, false,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true,  true, false,  true, false,   20,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),

                // (Contradictory cases)
                //RunTestCase( true,  true,  true,  true, false, null, null, ChunkType.Models, ChunkType.Unknown,      ChunkType.Unknown,      MemoryLocationType.LowMemory,  null,                          null),

                // Rotatable textures
                // -------------------------
                // Without Kraken
                RunTestCase(false, false, false, false,  true, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false, false,  true, false,  true, null, null, ChunkType.Unset,  ChunkType.Unset,        ChunkType.Unset,        null,                          null,                          null),
                RunTestCase(false,  true, false, false,  true, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true, false,  true, null,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Unset,        null,                          null,                          MemoryLocationType.LowMemory),
                RunTestCase( true, false, false, false,  true,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true, false,  true, false,  true,    1, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  null),
                RunTestCase( true,  true, false, false,  true,   20,    2, ChunkType.Unset,  ChunkType.SurfaceModel, ChunkType.Models,       null,                          MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),
                RunTestCase( true,  true,  true, false,  true,    1,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory,  MemoryLocationType.LowMemory),

                // With Kraken
                RunTestCase(false, false, false,  true,  true, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false, false,  true,  true,  true, null, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          null),
                RunTestCase(false,  true, false,  true,  true, null,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          MemoryLocationType.LowMemory),
                RunTestCase(false,  true,  true,  true,  true, null,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Unset,        MemoryLocationType.LowMemory,  null,                          MemoryLocationType.LowMemory),
                RunTestCase( true, false, false,  true,  true,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true, false,  true,  true,  true,   20, null, ChunkType.Models, ChunkType.Unset,        ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, null),
                RunTestCase( true,  true, false,  true,  true,   20,    2, ChunkType.Models, ChunkType.SurfaceModel, ChunkType.Models,       MemoryLocationType.LowMemory,  MemoryLocationType.HighMemory, MemoryLocationType.LowMemory),

                // (Contradictory cases)
                //RunTestCase( true,  true,  true,  true, false, null, null, ChunkType.Models, ChunkType.Unknown,      ChunkType.Unknown,      MemoryLocationType.LowMemory,  null,                          null),
            };

            results = results.Where(x => x != null).ToList();
            if (results.Count > 0)
                Assert.Fail(string.Join("\r\n", results));
        }
    }
}
