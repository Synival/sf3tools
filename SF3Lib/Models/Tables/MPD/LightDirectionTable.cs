using SF3.Models.Structs.MPD;
using SF3.RawData;

namespace SF3.Models.Tables.MPD {
    public class LightDirectionTable : Table<LightDirection> {
        public LightDirectionTable(IByteData data, int address) : base(data, address) {
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new LightDirection(Data, id, "Direction", address));

        public override int? MaxSize => 1;
    }
}
