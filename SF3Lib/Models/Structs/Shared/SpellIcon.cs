using CommonLib.Attributes;
using SF3.ByteData;
using SF3.Types;

namespace SF3.Models.Structs.Shared {
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
        [TableViewModelColumn(addressField: null, displayName: "Icon Offset (Derived)", displayOrder: 1, displayFormat: "X4")]
        public int IconOffset {
            get => IconOffsetAfterItems + RealOffsetStart;
            set => IconOffsetAfterItems = value - RealOffsetStart;
        }

        [BulkCopy]
        [TableViewModelColumn(addressField: nameof(_theSpellIconAddr), displayName: "Icon Offset (After Items)", displayOrder: 2, displayFormat: "X4")]
        public int IconOffsetAfterItems {
            get {
                return Has16BitIconAddr
                    ? Data.GetUInt16(_theSpellIconAddr)
                    : Data.GetInt32(_theSpellIconAddr);
            }
            set {
                if (Has16BitIconAddr)
                    Data.SetUInt16(_theSpellIconAddr, (ushort) value);
                else
                    Data.SetInt32(_theSpellIconAddr, value);
            }
        }
    }
}
