using CommonLib.Arrays;
using CommonLib.NamedValues;
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
            { ScenarioType.Scenario2, [
                "FIELD",
                "HOR",
                "MUHASI",
                "SOGEST",
                "TESMAP",
                "TEST01",
                "TM_OC",
                "YSKI00",
                "VOID",
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

        private static string[]? MPD_MatchFunc(IMPD_File mpdFile, string filename) {
            return [];
        }

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
            HashSet<int> matchChunksPossible   = [];
            HashSet<int> matchChunksAlways     = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];
            HashSet<int> matchChunksNever      = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21];

            var nomatchSet              = new List<string>();
            ushort nomatchFlagsPossible = 0x0000;
            ushort nomatchFlagsAlways   = 0xFFFF;
            ushort nomatchFlagsNever    = 0xFFFF;
            var nomatchFlagsSet         = new HashSet<ushort>();
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
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts, scenario)) {
                            var header = mpdFile.MPDHeader;
                            var chunkHeaders = mpdFile.ChunkLocations;
                            var mapFlags = mpdFile.MPDHeader.MapFlags;

                            // Condition for match checks here
                            var matchReports = MPD_MatchFunc(mpdFile, filename);
                            if (matchReports == null)
                                continue;

                            bool match = matchReports.Length > 0;
                            var fileStr = GetFileString(scenario, file, mpdFile);
                            Console.WriteLine(fileStr + " | " + (match ? "Match  " : "NoMatch"));
                            if (matchReports.Length > 0) {
                                foreach (var r in matchReports)
                                    Console.WriteLine("    " + filename.PadLeft(8) + " | " + r);
                                Console.WriteLine();
                            }

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

        private static string ChunkString(ChunkLocation[] chunkHeaders) {
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

        private static bool? HasHighMemoryModels(ModelChunk? mc) {
            if (mc == null)
                return null;
            return mc.PDatasByMemoryAddress.Values.Count == 0
                ? null
                : mc.PDatasByMemoryAddress.Values.First().RamAddress >= 0x0600_0000;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, IMPD_File mpdFile) {
            var mapFlags = mpdFile.MPDHeader.MapFlags;
            var chunkLocations = mpdFile.ChunkLocations;

            var hmm1  = HasHighMemoryModels(mpdFile.ModelCollections.Values.Cast<ModelChunk>().FirstOrDefault(x => x.ChunkIndex == 1));
            var hmm20 = HasHighMemoryModels(mpdFile.ModelCollections.Values.Cast<ModelChunk>().FirstOrDefault(x => x.ChunkIndex == 20));

            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                + " | " + mpdFile.Settings.ViewDistance.ToString("X2")
                + " | " + mapFlags.ToString("X4") + ", " + BitString(mapFlags)
                + " | " + ChunkString(chunkLocations.Rows)
                + " | " + (hmm1  == true ? "High, " : hmm1  == false ? "Low,  " : "N/A,  ")
                + (hmm20 == true ? "High" : hmm20 == false ? "Low " : "N/A ");
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, IMPD_File mpdFile) {
            var totalErrors = new List<string>();

            var header = mpdFile.MPDHeader;

            totalErrors.AddRange(ScanForCorrectScenario(inputScenario, mpdFile));
            totalErrors.AddRange(mpdFile.GetErrors());

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
    }
}
