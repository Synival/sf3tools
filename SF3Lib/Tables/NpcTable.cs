using SF3.FileEditors;
using SF3.Models;

namespace SF3.Tables {
    public class NpcTable : Table<Npc> {
        public NpcTable(IByteEditor fileEditor, string resourceFile, int address) : base(fileEditor, resourceFile, address) {
        }

        public override bool Load()
            => LoadFromResourceFile(
                (id, name, address) => new Npc(FileEditor, id, name, address),
                (rows, prev, cur) => prev == null || prev.SpriteID != 0xffff);

        public override int? MaxSize => 100;
    }
}
