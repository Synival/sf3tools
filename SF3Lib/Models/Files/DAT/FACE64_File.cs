using System;
using System.Collections.Generic;
using CommonLib.Imaging;
using CommonLib.NamedValues;
using CommonLib.Types;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class FACE64_File : DAT_FileBase {
        // TODO: Where is this loaded?
        public override int RamAddress => 0x00000000;
        // TODO: To where is this loaded?
        public override int RamAddressLimit => 0x00000000;

        protected FACE64_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario)
        : base(data, nameGetterContext, scenario, DAT_FileType.FACE64) {
        }

        public static FACE64_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType? scenario) {
            var newFile = new FACE64_File(data, nameGetterContext, scenario);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize " + newFile.GetType().Name);
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable> {
                (TextureTable = Face64_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext))
            };
            TextureViewerScale = 2;
            Spritesheet = new TexturesAsSpritesheet(this, false, 64, 64, 12);

            return tables;
        }

        public override void ReplaceImages8Bit(byte[][,] images, IPalette palette, bool minimalChanges)
            => throw new InvalidOperationException();
        public override void ReplaceImages16Bit(ushort[][,] images, bool minimalChanges)
            => throw new InvalidOperationException();

        public override ImageDataCanSet CanReplaceImages => ImageDataCanSet.Never;
    }
}
