using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : ResourceTable<Npc> {
        protected NpcTable(IByteData data, string name, string resourceFile, int address) : base(data, name, resourceFile, address, 100) {
        }

        public static NpcTable Create(IByteData data, string name, string resourceFile, int address) {
            var newTable = new NpcTable(data, name, resourceFile, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load()
            => Load(
                (id, name, address) => new Npc(Data, id, name, address),
                (rows, model) => model.SpriteID != 0xFFFF);
    }
}
