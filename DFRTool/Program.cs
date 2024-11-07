using DFRLib;
using NDesk.Options;

namespace DFRTool {
    internal class Program {
        private const string c_ShortUsageString =
            "Usage:\n" +
            "  dfrtool [GENERAL_OPTIONS] apply [APPLY_OPTIONS]... bin_file dfr_file\n" +
            "  dfrtool [GENERAL_OPTIONS] create [CREATE_OPTIONS]... original_file altered_file\n";
        private const string c_ErrorUsageString =
            c_ShortUsageString +
            "Try 'dfrtool --help' for more information.\n";
        private const string c_FullUsageString =
            c_ShortUsageString +
            "\n" +
            "Outputs text in DFR format based on a comparison between two files.\n" +
            "\n" +
            "General Options:\n" +
            "  -h, --help                print this help message\n" +
            "\n" +
            "Apply Options:\n" +
            "  (none)\n" +
            "\n" +
            "Create Ootions:\n" +
            "  -c, --combined-appends    merges all appended changes into one row\n" +
            "\n";

        private enum Command {
            Apply = 0,
            Create = 1
        };

        private static int Main(string[] args) {
            // Complain if no command was found.
            if (args == null || args.Length == 0) {
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Look for an 'apply' or 'create' keyword first.
            Command? command = null;
            int commandArg = -1;
            for (int i = 0; i < args.Length; i++) {
                if (args[i].ToLower() == "apply") {
                    command = Command.Apply;
                    commandArg = i;
                    break;  
                }
                else if (args[i].ToLower() == "create") {
                    command = Command.Create;
                    commandArg = i;
                    break;  
                }
            }

            // If a command was supplied, split arguments up.
            var generalArgs   = (command != null) ? args[0..commandArg] : args;
            var remainingArgs = (command != null) ? args[(commandArg + 1)..args.Length] : [];

            // Gather general options.
            var outputHelp = false;
            var anywhereOptions = new OptionSet() {
               { "h|help", v => outputHelp = true },
            };
            var generalOptions = new OptionSet() {
                // more options coming sometime!
            };

            string[] extraArgsBeforeCommand;
            try {
                _ = anywhereOptions.Parse(args);
                extraArgsBeforeCommand = generalOptions.Parse(generalArgs).ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("Exception caught: " + e.Message);
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Never show errors if -h was specified anywhere.
            if (outputHelp) {
                Console.Write(c_FullUsageString);
                return 0;
            }

            // Complain if no command was found.
            if (command == null) {
                Console.Error.WriteLine("Couldn't find 'apply' or 'create' command.");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized options before the command.
            if (extraArgsBeforeCommand.Length > 0) {
                // TODO: what arguments? maybe tell the user??
                Console.Error.WriteLine("Unrecognized arguments before 'apply' or 'create' command.");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            switch (command) {
                case Command.Apply:
                    // TODO: actual command, lawl
                    Console.WriteLine("Sorry, bud - not implemented yet lol");
                    return 0;

                case Command.Create:
                    return Create(remainingArgs);

                default:
                    Console.Error.WriteLine("Fatal error: unimplemented command '" + command.ToString() + "'");
                    return 1;
            }
        }

        private static int Create(string[] args) {
            var combineAppends = false;

            var options = new OptionSet() {
               { "c|combine-appends", v => combineAppends = true },
            };

            string[] extra;
            try {
                extra = options.Parse(args).ToArray();
            }
            catch (Exception e) {
                Console.WriteLine("Exception caught: " + e.Message);
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            // Require two arguments.
            if (extra.Length != 2) {
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            var filenameFrom = extra[0];
            var filenameTo = extra[1];

            // Create the diff. Log and errors.
            ByteDiff diff;
            try {
                diff = new ByteDiff(filenameFrom, filenameTo, new ByteDiffChunkBuilderOptions {
                    CombineAppendedChunks = combineAppends
                });
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            // We did it! Write the DFR file.
            Console.Write(diff.ToDFR());
            return 0;
        }
    }
}
