using System;
using System.IO;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Logging;
using CommonLib.Types;
using Newtonsoft.Json.Linq;
using SF3.CHR;

namespace CHRTool {
    public static class Describe {
        public static int Run(string[] args, bool verbose) {
            // Fetch the directory with the game data.
            string[] files;
            (files, args) = Utils.GetFilesAndPathsFromAgs(args, path => {
                return Directory.GetFiles(path, "*.SF3CHR")
                    .Concat(Directory.GetFiles(path, "*.SF3CHP"))
                    .Concat(Directory.GetFiles(path, "*.CHR"))
                    .Concat(Directory.GetFiles(path, "*.CHP"))
                    .Concat(Directory.GetFiles(path, "*.SF3Sprite"))
                    .Concat(Directory.GetFiles(path, "*.SF3CHRSprite"))
                    .OrderBy(x => x)
                    .ToArray();
            });
            if (files.Length == 0) {
                Logger.WriteLine("No file(s) or path(s) provided", LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // There shouldn't be any unrecognized arguments at this point.
            if (args.Length > 0) {
                Logger.WriteLine("Unrecognized arguments in 'describe' command: " + string.Join(" ", args), LogType.Error);
                Logger.Write(Constants.ErrorUsageString);
                return 1;
            }

            // It looks like we're ready to go! Fetch the file data.
            try {
                if (verbose)
                    Logger.WriteLine("Describing files:");

                foreach (var file in files) {
                    try {
                        if (verbose)
                            Logger.WriteLine("------------------------------------------------------------------------------");

                        Logger.WriteLine($"{file}:");
                        DescribeFile(file, verbose);
                    }
                    catch (Exception e) {
                        Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                    }
                }
            }
            catch (Exception e) {
                if (verbose)
                    Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine(e.GetTypeAndMessage(), LogType.Error);
                return 1;
            }

            if (verbose) {
                Logger.WriteLine("------------------------------------------------------------------------------");
                Logger.WriteLine("Done");
            }
            return 0;
        }

        private static void DescribeFile(string inputFile, bool verbose) {
            var inputFileLower = inputFile.ToLower();
            if (inputFileLower.EndsWith(".chr"))
                DescribeCHR(inputFile, verbose);
            else if (inputFileLower.EndsWith(".chp"))
                DescribeCHP(inputFile, verbose);
            else if (inputFileLower.EndsWith(".sf3chr"))
                DescribeSF3CHR(inputFile, verbose);
            else if (inputFileLower.EndsWith(".sf3chp"))
                DescribeSF3CHP(inputFile, verbose);
            else if (inputFileLower.EndsWith(".sf3sprite"))
                DescribeSF3Sprite(inputFile, verbose);
            else if (inputFileLower.EndsWith(".sf3chrsprite"))
                DescribeSF3CHRSprite(inputFile, verbose);
            else
                Logger.WriteLine($"Unknown file type for '{inputFileLower}'", LogType.Error);
        }

        private static void DescribeCHR(string inputFile, bool verbose) {
            // TODO: describe!
        }

        private static void DescribeCHP(string inputFile, bool verbose) {
            // TODO: describe!
        }

        private static void DescribeSF3CHR(string inputFile, bool verbose) {
            // TODO: describe!
        }

        private static void DescribeSF3CHP(string inputFile, bool verbose) {
            // TODO: describe!
        }

        private static void DescribeSF3Sprite(string inputFile, bool verbose) {
            // TODO: describe!
        }

        private static void DescribeSF3CHRSprite(string inputFile, bool verbose) {
            // TODO: describe!
        }
    }
}
