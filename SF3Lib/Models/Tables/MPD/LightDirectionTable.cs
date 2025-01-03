using SF3.Models.Structs.MPD;
using SF3.ByteData;

namespace SF3.Models.Tables.MPD {
    public class LightDirectionTable : Table<LightDirection> {
        protected LightDirectionTable(IByteData data, int address) : base(data, address) {
        }

        public static LightDirectionTable Create(IByteData data, int address) {
            var newTable = new LightDirectionTable(data, address);
            newTable.Load();
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new LightDirection(Data, id, "Direction", address));

        public override int? MaxSize => 1;
    }
}
