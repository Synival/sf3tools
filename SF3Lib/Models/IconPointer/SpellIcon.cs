using CommonLib.Attributes;
using SF3.RawEditors;
using SF3.Types;

namespace SF3.Models.IconPointer {
    public class SpellIcon : Model {
        //SPELLS
        private readonly int theSpellIcon;

        public SpellIcon(IRawEditor editor, int id, string name, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(editor, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart = realOffsetStart;
            theSpellIcon = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }

        [NameGetter(NamedValueType.Spell)]
        public int SpellID => ID;

        [BulkCopy]
        public int TheSpellIcon {
            get {
                return Has16BitIconAddr
                    ? Editor.GetWord(theSpellIcon)
                    : Editor.GetDouble(theSpellIcon);
            }
            set {
                if (Has16BitIconAddr)
                    Editor.SetWord(theSpellIcon, value);
                else
                    Editor.SetDouble(theSpellIcon, value);
            }
        }

        [BulkCopy]
        public int RealOffset {
            get => TheSpellIcon + RealOffsetStart;
            set => TheSpellIcon = value - RealOffsetStart;
        }
    }
}
