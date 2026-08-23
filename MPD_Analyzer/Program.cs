using System.Collections.Concurrent;
using System.Text;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using CommonLib.SGL;
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
            { ScenarioType.Scenario1,   "C:/SF3/Scenario1/" },
            { ScenarioType.Scenario2,   "C:/SF3/Scenario2/" },
            { ScenarioType.Scenario3,   "C:/SF3/Scenario3/" },
            { ScenarioType.PremiumDisk, "C:/SF3/PremiumDisk/" },
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

        public struct NormalMesh {
            public NormalMesh(byte[,] heights, VECTOR normal) {
                Heights = heights;
                Normal  = normal;
            }

            public byte[,] Heights;
            public VECTOR Normal;

            public override bool Equals(object? obj) {
                return obj is NormalMesh mesh &&
                    Normal == mesh.Normal &&
                    Heights[0, 0] == mesh.Heights[0, 0] &&
                    Heights[1, 0] == mesh.Heights[1, 0] &&
                    Heights[2, 0] == mesh.Heights[2, 0] &&
                    Heights[0, 1] == mesh.Heights[0, 1] &&
                    Heights[1, 1] == mesh.Heights[1, 1] &&
                    Heights[2, 1] == mesh.Heights[2, 1] &&
                    Heights[0, 2] == mesh.Heights[0, 2] &&
                    Heights[1, 2] == mesh.Heights[1, 2] &&
                    Heights[2, 2] == mesh.Heights[2, 2];
            }

            public override int GetHashCode() {
                var hashCode = Normal.GetHashCode();
                for (int y = 0; y < 3; y++)
                    for (int x = 0; x < 3; x++)
                        hashCode = hashCode * -1521134295 + Heights[x, y];
                return hashCode;
            }
        }

        public static HashSet<NormalMesh> s_normalMeshes = new HashSet<NormalMesh>();
        public static HashSet<NormalMesh> s_skippedMeshes = new HashSet<NormalMesh>();
        public static Mutex s_meshesMutex = new Mutex();

        private static string[]? MPD_MatchFunc(MPD_File mpdFile, ScenarioType scenario, string filename) {
            return MatchFuncs.ProjectCopyProducesSameMPDAsOriginal(mpdFile);
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

            var mutex = new Mutex();

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];
                var unusedMaps = UnusedMaps.TryGetValue(scenario, out var val) ? val : [];

                var parallelOptions = new ParallelOptions() {
                    MaxDegreeOfParallelism = -1
                };

                Parallel.ForEach(Partitioner.Create(filesKv.Value), parallelOptions, file => {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Skip maps that aren't used at all.
                    if (unusedMaps.Contains(filename))
                        return;

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts, scenario)) {
                            var header = mpdFile.MPDHeader;
                            var chunkHeaders = mpdFile.ChunkLocations;
                            var mapFlags = mpdFile.MPDHeader.MapFlags;

                            // Condition for match checks here
                            var matchReports = MPD_MatchFunc(mpdFile, scenario, filename);
                            if (matchReports == null)
                                return;

                            bool match = matchReports.Length > 0;
                            var fileStr = GetFileString(scenario, file, mpdFile);

                            var stringBuilder = new StringBuilder();

                            stringBuilder.AppendLine(fileStr + " | " + (match ? "Match  " : "NoMatch"));
                            if (matchReports.Length > 0) {
                                foreach (var r in matchReports)
                                    stringBuilder.AppendLine("    " + filename.PadLeft(8) + " | " + r);
                                stringBuilder.AppendLine();
                            }

                            mutex.WaitOne();
                            try {
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
                            }
                            finally {
                                mutex.ReleaseMutex();
                            }

                            //ScanForErrorsAndReport(scenario, mpdFile);
                            Console.Write(stringBuilder.ToString());
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                });
            }

            // Sort sets, which are in a somewhat random order.
            string MatchSorter(string str) => (str.StartsWith("Premium") ? "Z" : "") + str.Replace(" ", "");
            matchSet   = matchSet  .OrderBy(MatchSorter).ToList();
            nomatchSet = nomatchSet.OrderBy(MatchSorter).ToList();

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

            if (MatchFuncs.s_referencedTexturesByFile.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("===================================================");
                Console.WriteLine("| TEXTURE ORIGINS                                 |");
                Console.WriteLine("===================================================");

                var refsByHash = MatchFuncs.s_referencedTexturesByFile
                    .SelectMany(x => x.Value.Select(y => (File: x.Key, y.ID, y.Hash)))
                    .GroupBy(x => x.Hash)
                    .ToDictionary(x => x.Key, x => x
                        .Select(y => (y.File, y.ID))
                        .Distinct()
                        .OrderBy(x => x.File)
                        .ThenBy(x => x.ID)
                        .ToHashSet()
                    );

                var texsByHash = MatchFuncs.s_texturesByFile
                    .SelectMany(x => x.Value.Select(y => (File: x.Key, y.ID, y.Hash)))
                    .GroupBy(x => x.Hash)
                    .ToDictionary(x => x.Key, x => x
                        .Select(y => (y.File, y.ID))
                        .Distinct()
                        .OrderBy(x => x.File)
                        .ThenBy(x => x.ID)
                        .ToHashSet()
                    );

                using (var fileOut = new StreamWriter(new FileStream("TextureOrigins.txt", FileMode.Create))) {
                    void ConsoleFileWriteLine(string str) {
                        Console.WriteLine(str);
                        fileOut.WriteLine(str);
                    }

                    string NiceTexList(Dictionary<string, (string File, int ID)[]> refsByFile) {
                        return string.Join("; ", refsByFile
                            .Select(x => $"{x.Key} [" + string.Join(", ", x.Value.Select(y => $"0x:{y.ID:X2}")) + "]"));
                    }

                    foreach (var fileKv in MatchFuncs.s_unreferencedTexturesByFile) {
                        if (fileKv.Value.Count == 0)
                            continue;
 
                        var file = fileKv.Key;
                        ConsoleFileWriteLine($"{file}:");

                        foreach (var hashId in fileKv.Value) {
                            var hash = hashId.Hash;
                            var texId = hashId.ID;
                            var keyStr = $"0x{texId:X2} ({hash})";

                            if (!refsByHash.ContainsKey(hash)) {
                                if (texsByHash[hash].Count == 1)
                                    ConsoleFileWriteLine($" !! {keyStr}: Never referenced, only here!");
                                else {
                                    var all = texsByHash[hash].Where(x => x.File != file).ToArray();
                                    var allByFile = all.GroupBy(x => x.File).ToDictionary(x => x.Key, x => x.OrderBy(y => y.ID).ToArray());
                                    ConsoleFileWriteLine($" -- {keyStr}: Never referenced, but available: " + NiceTexList(allByFile));
                                }
                            }
                            else {
                                var refs = refsByHash[hash];
                                var refsByFile = refs.GroupBy(x => x.File).ToDictionary(x => x.Key, x => x.OrderBy(y => y.ID).ToArray());
                                ConsoleFileWriteLine($"    {keyStr}: " + NiceTexList(refsByFile));
                            }
                        }
                        ConsoleFileWriteLine("");
                    }
                }
            }

            if (s_normalMeshes.Count > 0 || s_skippedMeshes.Count > 0) {
                Console.WriteLine("");
                Console.WriteLine("===================================================");
                Console.WriteLine("| NORMAL MESHES                                   |");
                Console.WriteLine("===================================================");

                var sortedMeshes = s_normalMeshes
                    .OrderBy(x => x.Normal.Y.RawInt)
                    .ThenBy (x => (x.Heights[0, 0] << 16) + (x.Heights[1, 0] << 8) + x.Heights[2, 0])
                    .ThenBy (x => (x.Heights[0, 1] << 16) + (x.Heights[1, 1] << 8) + x.Heights[2, 1])
                    .ThenBy (x => (x.Heights[0, 2] << 16) + (x.Heights[1, 2] << 8) + x.Heights[2, 2])
                    .ToArray();

                using (var fileOut = new StreamWriter(new FileStream("StandardNormals.txt", FileMode.Create))) {
                    void ConsoleFileWriteLine(string str) {
                        //Console.WriteLine(str);
                        fileOut.WriteLine(str);
                    }
                    foreach (var sm in sortedMeshes) {
                        var h = sm.Heights;
                        var str =
                            $"[[{h[0,0]:X2}, {h[1,0]:X2}, {h[2,0]:X2}], " +
                             $"[{h[0,1]:X2}, {h[1,1]:X2}, {h[2,1]:X2}], " +
                             $"[{h[0,2]:X2}, {h[1,2]:X2}, {h[2,2]:X2}]] = {sm.Normal.ToString()}";
                        ConsoleFileWriteLine(str);
                    }
                }

                Console.WriteLine($"{sortedMeshes.Length} unique normals.");
                Console.WriteLine($"{s_skippedMeshes.Count} skipped.");
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

            var modelChunks = mpdFile.ModelCollections.Values.Select(x => x as ModelChunk).Where(x => x != null).ToArray();
            var hmm1  = HasHighMemoryModels(modelChunks.FirstOrDefault(x => x.ChunkIndex == 1));
            var hmm20 = HasHighMemoryModels(modelChunks.FirstOrDefault(x => x.ChunkIndex == 20));

            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                + " | " + mpdFile.Planes.GroundXRotation
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
