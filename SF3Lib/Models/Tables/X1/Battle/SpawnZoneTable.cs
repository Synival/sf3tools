using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class SpawnZoneTable : FixedSizeTable<SpawnZone> {
        protected SpawnZoneTable(IByteData data, string name, int address) : base(data, name, address, 16) {
        }

        public static SpawnZoneTable Create(IByteData data, string name, int address) {
            var newTable = new SpawnZoneTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, address) => new SpawnZone(Data, id, "SpawnZone" + id.ToString("D2"), address));
    }
}
