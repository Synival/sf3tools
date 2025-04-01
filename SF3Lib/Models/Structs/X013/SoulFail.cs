using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class SoulFail : Struct {
        private readonly int _expLostAddr;

        public SoulFail(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _expLostAddr = Address; // 1 bytes
        }

        [TableViewModelColumn(displayOrder: 0)]
        [BulkCopy]
        public int ExpLost {
            get => (sbyte) Data.GetByte(_expLostAddr);
            set => Data.SetByte(_expLostAddr, (byte) value);
        }
    }
}
