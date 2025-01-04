using CommonLib.Arrays;
using CommonLib.NamedValues;
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
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").Order().ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine(scenario.ToString() + ": " + Path.GetFileName(file) + ":");

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetterContexts)) {
                            // Anything Scenario 1 or higher should have Chunk[20].
                            bool shouldHaveChunk20 = scenario >= ScenarioType.Scenario2;
                            bool hasChunk20 = mpdFile.ChunkHeader.Rows[20].ChunkAddress > 0;
                            if (shouldHaveChunk20 != hasChunk20)
                                Console.WriteLine("  Chunk[20] problem! ShouldHave=" + shouldHaveChunk20 + ", DoesHave=" + hasChunk20);

                            // Anything Scenario 2 or higher should have Chunk[21].
                            bool shouldHaveChunk21 = scenario >= ScenarioType.Scenario2;
                            bool hasChunk21 = mpdFile.ChunkHeader.Rows[21].ChunkAddress > 0;
                            if (shouldHaveChunk21 != hasChunk21)
                                Console.WriteLine("  Chunk[21] problem! ShouldHave=" + shouldHaveChunk21 + ", DoesHave=" + hasChunk21);

                            // Anything Scenario 3 or higher should have Palette3.
                            bool shouldHavePalette3 = scenario >= ScenarioType.Scenario3;
                            bool hasPalette3 = mpdFile.MPDHeader.Data.GetWord(mpdFile.MPDHeader.Address + 0x44) == 0x0029;
                            if (shouldHavePalette3 != hasPalette3)
                                Console.WriteLine("  Palette3 problem! ShouldHave=" + shouldHavePalette3 + ", DoesHave=" + hasPalette3);

                            // Is this MPD file in the wrong format for this scenario? (Scenario 3 and Premium Disk are the same)
                            var expectedScenario = (scenario == ScenarioType.PremiumDisk) ? ScenarioType.Scenario3 : scenario;
                            if (mpdFile.Scenario != expectedScenario)
                                Console.WriteLine("  Wrong scenario for this disc! ShouldBe=" + expectedScenario + ", Is=" + mpdFile.Scenario);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  Exception: '" + e.Message + "'. Skipping!");
                    }
                }
            }
        }
    }
}
