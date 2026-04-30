using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class MapMoveCoordTable : FixedSizeTable<MapMoveCoord> {
        protected MapMoveCoordTable(IByteData data, string name, int address) : base(data, name, address, 72) {
        }

        public static MapMoveCoordTable Create(IByteData data, string name, int address)
            => Create(() => new MapMoveCoordTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new MapMoveCoord(Data, id, "MapMoveCoord" + id.ToString("D2"), address));
    }
}
