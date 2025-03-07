using System;
using SF3.ByteData;
using SF3.Models.Structs.X1.Town;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : TerminatedTable<Npc> {
        protected NpcTable(IByteData data, string name, int address)
        : base(data, name, address, 2, 100) {
        }

        public static NpcTable Create(IByteData data, string name, int address) {
            var newTable = new NpcTable(data, name, address);
            if (!newTable.Load())
                throw new InvalidOperationException("Couldn't initialize table");
            return newTable;
        }

        public override bool Load() {
            return Load(
                (id, address) => new Npc(Data, id, "Npc" + id.ToString("D2"), address),
                (rows, model) => model.SpriteID != 0xFFFF,
                false);
        }
    }
}
