using SF3.Models.Structs.X1.Battle;
using SF3.Models.Tables;
using SF3.ByteData;
using System;

namespace SF3.Models.Tables.X1.Battle {
    public class SpawnZoneTable : Table<SpawnZone> {
        protected SpawnZoneTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static SpawnZoneTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new SpawnZoneTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile((id, name, address) => new SpawnZone(Data, id, name, address));

        public override int? MaxSize => 30;
    }
}
