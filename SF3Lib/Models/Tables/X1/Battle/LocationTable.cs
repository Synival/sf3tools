using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class LocationTable : FixedSizeTable<Location> {
        protected LocationTable(IByteData data, string name, int address) : base(data, name, address, 32) {
        }

        public static LocationTable Create(IByteData data, string name, int address)
            => Create(() => new LocationTable(data, name, address));

        public override bool Load()
            => Load((id, address) => new Location(Data, id, $"{nameof(Location)}{id:D2}", address));
    }
}
