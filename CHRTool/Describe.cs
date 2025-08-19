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
            DescribeCHR(chrFile, verbose);
        }

        private static void DescribeCHR(ICHR_File chrFile, bool verbose) {
            foreach (var sprite in chrFile.SpriteTable) {
                var header = sprite.Header;
                var promotionLevelStr = (header.PromotionLevel == 0) ? "" : $", PromotionLevel={header.PromotionLevel}";
                Logger.WriteLine($"[0x{sprite.IDInGroup:X2}] {sprite.SpriteName} (SpriteID=0x{header?.SpriteID:X2}, Size=({header?.Width}x{header?.Height}), Dirs={header?.Directions}{promotionLevelStr}):");

                using (Logger.IndentedSection()) {
                    try {
                        if (sprite.FrameTable == null)
                            Logger.WriteLine("No frame table", LogType.Warning);
                        else if (verbose) {
                            Logger.WriteLine("Frames:");
                            using (Logger.IndentedSection())
                                foreach (var frame in sprite.FrameTable)
                                    Logger.WriteLine($"[0x{frame.ID:X2}]: {frame.SpriteName}.{frame.FrameName} ({frame.Direction})");
                        }

                        if (sprite.AnimationTable == null || sprite.AnimationOffsetTable == null)
                            Logger.WriteLine("No animation table", LogType.Warning);
                        else {
                            Logger.WriteLine("Animations:");
                            using (Logger.IndentedSection())
                                foreach (var animation in sprite.AnimationTable)
                                    Logger.WriteLine($"[0x{animation.AnimationIndex:X2}]: {animation.SpriteName}.{animation.AnimationName}");
                        }
                    }
                    catch (Exception e) {
                        Logger.LogException(e);
                    }
                }
            }
        }

        private static void DescribeCHP(string inputFile, bool verbose) {
            var bytes = File.ReadAllBytes(inputFile);
            var chpFile = CHP_File.Create(new ByteData(new ByteArray(bytes)), new NameGetterContext(ScenarioType.Scenario1), ScenarioType.Scenario1);

            int index = 0;
            foreach (var chrKv in chpFile.CHR_EntriesByOffset) {
                Logger.WriteLine($"[0x{index++:X2} @0x{chrKv.Key:X5}]:");
                using (Logger.IndentedSection()) {
                    try {
                        DescribeCHR(chrKv.Value, verbose);
                    }
                    catch (Exception e) {
                        Logger.LogException(e);
                    }
                }
            }
        }

        private static void DescribeSF3CHR(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chrDef = CHR_Def.FromJSON(text);
            DescribeSF3CHR(chrDef, verbose);
        }

        private static void DescribeSF3CHR(CHR_Def chrDef, bool verbose) {
            int index = 0;
            foreach (var sprite in chrDef.Sprites) {
                var promotionLevelStr = (sprite.PromotionLevel == 0) ? "" : $", PromotionLevel={sprite.PromotionLevel}";
                Logger.WriteLine($"[0x{index++:X2}] {sprite.SpriteName} (SpriteID=0x{sprite.SpriteID:X2}, Size=({sprite.Width}x{sprite.Height}), Dirs={sprite.Directions}{promotionLevelStr}):");

                using (Logger.IndentedSection()) {
                    try {
                        if (verbose && sprite.FrameGroupsForSpritesheets != null) {
                            Logger.WriteLine("Frames:");
                            using (Logger.IndentedSection()) {
                                int frameIndex = 0;
                                foreach (var fgss in sprite.FrameGroupsForSpritesheets) {
                                    var sizeStr = (fgss.Width.HasValue && fgss.Height.HasValue) ? $", Size=({fgss.Width.Value}x{fgss.Height.Value})" : "";
                                    var str = sizeStr;
                                    str = str.Substring(Math.Min(str.Length, 2));

                                    if (str != "")
                                        Logger.WriteLine(str);
                                    using (Logger.IndentedSection((str != "") ? 1 : 0)) {
                                        foreach (var frameGroup in fgss.FrameGroups ?? new FrameGroup[0]) {
                                            if (frameGroup.Frames == null) {
                                                var dirs = SpriteDirectionCountTypeExtensions.ToAnimationFrameDirections(sprite.Directions);
                                                foreach (var dir in dirs)
                                                    Logger.WriteLine($"[0x{frameIndex++:X2}, in group]: {fgss.SpriteName ?? sprite.SpriteName}.{frameGroup.Name} ({dir})");
                                            }
                                            else {
                                                foreach (var frame in frameGroup.Frames)
                                                    Logger.WriteLine($"[0x{frameIndex++:X2}, individual]: {fgss.SpriteName ?? sprite.SpriteName}.{frameGroup.Name} ({frame.Direction})");
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (sprite.AnimationsForSpritesheetAndDirections == null || sprite.AnimationsForSpritesheetAndDirections.Length == 0)
                            Logger.WriteLine("No animations", LogType.Warning);
                        else {
                            Logger.WriteLine("Animations:");
                            using (Logger.IndentedSection()) {
                                int animationIndex = 0;
                                foreach (var assd in sprite.AnimationsForSpritesheetAndDirections) {
                                    var sizeStr = (assd.Width.HasValue && assd.Height.HasValue) ? $", Size=({assd.Width.Value}x{assd.Height.Value})" : "";
                                    var dirsStr = assd.Directions.HasValue ? $", Dirs=({assd.Directions.Value})" : "";
                                    var str = sizeStr + dirsStr;
                                    str = str.Substring(Math.Min(str.Length, 2));

                                    if (str != "")
                                        Logger.WriteLine(str);
                                    using (Logger.IndentedSection((str != "") ? 1 : 0)) {
                                        foreach (var animation in assd.Animations ?? new string[0])
                                            Logger.WriteLine($"[0x{animationIndex++:X2}]: {assd.SpriteName ?? sprite.SpriteName}.{animation}");
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Logger.LogException(e);
                    }
                }
            }
        }

        private static void DescribeSF3CHP(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chpDef = CHP_Def.FromJSON(text);

            int index = 0;
            foreach (var chrKv in chpDef.CHRsBySector) {
                Logger.WriteLine($"[0x{index++:X2} @0x{chrKv.Key * 0x800:X5}]:");
                using (Logger.IndentedSection()) {
                    try {
                        DescribeSF3CHR(chrKv.Value, verbose);
                    }
                    catch (Exception e) {
                        Logger.LogException(e);
                    }
                }
            }
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
