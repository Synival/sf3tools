using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.IconPointer {
    public class SpellIcon : Struct {
        //SPELLS
        private readonly int _theSpellIconAddr;

        public SpellIcon(IByteData data, int id, string name, int address, bool has16BitIconAddr, int realOffsetStart)
        : base(data, id, name, address, has16BitIconAddr ? 0x02 : 0x04) {
            Has16BitIconAddr = has16BitIconAddr;
            RealOffsetStart = realOffsetStart;
            _theSpellIconAddr = Address; // 2 or 4 bytes
        }

        public bool Has16BitIconAddr { get; }
        public int RealOffsetStart { get; }

        [NameGetter(NamedValueType.Spell)]
        [TableViewModelColumn(addressField: null, displayName: "Spell Name", displayOrder: 0, minWidth: 120)]
        public int SpellID => ID;

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_theSpellIconAddr), displayName: "Icon Offset", displayOrder: 1, displayFormat: "X4")]
        public int TheSpellIcon {
            get {
                return Has16BitIconAddr
                    ? Data.GetWord(_theSpellIconAddr)
                    : Data.GetDouble(_theSpellIconAddr);
            }
            set {
                if (Has16BitIconAddr)
                    Data.SetWord(_theSpellIconAddr, value);
                else
                    Data.SetDouble(_theSpellIconAddr, value);
            }
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: null, displayName: "Offset in File for Viewing", displayOrder: 2, displayFormat: "X4")]
        public int RealOffset {
            get => TheSpellIcon + RealOffsetStart;
            set => TheSpellIcon = value - RealOffsetStart;
        }
    }
}
