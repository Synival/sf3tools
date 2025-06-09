using CommonLib.Attributes;
using SF3.ByteData;

namespace SF3.Models.Structs.MPD.Model {
    public class CollisionLineIndex : Struct {
        private readonly int _lineIndexAddr;

        public CollisionLineIndex(IByteData data, int id, string name, int address) : base(data, id, name, address, 0x02) {
            _lineIndexAddr = Address + 0x00; // 2 bytes
        }

        [TableViewModelColumn(addressField: nameof(_lineIndexAddr), displayOrder: 0, displayFormat: "X2")]
        [BulkCopy]
        public ushort LineIndex {
            get => (ushort) Data.GetWord(_lineIndexAddr);
            set => Data.SetWord(_lineIndexAddr, value);
        }
    }
}
