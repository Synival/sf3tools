using System;
using SF3.ByteData;
using SF3.Models.Structs.MPD;

namespace SF3.Models.Tables.MPD {
    public class LightDirectionTable : FixedSizeTable<LightDirection> {
        protected LightDirectionTable(IByteData data, string name, int address) : base(data, name, address, 1) {
        }

        public static LightDirectionTable Create(IByteData data, string name, int address) {
            var newTable = new LightDirectionTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new LightDirection(Data, id, "LightDirection", address));
    }
}
