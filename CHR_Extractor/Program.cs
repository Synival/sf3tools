using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Files;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
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

            // TODO: remove me when analysis is complete.
            var serializedFilesByScenarioAndFilename = new Dictionary<ScenarioType, Dictionary<string, object>>();

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
                            var deserializedFile = isChr
                                ? (object) ((CHR_Def) chrChpDef).ToCHR_File(nameGetter, scenario)
                                : (object) ((CHP_Def) chrChpDef).ToCHP_File();

                            // TODO: do this for CHPs as well
                            // Dump CHRs for opening/analysis
                            if (isChr) {
                                var newChrFile = (CHR_File) deserializedFile;
                                var pathOut = Path.Combine(c_pathOut, "Rebuilt_CHRs", c_scenarioPaths[scenario]);
                                Directory.CreateDirectory(pathOut);
                                pathOut = Path.Combine(pathOut, $"{filename}.CHR");
                                var newData = newChrFile.Data.Data.GetDataCopyOrReference();
                                using (var chrOut = File.Open(pathOut, FileMode.Create)) {
                                    chrOut.Write(newData, 0, newData.Length);
                                }

                                var origChrFile = (CHR_File) chrChpFile;
                                var origData = origChrFile.Data.Data.GetDataCopyOrReference();
                                if (newData.Length != origData.Length)
                                    Console.WriteLine($"    {origData.Length:X5} => {newData.Length:X5}");
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            Console.WriteLine("Processing complete.");
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ScenarioTableFile chrChpFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }
    }
}
