using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using static CommonLib.Extensions.StringExtensions;

namespace SF3Compress {
    public static class Decompress {
        public static int Run(string[] args, bool verbose) {
            int offset = 0;
            int? maxSize = null;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "o|offset=", v => offset = v.ConvertFromDecOrHex<int>() },
                { "s|size=",   v => maxSize = v.ConvertFromDecOrHex<int>() },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.LogException(e);
                return 1;
            }

            // Two arguments for filenames required.
            if (args.Length < 2) {
                Logger.WriteLine("Input and output files required", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            var inputFilename  = args[0];
            var outputFilename = args[1];
            args = args[2 .. args.Length];

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'decompress' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            if (verbose)
                Logger.WriteLine($"Decompressing {inputFilename} at +0x{offset:X4}:");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                try {
                    // TODO: use a stream!!!!!! this is AWFUL!!!!!!
                    var allData = File.ReadAllBytes(inputFilename);
                    if (offset != 0)
                        allData = allData[offset .. allData.Length];

                    var decompressedData = CommonLib.Utils.Compression.DecompressLZSS(allData, maxSize, out var bytesRead, out var endDataFound);
                    if (!endDataFound)
                        Logger.WriteLine($"{inputFilename}: No end code found -- data may be corrupt or ended prematurely", LogType.Warning);
                    File.WriteAllBytes(outputFilename, decompressedData);
                }
                catch (Exception ex) {
                    Logger.Write($"{inputFilename}: ", LogType.Error);
                    Logger.LogException(ex);
                }
            }
            if (verbose)
                Logger.WriteLine("Done");

            return 0;
        }
    }
}
