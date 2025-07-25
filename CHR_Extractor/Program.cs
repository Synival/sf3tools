using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Files;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;
using SF3.Models.Tables;
using SF3.NamedValues;
using SF3.Types;

namespace CHR_Extractor {
    public class Program {
        // ,--- Enter the paths for all your CHR files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        private static readonly Dictionary<ScenarioType, string> c_scenarioPaths = new() {
            { ScenarioType.Scenario1,   "S1" },
            { ScenarioType.Scenario2,   "S2" },
            { ScenarioType.Scenario3,   "S3" },
            { ScenarioType.PremiumDisk, "PD" },
        };

        private const string c_pathOut = "../../../../SF3Lib/Resources";

        public static void Main(string[] args) {
            Console.WriteLine("Processing all CHR and CHP files...");

            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => {
                    var files = Directory.GetFiles(c_pathsIn[x], "*.CHR").Order().ToList();
                    files.AddRange(Directory.GetFiles(c_pathsIn[x], "*.CHP").Order().ToList());
                    return files;
                });

            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // TODO: remove these when analysis is complete.
            var serializedFilesByScenarioAndFilename = new Dictionary<ScenarioType, Dictionary<string, object>>();
            var totalFileCount = 0;
            var totalAccurateFileCount = 0;

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;

                var scenarioCHR_Path = Path.Combine(c_pathOut, c_scenarioPaths[scenario], "CHRs");
                var scenarioCHP_Path = Path.Combine(c_pathOut, c_scenarioPaths[scenario], "CHPs");

                _ = Directory.CreateDirectory(scenarioCHR_Path);
                _ = Directory.CreateDirectory(scenarioCHP_Path);

                // TODO: remove me when analysis is complete.
                var nameGetter = nameGetterContexts[scenario];

                serializedFilesByScenarioAndFilename.Add(scenario, []);

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create a CHR/CHP file that works with our new ByteData.
                    try {
                        bool isChr = file.EndsWith(".CHR");
                        using (ScenarioTableFile chrChpFile = isChr
                            ? CHR_File.Create(byteData, nameGetterContexts[scenario], scenario)
                            : CHP_File.Create(byteData, nameGetterContexts[scenario], scenario)
                        ) {
                            var chrFiles = isChr
                                ? [(CHR_File) chrChpFile]
                                : ((CHP_File) chrChpFile).CHR_EntriesByOffset.Values.ToArray();

                            // List the file we're serializing.
                            var fileStr = GetFileString(scenario, file, chrChpFile);
                            Console.WriteLine($"{fileStr}");

                            // Serialize the file. Format depends on CHR vs CHP file.
                            var chrChpDef = isChr
                                ? (object) ((CHR_File) chrChpFile).ToCHR_Def()
                                : (object) ((CHP_File) chrChpFile).ToCHP_Def();

                            var serializedDef = isChr
                                ? ((CHR_Def) chrChpDef).ToJSON_String()
                                : ((CHP_Def) chrChpDef).ToJSON_String();

                            // Write either a .SF3CHR or .SF3CHP file out.
                            var spriteDefPath = Path.Combine(isChr ? scenarioCHR_Path : scenarioCHP_Path, filename + (isChr ? ".SF3CHR" : ".SF3CHP"));
                            using (var outFile = File.Open(spriteDefPath, FileMode.Create)) {
                                using (var stream = new StreamWriter(outFile)) {
                                    stream.NewLine = "\n";
                                    stream.Write(serializedDef);
                                }
                            }

                            // TODO: remove me when analysis is complete.
                            serializedFilesByScenarioAndFilename[scenario].Add(Path.GetFileName(file), chrChpDef);

                            // Deserialize the file to confirm its integrity.
                            byte[]? chpBuf = null;
                            if (!isChr) {
                                var origChp = (CHP_File) chrChpFile;
                                chpBuf = origChp.Data.GetDataCopy();
                            }
                            var deserializedFile = isChr
                                ? (object) ((CHR_Def) chrChpDef).ToCHR_File(nameGetter, scenario)
                                : (object) ((CHP_Def) chrChpDef).ToCHP_File(nameGetter, scenario, chpBuf);

                            bool fileIsDifferent = false;

                            var newData = isChr
                                ? ((CHR_File) deserializedFile).Data.Data.GetDataCopyOrReference()
                                : ((CHP_File) deserializedFile).Data.Data.GetDataCopyOrReference();

                            var origData = isChr
                                ? ((CHR_File) chrChpFile).Data.Data.GetDataCopyOrReference()
                                : ((CHP_File) chrChpFile).Data.Data.GetDataCopyOrReference();
                            if (newData.Length != origData.Length) {
                                Console.WriteLine($"    Size changed: 0x{origData.Length:X5} => 0x{newData.Length:X5}");
                                fileIsDifferent = true;
                            }

                            var len = Math.Min(origData.Length, newData.Length);
                            for (int i = 0; i < len; i++) {
                                if (origData[i] != newData[i]) {
                                    Console.WriteLine($"    Data differs at 0x{i:X5}");
                                    fileIsDifferent = true;
                                    break;
                                }
                            }

                            totalFileCount++;
                            if (fileIsDifferent) {
                                var origChrs = isChr
                                    ? [(CHR_File) deserializedFile]
                                    : ((CHP_File) deserializedFile).CHR_EntriesByOffset.Values.ToArray();
                                var newChrs = isChr
                                    ? [(CHR_File) chrChpFile]
                                    : ((CHP_File) chrChpFile).CHR_EntriesByOffset.Values.ToArray();

                                if (origChrs.Length != newChrs.Length)
                                    Console.WriteLine($"    Wrong number of CHRs: {origChrs.Length} => {newChrs.Length}");
                                else {
                                    for (int i = 0; i < origChrs.Length; i++) {
                                        var origChr = origChrs[i];
                                        var newChr  = newChrs[i];

                                        var origSprites = origChr.SpriteTable.ToArray();
                                        var newSprites  = newChr.SpriteTable.ToArray();

                                        if (origSprites.Length != newSprites.Length)
                                            Console.WriteLine($"    [{i:X2}]: Wrong number of sprites: {origSprites.Length} => {newSprites.Length}");
                                        else {
                                            for (int j = 0; j < origSprites.Length; j++) {
                                                var origSprite = origSprites[j];
                                                var newSprite  = newSprites[j];

                                                ITable[] GetTables(Sprite sprite) {
                                                    var tables = new List<ITable>();
                                                    tables.AddRange(sprite.AnimationFrameTablesByIndex.Select(x => x.Value).ToList());
                                                    tables.Add(sprite.AnimationOffsetTable);
                                                    tables.Add(sprite.FrameTable);
                                                    return tables.ToArray();
                                                }

                                                var origTables = GetTables(origSprite);
                                                var newTables  = GetTables(newSprite);

                                                if (origTables.Length != newTables.Length)
                                                    Console.WriteLine($"    [{i:X2}, {origSprite.SpriteName}]: Wrong number of CHR tables: {origTables.Length} => {newTables.Length}");
                                                else {
                                                    for (int k = 0; k < origTables.Length; k++) {
                                                        var origTable = origTables[k];
                                                        var newTable  = newTables[k];

                                                        if (origTable.Length != newTable.Length)
                                                            Console.WriteLine($"    [{i:X2}, {origSprite.SpriteName}, {origTable.Name}]: Wrong number of elements: {origTable.Length} => {newTable.Length}");

                                                        var origChrData = origTable.Data.GetDataCopyAt(origTable.Address, origTable.SizeInBytesPlusTerminator);
                                                        var newChrData  = newTable .Data.GetDataCopyAt(newTable.Address,  newTable.SizeInBytesPlusTerminator);

                                                        if (!Enumerable.SequenceEqual(origChrData, newChrData))
                                                            Console.WriteLine($"    [{i:X2}, {origSprite.SpriteName}, {origTable.Name}]: Table has different data");
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (fileIsDifferent) {
                                var pathOut = Path.Combine(c_pathOut, "Rebuilt_CHRs", c_scenarioPaths[scenario]);
                                Directory.CreateDirectory(pathOut);
                                pathOut = Path.Combine(pathOut, $"{filename}.{(isChr ? "CHR" : "CHP")}");
                                using (var newDataOut = File.Open(pathOut, FileMode.Create))
                                    newDataOut.Write(newData, 0, newData.Length);
                            }
                            else
                                totalAccurateFileCount++;
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            Console.WriteLine("Processing complete.");
            Console.WriteLine($"Accuracy rate by file: {totalAccurateFileCount * 100f / totalFileCount}%");
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ScenarioTableFile chrChpFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }
    }
}
