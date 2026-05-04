using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;
using SF3.Types;

namespace SF3.Models.Tables.X1.Battle {
    public class MapMoveCoordFlagsPointerTable : FixedSizeTable<MapMoveCoordFlagsTablePointer> {
        protected MapMoveCoordFlagsPointerTable(IByteData data, string name, int address, int ramAddress)
        : base(data, name, address, 5) {
            RamAddress = ramAddress;
        }

        public static MapMoveCoordFlagsPointerTable Create(IByteData data, string name, int address, int ramAddress)
            => Create(() => new MapMoveCoordFlagsPointerTable(data, name, address, ramAddress));

        public override bool Load()
            => Load((id, address) => new MapMoveCoordFlagsTablePointer(Data, id, $"{(MapLeaderType) id} Coord Flags", address, (MapLeaderType) id, this));

        public int RamAddress { get; }
    }
}
