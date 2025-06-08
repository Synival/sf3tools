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
            get => Data.GetByte(_noEntryAddr);
            set => Data.SetByte(_noEntryAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_airAddr), displayOrder: 1, displayFormat: "X2")]
        [BulkCopy]
        public int Air {
            get => Data.GetByte(_airAddr);
            set => Data.SetByte(_airAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_grasslandAddr), displayOrder: 2, displayFormat: "X2")]
        [BulkCopy]
        public int Grassland {
            get => Data.GetByte(_grasslandAddr);
            set => Data.SetByte(_grasslandAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_dirtAddr), displayOrder: 3, displayFormat: "X2")]
        [BulkCopy]
        public int Dirt {
            get => Data.GetByte(_dirtAddr);
            set => Data.SetByte(_dirtAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_darkGrassAddr), displayOrder: 4, displayFormat: "X2")]
        [BulkCopy]
        public int DarkGrass {
            get => Data.GetByte(_darkGrassAddr);
            set => Data.SetByte(_darkGrassAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_forestAddr), displayOrder: 5, displayFormat: "X2")]
        [BulkCopy]
        public int Forest {
            get => Data.GetByte(_forestAddr);
            set => Data.SetByte(_forestAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_brownMountainAddr), displayOrder: 6, displayFormat: "X2")]
        [BulkCopy]
        public int BrownMountain {
            get => Data.GetByte(_brownMountainAddr);
            set => Data.SetByte(_brownMountainAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_desertAddr), displayOrder: 7, displayFormat: "X2")]
        [BulkCopy]
        public int Desert {
            get => Data.GetByte(_desertAddr);
            set => Data.SetByte(_desertAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_greyMountainAddr), displayOrder: 8, displayFormat: "X2")]
        [BulkCopy]
        public int GreyMountain {
            get => Data.GetByte(_greyMountainAddr);
            set => Data.SetByte(_greyMountainAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_waterAddr), displayOrder: 9, displayFormat: "X2")]
        [BulkCopy]
        public int Water {
            get => Data.GetByte(_waterAddr);
            set => Data.SetByte(_waterAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_cantStayAddr), displayOrder: 10, displayFormat: "X2")]
        [BulkCopy]
        public int CantStay {
            get => Data.GetByte(_cantStayAddr);
            set => Data.SetByte(_cantStayAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_sandAddr), displayOrder: 11, displayFormat: "X2")]
        [BulkCopy]
        public int Sand {
            get => Data.GetByte(_sandAddr);
            set => Data.SetByte(_sandAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_enemyOnlyAddr), displayOrder: 12, displayFormat: "X2")]
        [BulkCopy]
        public int EnemyOnly {
            get => Data.GetByte(_enemyOnlyAddr);
            set => Data.SetByte(_enemyOnlyAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_playerOnlyAddr), displayOrder: 13, displayFormat: "X2")]
        [BulkCopy]
        public int PlayerOnly {
            get => Data.GetByte(_playerOnlyAddr);
            set => Data.SetByte(_playerOnlyAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0eAddr), displayOrder: 14, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownE {
            get => Data.GetByte(_unknown0eAddr);
            set => Data.SetByte(_unknown0eAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_unknown0fAddr), displayOrder: 15, displayFormat: "X2")]
        [BulkCopy]
        public int UnknownF {
            get => Data.GetByte(_unknown0fAddr);
            set => Data.SetByte(_unknown0fAddr, (byte) value);
        }
    }
}
