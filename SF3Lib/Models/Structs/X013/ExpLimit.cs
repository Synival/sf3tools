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
        public int ExpCheck {
            get => Data.GetByte(_expCheckAddr);
            set => Data.SetByte(_expCheckAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_expReplacementAddr), displayOrder: 1, displayName: "Replaced Value")]
        [BulkCopy]
        public int ExpReplacement {
            get => Data.GetByte(_expReplacementAddr);
            set => Data.SetByte(_expReplacementAddr, (byte) value);
        }
    }
}
