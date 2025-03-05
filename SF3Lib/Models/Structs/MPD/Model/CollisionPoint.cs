using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionPoint : Struct {
        private readonly int _xAddr;
        private readonly int _yAddr;

        public CollisionPoint(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x04) {
            _xAddr = Address + 0x00; // 2 bytes
            _yAddr = Address + 0x02; // 2 bytes
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 0, minWidth: 40)]
        public short X {
            get => (short) Data.GetWord(_xAddr);
            set => Data.SetWord(_xAddr, value);
        }

        [BulkCopy]
        [TableViewModelColumn(displayOrder: 1, minWidth: 40)]
        public short Y {
            get => (short) Data.GetWord(_yAddr);
            set => Data.SetWord(_yAddr, value);
        }
    }
}
