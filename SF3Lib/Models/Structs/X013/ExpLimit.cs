using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.X013 {
    public class ExpLimit : Struct {
        private readonly int _expCheckAddr;
        private readonly int _expReplacementAddr;

        public ExpLimit(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x07) {
            _expCheckAddr       = Address;     // 1 byte
            _expReplacementAddr = Address + 6; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_expCheckAddr), displayOrder: 0, displayName: "Checked Value")]
        [BulkCopy]
        public byte ExpCheck {
            get => Data.GetUInt8(_expCheckAddr);
            set => Data.SetUInt8(_expCheckAddr, value);
        }

        [TableViewModelColumn(addressField: nameof(_expReplacementAddr), displayOrder: 1, displayName: "Replaced Value")]
        [BulkCopy]
        public byte ExpReplacement {
            get => Data.GetUInt8(_expReplacementAddr);
            set => Data.SetUInt8(_expReplacementAddr, value);
        }
    }
}
