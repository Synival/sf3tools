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
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts)) {
                            var header = mpdFile.MPDHeader[0];
                            var chunkHeaders = mpdFile.ChunkHeader;

                            Console.WriteLine(scenario.ToString() + ": " + Path.GetFileName(file) + " (0x" + header.MapFlags.ToString("X4") + ")");

                            // Is this MPD file in the wrong format for this scenario? (Scenario 3 and Premium Disk are the same)
                            // (This actually happens!)
                            var expectedScenario = (scenario == ScenarioType.PremiumDisk) ? ScenarioType.Scenario3 : scenario;
                            if (mpdFile.Scenario != expectedScenario)
                                Console.WriteLine("  !!! Wrong scenario for this disc! ShouldBe=" + expectedScenario + ", Is=" + mpdFile.Scenario);

                            // Anything Scenario 2 or higher should have addresses for Chunk[20] and Chunk[21].
                            bool shouldHaveChunk20_21 = mpdFile.Scenario >= ScenarioType.Scenario2;
                            bool hasChunk20 = chunkHeaders[20].ChunkAddress > 0;
                            bool hasChunk21 = chunkHeaders[21].ChunkAddress > 0;

                            if (shouldHaveChunk20_21 != hasChunk20)
                                Console.WriteLine("  !!! Chunk[20] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk20);
                            if (shouldHaveChunk20_21 != hasChunk21)
                                Console.WriteLine("  !!! Chunk[21] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk21);

                            // Only a few files have both Chunk[1] and Chunk[20]. Let's log them.
                            if (chunkHeaders[1].Exists && chunkHeaders[20].Exists && mpdFile.SurfaceModelChunkIndex != 20)
                                Console.WriteLine("  Info: has Chunk[1] and Chunk[20]");

                            // For Scenario 2 onwards, if Chunk 2 is empty, Chunk 20 should probably have the surface data, or nothing at all.
                            if (mpdFile.Scenario >= ScenarioType.Scenario2) {
                                if (!chunkHeaders[2].Exists) {
                                    var fn = file;
                                    if (chunkHeaders[20].Exists && chunkHeaders[20].ChunkSize != 0xCF00)
                                        Console.WriteLine("  !!! Chunk[2] missing, Chunk[20] present, but not surface data!");
                                    if (chunkHeaders[21].Exists)
                                        Console.WriteLine("  !!! Chunk[2] missing, but Chunk[21] exists??");
                                }
                                else {
                                    if (!chunkHeaders[20].Exists) {
                                        // Three valid MPDs in Scenario 3 have Chunk[2] but no Chunk[20], but that's it.
                                        // This isn't the case in Scenario 2 or the Premium Disk.
                                        Console.WriteLine("  !!! Chunk[2] exists, but Chunk[20] should exist as well!");
                                        if (chunkHeaders[21].Exists)
                                            Console.WriteLine("  !!! Chunk[2] and Chunk[20] is missing, but Chunk[21] exists??");
                                    }
                                }
                            }

                            // Anything Scenario 3 or higher should have Palette3.
                            bool shouldHavePalette3 = mpdFile.Scenario >= ScenarioType.Scenario3;
                            bool hasPalette3 = header.Data.GetWord(header.Address + 0x44) == 0x0029;
                            if (shouldHavePalette3 != hasPalette3)
                                Console.WriteLine("  !!! Palette3 problem! ShouldHave=" + shouldHavePalette3 + ", DoesHave=" + hasPalette3);

                            // If the disc is Scenario 3, all palettes should be present.
                            if (mpdFile.Scenario >= ScenarioType.Scenario3) {
                                if (mpdFile.PaletteTables[0] == null)
                                    Console.WriteLine("  !!! Scenario3 Palette[0] doesn't exist!");
                                if (mpdFile.PaletteTables[1] == null)
                                    Console.WriteLine("  !!! Scenario3 Palette[1] doesn't exist!");
                                if (mpdFile.PaletteTables[2] == null)
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
                                                Console.WriteLine("  !!! Mismatched corner/BR walk mesh heights (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + moveHeights[br]);
                                        }
                                    }
                                    else {
                                        var modelHeights = corners.ToDictionary(c => c, tile.GetModelVertexHeightmap);
                                        foreach (var c in corners) {
                                            if (moveHeights[c] != modelHeights[c])
                                                Console.WriteLine("  !!! Mismatched walk/model mesh heights for (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + modelHeights[c]);
                                        }
                                    }

                                    // Report unknown or unhandled tile flags. Only Scenario 3+ has rotation flags 0x01 and 0x02.
                                    var weirdTexFlags = tile.ModelTextureFlags & ~0x30 & ~0x80;
                                    if (mpdFile.Scenario >= ScenarioType.Scenario3)
                                        weirdTexFlags &= ~0x03;
                                    if (weirdTexFlags != 0x00)
                                        Console.WriteLine("  !!! Unhandled tile texture flags: @(" + tile.X + ", " + tile.Y + "): " + weirdTexFlags.ToString("X2"));
                                }
                            }

                            if (header.Padding1 != 0 || header.Padding2 != 0 || header.Padding3 != 0 || header.Padding4 != 0) {
                                Console.WriteLine($"  !!! Padding has non-zero data:");
                                Console.WriteLine($"    1={header.Padding1}, 2={header.Padding2}, 3={header.Padding3}, 4={header.Padding4}");
                            }

                            // Chunk[0] and Chunk[4] should always be empty.
                            if (chunkHeaders[0].Exists)
                                Console.WriteLine("  !!! Chunk[0] exists!");
                            if (chunkHeaders[4].Exists)
                                Console.WriteLine("  !!! Chunk[4] exists!");

                            // Check for ground chunks.
                            if (header.HasRepeatingGround && header.HasTiledGround)
                                Console.WriteLine("  !!! Has both HasRepeatingGround and HasTiledGround!");
                            else if (header.HasRepeatingGround) {
                                if (!chunkHeaders[14].Exists)
                                    Console.WriteLine("  !!! HasRepeatingGround is 'true', but Chunk[14] is missing!");
                                if (!chunkHeaders[15].Exists)
                                    Console.WriteLine("  !!! HasRepeatingGround is 'true', but Chunk[15] is missing!");
                            }
                            else if (header.HasTiledGround) {
                                if (!chunkHeaders[14].Exists)
                                    Console.WriteLine("  !!! HasTiledGround is 'true', but Chunk[14] is missing!");
                                if (!chunkHeaders[15].Exists)
                                    Console.WriteLine("  !!! HasTiledGround is 'true', but Chunk[15] is missing!");
                                if (!chunkHeaders[16].Exists)
                                    Console.WriteLine("  !!! HasTiledGround is 'true', but Chunk[16] is missing!");
                                if (!chunkHeaders[19].Exists)
                                    Console.WriteLine("  !!! HasTiledGround is 'true', but Chunk[19] is missing!");
                            }
                            else if (header.HasBackground) {
                                if (!chunkHeaders[14].Exists)
                                    Console.WriteLine("  !!! HasBackground is 'true', but Chunk[14] is missing!");
                                if (!chunkHeaders[15].Exists)
                                    Console.WriteLine("  !!! HasBackground is 'true', but Chunk[15] is missing!");
                            }
                            else {
                                if (chunkHeaders[14].Exists)
                                    Console.WriteLine("  !!! Has no ground, but Chunk[14] exists!");
                                if (chunkHeaders[15].Exists)
                                    Console.WriteLine("  !!! Has no ground, but Chunk[15] exists!");
                                if (chunkHeaders[16].Exists)
                                    Console.WriteLine("  !!! Has no ground, but Chunk[16] exists!");
                                if (chunkHeaders[19].Exists)
                                    Console.WriteLine("  !!! Has no ground, but Chunk[19] exists!");
                            }

                            // Check Scenario 1 flags.
                            // TODO: this totally applies to other scenarios, but these flags aren't working yet.
                            if (scenario == ScenarioType.Scenario1) {
                                // Check for skybox chunks.
                                if (header.HasSkyBox) {
                                    if (!chunkHeaders[17].Exists)
                                        Console.WriteLine("  !!! HasSkyBox is 'true', but Chunk[17] is missing!");
                                    if (!chunkHeaders[18].Exists)
                                        Console.WriteLine("  !!! HasSkyBox is 'true', but Chunk[18] is missing!");
                                }
                                else if (header.HasBackground) {
                                    if (!chunkHeaders[17].Exists)
                                        Console.WriteLine("  !!! HasBackground is 'true', but Chunk[17] is missing!");
                                    if (!chunkHeaders[18].Exists)
                                        Console.WriteLine("  !!! HasBackground is 'true', but Chunk[18] is missing!");
                                    if (!chunkHeaders[19].Exists)
                                        Console.WriteLine("  !!! HasBackground is 'true', but Chunk[19] is missing!");
                                }
                                else {
                                    if (chunkHeaders[17].Exists)
                                        Console.WriteLine("  !!! HasSkyBox is 'false', but Chunk[17] exists!");
                                    if (chunkHeaders[18].Exists)
                                        Console.WriteLine("  !!! HasSkyBox is 'false', but Chunk[18] exists!");
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }
        }
    }
}
