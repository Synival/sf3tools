using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class BTLENKEI_File : DAT_FileBase {
        // TODO: Where is this loaded?
        public override int RamAddress => 0x00000000;
        // TODO: To where is this loaded?
        public override int RamAddressLimit => 0x00000000;

        protected BTLENKEI_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario, DAT_FileType.BTLENKEI) {
        }

        public static BTLENKEI_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new BTLENKEI_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable> {
                (TextureTable = BtlEnkei_TextureTable.Create(Data, nameof(BtlEnkei_TextureTable), 0, NameGetterContext, headerless: Scenario >= ScenarioType.Scenario3))
            };
            TextureViewerScale = 1;
            Spritesheet = new TexturesAsSpritesheet(this, false, 512, 256, 1);

            return tables;
        }

        public override void ReplaceImages8Bit(byte[][,] images, IPalette palette, bool minimalChanges)
            => throw new InvalidOperationException();
        public override void ReplaceImages16Bit(ushort[][,] image, bool minimalChangess)
            => throw new InvalidOperationException();

        public override bool CanReplaceImages8Bit => false;
        public override bool CanReplaceImages16Bit => false;
    }
}
