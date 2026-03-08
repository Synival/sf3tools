using System;
using System.Collections.Generic;
using CommonLib.Extensions;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class FACE32_File : DAT_FileBase {
        // TODO: Where is this loaded?
        public override int RamAddress => 0x00000000;
        // TODO: To where is this loaded?
        public override int RamAddressLimit => 0x00000000;

        protected FACE32_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario, DAT_FileType.FACE32) {
        }

        public static FACE32_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new FACE32_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();

            // Get the position of the palette by getting the first face image location.
            // The palette will be 0x200 before that.
            var paletteOffset = 0;
            for (var i = 0; i < Data.Length; i += 4) {
                var imageOffset = Data.GetDouble(i);
                if (imageOffset != -1) {
                    paletteOffset = imageOffset - 0x200;
                    break;
                }
            }

            // Get the palette
            // TODO: this should be a table!
            var palette = new Palette(Data.GetDataCopyAt(paletteOffset, 0x200).ToUShorts());

            var faceCount = paletteOffset / 4;
            tables.Add(
                TextureTable = Face32_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext, faceCount, palette)
            );
            TextureViewerScale = 4;
            Spritesheet = new TexturesAsSpritesheet(this, palette, false, 32, 32, 24);

            return tables;
        }

        public override void ReplaceImages8Bit(byte[][,] images, Palette palette)
            => throw new InvalidOperationException();
        public override void ReplaceImages16Bit(ushort[][,] images)
            => throw new InvalidOperationException();

        public override bool CanReplaceImages8Bit => false;
        public override bool CanReplaceImages16Bit => false;
    }
}
