using SF3.RawEditors;
using SF3.Models.X1.Town;

namespace SF3.Tables.X1.Town {
    public class NpcTable : Table<Npc> {
        public NpcTable(IRawEditor editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Npc(Editor, id, name, address),
                (rows, prev, cur) => prev == null || prev.SpriteID != 0xffff);

        public override int? MaxSize => 100;
    }
}
