using System;
using SF3.ByteData;
using SF3.Models.Structs.X013;

namespace SF3.Models.Tables.X013 {
    public class SupportStatsTable : ResourceTable<SupportStats> {
        protected SupportStatsTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 256) {
        }

        public static SupportStatsTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new SupportStatsTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SupportStats(Data, id, name, address));
    }
}
