using System;
using System.Collections.Generic;
using System.Linq;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.Utils;
using SF3.ByteData;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class ITEM_CG_File : DAT_FileBase {
        // TODO: Where is this loaded?
        public override int RamAddress => 0x00000000;
        // TODO: To where is this loaded?
        public override int RamAddressLimit => 0x00000000;

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

        public override void ReplaceImages8Bit(byte[][,] images, Palette palette) {
            images = images.Select(x => ImageUtils.GetImageDataConformingToPalette(x, palette, ItemCG_TextureTable.ItemSpellPalette)).ToArray();
            var compressedImages = images.Select(x => Compression.CompressLZSS(x.To1DArrayTransposed())).ToArray();

            var newData = new byte[compressedImages.Sum(x => x.Length)];
            int newDataPos = 0;
            foreach (var compressedImage in compressedImages) {
                for (int imagePos = 0; imagePos < compressedImage.Length; imagePos++)
                    newData[newDataPos++] = compressedImage[imagePos];
            }

            Data.SetDataTo(newData);

            for (int imageIndex = 0, imagePos = 0; imageIndex < compressedImages.Length; imagePos += compressedImages[imageIndex].Length, imageIndex++) {
                var image = (ItemCG_Texture) TextureTable[imageIndex];
                image.UpdateAddress(imagePos);
                image.MaxStoredImageSize = compressedImages[imageIndex].Length;
                image.InvalidateImage();
            }

            Spritesheet.Invalidate();
        }

        public override void ReplaceImages16Bit(ushort[][,] images)
            => throw new InvalidOperationException();

        public override bool CanReplaceImages8Bit => true;
        public override bool CanReplaceImages16Bit => false;

        public int SpellIconIndex { get; }
    }
}
