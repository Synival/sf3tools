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

namespace SpriteExtractor {
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

            public override string ToString() => FrameInfo.ToString();

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

            public override string ToString() => AnimInfo.ToString();

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

            var lastFrame = animation.AnimationFrames.LastOrDefault();
            var lastFrameWord = (lastFrame == null) ? 0 : (lastFrame.FrameID << 8) | lastFrame.Duration;

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

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| UPDATING SPRITE HEADER INFO                     |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            var spriteDefs = CHR_Utils.CreateAllSpriteDefs();
            var spriteInfos = CHR_Utils.GetUniqueSpriteInfos().OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

            T MostCommonKey<T>(Dictionary<T, int> dict)
                => dict.OrderByDescending(x => x.Value).ThenBy(x => x.Key).First().Key;

            foreach (var spriteDef in spriteDefs) {
                foreach (var spritesheet in spriteDef.Spritesheets) {
                    var key = $"{spriteDef.Name} ({spritesheet.Key})";

                    // Some spirtes have the wrong key because their animations are a different size than its header.
                    if (key == "Fey (Quonus Priest) (40x48)")
                        key = "Fey (Quonus Priest) (40x40)";
                    if (key == "Zero (U) (48x32)")
                        key = "Zero (U) (48x24)";

                    if (spriteInfos.ContainsKey(key)) {
                        var spriteInfo = spriteInfos[key];

                        spritesheet.Value.VerticalOffset = MostCommonKey(spriteInfo.VerticalOffsetValueCount);
                        spritesheet.Value.Unknown0x08    = MostCommonKey(spriteInfo.Unknown0x08Count);
                        spritesheet.Value.CollisionSize  = MostCommonKey(spriteInfo.CollisionSizeCount);
                        spritesheet.Value.Scale          = (int) MostCommonKey(spriteInfo.ScaleCount);
                    }
                }
            }

            // For some reason, we have to add "Nothing (Broken, No Frames)" to the "None" spritedef.
            spriteDefs.First(x => x.Name == "None").Spritesheets.First().Value.AnimationByDirections.First().Value.Animations.Add(
                "Nothing (Broken, No Frames)",
                new AnimationDef() { AnimationFrames = [] }
            );

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

                List<StandaloneFrameDef> framesFound = [];
                foreach (var spritesheet in spriteDef.Spritesheets) {
                    var spriteName = $"{spriteDef.Name} ({spritesheet.Key})";
                    var frameSize = SpritesheetDef.KeyToDimensions(spritesheet.Key);

                    var frames = spritesheet.Value.FrameGroups
                        .SelectMany(x => x.Value.Frames
                            .Select(y => new StandaloneFrameDef(y.Value, Enum.Parse<SpriteFrameDirection>(y.Key), x.Key, frameSize.Width, frameSize.Height))
                        )
                        .Where(x => s_framesByHash.ContainsKey(x.Hash))
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
                    bool hasErrors = false;
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
                        hasErrors |= hasDuplicateDirections;

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
                                if (false /* hasDuplicateDirections || hasBogusDirs */) {
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
                    Console.WriteLine($"Writing '{outputPath}'...");
                    using (var bitmap = new Bitmap(imageWidthInPixels, imageHeightInPixels, PixelFormat.Format16bppArgb1555)) {
                        var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                        Marshal.Copy(newData, 0, bmpData.Scan0, newData.Length);
                        bitmap.UnlockBits(bmpData);
                        try {
                            _ = Directory.CreateDirectory(Path.Combine(c_pathOut, FilesystemString(spriteDef.Name)));
                            bitmap.Save(outputPath);
                        }
                        catch { }
                    }
                }
#pragma warning restore CA1416 // Validate platform compatibility

                // Unless we have sprite def for "None", which has no frames, eliminate some empty frames/animations.
                if (spriteDef.Name != "None") {
                    // Filter out frames that don't have image data that made it to a sprite sheet.
                    framesFound = framesFound
                        .Distinct()
                        .OrderBy(x => x.Name)
                        .ThenBy(x => x.Direction)
                        .ThenBy(x => x.Hash)
                        .ToList();

                    // Filter out sprite sheets and variants that have no frames.
                    spriteDef.Spritesheets = framesFound
                        .OrderBy(x => x.Width)
                        .ThenBy(x => x.Height)
                        .OrderBy(x => x.Name)
                        .ThenBy(x => x.Direction)
                        .GroupBy(x => SpritesheetDef.DimensionsToKey(x.Width, x.Height))
                        .ToDictionary(x => x.Key, x => {
                            var size = SpritesheetDef.KeyToDimensions(x.Key);
                            var existingSpritesheet = spriteDef.Spritesheets[x.Key];
                            return new SpritesheetDef(
                                x.ToArray(),
                                spriteDef.Spritesheets[x.Key].AnimationByDirections
                                    .Where(y => framesFound.Any(z => size.Width == z.Width && size.Height == z.Height))
                                    .OrderBy(y => y.Key)
                                    .ToDictionary(y => y.Key, y => y.Value)
                            ) {
                                CollisionSize  = existingSpritesheet.CollisionSize,
                                Scale          = existingSpritesheet.Scale,
                                Unknown0x08    = existingSpritesheet.Unknown0x08,
                                VerticalOffset = existingSpritesheet.VerticalOffset,
                            };
                        });

                    var framesFoundHashSet = framesFound.Select(x => x.Hash).ToHashSet();

                    // Filter out animations that have missing frames or no frames at all.
                    foreach (var spritesheet in spriteDef.Spritesheets.Values) {
                        foreach (var variant in spritesheet.AnimationByDirections) {
                            var validAnimations = variant.Value.Animations
                                .Where(x => x.Value.AnimationFrames.Length > 0 && x.Value.AnimationFrames
                                    .All(y => y.FrameHashes == null || y.FrameHashes.All(z => z == null || framesFoundHashSet.Contains(z)))
                                )
                                .OrderBy(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);
                            variant.Value.Animations = validAnimations;
                        }
                    }
                }
            }
            Console.WriteLine("Spritesheet writing complete.");

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITE DEFS                            |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            var jsonSettings = new JsonSerializerSettings() {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,                    
            };

            foreach (var spriteDef in spriteDefs) {
                var spritePath = Path.Combine(c_pathOut, FilesystemString(spriteDef.Name));
                var spriteDefPath = Path.Combine(spritePath, FilesystemString(spriteDef.Name) + ".SF3Sprite");
                Console.WriteLine($"Writing '{spriteDefPath}'...");

                _ = Directory.CreateDirectory(Path.Combine(c_pathOut, FilesystemString(spriteDef.Name)));
                using (var file = File.Open(spriteDefPath, FileMode.Create)) {
                    using (var stream = new StreamWriter(file)) {
                        stream.NewLine = "\n";
                        stream.Write(JsonConvert.SerializeObject(spriteDef, jsonSettings));
                    }
                }
            }
            Console.WriteLine("Sprite def writing complete.");

            Console.WriteLine();
            Console.WriteLine("===================================================");
            Console.WriteLine("| CHECKING FOR MISSING ANIMATIONS / FRAMES        |");
            Console.WriteLine("===================================================");
            Console.WriteLine();

            var framesAccountedFor = new HashSet<string>();
            var animationsAccountedFor = new HashSet<string>();

            var frameTexturesByHash = s_framesByHash
                .ToDictionary(x => x.Key, x => x.Value.Texture);

            foreach (var spriteDef in spriteDefs) {
                foreach (var spritesheet in spriteDef.Spritesheets.Values) {
                    foreach (var frameGroup in spritesheet.FrameGroups)
                        foreach (var frame in frameGroup.Value.Frames)
                            framesAccountedFor.Add(frame.Value.Hash);

                    foreach (var animationGroup in spritesheet.AnimationByDirections)
                        foreach (var animation in animationGroup.Value.Animations)
                            animationsAccountedFor.Add(CHR_Utils.CreateAnimationHash(animationGroup.Key, animation.Value, spritesheet.FrameGroups, frameTexturesByHash));
                }
            }

            foreach (var frame in s_framesByHash) {
                if (!framesAccountedFor.Contains(frame.Key)) {
                    var fi = frame.Value.FrameInfo;
                    Console.WriteLine($"Frame unaccounted for: {fi.SpriteName}.{fi.FrameName}.{fi.Direction} ({fi.TextureHash})");
                }
            }

            foreach (var animation in s_animationsByHash) {
                if (!animationsAccountedFor.Contains(animation.Key)) {
                    var ai = animation.Value.AnimInfo;
                    Console.WriteLine($"Animation unaccounted for: {ai.SpriteName}.{ai.AnimationName} ({ai.AnimationHash})");
                }
            }
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
