using CommonLib.Attributes;
using SF3.FileEditors;
using SF3.NamedValues;
using SF3.Types;

namespace SF3.Models {
    public class SpellIcon : IModel {
        //SPELLS
        private readonly int theSpellIcon;

        public SpellIcon(IByteEditor fileEditor, int id, string name, int address, string spellName, bool has16BitIconAddr, int realOffsetStart) {
            Editor          = fileEditor;
            ID              = id;
            Name            = name;
            Address         = address;

            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart = realOffsetStart;
            SpellName       = spellName;

            if (Has16BitIconAddr) {
                Size = 2;
                theSpellIcon = Address; // 2 bytes
            }
            else {
                Size = 4;
                theSpellIcon = Address; // 4 bytes  
            }
        }

        public IByteEditor Editor { get; }

        [BulkCopyRowName]
        public string Name { get; }
        public int ID { get; }
        public int Address { get; }
        public int Size { get; }

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
