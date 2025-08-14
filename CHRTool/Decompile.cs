using System;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace CHRTool {
    public static class Decompile {
        public static int Run(string[] args, bool verbose) {
            var simplify = false;
            string outputFile = null;
            string outputDir = null;
            var cantSetOutputFile = false;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "S|simplify",    v => simplify = true },
                { "o|output=",     v => outputFile = v },
                { "d|output-dir=", v => outputDir = v },
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
                return Directory.GetFiles(path, "*.CHR")
                    .Concat(Directory.GetFiles(path, "*.CHP"))
                    .OrderBy(x => x)
                    .ToArray();
            });
            if (files.Length == 0) {
                Logger.WriteLine("No .CHR or .CHP file(s) or path(s) provided", LogType.Error);
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
                Logger.WriteLine("Unrecognized arguments in 'decompile' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            try {
                if (verbose) {
                    Logger.WriteLine("Decompiling to .SF3CHR(s) / SF3CHP(s)...");
                    Logger.WriteLine("------------------------------------------------------------------------------");
                }

                foreach (var file in files) {
                    try {
                        DecompileFile(file, outputFile, outputDir, verbose, simplify);
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

        private static void DecompileFile(string inputFile, string outputFile, string outputPath, bool verbose, bool simplify) {
            var inputFilename = Path.GetFileName(inputFile);

            bool isChp = inputFilename.ToLower().EndsWith(".chp");
            if (!isChp && !inputFilename.ToLower().EndsWith(".chr"))
                throw new Exception($"File '{inputFilename}' is not a .CHR or .CHP file");

            if (outputFile == null) {
                var outputExtension = isChp ? "SF3CHP" : "SF3CHR";
                outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.{outputExtension}");
            }
            if (outputPath != null) {
                var outputFilenameWithExtension = Path.GetFileName(outputFile);
                outputFile = Path.Combine(outputPath ?? Path.GetDirectoryName(inputFile), outputFilenameWithExtension);
            }

            Logger.WriteLine($"Decompiling '{inputFile}' to '{outputFile}'...");

            // Try to create the output directory if it doesn't exist.
            outputPath = Path.GetDirectoryName(outputFile);
            if (outputPath != "" && !Directory.Exists(outputPath)) {
                if (verbose)
                    Logger.WriteLine($"Creating path '{outputPath}'");
                Directory.CreateDirectory(outputPath);
            }

            // Fetch the data.
            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine($"Loading data from '{inputFile}...");
            }

            byte[] inputBytes = null;
            inputBytes = File.ReadAllBytes(inputFile);

            string outputText = null;
            if (isChp) {
                // Attempt to load it as a CHP_File.
                if (verbose)
                    Logger.WriteLine("Creating CHP_File...");
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                var chpFile = CHP_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                // Get a CHR_Def from the CHR_File.
                if (verbose)
                    Logger.WriteLine("Serializing to CHP_Def...");
                var chpDef = chpFile.ToCHP_Def();
                if (simplify) {
                    foreach (var chrDef in chpDef.CHRsBySector.Values)
                        foreach (var sprite in chrDef.Sprites)
                            sprite.FrameGroupsForSpritesheets = null;
                }

                // Serialize the file.
                if (verbose)
                    Logger.WriteLine($"Converting to JSON file...");
                outputText = chpDef.ToJSON_String();
            }
            else {
                // Attempt to load it as a CHR_File.
                if (verbose)
                    Logger.WriteLine("Creating CHR_File...");
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                var chrFile = CHR_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                // Get a CHR_Def from the CHR_File.
                if (verbose)
                    Logger.WriteLine("Serializing to CHR_Def...");
                var chrDef = chrFile.ToCHR_Def();
                if (simplify) {
                    foreach (var sprite in chrDef.Sprites)
                        sprite.FrameGroupsForSpritesheets = null;
                }

                // Serialize the file.
                if (verbose)
                    Logger.WriteLine($"Converting to JSON file...");
                outputText = chrDef.ToJSON_String();
            }

            if (verbose)
                Logger.WriteLine($"Writing to '{outputFile}'...");
            File.WriteAllText(outputFile, outputText);
        }

    }
}
