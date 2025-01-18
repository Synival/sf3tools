using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD {
    public class Boundary : Struct {
        private readonly int _x1Addr;
        private readonly int _y1Addr;
        private readonly int _x2Addr;
        private readonly int _y2Addr;

        public Boundary(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x8) {
            _x1Addr = address + 0x00; // 2 bytes
            _y1Addr = address + 0x02; // 2 bytes
            _x2Addr = address + 0x04; // 2 bytes
            _y2Addr = address + 0x06; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, minWidth: 60)]
        public short X1 {
            get => (short) Data.GetWord(_x1Addr);
            set => Data.SetWord(_x1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, minWidth: 60)]
        public short Y1 {
            get => (short) Data.GetWord(_y1Addr);
            set => Data.SetWord(_y1Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 2, minWidth: 60)]
        public short X2 {
            get => (short) Data.GetWord(_x2Addr);
            set => Data.SetWord(_x2Addr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 3, minWidth: 60)]
        public short Y2 {
            get => (short) Data.GetWord(_y2Addr);
            set => Data.SetWord(_y2Addr, value);
        }
    }
}
