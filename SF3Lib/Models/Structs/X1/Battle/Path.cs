using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class Path : Struct {
        private readonly int _pathEnds;
        private readonly int _alwaysFFFF;
        private readonly int _positionCountAddr;
        private readonly int _xPos1Addr;
        private readonly int _zPos1Addr;
        private readonly int _xPos2Addr;
        private readonly int _zPos2Addr;
        private readonly int _xPos3Addr;
        private readonly int _zPos3Addr;
        private readonly int _xPos4Addr;
        private readonly int _zPos4Addr;

        public Path(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x16) {
            _pathEnds        = Address + 0x00; // 2 bytes
            _alwaysFFFF      = Address + 0x02; // 2 bytes
            _positionCountAddr = Address + 0x04; // 2 bytes
            _xPos1Addr       = Address + 0x06; // 2 bytes
            _zPos1Addr       = Address + 0x08; // 2 bytes
            _xPos2Addr       = Address + 0x0A; // 2 bytes
            _zPos2Addr       = Address + 0x0C; // 2 bytes
            _xPos3Addr       = Address + 0x0E; // 2 bytes
            _zPos3Addr       = Address + 0x10; // 2 bytes
            _xPos4Addr       = Address + 0x12; // 2 bytes
            _zPos4Addr       = Address + 0x14; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_pathEnds), displayOrder: 0)]
        [BulkCopy]
        public bool PathEnds {
            get => Data.GetUInt16(_pathEnds) != 0;
            set => Data.SetUInt16(_pathEnds, (ushort) (value ? 1 : 0));
        }

        [TableViewModelColumn(addressField: nameof(_alwaysFFFF), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public ushort AlwaysFFFF {
            get => Data.GetUInt16(_alwaysFFFF);
            set => Data.SetUInt16(_alwaysFFFF, value);
        }

        [TableViewModelColumn(addressField: nameof(_positionCountAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public ushort PositionCount {
            get => Data.GetUInt16(_positionCountAddr);
            set => Data.SetUInt16(_positionCountAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos1Addr), displayOrder: 3, displayName: "xPos1")]
        [BulkCopy]
        public ushort XPos1 {
            get => Data.GetUInt16(_xPos1Addr);
            set => Data.SetUInt16(_xPos1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos1Addr), displayOrder: 4, displayName: "zPos1")]
        [BulkCopy]
        public ushort ZPos1 {
            get => Data.GetUInt16(_zPos1Addr);
            set => Data.SetUInt16(_zPos1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos2Addr), displayOrder: 5, displayName: "xPos2")]
        [BulkCopy]
        public ushort XPos2 {
            get => Data.GetUInt16(_xPos2Addr);
            set => Data.SetUInt16(_xPos2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos2Addr), displayOrder: 6, displayName: "zPos2")]
        [BulkCopy]
        public ushort ZPos2 {
            get => Data.GetUInt16(_zPos2Addr);
            set => Data.SetUInt16(_zPos2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos3Addr), displayOrder: 7, displayName: "xPos3")]
        [BulkCopy]
        public ushort XPos3 {
            get => Data.GetUInt16(_xPos3Addr);
            set => Data.SetUInt16(_xPos3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos3Addr), displayOrder: 8, displayName: "zPos3")]
        [BulkCopy]
        public ushort ZPos3 {
            get => Data.GetUInt16(_zPos3Addr);
            set => Data.SetUInt16(_zPos3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos4Addr), displayOrder: 9, displayName: "xPos4")]
        [BulkCopy]
        public ushort XPos4 {
            get => Data.GetUInt16(_xPos4Addr);
            set => Data.SetUInt16(_xPos4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos4Addr), displayOrder: 10, displayName: "zPos4")]
        [BulkCopy]
        public ushort ZPos4 {
            get => Data.GetUInt16(_zPos4Addr);
            set => Data.SetUInt16(_zPos4Addr, value);
        }
    }
}
