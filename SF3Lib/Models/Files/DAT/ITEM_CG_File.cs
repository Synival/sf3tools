using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.Types;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class ITEM_CG_File : DAT_FileBase {
        public override int RamAddress => 0x002D0000;
        public override int RamAddressLimit => 0x002E8000;

        protected ITEM_CG_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario, DAT_FileType.ITEM_CG) {
            SpellIconIndex = ValueNames.ItemInfo.Info[scenario.Value].Values.Max(x => x.Key) + 1;
        }

        public static ITEM_CG_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new ITEM_CG_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable> {
                (TextureTable = ItemCG_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext))
            };
            TextureViewerScale = 4;
            Spritesheet = new TexturesAsSpritesheet(this, ItemCG_TextureTable.ItemSpellPalette, true, 24, 24, 32);

            return tables;
        }

        public override void ReplaceImages8Bit(byte[][,] images, IPalette incomingPalette, bool minimalChanges) {
            var realPalette = ItemCG_TextureTable.ItemSpellPalette;

            images = images.Select(x => ImageUtils.GetImageDataConformingToPalette(x, incomingPalette, realPalette)).ToArray();
            var compressedImages = images.Select(x => Compression.CompressLZSS(x.To1DArrayTransposed())).ToArray();

            // Prefer to use original data whenever possible.
            if (minimalChanges) {
                for (int i = 0; i < compressedImages.Length; i++) {
                    // We can't simply check the 8-bit color data because there are duplicate colors in the palette.
                    // So, apply the palette, and check the 16-bit resulting colors.
                    var tex = TextureTable[i];
                    var oldImage = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(tex.ImageData8Bit.To1DArrayTransposed(), realPalette, true);
                    var newImage = BitmapUtils.ConvertIndexedDataToARGB8888BitmapData(images[i].To1DArrayTransposed(), realPalette, true);

                    // If the 16-bit images are the same, do nothing. 
                    if (Enumerable.SequenceEqual(oldImage, newImage))
                        compressedImages[i] = Data.GetDataCopyAt(tex.ImageDataOffset, tex.StoredImageDataSize.Value);
                    // If the image is reduced in size, pad it with zeroes so later images aren't displaced.
                    else if (compressedImages[i].Length < tex.StoredImageDataSize) {
                        var newCompressedImage = new byte[tex.StoredImageDataSize.Value];
                        for (int j = 0; j < compressedImages[i].Length; j++)
                            newCompressedImage[j] = compressedImages[i][j];
                        compressedImages[i] = newCompressedImage;
                    }
                }
            }

            // Determine the length of the file's new data.
            var newDataLength = compressedImages.Sum(x => x.Length);
            if (minimalChanges && newDataLength < Data.Length)
                newDataLength = Data.Length;

            // Copy images.
            var newData = new byte[newDataLength];
            int newDataPos = 0;
            foreach (var compressedImage in compressedImages)
                for (int imagePos = 0; imagePos < compressedImage.Length; imagePos++)
                    newData[newDataPos++] = compressedImage[imagePos];

            // Don't do anything if the end result is the exact same data.
            if (Enumerable.SequenceEqual(newData, Data.GetDataCopyOrReference()))
                return;

            // New data is populated -- set the file's data!
            Data.SetDataTo(newData);

            // Update the texture table and invalidate image data.
            for (int imageIndex = 0, imagePos = 0; imageIndex < compressedImages.Length; imagePos += compressedImages[imageIndex].Length, imageIndex++) {
                var image = (ItemCG_Texture) TextureTable[imageIndex];
                image.UpdateAddress(imagePos);
                image.MaxStoredImageSize = compressedImages[imageIndex].Length;
                image.InvalidateImage();
            }
            Spritesheet.Invalidate();
        }

        public override void ReplaceImages16Bit(ushort[][,] images, bool minimalChanges)
            => throw new InvalidOperationException();

        public override ImageDataCanSet CanReplaceImages => ImageDataCanSet.CanSet8Bit;

        public int SpellIconIndex { get; }
    }
}
