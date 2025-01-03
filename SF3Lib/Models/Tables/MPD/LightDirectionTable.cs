using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class LightDirectionTable : Table<LightDirection> {
        protected LightDirectionTable(IByteData data, int address) : base(data, address) {
        }

        public static LightDirectionTable Create(IByteData data, int address) {
            var newTable = new LightDirectionTable(data, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadUntilMax((id, address) => new LightDirection(Data, id, "Direction", address));

        public override int? MaxSize => 1;
    }
}
