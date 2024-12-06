using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using CommonLib;
using SF3;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace TextureExtractor {
    public class Program {
        private const string c_path = "../../../Private/MPD";
                                    // ^
                                    //  `-- Enter the path for all your MPD files here!

        private const string c_outputPath = "../../../Private/Output";

        private class TextureRef {
            public TextureRef(string filename, int id, int frame, int width, int height, byte[] imageData) {
                Filename  = filename;
                ID        = id;
                Frame     = frame;
                Width     = width;
                Height    = height;
                ImageData = imageData;

                using (var md5 = MD5.Create())
                    Hash = BitConverter.ToString(md5.ComputeHash(ImageData)).Replace("-", "").ToLower();
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
            => [new TextureRef(filename, id, frame, texture.Width, texture.Height, texture.BitmapDataARGB1555)];

        public static void Main(string[] args) {
            // Get a list of all .MPD files in a folder called 'Private' relative to this project.
            var allFiles = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => Directory.GetFiles(Path.Combine(c_path, x.ToString())).OrderBy(x => FileSortKey(x)).ToList());
            var nameGetterContexts = Enum.GetValues<ScenarioType>()
                .ToDictionary(x => x, x => new NameGetterContext(x));

            var texturesFound = new Dictionary<string, List<TextureRef>>();

            // For each file, fetch ALL textures in ALL their texture chunks.
            // (This doesn't yet fetch background/floor images or animated textures)
            foreach (var filesKv in allFiles) {
                var scenario = filesKv.Key;
                var nameGetter = nameGetterContexts[scenario];

                foreach (var file in filesKv.Value) {
                    var filename = Path.GetFileNameWithoutExtension(file);
                    Console.Write(Path.GetFileName(file) + ": ");

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
                                .SelectMany(x => x.Tiles)
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

            var orderedTextures = texturesFound
                .Select(x => x.Value)
                .OrderBy(x => x[0].ID)
                .ThenBy(x => x[0].Frame)
                .ThenByDescending(x => x.Count)
                .ThenBy(x => x[0].Width + x[0].Height)
                .ThenBy(x => x[0].Hash)
                .ToList();

            Directory.CreateDirectory(c_outputPath);

            foreach (var texRefs in orderedTextures) {
                var tex = texRefs[0];
                var subPath = Path.Combine(c_outputPath);
                Directory.CreateDirectory(subPath);

                var refStr = "Tex" + tex.ID + "_Frame" + tex.Frame + "_x" + texRefs.Count.ToString("D4") + "_" + tex.Width + "x" + tex.Height + "_" + tex.Hash;
                var outputPath = Path.Combine(subPath, refStr + ".BMP");
                Console.WriteLine("Writing: " + outputPath);
                using (var bitmap = new Bitmap(tex.Width, tex.Height, PixelFormat.Format16bppArgb1555)) {
                    var imageData = tex.ImageData;
                    var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    Marshal.Copy(imageData, 0, bmpData.Scan0, imageData.Length);
                    bitmap.UnlockBits(bmpData);
                    bitmap.Save(outputPath);
                }
            }
        }
    }
}
