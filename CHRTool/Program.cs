using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using SF3.Sprites;

namespace CHRTool {
    public class Program {
        private static int Main(string[] args) {
            // Complain if no command was found.
            if (args == null || args.Length == 0) {
                Logger.Write(Constants.VersionString);
                Logger.Write(Constants.ShortUsageString);
                return 1;
            }

            // Gather general options.
            var outputHelp     = false;
            var outputVersion  = false;
            var verbose        = false;
            var programDir     = AppDomain.CurrentDomain.BaseDirectory;
            var spriteDir      = Path.Combine(programDir, "Resources", "Sprites");
            var spritesheetDir = Path.Combine(programDir, "Resources", "Spritesheets");
            var frameHashLookupsFile = Path.Combine(programDir, "Resources", "FrameHashLookups.json");

            var anywhereOptions = new OptionSet() {
                { "h|help",           v => outputHelp = true },
                { "version",          v => outputVersion = true },
                { "sprite-dir=",      v => spriteDir = v },
                { "spritesheet-dir=", v => spritesheetDir = v },
                { "frame-hash-lookups-file=", v => frameHashLookupsFile = v },
                { "v|verbose",        v => verbose = true },
            };

            try {
                args = anywhereOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.LogException(e);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // Never show errors if -h was specified anywhere.
            if (outputHelp) {
                if (outputVersion)
                    Logger.Write(Constants.VersionString);
                Logger.Write(Constants.FullUsageString);
                return 0;
            }

            // Always just show version string if requested (unless help is requested).
            if (outputVersion) {
                Logger.Write(Constants.VersionString);
                return 0;
            }

            // Look for a command.
            CommandType? command = null;
            int commandArg = -1;
            for (int i = 0; i < args.Length; i++) {
                if (Constants.CommandKeywords.TryGetValue(args[i].ToLower(), out var commandOut)) {
                    command = commandOut;
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
                Logger.LogException(e);
                return 1;
            }

            // Complain if no command was found.
            if (command == null) {
                Logger.WriteLine("Couldn't find command", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized options before the command.
            if (extraArgsBeforeCommand.Length > 0) {
                Logger.WriteLine("Unrecognized arguments before command: " + string.Join(" ", extraArgsBeforeCommand), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // TODO: don't do it this way, omg :( :( :(
            if (spriteDir != null)
                SpriteResources.SpritePath = spriteDir;
            if (spritesheetDir != null)
                SpriteResources.SpritesheetPath = spritesheetDir;
            if (frameHashLookupsFile != null)
                SpriteResources.FrameHashLookupsFile = frameHashLookupsFile;

            int rval = 0;
            bool showWarningsAndErrors = false;
            switch (command) {
                case CommandType.Compile:
                    showWarningsAndErrors = true;
                    rval = Compile.Run(remainingArgs, verbose);
                    break;

                case CommandType.Decompile:
                    showWarningsAndErrors = true;
                    rval = Decompile.Run(remainingArgs, verbose);
                    break;

                case CommandType.ExtractSheets:
                    showWarningsAndErrors = true;
                    rval = ExtractSheets.Run(remainingArgs, spritesheetDir, verbose);
                    break;

                case CommandType.UpdateHashLookups:
                    showWarningsAndErrors = true;
                    rval = UpdateHashLookups.Run(remainingArgs, spriteDir, frameHashLookupsFile, verbose);
                    break;

                case CommandType.Describe:
                    rval = Describe.Run(remainingArgs, verbose);
                    break;

                default:
                    Logger.WriteLine("Internal error: unimplemented command '" + command.ToString() + "'", LogType.Error);
                    return 1;
            }

            var warnings = Logger.TotalWarningCount;
            var errors = Logger.TotalErrorCount;
            if (showWarningsAndErrors && (errors > 0 || warnings > 0 || verbose))
                Logger.WriteLine($"{warnings} warning(s), {errors} error(s)");

            // If errors were reported, never return 0 (success).
            if (rval == 0 && errors > 0) {
                if (verbose)
                    Logger.WriteLine($"Errors detected; returning failure");
                rval = 1;
            }
            return rval;
        }
    }
}
