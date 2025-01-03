using System.Collections.Generic;
using CommonLib.Attributes;
using CommonLib.NamedValues;
using SF3.ByteData;
using SF3.Models.Tables;
using SF3.Models.Tables.MPD;

namespace SF3.Models.Files.MPD.Objects {
    public class Surface : TableFile {
        protected Surface(IByteData data, INameGetterContext nameContext, int address, string name)
        : base(data, nameContext) {
            Address = address;
            Name    = name;
        }

        public static Surface Create(IByteData data, INameGetterContext nameContext, int address, string name) {
            var newFile = new Surface(data, nameContext, address, name);
            newFile.Init();
            return newFile;
        }

        public override IEnumerable<ITable> MakeTables() {
            return new List<ITable>() {
                (TileSurfaceHeightmapRows = TileSurfaceHeightmapRowTable.Create(Data, 0x0000)),
                (TileHeightTerrainRows    = TileHeightTerrainRowTable.Create   (Data, 0x4000)),
                (TileItemRows             = TileItemRowTable.Create            (Data, 0x6000)),
            };
        }

        [BulkCopyRowName]
        public string Name { get; }

        public int Address { get; }

        [BulkCopyRecurse]
        public TileSurfaceHeightmapRowTable TileSurfaceHeightmapRows { get; private set; }

        [BulkCopyRecurse]
        public TileHeightTerrainRowTable TileHeightTerrainRows { get; private set; }

        [BulkCopyRecurse]
        public TileItemRowTable TileItemRows { get; private set; }
    }
}
