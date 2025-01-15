using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.Types;

namespace MPD_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your MPD files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine(scenario.ToString() + ": " + Path.GetFileName(file) + ":");

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts)) {
                            // Is this MPD file in the wrong format for this scenario? (Scenario 3 and Premium Disk are the same)
                            // (This actually happens!)
                            var expectedScenario = (scenario == ScenarioType.PremiumDisk) ? ScenarioType.Scenario3 : scenario;
                            if (mpdFile.Scenario != expectedScenario)
                                Console.WriteLine("  !!! Wrong scenario for this disc! ShouldBe=" + expectedScenario + ", Is=" + mpdFile.Scenario);

                            // Anything Scenario 2 or higher should have addresses for Chunk[20] and Chunk[21].
                            bool shouldHaveChunk20_21 = mpdFile.Scenario >= ScenarioType.Scenario2;
                            bool hasChunk20 = mpdFile.ChunkHeader.Rows[20].ChunkAddress > 0;
                            bool hasChunk21 = mpdFile.ChunkHeader.Rows[21].ChunkAddress > 0;

                            if (shouldHaveChunk20_21 != hasChunk20)
                                Console.WriteLine("  !!! Chunk[20] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk20);
                            if (shouldHaveChunk20_21 != hasChunk21)
                                Console.WriteLine("  !!! Chunk[21] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk21);

                            // For Scenario 2 onwards, if Chunk 2 is empty, Chunk 20 should probably have the surface data, or nothing at all.
                            if (mpdFile.Scenario >= ScenarioType.Scenario2) {
                                if (!mpdFile.ChunkHeader.Rows[2].Exists) {
                                    var fn = file;
                                    if (mpdFile.ChunkHeader.Rows[20].Exists && mpdFile.ChunkHeader.Rows[20].ChunkSize != 0xCF00)
                                        Console.WriteLine("  !!! Chunk[2] missing, Chunk[20] present, but not surface data!");
                                    if (mpdFile.ChunkHeader.Rows[21].Exists)
                                        Console.WriteLine("  !!! Chunk[2] missing, but Chunk[21] exists??");
                                }
                                else {
                                    if (!mpdFile.ChunkHeader.Rows[20].Exists) {
                                        // Three valid MPDs in Scenario 3 have Chunk[2] but no Chunk[20], but that's it.
                                        // This isn't the case in Scenario 2 or the Premium Disk.
                                        Console.WriteLine("  !!! Chunk[2] exists, but Chunk[20] should exist as well!");
                                        if (mpdFile.ChunkHeader.Rows[21].Exists)
                                            Console.WriteLine("  !!! Chunk[2] and Chunk[20] is missing, but Chunk[21] exists??");
                                    }
                                }
                            }

                            // Anything Scenario 3 or higher should have Palette3.
                            bool shouldHavePalette3 = mpdFile.Scenario >= ScenarioType.Scenario3;
                            bool hasPalette3 = mpdFile.MPDHeader.Data.GetWord(mpdFile.MPDHeader.Address + 0x44) == 0x0029;
                            if (shouldHavePalette3 != hasPalette3)
                                Console.WriteLine("  !!! Palette3 problem! ShouldHave=" + shouldHavePalette3 + ", DoesHave=" + hasPalette3);

                            // If the disc is Scenario 3, all palettes should be present.
                            if (mpdFile.Scenario >= ScenarioType.Scenario3) {
                                if (mpdFile.TexturePalettes[0] == null)
                                    Console.WriteLine("  !!! Scenario3 Palette[0] doesn't exist!");
                                if (mpdFile.TexturePalettes[1] == null)
                                    Console.WriteLine("  !!! Scenario3 Palette[1] doesn't exist!");
                                if (mpdFile.TexturePalettes[2] == null)
                                    Console.WriteLine("  !!! Scenario3 Palette[2] doesn't exist!");
                            }

                            if (mpdFile.SurfaceModel != null) {
                                var corners = Enum.GetValues<CornerType>();
                                foreach (var tile in mpdFile.Tiles) {
                                    // This *would* report irregularities in heightmaps, if the existed :)
                                    var moveHeights  = corners.ToDictionary(c => c, tile.GetMoveHeightmap);
                                    if (tile.ModelIsFlat) {
                                        var br = CornerType.BottomRight;
                                        foreach (var c in corners) {
                                            if (c == br)
                                                continue;
                                            if (moveHeights[c] != moveHeights[br])
                                                Console.WriteLine("  Mismatched corner/BR walk mesh heights (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + moveHeights[br]);
                                        }
                                    }
                                    else {
                                        var modelHeights = corners.ToDictionary(c => c, tile.GetModelVertexHeightmap);
                                        foreach (var c in corners) {
                                            if (moveHeights[c] != modelHeights[c])
                                                Console.WriteLine("  Mismatched walk/model mesh heights for (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + modelHeights[c]);
                                        }
                                    }

                                    // Report unknown or unhandled tile flags. Only Scenario 3+ has rotation flags 0x01 and 0x02.
                                    var weirdTexFlags = tile.ModelTextureFlags & ~0x30 & ~0x80;
                                    if (mpdFile.Scenario >= ScenarioType.Scenario3)
                                        weirdTexFlags &= ~0x03;
                                    if (weirdTexFlags != 0x00)
                                        Console.WriteLine("  @(" + tile.X + ", " + tile.Y + "): " + weirdTexFlags.ToString("X2"));
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception: '" + e.Message + "'. Skipping!");
                    }
                }
            }
        }
    }
}
