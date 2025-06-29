﻿using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using CommonLib.Arrays;
using CommonLib.NamedValues;
using SF3;
using SF3.ByteData;
using SF3.Models.Files.CHR;
using SF3.Models.Structs.CHR;
using SF3.NamedValues;
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
            public TextureInfo(FrameTextureInfo frameInfo, ITexture texture) {
                FrameInfo = frameInfo;
                Texture = texture;
            }

            public FrameTextureInfo FrameInfo { get; }
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
            public AnimationInfo() {
            }

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

        private static void AddFrame(ScenarioType scenario, string filename, int spriteId, Frame frame) {
            var hash = frame.Texture.Hash;
            if (!s_framesByHash.ContainsKey(hash))
                s_framesByHash.Add(hash, new TextureInfo(frame.FrameInfo, frame.Texture));
            s_framesByHash[hash].Sprites.Add(new TextureSpriteInfo(scenario, filename, spriteId));
        }

        private static void AddAnimation(ScenarioType scenario, string filename, int spriteIndex, Animation animation) {
            var hash = animation.Hash;
            if (!s_animationsByHash.ContainsKey(hash))
                s_animationsByHash.Add(hash, new AnimationInfo());

            var lastFrame = animation.AnimationFrames.Last();
            var lastFrameWord = (lastFrame.FrameID << 8) | lastFrame.Duration;

            s_animationsByHash[hash].Sprites.Add(new AnimationFileSprite(scenario, filename, spriteIndex, animation.ID, lastFrameWord));
        }

        private static List<string> s_matchReports = [];

        private static HashSet<string> scn1List = new HashSet<string>() {
            "Eldar (P2)",
        };

        private static bool? CHR_MatchFunc(string filename, ICHR_File chrFile, INameGetterContext ngc) {
            var animationsWithMissingFrames = chrFile.SpriteTable.SelectMany(x => x.AnimationTable.Where(y => y.TotalFramesMissing > 0)).ToArray();
            foreach (var x in animationsWithMissingFrames)
                s_matchReports.Add($"{x.TotalFramesMissing} missing frames | {x.SpriteName}, {x.Name}");
            return s_matchReports.Count > 0;
        }

        public static void Main(string[] args) {
            Console.WriteLine("Press a key to start processing CHR/CHP files...");
            _ = Console.ReadKey();
            Console.WriteLine("Processing...");

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
            var matchSet   = new List<string>();
            var nomatchSet = new List<string>();

            var frameWordSet = new HashSet<uint>();

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create a CHR file that works with our new ByteData.
                    try {
                        using (var chrFile = CHR_File.Create(byteData, nameGetterContexts[scenario], scenario, file.EndsWith(".CHP"))) {
                            var match = CHR_MatchFunc(filename, chrFile, nameGetterContexts[scenario]);

                            // If the match is 'null', that means we're just skipping this file completely.
                            if (match == null) {
                                s_matchReports.Clear();
                                continue;
                            }

                            // List the file and any report we may have from CHR_MatchFunc().
                            var fileStr = GetFileString(scenario, file, chrFile);
                            Console.WriteLine($"{fileStr} | {match}");
                            foreach (var mr in s_matchReports)
                                Console.WriteLine("    " + mr);
                            s_matchReports.Clear();

                            if (match == true)
                                matchSet.Add(fileStr);
                            else
                                nomatchSet.Add(fileStr);

                            ScanForErrorsAndReport(scenario, chrFile);

                            // Build a table of all textures.
                            foreach (var sprite in chrFile.SpriteTable) {
                                foreach (var frame in sprite.FrameTable)
                                    AddFrame(chrFile.Scenario, filename, sprite.ID, frame);
                                foreach (var animation in sprite.AnimationTable) {
                                    AddAnimation(chrFile.Scenario, filename, sprite.ID, animation);
                                    foreach (var animFrame in animation.AnimationFrames)
                                        if (animFrame.FrameID >= 0xF0)
                                            frameWordSet.Add((uint) ((animFrame.FrameID << 16) | animFrame.Duration));
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }
            Console.WriteLine("Processing complete.");

            // Report any sprite frames with mixed sizes.
            var allTexturesByName = s_framesByHash.Values
                .GroupBy(x => $"{x.FrameInfo.SpriteName} ({x.FrameInfo.Width}x{x.FrameInfo.Height})")
                .ToDictionary(x => x.Key, x => x.ToArray());

            var texturesWithMultipleSizes = allTexturesByName
                .Where(x => !x.Value.All(y => y.Texture.Width == x.Value[0].Texture.Width && y.Texture.Height == x.Value[0].Texture.Height))
                .OrderBy(x => x.Key)
                .ToDictionary(x => x.Key, x => x.Value);

            Console.WriteLine("Sprites with multiple sizes in their frames:");
            foreach (var textureKv in texturesWithMultipleSizes) {
                var sizes = textureKv.Value
                    .Select(x => new { x.Texture.Width, x.Texture.Height })
                    .GroupBy(x => x.Width * 0x1000 + x.Height)
                    .OrderByDescending(x => x.Count())
                    .ToDictionary(x => x.First(), x => x.Count());

                Console.WriteLine("  " + textureKv.Key + ": " + string.Join(", ", sizes.Select(x => $"{x.Key.Width}x{x.Key.Height}[x{x.Value}]")));
                var first = sizes.First().Key;

                var texturesToFix = textureKv.Value
                    .Where(x => x.Texture.Width != first.Width || x.Texture.Height != first.Height)
                    .OrderBy(x => x.Texture.Width)
                    .ThenBy(x => x.Texture.Height)
                    .ThenBy(x => x.FrameInfo.TextureHash)
                    .ToArray();

                foreach (var tex in texturesToFix)
                    Console.WriteLine($"    {tex.FrameInfo.TextureHash} ({tex.Texture.Width}x{tex.Texture.Height})");
            }

/*
            Console.WriteLine("Writing new 'SpriteFramesByHash.xml'...");
            _ = Directory.CreateDirectory(c_pathOut);
            using (var file = File.OpenWrite(Path.Combine(c_pathOut, "SpriteFramesByHash.xml")))
                using (var stream = new StreamWriter(file))
                    SpriteFrameTextueUtils.WriteSpriteFramesByHashXML(stream);
*/

            var totalCount = matchSet.Count + nomatchSet.Count;

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| MATCH RESULTS                                   |");
            Console.WriteLine("===================================================");

            Console.WriteLine("");
            Console.WriteLine($"Match: {matchSet.Count}/{totalCount}");
            foreach (var str in matchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine($"NoMatch: {nomatchSet.Count}/{totalCount}");
            foreach (var str in nomatchSet)
                Console.WriteLine("  " + str);

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| ANIMATIONS                                      |");
            Console.WriteLine("===================================================");
            Console.WriteLine("");

            Console.WriteLine($"There are {s_animationsByHash.Count} unique animations.");
            var avgUsagesPerAnimation = s_animationsByHash.Sum(x => x.Value.Sprites.Count) / (float) s_animationsByHash.Count;
            Console.WriteLine($"Each animation is used {avgUsagesPerAnimation} times on average.");

            return;

            Console.WriteLine("");
            Console.WriteLine("===================================================");
            Console.WriteLine("| DUMPING IMAGES                                  |");
            Console.WriteLine("===================================================");
            Console.WriteLine("");

            var texInfos = s_framesByHash.Values
                .OrderBy(x => x.FrameInfo.SpriteName)
                .ThenBy(x => x.FrameInfo.Width)
                .ThenBy(x => x.FrameInfo.Height)
                .ThenBy(x => x.FrameInfo.AnimationName)
                .ThenBy(x => x.FrameInfo.TextureHash)
                .ToArray();

            _ = Directory.CreateDirectory(c_pathOut);
            foreach (var texInfo in texInfos) {
                var tex = texInfo.Texture;
                var fi = texInfo.FrameInfo;

                var spriteFileString = fi.SpriteName
                    .Replace("?", "X")
                    .Replace("-", "_")
                    .Replace(":", "_")
                    .Replace("/", "_");

                var path = Path.Combine(c_pathOut, spriteFileString, $"{fi.Width}x{fi.Height}", (fi.AnimationName == "") ? "Uncategorized" : fi.AnimationName.Replace('|', '_'));
                _ = Directory.CreateDirectory(path);

                var outputPath = Path.Combine(path, $"{fi.TextureHash}.BMP");
                Console.WriteLine("Writing: " + outputPath);
#pragma warning disable CA1416 // Validate platform compatibility
                using (var bitmap = new Bitmap(tex.Width, tex.Height, PixelFormat.Format16bppArgb1555)) {
                    var imageData = tex.BitmapDataARGB1555;
                    var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
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

        private static string BitString(uint bits) {
            var str = "";
            for (var i = 0; i < 32; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000_0000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string BitString(ushort bits) {
            var str = "";
            for (var i = 0; i < 16; i++) {
                if (i % 4 == 0 && i != 0)
                    str += ",";
                str += (bits & (0x8000 >> i)) != 0 ? "1" : "0";
            }
            return str;
        }

        private static string GetFileString(ScenarioType inputScenario, string filename, ICHR_File chrFile) {
            var typeStr = chrFile.IsCHP ? "CHP" : "CHR";
            return inputScenario.ToString().PadLeft(11) + ": " + Path.GetFileName(filename).PadLeft(12) + " | " + typeStr;
        }

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, ICHR_File chrFile) {
            var totalErrors = new List<string>();

            // TODO: scan for errors

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
