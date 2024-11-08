using DFRLib;
using NDesk.Options;

namespace DFRTool {
    internal class Program {
        private const string c_Version = "1.1.1";

        private const string c_VersionString =
            "DFRTool v" + c_Version + "\n";

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
            "  --version                 print DFRTool version\n" +
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
            var outputVersion = false;

            var anywhereOptions = new OptionSet() {
               { "h|help",  v => outputHelp = true },
               { "version", v => outputVersion = true },
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
                    return Apply(remainingArgs);

                case Command.Create:
                    return Create(remainingArgs);

                default:
                    Console.Error.WriteLine("Fatal error: unimplemented command '" + command.ToString() + "'");
                    return 1;
            }
        }

        private static int Apply(string[] args) {
            var options = new OptionSet() {
                // more options coming sometime!
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
                Console.Error.WriteLine("Incorrect number of 'apply' arguments.");
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            var fileToPatchName = extra[0];
            var dfrFilename = extra[1];

            try {
                var diff = new ByteDiff(dfrFilename);
                var oldBytes = File.ReadAllBytes(fileToPatchName);
                var newBytes = diff.ApplyTo(oldBytes);
                File.WriteAllBytes(fileToPatchName, newBytes);
            }
            catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            // We did it! Write the DFR file.
            Console.WriteLine(fileToPatchName + " patched successfully.");
            return 0;
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
                Console.Error.WriteLine("Incorrect number of 'create' arguments.");
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
