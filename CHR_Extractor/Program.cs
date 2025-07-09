using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using Newtonsoft.Json;
using SF3;
using SF3.ByteData;
using SF3.Models.Files;
using SF3.Models.Files.CHP;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;
using SF3.NamedValues;
using SF3.Sprites;
using SF3.Types;
using SF3.Utils;

namespace CHR_Analyzer {
    public class Program {
        // ,--- Enter the paths for all your CHR files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        private const string c_pathOut = "../../../Private";

        private class TextureInfo {
            public TextureInfo(UniqueFrameDef frameInfo, ITexture texture) {
                FrameInfo = frameInfo;
                Texture = texture;
            }

            public UniqueFrameDef FrameInfo { get; }
            public ITexture Texture { get; }
            public List<TextureSpriteInfo> Sprites { get; } = new List<TextureSpriteInfo>();
        }

        private class TextureSpriteInfo {
            public TextureSpriteInfo(ScenarioType scenario, string filename, int spriteId) {
                Scenario = scenario;
                Filename = filename;
                SpriteID = spriteId;
            }

            public readonly ScenarioType Scenario;
            public readonly string Filename;
            public readonly int SpriteID;
        }

        private class AnimationInfo {
            public AnimationInfo(UniqueAnimationDef animationInfo) {
                AnimInfo = animationInfo;
            }

            public UniqueAnimationDef AnimInfo { get; }
            public List<AnimationFileSprite> Sprites { get; } = new List<AnimationFileSprite>();
        }

        private class AnimationFileSprite {
            public AnimationFileSprite(ScenarioType scenario, string filename, int spriteIndex, int animIndex, int lastFrameWord) {
                Scenario      = scenario;
                Filename      = filename;
                SpriteIndex   = spriteIndex;
                AnimIndex     = animIndex;
                LastFrameWord = lastFrameWord;
            }

            public readonly ScenarioType Scenario;
            public readonly string Filename;
            public readonly int SpriteIndex;
            public readonly int AnimIndex;
            public readonly int LastFrameWord;
        }

        private static Dictionary<string, TextureInfo> s_framesByHash = [];
        private static Dictionary<string, AnimationInfo> s_animationsByHash = [];

        // TODO: remove and just fetch the serializable data from SF3Lib. Remove all related methods and classes.
        private static void AddFrame(ScenarioType scenario, string filename, int spriteId, Frame frame) {
            var hash = frame.Texture.Hash;
            if (!s_framesByHash.ContainsKey(hash))
                s_framesByHash.Add(hash, new TextureInfo(frame.FrameInfo, frame.Texture));
            s_framesByHash[hash].Sprites.Add(new TextureSpriteInfo(scenario, filename, spriteId));
        }

        // TODO: remove and just fetch the serializable data from SF3Lib. Remove all related methods and classes.
        private static void AddAnimation(ScenarioType scenario, string filename, int spriteIndex, Animation animation) {
            var hash = animation.Hash;
            if (!s_animationsByHash.ContainsKey(hash))
                s_animationsByHash.Add(hash, new AnimationInfo(animation.AnimationInfo));

            var lastFrame = animation.AnimationFrames.Last();
            var lastFrameWord = (lastFrame.FrameID << 8) | lastFrame.Duration;

            s_animationsByHash[hash].Sprites.Add(new AnimationFileSprite(scenario, filename, spriteIndex, animation.ID, lastFrameWord));
        }

        public static void Main(string[] args) {
            Console.WriteLine("Processing all CHR and CHP files...");

            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .Where(x => c_pathsIn.ContainsKey(x))
                .ToDictionary(x => x, x => {
                    var files = Directory.GetFiles(c_pathsIn[x], "*.CHR").Order().ToList();
                    files.AddRange(Directory.GetFiles(c_pathsIn[x], "*.CHP").Order().ToList());
                    return files;
                });

            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => (INameGetterContext) new NameGetterContext(x));

            // Open each file.
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create a CHR file that works with our new ByteData.
                    try {
                        bool isChr = file.EndsWith(".CHR");
                        using (ScenarioTableFile chrChpFile = isChr
                            ? CHR_File.Create(byteData, nameGetterContexts[scenario], scenario)
                            : CHP_File.Create(byteData, nameGetterContexts[scenario], scenario)
                        ) {
                            var chrFiles = isChr
                                ? [(CHR_File) chrChpFile]
                                : ((CHP_File) chrChpFile).CHR_EntriesByOffset.Values.ToArray();

                            // List the file and any report we may have from CHR_MatchFunc().
                            var fileStr = GetFileString(scenario, file, chrChpFile);
                            Console.WriteLine($"{fileStr}");

                            // Build a table of all textures.
                            foreach (var sprite in chrFiles.SelectMany(x => x.SpriteTable).ToArray()) {
                                foreach (var frame in sprite.FrameTable)
                                    AddFrame(scenario, filename, sprite.ID, frame);
                                foreach (var animation in sprite.AnimationTable)
                                    AddAnimation(scenario, filename, sprite.ID, animation);
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

            Console.WriteLine("Processing complete.");

            _ = Directory.CreateDirectory(c_pathOut);
            var spriteDefs = CHR_Utils.GetAllSpriteDefs();
            foreach (var spriteDef in spriteDefs)
                _ = Directory.CreateDirectory(Path.Combine(c_pathOut, FilesystemString(spriteDef.Name)));

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| WRITING EDITOR RESOURCES                        |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            Console.WriteLine("Writing new 'SpriteFramesByHash.xml'...");
            using (var file = File.Open(Path.Combine(c_pathOut, "SpriteFramesByHash.xml"), FileMode.Create))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueFramesByHashXML(stream);

            Console.WriteLine("Writing new 'SpriteAnimationsByHash.xml'...");
            using (var file = File.Open(Path.Combine(c_pathOut, "SpriteAnimationsByHash.xml"), FileMode.Create))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueAnimationsByHashXML(stream);

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITESHEETS                           |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            foreach (var spriteDef in spriteDefs) {
                var spritePath = Path.Combine(c_pathOut, FilesystemString(spriteDef.Name));
                var spriteSheetVariants = spriteDef.Variants
                    .GroupBy(x => (x.Width << 16) + x.Height)
                    .Select(x => x.First())
                    .ToArray();

                List<FrameDef> framesFound = [];
                foreach (var variantDef in spriteSheetVariants) {
                    var spriteName = $"{spriteDef.Name} ({variantDef.Width}x{variantDef.Height})";

                    var frames = spriteDef.Frames
                        .Where(x => x.Width == variantDef.Width && x.Height == variantDef.Height && s_framesByHash.ContainsKey(x.Hash))
                        .Select(x => new { SpriteDefFrame = x, s_framesByHash[x.Hash].Texture, s_framesByHash[x.Hash].FrameInfo })
                        .OrderBy(x => x.FrameInfo.Width)
                        .ThenBy(x => x.FrameInfo.Height)
                        .ThenBy(x => x.FrameInfo.FrameName)
                        .ThenBy(x => x.FrameInfo.Direction)
                        .ThenBy(x => x.FrameInfo.TextureHash)
                        .ToArray();

                    if (frames.Length == 0)
                        continue;

                    framesFound.AddRange(frames.Select(x => x.SpriteDefFrame));

                    var frameDirections = frames.Select(x => x.FrameInfo.Direction)
                        .Distinct()
                        .ToHashSet();

                    var frameDirectionToIndex = frameDirections
                        .OrderBy(x => x)
                        .Select((x, i) => new { Direction = x, Index = i })
                        .OrderBy(x => x.Direction)
                        .ToDictionary(x => x.Direction, x => x.Index);

                    var filename = FilesystemString(spriteName) + ".BMP";
                    var outputPath = Path.Combine(spritePath, filename);
                    Console.WriteLine($"Writing '{outputPath}'...");

                    var frameGroups = frames
                        .GroupBy(x => x.FrameInfo.FrameName)
                        .ToDictionary(x => x.Key, x => x.OrderBy(y => y.FrameInfo.Direction).ThenBy(y => y.FrameInfo.TextureHash).ToArray());

                    var frameWidthInPixels  = frames[0].Texture.Width;
                    var frameHeightInPixels = frames[0].Texture.Height;

                    var pixelsPerFrame = frameWidthInPixels * frameHeightInPixels;
                    var frameCount     = frames.Length;
                    var totalPixels    = pixelsPerFrame * frameCount;

                    var imageWidthInFrames  = Math.Max(frameDirections.Count, frameGroups.Max(x => x.Value.Length));
                    var imageHeightInFrames = frameGroups.Count;

                    var imageWidthInPixels  = imageWidthInFrames * frameWidthInPixels;
                    var imageHeightInPixels = imageHeightInFrames * frameHeightInPixels;

                    var newData = new byte[imageWidthInPixels * imageHeightInPixels * 2];

                    int y = 0;
                    foreach (var frameGroup in frameGroups) {
                        // Normally, frames' X positions are set according to their direction.
                        // But if a frame group had multiple directions -- which, unfortunately, can happen --
                        // set the frames' X position to their index.
                        bool hasDuplicateDirections = frameGroup.Value
                            .GroupBy(x => x.FrameInfo.Direction)
                            .Any(x => x.Count() != 1);

                        // Determine if this is a valid set of frames.
                        var frameGroupDirs = frameGroup.Value.Select(x => x.FrameInfo.Direction).Distinct().ToHashSet();

                        var hasFirst  = frameGroupDirs.Contains(SpriteFrameDirection.First);
                        var hasSecond = frameGroupDirs.Contains(SpriteFrameDirection.Second);

                        var hasS   = frameGroupDirs.Contains(SpriteFrameDirection.S);
                        var hasSSE = frameGroupDirs.Contains(SpriteFrameDirection.SSE);
                        var hasSE  = frameGroupDirs.Contains(SpriteFrameDirection.SE);
                        var hasESE = frameGroupDirs.Contains(SpriteFrameDirection.ESE);
                        var hasE   = frameGroupDirs.Contains(SpriteFrameDirection.E);
                        var hasENE = frameGroupDirs.Contains(SpriteFrameDirection.ENE);
                        var hasNE  = frameGroupDirs.Contains(SpriteFrameDirection.NE);
                        var hasNNE = frameGroupDirs.Contains(SpriteFrameDirection.NNE);
                        var hasN   = frameGroupDirs.Contains(SpriteFrameDirection.N);
                        var hasNNW = frameGroupDirs.Contains(SpriteFrameDirection.NNW);
                        var hasWNW = frameGroupDirs.Contains(SpriteFrameDirection.WNW);
                        var hasWSW = frameGroupDirs.Contains(SpriteFrameDirection.WSW);
                        var hasSSW = frameGroupDirs.Contains(SpriteFrameDirection.SSW);

                        var has1Dir  = frameGroupDirs.Count == 1 && hasFirst;
                        var has2Dirs = frameGroupDirs.Count == 2 && hasFirst && hasSecond;
                        var has4Dirs = frameGroupDirs.Count == 4         && hasSSE && hasESE && hasENE && hasNNE;
                        var has5Dirs = frameGroupDirs.Count == 5 && hasS      && hasSE  && hasE   && hasNE       && hasN;
                        var has6Dirs = frameGroupDirs.Count == 6 && hasS && hasSSE && hasESE && hasENE && hasNNE && hasN;
                        var has8Dirs = frameGroupDirs.Count == 8         && hasSSE && hasESE && hasENE && hasNNE         && hasNNW && hasWNW && hasWSW && hasSSW;
                        var has9Dirs = frameGroupDirs.Count == 9 && hasS && hasSSE && hasSE && hasESE && hasE && hasENE && hasNE && hasNNE && hasN;

                        var hasBogusDirs = !(has1Dir || has2Dirs || has4Dirs || has5Dirs || has6Dirs || has8Dirs || has9Dirs);

                        int frameIndex = 0;
                        foreach (var frame in frameGroup.Value) {
                            int x = ((hasDuplicateDirections) ? frameIndex : frameDirectionToIndex[frame.FrameInfo.Direction]) * frameWidthInPixels;
                            frame.SpriteDefFrame.SpriteSheetX = x;
                            frame.SpriteDefFrame.SpriteSheetY = y;

                            int pos = (y * imageWidthInPixels + x) * 2;
                            var frameData = frame.Texture.BitmapDataARGB1555;

                            int frameDataPos = 0;
                            for (int iy = 0; iy < frameHeightInPixels; iy++) {
                                int ipos = pos + (iy * imageWidthInPixels) * 2;

                                // If there are errors with this frame group, colorize them with a background color.
                                if (hasDuplicateDirections || hasBogusDirs) {
                                    for (int ix = 0; ix < frameWidthInPixels; ix++) {
                                        var argb1555 = (ushort) ((frameData[frameDataPos + 1] << 8) + frameData[frameDataPos]);
                                        if (argb1555 < 0x8000u) {
                                            newData[ipos++] = (byte) (hasBogusDirs ? 0x1F : 0x00);
                                            newData[ipos++] = (byte) (0x80 | (hasDuplicateDirections ? 0x7C : 0x00));
                                            frameDataPos += 2;
                                        }
                                        else {
                                            newData[ipos++] = frameData[frameDataPos++];
                                            newData[ipos++] = frameData[frameDataPos++];
                                        }
                                    }
                                }
                                else {
                                    for (int ix = 0; ix < frameWidthInPixels * 2; ix++)
                                        newData[ipos++] = frameData[frameDataPos++];
                                }
                            }

                            frameIndex++;
                        }
                        y += frameHeightInPixels;
                    }

#pragma warning disable CA1416 // Validate platform compatibility
                    using (var bitmap = new Bitmap(imageWidthInPixels, imageHeightInPixels, PixelFormat.Format16bppArgb1555)) {
                        var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(newData, 0, bmpData.Scan0, newData.Length);
                        bitmap.UnlockBits(bmpData);
                        try {
                            bitmap.Save(outputPath);
                        }
                        catch { }
                    }
                }
#pragma warning restore CA1416 // Validate platform compatibility

                // Filter out frames that don't have image data that made it to a sprite sheet.
                framesFound = framesFound
                    .Distinct()
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.Direction)
                    .ThenBy(x => x.Hash)
                    .ToList();
                spriteDef.Frames = framesFound.ToArray();
                var framesFoundHashSet = spriteDef.Frames.Select(x => x.Hash).ToHashSet();

                // Filter out variants that have no frames.
                var validVariants = spriteDef.Variants
                    .Where(x => spriteDef.Frames.Any(y => x.Width == y.Width && x.Height == y.Height))
                    .ToArray();
                spriteDef.Variants = validVariants;

                // Filter out animations that have missing frames or no frames at all.
                foreach (var variant in spriteDef.Variants) {
                    var validAnimations = variant.Animations
                        .Where(x => x.AnimationFrames.Length > 0 && x.AnimationFrames
                            .All(y => y.FrameHashes == null || y.FrameHashes.All(z => z == null || framesFoundHashSet.Contains(z)))
                        )
                        .ToArray();
                    variant.Animations = validAnimations;
                }
            }
            Console.WriteLine("Spritesheet writing complete.");

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITE DEFS                            |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            foreach (var spriteDef in spriteDefs) {
                spriteDef.Frames = spriteDef.Frames
                    .OrderBy(x => x.SpriteSheetY)
                    .ThenBy(x => x.SpriteSheetX)
                    .ToArray();

                var spritePath = Path.Combine(c_pathOut, FilesystemString(spriteDef.Name));
                var spriteDefPath = Path.Combine(spritePath, FilesystemString(spriteDef.Name) + ".json");
                Console.WriteLine($"Writing '{spriteDefPath}'...");

                if (spriteDef.Frames.Length == 0) {
                    Console.WriteLine($"    Skipping -- contains no frames");
                    continue;
                }

                using (var file = File.Open(spriteDefPath, FileMode.Create)) {
                    using (var stream = new StreamWriter(file)) {
                        stream.NewLine = "\n";
                        stream.Write(JsonConvert.SerializeObject(spriteDef, Formatting.Indented));
                    }
                }
            }
            Console.WriteLine("Sprite def writing complete.");
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ScenarioTableFile chrChpFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }

        private static string FilesystemString(string str) {
            return str
                .Replace(" | ", ", ")
                .Replace("|", ",")
                .Replace("?", "X")
                .Replace("-", "_")
                .Replace(":", "_")
                .Replace("/", "_");
        }
    }
}
