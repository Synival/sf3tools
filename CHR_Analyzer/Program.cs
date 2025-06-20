using System.Drawing;
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
            public TextureInfo(ITexture texture) {
                Texture = texture;
            }

            public ITexture Texture { get; }
            public List<TextureFileInfo> Files { get; } = new List<TextureFileInfo>();
        }

        private class TextureFileInfo {
            public TextureFileInfo(ScenarioType scenario, string filename) {
                Scenario = scenario;
                Filename = filename;
            }

            public ScenarioType Scenario;
            public string Filename;
        }

        private static Dictionary<string, TextureInfo> s_framesByHash = [];

        private static void AddFrame(ScenarioType scenario, string filename, Frame frame) {
            var hash = frame.Texture.Hash;
            if (!s_framesByHash.ContainsKey(hash))
                s_framesByHash.Add(hash, new TextureInfo(frame.Texture));
            s_framesByHash[hash].Files.Add(new TextureFileInfo(scenario, filename));
        }

        private static List<string> s_matchReports = [];

        private static bool? CHR_MatchFunc(string filename, ICHR_File chrFile, INameGetterContext ngc) {
            var mixedSprites = chrFile.SpriteTable
                .Where(x => x.Header.SpriteID != 0x187)
                .Select(x => new { Table = x, Frames = x.FrameTable.ToArray() })
                .Where(x => x.Frames.Where(y => !y.SpriteName.StartsWith("Transparent Frame")).GroupBy(y => y.SpriteName).Count() > 1)
                .Select(x => x.Table)
                .ToArray();

            if (mixedSprites.Length == 0)
                return null;

            foreach (var sprite in mixedSprites)
                s_matchReports.Add($"{sprite.SpriteName} (SpriteID: 0x{sprite.Header.SpriteID:X3}) - Images from multiple sprites");

            return true;
        }

        public static void Main(string[] args) {
            Console.WriteLine("Press a key to start...");
            _ = Console.ReadKey();

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

            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);

                    // Get a byte data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
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
                            Console.WriteLine(fileStr + " |");
                            foreach (var mr in s_matchReports)
                                Console.WriteLine("    " + mr);
                            s_matchReports.Clear();

                            if (match == true)
                                matchSet.Add(fileStr);
                            else
                                nomatchSet.Add(fileStr);

                            ScanForErrorsAndReport(scenario, chrFile);

                            // Build a table of all textures.
                            foreach (var sprite in chrFile.SpriteTable)
                                foreach (var frame in sprite.FrameTable)
                                    AddFrame(chrFile.Scenario, filename, frame);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("  !!! Exception for '" + filename + "': '" + e.Message + "'. Skipping!");
                    }
                }
            }

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
            Console.WriteLine("| BY HASH                                         |");
            Console.WriteLine("===================================================");
            Console.WriteLine("");

            Directory.CreateDirectory("SpriteDump");

            _ = Directory.CreateDirectory(c_pathOut);
            foreach (var kv in s_framesByHash) {
                var textureInfo = kv.Value;
                var tex = textureInfo.Texture;
                var frameTextureInfo = SpriteFrameTextueUtils.GetFrameTextureInfoByHash(kv.Key);

                var directions = frameTextureInfo.DirectionCounts!;
                if (directions.Count != 1)
                    continue;

                var spriteFileString = frameTextureInfo.SpriteName
                    .Replace("?", "X")
                    .Replace("-", "_")
                    .Replace(":", "_")
                    .Replace("/", "_");

                var mostCommonDirection = directions.OrderByDescending(x => x.Value).First().Key;

                var path = c_pathOut; // Path.Combine(c_pathOut, infos.SpriteNames);
                _ = Directory.CreateDirectory(path);

                var directionsStr = string.Join(",", directions);
                var outputPath = Path.Combine(path, $"{mostCommonDirection} - {spriteFileString} - {frameTextureInfo.DirectionsString} - {kv.Key}.BMP");
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

        private static void ScanForErrorsAndReport(ScenarioType inputScenario, ICHR_File mpdFile) {
            var totalErrors = new List<string>();

            // TODO: scan for errors

            foreach (var error in totalErrors)
                Console.WriteLine("    !!! " + error);
        }
    }
}
