using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.X023 {
    public class ShopAutoDealsPointer : Struct {
        private readonly int _shopAutoDealsAddr;
        private int? _flagAddr;

        public ShopAutoDealsPointer(IByteData data, int id, string name, int address, int? hasFlagOffset)
        : base(data, id, name, address, 0x04) {
            HasFlagOffset = hasFlagOffset;
            _shopAutoDealsAddr = Address + 0x00; // 4 bytes
            _flagAddr = HasFlagOffset.HasValue ? (int) (ShopAutoDeals - HasFlagOffset.Value) : (int?) null;
        }

        public int? HasFlagOffset { get; }
        public bool HasFlag => HasFlagOffset.HasValue;

        [TableViewModelColumn(displayOrder: 0, addressField: nameof(_shopAutoDealsAddr), isPointer: true, minWidth: 150)]
        [BulkCopy]
        public uint ShopAutoDeals {
            get => (uint) Data.GetDouble(_shopAutoDealsAddr);
            set => Data.SetDouble(_shopAutoDealsAddr, (int) value);
        }

        [TableViewModelColumn(displayOrder: 1, addressField: nameof(_flagAddr), displayFormat: "X2", minWidth: 250, visibilityProperty: nameof(HasFlag), displayName: "Flag (Scn3+)")]
        [NameGetter(NamedValueType.GameFlag)]
        [BulkCopy]
        public int? Flag {
            get => HasFlagOffset.HasValue ? Data.GetDouble((int) ShopAutoDeals - HasFlagOffset.Value) : (int?) null;
            set {
                if (HasFlagOffset.HasValue)
                    Data.SetDouble((int) ShopAutoDeals - HasFlagOffset.Value, value.Value);
            }
        }
    }
}
