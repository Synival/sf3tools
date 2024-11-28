﻿using SF3.Models.Files.MPD;
using SF3.NamedValues;
using SF3.RawData;
using SF3.Types;

namespace Grayscaler {
    public class Program {
        private const string c_path = "";
                                   // ^
                                   //  `-- Enter the path for all your MPD files here!

        public static void Main(string[] args) {
            // Get a list of all .MPD files in a folder called 'Private' relative to this project.
            var files = Directory.GetFiles(c_path, "*.MPD");

            // (NameGetterContext is irrelevant for this project. It's used to get named values
            //  for stuff, like character names, classes, spells, items, etc.)
            var scenario = ScenarioType.Scenario1;
            var nameGetter = new NameGetterContext(scenario);

            // For each file, update ALL textures in ALL their texture chunks.
            // (This doesn't update background/floor images or animated textures)
            foreach (var file in files) {
                Console.Write(file + ": ");

                // Get a raw data editing context for the file.
                var byteData = new ByteData(File.ReadAllBytes(file));

                // Create an MPD file that works with our new ByteData.
                var mpdFile = MPD_File.Create(byteData, nameGetter, scenario);

                // Gather all textures into one collection.
                var textures = mpdFile.TextureChunks.Where(x => x != null && x.TextureTable != null).SelectMany(x => x.TextureTable.Rows).ToArray();
                Console.WriteLine(textures.Length + " textures");

                // Transform every texture in ABGR1555 format to grayscale.
                foreach (var tc in textures) {
                    if (tc.AssumedPixelFormat != TexturePixelFormat.ABGR1555)
                        continue;
                    tc.ImageData16Bit = MakeTextureGrayscale(tc.CachedImageData16Bit);
                }

                // This will compress chunks and update the chunk table header.
                mpdFile.Finalize();

                // Write it back out!
                var output = mpdFile.Data.GetAllData();
                File.WriteAllBytes(file, output);
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
                    var r = (value >>  0) & 0x1F;
                    var g = (value >>  5) & 0x1F;
                    var b = (value >> 10) & 0x1F;
                    var l = Math.Min((int) (0.2126 * r + 0.7152 * g + 0.0722 * b), 0x1F);

                    // Update the value, preserving the transparency bit (0x8000).
                    imageData[x, y] = (ushort) ((value & 0x8000) | (l << 10) | (l << 5) | l);
                }
            }
            return imageData;
        }
    }
}
