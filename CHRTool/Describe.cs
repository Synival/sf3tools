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
using SF3.Sprites;
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
                Logger.WriteLine($"[0x{sprite.IDInGroup:X2}] {sprite.SpriteName} (SpriteID=0x{header?.SpriteID:X2}, {header?.Width}x{header?.Height}, Dirs={(SpriteDirectionCountType) header?.Directions}{promotionLevelStr}):");

                using (Logger.IndentedSection()) {
                    try {
                        if (sprite.FrameTable == null)
                            Logger.WriteLine("No frame table", LogType.Warning);
                        else if (verbose) {
                            Logger.WriteLine("Frames:");
                            using (Logger.IndentedSection()) {
                                // TODO: fancy grouping algoithm
                                foreach (var frame in sprite.FrameTable)
                                    Logger.WriteLine($"[0x{frame.ID:X2}]: {frame.SpriteName}.{frame.FrameName} ({frame.Direction})");
                            }
                        }

                        if (sprite.AnimationTable == null || sprite.AnimationOffsetTable == null)
                            Logger.WriteLine("No animation table", LogType.Warning);
                        else {
                            Logger.WriteLine("Animations:");
                            using (Logger.IndentedSection()) {
                                foreach (var animation in sprite.AnimationTable) {
                                    var prefix = (animation.SpriteName != sprite.SpriteName) ? $"({animation.SpriteName}) " : "";
                                    Logger.WriteLine($"[0x{animation.AnimationIndex:X2}]: {prefix}{animation.AnimationName}");
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
            foreach (var sprite in chrDef.Sprites)
                DescribeSF3CHRSprite(sprite, verbose, index++);
        }

        private static void DescribeSF3CHRSprite(string inputFile, bool verbose) {
            var text = File.ReadAllText(inputFile);
            var chrSpriteDef = SF3.CHR.SpriteDef.FromJSON(text);
            DescribeSF3CHRSprite(chrSpriteDef, verbose);
        }

        private static void DescribeSF3CHRSprite(SF3.CHR.SpriteDef sprite, bool verbose, int? index = null) {
            var promotionLevelStr = (sprite.PromotionLevel == 0) ? "" : $", PromotionLevel={sprite.PromotionLevel}";

            var indexStr = (index.HasValue) ? $"[0x{index++:X2}] " : "";
            Logger.WriteLine($"{indexStr}{sprite.SpriteName} (SpriteID=0x{sprite.SpriteID:X2}, {sprite.Width}x{sprite.Height}, Dirs={sprite.Directions}{promotionLevelStr}):");

            using (Logger.IndentedSection()) {
                try {
                    if (verbose && sprite.FrameGroupsForSpritesheets != null) {
                        Logger.WriteLine("Frames:");
                        using (Logger.IndentedSection()) {
                            int frameIndex = 0;
                            foreach (var fgss in sprite.FrameGroupsForSpritesheets) {
                                var spriteName = fgss.SpriteName ?? sprite.SpriteName;
                                var width  = fgss.Width  ?? sprite.Width;
                                var height = fgss.Height ?? sprite.Height;

                                var spriteStr = (spriteName != sprite.SpriteName) ? $", {sprite.SpriteName}" : "";
                                var sizeStr = (width != sprite.Width || height != sprite.Height) ? $", {width}x{height}" : "";
                                var prefix = spriteStr + sizeStr;

                                prefix = (prefix.Length > 0) ? $"({prefix.Substring(2)}) " : "";

                                foreach (var frameGroup in fgss.FrameGroups ?? new SF3.CHR.FrameGroup[0]) {
                                    if (frameGroup.Frames == null) {
                                        var dirs = SpriteDirectionCountTypeExtensions.ToAnimationFrameDirections(sprite.Directions);
                                        foreach (var dir in dirs)
                                            Logger.WriteLine($"[0x{frameIndex++:X2}, in group]: {prefix}{frameGroup.Name} ({dir})");
                                    }
                                    else {
                                        foreach (var frame in frameGroup.Frames)
                                            Logger.WriteLine($"[0x{frameIndex++:X2}, specific]: {prefix}{frameGroup.Name} ({frame.Direction})");
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
                                var spriteName = assd.SpriteName ?? sprite.SpriteName;
                                var width  = assd.Width  ?? sprite.Width;
                                var height = assd.Height ?? sprite.Height;
                                var dirs = assd.Directions ?? sprite.Directions;

                                var spriteStr = (spriteName != sprite.SpriteName) ? $", {sprite.SpriteName}" : "";
                                var sizeStr = (width != sprite.Width || height != sprite.Height) ? $", {width}x{height}" : "";
                                var dirsStr = (dirs != sprite.Directions) ? $", Dirs={dirs}" : "";
                                var prefix = spriteStr + sizeStr + dirsStr;

                                prefix = (prefix.Length > 0) ? $"({prefix.Substring(2)}) " : "";

                                foreach (var animation in assd.Animations ?? new string[0]) {
                                    Logger.WriteLine($"[0x{animationIndex++:X2}]: {prefix}{animation}");
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
 
            var sizeStr = (spriteDef.Width.HasValue && spriteDef.Height.HasValue) ? $" (DefaultSize={spriteDef.Width.Value}x{spriteDef.Height.Value})" : "";
            Logger.WriteLine($"{spriteDef.Name}{sizeStr}");
            using (Logger.IndentedSection()) {
                if ((spriteDef.Spritesheets?.Count ?? 0) == 0) {
                    Logger.WriteLine("No spritesheets", LogType.Warning);
                    return;
                }

                // TODO: more null checks
                if (verbose) {
                    Logger.WriteLine("Frames:");
                    using (Logger.IndentedSection()) {
                        var frames = spriteDef.Spritesheets
                            .Select(x => (Size: Spritesheet.KeyToDimensions(x.Key), x.Value))
                            .SelectMany(x => x.Value.FrameGroupsByName
                                .SelectMany(y => y.Value.Frames
                                    .Select(z => (x.Size.Width, x.Size.Height, Name: y.Key, Direction: z.Key))
                                )
                            )
                            .ToArray();

                        var frameGroups = frames
                            .GroupBy(x => $"{x.Name} ({x.Width}x{x.Height})")
                            .ToDictionary(x => x.Key, x => x.ToArray());

                        bool IsCompleteGroup((int Width, int Height, string Name, SpriteFrameDirection Direction)[] frames) {
                            var dirs = SpriteDirectionCountTypeExtensions.ToSpritesheetDirections((SpriteDirectionCountType) frames.Length);
                            if (dirs.Length != frames.Length)
                                return false;
                            for (int i = 0; i < frames.Length; i++)
                                if (frames[i].Direction != dirs[i])
                                    return false;
                            return true;
                        }

                        foreach (var frameGroupKv in frameGroups) {
                            var frames2 = frameGroupKv.Value;
                            var width   = frames2[0].Width;
                            var height  = frames2[0].Height;
                            var frameSizeStr = (width != spriteDef.Width || height != spriteDef.Height) ? $"({width}x{height}) " : "";

                            if (IsCompleteGroup(frameGroupKv.Value))
                                Logger.WriteLine($"{frameSizeStr}{frames2[0].Name} (Dirs={(SpriteDirectionCountType) frames2.Length})");
                            else {
                                foreach (var frame in frames2)
                                    Logger.WriteLine($"{frameSizeStr}{frame.Name} ({frame.Direction})");
                            }
                        }
                    }
                }

                // TODO: more null checks
                Logger.WriteLine("Animations:");
                using (Logger.IndentedSection()) {
                    var animations = spriteDef.Spritesheets
                        .Select(x => (Size: Spritesheet.KeyToDimensions(x.Key), x.Value))
                        .SelectMany(x => x.Value.AnimationSetsByDirections
                            .SelectMany(y => y.Value.AnimationsByName
                                .Select(z => (x.Size.Width, x.Size.Height, Directions: y.Key, Name: z.Key))
                            )
                        )
                        .ToArray();

                    foreach (var ani in animations) {
                        var aniSizeStr = (ani.Width != spriteDef.Width || ani.Height != spriteDef.Height) ? $"{ani.Width}x{ani.Height}, " : "";
                        Logger.WriteLine($"({aniSizeStr}Dirs={ani.Directions}) {ani.Name}");
                    }
                }
            }
        }
    }
}
