using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD.Surface;

namespace SF3.Models.Files.MPD {
    public class Surface : TableFile {
        protected Surface(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex)
        : base(data, nameContext) {
            Address    = address;
            Name       = name;
            ChunkIndex = chunkIndex;
        }

        public static Surface Create(IByteData data, INameGetterContext nameContext, int address, string name, int? chunkIndex) {
            var newFile = new Surface(data, nameContext, address, name, chunkIndex);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (HeightmapRowTable     = HeightmapRowTable.Create    (Data, 0x0000)),
                (HeightTerrainRowTable = HeightTerrainRowTable.Create(Data, 0x4000)),
                (EventIDRowTable       = EventIDRowTable.Create      (Data, 0x6000)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }
        public int? ChunkIndex { get; }

        [BulkCopyRecurse]
        public HeightmapRowTable HeightmapRowTable { get; private set; }

        [BulkCopyRecurse]
        public HeightTerrainRowTable HeightTerrainRowTable { get; private set; }

        [BulkCopyRecurse]
        public EventIDRowTable EventIDRowTable { get; private set; }
    }
}
