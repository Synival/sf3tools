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
            "Spritesheet Extractor v" + c_Version + "\n";

        private const string c_ShortUsageString =
            "Usage:\n" +
            "  spr_extract [OPTIONS] disc-path\n";

        private const string c_ErrorUsageString =
            c_ShortUsageString +
            "Try 'spr_extract --help' for more information.\n";

        private const string c_FullUsageString =
            c_ShortUsageString +
            "\n" +
            "Extracts spritesheets from Shining Force III CHR and CHP files.\n" +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "  --version                 print CHRTool version\n" +
            "  --sprite-dir=<dir>        directory for sprites (.SF3Sprite files)\n" +
            "      (default='<program-dir>/Resources/Sprites')\n" +
            "  --spritesheet-dir=<dir>   directory for spritesheets (.PNG files)\n" +
            "      (default='<program-dir>/Resources/Spritesheets')\n" +
            "\n";

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

            // There shouldn't be any unrecognized options before the command.
            if (args.Length > 1) {
                Console.Error.WriteLine($"Expected 1 non-option argument (path), got {args.Length}:");
                Console.Error.Write($"    {string.Join(" ", args)}");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // TODO: don't do it this way, omg :( :( :(
            if (spriteDir != null)
                SpriteUtils.SetSpritePath(spriteDir);
            if (spritesheetDir != null)
                SpriteUtils.SetSpritesheetPath(spritesheetDir);

            // TODO: start rippin'!
            Console.WriteLine("Coming soon (tm)!");
            return 0;
        }
    }
}
