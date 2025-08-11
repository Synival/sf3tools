using System;
using System.IO;
using CommonLib.Arrays;
using NDesk.Options;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace CHRTool {
    public static class Decompile {
        public static int Run(string[] args) {
            bool optimize = false;
            string outputFile = null;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "O|optimize",  v => optimize = true },
                { "output=",     v => outputFile = v },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // Fetch the filename for reading.
            if (args.Length == 0) {
                Console.Error.WriteLine("Missing input file");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }
            var inputFile = args[0];
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'decompile' command:");
                Console.Error.WriteLine($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            try {
                var inputFilename = Path.GetFileName(inputFile);
                if (outputFile == null)
                    outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.SF3CHR");
                var outputFilename = Path.GetFileName(outputFile);

                Console.WriteLine($"Decompiling '{inputFilename}' to '{outputFilename}'...");
                Console.WriteLine("------------------------------------------------------------------------------");

                // Fetch the data.
                Console.WriteLine($"Loading data from '{inputFile}...");
                byte[] chrBytes = null;
                chrBytes = File.ReadAllBytes(inputFile);

                // Attempt to load it as a CHR_File.
                Console.WriteLine("Creating CHR_File...");
                CHR_File chrFile = null;
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                chrFile = CHR_File.Create(new ByteData(new ByteArray(chrBytes)), nameGetterContext, ScenarioType.Scenario1);

                // Get a CHR_Def from the CHR_File.
                Console.WriteLine("Serializing to CHR_Def...");
                CHR_Def chrDef = null;
                chrDef = chrFile.ToCHR_Def();
                if (optimize) {
                    foreach (var sprite in chrDef.Sprites)
                        sprite.FrameGroupsForSpritesheets = null;
                }

                // Serialize the file.
                Console.WriteLine($"Converting to JSON file...");
                var chrDefText = chrDef.ToJSON_String();

                Console.WriteLine($"Writing to '{outputFile}'...");
                File.WriteAllText(outputFile, chrDefText);
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"Error:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Done");
            return 0;
        }
    }
}
