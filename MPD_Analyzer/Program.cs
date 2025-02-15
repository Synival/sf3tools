using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Files.MPD;
using SF3.Models.Structs.MPD;
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

        private static readonly Dictionary<ScenarioType, HashSet<string>> UnusedMaps = new() {
            { ScenarioType.Scenario1, [
                "FIELD",
                "HNSN00",
                "MGMA00",
                "MUHASI",
                "NASU00",
                "SHIO00",
                "SHIP2",
                "TESTMAP",
                "TNKA00",
                "TORI00",
                "TREE00",
                "TURI00",
            ]},
            { ScenarioType.Scenario3, [
                "AS_OKU",
                "BTL42",
                "FIELD",
                "MUHASI",
                "SNIOKI",
                "YSKI00",
                "VOID3",
            ]}
        };

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            var matchSet                = new List<string>();
            ushort matchFlagsPossible   = 0x0000;
            ushort matchFlagsAlways     = 0xFFFF;
            ushort matchFlagsNever      = 0xFFFF;
            var matchFlagsSet           = new HashSet<ushort>();

            var nomatchSet              = new List<string>();
            ushort nomatchFlagsPossible = 0x0000;
            ushort nomatchFlagsAlways   = 0xFFFF;
            ushort nomatchFlagsNever    = 0xFFFF;
            var nomatchFlagsSet         = new HashSet<ushort>();

            HashSet<int> matchChunksPossible   = [];
            HashSet<int> matchChunksAlways     = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];
            HashSet<int> matchChunksNever      = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];
            HashSet<int> nomatchChunksPossible = [];
            HashSet<int> nomatchChunksAlways   = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];
            HashSet<int> nomatchChunksNever    = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];
                var unusedMaps = UnusedMaps.TryGetValue(scenario, out var val) ? val : [];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Skip maps that aren't used at all.
                    if (unusedMaps.Contains(filename))
                        continue;

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts)) {
                            var header = mpdFile.MPDHeader[0];
                            var chunkHeaders = mpdFile.ChunkHeader;
                            var mapFlags = mpdFile.MPDHeader[0].MapFlags;

                            // Condition for match checks here
                            var lightAdj = mpdFile.LightAdjustmentTable?[0];
                            bool match = lightAdj?.HasGroundAdjust == true &&
                                (lightAdj.GroundRAdjustment != 0 ||
                                 lightAdj.GroundGAdjustment != 0 ||
                                 lightAdj.GroundBAdjustment != 0);

                            var fileStr = GetFileString(scenario, file, mpdFile);
                            Console.Write(fileStr + " | " + (match ? "Match  " : "NoMatch"));

                            Console.WriteLine();

                            if (match) {
                                matchSet.Add(fileStr);

                                matchFlagsPossible |= mapFlags;
                                matchFlagsAlways &= mapFlags;
                                matchFlagsNever  &= (ushort) ~mapFlags;
                                matchFlagsSet.Add(mapFlags);

                                foreach (var ch in chunkHeaders) {
                                    if (ch.Exists) {
                                        matchChunksPossible.Add(ch.ID);
                                        matchChunksNever.Remove(ch.ID);
                                    }
                                    else
                                        matchChunksAlways.Remove(ch.ID);
                                }
                            }
                            else {
                                nomatchSet.Add(fileStr);

                                nomatchFlagsPossible |= mapFlags;
                                nomatchFlagsAlways &= mapFlags;
                                nomatchFlagsNever &= (ushort) ~mapFlags;
                                nomatchFlagsSet.Add(mapFlags);

                                foreach (var ch in chunkHeaders) {
                                    if (ch.Exists) {
                                        nomatchChunksPossible.Add(ch.ID);
                                        nomatchChunksNever.Remove(ch.ID);
                                    }
                                    else
                                        nomatchChunksAlways.Remove(ch.ID);
                                }
                            }

                            ScanForErrorsAndReport(scenario, mpdFile);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            var totalCount = matchSet.Count + nomatchSet.Count;

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| MATCH RESULTS                                   |");
            Console.WriteLine("===================================================");

            Console.WriteLine("");
            Console.WriteLine($"Match: {matchSet.Count}/{totalCount}");
            foreach (var str in matchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine($"NoMatch: {nomatchSet.Count}/{totalCount}");
            foreach (var str in nomatchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| FLAG CHECKS                                     |");
            Console.WriteLine("===================================================");

            if (matchSet.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("Match:");
                Console.WriteLine("  Possible: " + matchFlagsPossible.ToString("X4") + ": " + BitString(matchFlagsPossible));
                Console.WriteLine("  Always:   " + matchFlagsAlways.ToString("X4")   + ": " + BitString(matchFlagsAlways));
                Console.WriteLine("  Never:    " + matchFlagsNever.ToString("X4")    + ": " + BitString(matchFlagsNever));
                Console.WriteLine("  Sets:");
                foreach (var mapFlags in matchFlagsSet.Order().ToList())
                    Console.WriteLine("    " + mapFlags.ToString("X4") + ": " + BitString(mapFlags));
            }

            if (nomatchSet.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("NoMatch:");
                Console.WriteLine("  Possible: " + nomatchFlagsPossible.ToString("X4") + ": " + BitString(nomatchFlagsPossible));
                Console.WriteLine("  Always:   " + nomatchFlagsAlways.ToString("X4")   + ": " + BitString(nomatchFlagsAlways));
                Console.WriteLine("  Never:    " + nomatchFlagsNever.ToString("X4")    + ": " + BitString(nomatchFlagsNever));
                Console.WriteLine("  Sets:");
                foreach (var mapFlags in nomatchFlagsSet.Order().ToList())
                    Console.WriteLine("    " + mapFlags.ToString("X4") + ": " + BitString(mapFlags));
            }

            if (matchSet.Count > 0 && nomatchSet.Count > 0) {
                var unionFlags = (ushort) (matchFlagsAlways & nomatchFlagsAlways);
                var diffFlags  = (ushort) (matchFlagsAlways ^ nomatchFlagsAlways);
                Console.WriteLine("");
                Console.WriteLine($"Always union: " + unionFlags.ToString("X4") + ": " + BitString(unionFlags));
                Console.WriteLine($"Always diff:  " + diffFlags.ToString("X4")  + ": " + BitString(diffFlags));
            }

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| CHUNK CHECKS                                    |");
            Console.WriteLine("===================================================");

            if (matchSet.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("Match:");
                Console.WriteLine("  Possible: " + string.Join(", ", matchChunksPossible.Order().ToArray()));
                Console.WriteLine("  Always:   " + string.Join(", ", matchChunksAlways.Order().ToArray()));
                Console.WriteLine("  Never:    " + string.Join(", ", matchChunksNever.Order().ToArray()));
            }

            if (nomatchSet.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("NoMatch:");
                Console.WriteLine("  Possible: " + string.Join(", ", nomatchChunksPossible.Order().ToArray()));
                Console.WriteLine("  Always:   " + string.Join(", ", nomatchChunksAlways.Order().ToArray()));
                Console.WriteLine("  Never:    " + string.Join(", ", nomatchChunksNever.Order().ToArray()));
            }
        }

        private static string BitString(ushort bits) {
            var str = "";
            for (var i = 0; i < 16; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string ChunkString(ChunkHeader[] chunkHeaders) {
            var chunkString = "";
            for (var i = 0; i < chunkHeaders.Length; i++) {
                if (chunkHeaders[i].Address == 0)
                    break;
                if (i % 4 == 0 && i != 0)
                    chunkString += ",";
                chunkString += (chunkHeaders[i].Exists) ? "1" : "0";
            }
            return chunkString;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, IMPD_File mpdFile) {
            var mapFlags = mpdFile.MPDHeader[0].MapFlags;
            var chunkHeaders = mpdFile.ChunkHeader;

            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                + " | " + mapFlags.ToString("X4") + ", " + BitString(mapFlags)
                + " | " + ChunkString(chunkHeaders.Rows);
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, IMPD_File mpdFile) {
            var totalErrors = new List<string>();

            var header = mpdFile.MPDHeader[0];

            totalErrors.AddRange(ScanForCorrectScenario(inputScenario, mpdFile));
            totalErrors.AddRange(ScanForNonZeroHeaderPadding(mpdFile));
            totalErrors.AddRange(ScanForChunkHeaderErrors(mpdFile));
            totalErrors.AddRange(ScanForPaletteErrors(mpdFile));
            totalErrors.AddRange(ScanForSurfaceModelErrors(mpdFile));
            totalErrors.AddRange(ScanForGroundErrors(mpdFile));
            totalErrors.AddRange(ScanForSkyBoxErrors(mpdFile));

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }

        private static string[] ScanForCorrectScenario(ScenarioType inputScenario, IMPD_File mpdFile) {
            // Is this MPD file in the wrong format for this scenario? (Scenario 3 and Premium Disk are the same)
            // (This actually happens!)
            var expectedScenario = (inputScenario == ScenarioType.PremiumDisk) ? ScenarioType.Scenario3 : inputScenario;
            return mpdFile.Scenario != expectedScenario
                ? ["Wrong scenario for this disc! ShouldBe=" + expectedScenario + ", Is=" + mpdFile.Scenario]
                : [];
        }

        private static string[] ScanForNonZeroHeaderPadding(IMPD_File mpdFile) {
            var header = mpdFile.MPDHeader[0];
            var errors = new List<string>();

            void CheckPadding(string prop, int value) {
                if (value != 0)
                    errors.Add($"{prop} has non-zero data: {value.ToString("X4")}");
            }

            CheckPadding(nameof(header.Padding1), header.Padding1);
            CheckPadding(nameof(header.Padding2), header.Padding2);
            CheckPadding(nameof(header.Padding3), header.Padding3);
            CheckPadding(nameof(header.Padding4), header.Padding4);

            return errors.ToArray();
        }

        private static string[] ScanForChunkHeaderErrors(IMPD_File mpdFile) {
            var chunkHeaders = mpdFile.ChunkHeader;
            var errors = new List<string>();

            // Chunk[0] and Chunk[4] should always be empty.
            if (chunkHeaders[0].Exists)
                errors.Add("Chunk[0] exists!");
            if (chunkHeaders[4].Exists)
                errors.Add("Chunk[4] exists!");

            // Anything Scenario 2 or higher should have addresses for Chunk[20] and Chunk[21].
            bool shouldHaveChunk20_21 = mpdFile.Scenario >= ScenarioType.Scenario2;
            bool hasChunk20 = chunkHeaders[20].ChunkAddress > 0;
            bool hasChunk21 = chunkHeaders[21].ChunkAddress > 0;

            if (shouldHaveChunk20_21 != hasChunk20)
                errors.Add("Chunk[20] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk20);
            if (shouldHaveChunk20_21 != hasChunk21)
                errors.Add("Chunk[21] problem! ShouldHave=" + shouldHaveChunk20_21 + ", DoesHave=" + hasChunk21);

            return errors.ToArray();
        }

        private static string[] ScanForPaletteErrors(IMPD_File mpdFile) {
            var header = mpdFile.MPDHeader[0];
            var errors = new List<string>();

            // Anything Scenario 3 or higher should have Palette3.
            bool shouldHavePalette3 = mpdFile.Scenario >= ScenarioType.Scenario3;
            bool hasPalette3 = header.Data.GetWord(header.Address + 0x44) == 0x0029;
            if (shouldHavePalette3 != hasPalette3)
                errors.Add("Palette3 problem! ShouldHave=" + shouldHavePalette3 + ", DoesHave=" + hasPalette3);

            // If the disc is Scenario 3, all palettes should be present.
            if (mpdFile.Scenario >= ScenarioType.Scenario3) {
                if (mpdFile.PaletteTables[0] == null)
                    errors.Add("Scenario3 Palette[0] doesn't exist!");
                if (mpdFile.PaletteTables[1] == null)
                    errors.Add("Scenario3 Palette[1] doesn't exist!");
                if (mpdFile.PaletteTables[2] == null)
                    errors.Add("Scenario3 Palette[2] doesn't exist!");
            }

            return errors.ToArray();
        }

        private static string[] ScanForSurfaceModelErrors(IMPD_File mpdFile) {
            var errors = new List<string>();

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
                                errors.Add("Mismatched corner/BR walk mesh heights (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + moveHeights[br]);
                        }
                    }
                    else {
                        var modelHeights = corners.ToDictionary(c => c, tile.GetModelVertexHeightmap);
                        foreach (var c in corners) {
                            if (moveHeights[c] != modelHeights[c])
                                errors.Add("Mismatched walk/model mesh heights for (" + tile.X + ", " + tile.Y + "), " + c.ToString() + ": " + moveHeights[c] + " != " + modelHeights[c]);
                        }
                    }

                    // Report unknown or unhandled tile flags. Only Scenario 3+ has rotation flags 0x01 and 0x02.
                    var weirdTexFlags = tile.ModelTextureFlags & ~0x30 & ~0x80;
                    if (mpdFile.Scenario >= ScenarioType.Scenario3)
                        weirdTexFlags &= ~0x03;
                    if (weirdTexFlags != 0x00)
                        errors.Add("Unhandled tile texture flags: @(" + tile.X + ", " + tile.Y + "): " + weirdTexFlags.ToString("X2"));
                }
            }

            return errors.ToArray();
        }

        private static string[] ScanForGroundErrors(IMPD_File mpdFile) {
            var header = mpdFile.MPDHeader[0];
            var chunkHeaders = mpdFile.ChunkHeader;
            var errors = new List<string>();

            // Check for ground chunks.
            if (header.GroundImageType == GroundImageType.Invalid)
                errors.Add($"Has invalid GroundImageType: {header.GroundImageType} ({((int) header.GroundImageType).ToString("X4")})");
            else if (header.GroundImageType == GroundImageType.Repeated) {
                // TODO: proper chunk indices
                if (!chunkHeaders[14].Exists)
                    errors.Add("HasRepeatingGround is 'true', but Chunk[14] is missing!");
                if (!chunkHeaders[15].Exists)
                    errors.Add("HasRepeatingGround is 'true', but Chunk[15] is missing!");
            }
            else if (header.GroundImageType == GroundImageType.Tiled) {
                // TODO: proper chunk indices
                if (!chunkHeaders[14].Exists)
                    errors.Add("HasTiledGround is 'true', but Chunk[14] is missing!");
                if (!chunkHeaders[15].Exists)
                    errors.Add("HasTiledGround is 'true', but Chunk[15] is missing!");
                if (!chunkHeaders[16].Exists)
                    errors.Add("HasTiledGround is 'true', but Chunk[16] is missing!");
                if (!chunkHeaders[19].Exists)
                    errors.Add("HasTiledGround is 'true', but Chunk[19] is missing!");
            }
            else {
                // TODO: proper chunk indices
                // TODO: check for backgrounds
                if (chunkHeaders[14].Exists)
                    errors.Add("Has no ground, but Chunk[14] exists!");
                if (chunkHeaders[15].Exists)
                    errors.Add("Has no ground, but Chunk[15] exists!");
                if (chunkHeaders[16].Exists)
                    errors.Add("Has no ground, but Chunk[16] exists!");
                if (chunkHeaders[19].Exists)
                    errors.Add("Has no ground, but Chunk[19] exists!");
            }

            return errors.ToArray();
        }

        private static string[] ScanForSkyBoxErrors(IMPD_File mpdFile) {
            var header = mpdFile.MPDHeader[0];
            var chunkHeaders = mpdFile.ChunkHeader;
            var errors = new List<string>();

            // Check for skybox chunks.
            if (header.HasSkyBox) {
                // TODO: proper chunk indices
                if (mpdFile.Scenario > ScenarioType.Scenario1 && !chunkHeaders[17].Exists && !chunkHeaders[18].Exists)
                    // TODO: not an error
                    errors.Add("(normal) SkyBox is elsewhere");
                else {
                    // TODO: proper chunk indices
                    if (!chunkHeaders[17].Exists)
                        errors.Add("HasSkyBox is 'true', but Chunk[17] is missing!");
                    if (!chunkHeaders[18].Exists)
                        errors.Add("HasSkyBox is 'true', but Chunk[18] is missing!");
                }
            }
            else if (header.BackgroundImageType.HasFlag(BackgroundImageType.Tiled)) {
                // TODO: proper chunk indices
                if (!chunkHeaders[17].Exists)
                    errors.Add("HasBackground is 'true', but Chunk[17] is missing!");
                if (!chunkHeaders[18].Exists)
                    errors.Add("HasBackground is 'true', but Chunk[18] is missing!");
                if (!chunkHeaders[19].Exists)
                    errors.Add("HasBackground is 'true', but Chunk[19] is missing!");
            }
            else {
                // TODO: proper chunk indices
                // TODO: check for backgrounds
                if (chunkHeaders[17].Exists)
                    errors.Add("HasSkyBox is 'false', but Chunk[17] exists!");
                if (chunkHeaders[18].Exists)
                    errors.Add("HasSkyBox is 'false', but Chunk[18] exists!");
            }

            return errors.ToArray();
        }
    }
}
