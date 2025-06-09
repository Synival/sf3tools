using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class ScriptedMovement : Struct {
        private readonly int _positionCountAddr;
        private readonly int _xPos1Addr;
        private readonly int _zPos1Addr;
        private readonly int _xPos2Addr;
        private readonly int _zPos2Addr;
        private readonly int _xPos3Addr;
        private readonly int _zPos3Addr;
        private readonly int _xPos4Addr;
        private readonly int _zPos4Addr;
        private readonly int _unknown0x00Addr;
        private readonly int _unknown0x02Addr;

        public ScriptedMovement(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x16) {
            _unknown0x00Addr = Address + 0x00; // 2 bytes
            _unknown0x02Addr = Address + 0x02; // 2 bytes
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

        [TableViewModelColumn(addressField: nameof(_unknown0x00Addr), displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x00 {
            get => Data.GetWord(_unknown0x00Addr);
            set => Data.SetWord(_unknown0x00Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0x02Addr), displayOrder: 1, displayName: "+0x02", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x02 {
            get => Data.GetWord(_unknown0x02Addr);
            set => Data.SetWord(_unknown0x02Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_positionCountAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int PositionCount {
            get => Data.GetWord(_positionCountAddr);
            set => Data.SetWord(_positionCountAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos1Addr), displayOrder: 3, displayName: "xPos1")]
        [BulkCopy]
        public int XPos1 {
            get => Data.GetWord(_xPos1Addr);
            set => Data.SetWord(_xPos1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos1Addr), displayOrder: 4, displayName: "zPos1")]
        [BulkCopy]
        public int ZPos1 {
            get => Data.GetWord(_zPos1Addr);
            set => Data.SetWord(_zPos1Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos2Addr), displayOrder: 5, displayName: "xPos2")]
        [BulkCopy]
        public int XPos2 {
            get => Data.GetWord(_xPos2Addr);
            set => Data.SetWord(_xPos2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos2Addr), displayOrder: 6, displayName: "zPos2")]
        [BulkCopy]
        public int ZPos2 {
            get => Data.GetWord(_zPos2Addr);
            set => Data.SetWord(_zPos2Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos3Addr), displayOrder: 7, displayName: "xPos3")]
        [BulkCopy]
        public int XPos3 {
            get => Data.GetWord(_xPos3Addr);
            set => Data.SetWord(_xPos3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos3Addr), displayOrder: 8, displayName: "zPos3")]
        [BulkCopy]
        public int ZPos3 {
            get => Data.GetWord(_zPos3Addr);
            set => Data.SetWord(_zPos3Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_xPos4Addr), displayOrder: 9, displayName: "xPos4")]
        [BulkCopy]
        public int XPos4 {
            get => Data.GetWord(_xPos4Addr);
            set => Data.SetWord(_xPos4Addr, value);
        }

        [TableViewModelColumn(addressField: nameof(_zPos4Addr), displayOrder: 10, displayName: "zPos4")]
        [BulkCopy]
        public int ZPos4 {
            get => Data.GetWord(_zPos4Addr);
            set => Data.SetWord(_zPos4Addr, value);
        }
    }
}
