using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X002 {
    public class StatBoost : Struct {
        private readonly int _statAddr;

        public StatBoost(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x01) {
            _statAddr = Address; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_statAddr), displayOrder: 0, displayName: "Stat Value")]
        [BulkCopy]
        public byte Stat {
            get => Data.GetUInt8(_statAddr);
            set => Data.SetUInt8(_statAddr, value);
        }
    }
}
