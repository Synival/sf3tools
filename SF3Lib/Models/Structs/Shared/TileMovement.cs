using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.Shared {
    public class TileMovement : Struct {
        private readonly int _noEntryAddr;
        private readonly int _airAddr;
        private readonly int _grasslandAddr;
        private readonly int _dirtAddr;
        private readonly int _darkGrassAddr;
        private readonly int _forestAddr;
        private readonly int _brownMountainAddr;
        private readonly int _desertAddr;
        private readonly int _greyMountainAddr;
        private readonly int _waterAddr;
        private readonly int _cantStayAddr;
        private readonly int _sandAddr;
        private readonly int _enemyOnlyAddr;
        private readonly int _playerOnlyAddr;
        private readonly int _unknown0eAddr;
        private readonly int _unknown0fAddr;

        public TileMovement(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x10) {
            _noEntryAddr       = Address;
            _airAddr           = Address + 0x01;
            _grasslandAddr     = Address + 0x02;
            _dirtAddr          = Address + 0x03;
            _darkGrassAddr     = Address + 0x04;
            _forestAddr        = Address + 0x05;
            _brownMountainAddr = Address + 0x06;
            _desertAddr        = Address + 0x07;
            _greyMountainAddr  = Address + 0x08;
            _waterAddr         = Address + 0x09;
            _cantStayAddr      = Address + 0x0a;
            _sandAddr          = Address + 0x0b;
            _enemyOnlyAddr     = Address + 0x0c;
            _playerOnlyAddr    = Address + 0x0d;
            _unknown0eAddr     = Address + 0x0e;
            _unknown0fAddr     = Address + 0x0f;
        }

        [TableViewModelColumn(addressField: nameof(_noEntryAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public int NoEntry {
            get => Data.GetUInt8(_noEntryAddr);
            set => Data.SetUInt8(_noEntryAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_airAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Air {
            get => Data.GetUInt8(_airAddr);
            set => Data.SetUInt8(_airAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_grasslandAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int Grassland {
            get => Data.GetUInt8(_grasslandAddr);
            set => Data.SetUInt8(_grasslandAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_dirtAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int Dirt {
            get => Data.GetUInt8(_dirtAddr);
            set => Data.SetUInt8(_dirtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_darkGrassAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int DarkGrass {
            get => Data.GetUInt8(_darkGrassAddr);
            set => Data.SetUInt8(_darkGrassAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_forestAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int Forest {
            get => Data.GetUInt8(_forestAddr);
            set => Data.SetUInt8(_forestAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_brownMountainAddr), displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public int BrownMountain {
            get => Data.GetUInt8(_brownMountainAddr);
            set => Data.SetUInt8(_brownMountainAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_desertAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int Desert {
            get => Data.GetUInt8(_desertAddr);
            set => Data.SetUInt8(_desertAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_greyMountainAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int GreyMountain {
            get => Data.GetUInt8(_greyMountainAddr);
            set => Data.SetUInt8(_greyMountainAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_waterAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Water {
            get => Data.GetUInt8(_waterAddr);
            set => Data.SetUInt8(_waterAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cantStayAddr), displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public int CantStay {
            get => Data.GetUInt8(_cantStayAddr);
            set => Data.SetUInt8(_cantStayAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sandAddr), displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public int Sand {
            get => Data.GetUInt8(_sandAddr);
            set => Data.SetUInt8(_sandAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_enemyOnlyAddr), displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public int EnemyOnly {
            get => Data.GetUInt8(_enemyOnlyAddr);
            set => Data.SetUInt8(_enemyOnlyAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_playerOnlyAddr), displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public int PlayerOnly {
            get => Data.GetUInt8(_playerOnlyAddr);
            set => Data.SetUInt8(_playerOnlyAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0eAddr), displayOrder: 14, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownE {
            get => Data.GetUInt8(_unknown0eAddr);
            set => Data.SetUInt8(_unknown0eAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0fAddr), displayOrder: 15, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownF {
            get => Data.GetUInt8(_unknown0fAddr);
            set => Data.SetUInt8(_unknown0fAddr, (byte) value);
        }
    }
}
