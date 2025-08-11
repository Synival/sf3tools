using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;
using SF3.CHR;

namespace CHRTool {
    public static class Compile {
        public static int Run(string[] args) {
            var optimize = false;
            string outputFile = null;
            List<string> spritesToAdd = new List<string>();

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "O|optimize",  v => optimize = true },
                { "output=",     v => outputFile = v },
                { "add-sprite=", v => spritesToAdd.Add(v) },
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
                Console.Error.WriteLine("Unrecognized arguments in 'compile' command:");
                Console.Error.WriteLine($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            try {
                var inputFilename = Path.GetFileName(inputFile);
                if (outputFile == null)
                    outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.CHR");
                var outputFilename = Path.GetFileName(outputFile);

                Console.WriteLine($"Compiling '{inputFilename}' to '{outputFilename}'...");
                Console.WriteLine("------------------------------------------------------------------------------");

                string chrDefText = null;
                Console.WriteLine($"Reading '{inputFile}...");
                chrDefText = File.ReadAllText(inputFile);

                // Attempt to deserialize.
                Console.WriteLine("Deserializing to CHR_Def...");
                CHR_Def chrDef = null;
                chrDef = CHR_Def.FromJSON(chrDefText);
                if (chrDef == null)
                    throw new NullReferenceException(); // eh, not really, but whatever

                // We should have everything necessary to compile. Give it a go!
                Console.WriteLine("Compiling...");
                var chrCompiler = new CHR_Compiler() {
                    OptimizeFrames            = optimize,
                    AddMissingAnimationFrames = true,
                };

                byte[] chrFileData = null;
                using (var memoryStream = new MemoryStream()) {
                    chrCompiler.Compile(chrDef, memoryStream);
                    chrFileData = memoryStream.ToArray();
                }

                // Output the file.
                Console.WriteLine($"Writing to '{outputFile}'...");
                File.WriteAllBytes(outputFile, chrFileData);
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
