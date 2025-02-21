using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class Warp : Struct {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        public Warp(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x04) {
            unknown1 = Address;
            unknown2 = Address + 1;
            type     = Address + 2;
            map      = Address + 3;
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public int WarpUnknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "+0x02", displayFormat: "X2")]
        [BulkCopy]
        public int WarpUnknown2 {
            get => Data.GetByte(unknown2);
            set => Data.SetByte(unknown2, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 2, displayName: "Type", displayFormat: "X2")]
        [BulkCopy]
        public int WarpType {
            get => Data.GetByte(type);
            set => Data.SetByte(type, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 3, displayName: "Map", displayFormat: "X2")]
        [BulkCopy]
        public int WarpMap {
            get => Data.GetByte(map);
            set => Data.SetByte(map, (byte) value);
        }
    }
}
