using CommonLib.Attributes;
using SF3.RawData;

namespace SF3.Models.Structs.IconPointer {
    public class ItemIcon : Struct {
        private readonly int theItemIcon;

        public ItemIcon(IByteData data, int id, string name, int address, bool has16BitIconAddr)
        : base(data, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            theItemIcon = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Icon Offset", displayOrder: 0, displayFormat: "X4")]
        public int TheItemIcon {
            get {
                return Has16BitIconAddr
                    ? Data.GetWord(theItemIcon)
                    : Data.GetDouble(theItemIcon);
            }
            set {
                if (Has16BitIconAddr)
                    Data.SetWord(theItemIcon, value);
                else
                    Data.SetDouble(theItemIcon, value);
            }
        }
    }
}
