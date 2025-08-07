using System;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace CHRTool {
    public static class ExtractSheets {
        public static int Run(string[] args, string spriteDir, string spritesheetDir, string hashLookupDir) {
            // (any extra options would go here.)

            // Fetch the directory with the game data for ripping spritesheets.
            if (args.Length == 0) {
                Console.Error.WriteLine("Missing game data directory");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }
            var gameDataDir = args[0];
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'extract-sheet' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine($"Extracting spritesheet frames from path '{gameDataDir}'...");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:      {spriteDir}");
            Console.WriteLine($"Spritesheet directory: {spritesheetDir}");
            Console.WriteLine($"Hash lookup directory: {hashLookupDir}");
            Console.WriteLine("------------------------------------------------------------------------------");

            string[] files;
            try {
                files = Directory.GetFiles(gameDataDir, "*.CHR")
                    .Concat(Directory.GetFiles(gameDataDir, "*.CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't get game data files from path '{gameDataDir}':");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // We don't care about the NameGetterContext or Scenario, since CHRs/CHP are all the same format,
            // and we don't care about any scenario-based resources. Just use Scenario 1.
            var scenario = ScenarioType.Scenario1;
            var nameGetterContext = new NameGetterContext(scenario);

            foreach (var file in files) {
                Console.WriteLine($"Extracting sprites from '{Path.GetFileName(file)}'...");
                try {
                    var bytes = File.ReadAllBytes(file);
                    var byteData = new ByteData(new ByteArray(bytes));

                    if (file.ToLower().EndsWith(".CHR")) {
                        var chrFile = CHR_File.Create(byteData, nameGetterContext, scenario);
                        ExtractFromCHR(chrFile, spritesheetDir);
                    }
                    else if (file.ToLower().EndsWith(".CHP")) {
                        var chpFile = CHP_File.Create(byteData, nameGetterContext, scenario);
                        foreach (var chrFile in chpFile.CHR_EntriesByOffset.Values)
                            ExtractFromCHR(chrFile, spritesheetDir);
                    }
                }
                catch (Exception e) {
                    Console.Error.WriteLine($"  Couldn't extract sheets from '{file}':");
                    Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                }
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Done");
            return 0;
        }

        private static void ExtractFromCHR(ICHR_File chrFile, string spritesheetDir) {
            // TODO: extract them sprites!
        }
    }
}
