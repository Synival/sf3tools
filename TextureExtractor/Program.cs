using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using CommonLib.Arrays;
using SF3;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace TextureExtractor {
    public class Program {
        // ,--- Enter the paths for all your MPD files here!
        // v
        private static readonly Dictionary<ScenarioType, string> c_pathsIn = new() {
            { ScenarioType.Scenario1,   "D:/" },
            { ScenarioType.Scenario2,   "E:/" },
            { ScenarioType.Scenario3,   "F:/" },
            { ScenarioType.PremiumDisk, "G:/" },
        };

        // ,--- All textures are dumped here!
        // v
        private const string c_pathOut = "../../../Private";

        private class TextureRef {
            public TextureRef(string filename, int id, int frame, int width, int height, byte[] imageData, string hash) {
                Filename  = filename;
                ID        = id;
                Frame     = frame;
                Width     = width;
                Height    = height;
                ImageData = imageData;
                Hash      = hash;
            }

            public string Filename;
            public int ID { get; }
            public int Frame { get; }
            public int Width { get; }
            public int Height { get; }
            public byte[] ImageData { get; }
            public string Hash { get; }
        }

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int memcmp(byte[] lhs, byte[] rhs, long count);

        private static bool ByteArraysAreEqual(byte[] lhs, byte[] rhs)
            => lhs.Length == rhs.Length && memcmp(lhs, rhs, lhs.Length) == 0;

        private static string FileSortKey(string fullPath) {
            var filename = Path.GetFileNameWithoutExtension(fullPath);
            if (filename.StartsWith("BTL") && int.TryParse(filename.Substring(3), out var btlNum))
                return btlNum.ToString("D3");
            else if (filename.StartsWith("BTLA") && int.TryParse(filename.Substring(4), out var btlNum2))
                return "10" + btlNum2.ToString("D1");
            else
                return filename;
        }

        private static TextureRef[] GenerateTextureRefs(string filename, int id, int frame, ITexture texture)
            => [new TextureRef(filename, id, frame, texture.Width, texture.Height, texture.BitmapDataARGB1555, texture.Hash)];

        public static void Main(string[] args) {
            // Get a list of all .MPD files from all scenarios located at 'c_pathsIn[Scenario]'.
            var allFiles = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => Directory.GetFiles(c_pathsIn[x], "*.MPD").OrderBy(x => FileSortKey(x)).ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => new NameGetterContext(x));

            var texturesFound = new Dictionary<string, List<TextureRef>>();

            // For each file, fetch ALL textures in ALL their texture chunks.
            // (This doesn't yet fetch floor images or scroll panes)
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    Console.Write(scenario.ToString() + ": " + Path.GetFileName(file) + ": ");

                    // Get a raw data editing context for the file.
                    var byteData = new ByteData(new ByteArray(File.ReadAllBytes(file)));

                    // Create an MPD file that works with our new ByteData.
                    try {
                        using (var mpdFile = MPD_File.Create(byteData, nameGetter, scenario)) {
                            // Let's only gather tiles used in surface character rows.
                            if (mpdFile.TileSurfaceCharacterRows == null) {
                                Console.WriteLine("No surface textures");
                                continue;
                            }

                            var tileSurfaceCharacterIDs = mpdFile.TileSurfaceCharacterRows.Rows
                                .SelectMany(x => x.GetRowCopy())
                                .Select(x => x & 0xFF)
                                .Distinct()
                                .ToArray();

                            // Gather all textures into one collection.
                            var textures = mpdFile.TextureChunks
                                .Where(x => x?.TextureTable != null)
                                .SelectMany(x => x.TextureTable.Rows)
                                .Where(x => tileSurfaceCharacterIDs.Contains(x.ID) && x.TextureIsLoaded && x.Texture.PixelFormat == TexturePixelFormat.ABGR1555 && x.Width % 2 == 0 && x.Height % 2 == 0)
                                .ToArray();

                            var frames = (mpdFile.TextureAnimations == null) ? [] : mpdFile.TextureAnimations.Rows
                                .SelectMany(x => x.Frames)
                                .Where(x => tileSurfaceCharacterIDs.Contains(x.TextureID) && x.TextureIsLoaded && x.Texture.PixelFormat == TexturePixelFormat.ABGR1555 && x.Width % 2 == 0 && x.Height % 2 == 0)
                                .ToArray();

                            Console.WriteLine(textures.Length + " eligable surface textures, " + frames.Length + " animation frames");

                            // Convert textures to a texture reference format with all the information we need.
                            var textureRefs = textures
                                .SelectMany(x => GenerateTextureRefs(filename, x.ID, 0, x.Texture))
                                .Concat(frames.SelectMany(x => GenerateTextureRefs(filename, x.TextureID, x.FrameNum, x.Texture)));

                            int uniqueCount = 0;
                            foreach (var tr in textureRefs) {
                                if (!texturesFound.ContainsKey(tr.Hash)) {
                                    texturesFound[tr.Hash] = [tr];
                                    uniqueCount++;
                                }
                                else {
                                    if (!ByteArraysAreEqual(texturesFound[tr.Hash][0].ImageData, tr.ImageData))
                                        throw new Exception("This ain't it, chief");
                                    texturesFound[tr.Hash].Add(tr);
                                }
                            }
                            Console.WriteLine("    " + uniqueCount.ToString() + " new unique texture(s)");
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("Exception: '" + e.Message + "'. Skipping!");
                    }
                }
            }

            var allTextures = texturesFound.SelectMany(x => x.Value).ToArray();
            var orderedTextures = allTextures
                .OrderBy(x => x.Filename)
                .ThenBy(x => x.ID)
                .GroupBy(x => x.Hash)
                .Select(x => new KeyValuePair<string, List<TextureRef>>(x.Key, x.ToList()))
                .OrderBy(x => x.Value[0].Filename)
                .ToList();

            Directory.CreateDirectory(c_pathOut);

            foreach (var texRefs in orderedTextures) {
                var tex = texRefs.Value[0];
                _ = Directory.CreateDirectory(c_pathOut);

                var refStr = tex.Hash + "_Tex" + tex.ID + "_Frame" + tex.Frame + "_x" + texRefs.Value.Count.ToString("D4") + "_" + tex.Width + "x" + tex.Height;
                var outputPath = Path.Combine(c_pathOut, refStr + ".BMP");
                Console.WriteLine("Writing: " + outputPath);
#pragma warning disable CA1416 // Validate platform compatibility
                using (var bitmap = new Bitmap(tex.Width, tex.Height, PixelFormat.Format16bppArgb1555)) {
                    var imageData = tex.ImageData;
                    var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
                    bitmap.UnlockBits(bmpData);
                    bitmap.Save(outputPath);
                }
#pragma warning restore CA1416 // Validate platform compatibility
            }
        }
    }
}
