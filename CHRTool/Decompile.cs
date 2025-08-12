using System;
using System.IO;
using CommonLib.Arrays;
using NDesk.Options;
using SF3.ByteData;
using SF3.Models.Files.CHP;
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

                bool isChp = inputFilename.ToLower().EndsWith(".chp");
                if (!isChp && !inputFilename.ToLower().EndsWith(".chr"))
                    throw new Exception($"File '{inputFilename}' is not a .CHR or .CHP file");

                if (outputFile == null) {
                    var outputExtension = isChp ? "SF3CHP" : "SF3CHR";
                    outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.{outputExtension}");
                }
                var outputFilename = Path.GetFileName(outputFile);

                Console.WriteLine($"Decompiling '{inputFilename}' to '{outputFilename}'...");
                Console.WriteLine("------------------------------------------------------------------------------");

                // Fetch the data.
                Console.WriteLine($"Loading data from '{inputFile}...");
                byte[] inputBytes = null;
                inputBytes = File.ReadAllBytes(inputFile);

                string outputText = null;
                if (isChp) {
                    // Attempt to load it as a CHP_File.
                    Console.WriteLine("Creating CHP_File...");
                    var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                    var chpFile = CHP_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                    // Get a CHR_Def from the CHR_File.
                    Console.WriteLine("Serializing to CHP_Def...");
                    var chpDef = chpFile.ToCHP_Def();
                    if (optimize) {
                        foreach (var chrDef in chpDef.CHRsBySector.Values)
                            foreach (var sprite in chrDef.Sprites)
                                sprite.FrameGroupsForSpritesheets = null;
                    }

                    // Serialize the file.
                    Console.WriteLine($"Converting to JSON file...");
                    outputText = chpDef.ToJSON_String();
                }
                else {
                    // Attempt to load it as a CHR_File.
                    Console.WriteLine("Creating CHR_File...");
                    var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                    var chrFile = CHR_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                    // Get a CHR_Def from the CHR_File.
                    Console.WriteLine("Serializing to CHR_Def...");
                    var chrDef = chrFile.ToCHR_Def();
                    if (optimize) {
                        foreach (var sprite in chrDef.Sprites)
                            sprite.FrameGroupsForSpritesheets = null;
                    }

                    // Serialize the file.
                    Console.WriteLine($"Converting to JSON file...");
                    outputText = chrDef.ToJSON_String();
                }

                Console.WriteLine($"Writing to '{outputFile}'...");
                File.WriteAllText(outputFile, outputText);
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
