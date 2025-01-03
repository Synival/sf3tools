using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : Table<Npc> {
        protected NpcTable(IByteData data, string resourceFile, int address) : base(data, resourceFile, address) {
        }

        public static NpcTable Create(IByteData data, string resourceFile, int address) {
            var newTable = new NpcTable(data, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Npc(Data, id, name, address),
                (rows, model) => model.SpriteID != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
