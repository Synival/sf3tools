using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLineIndex : Struct {
        public CollisionLineIndex(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
        }

        [TableViewModelColumn(addressField: nameof(Address), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public ushort LineIndex {
            get => (ushort) Data.GetWord(Address);
            set => Data.SetWord(Address, value);
        }
    }
}
