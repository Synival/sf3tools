using System;
using System.Diagnostics;
using System.IO;
using CommonLib.Arrays;
using CommonLib.Extensions;
using NDesk.Options;
using SF3.ByteData;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Types;

namespace CHRTool {
    public static class Decompile {
        public static int Run(string[] args, bool verbose) {
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
                Trace.TraceError("Error: " + e.Message);
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }

            // Fetch the filename for reading.
            if (args.Length == 0) {
                Trace.TraceError("Missing input file");
                Trace.Write(Constants.ErrorUsageString);
                return 1;
            }
            var inputFile = args[0];
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Trace.TraceError("Unrecognized arguments in 'decompile' command:");
                Trace.TraceError($"    {string.Join(" ", args)}");
                Trace.Write(Constants.ErrorUsageString);
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

                Trace.WriteLine($"Decompiling '{inputFilename}' to '{outputFilename}'...");

                // Fetch the data.
                if (verbose) {
                    Trace.WriteLine("------------------------------------------------------------------------------");
                    Trace.WriteLine($"Loading data from '{inputFile}...");
                }

                byte[] inputBytes = null;
                inputBytes = File.ReadAllBytes(inputFile);

                string outputText = null;
                if (isChp) {
                    // Attempt to load it as a CHP_File.
                    if (verbose)
                        Trace.WriteLine("Creating CHP_File...");
                    var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                    var chpFile = CHP_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                    // Get a CHR_Def from the CHR_File.
                    if (verbose)
                        Trace.WriteLine("Serializing to CHP_Def...");
                    var chpDef = chpFile.ToCHP_Def();
                    if (optimize) {
                        foreach (var chrDef in chpDef.CHRsBySector.Values)
                            foreach (var sprite in chrDef.Sprites)
                                sprite.FrameGroupsForSpritesheets = null;
                    }

                    // Serialize the file.
                    if (verbose)
                        Trace.WriteLine($"Converting to JSON file...");
                    outputText = chpDef.ToJSON_String();
                }
                else {
                    // Attempt to load it as a CHR_File.
                    if (verbose)
                        Trace.WriteLine("Creating CHR_File...");
                    var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                    var chrFile = CHR_File.Create(new ByteData(new ByteArray(inputBytes)), nameGetterContext, ScenarioType.Scenario1);

                    // Get a CHR_Def from the CHR_File.
                    if (verbose)
                        Trace.WriteLine("Serializing to CHR_Def...");
                    var chrDef = chrFile.ToCHR_Def();
                    if (optimize) {
                        foreach (var sprite in chrDef.Sprites)
                            sprite.FrameGroupsForSpritesheets = null;
                    }

                    // Serialize the file.
                    if (verbose)
                        Trace.WriteLine($"Converting to JSON file...");
                    outputText = chrDef.ToJSON_String();
                }

                if (verbose)
                    Trace.WriteLine($"Writing to '{outputFile}'...");
                File.WriteAllText(outputFile, outputText);
            }
            catch (Exception e) {
                Trace.WriteLine("------------------------------------------------------------------------------");
                Trace.TraceError($"Error:");
                Trace.TraceError($"    {e.GetTypeAndMessage()}");
                return 1;
            }

            Trace.WriteLine("------------------------------------------------------------------------------");
            Trace.WriteLine("Done");
            return 0;
        }
    }
}
