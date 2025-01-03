using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.IconPointer {
    public class SpellIcon : Struct {
        //SPELLS
        private readonly int theSpellIcon;

        public SpellIcon(IByteData data, int id, string name, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(data, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart = realOffsetStart;
            theSpellIcon = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }

        [NameGetter(NamedValueType.Spell)]
        [TableViewModelColumn(displayName: "Spell Name", displayOrder: 0, minWidth: 120)]
        public int SpellID => ID;

        [BulkCopy]
        [TableViewModelColumn(displayName: "Icon Offset", displayOrder: 1, displayFormat: "X4")]
        public int TheSpellIcon {
            get {
                return Has16BitIconAddr
                    ? Data.GetWord(theSpellIcon)
                    : Data.GetDouble(theSpellIcon);
            }
            set {
                if (Has16BitIconAddr)
                    Data.SetWord(theSpellIcon, value);
                else
                    Data.SetDouble(theSpellIcon, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(displayName: "Offset in File for Viewing", displayOrder: 2, displayFormat: "X4")]
        public int RealOffset {
            get => TheSpellIcon + RealOffsetStart;
            set => TheSpellIcon = value - RealOffsetStart;
        }
    }
}
