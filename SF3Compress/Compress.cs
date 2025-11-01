using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using static CommonLib.Extensions.StringExtensions;

namespace SF3Compress {
    public static class Compress {
        public static int Run(string[] args, bool verbose) {
            int offset = 0;
            int? size = null;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "o|offset=", v => offset = v.ConvertFromDecOrHex<int>() },
                { "s|size=",   v => size = v.ConvertFromDecOrHex<int>() },
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
                Logger.WriteLine("Unrecognized arguments in 'compress' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            if (verbose)
                Logger.WriteLine($"Compressing {inputFilename} at +0x{offset:X4}:");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                try {
                    // TODO: use a stream!!!!!! this is AWFUL!!!!!!
                    var allData = File.ReadAllBytes(inputFilename);
                    if (offset != 0 || size.HasValue)
                        allData = allData[offset .. (size.HasValue ? (offset + size.Value) : allData.Length)];

                    var compressedData = CommonLib.Utils.Compression.CompressLZSS(allData);
                    File.WriteAllBytes(outputFilename, compressedData);
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
