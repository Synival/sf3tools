using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class Boundary : Struct {
        private readonly int _x1Addr;
        private readonly int _z1Addr;
        private readonly int _x2Addr;
        private readonly int _z2Addr;

        public Boundary(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x8) {
            _x1Addr = address + 0x00; // 2 bytes
            _z1Addr = address + 0x02; // 2 bytes
            _x2Addr = address + 0x04; // 2 bytes
            _z2Addr = address + 0x06; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_x1Addr), displayOrder: 0, minWidth: 60)]
        public short X1 {
            get => (short) Data.GetWord(_x1Addr);
            set => Data.SetWord(_x1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_z1Addr), displayOrder: 1, minWidth: 60)]
        public short Z1 {
            get => (short) Data.GetWord(_z1Addr);
            set => Data.SetWord(_z1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_x2Addr), displayOrder: 2, minWidth: 60)]
        public short X2 {
            get => (short) Data.GetWord(_x2Addr);
            set => Data.SetWord(_x2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_z2Addr), displayOrder: 3, minWidth: 60)]
        public short Z2 {
            get => (short) Data.GetWord(_z2Addr);
            set => Data.SetWord(_z2Addr, value);
        }
    }
}
