using CommonLib.Attributes;
using SF3.FileEditors;

namespace SF3.Models {
    public class SpellIcon : Model {
        //SPELLS
        private readonly int theSpellIcon;

        public SpellIcon(IByteEditor editor, int id, string name, int address, string spellName, bool has16BitIconAddr, int realOffsetStart)
        : base(editor, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart = realOffsetStart;
            SpellName       = spellName;

            theSpellIcon = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }
        public string SpellName { get; }
        public int RealOffsetStart { get; }

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
