using System;
using System.Collections.Generic;
using System.IO;
using NDesk.Options;
using SF3.CHR;
using SF3.Utils;

namespace DFRTool {
    public class Program {
        private const string c_Version = "0.1";

        private const string c_VersionString =
            "CHRTool v" + c_Version + "\n";

        private const string c_ShortUsageString =
            "Usage:\n" +
            "  chrtool [GENERAL_OPTIONS] compile [COMPILE_OPTIONS]... SF3CHR_file\n" +
            "  chrtool [GENERAL_OPTIONS] decompile [DECOMPILE_OPTIONS]... CHR_file\n";

        private const string c_ErrorUsageString =
            c_ShortUsageString +
            "Try 'chrtool --help' for more information.\n";

        private const string c_FullUsageString =
            c_ShortUsageString +
            "\n" +
            "Compiles or decompiles CHR files from/to SF3CHR files.\n" +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print CHRTool version\n" +
            "  --sprite-dir=<dir>        directory for sprites (.SF3Sprite files)\n" +
            "      (default='<program-dir>/Resources/Sprites')\n" +
            "  --spritesheet-dir=<dir>   directory for spritesheets (.PNG files)\n" +
            "      (default='<program-dir>/Resources/Spritesheets')\n" +
            "\n" +
            "Compile Options:\n" +
            "  -O, --optimize            optimizes frames, ignoring anything explicit\n" +
            "  --output=<output-file>    specify output .CHR file\n" +
            "  --add-sprite=<file>       adds an SF3CHRSprite file\n" +
            "\n" +
            "Decompile Options:\n" +
            "  --output=<output-file>    specify output .SF3CHR file\n" +
            "\n";

        enum CommandType {
            Compile,
            Decompile
        }

        private static int Main(string[] args) {
            // Complain if no command was found.
            if (args == null || args.Length == 0) {
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Gather general options.
            var outputHelp     = false;
            var outputVersion  = false;
            var programDir     = AppDomain.CurrentDomain.BaseDirectory;
            var spriteDir      = Path.Combine(programDir, "Resources", "Sprites");
            var spritesheetDir = Path.Combine(programDir, "Resources", "Spritesheets");

            var anywhereOptions = new OptionSet() {
                { "h|help",           v => outputHelp = true },
                { "version",          v => outputVersion = true },
                { "sprite-dir=",      v => spriteDir = v },
                { "spritesheet-dir=", v => spritesheetDir = v },
            };

            try {
                args = anywhereOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Never show errors if -h was specified anywhere.
            if (outputHelp) {
                if (outputVersion)
                    Console.Write(c_VersionString);
                Console.Write(c_FullUsageString);
                return 0;
            }

            // Always just show version string if requested (unless help is requested).
            if (outputVersion) {
                Console.Write(c_VersionString);
                return 0;
            }

            // Look for a 'compile' or 'decompile' keyword.
            CommandType? command = null;
            int commandArg = -1;
            for (int i = 0; i < args.Length; i++) {
                if (args[i].ToLower() == "compile") {
                    command = CommandType.Compile;
                    commandArg = i;
                    break;
                }
                else if (args[i].ToLower() == "decompile") {
                    command = CommandType.Decompile;
                    commandArg = i;
                    break;
                }
            }

            // If a command was supplied, split arguments up.
            var generalArgs   = (command != null) ? args[0..commandArg] : args;
            var remainingArgs = (command != null) ? args[(commandArg + 1)..args.Length] : new string[0];

            var generalOptions = new OptionSet() {
                // more options coming sometime!
            };

            string[] extraArgsBeforeCommand;
            try {
                _ = anywhereOptions.Parse(args);
                extraArgsBeforeCommand = generalOptions.Parse(generalArgs).ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("Error: " + e.Message);
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Complain if no command was found.
            if (command == null) {
                Console.Error.WriteLine("Couldn't find 'compile' or 'decompile' command.");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized options before the command.
            if (extraArgsBeforeCommand.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments before 'compile' or 'decompile' command:");
                Console.Error.Write($"    {string.Join(" ", extraArgsBeforeCommand)}");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // TODO: don't do it this way, omg :( :( :(
            if (spriteDir != null)
                SpriteUtils.SetSpritePath(spriteDir);
            if (spritesheetDir != null)
                SpriteUtils.SetSpritesheetPath(spritesheetDir);

            switch (command) {
                case CommandType.Compile:
                    return Compile(remainingArgs, spriteDir, spritesheetDir);

                case CommandType.Decompile:
                    return Decompile(remainingArgs);

                default:
                    Console.Error.WriteLine("Internal error: unimplemented command '" + command.ToString() + "'");
                    return 1;
            }
        }

        private static int Compile(string[] args, string spriteDir, string spritesheetDir) {
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
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Fetch the filename for reading.
            if (args.Length == 0) {
                Console.Error.WriteLine("Missing input file");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }
            var inputFile = args[0];
            args = args[1..args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Console.Error.WriteLine("Unrecognized arguments in 'compile' command:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(c_ErrorUsageString);
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

        private static int Decompile(string[] args) {
            Console.Error.WriteLine("Coming soon (tm)!");
            return 1;
        }
    }
}
