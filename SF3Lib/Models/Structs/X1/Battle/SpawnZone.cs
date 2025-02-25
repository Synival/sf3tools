using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X1.Battle {
    public class SpawnZone : Struct {
        private readonly int _unknown0x00Addr;
        private readonly int _bottomLeftXAddr;
        private readonly int _bottomLeftZAddr;
        private readonly int _topLeftXAddr;
        private readonly int _topLeftZAddr;
        private readonly int _topRightXAddr;
        private readonly int _topRightZAddr;
        private readonly int _bottomRightXAddr;
        private readonly int _bottomRightZAddr;

        public SpawnZone(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x12) {
            _unknown0x00Addr  = Address + 0x00; // 2 bytes  
            _bottomLeftXAddr  = Address + 0x02; // 2 bytes
            _bottomLeftZAddr  = Address + 0x04; // 2 bytes
            _topLeftXAddr     = Address + 0x06; // 2 bytes
            _topLeftZAddr     = Address + 0x08; // 2 bytes
            _topRightXAddr    = Address + 0x0a; // 2 bytes
            _topRightZAddr    = Address + 0x0c; // 2 bytes
            _bottomRightXAddr = Address + 0x0e; // 2 bytes
            _bottomRightZAddr = Address + 0x10; // 2 bytes
        }

        [TableViewModelColumn(displayOrder: 0, displayName: "+0x00", displayFormat: "X2")]
        [BulkCopy]
        public int Unknown0x00 {
            get => Data.GetWord(_unknown0x00Addr);
            set => Data.SetWord(_unknown0x00Addr, value);
        }

        [TableViewModelColumn(displayOrder: 1)]
        [BulkCopy]
        public int BottomLeftX {
            get => Data.GetWord(_bottomLeftXAddr);
            set => Data.SetWord(_bottomLeftXAddr, value);
        }

        [TableViewModelColumn(displayOrder: 2)]
        [BulkCopy]
        public int BottomLeftZ {
            get => Data.GetWord(_bottomLeftZAddr);
            set => Data.SetWord(_bottomLeftZAddr, value);
        }

        [TableViewModelColumn(displayOrder: 3)]
        [BulkCopy]
        public int TopLeftX {
            get => Data.GetWord(_topLeftXAddr);
            set => Data.SetWord(_topLeftXAddr, value);
        }

        [TableViewModelColumn(displayOrder: 4)]
        [BulkCopy]
        public int TopLeftZ {
            get => Data.GetWord(_topLeftZAddr);
            set => Data.SetWord(_topLeftZAddr, value);
        }

        [TableViewModelColumn(displayOrder: 5)]
        [BulkCopy]
        public int TopRightX {
            get => Data.GetWord(_topRightXAddr);
            set => Data.SetWord(_topRightXAddr, value);
        }

        [TableViewModelColumn(displayOrder: 6)]
        [BulkCopy]
        public int TopRightZ {
            get => Data.GetWord(_topRightZAddr);
            set => Data.SetWord(_topRightZAddr, value);
        }

        [TableViewModelColumn(displayOrder: 7)]
        [BulkCopy]
        public int BottomRightX {
            get => Data.GetWord(_bottomRightXAddr);
            set => Data.SetWord(_bottomRightXAddr, value);
        }

        [TableViewModelColumn(displayOrder: 8)]
        [BulkCopy]
        public int BottomRightZ {
            get => Data.GetWord(_bottomRightZAddr);
            set => Data.SetWord(_bottomRightZAddr, value);
        }
    }
}
