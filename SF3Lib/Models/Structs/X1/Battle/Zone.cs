using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class Zone : Struct {
        private readonly int _numPointsAddr;
        private readonly int _padding0x02Addr;
        private readonly int _x1Addr;
        private readonly int _z1Addr;
        private readonly int _x2Addr;
        private readonly int _z2Addr;
        private readonly int _x3Addr;
        private readonly int _z3Addr;
        private readonly int _x4Addr;
        private readonly int _z4Addr;

        public Zone(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x12) {
            _numPointsAddr   = Address + 0x00; // 1 byte
            _padding0x02Addr = Address + 0x01; // 1 byte
            _x1Addr          = Address + 0x02; // 2 bytes
            _z1Addr          = Address + 0x04; // 2 bytes
            _x2Addr          = Address + 0x06; // 2 bytes
            _z2Addr          = Address + 0x08; // 2 bytes
            _x3Addr          = Address + 0x0a; // 2 bytes
            _z3Addr          = Address + 0x0c; // 2 bytes
            _x4Addr          = Address + 0x0e; // 2 bytes
            _z4Addr          = Address + 0x10; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_numPointsAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public byte NumPoints {
            get => Data.GetUInt8(_numPointsAddr);
            set => Data.SetUInt8(_numPointsAddr, value);
        }

        [BulkCopy]
        public byte Padding0x02 {
            get => Data.GetUInt8(_padding0x02Addr);
            set => Data.SetUInt8(_padding0x02Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_x1Addr), displayOrder: 1)]
        [BulkCopy]
        public ushort X1 {
            get => Data.GetUInt16(_x1Addr);
            set => Data.SetUInt16(_x1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_z1Addr), displayOrder: 2)]
        [BulkCopy]
        public ushort Z1 {
            get => Data.GetUInt16(_z1Addr);
            set => Data.SetUInt16(_z1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_x2Addr), displayOrder: 3)]
        [BulkCopy]
        public ushort X2 {
            get => Data.GetUInt16(_x2Addr);
            set => Data.SetUInt16(_x2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_z2Addr), displayOrder: 4)]
        [BulkCopy]
        public ushort Z2 {
            get => Data.GetUInt16(_z2Addr);
            set => Data.SetUInt16(_z2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_x3Addr), displayOrder: 5)]
        [BulkCopy]
        public ushort X3 {
            get => Data.GetUInt16(_x3Addr);
            set => Data.SetUInt16(_x3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_z3Addr), displayOrder: 6)]
        [BulkCopy]
        public ushort Z3 {
            get => Data.GetUInt16(_z3Addr);
            set => Data.SetUInt16(_z3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_x4Addr), displayOrder: 7)]
        [BulkCopy]
        public ushort X4 {
            get => Data.GetUInt16(_x4Addr);
            set => Data.SetUInt16(_x4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_z4Addr), displayOrder: 8)]
        [BulkCopy]
        public ushort Z4 {
            get => Data.GetUInt16(_z4Addr);
            set => Data.SetUInt16(_z4Addr, value);
        }
    }
}
