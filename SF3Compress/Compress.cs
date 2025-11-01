using System;
using System.IO;
using CommonLib.Logging;
using CommonLib.Types;
using NDesk.Options;
using static CommonLib.Extensions.StringExtensions;
using static CommonLib.Extensions.ArrayExtensions;

namespace SF3Compress {
    public static class Compress {
        public static int Run(string[] args, bool verbose) {
            int offset = 0;
            bool optimize = false;
            int? size = null;
            bool print = false;

            // Read any command line options.
            var compileOptions = new OptionSet() {
                { "o|offset=",  v => offset = v.ConvertFromDecOrHex<int>() },
                { "s|size=",    v => size = v.ConvertFromDecOrHex<int>() },
                { "O|optimize", v => optimize = true },
                { "p|print",    v => print = true },
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

                    var compressedData = CommonLib.Utils.Compression.CompressLZSS(allData, optimize);
                    if (print)
                        PrintCompressedLZSS(compressedData);
                    else
                        File.WriteAllBytes(outputFilename, compressedData);

                    Logger.WriteLine($"Compressed 0x{allData.Length:X2} ({allData.Length}) bytes down to 0x{compressedData.Length:X2} ({compressedData.Length}) bytes");
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

        public static void PrintCompressedLZSS(byte[] bytes) {
            int pos = 0;
            var words = bytes.ToUShorts();

            while (pos < words.Length) {
                if (pos > 0)
                    Logger.FinishLine();

                Logger.Write($"0x{pos * 2:X4}: ");
                Console.ForegroundColor = ConsoleColor.White;
                var controlWord = words[pos++];
                Logger.Write($"{controlWord:X4}");
                Console.ForegroundColor = ConsoleColor.Gray;
                Logger.Write(" |");

                for (int i = 0; i < 0x10 && pos < words.Length; i++) {
                    var bit = 1 << (15 - i);
                    var isControlBit = (controlWord & bit) != 0;

                    if (isControlBit)
                        Console.ForegroundColor = ConsoleColor.White;
                    Logger.Write($" {words[pos++]:X4}");
                    if (isControlBit)
                        Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            Logger.FinishLine();
        }
    }
}
