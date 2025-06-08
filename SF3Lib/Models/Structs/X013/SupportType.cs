using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X013 {
    public class SupportType : Struct {
        private readonly int _supportAAddr;
        private readonly int _supportBAddr;

        public SupportType(IByteData data, int id, string name, int address)
        : base(data, id, name, address, 0x02) {
            _supportAAddr = Address;     // 1 byte
            _supportBAddr = Address + 1; // 1 byte
        }

        [TableViewModelColumn(addressField: nameof(_supportAAddr), displayOrder: 0, minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportA {
            get => Data.GetByte(_supportAAddr);
            set => Data.SetByte(_supportAAddr, (byte) value);
        }

        [TableViewModelColumn(addressField: nameof(_supportBAddr), displayOrder: 1, minWidth: 150, displayFormat: "X2")]
        [BulkCopy]
        [NameGetter(NamedValueType.FriendshipBonusType)]
        public int SupportB {
            get => Data.GetByte(_supportBAddr);
            set => Data.SetByte(_supportBAddr, (byte) value);
        }
    }
}
