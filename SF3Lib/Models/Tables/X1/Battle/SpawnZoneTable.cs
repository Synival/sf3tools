using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Battle;

namespace SF3.Models.Tables.X1.Battle {
    public class SpawnZoneTable : ResourceTable<SpawnZone> {
        protected SpawnZoneTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address, 30) {
        }

        public static SpawnZoneTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SpawnZoneTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load((id, name, address) => new SpawnZone(Data, id, name, address));
    }
}
