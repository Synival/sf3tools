using SF3.Models.Structs.X1.Town;
using SF3.Models.Tables;
using SF3.RawData;

namespace SF3.Models.Tables.X1.Town {
    public class NpcTable : Table<Npc> {
        public NpcTable(IRawData editor, string resourceFile, int address) : base(editor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Npc(Editor, id, name, address),
                (rows, model) => model.SpriteID != 0xFFFF);

        public override int? MaxSize => 100;
    }
}
