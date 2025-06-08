using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.IconPointer {
    public class ItemIcon : Struct {
        private readonly int _theItemIconAddr;

        public ItemIcon(IByteData data, int id, string name, int address, bool has16BitIconAddr)
        : base(data, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            _theItemIconAddr = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_theItemIconAddr), displayName: "Icon Offset", displayOrder: 0, displayFormat: "X4")]
        public int TheItemIcon {
            get {
                return Has16BitIconAddr
                    ? Data.GetWord(_theItemIconAddr)
                    : Data.GetDouble(_theItemIconAddr);
            }
            set {
                if (Has16BitIconAddr)
                    Data.SetWord(_theItemIconAddr, value);
                else
                    Data.SetDouble(_theItemIconAddr, value);
            }
        }
    }
}
