using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using static CommonLib.Extensions.StringExtensions;
using static CommonLib.Extensions.ArrayExtensions;

namespace SF3Compress {
    public static class Decompress {
        public static int Run(string[] args, bool verbose) {
            int offset = 0;
            int? maxSize = null;
            bool print = false;
            int wordsPerRow = 8;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "o|offset=", v => offset = v.ConvertFromDecOrHex<int>() },
                { "s|size=",   v => maxSize = v.ConvertFromDecOrHex<int>() },
                { "p|print",   v => print = true },
                { "words-per-row=", v => wordsPerRow = v.ConvertFromDecOrHex<int>() },
            };
            try {
                args = compileOptions.Parse(args).ToArray();
            }
            catch (Exception e) {
                Logger.LogException(e);
                return 1;
            }

            // Two arguments for filenames required.
            if (print && args.Length < 1) {
                Logger.WriteLine("Input file required", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }
            else if (!print && args.Length < 2) {
                Logger.WriteLine("Input and output files required", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            var inputFilename  = args[0];
            var outputFilename = print ? null : args[1];
            args = args[(print ? 1 : 2) .. args.Length];

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
                    if (print)
                        PrintDecompressedLZSS(decompressedData, wordsPerRow);
                    else
                        File.WriteAllBytes(outputFilename, decompressedData);

                    Logger.WriteLine($"Decompressed 0x{bytesRead:X2} ({bytesRead}) bytes to 0x{decompressedData.Length:X2} ({decompressedData.Length}) bytes");

                    if (!endDataFound)
                        Logger.WriteLine($"{inputFilename}: No end code found -- data may be corrupt or ended prematurely", LogType.Warning);
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

        public static void PrintDecompressedLZSS(byte[] bytes, int wordsPerRow = 0x10) {
            int pos = 0;
            var words = bytes.ToUShorts();

            while (pos < words.Length) {
                if (pos > 0)
                    Logger.FinishLine();

                Logger.Write($"0x{pos * 2:X4}:");
                for (int i = 0; i < wordsPerRow && pos < words.Length; i++)
                    Logger.Write($" {words[pos++]:X4}");
            }
            Logger.FinishLine();
        }
    }
}
