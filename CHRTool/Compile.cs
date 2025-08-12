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

                bool isChp = inputFilename.ToLower().EndsWith(".sf3chp");
                if (!isChp && !inputFilename.ToLower().EndsWith(".sf3chr"))
                    throw new Exception($"File '{inputFilename}' is not a .SF3CHR or .SF3CHP file");

                if (outputFile == null) {
                    var outputExtension = isChp ? "CHP" : "CHR";
                    outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.{outputExtension}");
                }
                var outputFilename = Path.GetFileName(outputFile);

                Console.WriteLine($"Compiling '{inputFilename}' to '{outputFilename}'...");
                Console.WriteLine("------------------------------------------------------------------------------");

                string inputText = null;
                Console.WriteLine($"Reading '{inputFile}...");
                inputText = File.ReadAllText(inputFile);

                byte[] outputData = null;
                if (isChp) {
                    // Attempt to deserialize.
                    Console.WriteLine("Deserializing to CHP_Def...");
                    var chpDef = CHP_Def.FromJSON(inputText);
                    if (chpDef == null)
                        throw new NullReferenceException(); // eh, not really, but whatever

                    // We should have everything necessary to compile. Give it a go!
                    Console.WriteLine("Compiling...");
                    var chpCompiler = new CHP_Compiler() {
                        OptimizeFrames            = optimize,
                        AddMissingAnimationFrames = true,
                    };

                    using (var memoryStream = new MemoryStream()) {
                        chpCompiler.Compile(chpDef, memoryStream);
                        outputData = memoryStream.ToArray();
                    }
                }
                else {
                    // Attempt to deserialize.
                    Console.WriteLine("Deserializing to CHR_Def...");
                    CHR_Def chrDef = null;
                    chrDef = CHR_Def.FromJSON(inputText);
                    if (chrDef == null)
                        throw new NullReferenceException(); // eh, not really, but whatever

                    // We should have everything necessary to compile. Give it a go!
                    Console.WriteLine("Compiling...");
                    var chrCompiler = new CHR_Compiler() {
                        OptimizeFrames            = optimize,
                        AddMissingAnimationFrames = true,
                    };

                    using (var memoryStream = new MemoryStream()) {
                        chrCompiler.Compile(chrDef, memoryStream);
                        outputData = memoryStream.ToArray();
                    }
                }

                // Output the file.
                Console.WriteLine($"Writing to '{outputFile}'...");
                File.WriteAllBytes(outputFile, outputData);
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
