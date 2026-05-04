using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class MapMoveCoordFlagsTable : FixedSizeTable<MapMoveCoordFlags> {
        protected MapMoveCoordFlagsTable(IByteData data, string name, int address, MapLeaderType mapLeader)
        : base(data, name, address, 72) {
            MapLeader=mapLeader;
        }

        public static MapMoveCoordFlagsTable Create(IByteData data, string name, int address, MapLeaderType mapLeader)
            => Create(() => new MapMoveCoordFlagsTable(data, name, address, mapLeader));

        public override bool Load()
            => Load((id, address) => new MapMoveCoordFlags(Data, id, $"{nameof(MapMoveCoordFlags)}{id:D2}", address));

        public MapLeaderType MapLeader { get; set; }
    }
}
