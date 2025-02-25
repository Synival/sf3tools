using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.X1;
using SF3.NamedValues;
using SF3.Types;

namespace X1_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your X1 files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        /// <summary>
        /// Check for matching X1 files for certain conditions.
        /// </summary>
        /// <param name="x1File"></param>
        /// <returns></returns>
        private static bool? X1_Match_Func(IX1_File x1File) {
            // Sample: Skip non-battles, and match non-battles with scripted movements.

            // Return 'null' to skip.
            if (!x1File.IsBattle)
                return null;

            // Return 'true' if any battles in the battle table have at least 1 scripted movements.
            if (x1File.Battles.Values.Any(x => x.BattleHeader.NumScriptedMovements > 0))
                return true;

            // Not a match.
            return false;
        }

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "X1*.BIN").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            var matchSet   = new List<string>();
            var nomatchSet = new List<string>();

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        var isBTL99 = filename == "X1BTL99";
                        using (var x1File = X1_File.Create(byteData, nameGetterContexts[scenario], scenario, isBTL99)) {
                            // Condition for match checks here
                            var match = X1_Match_Func(x1File);
                            if (match == null)
                                continue;

                            var fileStr = GetFileString(scenario, file, x1File);
                            Console.Write(fileStr + " | " + (match == true ? "Match  " : "NoMatch"));

                            Console.WriteLine();

                            if (match == true)
                                matchSet.Add(fileStr);
                            else
                                nomatchSet.Add(fileStr);

                            ScanForErrorsAndReport(scenario, x1File);
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

        private static string BitString(ushort bits) {
            var str = "";
            for (var i = 0; i < 16; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, IX1_File x1File) {
            var typeStr = (x1File.IsBattle ? "Battle: " : "Town") + string.Join(", ", x1File.Battles?.Select(x => x.Key.ToString()) ?? [""]);
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12)
                + " | " + typeStr.PadRight(22)
                ;
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, IX1_File mpdFile) {
            var totalErrors = new List<string>();

            // TODO: scan for errors

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
