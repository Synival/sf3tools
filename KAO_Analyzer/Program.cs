using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.KAO;
using SF3.Models.Structs.KAO;
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

        private static string[]? KAO_MatchFunc(FaceChunk face, string filename) {
            var results = new List<string>();
            if (face.Header.Layer1Width  % 2 == 1) results.Add("Layer1Width is odd");
            if (face.Header.Layer1Height % 2 == 1) results.Add("Layer1Height is odd");
            if (face.Header.Layer2Width  % 2 == 1) results.Add("Layer2Width is odd");
            if (face.Header.Layer2Height % 2 == 1) results.Add("Layer2Height is odd");
            return results.Count > 0 ? results.ToArray() : null;
        }

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "KAO*.DAT").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Sets of matching/unmatching KAO files.
            var matchSet   = new List<string>();
            var nomatchSet = new List<string>();

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
                        using (var kaoFile = KAO_File.Create(byteData, nameGetter, scenario)) {
                            foreach (var face in kaoFile.FaceChunkTable) {
                                // Condition for match checks here
                                var matchReports = KAO_MatchFunc(face, filename);
                                if (matchReports == null)
                                    continue;

                                bool match = matchReports.Length > 0;
                                var fileStr = GetFileString(scenario, file, kaoFile, face);
                                Console.WriteLine(fileStr + " | " + (match ? "Match  " : "NoMatch"));
                                if (matchReports.Length > 0) {
                                    foreach (var r in matchReports)
                                        Console.WriteLine("    " + filename.PadLeft(8) + " | " + r);
                                    Console.WriteLine();
                                }

                                if (match)
                                    matchSet.Add(fileStr);
                                else
                                    nomatchSet.Add(fileStr);
                            }
                            ScanForErrorsAndReport(scenario, kaoFile);
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
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, KAO_File kaoFile, FaceChunk face) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12) + "[" + face.ID + "]";
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, KAO_File kaoFile) {
            var totalErrors = new List<string>();
            totalErrors.AddRange(kaoFile.GetErrors());
            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
