using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class ZoneTable : FixedSizeTable<Zone> {
        protected ZoneTable(IByteData data, string name, int address) : base(data, name, address, 16) {
        }

        public static ZoneTable Create(IByteData data, string name, int address)
            => Create(() => new ZoneTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new Zone(Data, id, $"{nameof(Zone)}{id:D2}", address));
    }
}
