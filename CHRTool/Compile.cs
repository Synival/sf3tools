using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using SF3.CHR;

namespace CHRTool {
    public static class Compile {
        public static int Run(string[] args, bool verbose) {
            var optimize = false;
            string outputFile = null;
            string outputDir = null;
            var spritesToAdd = new List<string>();
            var cantSetOutputFile = false;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "O|optimize",    v => optimize = true },
                { "o|output=",     v => outputFile = v },
                { "d|output-dir=", v => outputDir = v },
                { "add-sprite=",   v => spritesToAdd.Add(v) },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            // Fetch the directory with the game data for decompile CHR/CHP files.
            string[] files;
            (files, args) = Utils.GetFilesAndPathsFromAgs(args, path => {
                cantSetOutputFile = true;
                return Directory.GetFiles(path, "*.SF3CHR")
                    .Concat(Directory.GetFiles(path, "*.SF3CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            });
            if (files.Length == 0) {
                Logger.WriteLine("No .SF3CHR or .SF3CHP file(s) or path(s) provided", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }
            else if (files.Length > 1)
                cantSetOutputFile = true;

            if (cantSetOutputFile && outputFile != null) {
                Logger.WriteLine("Cannot use '--output' pararameter with multiple files", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'compile' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            try {
                if (verbose) {
                    Logger.WriteLine("Compiling to .CHR(s) / CHP(s)...");
                    Logger.WriteLine("------------------------------------------------------------------------------");
                }

                foreach (var file in files) {
                    try {
                        CompileFile(file, outputFile, outputDir, verbose, optimize);
                    }
                    catch (Exception e) {
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                }
            }
            catch (Exception e) {
                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine("Done");
            }
            return 0;
        }

        private static void CompileFile(string inputFile, string outputFile, string outputPath, bool verbose, bool optimize) {
            var inputFilename = Path.GetFileName(inputFile);

            bool isChp = inputFilename.ToLower().EndsWith(".sf3chp");
            if (!isChp && !inputFilename.ToLower().EndsWith(".sf3chr"))
                throw new Exception($"File '{inputFilename}' is not a .SF3CHR or .SF3CHP file");

            if (outputFile == null) {
                var outputExtension = isChp ? "CHP" : "CHR";
                outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.{outputExtension}");
            }
            if (outputPath != null) {
                var outputFilenameWithExtension = Path.GetFileName(outputFile);
                outputFile = Path.Combine(outputPath ?? Path.GetDirectoryName(inputFile), outputFilenameWithExtension);
            }
            var outputFilename = Path.GetFileName(outputFile);

            Logger.WriteLine($"Compiling '{inputFile}' to '{outputFile}'...");

            // Try to create the output directory if it doesn't exist.
            outputPath = Path.GetDirectoryName(outputFile);
            if (outputPath != "" && !Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            string inputText = null;
            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine($"Reading '{inputFile}...");
            }
            inputText = File.ReadAllText(inputFile);

            byte[] outputData = null;
            if (isChp) {
                // Attempt to deserialize.
                if (verbose)
                    Logger.WriteLine("Deserializing to CHP_Def...");
                var chpDef = CHP_Def.FromJSON(inputText);
                if (chpDef == null)
                    throw new NullReferenceException(); // eh, not really, but whatever

                // We should have everything necessary to compile. Give it a go!
                if (verbose)
                    Logger.WriteLine("Compiling...");
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
                if (verbose)
                    Logger.WriteLine("Deserializing to CHR_Def...");
                CHR_Def chrDef = null;
                chrDef = CHR_Def.FromJSON(inputText);
                if (chrDef == null)
                    throw new NullReferenceException(); // eh, not really, but whatever

                // We should have everything necessary to compile. Give it a go!
                if (verbose)
                    Logger.WriteLine("Compiling...");
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
            if (verbose)
                Logger.WriteLine($"Writing to '{outputFile}'...");
            File.WriteAllBytes(outputFile, outputData);
        }
    }
}
