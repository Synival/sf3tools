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

        // Dictionary<earlierHash, laterHash>
#if false
        private static readonly Dictionary<string, HashSet<string>> s_textureHashRelationships = [];

        private static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_texturesByFile = [];
        private static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_referencedTexturesByFile = [];
        private static readonly Dictionary<string, HashSet<(string Hash, int ID)>> s_unreferencedTexturesByFile = [];
        private static string[,]? s_bochiTextureHashes = null;
#endif

        private static string[]? MPD_MatchFunc(IMPD_File mpdFile, ScenarioType scenario, string filename) {
            // Gotta have the model collection!
            if (mpdFile.ModelCollections == null || !mpdFile.ModelCollections.ContainsKey(CollectionType.Primary))
                return null;
            // Gotta have textures!
            if (!(mpdFile.ModelCollections[CollectionType.Primary]?.Textures?.Count() >= 1))
                return null;

            var texturesById = mpdFile.ModelCollections[CollectionType.Primary].Textures.ToDictionary(x => x.ID, x => x);
            var modelsById = mpdFile.ModelCollections[CollectionType.Primary].Models.ToDictionary(x => x.ID, x => x);

#if false
            var duplicatedTextures = mpdFile.ModelCollections[CollectionType.Primary].Textures.GroupBy(x => x.Hash).Where(x => x.Count() > 1).Select(x => x.ToArray()).ToArray();
            if (duplicatedTextures.Length == 0)
                return null;
            return duplicatedTextures.Select(x => x[0].Hash + ": " + string.Join(", ", x.Select(y => $"0x{y.ID:X2}"))).ToArray();
#elif false
            var allModelsWithDuplicateTexturesInternally = modelsById
                .ToDictionary(x => x.Value, x => x.Value.Faces
                    .Select((x, i) => (Face: x, FaceIndex: i))
                    .Where(y => y.Face.Attributes.UseTexture)
                    .GroupBy(y => y.Face.Attributes.TextureNo)
                    .ToDictionary(y => y.Key, y => y.Select(z => (z.Face, z.FaceIndex, Texture: texturesById[z.Face.Attributes.TextureNo])).ToArray())
                    .GroupBy(y => y.Value.First().Texture.Hash)
                    .Where(y => y.Count() > 1)
                    .ToDictionary(y => y.Key, y => y.ToDictionary())
                )
                .Where(x => x.Value.Count > 0)
                .ToDictionary();

            if (allModelsWithDuplicateTexturesInternally.Count == 0)
                return null;
            return allModelsWithDuplicateTexturesInternally
                .Select(x => $"Model 0x{x.Key.ID:X2}:\r\n  " + string.Join("\r\n  ", x.Value
                    .Select(y => $"{y.Key}: " + string.Join("; ", y.Value
                        .Select(z => $"Tex0x{z.Key:X2} (Faces: " + string.Join(",", z.Value.Select(a => $"0x{a.FaceIndex:X2}")) + ")")
                    ))
                )).ToArray();
#elif false
            var texturesUsedByModel = modelsById.Values
                .ToDictionary(x => x.ID, x => x.Faces
                    .Where(x => x.Attributes.UseTexture)
                    .Select(x => x.Attributes.TextureNo)
                    .Distinct()
                    .ToHashSet()
            );
            var allTexturesUsedInModels = texturesUsedByModel.SelectMany(x => x.Value).Distinct().ToHashSet();

            var texturesSharedBetweenModels = allTexturesUsedInModels
                .ToDictionary(x => x, x => texturesUsedByModel.Where(y => y.Value.Contains(x)).Select(y => modelsById[y.Key]).ToArray())
                .Where(x => x.Value.Length > 1)
                .ToDictionary();

            if (texturesSharedBetweenModels.Count == 0)
                return null;
            return texturesSharedBetweenModels.Select(x => $"Tex0x{x.Key:X2}: " + string.Join(", ", x.Value.Select(y => $"Model0x{y.ID:X2}"))).ToArray();
#elif false
            if (!mpdFile.Surface.HasModel)
                return null;
            var surfaceMapTextures = mpdFile.Surface.GetAllTiles().Select(x => (int) x.TextureID).Distinct().Where(x => x != 0xFF).ToHashSet();
            var modelTextures = mpdFile.ModelCollections[CollectionType.Primary].Models.SelectMany(x => x.Faces.Select(y => (int) y.Attributes.TextureNo)).Distinct().ToHashSet();

            var texturesInBoth = surfaceMapTextures.Where(modelTextures.Contains).Select(x => texturesById[x]).ToArray();
            if (texturesInBoth.Length == 0)
                return null;
            return texturesInBoth.Select(x => x.Hash + $": 0x{x.ID:X2}").ToArray();
#elif false
            bool setting = s_bochiTextureHashes == null;
            if (s_bochiTextureHashes == null)
                s_bochiTextureHashes = new string[64, 64];

            var results = new List<string>();
            foreach (var tile in mpdFile.Surface.GetAllTiles()) {
                var texture = (tile.TextureID == 0xFF) ? null : texturesById[tile.TextureID];
                var hash = texture?.Hash ?? "(none)";
                if (setting)
                    s_bochiTextureHashes[tile.X, tile.Y] = hash;
                else if (s_bochiTextureHashes[tile.X, tile.Y] != hash)
                    results.Add($"Different texture at ({tile.X}, {tile.Y})");
            }

            return results.ToArray();
#elif false
            if (!mpdFile.Surface.HasModel)
                return null;
            var surfaceMapTextures = mpdFile.Surface
                .GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .GroupBy(x => x.TextureID)
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.OrderBy(y => y.X).ThenBy(y => y.Y).ToArray());

            return surfaceMapTextures
                .Select(x => $"Tex0x{x.Key:X2} ({texturesById[x.Key].Hash}): " + string.Join(", ", x.Value.Select(y => $"({y.X},{y.Y})"))).ToArray();
#elif false
            var textureIdsFromModels = modelsById.Values
                .SelectMany(x => x.Faces.Where(y => y.Attributes.UseTexture).Select(y => (int) y.Attributes.TextureNo))
                .Distinct()
                .ToHashSet();

            var textureIdsFromSurfaceMap = (!mpdFile.Surface.HasModel) ? [] : mpdFile.Surface.GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .Select(x => (int) x.TextureID)
                .Distinct()
                .ToHashSet();

            var usedTextureIds = new HashSet<int>();
            foreach (var id in textureIdsFromModels)
                usedTextureIds.Add(id);
            foreach (var id in textureIdsFromSurfaceMap)
                usedTextureIds.Add(id);

            // (count the animation frames as used textures, since they're still referenced and *probably* used)
            if (mpdFile.TextureAnimationsAlt != null)
                foreach (var entry in mpdFile.TextureAnimationsAlt)
                    usedTextureIds.Add(entry.TextureID);

            var usedTextures = usedTextureIds
                .OrderBy(x => x)
                .Where(texturesById.ContainsKey)
                .Select(x => texturesById[x])
                .ToArray();
            var unusedTextures = texturesById
                .Where(x => !usedTextureIds.Contains(x.Key))
                .OrderBy(x => x.Key)
                .Select(x => x.Value)
                .ToArray();

            string GetShortScenarioName() {
                switch (scenario) {
                    case ScenarioType.Scenario1:   return "S1";
                    case ScenarioType.Scenario2:   return "S2";
                    case ScenarioType.Scenario3:   return "S3";
                    case ScenarioType.PremiumDisk: return "PD";
                    default:                       return "??";
                }
            }
            var fileKey = $"{GetShortScenarioName()}|{filename}";
            s_texturesByFile[fileKey] = texturesById.Values.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();
            s_referencedTexturesByFile[fileKey] = usedTextures.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();
            s_unreferencedTexturesByFile[fileKey] = unusedTextures.Select(x => (x.Hash, x.ID)).Distinct().ToHashSet();

            return []; //unusedTextures.Select(x => $"Tex0x{x.ID:X2} ({x.Hash})").ToArray();
#elif false
            var lastTexture = texturesById.Max(x => x.Key);

            var missingTextureIdsFromModels = modelsById.Values
                .SelectMany(x => x.Faces.Where(y => y.Attributes.UseTexture).Select(y => (int) y.Attributes.TextureNo))
                .Distinct()
                .Where(x => x > lastTexture)
                .ToHashSet();

            var missingTextureIdsFromSurfaceMap = (!mpdFile.Surface.HasModel) ? [] : mpdFile.Surface.GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .Select(x => (int) x.TextureID)
                .Distinct()
                .Where(x => x > lastTexture)
                .ToHashSet();

            var missingTextureIds = new HashSet<int>();
            foreach (var id in missingTextureIdsFromModels)
                missingTextureIds.Add(id);
            foreach (var id in missingTextureIdsFromSurfaceMap)
                missingTextureIds.Add(id);

            if (mpdFile.TextureAnimationsAlt != null)
                foreach (var entry in mpdFile.TextureAnimationsAlt.OrderBy(x => x.TextureID))
                    if (entry.TextureID > lastTexture)
                        missingTextureIds.Add(entry.TextureID);

            return missingTextureIds.Select(x => $"0x{x:X2}").ToArray();
#elif false
            var usedModelIDs = mpdFile.ModelCollections[CollectionType.Primary].ModelInstances
                .Where(x => x.PositionX >= -0x800 && x.PositionX <= 0x1000)
                .Where(x => x.PositionY >= -0x100 && x.PositionY <= 0x100)
                .Where(x => x.PositionZ >= -0x800 && x.PositionZ <= 0x1000)
                .Select(x => x.ModelID)
                .Distinct()
                .Order()
                .ToHashSet();

            var unusedModelIDs = mpdFile.ModelCollections[CollectionType.Primary].Models
                .Select(x => x.ID)
                .Where(x => !usedModelIDs.Contains(x))
                .Order()
                .ToHashSet();

            return unusedModelIDs.Select(x => $"Model0x{x:X2}").ToArray();
#elif true
            if (mpdFile.TextureAnimationFrameChunk == null)
                return null;
            var unreferencedFrames = mpdFile.TextureAnimationFrameChunk.UniqueTextureAnimationFrameTable
                .Where(x => x.IsReferenced == false)
                .ToArray();
            return unreferencedFrames.Select(x => $"Offset=0x{x.Address:X4}").ToArray();
#else
            if (!mpdFile.Surface.HasModel)
                return null;
            var surfaceMapTextures = mpdFile.Surface
                .GetAllTiles()
                .Where(x => x.TextureID != 0xFF)
                .Select(x => (Tile: x, Texture: texturesById[x.TextureID]))
                .GroupBy(x => x.Texture.Hash)
                .ToDictionary(x => x.Key, x => x.ToArray());

            var textures = surfaceMapTextures
                .ToDictionary(x => x.Key, x => x.Value.Select(y => y.Texture.ID).Distinct().ToArray())
                .Where(x => x.Value.Length == 1)
                .Select(x => texturesById[x.Value[0]])
                .OrderBy(x => x.ID)
                .ToArray();

            foreach (var x in textures)
                if (!s_textureHashRelationships.ContainsKey(x.Hash))
                    s_textureHashRelationships.Add(x.Hash, new HashSet<string>());

            var badResults = new HashSet<string>();
            for (int i = 0; i < textures.Length; i++) {
                var earlierHash = textures[i].Hash;
                for (int j = i + 1; j < textures.Length; j++) {
                    var laterHash = textures[j].Hash;
                    if (s_textureHashRelationships[laterHash].Contains(earlierHash))
                        badResults.Add($"Inconsistency: {laterHash} < {earlierHash}");
                    else
                        s_textureHashRelationships[earlierHash].Add(laterHash);
                }
            }

            return badResults.ToArray();
#endif
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
                            var matchReports = MPD_MatchFunc(mpdFile, scenario, filename);
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

                            //ScanForErrorsAndReport(scenario, mpdFile);
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

#if false
            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| TEXTURE ORIGINS                                 |");
            Console.WriteLine("===================================================");

            var refsByHash = s_referencedTexturesByFile
                .SelectMany(x => x.Value.Select(y => (File: x.Key, y.ID, y.Hash)))
                .GroupBy(x => x.Hash)
                .ToDictionary(x => x.Key, x => x
                    .Select(y => (y.File, y.ID))
                    .Distinct()
                    .OrderBy(x => x.File)
                    .ThenBy(x => x.ID)
                    .ToHashSet()
                );

            var texsByHash = s_texturesByFile
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

                foreach (var fileKv in s_unreferencedTexturesByFile) {
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
#endif
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
