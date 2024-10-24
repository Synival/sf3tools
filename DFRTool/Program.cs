using DFRLib;
using NDesk.Options;

namespace DFRTool {
    internal class Program {
        const string c_ShortUsageString =
            "Usage: dfrtool [OPTION]... original_file altered_file\n";

        const string c_ErrorUsageString =
            c_ShortUsageString +
            "Try 'dfrtool --help' for more information.\n";

        const string c_FullUsageString =
            c_ShortUsageString +
            "Outputs text in DFR format based on a comparison between two files.\n" +
            "\n" +
            "Options:\n" +
            "  -h, --help                print this help message\n" +
            "  -c, --combined-appends    merges all appended changes into one row\n";

        static int Main(string[] args) {
            bool combineAppends = false;
            bool outputHelp = false;

            var options = new OptionSet() {
               { "c|combine-appends", v => combineAppends = true },
               { "h|help",            v => outputHelp = true },
            };

            List<string> extra;
            try {
                extra = options.Parse(args);
            }
            catch (Exception e) {
                Console.WriteLine("Exception caught: " + e.Message);
                Console.Error.Write(c_ErrorUsageString);
                return 1;
            }

            if (outputHelp) {
                Console.Write(c_FullUsageString);
                return 1;
            }

            // Require two arguments.
            if (extra.Count != 2) {
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
