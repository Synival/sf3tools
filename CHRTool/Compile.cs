using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using Newtonsoft.Json.Linq;
using SF3.CHR;

namespace CHRTool {
    public static class Compile {
        public static int Run(string[] args, bool verbose) {
            var optimize = false;
            string outputFile = null;
            string outputDir = null;
            var spritesToAdd = new List<string>();
            var spritesToAddDefList = new List<SpriteDef>();
            var cantSetOutputFile = false;
            string paddingFrom = null;
            bool optimizeSectors = false;

            // Keep track of any errors that may have occurred.
            int initialErrorCount = Logger.TotalErrorCount;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "O|optimize",    v => optimize = true },
                { "o|output=",     v => outputFile = v },
                { "d|output-dir=", v => outputDir = v },
                { "add-sprite=",   v => spritesToAdd.Add(v) },
                { "padding-from=", v => paddingFrom = v },
                { "optimize-sectors", v => optimizeSectors = true },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.LogException(e);
                return 1;
            }

            // Fetch the directory with the SF3CHR/SF3CHP files to compile to CHR/CHP files.
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

            byte[] paddingBytes = null;
            if (paddingFrom != null) {
                if (!File.Exists(paddingFrom)) {
                    Logger.WriteLine($"'padding-from' file '{paddingFrom}' cannot be found", LogType.Error);
                    Logger.Write(Constants.ErrorUsageString);
                    return 1;
                }
                try {
                    paddingBytes = File.ReadAllBytes(paddingFrom);
                }
                catch (Exception e) {
                    Logger.WriteLine($"Error reading from 'padding-from' file '{paddingFrom}':", LogType.Error);
                    Logger.LogException(e);
                    return 1;
                }
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'compile' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // Fetch any sprites we want to add. Make sure they're valid JObject's.
            if (verbose && spritesToAdd.Count > 0)
                Logger.WriteLine("Loading JSON for sprites to add...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                foreach (var spriteToAdd in spritesToAdd) {
                    Console.WriteLine($"Loading sprite to add '{spriteToAdd}'...");
                    using (Logger.IndentedSection()) {
                        try {
                            var text = File.ReadAllText(spriteToAdd);
                            var jObj = JObject.Parse(text);

                            // TODO: there's probably a much better way to do this...
                            if (optimize)
                                _ = jObj.Remove("Frames");

                            if (verbose)
                                Console.WriteLine("Converting to SpriteDef...");
                            using (Logger.IndentedSection(verbose ? 1 : 0))
                                spritesToAddDefList.Add(SpriteDef.FromJToken(jObj));
                        }
                        catch (Exception e) {
                            Logger.LogException(e);
                            return 1;
                        }
                    }
                }
            }
            var spritesToAddDefs = spritesToAddDefList.ToArray();

            // Paranoia check -- don't attempt to compile anything if we've encountered errors at this point.
            if (Logger.TotalErrorCount > initialErrorCount)
                return 1;

            // It looks like we're ready to go! Fetch the file data.
            if (verbose)
                Logger.WriteLine("Compiling to .CHR(s) / CHP(s)...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                try {
                    foreach (var file in files) {
                        var thisOutputFile = GetOutputFile(file, outputFile, outputDir);
                        Logger.WriteLine($"Compiling '{file}' to '{thisOutputFile}'...");
                        using (Logger.IndentedSection()) {
                            int errorCount = Logger.TotalErrorCount;
                            try {
                                CompileFile(file, thisOutputFile, outputDir, verbose, optimize, spritesToAddDefs, paddingBytes, optimizeSectors);
                            }
                            catch (Exception e) {
                                Logger.LogException(e);
                            }

                            // If something was compiled with errors, don't continue.
                            if (Logger.TotalErrorCount > errorCount && File.Exists(thisOutputFile)) {
                                if (verbose)
                                    Logger.WriteLine("Errors detected; aborting.");
                                return 1;
                            }
                        }
                    }
                }
                catch (Exception e) {
                    Logger.LogException(e);
                    return 1;
                }
            }

            // We're not "done" if errors were produced.
            if (Logger.TotalErrorCount > initialErrorCount) {
                if (verbose)
                    Logger.WriteLine("Errors detected; aborting.");
                return 1;
            }

            if (verbose)
                Logger.WriteLine("Done");

            return 0;
        }

        private static string GetOutputFile(string inputFile, string outputFile, string outputPath) {
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

            return outputFile;
        }

        private static void CompileFile(string inputFile, string outputFile, string outputPath, bool verbose, bool optimize, SpriteDef[] spritesToAdd, byte[] paddingBytes, bool optimizeSectors) {
            bool isChp = inputFile.ToLower().EndsWith(".sf3chp");
            int initialErrorCount = Logger.TotalErrorCount;

            // Try to create the output directory if it doesn't exist.
            outputPath = Path.GetDirectoryName(outputFile);
            if (outputPath != "" && !Directory.Exists(outputPath)) {
                if (verbose)
                    Logger.WriteLine($"Creating path '{outputPath}'");
                using (Logger.IndentedSection(verbose ? 1 : 0))
                    Directory.CreateDirectory(outputPath);
            }

            string inputText = null;
            if (verbose)
                Logger.WriteLine($"Reading '{inputFile}...");
            using (Logger.IndentedSection(verbose ? 1 : 0))
                inputText = File.ReadAllText(inputFile);

            byte[] outputData = isChp
                ? CompileCHP(inputFile, inputText, verbose, optimize, spritesToAdd, paddingBytes, optimizeSectors)
                : CompileCHR(inputFile, inputText, verbose, optimize, spritesToAdd, paddingBytes);
            if (outputData == null)
                return;

            // Don't write anything on error.
            if (Logger.TotalErrorCount > initialErrorCount) {
                if (verbose)
                    Logger.WriteLine($"Errors detected; not writing to '{outputFile}'.");
                return;
            }

            // Output the file.
            if (verbose)
                Logger.WriteLine($"Writing to '{outputFile}'...");
            using (Logger.IndentedSection(verbose ? 1 : 0))
                File.WriteAllBytes(outputFile, outputData);
        }

        private static byte[] CompileCHP(string inputFile, string inputText, bool verbose, bool optimize, SpriteDef[] spritesToAdd, byte[] paddingBytes, bool optimizeSectors) {
            // Attempt to deserialize.
            CHP_Def chpDef;
            if (verbose)
                Logger.WriteLine("Deserializing to CHP_Def...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                chpDef = CHP_Def.FromJSON(inputText);
                if (chpDef == null) {
                    Logger.WriteLine($"Failure during deserialization of '{inputFile}': null returned", LogType.Error);
                    return null;
                }
            }

            // Add sprites if requested
            if (spritesToAdd?.Length >= 1) {
                // TODO: get this working for CHP files!
                Logger.WriteLine("(cannot yet add sprites to CHP files using --add-sprite)", LogType.Warning);
            }

            // We should have everything necessary to compile. Give it a go!
            if (verbose)
                Logger.WriteLine("Compiling...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                var chpCompiler = new CHP_Compiler() {
                    OptimizeFrames            = optimize,
                    AddMissingAnimationFrames = true,
                    PaddingBytes              = paddingBytes,
                    OptimizeSectors           = optimizeSectors
                };

                using (var memoryStream = new MemoryStream()) {
                    chpCompiler.Compile(chpDef, memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        private static byte[] CompileCHR(string inputFile, string inputText, bool verbose, bool optimize, SpriteDef[] spritesToAdd, byte[] paddingBytes) {
            // Attempt to deserialize.
            CHR_Def chrDef = null;
            if (verbose)
                Logger.WriteLine("Deserializing to CHR_Def...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                chrDef = CHR_Def.FromJSON(inputText);
                if (chrDef == null) {
                    Logger.WriteLine($"Failure during deserialization of '{inputFile}': null returned", LogType.Error);
                    return null;
                }
            }

            // Add sprites if requested
            if (spritesToAdd?.Length >= 1)
                chrDef.Sprites = chrDef.Sprites.Concat(spritesToAdd).ToArray();

            // We should have everything necessary to compile. Give it a go!
            if (verbose)
                Logger.WriteLine("Compiling...");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                var chrCompiler = new CHR_Compiler() {
                    OptimizeFrames            = optimize,
                    AddMissingAnimationFrames = true,
                    PaddingBytes              = paddingBytes,
                };

                using (var memoryStream = new MemoryStream()) {
                    chrCompiler.Compile(chrDef, memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
