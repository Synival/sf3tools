using System.Collections.Concurrent;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Files.X8PC;
using SF3.NamedValues;
using SF3.Types;

namespace X8PC_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your X8PC files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "C:/SF3/Scenario1" },
            { ScenarioType.Scenario2,   "C:/SF3/Scenario2" },
            { ScenarioType.Scenario3,   "C:/SF3/Scenario3" },
            { ScenarioType.PremiumDisk, "C:/SF3/PremiumDisk" },
        };

        /// <summary>
        /// Check for matching X8PC files for certain conditions.
        /// </summary>
        /// <param name="x1File"></param>
        /// <returns>'null' if this file should be skipped, otherwise a list of results/reports that, if a match was found, will be non-empty.
        private static string[]? X8PC_Match_Func(string filename, IX8PC_File x8pcFile) {
            return MatchFuncs.HasUnassociatedXPData(x8pcFile);
        }

        private static int s_logIndex = 0;
        private static Dictionary<int, List<string>> s_logQueue = [];
        private static HashSet<int> s_logsDone = [];

        public static void LogAtIndex(int index, string log) {
            if (s_logIndex == index) {
                Console.WriteLine(log);
                return;
            }

            if (!s_logQueue.ContainsKey(index))
                s_logQueue[index] = [log];
            else
                s_logQueue[index].Add(log);
        }

        public static void NextLogIndex() {
            s_logIndex++;
            if (s_logQueue.TryGetValue(s_logIndex, out var logs)) {
                foreach (var log in logs)
                    Console.WriteLine(log);
                s_logQueue.Remove(s_logIndex);
            }
        }

        public static void DoneWithLogIndex(int index) {
            s_logsDone.Add(index);
            while (s_logsDone.Contains(s_logIndex))
                NextLogIndex();
        }

        public static void ResetLogging() {
            s_logIndex = 0;
            s_logQueue.Clear();
            s_logsDone.Clear();
        }

        public static void Main(string[] args) {
            Console.WriteLine("Press a key to start...");
            _ = Console.ReadKey();

            // Get a list of all X8PC*.BIN files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "X8PC*.BIN").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            var matchSet   = new List<string>();
            var nomatchSet = new List<string>();

            var mutex = new Mutex();

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                var parallelOptions = new ParallelOptions() {
                    MaxDegreeOfParallelism = -1
                };

                var filenamesWithIndex = filesKv.Value.Select((x, i) => (File: x, Index: i)).ToArray();

                ResetLogging();
                Parallel.ForEach(Partitioner.Create(filenamesWithIndex), parallelOptions, fileWithIndex => {
                    var file = fileWithIndex.File;
                    int fileIndex = fileWithIndex.Index;
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Skip the really tiny stubs -- there are TONS of these.
                    if (byteData.Length < 10)
                        return;

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var x8pcFile = X8PC_File.Create(byteData, nameGetterContexts[scenario], scenario)) {
                            var matchReports = X8PC_Match_Func(filename, x8pcFile);

                            // If the match is 'null', that means we're just skipping this file completely.
                            if (matchReports == null) {
                                DoneWithLogIndex(fileIndex);
                                return;
                            }

                            mutex.WaitOne();
                            try {
                                // List the file and any report we may have from X8PC_Match_Func().
                                var fileStr = GetFileString(scenario, file, x8pcFile);
                                LogAtIndex(fileIndex, fileStr + " | ");
                                foreach (var mr in matchReports)
                                    LogAtIndex(fileIndex, "    " + mr);

                                if (matchReports.Length > 0)
                                    matchSet.Add(fileStr);
                                else
                                    nomatchSet.Add(fileStr);
                            }
                            finally {
                                DoneWithLogIndex(fileIndex);
                                mutex.ReleaseMutex();
                            }

                            ScanForErrorsAndReport(scenario, x8pcFile);
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
        }

        private static string BitString(uint bits) {
            var str = "";
            for (var i = 0; i < 32; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000_0000 >> i)) != 0 ? "1" : "0";
            }
            return str;
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

        private static string GetFileString(ScenarioType inputScenario, string filename, IX8PC_File x8pcFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, IX8PC_File mpdFile) {
            var totalErrors = new List<string>();

            // TODO: scan for errors

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
