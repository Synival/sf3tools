using System;
using SF3.ByteData;
using SF3.Models.Structs.X033_X031;

namespace SF3.Models.Tables.X033_X031 {
    public class StatsTable : Table<Stats> {
        protected StatsTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static StatsTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new StatsTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new Stats(Data, id, name, address));

        public override int? MaxSize => 300;
    }
}
