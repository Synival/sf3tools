using System;
using SF3.ByteData;
using SF3.Models.Structs.X033_X031;

namespace SF3.Models.Tables.X033_X031 {
    public class StatsTable : ResourceTable<Stats> {
        protected StatsTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 300) {
        }

        public static StatsTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new StatsTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new Stats(Data, id, name, address));
    }
}
