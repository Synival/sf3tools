using System;
using System.Collections.Generic;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Structs.DAT;
using SF3.Models.Tables;
using SF3.Models.Tables.DAT;
using SF3.Types;

namespace SF3.Models.Files.DAT {
    public class DAT_File : ScenarioTableFile, IDAT_File {
        // Not applicable
        public override int RamAddress => 0x00000000;
        // Not applicable
        public override int RamAddressLimit => 0x00000000;

        protected DAT_File(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario, DAT_FileType fileType)
        : base(data, nameGetterContext, scenario) {
            FileType = fileType;
        }

        public static DAT_File Create(IByteData data, INameGetterContext nameGetterContext, ScenarioType scenario, DAT_FileType fileType) {
            var newFile = new DAT_File(data, nameGetterContext, scenario, fileType);
            if (!newFile.Init())
                throw new InvalidOperationException("Couldn't initialize CHR_File");
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            var tables = new List<ITable>();

            if (FileType == DAT_FileType.FACE64) {
                tables.Add(TextureTable = Face64_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext));
                TextureViewerScale = 2;
            }
            if (FileType == DAT_FileType.ITEM_CG) {
                tables.Add(TextureTable = ItemCG_TextureTable.Create(Data, nameof(TextureTable), 0, NameGetterContext));
                TextureViewerScale = 4;
            }

            return tables;
        }

        public DAT_FileType FileType { get; }
        public Table<TextureModelBase> TextureTable { get; private set; }
        public int TextureViewerScale { get; set; } = 0;
    }
}
