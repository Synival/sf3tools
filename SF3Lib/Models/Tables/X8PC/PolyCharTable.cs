using SF3.ByteData;
using SF3.Models.Structs.X8PC;

namespace SF3.Models.Tables.X8PC {
    public class PolyCharTable : AddressedTable<PolyChar> {
        protected PolyCharTable(IByteData data, string name, int[] addresses) : base(data, name, addresses) {
        }

        public static PolyCharTable Create(IByteData data, string name, int[] addresses)
            => Create(() => new PolyCharTable(data, name, addresses));

        public override bool Load() {
            return Load((id, address) => new PolyChar(Data, id, $"PolyChar{id + 1:D2} (@0x{address:X5})", address));
        }
    }
}
