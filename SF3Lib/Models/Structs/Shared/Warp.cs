using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class Warp : Struct {
        private readonly int unknown1;
        private readonly int unknown2;
        private readonly int type;
        private readonly int map;

        public Warp(IByteData data, int id, string name, int address, Warp prevWarp)
        : base(data, id, name, address, 0x04) {
            PrevWarp = prevWarp;

            unknown1 = Address;
            unknown2 = Address + 1;
            type     = Address + 2;
            map      = Address + 3;
        }

        public Warp PrevWarp { get; }

        [TableViewModelColumn(displayOrder: -1, displayFormat: "X2")]
        public int WarpID => (WarpUnknown1 == 0x00 && WarpUnknown2 == 0x00 && WarpType == 0x00) ? 0 : (PrevWarp?.WarpID ?? 0) + 1;

        [TableViewModelColumn(displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public int WarpUnknown1 {
            get => Data.GetByte(unknown1);
            set => Data.SetByte(unknown1, (byte) value);
        }

        [TableViewModelColumn(displayOrder: 1, displayName: "+0x01", displayFormat: "X2")]
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

        [TableViewModelColumn(displayOrder: 4, displayName: "MPD Tie-In", displayFormat: "X2")]
        public int? MPDTieIn => (WarpID > 0) ? WarpID + 0x10 : (int?) null;
    }
}
