using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SF3.Editors.MPD;
using SF3.Models.MPD.TextureChunk;
using SF3.NamedValues;
using SF3.RawEditors;
using SF3.Types;

namespace TextureExtractor {
    public class Program {
        private const string c_path = "../../../Private/MPD";
        // ^
        //  `-- Enter the path for all your MPD files here!

        private const string c_outputPath = "../../../Private/Output";

        private class TextureRef {
            public TextureRef(string filename, int chunk, int id, int segment, int width, int height, byte[] imageData) {
                Filename  = filename;
                Chunk     = chunk;
                ID        = id;
                Segment   = segment;
                Width     = width;
                Height    = height;
                ImageData = imageData;

                using (var md5 = MD5.Create())
                    Hash = ((segment == 0) ? "" : segment.ToString()) + BitConverter.ToString(md5.ComputeHash(ImageData)).Replace("-", "").ToLower();
            }

            public string Filename;
            public int Chunk;
            public int ID;
            public int Segment;
            public int Width;
            public int Height;
            public byte[] ImageData;
            public string Hash;
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

        private static TextureRef[] GenerateTextureRefs(string filename, int chunk, Texture texture) {
            // Enable this to make fancy-schmancy (potentially useless) subtextures
#if false
            var fullImageData = texture.CachedBitmapDataARGB1555;
            var imageWidth    = texture.Width / 2;
            var imageHeight   = texture.Height / 2;
            var imageDataSize = imageWidth * imageHeight * 2;

            var textureRefs = new TextureRef[4];
            var imageData = new byte[textureRefs.Length][]; // [imageDataSize];
            for (var i = 0; i < textureRefs.Length; i++)
                imageData[i] = new byte[imageDataSize];

            int pos = 0;
            for (int segmentY = 0; segmentY < 2; segmentY++) {
                for (int y = 0; y < imageHeight; y++) {
                    for (int segmentX = 0; segmentX < 2; segmentX++) {
                        int segment = segmentY * 2 + segmentX;
                        int segmentPos = y * imageWidth * 2;
                        for (int x = 0; x < imageWidth; x++) {
                            imageData[segment][segmentPos++] = fullImageData[pos++];
                            imageData[segment][segmentPos++] = fullImageData[pos++];
                        }
                    }
                }
            }

            for (var i = 0; i < textureRefs.Length; i++)
                textureRefs[i] = new TextureRef(filename, chunk, texture.ID, i + 1, imageWidth, imageHeight, imageData[i]);

            return textureRefs;
#else
            return [new TextureRef(filename, chunk, texture.ID, 0, texture.Width, texture.Height, texture.CachedBitmapDataARGB1555)];
#endif
        }

        public static void Main(string[] args) {
            // Get a list of all .MPD files in a folder called 'Private' relative to this project.
            var files = Directory
                .GetFiles(c_path, "*.MPD")
                .OrderBy(x => FileSortKey(x))
                .ToList();

            // (NameGetterContext is irrelevant for this project. It's used to get named values
            //  for stuff, like character names, classes, spells, items, etc.)
            var scenario = ScenarioType.Scenario1;
            var nameGetter = new NameGetterContext(scenario);

            var texturesFound = new Dictionary<string, List<TextureRef>>();

            // For each file, update ALL textures in ALL their texture chunks.
            // (This doesn't update background/floor images or animated textures)
            foreach (var file in files) {
                var filename = Path.GetFileNameWithoutExtension(file);
                Console.Write(Path.GetFileName(file) + ": ");

                // Get a raw data editing context for the file.
                var byteEditor = new ByteEditor(File.ReadAllBytes(file));

                // Create an MPD editor that works with our new ByteEditor.
                using (var mpdEditor = MPD_Editor.Create(byteEditor, nameGetter, scenario)) {
                    // Let's only gather tiles used in surface character rows.
                    if (mpdEditor.TileSurfaceCharacterRows == null) {
                        Console.WriteLine("No surface textures");
                        continue;
                    }

                    var tileSurfaceCharacterIDs = mpdEditor.TileSurfaceCharacterRows.Rows
                        .SelectMany(x => x.Tiles)
                        .Select(x => x & 0xFF)
                        .Distinct()
                        .ToArray();

                    // Gather all textures into one collection.
                    var textures = mpdEditor.TextureChunks
                        .Select((x, i) => new {ChunkIndex = i, Chunk = x})
                        .Where(x => x.Chunk != null && x.Chunk.TextureTable != null)
                        .SelectMany(x => x.Chunk.TextureTable.Rows.Select(y => new {x.ChunkIndex, Texture = y}))
                        .Where(x => tileSurfaceCharacterIDs.Contains(x.Texture.ID) && x.Texture.AssumedPixelFormat == TexturePixelFormat.ABGR1555 &&
                                    x.Texture.Width % 2 == 0 && x.Texture.Height % 2 == 0)
                        .ToArray();
                    Console.WriteLine(textures.Length + " eligable surface textures");

                    // Break each texture up into 4 sub-textures.
                    var textureRefs = textures.SelectMany(x => GenerateTextureRefs(filename, x.ChunkIndex, x.Texture));

                    // Transform every texture in ABGR1555 format to grayscale.
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

            var orderedTextures = texturesFound
                .Select(x => x.Value)
                .OrderBy(x => FileSortKey(x[0].Filename))
                .ThenByDescending(x => x.Count)
                .ThenBy(x => x[0].Width + x[0].Height)
                .ThenBy(x => x[0].Hash)
                .ToList();

            Directory.CreateDirectory(c_outputPath);

            foreach (var texRefs in orderedTextures) {
                var tex = texRefs[0];
                var subPath = Path.Combine(c_outputPath, FileSortKey(tex.Filename));
                Directory.CreateDirectory(subPath);

                var refStr = texRefs[0].Segment + "_" + texRefs.Count.ToString("D4");
                refStr += "_" + ((texRefs.All(x => x.ID == tex.ID)) ? tex.ID.ToString("D3") : "XXX");

                var outputPath = Path.Combine(subPath, refStr + "_" + tex.Width + "x" + tex.Height + "_" + tex.Hash + ".BMP");

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
