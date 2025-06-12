using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X023 {
    public class ShopAutoDeal : Struct {
        private readonly int _itemAddr;
        private readonly int? _flagAddr;

        public ShopAutoDeal(IByteData data, int id, string name, int address, bool hasFlag)
        : base(data, id, name, address, hasFlag ? 0x08 : 0x04) {
            HasFlag = hasFlag;
            _itemAddr = Address + 0x00; // 4 bytes
            _flagAddr = HasFlag ? (Address + 0x04) : (int?) null; // 4 bytes
        }

        public bool HasFlag { get; }

        [TableViewModelColumn(displayOrder: 0, addressField: nameof(_itemAddr), minWidth: 150)]
        [NameGetter(NamedValueType.Item)]
        [BulkCopy]
        public int Item {
            get => Data.GetDouble(_itemAddr);
            set => Data.SetDouble(_itemAddr, value);
        }

        [TableViewModelColumn(displayOrder: 1, addressField: nameof(_flagAddr), minWidth: 250, visibilityProperty: nameof(HasFlag), displayName: "Flag (Scn1,2)")]
        [NameGetter(NamedValueType.GameFlag)]
        [BulkCopy]
        public int? Flag {
            get => HasFlag ? (int?) Data.GetDouble(_flagAddr.Value) : null;
            set {
                if (HasFlag && value.HasValue)
                    Data.SetDouble(_flagAddr.Value, value.Value);
            }
        }
    }
}
