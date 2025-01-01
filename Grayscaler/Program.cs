using CommonLib.Arrays;
using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace Grayscaler {
    public class Program {
        private const string c_pathIn = "D:/";
        private const string c_pathOut = "../../../Private";
                                   // ^
                                   //  `-- Enter the path for all your MPD files here!

        public static void Main(string[] args) {
            // Get a list of all .MPD files from 'c_pathIn'.
            var filesIn = Directory.GetFiles(c_pathIn, "*.MPD");

            // (NameGetterContext is irrelevant for this project. It's used to get named values
            //  for stuff, like character names, classes, spells, items, etc.)
            var scenario = ScenarioType.Scenario1;
            var nameGetter = new NameGetterContext(scenario);

            // For each file, update ALL textures in ALL their texture chunks.
            // (This doesn't update scroll panes)
            foreach (var fileIn in filesIn) {
                Console.Write(fileIn + ": ");

                // Get a raw data editing context for the file.
                var byteData = new ByteData(new ByteArray(File.ReadAllBytes(fileIn)));

                // Create an MPD file that works with our new ByteData.
                var mpdFile = MPD_File.Create(byteData, nameGetter, scenario);

                // Gather all textures into one collection.
                var textures1 = (mpdFile.TextureChunks == null) ? [] : mpdFile.TextureChunks
                    .Where(x => x != null && x.TextureTable != null)
                    .SelectMany(x => x.TextureTable.Rows)
                    .Where(x => x.TextureIsLoaded && x.Texture.PixelFormat == TexturePixelFormat.ABGR1555)
                    .ToArray();

                var textures2 = (mpdFile.TextureAnimations == null) ? [] : mpdFile.TextureAnimations.Rows
                    .SelectMany(x => x.Frames)
                    .Where(x => x.FrameNum > 0)
                    .Where(x => x.TextureIsLoaded && x.Texture.PixelFormat == TexturePixelFormat.ABGR1555)
                    .ToArray();

                Console.WriteLine((textures1.Length + textures2.Length) + " textures");

                // Transform every texture in ABGR1555 format to grayscale.
                foreach (var tc in textures1)
                    tc.RawImageData16Bit = MakeTextureGrayscale(tc.Texture.ImageData16Bit);

                foreach (var tc in textures2) {
                    // TODO: This shouldn't have to go through the trouble of finding the frameData
                    var frameData = mpdFile.Chunk3Frames.First(x => x.Offset == tc.CompressedTextureOffset).Data.DecompressedData;
                    var referenceTex = textures1.FirstOrDefault(x => x.ID == tc.TextureID)?.Texture;
                    _ = tc.UpdateTextureABGR1555(frameData, MakeTextureGrayscale(tc.Texture.ImageData16Bit), referenceTex);
                }

                // This will compress chunks and update the chunk table header.
                _ = mpdFile.Finish();

                // Write it back out!
                var output = mpdFile.Data.GetDataCopy();
                var fileOut = Path.Combine(c_pathOut, Path.GetFileName(fileIn));
                File.WriteAllBytes(fileOut, output);
            };
        }

        /// <summary>
        /// Returns a new copy of 16-bit ABGR1555 image data in grayscale.
        /// </summary>
        /// <param name="imageDataIn">Input image data.</param>
        /// <returns>A new ushort[,] with updated image data.</returns>
        public static ushort[,] MakeTextureGrayscale(ushort[,] imageDataIn) {
            var imageData = (ushort[,]) imageDataIn.Clone();
            for (int y = 0; y < imageData.GetLength(1); y++) {
                for (int x = 0; x < imageData.GetLength(0); x++) {
                    var value = imageData[x, y];

                    // Produce a luminance value from RGB channels.
                    const byte r = 0x0F; // (value >>  0) & 0x1F;
                    const byte g = 0x0F; // (value >>  5) & 0x1F;
                    const byte b = 0x0F; // (value >> 10) & 0x1F;
                    var l = Math.Min((int) (0.2126 * r + 0.7152 * g + 0.0722 * b), 0x1F);

                    // Update the value, preserving the transparency bit (0x8000).
                    imageData[x, y] = (ushort) ((value & 0x8000) | (l << 10) | (l << 5) | l);
                }
            }
            return imageData;
        }
    }
}
