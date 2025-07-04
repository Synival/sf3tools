using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Arrays;
using CommonLib.NamedValues;
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
            public TextureInfo(UniqueFrameInfo frameInfo, ITexture texture) {
                FrameInfo = frameInfo;
                Texture = texture;
            }

            public UniqueFrameInfo FrameInfo { get; }
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
            public AnimationInfo(UniqueAnimationInfo animationInfo) {
                AnimInfo = animationInfo;
            }

            public UniqueAnimationInfo AnimInfo { get; }
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

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| WRITING METADATA                                |");
            Console.WriteLine("===================================================");

            Console.WriteLine();
            Console.WriteLine("Writing new 'SpriteFramesByHash.xml'...");
            _ = Directory.CreateDirectory(c_pathOut);
            using (var file = File.OpenWrite(Path.Combine(c_pathOut, "SpriteFramesByHash.xml")))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueFramesByHashXML(stream);

            Console.WriteLine("Writing new 'SpriteAnimationsByHash.json'...");
            _ = Directory.CreateDirectory(c_pathOut);
            using (var file = File.Open(Path.Combine(c_pathOut, "SpriteAnimationsByHash.json"), FileMode.Create))
                using (var stream = new StreamWriter(file))
                    CHR_Utils.WriteUniqueAnimationsByHashJSON(stream);

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| CREATING SPRITESHEETS                           |");
            Console.WriteLine("===================================================");
            Console.WriteLine("");

            var spriteSheets = s_framesByHash.Values
                .OrderBy(x => x.FrameInfo.SpriteName)
                .ThenBy(x => x.FrameInfo.Width)
                .ThenBy(x => x.FrameInfo.Height)
                .ThenBy(x => x.FrameInfo.FrameName)
                .ThenBy(x => x.FrameInfo.Direction)
                .ThenBy(x => x.FrameInfo.TextureHash)
                .GroupBy(x => $"{x.FrameInfo.SpriteName} ({x.FrameInfo.Width}x{x.FrameInfo.Height})")
                .ToDictionary(x => x.Key, x => x.ToArray());

            _ = Directory.CreateDirectory(c_pathOut);
            foreach (var spriteSheetKv in spriteSheets) {
                var spriteName = spriteSheetKv.Key;
                var frames = spriteSheetKv.Value;

                var filename = spriteName
                    .Replace("?", "X")
                    .Replace("-", "_")
                    .Replace(":", "_")
                    .Replace("/", "_") + ".BMP";

                var outputPath = Path.Combine(c_pathOut, filename);
                Console.WriteLine("Writing: " + outputPath);

                var frameGroups = frames
                    .GroupBy(x => x.FrameInfo.FrameName)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var frameWidthInPixels  = frames[0].Texture.Width;
                var frameHeightInPixels = frames[0].Texture.Height;

                var pixelsPerFrame = frameWidthInPixels * frameHeightInPixels;
                var frameCount     = frames.Length;
                var totalPixels    = pixelsPerFrame * frameCount;

                var imageWidthInFrames = frameGroups.Max(x => x.Value.Length);
                var imageHeightInFrames = frameGroups.Count;

                var imageWidthInPixels = imageWidthInFrames * frameWidthInPixels;
                var imageHeightInPixels = imageHeightInFrames * frameHeightInPixels;

                var newData = new byte[imageWidthInPixels * imageHeightInPixels * 2];

                int y = 0;
                foreach (var frameGroup in frameGroups) {
                    int x = (imageWidthInFrames - frameGroup.Value.Length) * frameWidthInPixels / 2;
                    foreach (var frame in frameGroup.Value) {
                        int pos = (y * imageWidthInPixels + x) * 2;
                        var frameData = frame.Texture.BitmapDataARGB1555;

                        int frameDataPos = 0;
                        for (int iy = 0; iy < frameHeightInPixels; iy++) {
                            int ipos = pos + (iy * imageWidthInPixels) * 2;
                            for (int ix = 0; ix < frameWidthInPixels * 2; ix++)
                                newData[ipos++] = frameData[frameDataPos++];
                        }
                        x += frameWidthInPixels;
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
#pragma warning restore CA1416 // Validate platform compatibility
            }
            Console.WriteLine("Image dumping complete.");
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ScenarioTableFile chrChpFile) {
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12);
        }
    }
}
