using System;
using System.IO;
using System.Linq;
using CommonLib.Arrays;
using CommonLib.Logging;
using CommonLib.Types;
using SF3.ByteData;
using SF3.CHR;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.NamedValues;
using SF3.Types;

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
            if (verbose)
                Logger.WriteLine("Describing files:");
            using (Logger.IndentedSection(verbose ? 1 : 0)) {
                foreach (var file in files) {
                    Logger.WriteLine($"{file}:");
                    using (Logger.IndentedSection()) {
                        try {
                            DescribeFile(file, verbose);
                        }
                        catch (Exception e) {
                            Logger.LogException(e);
                        }
                    }
                }
            }

            if (verbose)
                Logger.WriteLine("Done");

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
            var bytes = File.ReadAllBytes(inputFile);
            var chrFile = CHR_File.Create(new ByteData(new ByteArray(bytes)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);
        }

        private static void DescribeCHP(string inputFile, bool verbose) {
            var bytes = File.ReadAllBytes(inputFile);
            var chpFile = CHP_File.Create(new ByteData(new ByteArray(bytes)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);
        }

        private static void DescribeSF3CHR(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chrDef = CHR_Def.FromJSON(text);
        }

        private static void DescribeSF3CHP(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chpDef = CHP_Def.FromJSON(text);
        }

        private static void DescribeSF3Sprite(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var spriteDef = SF3.Sprites.SpriteDef.FromJSON(text);
        }

        private static void DescribeSF3CHRSprite(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chrSpriteDef = SF3.CHR.SpriteDef.FromJSON(text);
        }
    }
}
