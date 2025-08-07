using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;
using SF3.CHR;

namespace CHRTool {
    public static class Compile {
        public static int Run(string[] args, string spriteDir, string spritesheetDir) {
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
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            Console.WriteLine($"Sprite directory:     {spriteDir}");
            Console.WriteLine($"Spriteheet directory: {spritesheetDir}");

            var inputFilename = Path.GetFileName(inputFile);
            if (outputFile == null)
                outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.CHR");
            var outputFilename = Path.GetFileName(outputFile);
            Console.WriteLine($"Compiling '{inputFilename}' to '{outputFilename}'...");

            string chrDefText = null;
            try {
                chrDefText = File.ReadAllText(inputFile);
                Console.WriteLine("  Read input file successfully.");
            }
            catch (Exception e) {
                Console.WriteLine($"  Couldn't open '{inputFile}' for reading:");
                Console.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Attempt to deserialize.
            CHR_Def chrDef = null;
            try {
                chrDef = CHR_Def.FromJSON(chrDefText);
                if (chrDef == null)
                    throw new NullReferenceException(); // eh, not really, but whatever
                Console.WriteLine("  Deserialized CHR_Def successfully.");
            }
            catch (Exception e) {
                Console.WriteLine($"  Couldn't deserialize '{inputFile}' after reading:");
                Console.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // We should have everything necessary to compile. Give it a go!
            var chrCompiler = new CHR_Compiler() {
                OptimizeFrames            = optimize,
                AddMissingAnimationFrames = true,
                SpritePath                = spriteDir,
                SpritesheetPath           = spritesheetDir
            };
            byte[] chrFileData = null;
            try {
                using (var memoryStream = new MemoryStream()) {
                    chrCompiler.Compile(chrDef, memoryStream);
                    chrFileData = memoryStream.ToArray();
                }
                Console.WriteLine("  CHR compiled successfully.");
            }
            catch (Exception e) {
                Console.WriteLine($"  Couldn't compile '{inputFile}' after deserializing:");
                Console.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Output the file.
            try {
                File.WriteAllBytes(outputFile, chrFileData);
                Console.WriteLine("  Output file written successfully.");
            }
            catch (Exception e) {
                Console.WriteLine($"  Couldn't compile '{inputFile}' after deserializing:");
                Console.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            Console.WriteLine("Done");
            return 0;
        }
    }
}
