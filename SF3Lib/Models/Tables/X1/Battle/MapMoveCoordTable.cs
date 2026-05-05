using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class MapMoveCoordTable : FixedSizeTable<MapMoveCoord> {
        protected MapMoveCoordTable(IByteData data, string name, int address, MapLeaderType mapLeader, BattleHeader battleHeader)
        : base(data, name, address, 72) {
            MapLeader    = mapLeader;
            BattleHeader = battleHeader;
        }

        public static MapMoveCoordTable Create(IByteData data, string name, int address, MapLeaderType mapLeader, BattleHeader battleHeader)
            => Create(() => new MapMoveCoordTable(data, name, address, mapLeader, battleHeader));

        public override bool Load()
            => Load((id, address) => new MapMoveCoord(Data, id, $"{nameof(MapMoveCoord)}{id:D2}", address, MapLeader, BattleHeader));

        public MapLeaderType MapLeader { get; }
        public BattleHeader BattleHeader { get; }
    }
}
