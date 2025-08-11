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
        public static int Run(string[] args, string spriteDir, string spritesheetDir) {
            string outputFile = null;

            // Read any command line options.
            var compileOptions = new OptionSet() {
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
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            var inputFilename = Path.GetFileName(inputFile);
            if (outputFile == null)
                outputFile = Path.Combine(Path.GetDirectoryName(inputFile), $"{Path.GetFileNameWithoutExtension(inputFilename)}.SF3CHR");
            var outputFilename = Path.GetFileName(outputFile);
            Console.WriteLine($"Decompiling '{inputFilename}' to '{outputFilename}'...");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"Sprite directory:      {spriteDir}");
            Console.WriteLine($"Spritesheet directory: {spritesheetDir}");
            Console.WriteLine("------------------------------------------------------------------------------");



























            byte[] chrBytes = null;
            try {
                chrBytes = File.ReadAllBytes(inputFile);
                Console.WriteLine("  Read input file successfully.");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't open '{inputFile}' for reading:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Attempt to load it as a CHR_File.
            CHR_File chrFile = null;
            try {
                var nameGetterContext = new NameGetterContext(ScenarioType.Scenario1);
                chrFile = CHR_File.Create(new ByteData(new ByteArray(chrBytes)), nameGetterContext, ScenarioType.Scenario1);
                Console.WriteLine("  Loaded CHR_File successfully.");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't create CHR_File after reading:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Get a CHR_Def from the CHR_File.
            CHR_Def chrDef = null;
            try {
                chrDef = chrFile.ToCHR_Def();
                Console.WriteLine("  Created CHR_Def successfully.");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't create CHR_Def from CHR_File:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            // Serialize the file.
            try {
                var chrDefText = chrDef.ToJSON_String();
                File.WriteAllText(outputFile, chrDefText);
                Console.WriteLine("  Output file written successfully.");
            }
            catch (Exception e) {
                Console.WriteLine("------------------------------------------------------------------------------");
                Console.Error.WriteLine($"  Couldn't write to '{outputFile}' after serializing:");
                Console.Error.WriteLine($"    {e.GetType().Name}: {e.Message}");
                return 1;
            }

            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("Done");
            return 0;
        }
    }
}
